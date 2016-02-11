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
//using LiteCoSControls;

namespace KNEKT.Controls
{
    /// <summary>
    /// Interaction logic for ScaleInfo2.xaml
    /// </summary>
    public partial class ScaleInfo2 : UserControl
    {
        private int _InFlowrateMinimum = 0;
        private int _InFlowrateMaximum = 0;
        private int _MaxThroughputValue = 0;
        private int _DivideWriteValue = 0;
        private int _MultiplyWriteValue = 0;

        private bool _IsValueWholeNumber = true;
        private bool _IsValueSigned = false;

        private string _InFlowrateAddress = "DB0.DBD0";
        private Controller PLC_W;


        //------------------------------------------------------------------------------//
        //                                 Constructor                                  //
        //------------------------------------------------------------------------------//

        public ScaleInfo2()
        {
            InitializeComponent();
        }


        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//

        public string ScaleInfo_Title
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


        public string ScaleInfo_InFlowrateOffset
        {
            get
            {
                return this._InFlowrateAddress;
            }
            set
            {
                this._InFlowrateAddress = value;
            }
        }

        public Controller ScaleInfo_Controller_W
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

        public int ScaleInfo_InFlowrateMinimum
        {
            get
            {
                return _InFlowrateMinimum;
            }
            set
            {
                _InFlowrateMinimum = value;
            }
        }

        public int ScaleInfo_InFlowrateMaximum
        {
            get
            {
                return _InFlowrateMaximum;
            }
            set
            {
                _InFlowrateMaximum = value;
            }
        }

        public int ScaleInfo_MaxThroughPutValue
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

        public int ScaleInfo_DivideWriteValue
        {
            get
            {
                return _DivideWriteValue;
            }
            set
            {
                _DivideWriteValue = value;
            }
        }

        public int ScaleInfo_MultiplyWriteValue
        {
            get
            {
                return _MultiplyWriteValue;
            }
            set
            {
                _MultiplyWriteValue = value;
            }
        }

        public bool ScaleInfo_IsValueSigned
        {
            get
            {
                return _IsValueSigned;
            }
            set
            {
                _IsValueSigned = value;
            }
        }

        public bool ScaleInfo_IsValueWholeNumber
        {
            get
            {
                return _IsValueWholeNumber;
            }
            set
            {
                _IsValueWholeNumber = value;
            }
        }

        //------------------------------------------------------------------------------//
        //                              Button Clicks                                   //
        //------------------------------------------------------------------------------//

        private void lblInFlowrate_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bool bOpen = Classes.StandardCode.IsSpecificWindowOpen(Application.Current.Windows, this.lblTitle.Content.ToString());

            if (!bOpen)
            {
                //DisplayWindows.NumericKeypad numKeypad = new DisplayWindows.NumericKeypad(lblInFlowrate.Content.ToString(), this.lblTitle.Content.ToString(), ScaleInfo_Controller_W, ScaleInfo_InFlowrateOffset,false,false,false,ScaleInfo_MaxThroughPutValue);

                //numKeypad.NumericKeypad_MinLimit = ScaleInfo_InFlowrateMinimum;
                //numKeypad.NumericKeypad_MaxLimit = ScaleInfo_InFlowrateMaximum;

                //LiteCoSControls.NumericKeypad numKeypad = new LiteCoSControls.NumericKeypad(lblInFlowrate.Content.ToString());
                //numKeypad.NumericKeypad_MinLimit = _InFlowrateMinimum;
                //numKeypad.NumericKeypad_MaxLimit = _InFlowrateMaximum;
                //numKeypad.NumericKeypad_Title = this.lblTitle.Content.ToString();
                //numKeypad.NumericKeypad_TagDatatype = NumericKeypad.eTagDataTypes.DWORD; //We will assume that this will always be a DWORD as a Flowrate input will ALWAYS be a DBW (not negative)
                //numKeypad.NumericKeypad_PrimaryDBOffset = ScaleInfo_InFlowrateOffset;
                //numKeypad.NumericKeypad_IsValueWholeNumber = ScaleInfo_IsValueWholeNumber;
                //numKeypad.NumericKeypad_IsValueSigned = ScaleInfo_IsValueSigned;
                //numKeypad.NumericKeypad_Driver = LiteCoSControls.NumericKeypad.eDrivers.INGEARS7;
                //numKeypad.NumericKeypad_PLC_Write = ScaleInfo_Controller_W;
                //numKeypad.NumericKeypad_MultiplyWriteValue = ScaleInfo_MultiplyWriteValue;
                //numKeypad.NumericKeypad_DivideyWriteValue = ScaleInfo_DivideWriteValue;
                //numKeypad.NumericKeypad_TotalCapacity = ScaleInfo_MaxThroughPutValue;


                //numKeypad.Show();
            }
        }

        private void lblAlarmNo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Classes.MEAG m1 = new Classes.MEAG();
            m1.MEAG_AlarmCode = Convert.ToInt32(lblAlarmNo.Content);
            MainWindow.sElementDescription = m1.MEAG_AlarmDescritpion;
            MainWindow.sElementState = m1.MEAG_AlarmCodeDescription;
            //MessageBox.Show(m1.MEAG_AlarmCode + " - " + m1.MEAG_AlarmDescritpion, "MEAG Alarm Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
