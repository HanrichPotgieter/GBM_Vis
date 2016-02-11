using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Collections;

namespace KNEKT._PagesSettings
{
    /// <summary>
    /// Interaction logic for SmartTags.xaml
    /// </summary>
    public partial class SmartTags : Page
    {
        private string SqlConnectionString;// = "Data Source=JHBM26;Initial Catalog=KNEKT;User Id=sa;Password=SQLpassword1234;";
        private string AccConnectionString = "";
        private string GcProFileName = "";

        public SmartTags()
        {
            InitializeComponent();

            SqlConnectionString = MainWindow.SqlConnectionString;
        }



        //------------------------------------------------------------------------------//
        //                              Buttton Clicks                                  //
        //------------------------------------------------------------------------------//

        private void btnChooseGcPro_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = GcProFileName;
            txtStep1GcProFileName.Text = GcProFileName;
            ofd.Title = "GcPro Database Path";
            ofd.Filter = "MDB Files *.mdb|*.mdb";

            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            { 
                GcProFileName = ofd.FileName;
                txtStep1GcProFileName.Text = GcProFileName;
                
                if (GcProFileName != "")
                {                   
                    groupBox1.IsEnabled = false;
                    groupBox2.IsEnabled = true;
                }
            }            
        }

        private void btnStep2Back_Click(object sender, RoutedEventArgs e)
        {
            groupBox1.IsEnabled = true;
            groupBox2.IsEnabled = false;
        }


        private void btnStep2Migrate_Click(object sender, RoutedEventArgs e)
        {
            ArrayList alGcProItems = new ArrayList();
            ArrayList alObjectIDs = new ArrayList();

            try
            {
            //---GET DATA FROM ACCESS

                AccConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + GcProFileName;// ";User Id=admin;Password=;";
                using (OleDbConnection oleConnection = new OleDbConnection(AccConnectionString))
                {
                    oleConnection.Open();
                    listViewProgress.Items.Add("OLE Connection Successful...");
                    OleDbCommand oleCmd = oleConnection.CreateCommand();
                    oleCmd.CommandText = "SELECT * FROM RetrieveElementsMod";
                    OleDbDataReader reader = oleCmd.ExecuteReader();
                    int counter = 0;
                    while (reader.Read())
                    {
                        //                                    ObjectNo,                     Tagname,                         Description,          GroupNo,                                                 Address,             ParMsgType                                         FB     
                        alGcProItems.Add(new GcProItem(reader.GetValue(0).ToString(), reader.GetString(2), CleanString(reader.GetString(3)), reader.GetString(4).Substring(0, 5), GetAddressFromString(reader.GetString(5)), reader.GetValue(7).ToString(), GetFBFromString(reader.GetValue(8).ToString())));
                        alObjectIDs.Add(reader.GetValue(0).ToString());
                        counter++;
                    }
                    listViewProgress.Items.Add(""+counter+" Records Read...");
                    oleConnection.Close();
                    listViewProgress.Items.Add("OLE Connection Closed...");
                    listViewProgress.Items.Add("");
                }


            //---PUSH ACCESS DATA INTO SQL
                
                // 1) Select all values from sql where the object id does not exist in alGcproItems
                // 2) Insert all new items
                // 3) Tell user that old items already exist and await further commands

                int RecordsInserted = 0;
                foreach(GcProItem gcp in alGcProItems)
                {
                    int rowInstered = SaveRowToSQL("INSERT INTO SmartTags VALUES ('" + gcp.pObjectNo + "','" + gcp.pTagname + "','" + gcp.pTagDesc + "','" + gcp.pGroupNo + "','" + gcp.pAddress + "','" + gcp.pPType + "','" + gcp.pFB + "',1,0,0,0,0,0)", gcp.pObjectNo.ToString());
                    
                    if (rowInstered == -1)  //En exception occurred in the SaveRowToSQL()
                    {
                        break;
                    }
                    RecordsInserted += rowInstered;
                }
                listViewProgress.Items.Add(""+RecordsInserted+" Rows Inserted to SQL...");
                
            }
            catch (Exception ex)
            {
                listViewProgress.Items.Add("EXCEPTION..."+ex.Message);
            }
            finally
            {
                listViewProgress.Items.Add("Complete.");
            }
        }


