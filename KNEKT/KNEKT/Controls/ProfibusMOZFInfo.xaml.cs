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
    /// Interaction logic for ProfibusMOZFInfo.xaml
    /// </summary>
    public partial class ProfibusMOZFInfo : UserControl
    {
        private int _InMoistureMinimum = 0;
        private int _InMoistureMaximum = 100;
        private bool _TargetMoistureSigned = false;
        private int _MultiplyByWriteValueOffset = 0;
        private int _MultiplyByWriteValueInOffset = 0;
        private int _DivideByWriteValueOffset = 0;
        private int _DivideByWriteValueInOffset = 0;
        private string _TargetMoistureOffset = "DB0.DBD0";
        private string _SecondaryTargetMoistureOffset = "DB0.DBD0";
        private Controller PLC_W;

        //------------------------------------------------------------------------------//
        //                                 Constructor                                  //
        //------------------------------------------------------------------------------//

        public ProfibusMOZFInfo()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//

        public string ProfibusMOZFInfo_Title
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

        public bool ProfibusMOZFInfo_TargetMoistureSigned
        {
            get
            {
                return this._TargetMoistureSigned;
            }
            set
            {
                this._TargetMoistureSigned = value;
            }
        }

        public string ProfibusMOZFInfo_TargetMoistureOffset
        {
            get
            {
                return this._TargetMoistureOffset;
            }
            set
            {
                this._TargetMoistureOffset = value;
            }
        }

        public Controller ProfibusMOZFInfo_WriteController
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

        public int ProfibusMOZFInfo_InMoistureMinimum
        {
            get
            {
                return _InMoistureMinimum;
            }
            set
            {
                _InMoistureMinimum = value;
            }
        }

        public int ProfibusMOZFInfo_InMoistureMaximum
        {
            get
            {
                return _InMoistureMaximum;
            }
            set
            {
                _InMoistureMaximum = value;
            }
        }

        public int ProfibusMOZFInfo_MultiplyWriteValueOffset
        {
            get
            {
                return _MultiplyByWriteValueOffset;
            }
            set
            {
                _MultiplyByWriteValueOffset = value;
            }
        }

        public int ProfibusMOZFInfo_MultiplyWriteValueInOffset
        {
            get
            {
                return _MultiplyByWriteValueInOffset;
            }
            set
            {
                _MultiplyByWriteValueInOffset = value;
            }
        }

        public int ProfibusMOZFInfo_DivideByWriteValueOffset
        {
            get
            {
                return _DivideByWriteValueOffset;
            }
            set
            {
                _DivideByWriteValueOffset = value;
            }
        }

        public int ProfibusMOZFInfo_DivideWriteValueInOffset
        {
            get
            {
                return _DivideByWriteValueInOffset;
            }
            set
            {
                _DivideByWriteValueInOffset = value;
            }
        }


        public string ProfibusMOZFInfo_SecondaryTargetMoistureOffset
        {
            get
            {
                return this._SecondaryTargetMoistureOffset;
            }
            set
            {
                this._SecondaryTargetMoistureOffset = value;
            }
        }

        //"DB2000.DBW10"
        //------------------------------------------------------------------------------//
        //                              Button Clicks                                   //
        //------------------------------------------------------------------------------//

        private void lblTargeMoisture_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bool bOpen = Classes.StandardCode.IsSpecificWindowOpen(Application.Current.Windows, this.lblTitle.Content.ToString() + " Target");

            if (!bOpen && (MainWindow.stat_iUserLevel >= 2))
            {
                LiteCoSControls.NumericKeypad numKeypad1 = new NumericKeypad(lblTargetMoisture.Content.ToString());
                numKeypad1.NumericKeypad_Driver = NumericKeypad.eDrivers.INGEARS7;
                numKeypad1.NumericKeypad_TagDatatype = NumericKeypad.eTagDataTypes.DINT;
                numKeypad1.NumericKeypad_PLC_Write = ProfibusMOZFInfo_WriteController;
                numKeypad1.NumericKeypad_MinLimit = ProfibusMOZFInfo_InMoistureMinimum;
                numKeypad1.NumericKeypad_MaxLimit = ProfibusMOZFInfo_InMoistureMaximum;
                numKeypad1.NumericKeypad_Title = ProfibusMOZFInfo_Title;
                numKeypad1.NumericKeypad_PrimaryDBOffset = ProfibusMOZFInfo_TargetMoistureOffset; //"DB151.DBD506"
                numKeypad1.NumericKeypad_SecondaryDBOffset = ProfibusMOZFInfo_SecondaryTargetMoistureOffset;
                numKeypad1.NumericKeypad_IsValueWholeNumber = false;
                numKeypad1.NumericKeypad_IsValueSigned = ProfibusMOZFInfo_TargetMoistureSigned;
                numKeypad1.NumericKeypad_MultiplyWriteValue = ProfibusMOZFInfo_MultiplyWriteValueOffset;
                numKeypad1.NumericKeypad_DivideyWriteValue = ProfibusMOZFInfo_DivideByWriteValueOffset;

                numKeypad1.Show();
            }
        }

        private void lblAlarmMOZF_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Classes.MEAG m1 = new Classes.MEAG();
            m1.MEAG_AlarmCode = Convert.ToInt32(lblAlarmMOZF.Content);
            MainWindow.sElementDescription = m1.MEAG_AlarmDescritpion;
            MainWindow.sElementState = m1.MEAG_AlarmCodeDescription;
       }
       
    }
}
