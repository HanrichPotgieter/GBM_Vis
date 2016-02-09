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
    /// Interaction logic for SpeedControl.xaml
    /// </summary>
    public partial class SpeedControl : UserControl
    {
        private Controller PLC_W;
        private string _SpeedSetPointDBOffset = "DB0.DBD0";
        private int _MultiplyByWriteValue = 0;
        private int _DivideByWriteValue = 0;
        private string _SpeedControlNumKeypad_Title = "";
        private int _MaxThroughputValue = 0;

        public SpeedControl()
        {
            InitializeComponent();

            SpeedControl_SetPoint = 0;
            SpeedControl_FeedBack = 0;
            SpeedControl_IsSettable = false;
            SpeedControl_HasFeedBack = false;
            SpeedControl_IsOnProfibus = false;

        }

        public string SpeedControl_Title
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


        public string SpeedControlNumKeypad_Title
        {
            get
            {
                return _SpeedControlNumKeypad_Title;
            }
            set
            {
                _SpeedControlNumKeypad_Title = value;
            }
        }

        public string SpeedControl_SetpointDBOffset
        {
            get
            {
                return this._SpeedSetPointDBOffset;
            }
            set
            {
                this._SpeedSetPointDBOffset = value;
            }
        }
        private int _SpeedControlSP;
        public int SpeedControl_SetPoint
        {
            get { return _SpeedControlSP; }
            set 
            {
                if (value <= 100)
                {
                    _SpeedControlSP = value;
                    lblPercentageSP.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        lblPercentageSP.Content = value + " %";
                    }));                    
                }
            }
        }

       
        private int _SpeedControlFB;
        public int SpeedControl_FeedBack
        {
            get { return _SpeedControlFB; }
            set
            {
                if (value >= 0)
                {
                    _SpeedControlFB = value;
                    lblPercentageFB.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        lblPercentageFB.Content = value + " %";
                    }));
                }
            }
        }
        private bool _IsSettable;

        public bool SpeedControl_IsSettable
        {
            get { return _IsOnProfibus; }
            set 
            {
                _IsOnProfibus = value;
                if(value == false)
                {
                    //if (!bIsInFault)
                    //{
                        gridMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            //gridMain.Background = Brushes.White;
                        }));
                    //}
                }
                //else
                //{
                //    if (!bIsInFault)
                //    {
                //        gridMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //        {
                //            //gridMain.Background = Brushes.LightGray;
                //        }));
                //    }
                //}
            }
        }
        private bool _HasFeedBack;
        public bool SpeedControl_HasFeedBack
        {
            get { return _HasFeedBack; }
            set 
            {
                _HasFeedBack = value;
                if(value == false)
                {                   
                    borderMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                       borderMain.Height = 38;
                    })); 
                }
                else
                {
                    borderMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        borderMain.Height = 50;
                    }));
                }
            }
        }
        private bool _IsOnProfibus;
        public bool SpeedControl_IsOnProfibus
        {
            get { return _IsOnProfibus; }
            set
            {
                _IsOnProfibus = value;
                if (value == true)
                {
                    borderMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        borderMain.Height = 65;
                    }));
                }
                
            }
        }

        private bool bIsInFault;

        public bool BIsInFault
        {
            get { return bIsInFault; }
            set { bIsInFault = value; }
        }

        public Controller SpeedControl_WriteController
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


        public int SpeedControl_MultiplyWriteValue
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

        public int SpeedControl_DivideByWriteValue
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

        public int SpeedControl_MaxThroughPutValue
        {
            get
            {
                return _MaxThroughputValue;
            }
            set
            {
                _MaxThroughputValue = value;
            }
        }
        private void lblPercentageSP_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.stat_iUserLevel >= 5)
            {
                LiteCoSControls.NumericKeypad numKeypad1 = new LiteCoSControls.NumericKeypad(lblPercentageSP.Content.ToString());
                numKeypad1.NumericKeypad_Driver = LiteCoSControls.NumericKeypad.eDrivers.INGEARS7;
                numKeypad1.NumericKeypad_IsValueSigned = false;
                numKeypad1.NumericKeypad_IsValueWholeNumber = true;
                numKeypad1.NumericKeypad_MaxLimit = 100;
                numKeypad1.NumericKeypad_MinLimit = 0;
                numKeypad1.NumericKeypad_PrimaryDBOffset = SpeedControl_SetpointDBOffset; //"DB321.DBD98";
                numKeypad1.NumericKeypad_TagDatatype = LiteCoSControls.NumericKeypad.eTagDataTypes.REAL;
                numKeypad1.NumericKeypad_Title = SpeedControlNumKeypad_Title;
                numKeypad1.NumericKeypad_PLC_Write = SpeedControl_WriteController;
                numKeypad1.NumericKeypad_MultiplyWriteValue = SpeedControl_MultiplyWriteValue;
                numKeypad1.NumericKeypad_DivideyWriteValue = SpeedControl_DivideByWriteValue;
                numKeypad1.NumericKeypad_TotalCapacity = SpeedControl_MaxThroughPutValue;
                numKeypad1.Show();
            }
        }


    }
}
