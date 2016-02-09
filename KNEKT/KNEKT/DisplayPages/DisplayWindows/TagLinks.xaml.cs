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
using System.Windows.Shapes;

using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections;
using WPFBuhlerControls;

namespace KNEKT.DisplayPages.DisplayWindows
{
    /// <summary>
    /// Interaction logic for TagLinks.xaml
    /// </summary>
    public partial class TagLinks : Window
    {
        private string _PageName;
        private string _ControlName;
        private string _ControlType;
        private string _SelectedTagLinkPropertyValue;   //Contains the most recent "Property Column" value when checking if a taglink exists [DoesTagLinkExist()}

        //IEnumerable iePage1;

        Hashtable htControls;

        string sqlConnectionString = "";//"Server=JHBY03; Database=KNEKT; User ID=sa; Password=SQLpassword1234;";

        //------------------------------------------------------------------------------------------------------->>>

        Grid _grid;


        //---------------------------------------------------------------------------------//
        //                                Constructors                                     //
        //---------------------------------------------------------------------------------//

        public TagLinks(string pageName, Grid grid, string SQLConnectionString)
        {
            InitializeComponent();

            _grid = grid;
            _PageName = pageName;
            sqlConnectionString = SQLConnectionString;
            htControls = new Hashtable();

            GetAllDatabaseTags();
            GetLogicalTreeOfGrid(grid);
        }


        /// <summary>
        /// Loops through the logical tree structure for the given Grid Control to populate the Control listbox
        /// </summary>
        public void GetLogicalTreeOfGrid(Grid obj)
        {
            if (obj is FrameworkElement)
            {
                FrameworkElement feGridAsParent = (FrameworkElement)obj;

                IEnumerable children = LogicalTreeHelper.GetChildren(feGridAsParent);   //Gets all Children of the Grid
                SortedList slItems = new SortedList();

                foreach (object child in children)
                {
                    if (child is FrameworkElement)
                    {
                        FrameworkElement Control = (FrameworkElement)child;

                        if (Control.GetType().Namespace.ToString() == "WPFBuhlerControls")  //Only Get controls from this namespace
                            if (!(Control.ToString().Contains("Line_") ||
                                //  Control.ToString().Contains("Label_") ||
                                  Control.ToString().Contains("Label_1") ||
                                  Control.ToString().Contains("Label_C") ||
                                  Control.ToString().Contains("Label_Screening") ||
                                  Control.ToString().Contains("Label_T1") ||
                                  //Control.ToString().Contains("Bin") ||
                   
                                  Control.ToString().Contains("Magnet_"))

                                  && Control.Name != "" && Control.Name != "Hopper_BagIntake" ) //Only get controls that do NOT contain these words in their name                                
                            {
                                ListBoxItem lbi = new ListBoxItem();
                                lbi.Content = "(" + Control.GetType().Name + ")" + Control.Name;
                                lbi.Tag = Control.Name;
                                slItems.Add(lbi.Tag, lbi);
                                //lstBoxElements.Items.Add(lbi);
                                htControls.Add(Control.Name, Control);
                            }
                    }
                    //PrintLogicalTree(child);  //Call here for recursion
                }

                foreach (object key in slItems.Keys)
                {
                    lstBoxElements.Items.Add(slItems[key]);
                }
                    //PrintLogicalTree(child);  //Call here for recursion
               
            }
        }



        //---------------------------------------------------------------------------------//
        //                                 Database Methods                                //
        //---------------------------------------------------------------------------------//

