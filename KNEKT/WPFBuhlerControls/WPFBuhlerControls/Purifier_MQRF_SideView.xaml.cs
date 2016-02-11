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
    /// Interaction logic for Purifier_MQRF_SideView.xaml
    /// </summary>
    public partial class Purifier_MQRF_SideView : UserControl
    {
        private int _MotorColor;
        private string DescriptionPurifier;
        private string StatusPurifier;
        private bool FaultPurifier;
        private string _ObjectNo;
        private string _PLCName;

        public Purifier_MQRF_SideView()
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
                StatusPurifier = motor.Status_Motor;
                FaultPurifier = motor.Fault_Motor;
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
        public string Description_Purifier
        {
            get
            {
                return this.DescriptionPurifier;
            }
            set
            {
                this.DescriptionPurifier = value;
            }
        }

        [Category("Buhler")]
        public string Status_Purifier
        {
            get
            {
                return this.StatusPurifier;
            }
        }

        [Category("Buhler")]
        public bool Fault_Purifier
        {
            get
            {
                return this.FaultPurifier;
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
            PolyMain1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyMain1.Fill = brushColor;
            }));

            PolyMain2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyMain2.Fill = brushColor;
            }));

            PolyMain3.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyMain3.Fill = brushColor;
            }));

            RectLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectLeft.Fill = brushColor;
            }));

            RectRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectRight.Fill = brushColor;
            }));

            RectTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectTop.Fill = brushColor;
            }));

            PolyBo1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyBo1.Fill = brushColor;
            }));
        }
    }
}
