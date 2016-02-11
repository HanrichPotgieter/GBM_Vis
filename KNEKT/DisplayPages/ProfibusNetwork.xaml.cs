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
using System.Diagnostics;
using S7Link;
using System.Data;
using System.Data.SqlClient;

namespace KNEKT.DisplayPages
{
    /// <summary>
    /// Interaction logic for ProfibusNetwork.xaml
    /// </summary>
    public partial class ProfibusNetwork : Page
    {
        MainWindow mw;
        Controller PLC_W;
        Controller PLC_R;


        //------------------------------------------------------------------------------//
        //                                  Constructors                                //
        //------------------------------------------------------------------------------//

        public ProfibusNetwork()
        {
            InitializeComponent();

            MainWindow.sActiveLineName = "ProfibusNetwork";

            //SetPLCAddress();
            //SetPingLabel();
            //SetCheckBoxes();

            //LoadUserEvents();
        }

        public ProfibusNetwork(MainWindow mainWindow)
        {
            InitializeComponent();

            MainWindow.sActiveLineName = "Settings";
            mw = mainWindow;

            //SetPLCAddress();
            //SetPingLabel();
            //SetCheckBoxes();

            //LoadUserEvents();
        }

        public ProfibusNetwork(MainWindow mainWindow, Controller ReadController, Controller WriteController)
        {
            InitializeComponent();

            MainWindow.sActiveLineName = "ProfibusNetwork";

            mw = mainWindow;
            PLC_W = WriteController;
            PLC_R = ReadController;

            //SetPLCAddress();
            //SetPingLabel();
            //SetPLCLabels();
            //SetTagLabels();
            //SetReportPath();
            //SetCheckBoxes();

            //LoadUserEvents();
        }


        //------------------------------------------------------------------------------//
        //                          Import Window Handle DLLs                           //
        //------------------------------------------------------------------------------//


        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //public static extern int SetWindowPos(IntPtr hwnd, IntPtr
        //hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //public static extern int BringWindowToTop(IntPtr hwnd);


        //public IntPtr HWND_TOPMOST = (IntPtr)(-1);
        //public IntPtr HWND_NOTOPMOST = (IntPtr)(-2);
        //public int SWP_NOSIZE = 0x1;




        //------------------------------------------------------------------------------//
        //                          Functionality Methods                               //
        //------------------------------------------------------------------------------//

        //public void SetPingLabel()
        //{
        //    lblPLCPing.Content = "Able to Ping PLC : " + MainWindow.bPLCCommsGood;
        //    txtAutoLogOff.Text = "" + MainWindow.stat_iLogOffTime;
        //}

        //public void SetPLCAddress()
        //{
        //    try
        //    {
        //        txtPLCIP.Text = MainWindow.PLCIpAddress;
        //        txtPLCRack.Text = MainWindow.PLCRackNo;
        //        txtPLCSlot.Text = MainWindow.PLCSlotNum;
        //    }
        //    catch { }
        //}

        //public void SetPLCLabels()
        //{
        //    try
        //    {
        //        lblPLC_R_Connect.Content = "Connected : " + PLC_R.IsConnected;
        //        lblPLC_R_Error.Content = "Error           : " + PLC_R.ErrorCode;
        //        lblPLC_R_Controller.Content = "Controller   : " + PLC_R.IPAddress;

        //        lblPLC_W_Connect.Content = "Connected : " + PLC_W.IsConnected;
        //        lblPLC_W_Error.Content = "Error           : " + PLC_W.ErrorCode;
        //        lblPLC_W_Controller.Content = "Controller   : " + PLC_W.IPAddress;
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        //public void SetReportPath()
        //{
        //    txtReportViewerPath.Text = "" + MainWindow.stat_ReportViewerAddress;
        //}

        //public void SetCheckBoxes()
        //{
        //    cbShowHints.IsChecked = MainWindow.stat_bShowApplicationHints;
        //    cbElementNumbers.IsChecked = MainWindow.bShowTagnames;
        //}

        //public void SetTagLabels()
        //{
        //    try
        //    {
        //        int iSmartGood = 0;
        //        int iSmartBad = 0;
        //        int iSmartUncer = 0;
        //        int iAddGood = 0;
        //        int iAddBad = 0;
        //        int iAddUncer = 0;

        //        for (int i = 0; i < mw.tagroupSmartTags.Count; i++)
        //        {
        //            Tag t = mw.tagroupSmartTags.Tags[i] as Tag;
        //            int quality = t.QualityCode;

        //            if (quality == 0)
        //            {
        //                iSmartBad++;
        //            }
        //            else if (quality == 64)
        //            {
        //                iSmartUncer++;
        //            }
        //            else if (quality == 192)
        //            {
        //                iSmartGood++;
        //            }
        //        }