        public void GetAllDatabaseTags()
        {
            using (SqlConnection conn = new SqlConnection(sqlConnectionString))
            {
                try
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT st.Tagname, ISNULL(tl.Property,'-') Property FROM SmartTags st LEFT OUTER JOIN Taglinks tl on st.Tagname = tl.Tagname WHERE usertag = 0 ORDER BY st.Tagname";
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListBoxItem lbi = new ListBoxItem();
                        lbi.Content = reader.GetString(0);
                        lbi.Foreground = reader.GetString(1) == "-" ? Brushes.Black : Brushes.Green;
                        lbi.Background = reader.GetString(1) == "-" ? Brushes.White : Brushes.LightGreen;
                        lbi.Tag = reader.GetString(1) == "-" ? "" : reader.GetString(1);
                        lstBoxTags.Items.Add(lbi);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public bool DoesTagLinkExist(string tagname)
        {
            bool bExists = false;

            using (SqlConnection conn = new SqlConnection(sqlConnectionString))
            {
                try
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT Property, Tagname FROM Taglinks WHERE Tagname = '" + tagname + "'";
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        if (reader.GetString(0) != "" || reader.GetString(0) != null)
                        {
                            _SelectedTagLinkPropertyValue = reader.GetString(0);
                            bExists = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            return bExists;
        }

        public int CreateNewTagLink(string pageName, string controlType, string controlName, string propertyName, string tagname)
        {
            int iRowsInserted = 0;
            string currentInsert = "";
            using (SqlConnection conn = new SqlConnection(sqlConnectionString))
            {
                try
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO TagLinks VALUES('" + pageName.Trim() + "." + controlType.Trim() + controlName.Trim() + "." + propertyName.Trim() +  "','" + tagname.Trim() + "')";
                    conn.Open();
                    iRowsInserted = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n \n" + currentInsert);
                }
                finally
                {
                    conn.Close();
                }
            }

            return iRowsInserted;
        }

        public int UpdateSingleTagLink(string pageName, string controlType, string controlName, string propertyName, string tagname)    //Tag is linked to single control
        {
            int iRowsUpdated = 0;

            using (SqlConnection conn = new SqlConnection(sqlConnectionString))
            {
                try
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE TagLinks SET Property = '" + pageName + "." + controlType + controlName + "." + propertyName + "' WHERE Tagname = '" + tagname + "'";
                    conn.Open();
                    iRowsUpdated = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            return iRowsUpdated;
        }

        public int UpdateMultipleTagLink(string pageName, string controlType, string controlName, string propertyName, string tagname)  //Tag is linked to more than one control
        {
            int iRowsUpdated = 0;

            string sNewValue = pageName + "." + controlType + controlName + "." + propertyName;
            string sNewUpdate = "";
            bool bContinue = true;

            //page1.(Fan_HP)HPFan1.MotorColor;page2.(Fan_HP)HPFan1.MotorColor  //EXAMPLE
            //page1.(Fan_HP)HPFan1.MotorColor;
            //page2.(Fan_HP)HPFan1.MotorColor
            string[] sArrLinks = _SelectedTagLinkPropertyValue.Split(';');

            //Do Some Validation Checks
            for (int a = 0; a < sArrLinks.Length; a++)
            {
                if (sArrLinks[a] == sNewValue)
                {
                    //Dont update anything. The user clicked Yes instead of NO in the message box
                    bContinue = false;
                }
            }

            if (bContinue)
            {

                if (sArrLinks[sArrLinks.Length - 1] == ";")  //Is there a semicolon ";" at the end of the current database value
                {
                    sNewUpdate = _SelectedTagLinkPropertyValue + sNewValue;
                }
                else
                {
                    sNewUpdate = _SelectedTagLinkPropertyValue + ";" + sNewValue;
                }


                using (SqlConnection conn = new SqlConnection(sqlConnectionString))
                {
                    try
                    {
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "UPDATE TagLinks SET Property = '" + sNewUpdate + "' WHERE TagName = '" + tagname + "'";
                        conn.Open();
                        iRowsUpdated = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return iRowsUpdated;
        }

        //Returns -100 if the row was completely deleted, -200 if the row was updated
        public int RemovePropertyLink(string tagname, string pageName, string controlName, string property, ListBoxItem lbi)
        {
            string sDBPropertyValue = "";
            int iReturnCode = 0;

            //Check if the TagName is linked to multiple properties. If it is, UPDATE, else DELETE
            using (SqlConnection conn = new SqlConnection(sqlConnectionString))
            {
                try
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT Property FROM TagLinks WHERE TagName = '" + tagname + "'";
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        sDBPropertyValue = reader.GetString(0);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("RemovePropertyLink[1] --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    conn.Close();
                }
            }

            string[] sArr = sDBPropertyValue.Split(';');

            if (sArr.Length == 1 || (sArr.Length > 1 && sArr[1] == ""))
            {
                //Delete
                using (SqlConnection conn = new SqlConnection(sqlConnectionString))
                {
                    try
                    {
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "DELETE TagLinks WHERE Tagname = '" + tagname + "' AND SUBSTRING(Property,0,case when CHARINDEX('.',Property) = 0 then 2 else CHARINDEX('.',Property) end) = '" + pageName + "'";
                        conn.Open();
                        int iRowsDeleted = cmd.ExecuteNonQuery();

                        if (iRowsDeleted > 0)
                        {
                            iReturnCode = -100;
                            lbi.Tag = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("RemovePropertyLink[2] --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            else if (sArr.Length > 1)
            {
                //Update              
                string sUpdateString = "";

                for (int a = 0; a < sArr.Length; a++)
                {
                    //Check if current link is the one to remove
                    if (sArr[a] == pageName + "." + controlName + "." + property)
                    {
                        //This is the one to remove from the taglink so ignore the record
                    }
                    else
                    {
                        sUpdateString += sArr[a];
                    }
                }

                //Update the TagLinks Table with the new value
                using (SqlConnection conn = new SqlConnection(sqlConnectionString))
                {
                    try
                    {
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "UPDATE TagLinks SET Property = '" + sUpdateString + "' WHERE Tagname = '" + tagname + "' AND SUBSTRING(Property,0,case when CHARINDEX('.',Property) = 0 then 2 else CHARINDEX('.',Property) end) = '" + pageName + "'";
                        conn.Open();
                        int iRowsUpdated = cmd.ExecuteNonQuery();

                        if (iRowsUpdated > 0)
                        {
                            iReturnCode = -200;
                            lbi.Tag = sUpdateString;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("RemovePropertyLink[3] --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return iReturnCode;
        }

        /// <summary>
        /// Navigates to the first property in the linked list
        /// </summary>
        public void NavigateToProperty(string TagObject)
        {
            string sPageName = TagObject.Substring(0, TagObject.IndexOf('.'));
            string[] sArr = TagObject.Split(';'); //Only use the First Item in the array to jump to the linked property
            string sControl = TagObject.Substring(sArr[0].IndexOf('.') + 1, sArr[0].LastIndexOf('.') - sArr[0].IndexOf('.') - 1);
            sControl = sControl.Remove(0, sControl.IndexOf(')') + 1);
            string sProperty = TagObject.Substring(sArr[0].LastIndexOf('.') + 1, sArr[0].Length - sArr[0].LastIndexOf('.') - 1);

            int iIndexCountControl = 0;
            int iIndexCountProperty = 0;

            foreach (ListBoxItem lbi in lstBoxElements.Items)
            {
                if (lbi.Tag.ToString() == sControl)
                {
                    lstBoxElements.SelectedIndex = iIndexCountControl;

                    foreach (ListBoxItem lbi2 in lstBoxProperties.Items)
                    {
                        if (lbi2.Content.ToString() == sProperty)
                        {
                            lstBoxProperties.SelectedIndex = iIndexCountProperty;
                            //btnRemovePropertyLink.IsEnabled = true;
                            break;
                        }
                        else
                        {
                            //btnRemovePropertyLink.IsEnabled = false;
                        }

                        iIndexCountProperty++;
                    }

                    break;
                }
                iIndexCountControl++;
            }
        }




        //---------------------------------------------------------------------------------//
        //                                ListBox Events                                   //
        //---------------------------------------------------------------------------------//


        private void lstBoxElements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sItem = lstBoxElements.SelectedItem.ToString();
            string sKey = sItem.Substring(sItem.IndexOf(')') + 1, sItem.Length - sItem.IndexOf(')') - 1);
            string sType = sItem.Substring(sItem.IndexOf('('), sItem.IndexOf(')') - sItem.IndexOf('(') + 1);

            UserControl uc = (UserControl)htControls[sKey]; //Get the Control [Value in htControls] with the key of the control name
            _ControlName = sKey;
            _ControlType = sType;

            lstBoxProperties.Items.Clear();

            //Get all Color Properties of the Buhler Control
            foreach (var property in uc.GetType().GetProperties())
            {
                foreach (MethodInfo method in property.GetAccessors())
                {
                    string sShortName = method.Name.Substring(4, method.Name.Length - 4);
                    if (method.IsSpecialName && method.Name.StartsWith("set_") && !sShortName.ToLower().Contains("set") && (method.Name.Contains("Color") || method.Name.Contains("State")))
                    {
                        ListBoxItem lbi = new ListBoxItem();
                        lbi.Content = property.Name.ToString();
                        lstBoxProperties.Items.Add(lbi);
                    }
                }
            }
        }

        private void lstBoxTags_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstBoxTags.Items.Count > 0 && lstBoxTags.SelectedIndex >= 0)
            {
                string sTagObject = ((ListBoxItem)lstBoxTags.SelectedItem).Tag.ToString();  //Check the "Tag" Object property of the ListBox Item (Value is set in btnLink_Click)

                if (sTagObject != "")                                                       //If there is a value assigned to the tag, Navigate the left listBoxes to that property that is linked
                {
                    NavigateToProperty(sTagObject);
                    btnRemovePropertyLink.IsEnabled = true;
                }
                else
                {
                    btnRemovePropertyLink.IsEnabled = false;
                }
            }
            else
            {
                btnRemovePropertyLink.IsEnabled = false;
            }
        }


        //---------------------------------------------------------------------------------//
        //                                Button Clicks                                    //
        //---------------------------------------------------------------------------------//

        private void btnLink_Click(object sender, RoutedEventArgs e)
        {
            if (lstBoxElements.SelectedIndex >= 0 && lstBoxProperties.SelectedIndex >= 0 && lstBoxTags.SelectedIndex >= 0)
            {
                MessageBoxResult res = MessageBox.Show("Create a link between these two objects? \n\n" + ((ListBoxItem)lstBoxProperties.SelectedItem).Content.ToString().ToUpper() + "     -->     " + ((ListBoxItem)lstBoxTags.SelectedItem).Content.ToString().ToUpper() + "\n", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (res == MessageBoxResult.Yes)
                {
                    string sPageName = _PageName;
                    string sControlName = _ControlName;
                    string sPropertyName = ((ListBoxItem)lstBoxProperties.SelectedItem).Content.ToString();
                    string sTagname = ((ListBoxItem)lstBoxTags.SelectedItem).Content.ToString();
                    string sControlType = _ControlType;

                    if (DoesTagLinkExist(sTagname))
                    {
                        //
                        // if the tag is already linked, is this a multilinked tag or should the single tag be updated
                        //
                        MessageBoxResult result = MessageBox.Show("Is this tag used to update more that one element over multiple display pages?", "Confirmation required", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.No)
                        {
                            //Tag is Linked to single Control so update it
                            if (UpdateSingleTagLink(sPageName, sControlType, sControlName, sPropertyName, sTagname) > 0)
                            {
                                ((ListBoxItem)lstBoxTags.SelectedItem).Foreground = Brushes.Green;
                                ((ListBoxItem)lstBoxTags.SelectedItem).Background = Brushes.LightGreen;
                                ((ListBoxItem)lstBoxTags.SelectedItem).Tag = "" + sPageName + "." + sControlName + "." + sPropertyName;
                            }
                        }
                        else
                        {
                            //Tag is Linked to Multiple Controls
                            UpdateMultipleTagLink(sPageName, sControlType, sControlName, sPropertyName, sTagname);
                        }
                    }
                    else
                    {
                        if (CreateNewTagLink(sPageName, sControlType, sControlName, sPropertyName, sTagname) > 0)
                        {
                            ((ListBoxItem)lstBoxTags.SelectedItem).Foreground = Brushes.Green;
                            ((ListBoxItem)lstBoxTags.SelectedItem).Background = Brushes.LightGreen;
                            ((ListBoxItem)lstBoxTags.SelectedItem).Tag = "" + sPageName + "." + sControlName + "." + sPropertyName;
                        }
                    }
                }

            }
        }

        private void btnRemovePropertyLink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstBoxTags.Items.Count > 0 && lstBoxTags.SelectedIndex >= 0)
                {
                    string sTagname = ((ListBoxItem)lstBoxTags.SelectedItem).Content.ToString();
                    string sControl = ((ListBoxItem)lstBoxElements.SelectedItem).Content.ToString();
                    string sProperty = ((ListBoxItem)lstBoxProperties.SelectedItem).Content.ToString();
                    int iSelectedIndex = lstBoxTags.SelectedIndex;

                    int iRemoveLinkCode = RemovePropertyLink(sTagname, _PageName, sControl, sProperty, (ListBoxItem)lstBoxTags.Items[iSelectedIndex]); //-100 = Row was completely deleted , -200 = Row was updated

                    if (iRemoveLinkCode == -100)
                    {
                        ((ListBoxItem)lstBoxTags.Items[iSelectedIndex]).Foreground = Brushes.Black;
                        ((ListBoxItem)lstBoxTags.Items[iSelectedIndex]).Background = Brushes.White;
                        //((ListBoxItem)lstBoxTags.Items[iSelectedIndex]).Tag = "";
                    }
                    //else if (iRemoveLinkCode == -200)
                    //{
                    //    //((ListBoxItem)lstBoxTags.Items[iSelectedIndex]).Tag = "";
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("RemovePropertyLink[2] --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
