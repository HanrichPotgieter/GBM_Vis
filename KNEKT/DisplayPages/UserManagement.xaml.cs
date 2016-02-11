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
using System.Data.SqlClient;
using System.Collections;
using KNEKT.Classes;
using System.Diagnostics;

namespace KNEKT.DisplayPages
{
    /// <summary>
    /// Interaction logic for UserManagement.xaml
    /// </summary>
    public partial class UserManagement : Page
    {
        MainWindow mw;
        ArrayList alUsers = new ArrayList();

        public UserManagement(MainWindow mainWindow)
        {
            InitializeComponent();

            mw = mainWindow;
            GetAccessLevels();
            GetUsers();
        }


        //------------------------------------------------------------------------------//
        //                                  Database Methods                            //
        //------------------------------------------------------------------------------//        

        public void GetAccessLevels()
        {
            int currentUserLevel = MainWindow.stat_iUserLevel;            
                      
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
                {
                    sqlConn.Open();
                    SqlCommand cmd = sqlConn.CreateCommand();
                    cmd.CommandText = "SELECT AccessLevel, LevelName FROM UserAccessLevels WHERE AccessLevel <= "+currentUserLevel+" ORDER BY AccessLevel";
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cmbAdd_Levels.Items.Add(reader.GetInt32(0) + ") " + reader.GetString(1));
                        cmbUpd_UserLevel.Items.Add(reader.GetInt32(0) + ") " + reader.GetString(1));
                    }
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Get Access Levels -->" + ex.Message);
            }
        }

