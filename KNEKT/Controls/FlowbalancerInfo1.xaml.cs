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
    /// Interaction logic for FlowbalancerInfo1.xaml
    /// </summary>
    public partial class FlowbalancerInfo1 : UserControl
    {
        private double _InFlowrateMinimum = 0;
        private double _InFlowrateMaximum = 0;
        private int _DivideByWriteValue = 0;
        private int _MultiplyByWriteValue = 0;
        private bool _IsValueSigned = false;
        private bool _IsValueWholeNumber = false;
        private int _MaxThroughputValue = 0;

        private string _InFlowrateAddress = "DB0.DBD0";
        private Controller PLC_W;
        private bool bPercentageInput = false;


        //------------------------------------------------------------------------------//
        //                                 Constructor                                  //
        //------------------------------------------------------------------------------//

        public FlowbalancerInfo1()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//

        public string FlowbalancerInfo1_Title
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


        public string FlowbalancerInfo1_InFlowrateOffset
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

        public Controller FlowbalancerInfo1_WriteController
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

        public bool FlowbalancerInfo1_PercentageInput
        {
            get
            {
                return this.bPercentageInput;
            }
            set
            {
                this.bPercentageInput = value;
            }
        }

        public double FlowbalancerInfo_InFlowrateMinimum
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

        public double FlowbalancerInfo_InFlowrateMaximum
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

        public int FlowbalancerInfo_MultiplyByWriteValue
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

        public int FlowbalancerInfo_DivideByWriteValue
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

        public bool FlowbalancerInfo_IsValueSigned
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

        public bool FlowbalancerInfo_IsValueWholeNumber
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

        public int FlowbalancerInfo_MaxThroughPutValue
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

        //------------------------------------------------------------------------------//
        //                              Button Clicks                                   //
        //------------------------------------------------------------------------------//

        private void lblInFlowrate_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bool bOpen = Classes.StandardCode.IsSpecificWindowOpen(Application.Current.Windows, this.lblTitle.Content.ToString());

            if (!bOpen && (MainWindow.stat_iUserLevel >= 2))
            {
               // DisplayWindows.NumericKeypad numKeypad = new DisplayWindows.NumericKeypad(lblInFlowrate.Content.ToString(), FlowbalancerInfo1_Title, FlowbalancerInfo1_WriteController, FlowbalancerInfo1_InFlowrateOffset, FlowbalancerInfo1_PercentageInput);

                //numKeypad.NumericKeypad_MinLimit = FlowbalancerInfo_InFlowrateMinimum;
                //numKeypad.NumericKeypad_MaxLimit = FlowbalancerInfo_InFlowrateMaximum;

                //numKeypad.Show();

                LiteCoSControls.NumericKeypad numKeypad1 = new NumericKeypad(lblInFlowrate.Content.ToString());
                numKeypad1.NumericKeypad_Driver = NumericKeypad.eDrivers.INGEARS7;
                numKeypad1.NumericKeypad_TagDatatype = NumericKeypad.eTagDataTypes.DINT;
                numKeypad1.NumericKeypad_PLC_Write = PLC_W;
                numKeypad1.NumericKeypad_MinLimit = FlowbalancerInfo_InFlowrateMinimum;
                numKeypad1.NumericKeypad_MaxLimit = FlowbalancerInfo_InFlowrateMaximum;
                numKeypad1.NumericKeypad_Title = FlowbalancerInfo1_Title;
                numKeypad1.NumericKeypad_PrimaryDBOffset = FlowbalancerInfo1_InFlowrateOffset;
                numKeypad1.NumericKeypad_IsValueWholeNumber = FlowbalancerInfo_IsValueWholeNumber;
                numKeypad1.NumericKeypad_IsValueSigned = FlowbalancerInfo_IsValueSigned;
                numKeypad1.NumericKeypad_MultiplyWriteValue = FlowbalancerInfo_MultiplyByWriteValue;
                numKeypad1.NumericKeypad_DivideyWriteValue = FlowbalancerInfo_DivideByWriteValue;

                numKeypad1.Show();
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
