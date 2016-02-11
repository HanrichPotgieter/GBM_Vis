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
    /// Interaction logic for RollerStand_MDDP.xaml
    /// </summary>
    public partial class RollerStand_MDDP : UserControl
    {
        private int _MotorColor1;
        private int _MotorColor2;
        private int _FeedRoller1Color;
        private int _FeedRoller2Color;
        private string _ObjectNo1;
        private string _ObjectNo2;
        private string _ObjectNo3;
        private string _ObjectNo4;

        private string _PLCName;

        private string DescriptionMotor1;
        private string StatusMotor1;
        private bool FaultMotor1;

        private string DescriptionMotor2;
        private string StatusMotor2;
        private bool FaultMotor2;

        private string DescriptionFeedRoller1;
        private string StatusFeedRoller1;
        private bool FaultFeedRoller1;

        private string DescriptionFeedRoller2;
        private string StatusFeedRoller2;
        private bool FaultFeedRoller2;

        public RollerStand_MDDP()
        {
            InitializeComponent();
            PolyTopMid.Fill = KNEKTColors.Gray;
            PolyTopInlet.Fill = KNEKTColors.Gray;
            PolyFeedRectangle.Fill = KNEKTColors.Gray;
            PolySeperationLeft.Fill = KNEKTColors.Gray;
            PolySeperationRight.Fill = KNEKTColors.Gray;
            rectMidMid1.Fill = KNEKTColors.Gray;
            rectMidMid2.Fill = KNEKTColors.Gray;
            rectMidBot.Fill = KNEKTColors.Gray;
            rectMotLeftTop.Fill = KNEKTColors.Gray;
            rectMotRightTop.Fill = KNEKTColors.Gray;   
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


        //
        //  Feed Roller
        //


        [Category("Buhler")]
        public int FeedRoller1_Color
        {
            get
            {
                return _FeedRoller1Color;
            }
            set
            {
                _FeedRoller1Color = value;
                FB11 feedRoller1Color = new FB11();
                _SetColorFeedRoller(feedRoller1Color.SetColor(value), 1);
                StatusFeedRoller1 = feedRoller1Color.Status_DO_Element;
                FaultFeedRoller1 = feedRoller1Color.Fault_DO_Element;
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
        public int FeedRoller2_Color
        {
            get
            {
                return _FeedRoller2Color;
            }
            set
            {
                _FeedRoller2Color = value;
                FB11 feedRoller2Color = new FB11();
                _SetColorFeedRoller(feedRoller2Color.SetColor(value), 2);
                StatusFeedRoller2 = feedRoller2Color.Status_DO_Element;
                FaultFeedRoller2 = feedRoller2Color.Fault_DO_Element;
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


        //
        //  MOTOR 1 (LEFT)
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
        //  MOTOR 2 (RIGHT)
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
                rectMotLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    rectMotLeft.Fill = brushColor;
                }));
            }
            else if (MotorNum == 2)
            {
                rectMotRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    rectMotRight.Fill = brushColor;
                }));
            }
        }

        private void _SetColorFeedRoller(Brush brushColor, int feedRollerNum)
        {
            if (feedRollerNum == 1)
            {
                PolyFeedRollerMot1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    PolyFeedRollerMot1.Fill = brushColor;
                }));
            }
            else if (feedRollerNum == 2)
            {
                PolyFeedRollerMot2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    PolyFeedRollerMot2.Fill = brushColor;
                }));
            }
        }
    }
}