        public void GetUsers()
        {
            lstBoxUpd_Users.Items.Clear();
            alUsers.Clear();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
                {
                    sqlConn.Open();
                    SqlCommand cmd = sqlConn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM Users WHERE AccessLevel < 10";
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        alUsers.Add(new User(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4)));
                        lstBoxUpd_Users.Items.Add(reader.GetString(0));
                    }
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Get Users -->" + ex.Message);
            }
        }


        public void InsertNewUser()
        {
            try
            {
                string sItem = cmbAdd_Levels.SelectedItem.ToString();
                string sAccessLevel = sItem.Substring(0,sItem.IndexOf(')'));

                string UserName = txtAdd_UserName.Text;
                string FirstName = txtAdd_FirstName.Text;
                string LastName = txtAdd_LastName.Text;
                string Password = txtAdd_Pass.Password;

                bool bUsernameExists = false;

                using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
                {
                    sqlConn.Open();
                    SqlCommand cmd = sqlConn.CreateCommand();
                    cmd.CommandText = "SELECT Username FROM Users WHERE Username = '"+UserName+"'";
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        bUsernameExists = true;
                    }
                    sqlConn.Close();
                }

                if (!bUsernameExists)
                {
                    using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
                    {
                        sqlConn.Open();
                        SqlCommand cmd = sqlConn.CreateCommand();
                        cmd.CommandText = "INSERT INTO Users VALUES ('" + UserName + "','" + FirstName + "','" + LastName + "','" + Password + "'," + Int32.Parse(sAccessLevel) + ")";
                        int rowsInserted = cmd.ExecuteNonQuery();

                        if (rowsInserted > 0)
                        {
                            MessageBox.Show("User Added Successfully", "User Added", MessageBoxButton.OK, MessageBoxImage.Information);
                            
                            txtAdd_UserName.Text = "";
                            txtAdd_FirstName.Text = "";
                            txtAdd_LastName.Text = "";
                            txtAdd_Pass.Password = "";
                            txtAdd_ConfirmPass.Password = "";
                        }

                        sqlConn.Close();
                    }
                }
                else
                {
                    txtAdd_UserName.Focus();
                    MessageBox.Show("Username already exists", "Username Conflict", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Insert User -->" + ex.Message);
            }
        }

        public void UpdateUser()
        {
            try
            {
                string sItem = cmbUpd_UserLevel.SelectedItem.ToString();
                string sAccessLevel = sItem.Substring(0, sItem.IndexOf(')'));


                string FirstName = txtUpd_FirstName.Text;
                string LastName = txtUpd_LastName.Text;
                string Password = txtUpd_Pass.Password;

                    using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
                    {
                        sqlConn.Open();
                        SqlCommand cmd = sqlConn.CreateCommand();
                        cmd.CommandText = "UPDATE Users SET FirstName = '"+FirstName+"', LastName = '"+LastName +"', UserPassword = '"+Password+"', AccessLevel = "+Int32.Parse(sAccessLevel)+" WHERE Username = '"+txtUpd_Username.Text+"'";
                        int rowsInserted = cmd.ExecuteNonQuery();

                        if (rowsInserted > 0)
                        {
                            MessageBox.Show("User Updated Successfully", "User Updated", MessageBoxButton.OK, MessageBoxImage.Information);
                           
                            txtUpd_FirstName.Text = "";
                            txtUpd_LastName.Text = "";
                            txtUpd_Pass.Password = "";
                            txtUpd_ConfirmPass.Password = "";
                            GetUsers();
                        }

                        sqlConn.Close();
                    }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Update User -->" + ex.Message);
            }
        }


        //------------------------------------------------------------------------------//
        //                                  Button Clicks                               //
        //------------------------------------------------------------------------------//        


        //
        // Add User
        //
        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            bool bError = false;

            if (txtAdd_UserName.Text == "")
            {
                bError = true;
                lblErrorAddUser.Content = "* UserName is required ";
            }
            else if (txtAdd_FirstName.Text == "")
            {
                bError = true;
                lblErrorAddUser.Content = "* First Name is required ";
            }
            else if (txtAdd_LastName.Text == "")
            {
                bError = true;
                lblErrorAddUser.Content = "* Last Name is required ";
            }
            else if (txtAdd_Pass.Password == "")
            {
                bError = true;
                lblErrorAddUser.Content = "* Password is required ";
            }
            else if (txtAdd_ConfirmPass.Password == "")
            {
                bError = true;
                lblErrorAddUser.Content = "* Confirmation of Password is required ";
            }
            else if (txtAdd_Pass.Password != txtAdd_ConfirmPass.Password)
            {
                bError = true;
                lblErrorAddUser.Content = "* Passwords do not match ";
            }
            else if (cmbAdd_Levels.SelectedIndex < 0)
            {
                bError = true;
                lblErrorAddUser.Content = "* Select a User Level ";
            }            
            else
            {
                mw.UIInteraction_Change(this.btnAddUser, e, "AddedUser ("+txtAdd_UserName.Text+")");
                InsertNewUser();
                GetUsers();
            }

            if (bError)
            {
                lblErrorAddUser.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lblErrorAddUser.Visibility = System.Windows.Visibility.Hidden;
            }
        }


        //
        // Update User
        //
        private void btnUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            if (lstBoxUpd_Users.SelectedIndex >= 0)
            {
                bool bError = false;

                if (txtUpd_FirstName.Text == "")
                {
                    bError = true;
                    lblErrorUpdUser.Content = "* First Name is required ";
                }
                else if (txtUpd_LastName.Text == "")
                {
                    bError = true;
                    lblErrorUpdUser.Content = "* Last Name is required ";
                }
                else if (txtUpd_Pass.Password == "")
                {
                    bError = true;
                    lblErrorUpdUser.Content = "* Password is required ";
                }
                else if (txtUpd_ConfirmPass.Password == "")
                {
                    bError = true;
                    lblErrorUpdUser.Content = "* Confirmation of Password is required ";
                }
                else if (txtUpd_Pass.Password != txtUpd_ConfirmPass.Password)
                {
                    bError = true;
                    lblErrorUpdUser.Content = "* Passwords do not match ";
                }
                else if (cmbUpd_UserLevel.SelectedIndex < 0)
                {
                    bError = true;
                    lblErrorUpdUser.Content = "* Select a User Level ";
                }
                else
                {
                    mw.UIInteraction_Change(this.btnAddUser, e, "UpdateUser (" + txtUpd_Username.Text + ")");
                    UpdateUser();
                }

                if (bError)
                {
                    lblErrorUpdUser.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    lblErrorUpdUser.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }

        private void lstBoxUpd_Users_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {                
                int index = lstBoxUpd_Users.SelectedIndex;
             
                if(index >= 0)
                {
                    User u = (User)alUsers[index];

                    txtUpd_Username.Text = u.Username;
                    txtUpd_FirstName.Text = u.FirstName;
                    txtUpd_LastName.Text = u.LastName;
                    txtUpd_Pass.Password = u.Password;
                    txtUpd_ConfirmPass.Password = u.Password;

                    int iSelectedIndex = 0;

                    foreach (string s in cmbUpd_UserLevel.Items)
                    {
                        string sItem = s;
                        string sAccessLevel = sItem.Substring(0, sItem.IndexOf(')'));
                        int iAccessLevel = Int32.Parse(sAccessLevel);

                        if (iAccessLevel == u.UserLevel)
                        {
                            cmbUpd_UserLevel.SelectedIndex = iSelectedIndex;
                            break;
                        }

                        iSelectedIndex++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Selection Change -->" + ex.Message);
            }
        }

        private void txtAdd_Pass_TouchDown(object sender, TouchEventArgs e)
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
                MessageBox.Show("Error " + ex.Message);
            }
        }

        private void txtAdd_ConfirmPass_TouchDown(object sender, TouchEventArgs e)
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
                MessageBox.Show("Error " + ex.Message);
            }
        }

        private void txtAdd_Pass_LostFocus(object sender, RoutedEventArgs e)
        {
            Process[] pa = Process.GetProcessesByName("osk");
            for (int i = 0; i < pa.Length; i++)
            {
                pa[i].Kill();
            }
        }

        private void txtAdd_ConfirmPass_LostFocus(object sender, RoutedEventArgs e)
        {
            Process[] pa = Process.GetProcessesByName("osk");
            for (int i = 0; i < pa.Length; i++)
            {
                pa[i].Kill();
            }
        }

        private void txtUpd_Pass_TouchDown(object sender, TouchEventArgs e)
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
                MessageBox.Show("Error " + ex.Message);
            }
        }

        private void txtUpd_ConfirmPass_TouchDown(object sender, TouchEventArgs e)
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
                MessageBox.Show("Error " + ex.Message);
            }
        }

        private void txtUpd_Pass_LostFocus(object sender, RoutedEventArgs e)
        {
            Process[] pa = Process.GetProcessesByName("osk");
            for (int i = 0; i < pa.Length; i++)
            {
                pa[i].Kill();
            }
        }

        private void txtUpd_ConfirmPass_LostFocus(object sender, RoutedEventArgs e)
        {
            Process[] pa = Process.GetProcessesByName("osk");
            for (int i = 0; i < pa.Length; i++)
            {
                pa[i].Kill();
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.stat_iLogOffCounter = 0;
        }

        private void Grid_TouchDown(object sender, TouchEventArgs e)
        {
            MainWindow.stat_iLogOffCounter = 0;
        }        
    }
}
