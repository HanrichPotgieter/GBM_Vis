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
    /// Interaction logic for Degerminator_MHXM.xaml
    /// </summary>
    public partial class Degerminator_MHXM : UserControl
    {
        private int _MotorColorMain;
        private int _MotorColorFan;
        private bool _HasFan;

        private string DescriptionMainMotor;
        private string StatusMainMotor;
        private bool FaultMainMotor;
        private string _ObjectNo1;

        private string DescriptionFan;
        private string StatusFan;
        private bool FaultFan;
        private string _ObjectNo2;
        private string _PLCName;

        public Degerminator_MHXM()
        {
            InitializeComponent();

            Degerminator_HasFan = true;
        }

        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
        //------------------------------------------------------------------------------//

        [Obsolete("MainMotorColor is Obsolete. Please use MotorColorMain instead"), Category("Buhler")]
        public int MainMotorColor
        {
            set
            {
                FB12 Motor = new FB12();
                _SetColorMotor1(Motor.SetColor(value));
                StatusMainMotor = Motor.Status_Motor;
                FaultMainMotor = Motor.Fault_Motor;
            }
        }

        [Obsolete("FanColor is Obsolete. Please use MotorColorFan instead"), Category("Buhler")]
        public int FanColor
        {
            set
            {
                FB12 Motor = new FB12();
                _SetColorMotor2(Motor.SetColor(value));
                StatusFan = Motor.Status_Motor;
                FaultFan = Motor.Fault_Motor;
            }
        }

        [Category("Buhler")]
        public int MotorColorMain
        {
            get
            {
                return _MotorColorMain;
            }
            set
            {
                _MotorColorMain = value;
                FB12 Motor = new FB12();
                _SetColorMotor1(Motor.SetColor(value));
                StatusMainMotor = Motor.Status_Motor;
                FaultMainMotor = Motor.Fault_Motor;
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
        public int MotorColorFan
        {
            get
            {
                return _MotorColorFan;
            }
            set
            {
                _MotorColorFan = value;
                FB12 Motor = new FB12();
                _SetColorMotor2(Motor.SetColor(value));
                StatusFan = Motor.Status_Motor;
                FaultFan = Motor.Fault_Motor;
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
        //  Motor 1
        //
        [Category("Buhler")]
        public string Description_MainMotor
        {
            get
            {
                return this.DescriptionMainMotor;
            }
            set
            {
                this.DescriptionMainMotor = value;
            }
        }

        [Category("Buhler")]
        public string Status_MainMotor
        {
            get
            {
                return this.StatusMainMotor;
            }
        }

        [Category("Buhler")]
        public bool Fault_MainMotor
        {
            get
            {
                return this.FaultMainMotor;
            }
        }

        //
        //  Motor 2
        //
        [Category("Buhler")]
        public string Description_Fan
        {
            get
            {
                return this.DescriptionFan;
            }
            set
            {
                this.DescriptionFan = value;
            }
        }

        [Category("Buhler")]
        public string Status_Fan
        {
            get
            {
                return this.StatusFan;
            }
        }

        [Category("Buhler")]
        public bool Fault_Fan
        {
            get
            {
                return this.FaultFan;
            }
        }

        [Category("Buhler")]
        public bool Degerminator_HasFan
        {
            get
            {
                return this._HasFan;
            }
            set
            {
                _HasFan = value;
                SetFanVisibility(value);
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
        private void _SetColorMotor1(Brush brushColor)
        {
            polyBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyBot.Fill = brushColor;
            }));

            RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectMain.Fill = brushColor;
            }));
        }

        private void _SetColorMotor2(Brush brushColor)
        {
            ellipseBig.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                ellipseBig.Fill = brushColor;
            }));

            ellipseSmall.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                ellipseSmall.Fill = brushColor;
            }));

            polyConnect.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyConnect.Fill = brushColor;
            }));
        }

        private void SetFanVisibility(bool IsVisible)
        {
            ellipseBig.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                ellipseBig.Visibility = IsVisible ? Visibility.Visible : Visibility.Hidden;
            }));

            ellipseSmall.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                ellipseSmall.Visibility = IsVisible ? Visibility.Visible : Visibility.Hidden;
            }));

            polyConnect.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyConnect.Visibility = IsVisible ? Visibility.Visible : Visibility.Hidden;
            }));
        }
    }
}
