using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KNEKT.Classes;
using S7Link;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
using System.Reflection;
using System.Windows.Data;

namespace KNEKT
{
    public class IngredientsViewModel : INotifyPropertyChanged
    {
        ObservableCollection<Recipe> ocAvailableRecipes = new ObservableCollection<Recipe>();
        ObservableCollection<Ingredient> ocAllIngredients = new ObservableCollection<Ingredient>();
        ObservableCollection<Bin> ocUsedReceiverBins = new ObservableCollection<Bin>();
        ObservableCollection<Bin> ocAvailableReceiverBins = new ObservableCollection<Bin>();
        ObservableCollection<GcProReceiverData> ocGcProReceiverData = new ObservableCollection<GcProReceiverData>();

        public DelegateCommand cmdSaveRecipe;
        public DelegateCommand cmdNewRecipe;
        public DelegateCommand cmdCancel;
        public DelegateCommand cmdUseReceiverBin;
        public DelegateCommand cmdRemoveReceiverBin;
        public DelegateCommand cmdDownload;

        string sqlConnection = MainWindow.SqlConnectionString;//"Data Source=JHBY03\\SQLEXPRESS;Initial Catalog=KNEKT_AFGRI;User Id=sa;Password=SQLpassword1234;Connection Timeout=10;";

        Page JobPage;

        //----------------------------------------------------------------------//
        //                               Constructor                            //
        //----------------------------------------------------------------------//

        public IngredientsViewModel(string ProcessLine, Page jobPage)
        {
            ProcessLineName = ProcessLine;
            JobPage = jobPage;
            IngredientWeightChange += new EventHandler(Recipe_IngredientWeightChange);
            IngredientPercentageChange += new EventHandler(IngredientsViewModel_IngredientPercentageChange);
            GetSavedIngredients();
            GetSavedRecipes();

            cmdSaveRecipe = new DelegateCommand(cmdSaveRecipeImplementation);
            cmdNewRecipe = new DelegateCommand(cmdNewRecipeImplementation);
            cmdCancel = new DelegateCommand(cmdCancelImplementation);
            cmdUseReceiverBin = new DelegateCommand(cmdUseReceiverBinImplementation);
            cmdRemoveReceiverBin = new DelegateCommand(cmdRemoveReceiverBinImplementation);
            cmdDownload = new DelegateCommand(cmdDownloadImplementation);
        }



        //----------------------------------------------------------------------//
        //                                  Methods                             //
        //----------------------------------------------------------------------//

