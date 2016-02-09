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
using System.IO;
using KNEKT.Classes;
using System.Diagnostics;

namespace KNEKT._PagesSettings
{
    /// <summary>
    /// Interaction logic for SQLSettings.xaml
    /// </summary>
    public partial class SQLSettings : Page
    {

        public SQLSettings()
        {
            InitializeComponent();

            StandardCode standardCode = new StandardCode();
            standardCode.GetDatabaseCredentials();

            txtServername.Text = MainWindow.sqlServername;
            txtDatabasename.Text = MainWindow.sqlDatabase;
            txtUsername.Text = MainWindow.sqlUsername;
            txtPassword.Password = MainWindow.sqlPassword;


        }

        public SQLSettings(MainWindow mw)
        {
            InitializeComponent();

            StandardCode standardCode = new StandardCode();
            standardCode.GetDatabaseCredentials();

            txtServername.Text = MainWindow.sqlServername;
            txtDatabasename.Text = MainWindow.sqlDatabase;
            txtUsername.Text = MainWindow.sqlUsername;
            txtPassword.Password = MainWindow.sqlPassword;
            // standardCode.HideLineButtons();

            if (!MainWindow.stat_bLoggedIn)
            {

                MainWindow.stat_iActiveLineNumber = 0;
                mw.HideLineButtons();
                mw.SetControlVisibilityOnLineChange();
                mw.btnSettings.Visibility = Visibility.Hidden;
                mw.btnStart.Visibility = Visibility.Hidden;
                mw.btnStop.Visibility = Visibility.Hidden;
                mw.btnLogOn.Visibility = Visibility.Hidden;
                mw.btnLogOff.Visibility = Visibility.Hidden;
                mw.imageBuhlerLogo.Opacity = 1;
                mw.btnReports.Visibility = Visibility.Hidden;
                mw.btnDisableZoom.Visibility = Visibility.Hidden;
                mw.btnEnableZoom.Visibility = Visibility.Hidden;
                mw.btnSaveZoom.Visibility = Visibility.Hidden;
                mw.image1.IsEnabled = false;
                mw.txtAlarms.IsEnabled = false;
                mw.lblLastReadTime.Visibility = Visibility.Hidden;
                mw.lblBadTags.Visibility = Visibility.Hidden;
                mw.txtLoggedInUser.IsEnabled = false;
                mw.progBarUserLevel.IsEnabled = false;
                //  standardCode.HideLineButtons(myMainWindow);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (txtDatabasename.Text == "")
            {
                lblError.Content = "* Enter SQL Database Name";
                lblError.Visibility = Visibility.Visible;
                txtDatabasename.Focus();
            }
            else if (txtPassword.Password == "")
            {
                lblError.Content = "* Enter SQL Password";
                lblError.Visibility = Visibility.Visible;
                txtPassword.Focus();
            }
            else if (txtServername.Text == "")
            {
                lblError.Content = "* Enter SQL Server Name";
                lblError.Visibility = Visibility.Visible;
                txtServername.Focus();
            }
            else if (txtUsername.Text == "")
            {
                lblError.Content = "* Enter SQL Username";
                lblError.Visibility = Visibility.Visible;
                txtUsername.Focus();
            }
            else
            {
                try
                {
                    FileStream fs = new FileStream(@"" + MainWindow.dllName, FileMode.Open, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);

                    SimpleAES aes = new SimpleAES();

                    string sServerName = aes.EncryptToString(txtServername.Text);
                    string sDatabaseName = aes.EncryptToString(txtDatabasename.Text);
                    string sUsername = aes.EncryptToString(txtUsername.Text);
                    string sPassword = aes.EncryptToString(txtPassword.Password);

                    sw.WriteLine(sServerName);
                    sw.WriteLine(sDatabaseName);
                    sw.WriteLine(sUsername);
                    sw.WriteLine(sPassword);

                    MessageBox.Show("Please restart the application to apply the changes", "Details Updated", MessageBoxButton.OK, MessageBoxImage.Information);

                    sw.Close();
                    fs.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Save SQL Cred --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void txtServername_MouseEnter(object sender, MouseEventArgs e)
        {
            lblError.Visibility = Visibility.Hidden;
        }

        private void txtDatabasename_MouseEnter(object sender, MouseEventArgs e)
        {
            lblError.Visibility = Visibility.Hidden;
        }

        private void txtUsername_MouseEnter(object sender, MouseEventArgs e)
        {
            lblError.Visibility = Visibility.Hidden;
        }

        private void txtPassword_MouseEnter(object sender, MouseEventArgs e)
        {
            lblError.Visibility = Visibility.Hidden;
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            StandardCode standardCode = new StandardCode();
            string SqlConnectionString = "Data Source=" + txtServername.Text + ";Initial Catalog=" + txtDatabasename.Text + ";User Id=" + txtUsername.Text + ";Password=" + txtPassword.Password + ";";
            bool connected = standardCode.TestSQLDatabaseConnection(SqlConnectionString);

            MessageBox.Show("Able to connect to SQL : " + connected, "Connection " + connected, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void txtPassword_TouchDown(object sender, TouchEventArgs e)
        {
            txtPassword.Focus();
            Process.Start("osk.exe");
        }

        private void txtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            Process[] pa = Process.GetProcessesByName("osk");
            for (int i = 0; i < pa.Length; i++)
            {
                pa[i].Kill();
            }
        }
    }
}
