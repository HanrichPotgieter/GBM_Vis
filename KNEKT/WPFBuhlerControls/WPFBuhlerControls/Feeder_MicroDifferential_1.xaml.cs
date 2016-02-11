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
    /// Interaction logic for Feeder_MicroDifferential_1.xaml
    /// </summary>
    public partial class Feeder_MicroDifferential_1 : UserControl
    {
        private int _MotorColor;
        private string DescriptionMotor;
        private string StatusMotor;
        private bool FaultMotor;
        private string _ObjectNo;
        private string _PLCName;

        public Feeder_MicroDifferential_1()
        {
            InitializeComponent();
            polyHopper.Fill = KNEKTColors.BinColor;
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
                _SetMotorColor(Motor.SetColor(value));
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
        //                                  Methods                                     //
        //------------------------------------------------------------------------------//
        private void _SetMotorColor(Brush brushColor)
        {
            RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectMain.Fill = brushColor;
            }));

            polyBin.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyBin.Fill = brushColor;
            }));
        }
    }
}
