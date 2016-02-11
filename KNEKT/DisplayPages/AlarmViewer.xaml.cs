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
using System.Collections;
using System.IO;

namespace KNEKT.DisplayPages
{
    /// <summary>
    /// Interaction logic for AlarmViewer.xaml
    /// </summary>
    public partial class AlarmViewer : Page
    {
        ArrayList alLogger = new ArrayList();

        public AlarmViewer()
        {
            InitializeComponent();
        }

        public AlarmViewer(ArrayList alLoggerToUI)
        {
            InitializeComponent();
            alLogger = alLoggerToUI;

            //
            //Add items to the listboxes
            //
            for (int i = alLoggerToUI.Count-1; i > 0; i--)
            {
                ListBoxItem lbi = new ListBoxItem();
                LogItem li = (LogItem)alLoggerToUI[i];

                lbi.Content = li.ts_DateTime + "     " + li.ObjectName + "     " + li.ObjectAction;
                int code = li.Code;
                                        //
                                        //Event
                                        //
                if (code == 10)         
                {
                    lbi.Foreground = Brushes.Black;
                }
                                        //
                                        //Alarm, Also add entry to the alarm Tab
                                        //
                else if (code == 20)
                {
                    ListBoxItem lbiAlarm = new ListBoxItem(); ;
                    lbiAlarm.Foreground = Brushes.Red;
                    lbiAlarm.Content = li.ts_DateTime + "     " + li.ObjectName + "     " + li.ObjectAction;
                    listBoxAlarms.Items.Add(lbiAlarm);
                    lbi.Foreground = Brushes.Red;
                }
                                        //
                                        //User action
                                        //
                else
                {
                    lbi.Foreground = Brushes.Green;
                }                

                listBoxEvents.Items.Add(lbi);
            }
        }

        private void btnExportLogs_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowDialog();
            string filePath = fbd.SelectedPath;
            int year = DateTime.Now.Year;
            string month = ""+DateTime.Now.Month;
            string day = ""+DateTime.Now.Day;

            if (Int32.Parse(month) < 10)
            {
                month = "0" + month;
            }

            if (Int32.Parse(day) < 10)
            {
                day = "0" + day;
            }

            string fileName = "Events [" + year + month + day +" "+ DateTime.Now.Hour + "H" + DateTime.Now.Minute + "].csv";

            if (filePath != "")
            {
                try
                {
                    FileStream fs = new FileStream(filePath + "\\" + fileName, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);

                    for (int i = alLogger.Count - 1; i > 0; i--)
                    {
                        ListBoxItem lbi = new ListBoxItem();
                        LogItem li = (LogItem)alLogger[i];
                        string line = li.ts_DateTime + "," + li.OADate + ", '" + li.ObjectName + "," + li.ObjectAction + "," + li.Code;
                        sw.WriteLine(line);
                    }

                    sw.Close();
                    fs.Close();

                    MessageBox.Show("Logs have been exported Successfully","Success",MessageBoxButton.OK,MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Export Logs --> " + ex.Message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }
        }
    }
}