        //        for (int i = 0; i < mw.tagroupAdditionalSmartTags.Count; i++)
        //        {
        //            Tag t = mw.tagroupAdditionalSmartTags.Tags[i] as Tag;
        //            int quality = t.QualityCode;

        //            if (quality == 0)
        //            {
        //                iAddBad++;
        //            }
        //            else if (quality == 64)
        //            {
        //                iAddUncer++;
        //            }
        //            else if (quality == 192)
        //            {
        //                iAddGood++;
        //            }
        //        }

        //        lblSmartTag_Good.Content = "Good       : " + iSmartGood;
        //        lblSmartTag_Uncert.Content = "Uncertain : " + iSmartUncer;
        //        lblSmartTag_Bad.Content = "Bad          : " + iSmartBad;

        //        lblAdditionalTag_Good.Content = "Good       : " + iAddGood;
        //        lblAdditionalTag_Uncert.Content = "Uncertain : " + iAddUncer;
        //        lblAdditionalTag_Bad.Content = "Bad          : " + iAddBad;
        //    }
        //    catch { }
        //}



        //------------------------------------------------------------------------------//
        //                                  Button Clicks                               //
        //------------------------------------------------------------------------------//

        //private void btnImportTags_Click(object sender, RoutedEventArgs e)
        //{
        //    NavigationService.Navigate(new _PagesSettings.SmartTags());
        //}

        //private void btnLineParameters_Click(object sender, RoutedEventArgs e)
        //{
        //    NavigationService.Navigate(new _PagesSettings.LineCommandSettings());
        //}

        //private void btnAdditionalElements_Click(object sender, RoutedEventArgs e)
        //{
        //    NavigationService.Navigate(new _PagesSettings.AdditionalTags());
        //}


        //private void btnUserManagement_Click(object sender, RoutedEventArgs e)
        //{
        //    NavigationService.Navigate(new DisplayPages.UserManagement(mw));
        //}

        //private void btnSQLSetup_Click(object sender, RoutedEventArgs e)
        //{
        //    NavigationService.Navigate(new _PagesSettings.SQLSettings());
        //}



        //private void btnStartTaskMan_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        Process process = new Process();
        //        process.StartInfo.FileName = "taskmgr.exe";
        //        process.Start();
        //        process.WaitForInputIdle();
        //        BringWindowToTop(process.MainWindowHandle);
        //        SetWindowPos(process.MainWindowHandle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE);
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        //private void btnStartNotePad_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        Process process = new Process();
        //        process.StartInfo.FileName = "notepad.exe";
        //        process.Start();
        //        process.WaitForInputIdle();
        //        BringWindowToTop(process.MainWindowHandle);
        //        SetWindowPos(process.MainWindowHandle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE);
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}


        //private void btnSaveIP_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
        //        {
        //            sqlConn.Open();
        //            SqlCommand cmd1 = sqlConn.CreateCommand();
        //            SqlCommand cmd2 = sqlConn.CreateCommand();
        //            SqlCommand cmd3 = sqlConn.CreateCommand();

        //            cmd1.CommandText = "UPDATE ApplicationSettings SET SettingValue = '" + txtPLCIP.Text + "' WHERE SettingID = 'PLC_IP'";
        //            cmd2.CommandText = "UPDATE ApplicationSettings SET SettingValue = '" + txtPLCRack.Text + "' WHERE SettingID = 'PLC_RACK'";
        //            cmd3.CommandText = "UPDATE ApplicationSettings SET SettingValue = '" + txtPLCSlot.Text + "' WHERE SettingID = 'PLC_SLOT'";

        //            int IPUpdated = cmd1.ExecuteNonQuery();
        //            int RackUpdated = cmd2.ExecuteNonQuery();
        //            int SlotUpdated = cmd3.ExecuteNonQuery();

        //            if (IPUpdated > 0 & RackUpdated > 0 & SlotUpdated > 0)
        //            {
        //                MessageBox.Show("Please restart the application to apply the changes", "Update Successful", MessageBoxButton.OK, MessageBoxImage.Information);
        //            }
        //            else
        //            {
        //                MessageBox.Show("Update failed", "Update Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //            sqlConn.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Save IP --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        //private void btnSaveLogOff_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        int minutes = Convert.ToInt32(txtAutoLogOff.Text);
        //        int seconds = minutes * 60;

        //        using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
        //        {
        //            sqlConn.Open();
        //            SqlCommand cmd1 = sqlConn.CreateCommand();
        //            cmd1.CommandText = "UPDATE ApplicationSettings SET SettingValue = '" + seconds + "' WHERE SettingID = 'USR_LOGOFF'";
        //            int rowsUpdated = cmd1.ExecuteNonQuery();
        //            if (rowsUpdated > 0)
        //            {
        //                MessageBox.Show("Please restart the application to apply the changes", "Update Successful", MessageBoxButton.OK, MessageBoxImage.Information);
        //            }
        //            sqlConn.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Save LogOff --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}



