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
using S7Link;
using KNEKT.Classes;
using System.Collections;

namespace KNEKT.DisplayWindows
{
    /// <summary>
    /// Interaction logic for JobList.xaml
    /// </summary>
    public partial class JobList : Window
    {
        private int _MaximumSenders = 100;
        private int _MaximumReceivers = 100;
        private int _NumberOfUsedSenders = 0;
        private int _NumberOfUsedReceivers = 0;

        Controller PLC_R;
        Controller PLC_W;

        SortedList slSenders;
        SortedList slRecievers;
        SortedList slSenderDBs;
        SortedList slRecieverDBs;

        ArrayList alUsedSenders = new ArrayList();
        ArrayList alUsedRecievers = new ArrayList();


        public JobList(SortedList SenderBins, SortedList RecieverBins, SortedList SenderDBs, SortedList RecieverDBs, Controller ReadController, Controller WriteController)
        {
            InitializeComponent();

            slSenders = SenderBins;
            slRecievers = RecieverBins;
            slSenderDBs = SenderDBs;
            slRecieverDBs = RecieverDBs;

            PLC_R = ReadController;
            PLC_W = WriteController;

            ReadUsedSendersAndRecievers();
        }


        /// <summary>
        /// Load the currently selected Ingredients from
        /// the PLC and add them to the relevant lists
        /// </summary>
        public void ReadUsedSendersAndRecievers()
        {
            int index = 0;

            foreach (string s in slSenderDBs.Keys)
            {
                try
                {
                    string offset = s;
                    Tag t1 = new Tag(offset, S7Link.Tag.ATOMIC.WORD, 1);

                    if (PLC_R.IsConnected)
                    {
                        PLC_R.ReadTag(t1);
                        int value = Convert.ToInt32(t1.Value);

                        if (value != 0)                                 //If there is a number loaded in, remove from the Arraylist
                        {
                            lstBxUsedSenders.Items.Add(slSenders[value].ToString());
                            alUsedSenders.Add(value);
                            _NumberOfUsedSenders++;
                        }

                        index++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading used senders.\n\nOffset \t: " + s + "\n" + ex.Message, "Error Retreiving Data", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
            }

            //int count = 0;
            foreach (int s in slSenders.Keys)
            {
                if (!alUsedSenders.Contains(s))
                {
                    lstBxAvailableSenders.Items.Add(slSenders[s].ToString());
                }
            }






            index = 0;

            foreach (string s in slRecieverDBs.Keys)
            {
                try
                {
                    string offset = s;
                    Tag t1 = new Tag(offset, S7Link.Tag.ATOMIC.WORD, 1);

                    if (PLC_R.IsConnected)
                    {
                        PLC_R.ReadTag(t1);
                        int value = Convert.ToInt32(t1.Value);

                        if (value != 0)                                 //If there is a number loaded in, remove from the Arraylist
                        {
                            lstBxUsedRecievers.Items.Add(slRecievers[value].ToString()); //Add the value of the sorted list to the listbox   
                            alUsedRecievers.Add(value);
                            _NumberOfUsedReceivers++;
                        }

                        index++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading used recievers.\n\nOffset \t: " + s + "\n" + ex.Message, "Error Retreiving Data", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
            }


            //int count = 0;
            foreach (int s in slRecievers.Keys)
            {
                if (!alUsedRecievers.Contains(s))
                {
                    lstBxAvailableRecievers.Items.Add(slRecievers[s].ToString());
                }
            }
        }


        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//

        public int JobList_MaximumSenders
        {
            get
            {
                return _MaximumSenders;
            }
            set
            {
                _MaximumSenders = value;
            }
        }

        public int JobList_MaximumReceivers
        {
            get
            {
                return _MaximumReceivers;
            }
            set
            {
                _MaximumReceivers = value;
            }
        }


        //------------------------------------------------------------------------------//
        //                              Listbox Event Handlers                          //
        //------------------------------------------------------------------------------//        


        //
        //Available Senders
        //
        private void lstBxAvailableSenders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstBxAvailableSenders.SelectedItem != null)
            {
                if (lstBxAvailableSenders.SelectedItem.ToString().Length != 0)
                {
                    lstBxUsedSenders.Items.Add(lstBxAvailableSenders.SelectedItem.ToString());
                    lstBxAvailableSenders.Items.RemoveAt(lstBxAvailableSenders.SelectedIndex);
                    _NumberOfUsedSenders++;
                }
            }

            if (_NumberOfUsedSenders >= JobList_MaximumSenders)
            {
                btnAddSender.IsEnabled = false;
            }
            else
            {
                btnAddSender.IsEnabled = true;
            }
        }


        //
        //Used Senders
        //
        private void lstBxUsedSenders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstBxUsedSenders.SelectedItem != null)
            {
                if (lstBxUsedSenders.SelectedItem.ToString().Length != 0)
                {
                    lstBxAvailableSenders.Items.Add(lstBxUsedSenders.SelectedItem.ToString());
                    lstBxUsedSenders.Items.RemoveAt(lstBxUsedSenders.SelectedIndex);
                }
            }
        }


