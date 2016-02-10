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
    /// Interaction logic for Fan_Blower.xaml
    /// </summary>
    public partial class Fan_Blower : UserControl
    {
        private int _MotorColor;
        private string DescriptionMotor;
        private string StatusMotor;
        private bool FaultMotor;
        private bool _BlowerDirection = false; //Obsolete D.le Roux 2013-06-19
        private int _Direction = 1;
        private string _ObjectNo;
        private string _PLCName;


        public Fan_Blower()
        {
            InitializeComponent();

            polyUpLeft.Visibility = Visibility.Hidden;
            polyUpRight.Visibility = Visibility.Hidden;
            polyLeftTop.Visibility = Visibility.Visible;
            polyLeftBot.Visibility = Visibility.Visible;
            polyDownLeft.Visibility = Visibility.Hidden;
            polyDownRight.Visibility = Visibility.Hidden;
            polyRightTop.Visibility = Visibility.Hidden;
            polyRightBot.Visibility = Visibility.Hidden;
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
                _SetColorMotor(Motor.SetColor(Convert.ToInt32(value)));
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

        [Obsolete("Blower_IsBlowDirectionLeft, please use Direction instead")]
        public bool Blower_IsBlowDirectionLeft
        {
            get
            {
                return _BlowerDirection;
            }
            set
            {
                _BlowerDirection = value;
                SetBlowDirection();
            }
        }

        [Category("Buhler")]
        public int Direction
        {
            get
            {
                return _Direction;
            }
            set
            {
                _Direction = value;
                SetBlowDirectionInt();
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
        private void _SetColorMotor(Brush brushColor)
        {
            elipseMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                elipseMain.Fill = brushColor;      
            }));
        }

        //For obsolete Property
        private void SetBlowDirection()
        {
            if (_BlowerDirection)
            {
                polyLeftTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    polyLeftTop.Visibility = Visibility.Hidden;
                }));

                polyLeftBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    polyLeftBot.Visibility = Visibility.Hidden;
                }));

                polyRightTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    polyRightTop.Visibility = Visibility.Visible;
                }));

                polyRightBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    polyRightBot.Visibility = Visibility.Visible;
                }));
            }
            else
            {
                polyLeftTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    polyLeftTop.Visibility = Visibility.Visible;
                }));

                polyLeftBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    polyLeftBot.Visibility = Visibility.Visible;
                }));

                polyRightTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    polyRightTop.Visibility = Visibility.Hidden;
                }));

                polyRightBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    polyRightBot.Visibility = Visibility.Hidden;
                }));
            }
        }


        private void SetBlowDirectionInt()
        {
            if (_Direction == 1) //Blower Up
            {
                //UP
                polyUpLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    polyUpLeft.Visibility = Visibility.Visible;
                }));

                polyUpRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    polyUpRight.Visibility = Visibility.Visible;
                }));

                //RIGHT
                polyRightTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyRightTop.Visibility = Visibility.Hidden;
                }));

                polyRightBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyRightBot.Visibility = Visibility.Hidden;
                }));

                //DOWN
                polyDownLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyDownLeft.Visibility = Visibility.Hidden;
                }));

                polyDownRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyDownRight.Visibility = Visibility.Hidden;
                }));

                //LEFT
                polyLeftTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    polyLeftTop.Visibility = Visibility.Hidden;
                }));

                polyLeftBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    polyLeftBot.Visibility = Visibility.Hidden;
                }));            
            }
            else if(_Direction == 2) //Blower Right
            {
                //UP
                polyUpLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyUpLeft.Visibility = Visibility.Hidden;
                }));

                polyUpRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyUpRight.Visibility = Visibility.Hidden;
                }));

                //RIGHT
                polyRightTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyRightTop.Visibility = Visibility.Visible;
                }));

                polyRightBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyRightBot.Visibility = Visibility.Visible;
                }));

                //DOWN
                polyDownLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyDownLeft.Visibility = Visibility.Hidden;
                }));

                polyDownRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyDownRight.Visibility = Visibility.Hidden;
                }));

                //LEFT
                polyLeftTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyLeftTop.Visibility = Visibility.Hidden;
                }));

                polyLeftBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyLeftBot.Visibility = Visibility.Hidden;
                }));    
            }
            else if (_Direction == 3)
            {
                //UP
                polyUpLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyUpLeft.Visibility = Visibility.Hidden;
                }));

                polyUpRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyUpRight.Visibility = Visibility.Hidden;
                }));

                //RIGHT
                polyRightTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyRightTop.Visibility = Visibility.Hidden;
                }));

                polyRightBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyRightBot.Visibility = Visibility.Hidden;
                }));

                //DOWN
                polyDownLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyDownLeft.Visibility = Visibility.Visible;
                }));

                polyDownRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyDownRight.Visibility = Visibility.Visible;
                }));

                //LEFT
                polyLeftTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyLeftTop.Visibility = Visibility.Hidden;
                }));

                polyLeftBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyLeftBot.Visibility = Visibility.Hidden;
                }));    
            }
            else if (_Direction == 4)
            {
                //UP
                polyUpLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyUpLeft.Visibility = Visibility.Hidden;
                }));

                polyUpRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyUpRight.Visibility = Visibility.Hidden;
                }));

                //RIGHT
                polyRightTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyRightTop.Visibility = Visibility.Hidden;
                }));

                polyRightBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyRightBot.Visibility = Visibility.Hidden;
                }));

                //DOWN
                polyDownLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyDownLeft.Visibility = Visibility.Hidden;
                }));

                polyDownRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyDownRight.Visibility = Visibility.Hidden;
                }));

                //LEFT
                polyLeftTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyLeftTop.Visibility = Visibility.Visible;
                }));

                polyLeftBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polyLeftBot.Visibility = Visibility.Visible;
                }));    
            }
        }

    }
}