        private void GetSavedIngredients()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(sqlConnection))
                {
                    sqlConn.Open();
                    SqlCommand cmd = sqlConn.CreateCommand();
                    cmd.CommandText = @"SELECT BinID IngredientID, ProductName IngredientName, BinNumber, BinScaleNumber ScaleNumber, MinValue, MaxValue, ProductType 
                                        FROM(
                                             SELECT b.BinID, b.BinNumber, bp.BinProductID, p.ProductName +' (' + p.ProductAbbreviation+')' ProductName, b.BinScaleNumber, b.MinValue, b.MaxValue, p.ProductType,
		                                        RANK() OVER (PARTITION BY bp.BinID ORDER BY bp.ts_datetime DESC) groupNumber
	                                         FROM BinProductChangeLog bp
	                                            INNER JOIN Bins b on b.BinID = bp.binID AND b.binNumber in(402, 403,404,405,406,407, 5031, 5033, 5034, 5035, 5036) --Only Mixing line Bins
	                                            INNER JOIN Products p on bp.BinProductID = p.ProductID
                                            )a
                                        WHERE groupNumber = 1 --Only get latest record per bin";
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ocAllIngredients.Add(new Ingredient() { IngredientID = reader.GetInt32(0), IngredientName = reader.GetString(1), IngredientBinNumber = reader.GetInt32(2), IngredientScaleNumber = reader.GetInt32(3), IngredientPercentageValue = 0, IngredientKilogramValue = 0, MinimumValue = Convert.ToDouble(reader.GetDecimal(4)), MaximumValue = Convert.ToDouble(reader.GetDecimal(5)), IngredientType = Convert.ToInt32(reader.GetInt32(6)) });
                    }
                    sqlConn.Close();
                }
            }
            catch (Exception)
            {
                //ErrorMessgae = ex.Message;
            }
        }

        private void GetSavedRecipes()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(sqlConnection))
                {
                    sqlConn.Open();

                    SqlCommand cmd = sqlConn.CreateCommand();

                    //cmd.CommandText = "select r.*, ri.TargetPercent, ri.IngredientWeight, ri.SortIndex, i.* from recipes r inner join RecipeIngredient ri on r.recipeId = ri.recipeId and r.ProcessLine = '" + ProcessLineName + "' inner join ingredients i on ri.ingredientid = i.ingredientid order by r.RecipeID, ri.SortIndex";
                    cmd.CommandText = @" SELECT r.*, ri.TargetPercent, ri.IngredientWeight, ri.SortIndex, i.* 
                                         FROM recipes r inner join RecipeIngredient ri on r.recipeId = ri.recipeId and r.ProcessLine = '" + ProcessLineName + "'"
                                         + @"INNER JOIN (SELECT BinID IngredientID, ProductName IngredientName, BinNumber, BinScaleNumber ScaleNumber, MinValue, MaxValue, ProductType
                                                          FROM( SELECT b.BinID, b.BinNumber, bp.BinProductID, p.ProductName +'(' + p.ProductAbbreviation+')' ProductName, b.BinScaleNumber, b.MinValue, b.MaxValue, p.ProductType, 
                                                                    RANK() OVER (PARTITION BY bp.BinID ORDER BY bp.ts_datetime DESC) groupNumber 
                                                                FROM BinProductChangeLog bp 
                                                                INNER JOIN Bins b on b.BinID = bp.binID AND b.binNumber in(402, 403,404,405,406,407, 5031, 5033, 5034, 5035, 5036) /*Only Mixing line Bins*/ 
                                                                INNER JOIN Products p on bp.BinProductID = p.ProductID )a WHERE groupNumber = 1) i /*Only get latest record per bin */ 
                                            ON ri.ingredientid = i.ingredientid order by r.RecipeID, ri.SortIndex";
                    SqlDataReader resultReader = cmd.ExecuteReader();

                    int iRecipeID = 0;
                    Recipe r = new Recipe();

                    while (resultReader.Read())
                    {
                        if (iRecipeID != resultReader.GetInt32(0))
                        {
                            iRecipeID = resultReader.GetInt32(0);
                            r = new Recipe() { RecipeId = Convert.ToInt32(resultReader["RecipeID"]), RecipeName = resultReader["RecipeName"].ToString(), RecipeBatchWeight = Convert.ToDouble(resultReader["BatchWeight"]), ProcessLineName = resultReader["ProcessLine"].ToString() };
                            r.ocIngredients.Add(new Ingredient()
                            {
                                IngredientPercentageValue = Convert.ToDouble(resultReader["TargetPercent"]),
                                IngredientKilogramValue = Convert.ToDouble(resultReader["IngredientWeight"]),
                                IngredientID = Convert.ToInt32(resultReader["IngredientID"]),
                                IngredientName = resultReader["IngredientName"].ToString(),
                                IngredientBinNumber = Convert.ToInt32(resultReader["BinNumber"]),
                                IngredientScaleNumber = Convert.ToInt32(resultReader["ScaleNumber"]),
                                MinimumValue = Convert.ToDouble(resultReader["MinValue"]),
                                MaximumValue = Convert.ToDouble(resultReader["MaxValue"]),
                                IngredientType  = Convert.ToInt32(resultReader["ProductType"])
                            });
                            ocAvailableRecipes.Add(r);
                        }
                        else
                        {
                            r.ocIngredients.Add(new Ingredient()
                            {
                                IngredientPercentageValue = Convert.ToDouble(resultReader["TargetPercent"]),
                                IngredientKilogramValue = Convert.ToDouble(resultReader["IngredientWeight"]),
                                IngredientID = Convert.ToInt32(resultReader["IngredientID"]),
                                IngredientName = resultReader["IngredientName"].ToString(),
                                IngredientBinNumber = Convert.ToInt32(resultReader["BinNumber"]),
                                IngredientScaleNumber = Convert.ToInt32(resultReader["ScaleNumber"]),
                                MinimumValue = Convert.ToDouble(resultReader["MinValue"]),
                                MaximumValue = Convert.ToDouble(resultReader["MaxValue"]),
                                IngredientType = Convert.ToInt32(resultReader["ProductType"])
                            });
                        }
                    }

                    resultReader.Close();
                    sqlConn.Close();
                }
            }
            catch (Exception)
            {
                //ErrorMessgae = ex.Message;
            }
        }

        public void ReadUsedRecievers()
        {
            foreach (GcProReceiverData s in GcProReceiverDataList)
            {
                try
                {
                    string offset = s.ReceiverDBOffset;
                    Tag t1 = new Tag(offset, S7Link.Tag.ATOMIC.WORD, 1);

                    if (PLCR.IsConnected)
                    {
                        PLCR.ReadTag(t1);
                        int value = Convert.ToInt32(t1.Value);

                        if (value != 0)
                        {
                            Bin CurrentBin = new Bin();
                            foreach (Bin b in AvailableReceiverBins)
                            {
                                CurrentBin = b;

                                if (b.BinId == value)
                                {
                                    UsedReceiverBins.Add(b);
                                    break;
                                }
                                else
                                    CurrentBin = new Bin();
                            }

                            AvailableReceiverBins.Remove(CurrentBin);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public void SetParamPageRecipeIDProperties(Page obj)
        {
            object objDataContext = obj.DataContext;
            PropertyInfo[] piProperties = objDataContext.GetType().GetProperties();

            foreach (PropertyInfo pi in piProperties)
            {
                if (pi.Name == "RecipeID")
                    pi.SetValue(objDataContext, SelectedRecipe.RecipeId, null);
            }
        }

        //----------------------------------------------------------------------//
        //                              Commands                                //
        //----------------------------------------------------------------------//


        public ICommand SaveRecipeCommand
        {
            get { return cmdSaveRecipe; }
        }
        public void cmdSaveRecipeImplementation()
        {
            //Does Recipe exist? If it does, Update else Insert
            try
            {
                int iRecipeID = SelectedRecipe.RecipeId;

                if (iRecipeID <= 0)
                {
                    //Insert new Recipe
                    using (SqlConnection conn = new SqlConnection(sqlConnection))
                    {
                        conn.Open();
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "INSERT INTO Recipes (RecipeName, BatchWeight, ProcessLine) VALUES ('" + SelectedRecipe.RecipeName + "'," + SelectedRecipe.RecipeBatchWeight + ", '" + SelectedRecipe.ProcessLineName + "')";
                        int iRowsInserted = cmd.ExecuteNonQuery();
                        conn.Close();
                    }

                    //Get Recipe ID
                    using (SqlConnection conn = new SqlConnection(sqlConnection))
                    {
                        conn.Open();
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "SELECT RecipeID FROM Recipes WHERE RecipeName = '" + SelectedRecipe.RecipeName + "'";
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                            iRecipeID = reader.GetInt32(0);

                        SelectedRecipe.RecipeId = iRecipeID;
                        SetParamPageRecipeIDProperties(ParameterPage);

                        conn.Close();
                    }

                    //Insert Recipe/Ingredients
                    using (SqlConnection conn = new SqlConnection(sqlConnection))
                    {
                        conn.Open();
                        SqlCommand cmd = conn.CreateCommand();
                        StringBuilder sbCommandText = new StringBuilder("INSERT INTO RecipeIngredient VALUES ");

                        int iSortIndex = 0;
                        foreach (Ingredient i in SelectedRecipe.UsedRecipeIngredients)
                            sbCommandText.Append("(" + iRecipeID + "," + i.IngredientID + "," + iSortIndex++ + "," + i.IngredientPercentageValue + "," + i.IngredientKilogramValue + "),");

                        sbCommandText.Remove(sbCommandText.Length - 1, 1); //Remove Last ,

                        cmd.CommandText = sbCommandText.ToString();
                        int iRowsInserted = cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                else
                {
                    //Update Recipe
                    using (SqlConnection conn = new SqlConnection(sqlConnection))
                    {
                        conn.Open();
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "UPDATE Recipes SET RecipeName = '" + SelectedRecipe.RecipeName + "', BatchWeight = " + SelectedRecipe.RecipeBatchWeight + ", ProcessLine = '" + SelectedRecipe.ProcessLineName + "' WHERE RecipeID = " + SelectedRecipe.RecipeId;
                        int iRowsUpdated = cmd.ExecuteNonQuery();
                        conn.Close();
                    }

                    //Update Recipe/Ingredients
                    //Delete existing records
                    using (SqlConnection conn = new SqlConnection(sqlConnection))
                    {
                        conn.Open();
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "DELETE RecipeIngredient WHERE RecipeID = " + SelectedRecipe.RecipeId;
                        int iRowsUpdated = cmd.ExecuteNonQuery();
                        conn.Close();
                    }

                    //Insert Updated records
                    using (SqlConnection conn = new SqlConnection(sqlConnection))
                    {
                        conn.Open();
                        SqlCommand cmd = conn.CreateCommand();
                        StringBuilder sbCommandText = new StringBuilder("INSERT INTO RecipeIngredient VALUES ");

                        int iSortIndex = 0;
                        foreach (Ingredient i in SelectedRecipe.UsedRecipeIngredients)
                            sbCommandText.Append("(" + iRecipeID + "," + i.IngredientID + "," + iSortIndex++ + "," + i.IngredientPercentageValue + "," + i.IngredientKilogramValue + "),");


                        sbCommandText.Remove(sbCommandText.Length - 1, 1); //Remove Last ,

                        cmd.CommandText = sbCommandText.ToString();
                        int iRowsInserted = cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorMessgae = ex.Message;
            }
        }


        public ICommand NewRecipeCommand
        {
            get { return cmdNewRecipe; }
        }
        public void cmdNewRecipeImplementation()
        {
            Recipe r = new Recipe() { RecipeId = -1, RecipeName = "New Recipe" };
            r.ProcessLineName = ProcessLineName;
            AvailableRecipes.Add(r);
            SelectedRecipe = r;
        }



        public ICommand CancelCommand
        {
            get { return cmdCancel; }
        }
        public void cmdCancelImplementation()
        {

        }


        public ICommand UseReceiverBinCommand
        {
            get { return cmdUseReceiverBin; }
        }
        public void cmdUseReceiverBinImplementation()
        {
            if (SelectedAvailableReceiverBin != null)
            {
                UsedReceiverBins.Add(SelectedAvailableReceiverBin);
                AvailableReceiverBins.Remove(SelectedAvailableReceiverBin);
            }
        }


        public ICommand RemoveReceiverBinCommand
        {
            get { return cmdRemoveReceiverBin; }
        }
        public void cmdRemoveReceiverBinImplementation()
        {
            if (SelectedUsedReceiverBin != null)
            {
                AvailableReceiverBins.Add(SelectedUsedReceiverBin);
                UsedReceiverBins.Remove(SelectedUsedReceiverBin);
            }
        }

        public ICommand DownloadCommand
        {
            get { return cmdDownload; }
        }
        public void cmdDownloadImplementation()
        {
            try
            {
                //Sort the used ingredients by ScaleNumber
                var cvs = CollectionViewSource.GetDefaultView(SelectedRecipe.UsedRecipeIngredients);
                if (cvs.SortDescriptions.Count <= 0) //Only add sortDescription once
                    cvs.SortDescriptions.Add(new SortDescription("IngredientScaleNumber", ListSortDirection.Ascending));


                string sDBNumber = "";

                //
                //Download Recipe
                //
                foreach (CWAScaleDBInfo cwadb in ScaleDBInfo)
                {
                    //Get all used ingredients for current scale
                    var RecipeItems = from Ingredients in SelectedRecipe.UsedRecipeIngredients
                                      where Ingredients.IngredientScaleNumber == cwadb.ScaleNumber
                                      select Ingredients;


                    int iNumOfUsedInredients = RecipeItems.Count();                                  //Number of ingredients for the current scale

                    sDBNumber = cwadb.FirstIngredientDBOffset.Substring(0, cwadb.FirstIngredientDBOffset.IndexOf('.')); //eg. DB820         

                    int iStartBD = Convert.ToInt32(cwadb.FirstIngredientDBOffset.Substring(cwadb.FirstIngredientDBOffset.IndexOf("DBW"), cwadb.FirstIngredientDBOffset.Length - cwadb.FirstIngredientDBOffset.IndexOf("DBW")).Remove(0, 3));

                    int iTargetDB = iStartBD + 4;                                                   //ValueTarget is 4 spaces on from the BinNumber

                    //Write ingredients into recipe DB
                    foreach (Ingredient i in RecipeItems)
                    {
                        int iBin = i.IngredientBinNumber;
                        double dGramValue = i.IngredientKilogramValue * 1000;

                        Tag tBin = new Tag(sDBNumber + ".DBW" + iStartBD, Tag.ATOMIC.WORD, 1);
                        Tag tTarget = new Tag(sDBNumber + ".DBD" + iTargetDB, Tag.ATOMIC.DWORD, 1);
                        tBin.Value = iBin;
                        tTarget.Value = dGramValue;

                        if (!PLCW.IsConnected)
                            PLCW.Connect();

                        PLCW.WriteTag(tBin);
                        PLCW.WriteTag(tTarget);

                        iStartBD += 10;                                                             //Next BinNumberOffset is 10 spaces on 
                        iTargetDB += 10;                                                            //Next ValueTarget is 10 spaces on 
                    }

                    //Write zeros into the remaining slots of the ScaleDBInfo
                    for (int i = iNumOfUsedInredients; i < cwadb.NumberOfIngredients; i++)
                    {
                        Tag t = new Tag(sDBNumber + ".DBW" + iStartBD, Tag.ATOMIC.WORD, 1);
                        t.Value = 0;

                        if (!PLCW.IsConnected)
                            PLCW.Connect();

                        PLCW.WriteTag(t);

                        iStartBD += 10;                                                             //Next BinNumberOffset is 10 spaces on 
                        iTargetDB += 10;                                                            //Next ValueTarget is 10 spaces on 
                    }

                    //Write ObjMask And ObjUsed If Scale Has Ingredients assigned
                    if (RecipeItems.Count() > 0)
                    {
                        Tag tObjMask = new Tag(cwadb.ObjMaskDBOffset, Tag.ATOMIC.BOOL, 1);
                        Tag tObjUsed = new Tag(cwadb.ObjUsedDBOffset, Tag.ATOMIC.BOOL, 1);
                        tObjMask.Value = true;
                        tObjUsed.Value = true;

                        if (!PLCW.IsConnected)
                            PLCW.Connect();

                        PLCW.WriteTag(tObjMask);
                        PLCW.WriteTag(tObjUsed);
                    }
                }


                //*******TODO*******
                //
                //Download Job Parameters
                //
                var JobPageDataContext = (MIX1JobParameterViewModel)JobPage.DataContext;
                foreach (object key in JobPageDataContext.htParameters.Keys)
                {
                    string DBOffset = ((ParameterValue)JobPageDataContext.htParameters[key]).DBOffset;
                    S7Link.Tag.ATOMIC TagDataType = ((ParameterValue)JobPageDataContext.htParameters[key]).TagDataType;
                    string value = ((ParameterValue)JobPageDataContext.htParameters[key]).Value;

                    Tag t = new Tag(DBOffset, TagDataType, 1);
                    //Tag AS = new Tag("DB189.DBD76", 1);
                    t.Value = value;
                    //AS.Value = value;

                    if (!PLCW.IsConnected)
                        PLCW.Connect();

                    PLCW.WriteTag(t);
                }


                //
                //Other Job Info
                //
                Tag tJobNumber = new Tag(sDBNumber + ".DBD350", Tag.ATOMIC.DWORD, 1);               //Job Number
                Tag tBatchCount = new Tag(((ParameterValue)JobPageDataContext.htParameters["NumberOfBatches"]).DBOffset,
                                          ((ParameterValue)JobPageDataContext.htParameters["NumberOfBatches"]).TagDataType, 1);              //Number of batches
                Tag tBatchWeight = new Tag(sDBNumber + ".DBD370", Tag.ATOMIC.DINT, 1);             //Batch Weight
                Tag tJobWeight = new Tag(sDBNumber + ".DBW354", Tag.ATOMIC.DWORD, 1);               //Job Weight
                //Tag tAirlockSpeed = new Tag(((ParameterValue)JobPageDataContext.htParameters["AirlockSpeed"]).DBOffset,
                //                          ((ParameterValue)JobPageDataContext.htParameters["AirlockSpeed"]).TagDataType, 1);                    //Airlock Speed Setpoint

                //Get latest JobNumber
                int iJobNumber = 0;
                using (SqlConnection conn = new SqlConnection(MainWindow.SqlConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "select isnull(MAX(JobNumber),0) from Jobs";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                        iJobNumber = reader.GetInt32(0);
                    reader.Close();
                }
                iJobNumber++;

                tJobNumber.Value = iJobNumber;
                tBatchCount.Value = ((ParameterValue)JobPageDataContext.htParameters["NumberOfBatches"]).Value;
                tBatchWeight.Value = SelectedRecipe.RecipeBatchWeight * 1000;
                tJobWeight.Value = (SelectedRecipe.RecipeBatchWeight * 1000) * Convert.ToInt32(((ParameterValue)JobPageDataContext.htParameters["NumberOfBatches"]).Value);

                if (!PLCW.IsConnected)
                    PLCW.Connect();

                PLCW.WriteTag(tJobNumber);
                PLCW.WriteTag(tBatchCount);
                PLCW.WriteTag(tBatchWeight);
                PLCW.WriteTag(tJobWeight);
                //PLCW.WriteTag(tAirlockSpeed);


                //
                //Download Receivers
                //
                int iIndex = 0;
                foreach (GcProReceiverData g in GcProReceiverDataList)
                {
                    Tag t = new Tag(g.ReceiverDBOffset, Tag.ATOMIC.WORD, 1);

                    if (UsedReceiverBins.Count > 0 && iIndex < UsedReceiverBins.Count)
                        t.Value = UsedReceiverBins[iIndex].BinId;
                    else
                        t.Value = 0;

                    if (!PLCW.IsConnected)
                        PLCW.Connect();

                    PLCW.WriteTag(t);

                    iIndex++;
                }

                //
                //Save Start Of job to Database
                //                                
                using (SqlConnection conn = new SqlConnection(MainWindow.SqlConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "insert into jobs (JobNumber, StartDate) values (" + iJobNumber + ",'" + DateTime.Now + "')";
                    int iRowsInserted = cmd.ExecuteNonQuery();
                }

                iJobNumber++;
            }
            catch (Exception)
            {

            }
            finally
            {
                PLCW.Disconnect();
            }
        }


        //----------------------------------------------------------------------//
        //                             Properties                               //
        //----------------------------------------------------------------------//


        public ObservableCollection<Ingredient> AllIngredients
        {
            get { return ocAllIngredients; }
            set { ocAllIngredients = value; }
        }

        public ObservableCollection<Recipe> AvailableRecipes
        {
            get { return ocAvailableRecipes; }
            set { ocAvailableRecipes = value; }
        }

        public ObservableCollection<Bin> UsedReceiverBins
        {
            get { return ocUsedReceiverBins; }
            set
            {
                ocUsedReceiverBins = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UsedReceiverBins"));
            }
        }

        public ObservableCollection<Bin> AvailableReceiverBins
        {
            get { return ocAvailableReceiverBins; }
            set
            {
                ocAvailableReceiverBins = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AvailableReceiverBins"));
            }
        }

        public ObservableCollection<GcProReceiverData> GcProReceiverDataList
        {
            get { return ocGcProReceiverData; }
            set
            {
                ocGcProReceiverData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GcProReceiverDataList"));
            }
        }

        private Recipe _SelectedRecipe;
        public Recipe SelectedRecipe
        {
            get { return _SelectedRecipe; }
            set
            {
                if (value != _SelectedRecipe)
                {
                    _SelectedRecipe = value;
                    Recipe._stat_RecipeBatchWeight = value.RecipeBatchWeight;


                    SelectedRecipe.AvailableRecipeIngredients.Clear();

                    var res = from c in AllIngredients
                              where !SelectedRecipe.UsedRecipeIngredients.Select(o => o.IngredientID).Contains(c.IngredientID)
                              select c;

                    List<Ingredient> l = res.ToList();
                    foreach (Ingredient i in l)
                    {
                        SelectedRecipe.AvailableRecipeIngredients.Add(i);
                    }

                    //Used for the Scale Limit (Ensure that the sum of the ingredients can fit into the scale)
                    SelectedRecipe.TotalWeightOfIngredients = 0;
                    foreach (Ingredient i in SelectedRecipe.UsedRecipeIngredients)
                    {
                        SelectedRecipe.TotalWeightOfIngredients += i.IngredientKilogramValue;
                        SelectedRecipe.TotalPercentageOfIngredients += i.IngredientPercentageValue;
                    }
                }

                SetParamPageRecipeIDProperties(ParameterPage);
            }
        }


        //When ingredient in Ingredient Class changes, the selected recipe total is updated
        //-->
        public static event EventHandler IngredientPercentageChange;
        private void IngredientsViewModel_IngredientPercentageChange(object sender, EventArgs e)
        {
            try
            {
                if (SelectedRecipe != null)
                {
                    SelectedRecipe.TotalPercentageOfIngredients = 0;
                    foreach (Ingredient i in SelectedRecipe.UsedRecipeIngredients)
                    {
                        if (i.IngredientType != 2)
                        {
                            SelectedRecipe.TotalPercentageOfIngredients += i.IngredientPercentageValue;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private static double _IngredientTotalPercentage;
        public static double IngredientTotalPercentageChanged
        {
            get { return _IngredientTotalPercentage; }
            set
            {
                _IngredientTotalPercentage = value;
                IngredientPercentageChange(value, new EventArgs());
            }
        }


        public static event EventHandler IngredientWeightChange;
        private void Recipe_IngredientWeightChange(object sender, EventArgs e)
        {
            try
            {
                if (SelectedRecipe != null)
                {
                    SelectedRecipe.TotalWeightOfIngredients = 0;
                    foreach (Ingredient i in SelectedRecipe.UsedRecipeIngredients)
                    {
                       // if (i.IngredientType != 2)
                       // {
                            SelectedRecipe.TotalWeightOfIngredients += i.IngredientKilogramValue;
                       // }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private static double _IngredientTotals;
        public static double IngredientTotalChanged
        {
            get { return _IngredientTotals; }
            set
            {
                _IngredientTotals = value;
                IngredientWeightChange(value, new EventArgs());
            }
        }
        //<--


        private Bin _SelectedUsedReceiverBin;
        public Bin SelectedUsedReceiverBin
        {
            get { return _SelectedUsedReceiverBin; }
            set
            {
                _SelectedUsedReceiverBin = value;
            }
        }

        private Bin _SelectedAvailableReceiverBin;
        public Bin SelectedAvailableReceiverBin
        {
            get { return _SelectedAvailableReceiverBin; }
            set
            {
                _SelectedAvailableReceiverBin = value;
            }
        }

        private Controller _PLCW;
        public Controller PLCW
        {
            get { return _PLCW; }
            set { _PLCW = value; }
        }

        private Controller _PLCR;
        public Controller PLCR
        {
            get { return _PLCR; }
            set { _PLCR = value; }
        }

        private string _ProcessLineName;
        public string ProcessLineName
        {
            get { return _ProcessLineName; }
            set { _ProcessLineName = value; }
        }

        private Page _ParameterPage;
        public Page ParameterPage
        {
            get { return _ParameterPage; }
            set { _ParameterPage = value; }
        }

        private List<CWAScaleDBInfo> _ScaleDBInfo;
        public List<CWAScaleDBInfo> ScaleDBInfo
        {
            get { return _ScaleDBInfo; }
            set { _ScaleDBInfo = value; }
        }

        private static int _MixerCapacity;
        public static int MixerCapacity
        {
            get { return _MixerCapacity; }
            set { _MixerCapacity = value; }
        }


        //----------------------------------------------------------------------//
        //                                  Events                              //
        //----------------------------------------------------------------------//

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}
