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
    /// Interaction logic for RotarySpout_MAYV.xaml
    /// </summary>
    public partial class RotarySpout_MAYV : UserControl
    {
        private int _MotorColor;
        private string DescriptionRotarySpout;
        private string StatusRotarySpout;
        private bool FaultRotarySpout;
        private string _ObjectNo;
        private string _PLCName;

        public RotarySpout_MAYV()
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
                StatusRotarySpout = motor.Status_Motor;
                FaultRotarySpout = motor.Fault_Motor;
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
        public string Description_RotarySpout
        {
            get
            {
                return this.DescriptionRotarySpout;
            }
            set
            {
                this.DescriptionRotarySpout = value;
            }
        }

        [Category("Buhler")]
        public string Status_RotarySpout
        {
            get
            {
                return this.StatusRotarySpout;
            }
        }

        [Category("Buhler")]
        public bool Fault_RotarySpout
        {
            get
            {
                return this.FaultRotarySpout;
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
        //                                Methods                                       //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            PolyMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                PolyMain.Fill = brushColor;
            }));

            RectBottom1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectBottom1.Fill = brushColor;
            }));

            RectBottom2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectBottom2.Fill = brushColor;
            }));

            RectBottom3.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectBottom3.Fill = brushColor;
            }));

            RectBottom4.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectBottom4.Fill = brushColor;
            }));

            RectBottom5.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectBottom5.Fill = brushColor;
            }));

            RectCoverBox.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectCoverBox.Fill = brushColor;
            }));
        }
    }
}
