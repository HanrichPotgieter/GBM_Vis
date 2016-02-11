using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Input;

namespace KNEKT.Classes
{
    public class MIX1JobParameterViewModel : INotifyPropertyChanged
    {
        public Hashtable htParameters = new Hashtable();
        public DelegateCommand cmdSaveRecipeParameters;
        public DelegateCommand cmdRestoreRecipeParameters;

        //----------------------------------------------------------------------//
        //                               Constructor                            //
        //----------------------------------------------------------------------//
        public MIX1JobParameterViewModel()
        {
            //---------------Modify this section for each Parameter Page----------------//
            htParameters.Add("PreMixingTime", new ParameterValue() { Value = "0", DBOffset = "DB820.DBD422", TagDataType = S7Link.Tag.ATOMIC.DWORD });
            htParameters.Add("MainMixingTime", new ParameterValue() { Value = "0", DBOffset = "DB820.DBD426", TagDataType = S7Link.Tag.ATOMIC.DWORD });
            htParameters.Add("NumberOfBatches", new ParameterValue() { Value = "0", DBOffset = "DB820.DBW358", TagDataType = S7Link.Tag.ATOMIC.WORD });  //**DO NOT CHANGE THE NAME OF THIS PARAMETER
            htParameters.Add("AirlockSpeed", new ParameterValue() { Value = "0", DBOffset = "DB318.DBD78", TagDataType = S7Link.Tag.ATOMIC.REAL });
            htParameters.Add("FlapToBagOff", new ParameterValue() { Value = "false", DBOffset = "DB820.DBX410.0", TagDataType = S7Link.Tag.ATOMIC.BOOL });


            cmdSaveRecipeParameters = new DelegateCommand(cmdSaveRecipeParametersImplementation);
            cmdRestoreRecipeParameters = new DelegateCommand(cmdRestoreRecipeParametersImplementation);
        }

        string sqlConnection = MainWindow.SqlConnectionString;//"Data Source=JHBY03\\SQLEXPRESS;Initial Catalog=KNEKT_AFGRI;User Id=sa;Password=SQLpassword1234;Connection Timeout=10;";


        //----------------------------------------------------------------------//
        //                                Methods                               //
        //----------------------------------------------------------------------//

        private void GetRecipeParameters()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(sqlConnection))
                {
                    sqlConn.Open();
                    SqlCommand cmd = sqlConn.CreateCommand();
                    cmd.CommandText = "Select * from RecipeParameters Where RecipeID = " + RecipeID;
                    SqlDataReader reader = cmd.ExecuteReader();

                    bool bHasRows = false;
                    while (reader.Read())
                    {
                        ((ParameterValue)htParameters[reader["ParameterName"]]).Value = reader["ParameterValue"].ToString();
                        bHasRows = true;
                    }

                    if (!bHasRows)
                    {
                        ((ParameterValue)htParameters["PreMixingTime"]).Value = "0";
                        ((ParameterValue)htParameters["MainMixingTime"]).Value = "0";
                        ((ParameterValue)htParameters["NumberOfBatches"]).Value = "0";
                        ((ParameterValue)htParameters["AirlockSpeed"]).Value = "0";
                        ((ParameterValue)htParameters["FlapToBagOff"]).Value = "false";
                    }

                    sqlConn.Close();
                }

                //Update Properties
                PreMixingTime = (((ParameterValue)htParameters["PreMixingTime"]).Value).ToString();
                MainMixingTime = (((ParameterValue)htParameters["MainMixingTime"]).Value).ToString();
                NumberOfBatches = (((ParameterValue)htParameters["NumberOfBatches"]).Value).ToString();
                AirlockSpeed = (((ParameterValue)htParameters["AirlockSpeed"]).Value).ToString();
                FlapToBagOff = Convert.ToBoolean((((ParameterValue)htParameters["FlapToBagOff"]).Value).ToString());
            }
            catch (Exception)
            {
                //ErrorMessgae = ex.Message;
            }
        }

        //----------------------------------------------------------------------//
        //                               Properties                             //
        //----------------------------------------------------------------------//

        private int _RecipeID;
        public int RecipeID
        {
            get { return _RecipeID; }
            set
            {
                _RecipeID = value;
                GetRecipeParameters();
            }
        }


        public Hashtable HTParameters
        {
            get { return htParameters; }
        }

        //----------------------------------------------------------------------//


        private string _PreMixingTime;
        public string PreMixingTime
        {
            get { return _PreMixingTime; }
            set
            {
                _PreMixingTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreMixingTime"), value.ToString());
            }
        }

        private string _MainMixingTime;
        public string MainMixingTime
        {
            get { return _MainMixingTime; }
            set
            {
                _MainMixingTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MainMixingTime"), value.ToString());
            }
        }

        private string _NumberOfBatches;
        public string NumberOfBatches
        {
            get { return _NumberOfBatches; }
            set
            {
                _NumberOfBatches = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NumberOfBatches"), value.ToString());
            }
        }

        private string _AirlockSpeed;
        public string AirlockSpeed
        {
            get { return _AirlockSpeed; }
            set
            {
                _AirlockSpeed = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AirlockSpeed"), value.ToString());
            }
        }

        private bool _FlapToBagOff;
        public bool FlapToBagOff
        {
            get { return _FlapToBagOff; }
            set
            {
                _FlapToBagOff = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FlapToBagOff"), value.ToString());
            }
        }


        //----------------------------------------------------------------------//
        //                               Events                                 //
        //----------------------------------------------------------------------//

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e, string PropertyValue)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
                ((ParameterValue)htParameters[e.PropertyName]).Value = PropertyValue;
            }
        }


        //----------------------------------------------------------------------//
        //                              Commands                                //
        //----------------------------------------------------------------------//


        public ICommand SaveRecipeParameters
        {
            get { return cmdSaveRecipeParameters; }
        }
        public void cmdSaveRecipeParametersImplementation()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConnection))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "DELETE RecipeParameters WHERE RecipeID = " + RecipeID;
                    int iRowsUpdated = cmd.ExecuteNonQuery();
                    conn.Close();
                }

                //Insert Updated records
                using (SqlConnection conn = new SqlConnection(sqlConnection))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    StringBuilder sbCommandText = new StringBuilder("INSERT INTO RecipeParameters VALUES ");

                    foreach (object key in htParameters.Keys)
                    {
                        sbCommandText.Append("(" + RecipeID + ",'" + key.ToString() + "','" + ((ParameterValue)htParameters[key]).Value.ToString() + "'),");
                    }

                    sbCommandText.Remove(sbCommandText.Length - 1, 1); //Remove Last ,

                    cmd.CommandText = sbCommandText.ToString();
                    int iRowsInserted = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            catch (Exception)
            {
                //ErrorMessgae = ex.Message;
            }
        }

        public ICommand RestoreRecipeParameters
        {
            get { return cmdRestoreRecipeParameters; }
        }
        public void cmdRestoreRecipeParametersImplementation()
        {
            GetRecipeParameters();
        }
    }
}
