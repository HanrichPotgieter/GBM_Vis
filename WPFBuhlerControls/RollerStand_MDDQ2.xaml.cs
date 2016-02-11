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
    /// Interaction logic for RollerStand_MDDQ2.xaml
    /// </summary>
    public partial class RollerStand_MDDQ2 : UserControl
    {
        private int _MotorColor1;
        private int _MotorColor2;
        private int _MotorColor3;
        private int _MotorColor4;
        private int _FeedRollerColor1;
        private int _FeedRollerColor2;
        private string _ObjectNo1;
        private string _ObjectNo2;
        private string _ObjectNo3;
        private string _ObjectNo4;
        private string _ObjectNo5;
        private string _ObjectNo6;

        private string _PLCName;

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

        private string DescriptionFeedRoller1;
        private string StatusFeedRoller1;
        private bool FaultFeedRoller1;

        private string DescriptionFeedRoller2;
        private string StatusFeedRoller2;
        private bool FaultFeedRoller2;

        public RollerStand_MDDQ2()
        {
            InitializeComponent();
            PolyTopMid.Fill = KNEKTColors.Gray;
            PolyTopInlet.Fill = KNEKTColors.Gray;
            PolyFeedRectangle1.Fill = KNEKTColors.Gray;
            PolyFeedRectangle2.Fill = KNEKTColors.Gray;
            PolySeperationLeft.Fill = KNEKTColors.Gray;
            PolySeperationRight.Fill = KNEKTColors.Gray;
            rectMidBot.Fill = KNEKTColors.Gray;
            rectMidBotInner.Fill = KNEKTColors.Gray;
            //rectMot.Fill = KNEKTColors.Gray;
            //rectMotRight.Fill = KNEKTColors.Gray;   
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
        //  Feed Roller
        //
        [Category("Buhler")]
        public int FeedRoller_Color1
        {
            get
            {
                return _FeedRollerColor1;
            }
            set
            {
                _FeedRollerColor1 = value;
                FB11 feedRollerColor1 = new FB11();
                _SetColorFeedRoller(feedRollerColor1.SetColor(value), 1);
                StatusFeedRoller1 = feedRollerColor1.Status_DO_Element;
                FaultFeedRoller1 = feedRollerColor1.Fault_DO_Element;
            }
        }

        [Category("Buhler")]
        public string Description_FeedRoller1
        {
            get
            {
                return this.DescriptionFeedRoller1;
            }
            set
            {
                this.DescriptionFeedRoller1 = value;
            }
        }

        [Category("Buhler")]
        public string Status_FeedRoller1
        {
            get
            {
                return this.StatusFeedRoller1;
            }
        }

        [Category("Buhler")]
        public bool Fault_FeedRoller1
        {
            get
            {
                return this.FaultFeedRoller1;
            }
        }

        [Category("Buhler")]
        public string ObjectNumber5
        {
            get
            {
                return this._ObjectNo5;
            }
            set
            {
                this._ObjectNo5 = value;
            }
        }

        [Category("Buhler")]
        public int FeedRoller_Color2
        {
            get
            {
                return _FeedRollerColor2;
            }
            set
            {
                _FeedRollerColor2 = value;
                FB11 feedRollerColor2 = new FB11();
                _SetColorFeedRoller(feedRollerColor2.SetColor(value), 2);
                StatusFeedRoller2 = feedRollerColor2.Status_DO_Element;
                FaultFeedRoller2 = feedRollerColor2.Fault_DO_Element;
            }
        }

        [Category("Buhler")]
        public string Description_FeedRoller2
        {
            get
            {
                return this.DescriptionFeedRoller2;
            }
            set
            {
                this.DescriptionFeedRoller2 = value;
            }
        }

        [Category("Buhler")]
        public string Status_FeedRoller2
        {
            get
            {
                return this.StatusFeedRoller2;
            }
        }

        [Category("Buhler")]
        public bool Fault_FeedRoller2
        {
            get
            {
                return this.FaultFeedRoller2;
            }
        }

        [Category("Buhler")]
        public string ObjectNumber6
        {
            get
            {
                return this._ObjectNo6;
            }
            set
            {
                this._ObjectNo6 = value;
            }
        }

        //
        //  MOTOR 1 (LEFT TOP)
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
        //  MOTOR 2 (RIGHT TOP)
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
        //  MOTOR 1 (LEFT BOTTOM)
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
        //  MOTOR 4 (RIGHT BOTTOM)
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
        //                                   Methods                                    //
        //------------------------------------------------------------------------------//
        private void _SetColorMotor(Brush brushColor, int MotorNum)
        {
            if (MotorNum == 1)
            {
                rectMotLeft1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    rectMotLeft1.Fill = brushColor;
                }));
            }
            else if (MotorNum == 2)
            {
                rectMotRight1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    rectMotRight1.Fill = brushColor;
                }));
            }
            else if (MotorNum == 3)
            {
                rectMotLeft2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    rectMotLeft2.Fill = brushColor;
                }));
            }
            else if (MotorNum == 4)
            {
                rectMotRight2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    rectMotRight2.Fill = brushColor;
                }));
            }
        }

        private void _SetColorFeedRoller(Brush brushColor, int FeedRollerNum)
        {
            if (FeedRollerNum == 1)
            {
                PolyFeedRoller1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    PolyFeedRoller1.Fill = brushColor;
                }));
            }
            else if (FeedRollerNum == 2)
            {
                PolyFeedRoller2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    PolyFeedRoller2.Fill = brushColor;
                }));
            }
        }
    }
}
