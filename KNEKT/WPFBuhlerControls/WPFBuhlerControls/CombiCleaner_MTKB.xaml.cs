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
    /// Interaction logic for CombiCleaner_MTKB.xaml
    /// </summary>
    public partial class CombiCleaner_MTKB : UserControl
    {
        private int _MotorColor;
        private string DescriptionMotor;
        private string StatusMotor;
        private bool FaultMotor;
        private string _ObjectNo;
        private string _PLCName;

        public CombiCleaner_MTKB()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
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
                _SetColorMotor(Motor.SetColor(value));
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
        private void _SetColorMotor(Brush brushColor)
        {
            polyOuterRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyOuterRight.Fill = brushColor;
            }));

            polyBottom.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyBottom.Fill = brushColor;
            }));

            PolyBottomSmall.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyBottomSmall.Fill = brushColor;
            }));

            polyLineRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyLineRight.Fill = brushColor;
            }));

            polyTopMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyTopMain.Fill = brushColor;
            }));

            polySmallSquare.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polySmallSquare.Fill = brushColor;
            }));
        }
    }
}
