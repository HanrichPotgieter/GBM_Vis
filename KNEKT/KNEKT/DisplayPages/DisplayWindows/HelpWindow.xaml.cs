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
using KNEKT.Classes;
using System.Collections;

namespace KNEKT.DisplayPages
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        // HelpPageViewModel hpvm;
        ArrayList alGrids = new ArrayList();
        ArrayList alGridInfoTexts = new ArrayList();
        ArrayList alPageRefEllipses = new ArrayList();
        ArrayList alGridTextLines = new ArrayList();
        private int pageNumber = 0;
        private Grid tempGrid = new Grid();


        public HelpWindow()
        {
            InitializeComponent();
            txtDateTime.Text = DateTime.Now.ToString();
            txtLineName.Text = "Line Number 1";



            FrameworkElement feGridAsParent = (FrameworkElement)GridMain;
            IEnumerable children = LogicalTreeHelper.GetChildren(feGridAsParent);   //Gets all Children of the Grid

            foreach (object child in children)
            {
                if (child is FrameworkElement)
                {
                    FrameworkElement Control = (FrameworkElement)child;

                    if (Control.Name.ToLower().Contains("grid"))
                    {
                        //listBox1.Items.Add(Control.ToString());

                        if (Control.Name.ToString() != "PageReferenceGrid")
                        {
                            alGrids.Add(Control);
                            Control.Opacity = 0.1;
                        }
                        FrameworkElement feCurrentChildGridAsParent = (FrameworkElement)Control;
                        IEnumerable currentGridChildren = LogicalTreeHelper.GetChildren(feCurrentChildGridAsParent);   //Gets all Children of the Current Child Grid

                        foreach (object currentChild in currentGridChildren)
                        {
                            if (currentChild is FrameworkElement)
                            {
                                FrameworkElement currentControl = (FrameworkElement)currentChild;

                                if (currentControl.Name.ToLower().Contains("gridtext"))
                                {
                                    alGridInfoTexts.Add(currentControl);
                                    currentControl.Visibility = Visibility.Hidden;
                                }

                                if (currentControl.Name.ToLower().Contains("pagerefellipse"))
                                {
                                    alPageRefEllipses.Add(currentControl);
                                }

                                if (currentControl.Name.ToLower().StartsWith("textline"))
                                {
                                    alGridTextLines.Add(currentControl);
                                    currentControl.Visibility = Visibility.Hidden;
                                }
                            }
                        }
                    }
                }
            }

            PageChange();
        }


        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (pageNumber <= 36)
            {
                pageNumber++;
            }
            PageChange();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (pageNumber == 0)
            {
            }
            else
            {
                pageNumber--;
            }
            PageChange();
        }

        public void HideUnusedGrids(string gName, string tInfoText, string eEllipseNumber, string glTextLine)
        {

            foreach (Grid g in alGrids)
            {
                g.Opacity = 0.1;
                if (gName == "gridLineColourLegend")
                {
                    gridLineColourLegend.Visibility = Visibility.Visible;
                    gridLineColourLegend.Opacity = 1;
                    gridElementColourLegend.Visibility = Visibility.Hidden;
                }
                else if (gName == "gridElementColourLegend")
                {
                    gridElementColourLegend.Visibility = Visibility.Visible;
                    gridElementColourLegend.Opacity = 1;
                    gridLineColourLegend.Visibility = Visibility.Hidden;
                }
                else
                {
                    gridLineColourLegend.Visibility = Visibility.Hidden;
                    gridElementColourLegend.Visibility = Visibility.Hidden;

                    if (g.Name != gName)
                    {
                        g.Opacity = 0.1;
                    }
                    else
                    {
                        g.Opacity = 1;
                    }
                }
            }

            foreach (TextBlock t in alGridInfoTexts)
            {
                if (t.Name != tInfoText)
                {
                    t.Visibility = Visibility.Hidden;
                }
                else
                {
                    t.Visibility = Visibility.Visible;
                }
            }

            foreach (Grid gl in alGridTextLines)
            {
                if (gl.Name != glTextLine)
                {
                    gl.Visibility = Visibility.Hidden;
                }
                else
                {
                    gl.Visibility = Visibility.Visible;
                }
            }

            foreach (Ellipse e in alPageRefEllipses)
            {
                if (e.Name != eEllipseNumber)
                {
                    //Color c = Color.FromRgb(22,22,22);
                    //e.Fill = new SolidColorBrush(c);
                    e.Fill = Brushes.DarkGray;
                }
                else
                {
                    e.Fill = Brushes.Red;
                }
            }
        }

        public void PageChange()
        {
            switch (pageNumber)
            {
                case 0:
                    // gridStartHelp.Visibility = Visibility.Visible;
                    // zgridStartHelp.Opacity = 1;
                    HideUnusedGrids(gridStartHelp.Name.ToString(), gridStartHelpGridText.Name.ToString(), "", "");
                    break;

                case 1:
                    HideUnusedGrids(gridLineButtons.Name.ToString(), gridLineButtonsGridText.Name.ToString(), PageRefEllipse1.Name.ToString(), textLineLineButtons.Name);
                    break;

                case 2:
                    HideUnusedGrids(gridElementDesc.Name.ToString(), gridElementDescGridText.Name.ToString(), PageRefEllipse2.Name.ToString(), textLineElementDesc.Name);
                    break;

                case 3:
                    HideUnusedGrids(gridElementStatus.Name.ToString(), gridElementStatusGridText.Name.ToString(), PageRefEllipse3.Name.ToString(), textLineElementStatus.Name);
                    break;

                case 4:
                    HideUnusedGrids(gridPLCComms.Name.ToString(), gridPLCCommsGridText.Name.ToString(), PageRefEllipse4.Name.ToString(), textLinePLCComms.Name);
                    break;

                case 5:
                    HideUnusedGrids(gridLineName.Name.ToString(), gridLineNameGridText.Name.ToString(), PageRefEllipse5.Name.ToString(), textLineLineName.Name);
                    break;

                case 6:
                    HideUnusedGrids(gridAlarmViewer.Name.ToString(), gridAlarmViewerGridText.Name.ToString(), PageRefEllipse6.Name.ToString(), textLineAlarmViewer.Name);
                    break;

                case 7:
                    HideUnusedGrids(gridAlarmMessages.Name.ToString(), gridAlarmMessagesGridText.Name.ToString(), PageRefEllipse7.Name.ToString(), textLineAlarmMessages.Name);
                    break;

                case 8:
                    HideUnusedGrids(gridDateTime.Name.ToString(), gridDateTimeGridText.Name.ToString(), PageRefEllipse8.Name.ToString(), textLineDateTime.Name);
                    break;

                case 9:
                    HideUnusedGrids(gridLogOnButton.Name.ToString(), gridLogOnButtonGridText.Name.ToString(), PageRefEllipse9.Name.ToString(), textLineLogOnButton.Name);
                    break;

                case 10:
                    HideUnusedGrids(gridLogOffButton.Name.ToString(), gridLogOffButtonGridText.Name.ToString(), PageRefEllipse10.Name.ToString(), textLineLogOffButton.Name);
                    break;

                case 11:
                    HideUnusedGrids(gridLoggedInUser.Name.ToString(), gridLoggedInUserGridText.Name.ToString(), PageRefEllipse11.Name.ToString(), textLineLogggedInUser.Name);
                    break;

                case 12:
                    HideUnusedGrids(gridSettingsButton.Name.ToString(), gridSettingsButtonGridText.Name.ToString(), PageRefEllipse12.Name.ToString(), textLineSettingsButton.Name);
                    break;

                case 13:
                    HideUnusedGrids(gridHelpButton.Name.ToString(), gridHelpButtonGridText.Name.ToString(), PageRefEllipse13.Name.ToString(), textLineHelpButton.Name);
                    break;

                case 14:
                    HideUnusedGrids(gridReportingButton.Name.ToString(), gridReportingButtonGridText.Name.ToString(), PageRefEllipse14.Name.ToString(), textLineReportingButton.Name);
                    break;

                case 15:
                    HideUnusedGrids(gridJobButton.Name.ToString(), gridJobButtonGridText.Name.ToString(), PageRefEllipse15.Name.ToString(), textLineJobButton.Name);
                    break;

                case 16:
                    HideUnusedGrids(gridStartButton.Name.ToString(), gridStartButtonGridText.Name.ToString(), PageRefEllipse16.Name.ToString(), textLineStartButton.Name);
                    break;

                case 17:
                    HideUnusedGrids(gridPauseButton.Name.ToString(), gridPauseButtonGridText.Name.ToString(), PageRefEllipse17.Name.ToString(), textLinePauseButton.Name);
                    break;

                case 18:
                    HideUnusedGrids(gridStopButton.Name.ToString(), gridStopButtonGridText.Name.ToString(), PageRefEllipse18.Name.ToString(), textLineStopButton.Name);
                    break;

                case 19:
                    HideUnusedGrids(gridAcknowledgeButton.Name.ToString(), gridAcknowledgeButtonGridText.Name.ToString(), PageRefEllipse19.Name.ToString(), textLineAcknowledgeButton.Name);
                    break;

                case 20:
                    HideUnusedGrids(gridMuteSirenButton.Name.ToString(), gridMuteSirenButtonGridText.Name.ToString(), PageRefEllipse20.Name.ToString(), textLineMuteSirenButton.Name);
                    break;

                case 21:
                    HideUnusedGrids(gridEmergencyStopButton.Name.ToString(), gridEmergencyStopButtonGridText.Name.ToString(), PageRefEllipse21.Name.ToString(), textLineEmergencyStopButton.Name);
                    break;

                case 22:
                    HideUnusedGrids(gridLineColourLegend.Name.ToString(), gridLineColourLegendGridText1.Name.ToString(), PageRefEllipse22.Name.ToString(), textLineLineColourLegendLine1.Name);
                    break;

                case 23:
                    HideUnusedGrids(gridLineColourLegend.Name.ToString(), gridLineColourLegendGridText2.Name.ToString(), PageRefEllipse23.Name.ToString(), textLineLineColourLegendLine2.Name);
                    break;

                case 24:
                    HideUnusedGrids(gridLineColourLegend.Name.ToString(), gridLineColourLegendGridText3.Name.ToString(), PageRefEllipse24.Name.ToString(), textLineLineColourLegendLine3.Name);
                    break;

                case 25:
                    HideUnusedGrids(gridLineColourLegend.Name.ToString(), gridLineColourLegendGridText4.Name.ToString(), PageRefEllipse25.Name.ToString(), textLineLineColourLegendLine4.Name);
                    break;

                case 26:
                    HideUnusedGrids(gridLineColourLegend.Name.ToString(), gridLineColourLegendGridText5.Name.ToString(), PageRefEllipse26.Name.ToString(), textLineLineColourLegendLine5.Name);
                    break;

                case 27:
                    HideUnusedGrids(gridLineColourLegend.Name.ToString(), gridLineColourLegendGridText6.Name.ToString(), PageRefEllipse27.Name.ToString(), textLineLineColourLegendLine6.Name);
                    break;

                case 28:
                    HideUnusedGrids(gridLineColourLegend.Name.ToString(), gridLineColourLegendGridText7.Name.ToString(), PageRefEllipse28.Name.ToString(), textLineLineColourLegendLine7.Name);
                    break;

                case 29:
                    HideUnusedGrids(gridLineColourLegend.Name.ToString(), gridLineColourLegendGridText8.Name.ToString(), PageRefEllipse29.Name.ToString(), textLineLineColourLegendLine8.Name);
                    break;

                case 30:
                    HideUnusedGrids(gridElementColourLegend.Name.ToString(), gridElementColourLegendGridText1.Name.ToString(), PageRefEllipse30.Name.ToString(), textLineElementColourLegendLine1.Name);
                    break;

                case 31:
                    HideUnusedGrids(gridElementColourLegend.Name.ToString(), gridElementColourLegendGridText2.Name.ToString(), PageRefEllipse31.Name.ToString(), textLineElementColourLegendLine2.Name);
                    break;

                case 32:
                    HideUnusedGrids(gridElementColourLegend.Name.ToString(), gridElementColourLegendGridText3.Name.ToString(), PageRefEllipse32.Name.ToString(), textLineElementColourLegendLine3.Name);
                    break;

                case 33:
                    HideUnusedGrids(gridElementColourLegend.Name.ToString(), gridElementColourLegendGridText4.Name.ToString(), PageRefEllipse33.Name.ToString(), textLineElementColourLegendLine4.Name);
                    break;

                case 34:
                    HideUnusedGrids(gridElementColourLegend.Name.ToString(), gridElementColourLegendGridText5.Name.ToString(), PageRefEllipse34.Name.ToString(), textLineElementColourLegendLine5.Name);
                    break;

                case 35:
                    HideUnusedGrids(gridElementColourLegend.Name.ToString(), gridElementColourLegendGridText6.Name.ToString(), PageRefEllipse35.Name.ToString(), textLineElementColourLegendLine6.Name);
                    break;

                case 36:
                    HideUnusedGrids(gridElementColourLegend.Name.ToString(), gridElementColourLegendGridText7.Name.ToString(), PageRefEllipse36.Name.ToString(), textLineElementColourLegendLine7.Name);
                    break;

                case 37:
                    this.Close();
                    break;

                default:
                    pageNumber = 0;
                    HideUnusedGrids("", "", "", "");
                    break;
            }
        }

        private void btnExitHelp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region Ellipse Mouse Downs
        private void PageRefEllipse1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 1;
            PageChange();
        }

        private void PageRefEllipse2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 2;
            PageChange();
        }

        private void PageRefEllipse3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 3;
            PageChange();
        }

        private void PageRefEllipse4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 4;
            PageChange();
        }

        private void PageRefEllipse5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 5;
            PageChange();
        }

        private void PageRefEllipse6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 6;
            PageChange();
        }

        private void PageRefEllipse7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 7;
            PageChange();
        }

        private void PageRefEllipse8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 8;
            PageChange();
        }

        private void PageRefEllipse9_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 9;
            PageChange();
        }

        private void PageRefEllipse10_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 10;
            PageChange();
        }

        private void PageRefEllipse11_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 11;
            PageChange();
        }

        private void PageRefEllipse12_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 12;
            PageChange();
        }

        private void PageRefEllipse13_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 13;
            PageChange();
        }

        private void PageRefEllipse14_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 14;
            PageChange();
        }

        private void PageRefEllipse15_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 15;
            PageChange();
        }

        private void PageRefEllipse16_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 16;
            PageChange();
        }

        private void PageRefEllipse17_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 17;
            PageChange();
        }

        private void PageRefEllipse18_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 18;
            PageChange();
        }

        private void PageRefEllipse19_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 19;
            PageChange();
        }

        private void PageRefEllipse20_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 20;
            PageChange();
        }

        private void PageRefEllipse21_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 21;
            PageChange();
        }

        private void PageRefEllipse22_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 22;
            PageChange();
        }

        private void PageRefEllipse23_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 23;
            PageChange();
        }

        private void PageRefEllipse24_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 24;
            PageChange();
        }

        private void PageRefEllipse25_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 25;
            PageChange();
        }

        private void PageRefEllipse26_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 26;
            PageChange();
        }

        private void PageRefEllipse27_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 27;
            PageChange();
        }

        private void PageRefEllipse28_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 28;
            PageChange();
        }

        private void PageRefEllipse29_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 29;
            PageChange();
        }

        private void PageRefEllipse30_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 30;
            PageChange();
        }

        private void PageRefEllipse31_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 31;
            PageChange();
        }

        private void PageRefEllipse32_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 32;
            PageChange();
        }

        private void PageRefEllipse33_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 33;
            PageChange();
        }

        private void PageRefEllipse34_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 34;
            PageChange();
        }

        private void PageRefEllipse35_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 35;
            PageChange();
        }

        private void PageRefEllipse36_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageNumber = 36;
            PageChange();
        }
        #endregion

    }
}
