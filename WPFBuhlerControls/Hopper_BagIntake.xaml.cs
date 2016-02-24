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
    /// Interaction logic for Hopper_BagIntake.xaml
    /// </summary>
    public partial class Hopper_BagIntake : UserControl
    {
        private bool _HasVibratoryMotor;
        private int _MotorColor;
        private string DescriptionMotor;
        private string StatusMotor;
        private bool FaultMotor;
        private string _ObjectNo;
        private string _PLCName;
        
        public Hopper_BagIntake()
        {
            InitializeComponent();
            Hopper_BagIntake_HasMotor = false;
        }



        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int MotorColor
        {
            get
            {
                return _MotorColor;
            }
            set
            {
                _MotorColor = value;
                FB12 Motor = new FB12();
                _SetColor(Motor.SetColor(value));
                StatusMotor = Motor.Status_Motor;
                FaultMotor = Motor.Fault_Motor;
            }
        }

        [Category("Buhler")]
        public string ObjectNumber
        {
            get
            {
                return this._ObjectNo;
            }
            set
            {
                this._ObjectNo = value;
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

        //
        //  MOTOR
        //
        [Category("Buhler")]
        public string Description_Motor
        {
            get
            {
                return this.DescriptionMotor;
            }
            set
            {
                this.DescriptionMotor = value;
            }
        }

        [Category("Buhler")]
        public string Status_Motor
        {
            get
            {
                return this.StatusMotor;
            }
        }

        [Category("Buhler")]
        public bool Fault_Motor
        {
            get
            {
                return this.FaultMotor;
            }
        }

        [Category("Buhler")]
        public bool Hopper_BagIntake_HasMotor
        {
            get
            {
                return _HasVibratoryMotor;
            }
            set
            {
                _HasVibratoryMotor = value;

                if (!value)
                {
                    _SetColor(KNEKTColors.BinColor);
                }
                else
                {
                    _SetColor(Brushes.Transparent);
                }
            }
        }

        //------------------------------------------------------------------------------//
        //                                    Methods                                   //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            polyBinTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
            {
                // Create a linear gradient brush with five stops 
                LinearGradientBrush BinConeGradient = new LinearGradientBrush();
                BinConeGradient.StartPoint = new Point(0, 0);
                BinConeGradient.EndPoint = new Point(1, 0);

                // Create and add Gradient stops
                GradientStop Yellow = new GradientStop();
                Yellow.Color = Colors.DarkGoldenrod;
                Yellow.Offset = 0.0;
                BinConeGradient.GradientStops.Add(Yellow);

                // Create and add Gradient stops
                GradientStop White1 = new GradientStop();
                White1.Color = Colors.LightGoldenrodYellow;
                White1.Offset = 0.3;
                BinConeGradient.GradientStops.Add(White1);

                // Create and add Gradient stops
                GradientStop White2 = new GradientStop();
                White2.Color = Colors.LightGoldenrodYellow;
                White2.Offset = 0.7;
                BinConeGradient.GradientStops.Add(White2);

                // Create and add Gradient stops
                GradientStop YellowR = new GradientStop();
                YellowR.Color = Colors.DarkGoldenrod;
                YellowR.Offset = 1.0;
                BinConeGradient.GradientStops.Add(YellowR);

                polyBinTop.Fill = BinConeGradient;
            }));

            polyBinBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
            {
                // Create a linear gradient brush with five stops 
                LinearGradientBrush BinConeGradient = new LinearGradientBrush();
                BinConeGradient.StartPoint = new Point(0, 0);
                BinConeGradient.EndPoint = new Point(1, 0);

                // Create and add Gradient stops
                GradientStop Yellow = new GradientStop();
                Yellow.Color = Colors.DarkGoldenrod;
                Yellow.Offset = 0.0;
                BinConeGradient.GradientStops.Add(Yellow);

                // Create and add Gradient stops
                GradientStop White1 = new GradientStop();
                White1.Color = Colors.LightGoldenrodYellow;
                White1.Offset = 0.3;
                BinConeGradient.GradientStops.Add(White1);

                // Create and add Gradient stops
                GradientStop White2 = new GradientStop();
                White2.Color = Colors.LightGoldenrodYellow;
                White2.Offset = 0.7;
                BinConeGradient.GradientStops.Add(White2);

                // Create and add Gradient stops
                GradientStop YellowR = new GradientStop();
                YellowR.Color = Colors.DarkGoldenrod;
                YellowR.Offset = 1.0;
                BinConeGradient.GradientStops.Add(YellowR);

                polyBinBot.Fill = BinConeGradient;
            }));
        }



    }
}
