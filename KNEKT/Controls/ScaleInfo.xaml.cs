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
    /// Interaction logic for ScaleInfo.xaml
    /// </summary>
    public partial class ScaleInfo : UserControl
    {
        private double _InFlowrateMinimum = 0;
        private double _InFlowrateMaximum = 0;
        private int _MultiplyWriteValue = 0;
        private int _DivideWriteValue = 0;
        private bool _IsValueWholeNumber = true;
        private bool _IsValueSigned = false;


        private string _InFlowrateAddress = "DB0.DBD0";
        private Controller PLC_W;


        //------------------------------------------------------------------------------//
        //                                 Constructor                                  //
        //------------------------------------------------------------------------------//

        public ScaleInfo()
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

        public double ScaleInfo_InFlowrateMinimum
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

        public double ScaleInfo_InFlowrateMaximum
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

            if  (!bOpen && (MainWindow.stat_iUserLevel >= 2))
            {
                //DisplayWindows.NumericKeypad numKeypad = new DisplayWindows.NumericKeypad(lblInFlowrate.Content.ToString(), this.lblTitle.Content.ToString(), ScaleInfo_Controller_W, ScaleInfo_InFlowrateOffset);

                //numKeypad.NumericKeypad_MinLimit = ScaleInfo_InFlowrateMinimum;
                //numKeypad.NumericKeypad_MaxLimit = ScaleInfo_InFlowrateMaximum;

                //numKeypad.Show();


                LiteCoSControls.NumericKeypad numKeypad1 = new NumericKeypad(lblInFlowrate.Content.ToString());
                numKeypad1.NumericKeypad_Driver = NumericKeypad.eDrivers.INGEARS7;
                numKeypad1.NumericKeypad_TagDatatype = NumericKeypad.eTagDataTypes.DWORD;
                numKeypad1.NumericKeypad_PLC_Write = PLC_W;
                numKeypad1.NumericKeypad_MinLimit = ScaleInfo_InFlowrateMinimum;
                numKeypad1.NumericKeypad_MaxLimit = ScaleInfo_InFlowrateMaximum;
                numKeypad1.NumericKeypad_Title = lblTitle.Content.ToString();
                numKeypad1.NumericKeypad_PrimaryDBOffset = ScaleInfo_InFlowrateOffset;
                numKeypad1.NumericKeypad_IsValueWholeNumber = ScaleInfo_IsValueWholeNumber;
                numKeypad1.NumericKeypad_IsValueSigned = ScaleInfo_IsValueSigned;
                numKeypad1.NumericKeypad_MultiplyWriteValue = ScaleInfo_MultiplyWriteValue;
                numKeypad1.NumericKeypad_DivideyWriteValue = ScaleInfo_DivideWriteValue;

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
