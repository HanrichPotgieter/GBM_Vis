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
    /// Interaction logic for DataLogger.xaml
    /// </summary>
    public partial class DataLogger : Page
    {
        ArrayList alTags = new ArrayList();
        Hashtable htMeasurements = new Hashtable();

        //------------------------------------------------------------------------------//
        //                                  Constructor                                 //
        //------------------------------------------------------------------------------//   
        public DataLogger()
        {
            InitializeComponent();
            LoadListBoxes();
            LoadMeasurements();
        }


        //------------------------------------------------------------------------------//
        //                            Functionality Methods                             //
        //------------------------------------------------------------------------------//   

        public void LoadListBoxes()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
                {
                    sqlConn.Open();
                    SqlCommand cmd = sqlConn.CreateCommand();
                    cmd.CommandText = "SELECT Tagname, TagDescription, RecTrend, RecOnChange, RecOnTick, MeasurementID FROM SmartTags WHERE GcProTag = 1 ORDER BY TagName";
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        lstBoxGcProTags.Items.Add(reader.GetString(0) + ".\t--> " + reader.GetString(1));
                        alTags.Add(new BuhlerTag(reader.GetString(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5)));
                    }
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load GcPro Tags --> " + ex.Message, "Error Loading Tags", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            try
            {
                using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
                {
                    sqlConn.Open();
                    SqlCommand cmd = sqlConn.CreateCommand();
                    cmd.CommandText = "SELECT Tagname, TagDescription, RecTrend, RecOnChange, RecOnTick, MeasurementID FROM SmartTags WHERE UserTag = 1 ORDER BY TagName";
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        lstBoxUserTags.Items.Add(reader.GetString(0) + ".\t--> " + reader.GetString(1));
                        alTags.Add(new BuhlerTag(reader.GetString(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5)));
                    }
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load UserTags --> " + ex.Message, "Error Loading Tags", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public void LoadMeasurements()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
                {
                    sqlConn.Open();
                    SqlCommand cmd = sqlConn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM Measurement";
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        comboBoxMeasurementGCP.Items.Add(reader.GetInt32(0) + ") " + reader.GetString(1));
                        comboBoxMeasurementUT.Items.Add(reader.GetInt32(0) + ") " + reader.GetString(1));
                        htMeasurements.Add(reader.GetInt32(0), reader.GetInt32(0) + ") " + reader.GetString(1));
                    }
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Measurements --> " + ex.Message, "Error Loading Tags", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            comboBoxMeasurementGCP.SelectedIndex = 0;
            comboBoxMeasurementUT.SelectedIndex = 0;
        }


        //------------------------------------------------------------------------------//
        //                               Event Handlers                                 //
        //------------------------------------------------------------------------------//   

        private void lstBoxGcProTags_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string selectedItem = lstBoxGcProTags.SelectedItem.ToString();
                int index = selectedItem.IndexOf('.');
                string tagname = selectedItem.Substring(0, index);

                foreach (BuhlerTag bt in alTags)
                {
                    if (tagname == bt.Tagname)
                    {
                        cbTrendGCP.IsChecked = bt.RecordTrend == 1 ? true : false;
                        cbChangeGCP.IsChecked = bt.RecordChange == 1 ? true : false;
                        cbTickGCP.IsChecked = bt.RecordOnTick == 1 ? true : false;

                        //if (bt.RecordOnTick == 1)
                        //{
                        comboBoxMeasurementGCP.SelectedItem = htMeasurements[bt.MeasurementID];
                        //comboBoxMeasurementGCP.IsEnabled = true;
                        //}
                        //else
                        //{
                        //    comboBoxMeasurementGCP.IsEnabled = false;
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select GCP Tag --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void lstBoxUserTags_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string selectedItem = lstBoxUserTags.SelectedItem.ToString();
                int index = selectedItem.IndexOf('.');
                string tagname = selectedItem.Substring(0, index);

                foreach (BuhlerTag bt in alTags)
                {
                    if (tagname == bt.Tagname)
                    {
                        cbTrendUT.IsChecked = bt.RecordTrend == 1 ? true : false;
                        cbChangeUT.IsChecked = bt.RecordChange == 1 ? true : false;
                        cbTickUT.IsChecked = bt.RecordOnTick == 1 ? true : false;

                        //if (bt.RecordOnTick == 1)
                        //{
                        comboBoxMeasurementUT.SelectedItem = htMeasurements[bt.MeasurementID];
                        //comboBoxMeasurementUT.IsEnabled = true;
                        //}
                        //else
                        //{
                        //    comboBoxMeasurementUT.IsEnabled = false;
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select User Tag --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdateGCP_Click(object sender, RoutedEventArgs e)
        {
            if (lstBoxGcProTags.SelectedIndex >= 0)
            {
                string selectedMeasurement = comboBoxMeasurementGCP.SelectedItem.ToString();
                int MeasurementID = Int32.Parse(selectedMeasurement.Substring(0, selectedMeasurement.IndexOf(')')));

                string selectedItem = lstBoxGcProTags.SelectedItem.ToString();
                int index = selectedItem.IndexOf('.');
                string tagname = selectedItem.Substring(0, index);

                int iTrend = cbTrendGCP.IsChecked == true ? 1 : 0;
                int iChange = cbChangeGCP.IsChecked == true ? 1 : 0;
                int iTick = cbTickGCP.IsChecked == true ? 1 : 0;

                try
                {
                    using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
                    {
                        sqlConn.Open();
                        SqlCommand cmd = sqlConn.CreateCommand();
                        cmd.CommandText = "UPDATE SmartTags SET RecTrend = " + iTrend + ", RecOnChange = " + iChange + ", RecOnTick = " + iTick + ", MeasurementID = " + MeasurementID + " WHERE Tagname = '" + tagname + "'";
                        int i = cmd.ExecuteNonQuery();

                        if (i > 0)
                        {
                            MessageBox.Show("Update Successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                        sqlConn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Update GCP Tag --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnUpdateUT_Click(object sender, RoutedEventArgs e)
        {
            if (lstBoxUserTags.SelectedIndex >= 0)
            {
                string selectedMeasurement = comboBoxMeasurementUT.SelectedItem.ToString();
                int MeasurementID = Int32.Parse(selectedMeasurement.Substring(0, selectedMeasurement.IndexOf(')')));

                string selectedItem = lstBoxUserTags.SelectedItem.ToString();
                int index = selectedItem.IndexOf('.');
                string tagname = selectedItem.Substring(0, index);

                int iTrend = cbTrendUT.IsChecked == true ? 1 : 0;
                int iChange = cbChangeUT.IsChecked == true ? 1 : 0;
                int iTick = cbTickUT.IsChecked == true ? 1 : 0;

                try
                {
                    using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
                    {
                        sqlConn.Open();
                        SqlCommand cmd = sqlConn.CreateCommand();
                        cmd.CommandText = "UPDATE SmartTags SET RecTrend = " + iTrend + ", RecOnChange = " + iChange + ", RecOnTick = " + iTick + ", MeasurementID = " + MeasurementID + " WHERE Tagname = '" + tagname + "'";
                        int i = cmd.ExecuteNonQuery();

                        if (i > 0)
                        {
                            MessageBox.Show("Update Successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                        sqlConn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Update User Tag --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void cbTickGCP_Checked(object sender, RoutedEventArgs e)
        {
            //if (cbTickGCP.IsChecked == true)
            //{
            //    comboBoxMeasurementGCP.IsEnabled = true;
            //}
            //else
            //{
            //    comboBoxMeasurementGCP.IsEnabled = false;
            //}
        }

        private void cbTickUT_Checked(object sender, RoutedEventArgs e)
        {
            //if (cbTickUT.IsChecked == true)
            //{
            //    comboBoxMeasurementUT.IsEnabled = true;
            //}
            //else
            //{
            //    comboBoxMeasurementUT.IsEnabled = false;
            //}
        }
    }
}
