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
    /// Interaction logic for FlapBox.xaml
    /// </summary>
    public partial class FlapBox : UserControl
    {
        private int _FlapDirection;

        private string DescriptionFlap;
        private string StatusFlap;
        private bool FaultFlap;
        private string _ObjectNo;

        private int FlapHighNumberPosition;     //1 = Up, 2 = Right, 3 = Down, 4 = left
        private int FlapLowNumberPosition;      //1 = Up, 2 = Right, 3 = Down, 4 = left
        private int FlapSourceDirection = 1;
        private int FlapSourceDirection_2nd = 0;
        private Polyline PolySourceDirection1; 
        private Polyline PolySourceDirection2;
        private string _PLCName;


        public FlapBox()
        {
            InitializeComponent();

            elipseMain.Stroke = KNEKTColors.LineColor;

            polyLeft.Visibility = Visibility.Hidden;
            polyRight.Visibility = Visibility.Hidden;
            polyUp.Visibility = Visibility.Hidden;
            polyDown.Visibility = Visibility.Hidden;
            
            polyLineTop.Visibility = Visibility.Hidden;
            polyLineRight.Visibility = Visibility.Hidden;
            polyLineDown.Visibility = Visibility.Hidden;
            polyLineLeft.Visibility = Visibility.Hidden;

            polyLineTop.Stroke = KNEKTColors.Green;
            polyLineLeft.Stroke = KNEKTColors.Green;
            polyLineDown.Stroke = KNEKTColors.Green;
            polyLineRight.Stroke = KNEKTColors.Green;

            polyLineTopTail.Visibility = Visibility.Hidden;
            polyLineLeftTail.Visibility = Visibility.Hidden;
            polyLineDownTail.Visibility = Visibility.Hidden;
            polyLineRightTail.Visibility = Visibility.Hidden;

            polyLineTopTail.Stroke = KNEKTColors.Green;
            polyLineLeftTail.Stroke = KNEKTColors.Green;
            polyLineDownTail.Stroke = KNEKTColors.Green;
            polyLineRightTail.Stroke = KNEKTColors.Green;

        }


        //------------------------------------------------------------------------------//
        //                               Public Properties                              //
        //------------------------------------------------------------------------------//

        [Obsolete("SetFlapState is Obsolete. Please use FlapState instead"), Category("Buhler")]
        public int SetFlapState
        {
            get
            {
                return FlapBox_LowNumberPosition;
            }
            set
            {
                FB13 Flap = new FB13();
                _SetColor(Brushes.Green);

                //StatusFlap = Flap.Status_Slide;
                FaultFlap = Flap.Fault_Slide;

                //FlapBox_SetDirection = value;
                if (value == 1 || value == 4 || value == 11 || value == 513 || value == 257) //LN
                {
                    FlapBox_SetDirection = FlapBox_LowNumberPosition;
                    StatusFlap = "LowNumber";
                }
                else if (value == 2 || value == 3 || value == 515 || value == 259) //HN
                {
                    FlapBox_SetDirection = FlapBox_HighNumberPosition;
                    StatusFlap = "HighNumber";
                }
                else
                {
                    FlapBox_SetDirection = 5;
                    StatusFlap = "NoPosition";
                }

                //Determine which source line to show if there are multiple source positions
                if (FlapSourceDirection_2nd >= 1 && FlapSourceDirection_2nd <= 4) 
                {

                    //In StHN show Source Direction 1
                    if (StatusFlap == "HighNumber") 
                    {
                        PolySourceDirection1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ PolySourceDirection1.Visibility = Visibility.Visible; }));  
                        PolySourceDirection2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ PolySourceDirection2.Visibility = Visibility.Hidden; }));                          
                    }

                    //In StLN show Source Direction 2
                    if (StatusFlap == "LowNumber") 
                    {
                        PolySourceDirection1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { PolySourceDirection1.Visibility = Visibility.Hidden; }));
                        PolySourceDirection2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { PolySourceDirection2.Visibility = Visibility.Visible; }));  
                    }

                }
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

        //-------------------------


        [Category("Buhler")]
        public int FlapState
        {
            get
            {
                return _FlapDirection;
            }
            set
            {
                _FlapDirection = value;
                FB13 Flap = new FB13();
                _SetColor(Brushes.Green);                
                FaultFlap = Flap.Fault_Slide;

                //FlapBox_SetDirection = value;
                if (value == 1 || value == 4 || value == 11 || value == 513 || value == 257) //LN
                {
                    FlapBox_SetDirection = FlapBox_LowNumberPosition;
                    StatusFlap = "LowNumber";
                }
                else if (value == 2 || value == 3 || value == 515 || value == 259) //HN
                {
                    FlapBox_SetDirection = FlapBox_HighNumberPosition;
                    StatusFlap = "HighNumber";
                }
                else
                {
                    FlapBox_SetDirection = 5;
                    StatusFlap = "NoPosition";
                }

                 //Determine which source line to show if there are multiple source positions
                if (FlapSourceDirection_2nd >= 1 && FlapSourceDirection_2nd <= 4)
                {

                    //In StHN show Source Direction 1
                    if (StatusFlap == "HighNumber")
                    {
                        PolySourceDirection1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { PolySourceDirection1.Visibility = Visibility.Visible; }));
                        PolySourceDirection2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { PolySourceDirection2.Visibility = Visibility.Hidden; }));
                    }

                    //In StLN show Source Direction 2
                    if (StatusFlap == "LowNumber")
                    {
                        PolySourceDirection1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { PolySourceDirection1.Visibility = Visibility.Hidden; }));
                        PolySourceDirection2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { PolySourceDirection2.Visibility = Visibility.Visible; }));
                    }
                }
            }
        }

        [Category("Buhler")]
        public int FlapBox_HighNumberPosition
        {
            get
            {
                return this.FlapHighNumberPosition;
            }
            set
            {
                this.FlapHighNumberPosition = value;
                //this.FlapDirection = value;
            }
        }

        [Category("Buhler")]
        public int FlapBox_LowNumberPosition
        {
            get
            {
                return this.FlapLowNumberPosition;
            }
            set
            {
                this.FlapLowNumberPosition = value;
                //this.FlapDirection = value;
            }
        }

        [Category("Buhler")]
        public int FlapBox_SourceDirection
        {
            get
            {
                return FlapSourceDirection;
            }
            set
            {
                FlapSourceDirection = value;

                switch (value)
                {
                    case 1:
                        polyLineTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineTop.Visibility = Visibility.Visible; }));
                        polyLineRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineRight.Visibility = Visibility.Hidden; }));
                        polyLineDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineDown.Visibility = Visibility.Hidden; }));
                        polyLineLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineLeft.Visibility = Visibility.Hidden; }));
                        PolySourceDirection1 = polyLineTop;
                        break;

                    case 2:
                        polyLineTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineTop.Visibility = Visibility.Hidden; }));
                        polyLineRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineRight.Visibility = Visibility.Visible;}));
                        polyLineDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineDown.Visibility = Visibility.Hidden;}));
                        polyLineLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineLeft.Visibility = Visibility.Hidden;}));
                        PolySourceDirection1 = polyLineRight;
                        break;

                    case 3:
                        polyLineTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineTop.Visibility = Visibility.Hidden; }));
                        polyLineRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineRight.Visibility = Visibility.Hidden; }));
                        polyLineDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineDown.Visibility = Visibility.Visible; }));
                        polyLineLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineLeft.Visibility = Visibility.Hidden; }));
                        PolySourceDirection1 = polyLineDown;
                        break;

                    case 4:
                        polyLineTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineTop.Visibility = Visibility.Hidden; }));
                        polyLineRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineRight.Visibility = Visibility.Hidden; }));
                        polyLineDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineDown.Visibility = Visibility.Hidden; }));
                        polyLineLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineLeft.Visibility = Visibility.Visible; }));
                        PolySourceDirection1 = polyLineLeft;
                        break;

                    default:
                        polyLineTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { polyLineTop.Visibility = Visibility.Hidden; }));
                        polyLineRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { polyLineRight.Visibility = Visibility.Hidden; }));
                        polyLineDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { polyLineDown.Visibility = Visibility.Hidden; }));
                        polyLineLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { polyLineLeft.Visibility = Visibility.Hidden; }));
                        break;
                }
            }
        }

        [Category("Buhler")]
        public int FlapBox_SourceDirection2 
        {
            get
            {
                return FlapSourceDirection_2nd;
            }
            set
            {

                FlapSourceDirection_2nd = value;

                if (value != FlapSourceDirection) //Ensure that the same source direction is not set more than once
                {

                    if (FlapSourceDirection == 1) 
                    {
                        switch (value)
                        {
                            case 2:
                                polyLineRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineRight.Visibility = Visibility.Visible; }));  
                                polyLineDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineDown.Visibility = Visibility.Hidden; }));  
                                polyLineLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineLeft.Visibility = Visibility.Hidden; }));  
                                PolySourceDirection2 = polyLineRight;
                                break;

                            case 3:
                                polyLineRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineRight.Visibility = Visibility.Hidden; }));  
                                polyLineDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineDown.Visibility = Visibility.Visible; }));  
                                polyLineLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineLeft.Visibility = Visibility.Hidden; })); 
                                PolySourceDirection2 = polyLineDown;
                                break;

                            case 4:
                                polyLineRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineRight.Visibility = Visibility.Hidden; }));  
                                polyLineDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineDown.Visibility = Visibility.Hidden; }));  
                                polyLineLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineLeft.Visibility = Visibility.Visible; })); 
                                PolySourceDirection2 = polyLineLeft;
                                break;

                            default:
                                polyLineRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineRight.Visibility = Visibility.Hidden; }));  
                                polyLineDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineDown.Visibility = Visibility.Hidden; }));  
                                polyLineLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineLeft.Visibility = Visibility.Hidden; })); 
                                break;
                        }
                    }


                    if (FlapSourceDirection == 2) 
                    {
                        switch (value)
                        {
                            case 1:
                                polyLineTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineTop.Visibility = Visibility.Visible; })); 
                                polyLineDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineDown.Visibility = Visibility.Hidden; })); 
                                polyLineLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineLeft.Visibility = Visibility.Hidden; })); 
                                PolySourceDirection2 = polyLineTop;
                                break;

                            case 3:
                                polyLineTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineTop.Visibility = Visibility.Hidden; })); 
                                polyLineDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineDown.Visibility = Visibility.Visible; })); 
                                polyLineLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineLeft.Visibility = Visibility.Hidden; }));
                                PolySourceDirection2 = polyLineDown;
                                break;

                            case 4:
                                polyLineTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineTop.Visibility = Visibility.Hidden; })); 
                                polyLineDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineDown.Visibility = Visibility.Hidden; })); 
                                polyLineLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineLeft.Visibility = Visibility.Visible; }));
                                PolySourceDirection2 = polyLineLeft;
                                break;

                            default:
                                polyLineTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineTop.Visibility = Visibility.Hidden; })); 
                                polyLineDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineDown.Visibility = Visibility.Hidden; })); 
                                polyLineLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineLeft.Visibility = Visibility.Hidden; }));
                                break;
                        }
                    }

                    if (FlapSourceDirection == 3) 
                    {
                        switch (value)
                        {
                            case 1:
                                polyLineTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineTop.Visibility = Visibility.Visible; })); 
                                polyLineRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineRight.Visibility = Visibility.Hidden; })); 
                                polyLineLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineLeft.Visibility = Visibility.Hidden; }));                                
                                PolySourceDirection2 = polyLineTop;
                                break;

                            case 2:
                                 polyLineTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineTop.Visibility = Visibility.Hidden; })); 
                                polyLineRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineRight.Visibility = Visibility.Visible; })); 
                                polyLineLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineLeft.Visibility = Visibility.Hidden; })); 
                                PolySourceDirection2 = polyLineRight;
                                break;

                            case 4:
                                 polyLineTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineTop.Visibility = Visibility.Hidden; })); 
                                polyLineRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineRight.Visibility = Visibility.Hidden; })); 
                                polyLineLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineLeft.Visibility = Visibility.Visible; })); 
                                PolySourceDirection2 = polyLineLeft;
                                break;

                            default:
                                polyLineTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineTop.Visibility = Visibility.Hidden; })); 
                                polyLineRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineRight.Visibility = Visibility.Hidden; })); 
                                polyLineLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineLeft.Visibility = Visibility.Hidden; }));
                                break;
                        }
                    }

                    if (FlapSourceDirection == 4) 
                    {
                        switch (value)
                        {
                            case 1:
                                polyLineTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineTop.Visibility = Visibility.Visible; })); 
                                polyLineRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineRight.Visibility = Visibility.Hidden; })); 
                                polyLineDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineDown.Visibility = Visibility.Hidden; })); 
                                PolySourceDirection2 = polyLineTop;
                                break;

                            case 2:
                               polyLineTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineTop.Visibility = Visibility.Hidden; })); 
                                polyLineRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineRight.Visibility = Visibility.Visible; })); 
                                polyLineDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineDown.Visibility = Visibility.Hidden; })); 
                                PolySourceDirection2 = polyLineRight;
                                break;

                            case 3:
                                polyLineTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineTop.Visibility = Visibility.Hidden; })); 
                                polyLineRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineRight.Visibility = Visibility.Hidden; })); 
                                polyLineDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineDown.Visibility = Visibility.Visible; })); 
                                PolySourceDirection2 = polyLineDown;
                                break;

                            default:
                                polyLineTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineTop.Visibility = Visibility.Hidden; })); 
                                polyLineRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineRight.Visibility = Visibility.Hidden; })); 
                                polyLineDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate(){ polyLineDown.Visibility = Visibility.Hidden; }));
                                break;
                        }
                    }

                }

            }
         }

        [Category("Buhler")]
        public string Description_Flap
        {
            get
            {
                return this.DescriptionFlap;
            }
            set
            {
                this.DescriptionFlap = value;
            }
        }

        [Category("Buhler")]
        public string Status_Flap
        {
            get
            {
                return this.StatusFlap;
            }
        }

        [Category("Buhler")]
        public bool Fault_Flap
        {
            get
            {
                return this.FaultFlap;
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
        //                              Private Properties                              //
        //------------------------------------------------------------------------------//

        /// <summary>
        /// Set the direction of the flapbox. 1 = Up, 2 = Right, 3 = Down, 4 = Left
        /// </summary>
        private int FlapBox_SetDirection
        {
            set
            {
                switch (value)
                {
                    case 1:                                             //UP
                        polyUp.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyUp.Visibility = Visibility.Visible;
                        }));

                        polyRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyRight.Visibility = Visibility.Hidden;
                        }));                       

                        polyDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyDown.Visibility = Visibility.Hidden;
                        }));

                        polyLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyLeft.Visibility = Visibility.Hidden;
                        }));
                        
                        elipseMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            elipseMain.Fill = Brushes.Transparent;
                        }));


                        //
                        //TAILS
                        //
                        polyLineTopTail.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyLineTopTail.Visibility = Visibility.Visible;
                        }));

                        polyLineRightTail.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyLineRightTail.Visibility = Visibility.Hidden;
                        }));

                        polyLineDownTail.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyLineDownTail.Visibility = Visibility.Hidden;
                        }));

                        polyLineLeftTail.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyLineLeftTail.Visibility = Visibility.Hidden;
                        }));
                        break;


                    case 2:                                             //RIGHT
                        polyUp.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyUp.Visibility = Visibility.Hidden;
                        }));

                        polyRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyRight.Visibility = Visibility.Visible;
                        }));
                       
                        polyDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyDown.Visibility = Visibility.Hidden;
                        }));

                        polyLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyLeft.Visibility = Visibility.Hidden;
                        }));
                        
                        elipseMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            elipseMain.Fill = Brushes.Transparent;
                        }));

                        //
                        //TAILS
                        //
                        polyLineTopTail.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyLineTopTail.Visibility = Visibility.Hidden;
                        }));

                        polyLineRightTail.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyLineRightTail.Visibility = Visibility.Visible;
                        }));

                        polyLineDownTail.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyLineDownTail.Visibility = Visibility.Hidden;
                        }));

                        polyLineLeftTail.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyLineLeftTail.Visibility = Visibility.Hidden;
                        }));
                        break;


                    case 3:                                             //DOWN
                        
                        polyUp.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyUp.Visibility = Visibility.Hidden;
                        }));

                        polyRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyRight.Visibility = Visibility.Hidden;
                        }));                       

                        polyDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyDown.Visibility = Visibility.Visible;
                        }));

                        polyLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyLeft.Visibility = Visibility.Hidden;
                        }));                        

                        elipseMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            elipseMain.Fill = Brushes.Transparent;
                        }));

                        //
                        //TAILS
                        //
                        polyLineTopTail.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyLineTopTail.Visibility = Visibility.Hidden;
                        }));

                        polyLineRightTail.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyLineRightTail.Visibility = Visibility.Hidden;
                        }));

                        polyLineDownTail.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyLineDownTail.Visibility = Visibility.Visible;
                        }));

                        polyLineLeftTail.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyLineLeftTail.Visibility = Visibility.Hidden;
                        }));
                        break;


                    case 4:                                             //LEFT                        
                        polyUp.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyUp.Visibility = Visibility.Hidden;
                        }));

                        polyRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyRight.Visibility = Visibility.Hidden;
                        }));
                       
                        polyDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyDown.Visibility = Visibility.Hidden;
                        }));

                        polyLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyLeft.Visibility = Visibility.Visible;
                        }));
                        
                        elipseMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            elipseMain.Fill = Brushes.Transparent;
                        }));

                        //
                        //TAILS
                        //
                        polyLineTopTail.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyLineTopTail.Visibility = Visibility.Hidden;
                        }));

                        polyLineRightTail.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyLineRightTail.Visibility = Visibility.Hidden;
                        }));

                        polyLineDownTail.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyLineDownTail.Visibility = Visibility.Hidden;
                        }));

                        polyLineLeftTail.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyLineLeftTail.Visibility = Visibility.Visible;
                        }));
                        break;


                    case 5:                                             //NO POSITION
                        elipseMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            elipseMain.Fill = Brushes.Red;
                        }));
                        break;

                }
            }
        }


       


        //------------------------------------------------------------------------------//
        //                        FlapBox Set Color Methods                             //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            polyLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyLeft.Fill = brushColor;
                polyLeft.Stroke = brushColor;
            }));

            polyRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyRight.Fill = brushColor;
                polyRight.Stroke = brushColor;
            }));

            polyDown.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyDown.Fill = brushColor;
                polyDown.Stroke = brushColor;
            }));

            polyUp.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyUp.Fill = brushColor;
                polyUp.Stroke = brushColor;
            }));            
        }
    }
}
