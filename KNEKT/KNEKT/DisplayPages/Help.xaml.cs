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
using System.IO;
using KNEKT.Classes;
using System.Collections;

namespace KNEKT.DisplayPages
{
    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : Page
    {
        public Window myMainWindow;
        public Help(MainWindow mw)
        {
            InitializeComponent();
            myMainWindow = mw;
           
            try
            {

                //
                //  Load all videos from directory
                //
                foreach (string s in Directory.GetFiles(MainWindow.stat_HelpVideoFolder))
                {
                    int lastSlash = s.LastIndexOf('\\');
                    string sRest = s.Substring(lastSlash + 1, s.Length - lastSlash - 1);

                    string Filname = sRest;

                    int posOfDot = Filname.LastIndexOf(".");
                    string sExtension = Filname.Substring(posOfDot + 1, Filname.Length - posOfDot - 1);

                    if (sExtension.ToLower() == "swf")
                    {
                        lstBoxItems.Items.Add(sRest);
                    }
                }


                //
                //  Load all manuals from Directory
                //
                foreach (string s in Directory.GetFiles(MainWindow.stat_HelpManualFolder))
                {
                    int lastSlash = s.LastIndexOf('\\');
                    string sRest = s.Substring(lastSlash + 1, s.Length - lastSlash - 1);

                    string Filname = sRest;

                    int posOfDot = Filname.LastIndexOf(".");
                    string sExtension = Filname.Substring(posOfDot + 1, Filname.Length - posOfDot - 1);

                    if (sExtension.ToLower() == "pdf")
                    {
                        lstBoxManuals.Items.Add(sRest);
                    }
                }
            }
            catch (IOException ioe)
            {
                MessageBox.Show("IO : " + ioe.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (UnauthorizedAccessException uae)
            {
                MessageBox.Show("Unauthorized Access : " + uae.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ae)
            {
                MessageBox.Show("Argument : " + ae.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("General Error : " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            _Stopped.MotorColor = 1;
            _Starting.MotorColor = 2;
            _Started.MotorColor = 3;
            _Stopping.MotorColor = 7;
            _Fault.FlowbalancerColor = 32;
            _Covered.LevelColor = 3;
            _Uncovered.LevelColor = 1;
        }



        //
        // Play / Pause
        //
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (lstBoxItems.SelectedIndex >= 0)
            {
                string fileName = lstBoxItems.SelectedItem.ToString();
                string fileExtension = fileName.Substring(fileName.LastIndexOf('.'), (fileName.Length - fileName.LastIndexOf('.')));

                if (fileExtension.ToLower() == ".swf")
                {
                    webBrowser2.Visibility = Visibility.Visible;
                    webBrowser2.Navigate(MainWindow.stat_HelpVideoFolder + lstBoxItems.SelectedItem.ToString());
                }
                else
                {
                    //webBrowser1.Visibility = Visibility.Hidden;

                    //if (!bPLaying) //PLay
                    //{
                    //    videoPlayer.Source = new Uri(MainWindow.stat_HelpVideoFolder + lstBoxItems.SelectedItem.ToString());
                    //    videoPlayer.Play();
                    //    bPLaying = true;
                    //    btnPlay.Content = "Pause";
                    //}
                    //else //Pause
                    //{
                    //    videoPlayer.Pause();
                    //    bPLaying = false;
                    //    btnPlay.Content = "Play";
                    //}
                }

            }
        }

        //
        //Stop
        //
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            //videoPlayer.Stop();
        }

        private void volumeControl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //videoPlayer.Volume = (double)volumeControl.Value;
            //try
            //{
            //    lblVolume.Content = "Volume " + Math.Round((videoPlayer.Volume*100),2) + " %";
            //}
            //catch
            //{
            //    lblVolume.Content = "Error";
            //}
        }

        private void videoSeek_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //int sliderValue = (int)videoSeek.Value;

            //TimeSpan ts = new TimeSpan(0, 0, 0, 0, sliderValue);
            //videoPlayer.Position = ts;
        }

        private void btnOpenManual_Click(object sender, RoutedEventArgs e)
        {
            if (lstBoxManuals.SelectedIndex >= 0)
            {
                string fileName = lstBoxManuals.SelectedItem.ToString();
                string fileExtension = fileName.Substring(fileName.LastIndexOf('.'), (fileName.Length - fileName.LastIndexOf('.')));

                if (fileExtension.ToLower() == ".pdf")
                {
                    webBrowser1.Visibility = Visibility.Visible;
                    webBrowser1.Navigate(MainWindow.stat_HelpManualFolder + lstBoxManuals.SelectedItem.ToString());
                }
            }
        }

        private void btnStartTutorial_Click(object sender, RoutedEventArgs e)
        {
            DisplayPages.HelpWindow pageHelpWindow = new DisplayPages.HelpWindow();
            pageHelpWindow.Owner = myMainWindow;
            pageHelpWindow.ShowDialog();
        }

    }
}