        //private void btnRestart_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {

        //        MessageBoxResult mbr = MessageBox.Show("This action will restart the application. Continue?", "Restart Application?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

        //        if (mbr == MessageBoxResult.Yes)
        //        {
        //            try
        //            {
        //                using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
        //                {
        //                    sqlConn.Open();
        //                    SqlCommand cmd = sqlConn.CreateCommand();

        //                    string sInsert = "Application Restarted (" + MainWindow.stat_sLoggedInUser + ")";

        //                    cmd.CommandText = "INSERT INTO ApplicationLog VALUES ('" + DateTime.Now + "'," + DateTime.Now.ToOADate() + ",'" + btnRestart.Name + "','" + sInsert + "',30)";
        //                    int rowsInserted = cmd.ExecuteNonQuery();
        //                    sqlConn.Close();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("Log Restart --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
        //            Application.Current.Shutdown();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("App Restart --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        /// <summary>
        /// Loads all event 30 events from the application log
        /// </summary>
        //public void LoadUserEvents()
        //{
        //    try
        //    {
        //        using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
        //        {
        //            sqlConn.Open();
        //            SqlCommand cmd1 = sqlConn.CreateCommand();
        //            cmd1.CommandText = "SELECT ts_DateTime, ObjectName, ObjectAction FROM ApplicationLog WHERE ts_DateTime >= DATEADD(dd,-5,GETDATE()) AND Code = 30  ORDER BY OADate DESC";
        //            SqlDataReader reader = cmd1.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                lstBoxEvents.Items.Add("" + reader.GetDateTime(0) + "\t" + reader.GetString(1) + "\t" + reader.GetString(2));
        //            }

        //            sqlConn.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Load Events --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //SetPLCAddress();
            //SetPingLabel();
            //SetPLCLabels();
            //SetTagLabels();


            //lstBoxEvents.Items.Clear();
            //LoadUserEvents();
        }

        private void Grid_TouchDown(object sender, TouchEventArgs e)
        {
            MainWindow.stat_iLogOffCounter = 0;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.stat_iLogOffCounter = 0;
        }



        //private void cbShowHints_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
        //        {
        //            sqlConn.Open();
        //            SqlCommand cmd1 = sqlConn.CreateCommand();
        //            cmd1.CommandText = "UPDATE ApplicationSettings SET SettingValue = '" + cbShowHints.IsChecked + "' WHERE SettingID = 'APP_HINTS'";
        //            int rowsUpdated = cmd1.ExecuteNonQuery();
        //            if (rowsUpdated > 0)
        //            {
        //                MessageBox.Show("Please restart the application to apply the changes", "Update Successful", MessageBoxButton.OK, MessageBoxImage.Information);
        //            }
        //            sqlConn.Close();
        //        }

        //        MainWindow.bShowTagnames = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("ElementNumOff --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        //private void cbElementNumbers_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
        //        {
        //            sqlConn.Open();
        //            SqlCommand cmd1 = sqlConn.CreateCommand();
        //            cmd1.CommandText = "UPDATE ApplicationSettings SET SettingValue = '" + cbElementNumbers.IsChecked + "' WHERE SettingID = 'VIS_TAGNAMES'";
        //            int rowsUpdated = cmd1.ExecuteNonQuery();
        //            if (rowsUpdated > 0)
        //            {
        //                MessageBox.Show("Please restart the application to apply the changes", "Update Successful", MessageBoxButton.OK, MessageBoxImage.Information);
        //            }
        //            sqlConn.Close();
        //        }

        //        MainWindow.bShowTagnames = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("ElementNumOff --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        //private void btnDataLogging_Click(object sender, RoutedEventArgs e)
        //{
        //    NavigationService.Navigate(new _PagesSettings.DataLogger());
        //}

        //private void btnSaveReportViewerPath_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
        //        {
        //            sqlConn.Open();
        //            SqlCommand cmd1 = sqlConn.CreateCommand();
        //            cmd1.CommandText = "UPDATE ApplicationSettings SET SettingValue = '" + txtReportViewerPath.Text + "' WHERE SettingID = 'APP_REPORTVIEWPATH'";
        //            int rowsUpdated = cmd1.ExecuteNonQuery();
        //            if (rowsUpdated > 0)
        //            {
        //                MessageBox.Show("Please restart the application to apply the changes", "Update Successful", MessageBoxButton.OK, MessageBoxImage.Information);
        //            }
        //            sqlConn.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Please enter a valid web URL. " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

    }
}
