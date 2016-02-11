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
using System.Collections;

namespace KNEKT._PagesSettings
{
    /// <summary>
    /// Interaction logic for AdditionalTags.xaml
    /// </summary>
    public partial class AdditionalTags : Page
    {
        ArrayList alTags = new ArrayList();

        public AdditionalTags()
        {
            InitializeComponent();

            LoadUserCreatedTags();
        }

        public void LoadUserCreatedTags()
        {
            //
            //  Select all tags from SQL
            //
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
                {
                    sqlConn.Open();
                    SqlCommand cmd = sqlConn.CreateCommand();
                    cmd.CommandText = "SELECT ObjectNo, Tagname, TagDescription, DBOffset FROM SmartTags WHERE UserTag = 1 ORDER BY Tagname";
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        listBox1.Items.Add("[" + reader.GetString(3) + "] " + reader.GetString(1) + "   (" + reader.GetString(2) + ")");
                    }
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadAdditionalTags --> " + ex.Message, "Load Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (txtDBOffset.Text != "" & txttagDesc.Text != "" & txtTagname.Text != "")
            {

                try
                {
                    string sAddToAddress = "";
                    if (cbReal.IsChecked == true)
                    {
                        sAddToAddress = "{R}";
                    }
                    else if (cbInt.IsChecked == true)
                    {
                        sAddToAddress = "{I}";
                    }
                    else if (cbDINT.IsChecked == true)
                    {
                        sAddToAddress = "{D}";
                    }

                    using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
                    {
                        string sMaxID = "";

                        //
                        //Get the max object ID
                        //
                        sqlConn.Open();
                        SqlCommand cmd = sqlConn.CreateCommand();
                        cmd.CommandText = "SELECT ISNULL(MAX(objectNo),'900000000') FROM SmartTags WHERE ObjectNo >= '900000000'";
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            sMaxID = reader.GetString(0);
                        }
                        sqlConn.Close();

                        //Plus one to the max object id
                        decimal d1 = Convert.ToDecimal(sMaxID);
                        d1++;
                        sMaxID = "" + d1;

                        //
                        //Insert the new additional element tag
                        //
                        sqlConn.Open();
                        SqlCommand cmd1 = sqlConn.CreateCommand();
                        cmd1.CommandText = "INSERT INTO SmartTags VALUES ('" + sMaxID + "','" + txtTagname.Text + "','" + txttagDesc.Text + "','','" + txtDBOffset.Text + sAddToAddress + "','','','0','1','0','0','0','0')";
                        int rowsInserted = cmd1.ExecuteNonQuery();
                        if (rowsInserted > 0)
                        {
                            listBox1.Items.Add(("[" + txtDBOffset.Text + "] " + txtTagname.Text + "   (" + txttagDesc.Text + ")"));

                            txtDBOffset.Text = "";
                            txttagDesc.Text = "";
                            txtTagname.Text = "";

                            txtTagname.Focus();

                            cbReal.IsChecked = false;
                        }

                        sqlConn.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Insert Failed --> " + ex.Message, "Insert Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex >= 0)
                {

                    txtTagname.IsReadOnly = true;

                    string text = listBox1.SelectedItem.ToString();

                    int iStart = 1;
                    int iPosOfLastSquareBracket = text.IndexOf(']');
                    int iPosOfFirstRoundBracket = text.IndexOf('(');
                    int iPosOfLastRoundBracket = text.IndexOf(')');

                    string sAddress = text.Substring(iStart, (iPosOfLastSquareBracket - 1));
                    string sTagname = text.Substring((iPosOfLastSquareBracket + 1), (iPosOfFirstRoundBracket - (iPosOfLastSquareBracket + 1)));
                    string sDescrip = text.Substring((iPosOfFirstRoundBracket + 1), ((iPosOfLastRoundBracket) - iPosOfFirstRoundBracket - 1));

                    //
                    //Is the tag of data type REAL
                    //
                    if (sAddress.Contains("{R}"))
                    {
                        sAddress = sAddress.Remove(sAddress.IndexOf('{', 3));
                        cbReal.IsChecked = true;
                    }
                    else if (sAddress.Contains("{I}"))
                    {
                        sAddress = sAddress.Remove(sAddress.IndexOf('{', 3));
                        cbInt.IsChecked = true;
                    }
                    else if (sAddress.Contains("{D}"))
                    {
                        sAddress = sAddress.Remove(sAddress.IndexOf('{', 3));
                        cbDINT.IsChecked = true;
                    }
                    else
                    {
                        cbNormal.IsChecked = true;
                    }

                    //MessageBox.Show(sAddress + "\n|" + sTagname + "\n" + sDescrip);

                    txtDBOffset.Text = sAddress.Trim();
                    txttagDesc.Text = sDescrip.Trim();
                    txtTagname.Text = sTagname.Trim();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("" + exc.Message);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                string sAddToAddress = "";
                if (cbReal.IsChecked == true)
                {
                    sAddToAddress = "{R}";
                }
                else if (cbInt.IsChecked == true)
                {
                    sAddToAddress = "{I}";
                }
                else if (cbDINT.IsChecked == true)
                {
                    sAddToAddress = "{D}";
                }

                try
                {
                    using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
                    {
                        sqlConn.Open();
                        SqlCommand cmd = sqlConn.CreateCommand();
                        cmd.CommandText = "UPDATE SmartTags SET DBOffset = '" + txtDBOffset.Text + sAddToAddress + "', TagDescription = '" + txttagDesc.Text + "' WHERE UserTag = '1' AND Tagname = '" + txtTagname.Text + "'";
                        int rowsUpdated = cmd.ExecuteNonQuery();

                        if (rowsUpdated > 0)
                        {
                            MessageBox.Show("Smart Tag updated", "Update Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                        sqlConn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("UpdateFailed --> " + ex.Message, "Update Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Clear Button
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            listBox1.SelectedIndex = -1;

            txtDBOffset.Text = "";
            txtTagname.Text = "";
            txttagDesc.Text = "";

            txtTagname.IsReadOnly = false;
        }


        private void txttagDesc_LostFocus(object sender, RoutedEventArgs e)
        {
            txttagDesc.Text = txttagDesc.Text.ToUpper();
        }

        private void txtDBOffset_LostFocus(object sender, RoutedEventArgs e)
        {
            txtDBOffset.Text = txtDBOffset.Text.ToUpper();
        }

        private void txtTagname_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                string temp = txtTagname.Text;
                char c = temp[0];
                if (c != '_')
                {
                    txtTagname.Text = "_" + temp;
                }
            }
            catch { }
        }

        private void btnDeleteTag_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                
                var mbr = MessageBox.Show("Are you sure you want to delete Tag:  " + txtTagname.Text + "?", "Confirmation Required", MessageBoxButton.YesNo, MessageBoxImage.Question);
                string result = mbr.ToString();
                if (result == "Yes")
                {

                    using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
                    {
                        sqlConn.Open();
                        SqlCommand cmd = sqlConn.CreateCommand();
                        cmd.CommandText = "DELETE SmartTags WHERE UserTag = '1' AND Tagname = '" + txtTagname.Text + "'";
                        int rowsUpdated = cmd.ExecuteNonQuery();

                        if (rowsUpdated > 0)
                        {
                            listBox1.Items.Remove(("[" + txtDBOffset.Text + "] " + txtTagname.Text + "   (" + txttagDesc.Text + ")"));
                            listBox1.SelectedIndex = -1;

                            txtDBOffset.Text = "";
                            txtTagname.Text = "";
                            txttagDesc.Text = "";

                            txtTagname.IsReadOnly = false;
                            MessageBox.Show("Smart Tag deleted successfuly", "Delete Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                        sqlConn.Close();
                    }
                }
                else
                { 

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete Failed --> " + ex.Message, "Delete Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
