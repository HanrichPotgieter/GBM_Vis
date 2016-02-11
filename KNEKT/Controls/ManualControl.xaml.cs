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
using S7Link;

namespace KNEKT.Controls
{
    /// <summary>
    /// Interaction logic for ManualControl.xaml
    /// </summary>
    public partial class ManualControl : UserControl
    {
        private string _OnDBAddress = "DB0.DBD0";
        private Controller PLC_W;
        private bool _OnButtonPressed = false;

        //------------------------------------------------------------------------------//
        //                                 Constructor                                  //
        //------------------------------------------------------------------------------//
        public ManualControl()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
        //------------------------------------------------------------------------------//

        public string ManualControl_Title
        {
            get
            {
                return lblTitle.Content.ToString();
            }
            set
            {
                lblTitle.Content = value;
            }
        }

        public Controller ManualControl_WriteController
        {
            set
            {
                PLC_W = value;
            }
        }

        /// <summary>
        /// DB Offset to read for display of which button is on
        /// </summary>
        public string ManualControl_OnBit
        {
            get
            {
                return _OnDBAddress;
            }
            set
            {
                _OnDBAddress = value;
            }
        }


        public bool ManualControl_SetSelectedButton
        {
            get
            {
                return _OnButtonPressed;
            }
            set
            {
                if (value) //On Button Pressed
                {
                    #region Button Gradients


                    btnOn.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        LinearGradientBrush lgb = new LinearGradientBrush();
                        lgb.StartPoint = new Point(1, 0);
                        lgb.EndPoint = new Point(1, 1);
                        GradientStop g1 = new GradientStop(Colors.DarkGreen, 0.0);
                        GradientStop g2 = new GradientStop(Colors.Green, 0.3);
                        GradientStop g3 = new GradientStop(Colors.Green, 0.8);
                        GradientStop g4 = new GradientStop(Colors.DarkGreen, 1.1);

                        lgb.GradientStops.Add(g1);
                        lgb.GradientStops.Add(g2);
                        lgb.GradientStops.Add(g3);
                        lgb.GradientStops.Add(g4);

                        btnOn.Background = lgb;

                        //rectOn.Visibility = Visibility.Hidden;

                    }));
                    btnOff.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        LinearGradientBrush lgb = new LinearGradientBrush();
                        lgb.StartPoint = new Point(1, 0);
                        lgb.EndPoint = new Point(1, 1);
                        GradientStop g1 = new GradientStop(Colors.White, 0.0);
                        GradientStop g2 = new GradientStop(Colors.Pink, 0.5);
                        //GradientStop g3 = new GradientStop(Colors.Red, 0.9);
                        GradientStop g4 = new GradientStop(Colors.Red, 1.1);

                        lgb.GradientStops.Add(g1);
                        lgb.GradientStops.Add(g2);
                        //lgb.GradientStops.Add(g3);
                        lgb.GradientStops.Add(g4);

                        btnOff.Background = lgb;

                        //rectOff.Visibility = Visibility.Visible;

                    }));

                    #endregion



                    _OnButtonPressed = true;
                }
                else //Off Button Pressed
                {
                    #region Button Gradients


                    btnOn.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        LinearGradientBrush lgb = new LinearGradientBrush();
                        lgb.StartPoint = new Point(1, 0);
                        lgb.EndPoint = new Point(1, 1);
                        GradientStop g1 = new GradientStop(Colors.White, 0.0);
                        GradientStop g2 = new GradientStop(Colors.LightGreen, 0.3);
                        //GradientStop g3 = new GradientStop(Colors.Green, 0.9);
                        //GradientStop g4 = new GradientStop(Colors.DarkGreen, 1.1);

                        lgb.GradientStops.Add(g1);
                        lgb.GradientStops.Add(g2);
                        //lgb.GradientStops.Add(g3);
                        //lgb.GradientStops.Add(g4);

                        btnOn.Background = lgb;

                        //rectOn.Visibility = Visibility.Visible;

                    }));
                    btnOff.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        LinearGradientBrush lgb = new LinearGradientBrush();
                        lgb.StartPoint = new Point(1, 0);
                        lgb.EndPoint = new Point(1, 1);
                        GradientStop g1 = new GradientStop(Colors.DarkRed, 0.0);
                        GradientStop g2 = new GradientStop(Colors.Red, 0.3);
                        GradientStop g3 = new GradientStop(Colors.Red, 0.8);
                        GradientStop g4 = new GradientStop(Colors.DarkRed, 1.1);

                        lgb.GradientStops.Add(g1);
                        lgb.GradientStops.Add(g2);
                        lgb.GradientStops.Add(g3);
                        lgb.GradientStops.Add(g4);

                        btnOff.Background = lgb;

                        //rectOff.Visibility = Visibility.Hidden;

                    }));

                    #endregion


                    _OnButtonPressed = false;
                }
            }
        }

        private void btnOn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_OnDBAddress != "DB0.DBD0")
                {

                    if (!PLC_W.IsConnected)
                        PLC_W.Connect();

                    if (PLC_W.IsConnected)
                    {
                        Tag t = new Tag(_OnDBAddress);
                        t.DataType = S7Link.Tag.ATOMIC.BOOL;
                        t.Length = 1;
                        t.Value = true;
                        PLC_W.WriteTag(t);

                        LogItem li = new LogItem(DateTime.Now, DateTime.Now.ToOADate(), "btnOn", this.lblTitle.Content.ToString(), 30);
                        MainWindow.alLoggerToSQL.Add(li);
                    }
                    else
                    {
                        MessageBox.Show("Write --> Not Connected");
                    }
                }
                else
                {
                    MessageBox.Show("Value has not been set", "Invalid Setting", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error -->" + ex.Message);
            }
        }

        private void btnOff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_OnDBAddress != "DB0.DBD0")
                {

                    if (!PLC_W.IsConnected)
                        PLC_W.Connect();

                    if (PLC_W.IsConnected)
                    {
                        Tag t = new Tag(_OnDBAddress);
                        t.DataType = S7Link.Tag.ATOMIC.BOOL;
                        t.Length = 1;
                        t.Value = false;
                        PLC_W.WriteTag(t);

                        LogItem li = new LogItem(DateTime.Now, DateTime.Now.ToOADate(), "btnOff", this.lblTitle.Content.ToString(), 30);
                        MainWindow.alLoggerToSQL.Add(li);
                    }
                    else
                    {
                        MessageBox.Show("Write --> Not Connected");
                    }
                }
                else
                {
                    MessageBox.Show("Value has not been set", "Invalid Setting", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error -->" + ex.Message);
            }
        }



    }
}
