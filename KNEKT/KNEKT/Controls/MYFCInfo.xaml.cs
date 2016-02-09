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
    /// Interaction logic for MYFCInfo.xaml
    /// </summary>
    public partial class MYFCInfo : UserControl
    {
        private int _InMoistureMinimum = 0;
        private int _InMoistureMaximum = 100;
        private int _OffsetMoistureMinimum = 0;
        private int _OffsetMoistureMaximum = 100;
        private int _MultiplyByWriteValueOffset = 0;
        private int _MultiplyByWriteValueInOffset = 0;
        private int _DivideByWriteValueOffset = 0;
        private int _DivideByWriteValueInOffset = 0;
        private bool _TargetMoistureSigned = false;
        private bool _OffsetMoistureSigned = false;


        private string _TargetMoistureOffset = "DB0.DBD0";
        private string _InOffsetMoisture = "DB0.DBD0";
        private Controller PLC_W;

        //DisplayWindows.NumericKeypad numKeypad1;
        //DisplayWindows.NumericKeypad numKeypad2;

        //------------------------------------------------------------------------------//
        //                                 Constructor                                  //
        //------------------------------------------------------------------------------//

        public MYFCInfo()
        {
            InitializeComponent();
        }


        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//

        public string MYFCInfo_Title
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

        public bool MYFCInfo_TargetMoistureSigned
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

        public bool MYFCInfo_OffsetMoistureSigned
        {
            get
            {
                return this._OffsetMoistureSigned;
            }
            set
            {
                this._OffsetMoistureSigned = value;
            }
        }


        public string MYFCInfo_TargetMoistureOffset
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

        public string MYFCInfo_InOffsetMoisture
        {
            get
            {
                return this._InOffsetMoisture;
            }
            set
            {
                this._InOffsetMoisture = value;
            }
        }

        public Controller MYFCInfo_WriteController
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

        public int MYFCInfo_InMoistureMinimum
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

        public int MYFCInfo_InMoistureMaximum
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

        public int MYFCInfo_OffsetMoistureMinimum
        {
            get
            {
                return _OffsetMoistureMinimum;
            }
            set
            {
                _OffsetMoistureMinimum = value;
            }
        }

        public int MYFCInfo_OffsetMoistureMaximum
        {
            get
            {
                return _OffsetMoistureMaximum;
            }
            set
            {
                _OffsetMoistureMaximum = value;
            }
        }

        public int MYFCInfo_MultiplyWriteValueOffset
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

        public int MYFCInfo_MultiplyWriteValueInOffset
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

        public int MYFCInfo_DivideByWriteValueOffset
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

        public int MYFCInfo_DivideWriteValueInOffset
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


        //------------------------------------------------------------------------------//
        //                              Button Clicks                                   //
        //------------------------------------------------------------------------------//

        private void lblTargeMoisture_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bool bOpen = Classes.StandardCode.IsSpecificWindowOpen(Application.Current.Windows,this.lblTitle.Content.ToString() + " Target");

            if (!bOpen && (MainWindow.stat_iUserLevel >= 2))
            {
                //numKeypad1 = new NumericKeypad(lblTargetMoisture.Content.ToString(), this.lblTitle.Content.ToString() + " Target", MYFCInfo_WriteController, MYFCInfo_TargetMoistureOffset, true);
                LiteCoSControls.NumericKeypad numKeypad1 = new NumericKeypad(lblTargetMoisture.Content.ToString());
                numKeypad1.NumericKeypad_Driver = NumericKeypad.eDrivers.INGEARS7;
                numKeypad1.NumericKeypad_TagDatatype = NumericKeypad.eTagDataTypes.DINT;
                numKeypad1.NumericKeypad_PLC_Write = MYFCInfo_WriteController;
                numKeypad1.NumericKeypad_MinLimit = MYFCInfo_InMoistureMinimum;
                numKeypad1.NumericKeypad_MaxLimit = MYFCInfo_InMoistureMaximum;
                numKeypad1.NumericKeypad_Title = MYFCInfo_Title;
                numKeypad1.NumericKeypad_PrimaryDBOffset = MYFCInfo_TargetMoistureOffset; //"DB151.DBD506"
                numKeypad1.NumericKeypad_IsValueWholeNumber = false;
                numKeypad1.NumericKeypad_IsValueSigned = MYFCInfo_TargetMoistureSigned;
                numKeypad1.NumericKeypad_MultiplyWriteValue = MYFCInfo_MultiplyWriteValueOffset;
                numKeypad1.NumericKeypad_DivideyWriteValue = MYFCInfo_DivideByWriteValueOffset;

                numKeypad1.Show();
            }
        }

        private void lblOffsetMoisture_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bool bOpen = Classes.StandardCode.IsSpecificWindowOpen(Application.Current.Windows, this.lblTitle.Content.ToString() + " Offset");

            if (!bOpen && (MainWindow.stat_iUserLevel >= 2))
            {


                LiteCoSControls.NumericKeypad numKeypad2 = new NumericKeypad(lblOffsetMoisture.Content.ToString());
                numKeypad2.NumericKeypad_Driver = NumericKeypad.eDrivers.INGEARS7;
                numKeypad2.NumericKeypad_TagDatatype = NumericKeypad.eTagDataTypes.INT;
                numKeypad2.NumericKeypad_PLC_Write = MYFCInfo_WriteController;
                numKeypad2.NumericKeypad_MinLimit = MYFCInfo_InMoistureMinimum;
                numKeypad2.NumericKeypad_MaxLimit = MYFCInfo_InMoistureMaximum;
                numKeypad2.NumericKeypad_Title = MYFCInfo_Title;
                numKeypad2.NumericKeypad_PrimaryDBOffset = MYFCInfo_InOffsetMoisture; //"DB151.DBD510"
                numKeypad2.NumericKeypad_IsValueWholeNumber = false;
                numKeypad2.NumericKeypad_IsValueSigned = MYFCInfo_OffsetMoistureSigned;
                numKeypad2.NumericKeypad_MultiplyWriteValue = 10; //MYFCInfo_MultiplyWriteValueInOffset;
                numKeypad2.NumericKeypad_DivideyWriteValue = MYFCInfo_DivideWriteValueInOffset;

                numKeypad2.Show();
            }
        }

        private void lblAlarmMYFC_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Classes.MEAG m1 = new Classes.MEAG();
            m1.MEAG_AlarmCode = Convert.ToInt32(lblAlarmMYFC.Content);
            MainWindow.sElementDescription = m1.MEAG_AlarmDescritpion;
            MainWindow.sElementState = m1.MEAG_AlarmCodeDescription;
        }

        private void lblAlarmMOZF_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Classes.MEAG m1 = new Classes.MEAG();
            m1.MEAG_AlarmCode = Convert.ToInt32(lblAlarmMOZF.Content);
            MainWindow.sElementDescription = m1.MEAG_AlarmDescritpion;
            MainWindow.sElementState = m1.MEAG_AlarmCodeDescription;
            //MessageBox.Show(m1.MEAG_AlarmCode + " - " + m1.MEAG_AlarmDescritpion, "MEAG Alarm Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