        //
        //Available Recievers
        //
        private void lstBxAvailableRecievers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstBxAvailableRecievers.SelectedItem != null)
            {
                if (lstBxAvailableRecievers.SelectedItem.ToString().Length != 0)
                {
                    lstBxUsedRecievers.Items.Add(lstBxAvailableRecievers.SelectedItem.ToString());
                    lstBxAvailableRecievers.Items.RemoveAt(lstBxAvailableRecievers.SelectedIndex);
                    _NumberOfUsedReceivers++;
                }
            }

            if (_NumberOfUsedReceivers >= JobList_MaximumReceivers)
            {
                btnAddReceiver.IsEnabled = false;
            }
            else
            {
                btnAddReceiver.IsEnabled = true;
            }
        }

        //
        //Used Recievers
        //
        private void lstBxUsedRecievers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstBxUsedRecievers.SelectedItem != null)
            {
                if (lstBxUsedRecievers.SelectedItem.ToString().Length != 0)
                {
                    lstBxAvailableRecievers.Items.Add(lstBxUsedRecievers.SelectedItem.ToString());
                    lstBxUsedRecievers.Items.RemoveAt(lstBxUsedRecievers.SelectedIndex);
                }
            }
        }


        private void lstBxAvailableRecievers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstBxAvailableRecievers.SelectedIndex != -1 && (_NumberOfUsedReceivers < JobList_MaximumReceivers))
            {
                btnAddReceiver.IsEnabled = true;
            }
            else
            {
                btnAddReceiver.IsEnabled = false;
            }
        }


        private void lstBxUsedRecievers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstBxUsedRecievers.SelectedIndex != -1)
            {
                btnRemoveReceiver.IsEnabled = true;
            }
            else
            {
                btnRemoveReceiver.IsEnabled = false;
            }
        }


        private void lstBxAvailableSenders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstBxAvailableSenders.SelectedIndex != -1 && (_NumberOfUsedSenders < JobList_MaximumSenders))
            {
                btnAddSender.IsEnabled = true;
            }
            else
            {
                btnAddSender.IsEnabled = false;
            }
        }


        private void lstBxUsedSenders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstBxUsedSenders.SelectedIndex != -1)
            {
                btnRemoveSender.IsEnabled = true;
            }
            else
            {
                btnRemoveSender.IsEnabled = false;
            }
        }


        //------------------------------------------------------------------------------//
        //                              Buttton Clicks                                  //
        //------------------------------------------------------------------------------//  

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Are you sure you want to Execute the new Job?", "Are you Sure?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbr == MessageBoxResult.Yes)
            {
                bool bDownloaded = false;
                int itemCount = 0;


                //
                //Senders
                //
                foreach (string s in lstBxUsedSenders.Items)
                {
                    int index = slSenders.IndexOfValue(s);
                    object binNum = slSenders.GetKey(index);


                    string tagToWrite = slSenderDBs.GetKey(itemCount).ToString();
                    itemCount++;


                    if (!PLC_W.IsConnected)
                        PLC_W.Connect();

                    try
                    {
                        if (PLC_W.IsConnected)
                        {
                            Tag t1 = new Tag(tagToWrite, S7Link.Tag.ATOMIC.WORD, 1);
                            t1.Value = binNum;
                            PLC_W.WriteTag(t1);
                            bDownloaded = true;
                        }
                        else
                        {
                            bDownloaded = false;
                            MessageBox.Show("Write Channel is Not Connected");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error writing new senders.\n\nIndex \t: " + index + "\nBin Number \t:" + binNum + "\n" + ex.Message, "Error Writing Data", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
                }


                //Write the remaining Values in the DB to Zero

                for (int index = itemCount; index < slSenderDBs.Count; index++)                   //Loop through all recievers in DB list
                {
                    string dbOffset = slSenderDBs.GetKey(index).ToString();
                    int valueToWrite = 0;

                    if (!PLC_W.IsConnected)
                        PLC_W.Connect();

                    if (PLC_W.IsConnected)
                    {
                        Tag t1 = new Tag(dbOffset, S7Link.Tag.ATOMIC.WORD, 1);
                        t1.Value = valueToWrite;
                        PLC_W.WriteTag(t1);
                        bDownloaded = true;
                    }
                    else
                    {
                        bDownloaded = false;
                        MessageBox.Show("Write Channel is Not Connected");
                    }
                }


                //
                //Receivers
                //

                itemCount = 0;

                foreach (string s in lstBxUsedRecievers.Items)
                {
                    int index = slRecievers.IndexOfValue(s);
                    object binNum = slRecievers.GetKey(index);


                    string tagToWrite = slRecieverDBs.GetKey(itemCount).ToString();
                    itemCount++;


                    if (!PLC_W.IsConnected)
                        PLC_W.Connect();
                    try
                    {
                        if (PLC_W.IsConnected)
                        {
                            Tag t1 = new Tag(tagToWrite, S7Link.Tag.ATOMIC.WORD, 1);
                            t1.Value = binNum;
                            PLC_W.WriteTag(t1);
                            bDownloaded = true;
                        }
                        else
                        {
                            bDownloaded = false;
                            MessageBox.Show("Write Channel is Not Connected");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error writing new senders.\n\nIndex \t: " + index + "\nBin Number \t:" + binNum + "\n" + ex.Message, "Error Writing Data", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
                }


                //Write the remaining Values in the DB to Zero

                for (int index = itemCount; index < slRecieverDBs.Count; index++)                   //Loop through all recievers in DB list
                {
                    string dbOffset = slRecieverDBs.GetKey(index).ToString();
                    int valueToWrite = 0;

                    if (!PLC_W.IsConnected)
                        PLC_W.Connect();

                    if (PLC_W.IsConnected)
                    {
                        Tag t1 = new Tag(dbOffset, S7Link.Tag.ATOMIC.WORD, 1);
                        t1.Value = valueToWrite;
                        PLC_W.WriteTag(t1);
                        bDownloaded = true;
                    }
                    else
                    {
                        bDownloaded = false;
                        MessageBox.Show("Write Channel is Not Connected");
                    }
                }


                if (bDownloaded)
                {
                    MessageBox.Show("Download Successful", "Download Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow.bCmdModify = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to download!", "Download Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void btnAddReceiver_Click(object sender, RoutedEventArgs e)
        {
            if (lstBxAvailableRecievers.Items.Count > 0)
            {
                if (lstBxAvailableRecievers.SelectedItem != null)
                {
                    string item = lstBxAvailableRecievers.SelectedItem.ToString();
                    lstBxUsedRecievers.Items.Add(item);
                    lstBxAvailableRecievers.Items.Remove(item);
                    _NumberOfUsedReceivers++;
                }
            }

            if (_NumberOfUsedReceivers >= JobList_MaximumReceivers)
            {
                btnAddReceiver.IsEnabled = false;
            }
            else
            {
                btnAddReceiver.IsEnabled = true;
            }
        }

        private void btnRemoveReceiver_Click(object sender, RoutedEventArgs e)
        {
            if (lstBxUsedRecievers.Items.Count > 0)
            {
                if (lstBxUsedRecievers.SelectedItem != null)
                {
                    string item = lstBxUsedRecievers.SelectedItem.ToString();
                    lstBxAvailableRecievers.Items.Add(item);
                    lstBxUsedRecievers.Items.Remove(item);
                    _NumberOfUsedReceivers--;
                }
            }
        }

        private void btnAddSender_Click(object sender, RoutedEventArgs e)
        {
            if (lstBxAvailableSenders.Items.Count > 0)
            {
                if (lstBxAvailableSenders.SelectedItem != null)
                {
                    string item = lstBxAvailableSenders.SelectedItem.ToString();
                    lstBxUsedSenders.Items.Add(item);
                    lstBxAvailableSenders.Items.Remove(item);
                    _NumberOfUsedSenders++;
                }
            }

            if (_NumberOfUsedSenders >= JobList_MaximumSenders)
            {
                btnAddSender.IsEnabled = false;
            }
            else
            {
                btnAddSender.IsEnabled = true;
            }
        }

        private void btnRemoveSender_Click(object sender, RoutedEventArgs e)
        {
            if (lstBxUsedSenders.Items.Count > 0)
            {
                if (lstBxUsedSenders.SelectedItem != null)
                {
                    string item = lstBxUsedSenders.SelectedItem.ToString();
                    lstBxAvailableSenders.Items.Add(item);
                    lstBxUsedSenders.Items.Remove(item);
                    _NumberOfUsedSenders--;
                }
            }
        }




    }
}
