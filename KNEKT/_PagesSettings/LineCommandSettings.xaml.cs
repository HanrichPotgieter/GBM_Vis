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
using System.Data;
using System.Data.SqlClient;

namespace KNEKT._PagesSettings
{
    /// <summary>
    /// Interaction logic for LineCommandSettings.xaml
    /// </summary>
    public partial class LineCommandSettings : Page
    {


        //------------------------------------------------------------------------------//
        //                                  Constructor                                 //
        //------------------------------------------------------------------------------//        

        public LineCommandSettings()
        {
            InitializeComponent();

            GetLinesFromSQL();
        }


        //------------------------------------------------------------------------------//
        //                              Database Methods                                //
        //------------------------------------------------------------------------------//

        public void GetLinesFromSQL()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))      //Select all tags from SQL
                {
                    sqlConn.Open();
                    SqlCommand cmd = sqlConn.CreateCommand();
                    cmd.CommandText = "SELECT LIneNumber, LineName FROM LineParameters";
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        listBoxLines.Items.Add(reader.GetInt32(0) +") " + reader.GetString(1));
                    }
                    sqlConn.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
        }


        public void GetLineInfo(string Item)
        {
            try
            {
                int ilineNumber = Int32.Parse(Item.Substring(0, Item.IndexOf(')')));


                using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))      //Select all tags from SQL
                {
                    sqlConn.Open();
                    SqlCommand cmd = sqlConn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM LineParameters WHERE LineNumber = "+ilineNumber;
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {                       
                        //LINE INFO
                        txtLineDB.Text = reader.GetString(2);
                        txtcmdImmStop.Text = reader.GetString(3);
                        txtcmdHorn.Text = reader.GetString(4);
                        txtcmdSeqStop.Text = reader.GetString(5);
                        txtcmdFaultRes.Text = reader.GetString(6);
                        txtcmdFeedOn.Text = reader.GetString(7);
                        txtcmdStart.Text = reader.GetString(8);
                        txtcmdTransfer.Text = reader.GetString(9);
                        txtReqExec.Text = reader.GetString(10);
                        txtcmdFeedOff.Text = reader.GetString(11);
                        txtLineStateCode.Text = reader.GetString(12);

                        //SECTION INFO
                        txtS1DB.Text = reader.GetString(13);
                        txtS1State.Text = reader.GetString(14);
                        txtS1parEmpt.Text = reader.GetString(15);
                        txtS1outEmpt.Text = reader.GetString(16);
                        txtS1ErrCode.Text = reader.GetString(17);
                        txtS1ErrObj.Text = reader.GetString(18);
                        txtS2DB.Text = reader.GetString(19);
                        txtS2State.Text = reader.GetString(20);
                        txtS2parEmpt.Text = reader.GetString(21);
                        txtS2outEmpt.Text = reader.GetString(22);
                        txtS2ErrCode.Text = reader.GetString(23);
                        txtS2ErrObj.Text = reader.GetString(24);
                        txtS3DB.Text = reader.GetString(25);
                        txtS3State.Text = reader.GetString(26);
                        txtS3parEmpt.Text = reader.GetString(27);
                        txtS3outEmpt.Text = reader.GetString(28);
                        txtS3ErrCode.Text = reader.GetString(29);
                        txtS3ErrObj.Text = reader.GetString(30);
                    }
                    sqlConn.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
        }

        public void SetLineInfo(string Item)
        {
            try
            {
                int ilineNumber = Int32.Parse(Item.Substring(0, Item.IndexOf(')')));


                using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))      //Select all tags from SQL
                {
                    sqlConn.Open();
                    SqlCommand cmd = sqlConn.CreateCommand();
                    cmd.CommandText = "UPDATE LineParameters SET LineDB = '"+txtLineDB.Text+"', cmdImmediateStop = '"+txtcmdImmStop.Text+"', cmdHornOff = '"+txtcmdHorn.Text+"', "+
                                                "cmdSequenceStop = '"+txtcmdSeqStop.Text+"', cmdFaultReset = '"+txtcmdFaultRes.Text+"', cmdFeedOn = '"+txtcmdFeedOn.Text+"', "+
                                                "cmdStart = '"+txtcmdStart.Text+"', cmdTransferOn = '"+txtcmdTransfer.Text+"', RequestExecute = '"+txtReqExec.Text+"', "+
                                                "cmdFeedOff = '"+txtcmdFeedOff.Text+"', LineStateCode = '"+txtLineStateCode.Text+"' WHERE LineNumber = " + ilineNumber;
                    int rowsUpdated = cmd.ExecuteNonQuery();
                    if (rowsUpdated > 0)
                    {
                        MessageBox.Show("Line Information Updated Successfully");
                    }
                    sqlConn.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
        }

        public void SetSectionInfo(string Item)
        {
            try
            {
                int ilineNumber = Int32.Parse(Item.Substring(0, Item.IndexOf(')')));


                using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))      //Select all tags from SQL
                {
                    sqlConn.Open();
                    SqlCommand cmd = sqlConn.CreateCommand();
                    cmd.CommandText = "UPDATE LineParameters SET S1_DB = '" + txtS1DB.Text + "', S1_StateCode = '" + txtS1State.Text + "', S1_parEmptyingTime = '" + txtS1parEmpt.Text + "', S1_outEmptyingTime = '"+txtS1outEmpt.Text+"', S1_ErrorCode = '"+txtS1ErrCode.Text+"', S1_ErrorObject = '"+txtS1ErrObj.Text+"', " +
                                                                "S2_DB = '" + txtS2DB.Text + "', S2_StateCode = '" + txtS2State.Text + "', S2_parEmptyingTime = '" + txtS2parEmpt.Text + "', S2_outEmptyingTime = '"+txtS2outEmpt.Text+"', S2_ErrorCode = '"+txtS2ErrCode.Text+"', S2_ErrorObject = '"+txtS2ErrObj.Text+"', " +
                                                                "S3_DB = '" + txtS3DB.Text + "', S3_StateCode = '" + txtS3State.Text + "', S3_parEmptyingTime = '" + txtS3parEmpt.Text + "', S3_outEmptyingTime = '"+txtS3outEmpt.Text+"', S3_ErrorCode = '"+txtS3ErrCode.Text+"', S3_ErrorObject = '"+txtS3ErrObj.Text+"' "+
                                      "WHERE LineNumber = " + ilineNumber;
                    int rowsUpdated = cmd.ExecuteNonQuery();
                    if (rowsUpdated > 0)
                    {
                        MessageBox.Show("Section Information Updated Successfully");
                    }
                    sqlConn.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
        }

        //------------------------------------------------------------------------------//
        //                                    Events                                    //
        //------------------------------------------------------------------------------//        

        private void listBoxLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxLines.SelectedIndex >= 0)
            {
                GetLineInfo(listBoxLines.SelectedItem.ToString());
            }
        }

        private void btnUpdateLineInfo_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxLines.SelectedIndex >= 0)
            {
                SetLineInfo(listBoxLines.SelectedItem.ToString());
            }
        }

        private void btnUpdateSectionInfo_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxLines.SelectedIndex >= 0)
            {
                SetSectionInfo(listBoxLines.SelectedItem.ToString());
            }
        }

        private void btnClearLineInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Proceeding will clear this line's Information. Continue?", "Are you Sure?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                txtLineDB.Text = "";
                txtcmdImmStop.Text = "";
                txtcmdHorn.Text = "";
                txtcmdSeqStop.Text = "";
                txtcmdFaultRes.Text = "";
                txtcmdFeedOn.Text = "";
                txtcmdStart.Text = "";
                txtcmdTransfer.Text = "";
                txtReqExec.Text = "";
                txtcmdFeedOff.Text = "";
                txtLineStateCode.Text = "";
            }
        }

        private void btnClearSectionInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Proceeding will clear this line's section Information. Continue?", "Are you Sure?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                txtS1DB.Text = "";
                txtS1State.Text = "";
                txtS1parEmpt.Text = "";
                txtS1outEmpt.Text = "";
                txtS1ErrCode.Text = "";
                txtS1ErrObj.Text = "";
                txtS2DB.Text = "";
                txtS2State.Text = "";
                txtS2parEmpt.Text = "";
                txtS2outEmpt.Text = "";
                txtS2ErrCode.Text = "";
                txtS2ErrObj.Text = "";
                txtS3DB.Text = "";
                txtS3State.Text = "";
                txtS3parEmpt.Text = "";
                txtS3outEmpt.Text = "";
                txtS3ErrCode.Text = "";
                txtS3ErrObj.Text = "";
            }
        }
    }
}