        //------------------------------------------------------------------------------//
        //                           Visual Format Methods                              //
        //------------------------------------------------------------------------------//


        //------------------------------------------------------------------------------//
        //                           Text Format Methods                                //
        //------------------------------------------------------------------------------//
        
        //Remove any unwanted special characters from the string
        private string CleanString(string StringToClean)
        {
            string s = "";

            foreach (char c in StringToClean)
            {
                //If the character is a "dirty" character leave it
                if (!(c == '\'' || c == '\\' || c == '"'))
                {
                    s += c;
                }
            }
            return s;
        }

        //Retrieve only the DB offset from string. eg.[DB101.DBB128] ObjNr:100001002
        private string GetAddressFromString(string StringToCheck)
        {
            string s = "";
            string t = "";

            int index = StringToCheck.IndexOf(" ObjNr:");
            s = StringToCheck.Substring(0, index);

            //eg.[DB101.DBB128]
            //Change DB[B] to DB[W]
            int indexB = s.LastIndexOf('B');
            int indexBPlusOne = indexB + 1;
            int lenght = s.Length-1;
            int remaining = lenght - indexB;

            t = s.Substring(0, indexB)+"W"+s.Substring(indexBPlusOne,remaining);

            return t;
        }

        //Return only the FB number from the string. eg.10[13] or 1[880]
        private string GetFBFromString(string FBstring)
        {
            string s = "";

            if (FBstring[1] == '0')
            {
                s = FBstring.Substring(2, 2);
            }
            else
            {
                s = FBstring.Substring(1, 3);
            }

            return s;
        }



        //------------------------------------------------------------------------------//
        //                           Database Methods                                   //
        //------------------------------------------------------------------------------//
        private int SaveRowToSQL(string cmd, string ID)
        {
            int rowsInserted = 0;
            try
            {
                using(SqlConnection sqlcon = new SqlConnection(SqlConnectionString))
                {
                    sqlcon.Open();
                    SqlCommand cmdSqlInsert = sqlcon.CreateCommand();
                    cmdSqlInsert.CommandText = cmd;
                    rowsInserted = cmdSqlInsert.ExecuteNonQuery();
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Substring(0, 35) == "Violation of PRIMARY KEY constraint")
                {
                    listViewProgress.Items.Add("Record Already Exists In Database");
                }
                else
                {
                    listViewProgress.Items.Add("EXCEPTION " + ex.Message);
                }
                rowsInserted = -1;
            }
            return rowsInserted;
        }


        //------------------------------------------------------------------------------//
        //                           Error Loggging                                     //
        //------------------------------------------------------------------------------//
        public void WriteToErrorLog(string Message)
        {

        }

        private void btnClearGcProSQLTags_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Would you like to clear all existing SmartTags from the Database that have been previously imported from GcPro?", "Confirmation Required", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (mbr == MessageBoxResult.Yes)
            {
                try
                {
                    using (SqlConnection sqlcon = new SqlConnection(SqlConnectionString))
                    {
                        sqlcon.Open();
                        SqlCommand cmdSqlDelete = sqlcon.CreateCommand();
                        cmdSqlDelete.CommandText = "DELETE SmartTags WHERE GcProTag = 1";
                        int rowsDeleted = cmdSqlDelete.ExecuteNonQuery();

                        if (rowsDeleted > 0)
                        {
                            MessageBox.Show("All "+rowsDeleted+" GcPro Tags have been removed from the Database", "Tags Removed Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                        sqlcon.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ClearTags >> "+ex.Message, "Failed to clear tags", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
