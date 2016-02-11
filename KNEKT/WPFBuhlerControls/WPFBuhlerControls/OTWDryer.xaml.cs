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
using WPFBuhlerControls.FB_Code;
using System.ComponentModel;

namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for OTWDryer.xaml
    /// </summary>
    public partial class OTWDryer : UserControl
    {
        private int _PulsatorMotorColor;
        private string DescriptionPulsatorMotor;
        private string StatusPulsatorMotor;
        private bool FaultPulsatorMotor;
        private string _ObjectNo1;

        private int _BeltMotorColor;
        private string DescriptionBeltMotor;
        private string StatusBeltMotor;
        private bool FaultBeltMotor;
        private string _ObjectNo2;

        private string _PLCName;

        public OTWDryer()
        {
            InitializeComponent();
        }



        [Category("Buhler")]
        public int PulsatorMotorColor
        {
            get
            {
                return _PulsatorMotorColor;
            }
            set
            {
                _PulsatorMotorColor = value;
                FB12 motor = new FB12();
                _SetColor(motor.SetColor(value), 1);
                StatusPulsatorMotor = motor.Status_Motor;
                FaultPulsatorMotor = motor.Fault_Motor;
            }
        }

        [Category("Buhler")]
        public string Description_PulsatorMotor
        {
            get
            {
                return this.DescriptionPulsatorMotor;
            }
            set
            {
                this.DescriptionPulsatorMotor = value;
            }
        }

        [Category("Buhler")]
        public string Status_PulsatorMotor
        {
            get
            {
                return this.StatusPulsatorMotor;
            }
        }

        [Category("Buhler")]
        public bool Fault_PulsatorMotor
        {
            get
            {
                return this.FaultPulsatorMotor;
            }
        }


        [Category("Buhler")]
        public int BeltMotorColor
        {
            get
            {
                return _BeltMotorColor;
            }
            set
            {
                _BeltMotorColor = value;
                FB12 motor = new FB12();
                _SetColor(motor.SetColor(value), 2);
                StatusBeltMotor = motor.Status_Motor;
                FaultBeltMotor = motor.Fault_Motor;
            }
        }

        [Category("Buhler")]
        public string Description_BeltMotor
        {
            get
            {
                return this.DescriptionBeltMotor;
            }
            set
            {
                this.DescriptionBeltMotor = value;
            }
        }

        [Category("Buhler")]
        public string Status_BeltMotor
        {
            get
            {
                return this.StatusBeltMotor;
            }
        }

        [Category("Buhler")]
        public bool Fault_BeltMotor
        {
            get
            {
                return this.FaultBeltMotor;
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
        //                                    Methods                                   //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor, int MotorNum)
        {
            if (MotorNum == 1)
            {
                polyBelt.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyBelt.Fill = brushColor;
                }));

                ellipseLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    ellipseLeft.Fill = brushColor;
                }));

                ellipseRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    ellipseRight.Fill = brushColor;
                }));
            }
            else if (MotorNum == 2)
            {
                polyMotorBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyMotorBot.Fill = brushColor;
                }));
            }                       
        }
    }
}
