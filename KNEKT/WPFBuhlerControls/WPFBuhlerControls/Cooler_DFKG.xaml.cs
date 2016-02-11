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
    /// Interaction logic for Cooler_DFKG.xaml
    /// </summary>
    public partial class Cooler_DFKG : UserControl
    {
        private int _MotorColor;
        private string DescriptionCooler;
        private string StatusCooler;
        private bool FaultCooler;
        private string _ObjectNo;
        private string _PLCName;

        public Cooler_DFKG()
        {
            InitializeComponent();
        }

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
                StatusCooler = Motor.Status_Motor;
                FaultCooler = Motor.Fault_Motor;
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
        public string Description_Cooler
        {
            get
            {
                return this.DescriptionCooler;
            }
            set
            {
                this.DescriptionCooler = value;
            }
        }

        [Category("Buhler")]
        public string Status_Cooler
        {
            get
            {
                return this.StatusCooler;
            }
        }

        [Category("Buhler")]
        public bool Fault_Cooler
        {
            get
            {
                return this.FaultCooler;
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
        //                                 Methods                                      //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            PolyTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyTop.Fill = brushColor;
            }));

            PolyUpperBody.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyUpperBody.Fill = brushColor;
            }));

            PolyBottom.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyBottom.Fill = brushColor;
            }));

            PolyLegs.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyLegs.Fill = brushColor;
            }));
        }


    }
}
