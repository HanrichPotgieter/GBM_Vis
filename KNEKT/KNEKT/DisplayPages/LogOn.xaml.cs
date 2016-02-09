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
using System.Diagnostics;

namespace KNEKT.DisplayPages
{
    /// <summary>
    /// Interaction logic for UserManagement.xaml
    /// </summary>
    public partial class LogOn : Page
    {
        MainWindow mw;

        public LogOn()
        {
            InitializeComponent();
            //txtUsername.Focus();            
        }

        public LogOn(MainWindow mainWindow)
        {
            InitializeComponent();
            mw = mainWindow;
            //txtUsername.Focus();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
            {
                sqlConn.Open();
                SqlCommand cmd = sqlConn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Users WHERE UserName = '"+txtUsername.Text+"' AND UserPassword = '" + txtPassword.Password +"'";
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {                    
                    MainWindow.stat_sLoggedInUser = reader.GetString(1) + " " + reader.GetString(2);
                    MainWindow.stat_iUserLevel = reader.GetInt32(4);
                    MainWindow.bLoggedIn = true;
                    MainWindow.sCurrentUsername = "" + txtUsername.Text;
                    MainWindow.sCurrentPassword = "" + txtPassword.Password;

                    mw.UIInteraction_Change(this.btnLogin, e, "LogOn (" + reader.GetString(1) + " " + reader.GetString(2) + ")");                    

                    NavigationService.Navigate(new DisplayPages.StartPage());
                    mw.SetControlVisibilityOnLineChange();
                    MessageBox.Show("You are now logged in " + reader.GetString(1) + " " + reader.GetString(2), "Login Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Get an application-scope resource
                    //Application.Current.Resources["bLoggedIn"] = true;
                    //bool LoggedIn = (bool)Application.Current.Resources["bLoggedIn"];
                }
                else
                {
                    lblError.Visibility = System.Windows.Visibility.Visible;
                }
                sqlConn.Close();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DisplayPages.StartPage());
        }


        private void txtUsername_MouseEnter(object sender, MouseEventArgs e)
        {
            lblError.Visibility = System.Windows.Visibility.Hidden;
        }

        private void txtPassword_MouseEnter(object sender, MouseEventArgs e)
        {
            lblError.Visibility = System.Windows.Visibility.Hidden;
        }
       

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (MainWindow.stat_bLoggedIn == true)
            {
                lblError.Content = "*A user is already Logged in, Log off first to continue";
                lblError.Visibility = Visibility.Visible;
                btnLogin.IsEnabled = false;
            }
            else
            {
                lblError.Visibility = Visibility.Hidden;
                btnLogin.IsEnabled = true;
            }
        }

        private void txtPassword_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(@"" + MainWindow.stat_OSKPath + "osk.exe");
                psi.ErrorDialog = true;
                psi.WindowStyle = ProcessWindowStyle.Maximized;              
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error "+ex.Message);
            }
        }

        private void txtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Process[] pa = Process.GetProcessesByName("osk");
                for (int i = 0; i < pa.Length; i++)
                {
                    pa[i].Kill();
                }
            }
            catch (InvalidOperationException ioe)
            {
                MessageBox.Show("Invalid Operation : " + ioe.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("General : " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }    

    }
}
