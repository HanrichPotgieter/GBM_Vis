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
    /// Interaction logic for RollerStand_MDDQ.xaml
    /// </summary>
    public partial class RollerStand_MDDQ : UserControl
    {
        private int _MotorColor1;
        private int _MotorColor2;
        private int _MotorColor3;
        private int _MotorColor4;
        private int _FrequencyColor1;
        private int _FrequencyColor2;
        private string _ObjectNo1;
        private string _ObjectNo2;
        private string _ObjectNo3;
        private string _ObjectNo4;

        private string DescriptionMotor1;
        private string StatusMotor1;
        private bool FaultMotor1;

        private string DescriptionMotor2;
        private string StatusMotor2;
        private bool FaultMotor2;

        private string DescriptionMotor3;
        private string StatusMotor3;
        private bool FaultMotor3;

        private string DescriptionMotor4;
        private string StatusMotor4;
        private bool FaultMotor4;

        private string DescriptionFrequencyConverter1;
        private string StatusFrequencyConverter1;
        private bool FaultFrequencyConverter1;

        private string DescriptionFrequencyConverter2;
        private string StatusFrequencyConverter2;
        private bool FaultFrequencyConverter2;

        private string _PLCName;


        public RollerStand_MDDQ()
        {
            InitializeComponent();

            PolyLeftMid.Fill = KNEKTColors.White;
            PolyRightMid.Fill = KNEKTColors.White;       
        }

        [Obsolete("SetMotorColor is deprecated , please use MotorColor1, MotorColor2, MotorColor3, MotorColor4 instead.")]
        public void SetMotorColor(int StateCode, int MotorNumber)
        {
            FB12 Motor = new FB12();
            _SetColorMotor(Motor.SetColor(StateCode), MotorNumber);

            if (MotorNumber == 1)
            {
                StatusMotor1 = Motor.Status_Motor;
                FaultMotor1 = Motor.Fault_Motor;
            }
            else if (MotorNumber == 2)
            {
                StatusMotor2 = Motor.Status_Motor;
                FaultMotor2 = Motor.Fault_Motor;
            }
            else if (MotorNumber == 3)
            {
                StatusMotor3 = Motor.Status_Motor;
                FaultMotor3 = Motor.Fault_Motor;
            }
            else if (MotorNumber == 4)
            {
                StatusMotor4 = Motor.Status_Motor;
                FaultMotor4 = Motor.Fault_Motor;
            }
        }


        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//

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

        [Category("Buhler")]
        public int MotorColor4
        {
            get
            {
                return _MotorColor4;
            }
            set
            {
                _MotorColor4 = value;
                FB12 Motor = new FB12();
                _SetColorMotor(Motor.SetColor(value), 4);
                StatusMotor4 = Motor.Status_Motor;
                FaultMotor4 = Motor.Fault_Motor;
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


        //
        //  FREQUENCY CONVERTER (LEFT)
        //
        [Category("Buhler")]
        public int FrequencyConverter_ColorSide1
        {
            get
            {
                return _FrequencyColor1;
            }
            set
            {
                _FrequencyColor1 = value;
                FB14 Converter = new FB14();
                _SetColorFreq1(Converter.SetColor(value, 7154));
                StatusFrequencyConverter1 = Converter.Status_DI_Status;
                FaultFrequencyConverter1 = Converter.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public string Description_FrequencyConverter1
        {
            get
            {
                return this.DescriptionFrequencyConverter1;
            }
            set
            {
                this.DescriptionFrequencyConverter1 = value;
            }
        }

        [Category("Buhler")]
        public string Status_FrequencyConverter1
        {
            get
            {
                return this.StatusFrequencyConverter1;
            }
        }

        [Category("Buhler")]
        public bool Fault_FrequencyConverter1
        {
            get
            {
                return this.FaultFrequencyConverter1;
            }
        }


        //
        //  FREQUENCY CONVERTER (RIGHT)
        //
        [Category("Buhler")]
        public int FrequencyConverter_ColorSide2
        {
            get
            {
                return _FrequencyColor2;
            }
            set
            {
                _FrequencyColor2 = value;
                FB14 Converter = new FB14();
                _SetColorFreq2(Converter.SetColor(value, 7154));
                StatusFrequencyConverter2 = Converter.Status_DI_Status;
                FaultFrequencyConverter2 = Converter.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public string Description_FrequencyConverter2
        {
            get
            {
                return this.DescriptionFrequencyConverter2;
            }
            set
            {
                this.DescriptionFrequencyConverter2 = value;
            }
        }

        [Category("Buhler")]
        public string Status_FrequencyConverter2
        {
            get
            {
                return this.StatusFrequencyConverter2;
            }
        }

        [Category("Buhler")]
        public bool Fault_FrequencyConverter2
        {
            get
            {
                return this.FaultFrequencyConverter2;
            }
        }


        //
        /// Top Left Motor
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
        // Top Right Motor
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
        // Bottom Left Motor
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



        //
        // Bottom Right Motor
        //
        [Category("Buhler")]
        public string Description_Motor4
        {
            get
            {
                return this.DescriptionMotor4;
            }
            set
            {
                this.DescriptionMotor4 = value;
            }
        }

        [Category("Buhler")]
        public string Status_Motor4
        {
            get
            {
                return this.StatusMotor4;
            }
        }

        [Category("Buhler")]
        public bool Fault_Motor4
        {
            get
            {
                return this.FaultMotor4;
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
        //                                      Methods                                 //
        //------------------------------------------------------------------------------//
        private void _SetColorMotor(Brush brushColor, int MotorNum)
        {
            if (MotorNum == 1)
            {
                rectTopLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    rectTopLeft.Fill = brushColor;
                }));
            }
            else if (MotorNum == 2)
            {
                rectBotLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    rectBotLeft.Fill = brushColor;
                }));
            }
            else if (MotorNum == 3)
            {
                rectTopRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    rectTopRight.Fill = brushColor;
                }));
            }
            else if (MotorNum == 4)
            {
                rectBotRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    rectBotRight.Fill = brushColor;
                }));
            }
        }

        private void _SetColorFreq1(Brush brushColor)
        {
            PolyLeftMid.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyLeftMid.Fill = brushColor;
            }));
        }

        private void _SetColorFreq2(Brush brushColor)
        {
            PolyRightMid.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                PolyRightMid.Fill = brushColor;
            }));
        }
    }
}
