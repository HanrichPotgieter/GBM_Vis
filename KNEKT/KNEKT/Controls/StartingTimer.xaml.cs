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
using LiteCoSControls;

namespace KNEKT.Controls
{
    /// <summary>
    /// Interaction logic for StartingTimer.xaml
    /// </summary>
    public partial class StartingTimer : UserControl
    {
        private double dMinutes = 0;
        private double dSeconds = 0;
        private int _TargetTimeMinimum = 0;
        private int _TargetTimeMaximum = 100;
        private bool _TargetTimeSigned = false;
        private int _MultiplyByWriteValue = 0;
        private int _DivideByWriteValue = 0;
        private string _TargetTimeDBOffset = "DB0.DBD0";
        private Controller PLC_W;

        //------------------------------------------------------------------------------//
        //                                 Constructor                                  //
        //------------------------------------------------------------------------------//

        public StartingTimer()
        {
            InitializeComponent();
            StartingTimer_ShowInMinutesAndSeconds = false;
            StartingTimer_ProgressBarColor = Brushes.Aqua;
        }

        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//

        public string StartingTimer_Title
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

        public bool StartingTimer_TargetTimeSigned
        {
            get
            {
                return this._TargetTimeSigned;
            }
            set
            {
                this._TargetTimeSigned = value;
            }
        }

        public string StartingTimer_TargetTimeDBOffset
        {
            get
            {
                return this._TargetTimeDBOffset;
            }
            set
            {
                this._TargetTimeDBOffset = value;
            }
        }

        public Controller StartingTimer_WriteController
        {
            get
            {
                return this.PLC_W;
            }
            set
            {
                this.PLC_W = value;
            }
        }

        public int StartingTimer_TargetTimeMinimum
        {
            get
            {
                return _TargetTimeMinimum;
            }
            set
            {
                _TargetTimeMinimum = value;
            }
        }

        public int StartingTimer_TargetTimeMaximum
        {
            get
            {
                return _TargetTimeMaximum;
            }
            set
            {
                _TargetTimeMaximum = value;
            }
        }

        public int StartingTimer_MultiplyByWriteValue
        {
            get
            {
                return _MultiplyByWriteValue;
            }
            set
            {
                _MultiplyByWriteValue = value;
            }
        }
        
        public int StartingTimer_DivideByWriteValue
        {
            get
            {
                return _DivideByWriteValue;
            }
            set
            {
                _DivideByWriteValue = value;
            }
        }

        private void progressBarStarting_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //if (_ShowInMinutesAndSeconds)
            //{
            //    lblProgress.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            //    {
            //        lblProgress.Content = "" + dMinutes + "m:" + dSeconds + "s";
            //    }));
            //}

            //{
            //    lblProgress.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            //    {
            //        lblProgress.Content = "" + e.NewValue;
            //    }));
            //}


        }

        private bool _ShowInMinutesAndSeconds;
        public bool StartingTimer_ShowInMinutesAndSeconds
        {
            get { return _ShowInMinutesAndSeconds; }
            set { _ShowInMinutesAndSeconds = value; }
        }


        private double _StartingTimerValue;
        public double StartingTimer_Value
        {
            get { return _StartingTimerValue; }
            set 
            { 
                _StartingTimerValue = value;

                if (_ShowInMinutesAndSeconds)
                {
                    dMinutes = (int)(value / 60);
                    dSeconds = (value % 60);

                    progressBarStarting.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        progressBarStarting.Value = value;
                    }));

                    lblProgress.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        lblProgress.Content = "" + dMinutes + "m : " + dSeconds + "s";
                    }));
                }
                else
                {
                    progressBarStarting.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        progressBarStarting.Value = value;
                    }));

                    lblProgress.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        lblProgress.Content = ""+value;
                    }));
                }                
            }
        }

        private Brush _ProgressBarColor;
        public Brush StartingTimer_ProgressBarColor
        {
            get { return _ProgressBarColor; }
            set 
            { 
                _ProgressBarColor = value;
                progressBarStarting.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    progressBarStarting.Foreground = value;
                }));
            }
        }

        private int _Maximum;
        public int StartingTimer_Maximum
        {
            get { return _Maximum; }
            set 
            { 
                _Maximum = value;
                progressBarStarting.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    progressBarStarting.Maximum = value;
                }));
            }
        }

        //------------------------------------------------------------------------------//
        //                              Button Clicks                                   //
        //------------------------------------------------------------------------------//
                
        private void lblTargetTimeMins_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bool bOpen = Classes.StandardCode.IsSpecificWindowOpen(Application.Current.Windows, this.lblTitle.Content.ToString() + " Target");

            if (!bOpen)
            {
                LiteCoSControls.NumericKeypad numKeypad1 = new NumericKeypad(lblTargetTimeMins.Content.ToString());
                numKeypad1.NumericKeypad_Driver = NumericKeypad.eDrivers.INGEARS7;
                numKeypad1.NumericKeypad_TagDatatype = NumericKeypad.eTagDataTypes.INT;
                numKeypad1.NumericKeypad_PLC_Write = StartingTimer_WriteController;
                numKeypad1.NumericKeypad_MinLimit = StartingTimer_TargetTimeMinimum;
                numKeypad1.NumericKeypad_MaxLimit = StartingTimer_TargetTimeMaximum;
                numKeypad1.NumericKeypad_Title = StartingTimer_Title;
                numKeypad1.NumericKeypad_PrimaryDBOffset = StartingTimer_TargetTimeDBOffset; //"DBX.DBWX"
                numKeypad1.NumericKeypad_IsValueWholeNumber = false;
                numKeypad1.NumericKeypad_IsValueSigned = StartingTimer_TargetTimeSigned;
                numKeypad1.NumericKeypad_MultiplyWriteValue = StartingTimer_MultiplyByWriteValue;
                numKeypad1.NumericKeypad_DivideyWriteValue = StartingTimer_DivideByWriteValue;

                numKeypad1.Show();
            }
        }
    }
}
