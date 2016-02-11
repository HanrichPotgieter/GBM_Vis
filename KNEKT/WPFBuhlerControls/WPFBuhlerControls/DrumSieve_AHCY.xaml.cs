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
    /// Interaction logic for DrumSieve_AHCY.xaml
    /// </summary>
    public partial class DrumSieve_AHCY : UserControl
    {
        private int _MotorColor;
        private string DescriptionDrumSieve;
        private string StatusDrumSieve;
        private bool FaultDrumSieve;
        private string _ObjectNo;
        private string _PLCName;

        public DrumSieve_AHCY()
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
                _SetColorMotor(motor.SetColor(value));
                StatusDrumSieve = motor.Status_Motor;
                FaultDrumSieve = motor.Fault_Motor;
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
        public string Description_DrumSieve
        {
            get
            {
                return this.DescriptionDrumSieve;
            }
            set
            {
                this.DescriptionDrumSieve = value;
            }
        }

        [Category("Buhler")]
        public string Status_DrumSieve
        {
            get
            {
                return this.StatusDrumSieve;
            }
        }

        [Category("Buhler")]
        public bool Fault_DrumSieve
        {
            get
            {
                return this.FaultDrumSieve;
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
            RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectMain.Fill = brushColor;
            }));

            RectMid1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectMid1.Fill = brushColor;
            }));

            RectBot1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectBot1.Fill = brushColor;
            }));

            RectRight1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectRight1.Fill = brushColor;
            }));

            RectRight2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectRight2.Fill = brushColor;
            }));

            PolyMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                PolyMain.Fill = brushColor;
            }));

            PolyRight1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                PolyRight1.Fill = brushColor;
            }));
        }
    }
}
