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
    /// Interaction logic for PowerMill_MDGB2.xaml
    /// </summary>
    public partial class PowerMill_MDGB2 : UserControl
    {
        private int _MotorColorRollLeft;
        private int _MotorColorRollRight;
        private int _MotorColorDetacher;
        private int _MotorColorHydraulicPump;

        private string _ObjectNo1;
        private string _ObjectNo2;
        private string _ObjectNo3;
        private string _ObjectNo4;

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

        private string _PLCName;

        public PowerMill_MDGB2()
        {
            InitializeComponent();
        }

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
        public string ObjectNumber1
        {
            get
            {
                return this._ObjectNo1;
            }
            set
            {
                this._ObjectNo1 = value;
            }
        }

        [Category("Buhler")]
        public string ObjectNumber2
        {
            get
            {
                return this._ObjectNo2;
            }
            set
            {
                this._ObjectNo2 = value;
            }
        }

        [Category("Buhler")]
        public string ObjectNumber3
        {
            get
            {
                return this._ObjectNo3;
            }
            set
            {
                this._ObjectNo3 = value;
            }
        }

        [Category("Buhler")]
        public string ObjectNumber4
        {
            get
            {
                return this._ObjectNo4;
            }
            set
            {
                this._ObjectNo4 = value;
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
            PolyPump.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                PolyPump.Fill = brushColor;
            }));
        }

        private void _SetColorRollLeft(Brush brushColor)
        {
            EllipseLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                EllipseLeft.Fill = brushColor;
            }));
        }

        private void _SetColorRollRight(Brush brushColor)
        {
            EllipseRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                EllipseRight.Fill = brushColor;
            }));
        }

        private void _SetColorDetacher(Brush brushColor)
        {
            RectDetacher.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectDetacher.Fill = brushColor;
            }));

            EllipseDetacherLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                EllipseDetacherLeft.Fill = brushColor;
            }));

            EllipseDetacherRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                EllipseDetacherRight.Fill = brushColor;
            }));
        }
    }
}
