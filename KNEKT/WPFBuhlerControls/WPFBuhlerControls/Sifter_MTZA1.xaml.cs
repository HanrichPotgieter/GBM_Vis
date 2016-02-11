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
    /// Interaction logic for Sifter_MTZA1.xaml
    /// </summary>
    public partial class Sifter_MTZA1 : UserControl
    {
        private int _MotorColorFeed;
        private int _MotorColorAspiration;

        private string DescriptionFeedMotor;
        private string StatusFeedMotor;
        private bool FaultFeedMotor;
        private string _ObjectNo1;

        private string DescriptionAspirationMotor;
        private string StatusAspirationMotor;
        private bool FaultAspirationMotor;
        private string _ObjectNo2;
        private string _PLCName;

        public Sifter_MTZA1()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                   Properties                                 //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int ColorFeedMotor
        {
            get
            {
                return _MotorColorFeed;
            }
            set
            {
                _MotorColorFeed = value;
                FB12 motor = new FB12();
                _SetColorFeedMotor(motor.SetColor(value));
                StatusFeedMotor = motor.Status_Motor;
                FaultFeedMotor = motor.Fault_Motor;
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
        public int ColorAspirationMotor
        {
            get
            {
                return _MotorColorAspiration;
            }
            set
            {
                _MotorColorAspiration = value;
                FB12 motor = new FB12();
                _SetColorAspirationMotor(motor.SetColor(value));
                StatusAspirationMotor = motor.Status_Motor;
                FaultAspirationMotor = motor.Fault_Motor;
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

        #region Feed Motor


        [Category("Buhler")]
        public string Description_FeedMotor
        {
            get
            {
                return this.DescriptionFeedMotor;
            }
            set
            {
                this.DescriptionFeedMotor = value;
            }
        }

        [Category("Buhler")]
        public string Status_FeedMotor
        {
            get
            {
                return this.StatusFeedMotor;
            }
        }

        [Category("Buhler")]
        public bool Fault_FeedMotor
        {
            get
            {
                return this.FaultFeedMotor;
            }
        }

        #endregion


        #region Aspiration Motor


        [Category("Buhler")]
        public string Description_AspirationMotor
        {
            get
            {
                return this.DescriptionAspirationMotor;
            }
            set
            {
                this.DescriptionAspirationMotor = value;
            }
        }

        [Category("Buhler")]
        public string Status_AspirationMotor
        {
            get
            {
                return this.StatusAspirationMotor;
            }
        }

        [Category("Buhler")]
        public bool Fault_AspirationMotor
        {
            get
            {
                return this.FaultAspirationMotor;
            }
        }


        #endregion


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
        private void _SetColorFeedMotor(Brush brushColor)
        {
            PolyTopRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                PolyTopRight.Fill = brushColor;
            }));

            PolyBotRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                PolyBotRight.Fill = brushColor;
            }));
        }



        private void _SetColorAspirationMotor(Brush brushColor)
        {
            PolyLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                PolyLeft.Fill = brushColor;
            }));

            PolySquare.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                PolySquare.Fill = brushColor;
            }));            
        }

    }
}
