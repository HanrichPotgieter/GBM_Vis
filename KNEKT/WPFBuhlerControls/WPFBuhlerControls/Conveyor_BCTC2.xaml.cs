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
    /// Interaction logic for Conveyor_BCTC2.xaml
    /// </summary>
    public partial class Conveyor_BCTC2 : UserControl
    {
        private int _MotorColor1;
        private int _MotorColor2;
        private int _MotorColor3;

        private string DescriptionMotor1;
        private string StatusMotor1;
        private bool FaultMotor1;
        private string _ObjectNo1;

        private string DescriptionMotor2;
        private string StatusMotor2;
        private bool FaultMotor2;
        private string _ObjectNo2;

        private string DescriptionMotor3;
        private string StatusMotor3;
        private bool FaultMotor3;
        private string _ObjectNo3;

        private string _PLCName;


        public Conveyor_BCTC2()
        {
            InitializeComponent();
        }

        [Category("Buhler")]
        public int MotorColor1
        {
            get
            {
                return _MotorColor1;
            }
            set
            {
                _MotorColor1 = value;
                FB12 Motor = new FB12();
                _SetColorMotor(Motor.SetColor(value), 1);
                StatusMotor1 = Motor.Status_Motor;
                FaultMotor1 = Motor.Fault_Motor;
            }
        }

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
        public int MotorColor2
        {
            get
            {
                return _MotorColor2;
            }
            set
            {
                _MotorColor2 = value;
                FB12 Motor = new FB12();
                _SetColorMotor(Motor.SetColor(value), 2);
                StatusMotor2 = Motor.Status_Motor;
                FaultMotor2 = Motor.Fault_Motor;
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
        public int MotorColor3
        {
            get
            {
                return _MotorColor3;
            }
            set
            {
                _MotorColor3 = value;
                FB12 Motor = new FB12();
                _SetColorMotor(Motor.SetColor(value), 3);
                StatusMotor3 = Motor.Status_Motor;
                FaultMotor3 = Motor.Fault_Motor;
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

        //
        //  MOTOR 1 (TOP)
        //
        [Category("Buhler")]
        public string Description_Motor1
        {
            get
            {
                return this.DescriptionMotor1;
            }
            set
            {
                this.DescriptionMotor1 = value;
            }
        }

        [Category("Buhler")]
        public string Status_Motor1
        {
            get
            {
                return this.StatusMotor1;
            }
        }

        [Category("Buhler")]
        public bool Fault_Motor1
        {
            get
            {
                return this.FaultMotor1;
            }
        }


        //
        //  MOTOR 2 (MIDDLE)
        //
        [Category("Buhler")]
        public string Description_Motor2
        {
            get
            {
                return this.DescriptionMotor2;
            }
            set
            {
                this.DescriptionMotor2 = value;
            }
        }

        [Category("Buhler")]
        public string Status_Motor2
        {
            get
            {
                return this.StatusMotor2;
            }
        }

        [Category("Buhler")]
        public bool Fault_Motor2
        {
            get
            {
                return this.FaultMotor2;
            }
        }

        //
        //  MOTOR 3 (BOTTOM)
        //
        [Category("Buhler")]
        public string Description_Motor3
        {
            get
            {
                return this.DescriptionMotor3;
            }
            set
            {
                this.DescriptionMotor3 = value;
            }
        }

        [Category("Buhler")]
        public string Status_Motor3
        {
            get
            {
                return this.StatusMotor3;
            }
        }

        [Category("Buhler")]
        public bool Fault_Motor3
        {
            get
            {
                return this.FaultMotor3;
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
        //                                  Methods                                     //
        //------------------------------------------------------------------------------//
        private void _SetColorMotor(Brush brushColor, int MotorNum)
        {
            if (MotorNum == 1)
            {
                PolyMotor1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    PolyMotor1.Fill = brushColor;
                }));
            }
            else if (MotorNum == 2)
            {
                PolyMotor2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    PolyMotor2.Fill = brushColor;
                }));
            }
            else if (MotorNum == 3)
            {
                PolyMotor3.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    PolyMotor3.Fill = brushColor;
                }));
            }
        }
    }
}
