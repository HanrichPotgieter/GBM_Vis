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
using System.ComponentModel;
using WPFBuhlerControls.FB_Code;

namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for PowerMill_MDGB3.xaml
    /// </summary>
    public partial class PowerMill_MDGB3 : UserControl
    {
        #region PowerMill variables
        private int _MotorColorRollLeft;
        private int _MotorColorRollRight;
        private int _MotorColorDetacher;
        private int _MotorColorHydraulicPump;

        private string _ObjectNoRoll1;
        private string _ObjectNoRoll2;
        private string _ObjectNoDetacher1;
        private string _ObjectNoDetacher2;

        private string DescriptionLeftRoll;
        private string StatusLeftRoll;
        private bool FaultLeftRoll;

        private string DescriptionRightRoll;
        private string StatusRightRoll;
        private bool FaultRightRoll;

        private string DescriptionDetacher;
        private string StatusDetacher;
        private bool FaultDetacher;

        private string DescriptionHydraulicPump;
        private string StatusHydraulicPump;
        private bool FaultHydraulicPump;

        //
        //Engage
        //
        private int _EngageColor;
        private string DescriptionEngage;
        private string StatusEngage;
        private bool FaultEngage;
        private string _ObjectNoEngage;

        
        #endregion

        #region Pressure Switch variables
        
        private int _ColorPressureSwitch;
        private string DescriptionPressureSwitch;
        private string StatusPressureSwitch;
        private bool FaultPressureSwitch;
        private bool _IsMinPressureAlarm;
        private bool _IsHighPressureAlarm;
        private bool _IsAnalogPressureSwitch = true;

        private bool bHasPressureSwitch;

        FB16 Switch;

        #endregion

        #region High Level variables

        private int _LevelColorHL1;
        private string DescriptionHighLevel1;
        private string StatusHighLevel1;
        private bool FaultHighLevel1;
        private string _ObjectNoHL1;

        private int _LevelColorHL2;
        private string DescriptionHighLevel2;
        private string StatusHighLevel2;
        private bool FaultHighLevel2;
        private string _ObjectNoHL2;
        #endregion

        private string _PLCName;

        public PowerMill_MDGB3()
        {
            InitializeComponent();
            PolyFeedEngage.Fill = KNEKTColors.Gray;
            Switch = new FB16();
        }

        #region PowerMill Properties
        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int LeftRollColor
        {
            get
            {
                return _MotorColorRollLeft;
            }
            set
            {
                _MotorColorRollLeft = value;
                FB18 motor = new FB18();
                _SetColorRollLeft(motor.SetColor(value));
                StatusLeftRoll = motor.Status_VSD;
                FaultLeftRoll = motor.Fault_VSD;
            }
        }

        [Category("Buhler")]
        public int RightRollColor
        {
            get
            {
                return _MotorColorRollRight;
            }
            set
            {
                _MotorColorRollRight = value;
                FB18 motor = new FB18();
                _SetColorRollRight(motor.SetColor(value));
                StatusRightRoll = motor.Status_VSD;
                FaultRightRoll = motor.Fault_VSD;
            }
        }

        [Category("Buhler")]
        public int DetacherColor
        {
            get
            {
                return _MotorColorDetacher;
            }
            set
            {
                _MotorColorDetacher = value;
                FB18 motor = new FB18();
                _SetColorDetacher(motor.SetColor(value));
                StatusDetacher = motor.Status_VSD;
                FaultDetacher = motor.Fault_VSD;
            }
        }

        [Category("Buhler")]
        public int HydraulicPumpColor
        {
            get
            {
                return _MotorColorHydraulicPump;
            }
            set
            {
                _MotorColorHydraulicPump = value;
                FB18 motor = new FB18();
                _SetColorHydraulicPump(motor.SetColor(value));
                StatusHydraulicPump = motor.Status_VSD;
                FaultHydraulicPump = motor.Fault_VSD;
            }
        }


        #region  Left Roll

        [Category("Buhler")]
        public string Description_LeftRoll
        {
            get
            {
                return this.DescriptionLeftRoll;
            }
            set
            {
                this.DescriptionLeftRoll = value;
            }
        }

        [Category("Buhler")]
        public string Status_LeftRoll
        {
            get
            {
                return this.StatusLeftRoll;
            }
        }

        [Category("Buhler")]
        public bool Fault_LeftRoll
        {
            get
            {
                return this.FaultLeftRoll;
            }
        }
        #endregion

        #region  Right Roll

        [Category("Buhler")]
        public string Description_RightRoll
        {
            get
            {
                return this.DescriptionRightRoll;
            }
            set
            {
                this.DescriptionRightRoll = value;
            }
        }

        [Category("Buhler")]
        public string Status_RightRoll
        {
            get
            {
                return this.StatusRightRoll;
            }
        }

        [Category("Buhler")]
        public bool Fault_RightRoll
        {
            get
            {
                return this.FaultRightRoll;
            }
        }
        #endregion

        #region  Detacher

        [Category("Buhler")]
        public string Description_Detacher
        {
            get
            {
                return this.DescriptionDetacher;
            }
            set
            {
                this.DescriptionDetacher = value;
            }
        }

        [Category("Buhler")]
        public string Status_Detacher
        {
            get
            {
                return this.StatusDetacher;
            }
        }

        [Category("Buhler")]
        public bool Fault_Detacher
        {
            get
            {
                return this.FaultDetacher;
            }
        }
        #endregion

        #region  Hydraulic Pump

        [Category("Buhler")]
        public string Description_HydraulicPump
        {
            get
            {
                return this.DescriptionHydraulicPump;
            }
            set
            {
                this.DescriptionHydraulicPump = value;
            }
        }

        [Category("Buhler")]
        public string Status_HydraulicPump
        {
            get
            {
                return this.StatusHydraulicPump;
            }
        }

        [Category("Buhler")]
        public bool Fault_HydraulicPump
        {
            get
            {
                return this.FaultHydraulicPump;
            }
        }
        #endregion

        [Category("Buhler")]
        public string ObjectNumberRoll1
        {
            get
            {
                return this._ObjectNoRoll1;
            }
            set
            {
                this._ObjectNoRoll1 = value;
            }
        }

        [Category("Buhler")]
        public string ObjectNumberRoll2
        {
            get
            {
                return this._ObjectNoRoll2;
            }
            set
            {
                this._ObjectNoRoll2 = value;
            }
        }

        [Category("Buhler")]
        public string ObjectNumberDetacher1
        {
            get
            {
                return this._ObjectNoDetacher1;
            }
            set
            {
                this._ObjectNoDetacher1 = value;
            }
        }

        [Category("Buhler")]
        public string ObjectNumberDetacher2
        {
            get
            {
                return this._ObjectNoDetacher2;
            }
            set
            {
                this._ObjectNoDetacher2 = value;
            }
        }

        [Category("Buhler")]
        public string PLCName
        {
            get
            {
                return this._PLCName;
            }
            set
            {
                this._PLCName = value;
            }
        }

        //------------------------------------------------------------------------------//
        //                                   Methods                                    //
        //------------------------------------------------------------------------------//
        private void _SetColorHydraulicPump(Brush brushColor)
        {
            elipseMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                elipseMain.Fill = brushColor;
            }));
        }

        private void _SetColorRollLeft(Brush brushColor)
        {
            EllipseLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                EllipseLeft.Fill = brushColor;
            }));
        }

        private void _SetColorRollRight(Brush brushColor)
        {
            EllipseRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                EllipseRight.Fill = brushColor;
            }));
        }

        private void _SetColorDetacher(Brush brushColor)
        {
            EllipseDetacherLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                EllipseDetacherLeft.Fill = brushColor;
            }));

            EllipseDetacherRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                EllipseDetacherRight.Fill = brushColor;
            }));
        }

        //
        //Engage
        //

        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int EngageColor
        {
            get
            {
                return _EngageColor;
            }
            set
            {
                _EngageColor = value;
                FB837 engage = new FB837();
                _SetColorEngage(engage.SetColor(value));
                StatusEngage = engage.Status_MEAG;
                FaultEngage = engage.Fault_MEAG;
            }


        }

        [Category("Buhler")]
        public string ObjectNumberEngage
        {
            get
            {
                return this._ObjectNoEngage;
            }
            set
            {
                this._ObjectNoEngage = value;
            }
        }

        [Category("Buhler")]
        public string Description_Engage
        {
            get
            {
                return this.DescriptionEngage;
            }
            set
            {
                this.DescriptionEngage = value;
            }
        }

        [Category("Buhler")]
        public string Status_Engage
        {
            get
            {
                return this.StatusEngage;
            }
        }

        [Category("Buhler")]
        public bool Fault_Engage
        {
            get
            {
                return this.FaultEngage;
            }
        }

        //------------------------------------------------------------------------------//
        //                                  Methods                                     //
        //------------------------------------------------------------------------------//
        private void _SetColorEngage(Brush brushColor)
        {
            PolyFeedEngage.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyFeedEngage.Fill = brushColor;
            }));
        }

        #endregion

        #region Pressure Switch variables
        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int PressureSwitchColor
        {
            get
            {
                return _ColorPressureSwitch;
            }
            set
            {
                _ColorPressureSwitch = value;
                if (PressureSwitch_IsAnalogPressureSwitch)
                {
                    Switch.IsMinimumPressureAlarm = IsMinimumPressureAlarm;
                    _SetColorPressureSwitch(Switch.SetColor(value));
                    StatusPressureSwitch = Switch.Status_PressureSwitch;
                    FaultPressureSwitch = Switch.Fault_PressureSwitch;
                }
            }
        }

        [Category("Buhler")]
        public string Description_PressureSwitch
        {
            get
            {
                return this.DescriptionPressureSwitch;
            }
            set
            {
                this.DescriptionPressureSwitch = value;
            }
        }

        /// <summary>
        /// If true, Pressure switch will be Red when in Minimum pressure
        /// </summary>
        [Category("Buhler")]
        public bool IsMinimumPressureAlarm
        {
            get
            {
                return this._IsMinPressureAlarm;
            }
            set
            {
                this._IsMinPressureAlarm = value;
                Switch.IsMinimumPressureAlarm = value;
            }
        }

        /// <summary>
        /// If true, Pressure switch will be Red when in Minimum pressure
        /// </summary>
        [Category("Buhler")]
        public bool IsHighPressureAlarm
        {
            get
            {
                return this._IsHighPressureAlarm;
            }
            set
            {
                this._IsHighPressureAlarm = value;
                Switch.IsHighPressureAlarm = value;
            }
        }

        [Category("Buhler")]
        public string Status_PressureSwitch
        {
            get
            {
                return this.StatusPressureSwitch;
            }
        }

        [Category("Buhler")]
        public bool Fault_PressureSwitch
        {
            get
            {
                return this.FaultPressureSwitch;
            }
        }

        [Category("Buhler")]
        public bool PressureSwitch_IsAnalogPressureSwitch
        {
            get
            {
                return this._IsAnalogPressureSwitch;
            }
            set
            {
                _IsAnalogPressureSwitch = value;
            }
        }

        [Category("Buhler")]
        public bool PowerMill_HasPressureSwitch
        {
            get
            {
                return this.bHasPressureSwitch;
            }
            set
            {
                this.bHasPressureSwitch = value;

                if (value)
                {
                    elipsePS.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        elipsePS.Visibility = Visibility.Visible;
                    }));

                    polyP.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        polyP.Visibility = Visibility.Visible;
                    }));

                    polyS.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        polyS.Visibility = Visibility.Visible;
                    }));
                }
                else
                {
                    elipsePS.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        elipsePS.Visibility = Visibility.Hidden;
                    }));

                    polyP.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        polyP.Visibility = Visibility.Hidden;
                    }));

                    polyS.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        polyS.Visibility = Visibility.Hidden;
                    }));
                }
            }
        }


        //------------------------------------------------------------------------------//
        //                                   Methods                                    //
        //------------------------------------------------------------------------------//
        private void _SetColorPressureSwitch(Brush brushColor)
        {
            elipsePS.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                elipsePS.Fill = brushColor;
            }));
        }

        #endregion 

        #region High Levels
        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int LevelColorHL1
        {
            get
            {
                return _LevelColorHL1;
            }
            set
            {
                _LevelColorHL1 = value;
                FB14 HL = new FB14();
                _SetColorHL1(HL.SetColor(value, 7165));
                StatusHighLevel1 = HL.Status_DI_Status;
                FaultHighLevel1 = HL.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public string Description_HighLevel1
        {
            get
            {
                return this.DescriptionHighLevel1;
            }
            set
            {
                this.DescriptionHighLevel1 = value;
            }
        }

        [Category("Buhler")]
        public string Status_HighLevel1
        {
            get
            {
                return this.StatusHighLevel1;
            }
        }

        [Category("Buhler")]
        public bool Fault_HighLevel1
        {
            get
            {
                return this.FaultHighLevel1;
            }
        }

        [Category("Buhler")]
        public string ObjectNumberHL1
        {
            get
            {
                return this._ObjectNoHL1;
            }
            set
            {
                this._ObjectNoHL1 = value;
            }
        }

        [Category("Buhler")]
        public int LevelColorHL2
        {
            get
            {
                return _LevelColorHL2;
            }
            set
            {
                _LevelColorHL2 = value;
                FB14 HL = new FB14();
                _SetColorHL2(HL.SetColor(value, 7165));
                StatusHighLevel2 = HL.Status_DI_Status;
                FaultHighLevel2 = HL.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public string Description_HighLevel2
        {
            get
            {
                return this.DescriptionHighLevel2;
            }
            set
            {
                this.DescriptionHighLevel2 = value;
            }
        }

        [Category("Buhler")]
        public string Status_HighLevel2
        {
            get
            {
                return this.StatusHighLevel2;
            }
        }

        [Category("Buhler")]
        public bool Fault_HighLevel2
        {
            get
            {
                return this.FaultHighLevel2;
            }
        }

        [Category("Buhler")]
        public string ObjectNumberHL2
        {
            get
            {
                return this._ObjectNoHL2;
            }
            set
            {
                this._ObjectNoHL2 = value;
            }
        }

        //------------------------------------------------------------------------------//
        //                                  Methods                                     //
        //------------------------------------------------------------------------------//
        private void _SetColorHL1(Brush brushColor)
        {
            RectLevel1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectLevel1.Fill = brushColor;
            }));
        }

        private void _SetColorHL2(Brush brushColor)
        {
            RectLevel2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectLevel2.Fill = brushColor;
            }));
        }
        #endregion


    }
}
