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
    /// Interaction logic for VibroSifter_MKZH.xaml
    /// </summary>
    public partial class VibroSifter_MKZH : UserControl
    {
        private int _MotorColor;
        private string DescriptionVibroSifter;
        private string StatusVibroSifter;
        private bool FaultVibroSifter;
        private string _ObjectNo;
        private string _PLCName;

        public VibroSifter_MKZH()
        {
            InitializeComponent();
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
                FB12 motor = new FB12();
                _SetColor(motor.SetColor(value));
                StatusVibroSifter = motor.Status_Motor;
                FaultVibroSifter = motor.Fault_Motor;
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
        public string Description_VibroSifter
        {
            get
            {
                return this.DescriptionVibroSifter;
            }
            set
            {
                this.DescriptionVibroSifter = value;
            }
        }

        [Category("Buhler")]
        public string Status_VibroSifter
        {
            get
            {
                return this.StatusVibroSifter;
            }
        }

        [Category("Buhler")]
        public bool Fault_VibroSifter
        {
            get
            {
                return this.FaultVibroSifter;
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
        private void _SetColor(Brush brushColor)
        {
            RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectMain.Fill = brushColor;
            }));

            RectTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectTop.Fill = brushColor;
            }));

            RectCenter.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectCenter.Fill = brushColor;
            }));

            ellipseMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                ellipseMain.Fill = brushColor;
            }));

            polyLeftFoot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyLeftFoot.Fill = brushColor;
            }));

            polyRightFoot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyRightFoot.Fill = brushColor;
            }));

            polyLeftTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyLeftTop.Fill = brushColor;
            }));

            polyRightTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyRightTop.Fill = brushColor;
            }));

            polyTopTriangle.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyTopTriangle.Fill = brushColor;
            }));

            polyLeftWing.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyLeftWing.Fill = brushColor;
            }));

            polyRightWing.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyRightWing.Fill = brushColor;
            }));
        }
    }
}
