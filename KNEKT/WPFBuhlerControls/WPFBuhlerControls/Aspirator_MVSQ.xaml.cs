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
    /// Interaction logic for Aspirator_MVSQ.xaml
    /// </summary>
    public partial class Aspirator_MVSQ : UserControl
    {
        private int _MotorColor1;
        private int _MotorColor2;

        private string DescriptionMotor1;
        private string StatusMotor1;
        private bool FaultMotor1;
        private string _ObjectNo1;

        private string DescriptionMotor2;
        private string StatusMotor2;
        private bool FaultMotor2;
        private string _ObjectNo2;
        private string _PLCName;

        public Aspirator_MVSQ()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                   Properties                                 //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int MotorColor1
        {
            get
            {
                return _MotorColor1;
            }
            set
            {
                _MotorColor1 = value;
                FB12 Motor = new FB12();
                _SetColorMotor1(Motor.SetColor(value));
                StatusMotor1 = Motor.Status_Motor;
                FaultMotor1 = Motor.Fault_Motor;
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
        public int MotorColor2
        {
            get
            {
                return _MotorColor2;
            }
            set
            {
                _MotorColor2 = value;
                FB12 Motor = new FB12();
                _SetColorMotor2(Motor.SetColor(value));
                StatusMotor2 = Motor.Status_Motor;
                FaultMotor2 = Motor.Fault_Motor;
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


        //
        //  Motor 1
        //
        [Category("Buhler")]
        public string Description_Motor1
        {
            get
            {
                return this.DescriptionMotor1;
            }
            set
            {
                this.DescriptionMotor1 = value;
            }
        }

        [Category("Buhler")]
        public string Status_Motor1
        {
            get
            {
                return this.StatusMotor1;
            }
        }

        [Category("Buhler")]
        public bool Fault_Motor1
        {
            get
            {
                return this.FaultMotor1;
            }
        }

        //
        //  Motor 2
        //
        [Category("Buhler")]
        public string Description_Motor2
        {
            get
            {
                return this.DescriptionMotor2;
            }
            set
            {
                this.DescriptionMotor2 = value;
            }
        }

        [Category("Buhler")]
        public string Status_Motor2
        {
            get
            {
                return this.StatusMotor2;
            }
        }

        [Category("Buhler")]
        public bool Fault_Motor2
        {
            get
            {
                return this.FaultMotor2;
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
        private void _SetColorMotor1(Brush brushColor)
        {
            PolyMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyMain.Fill = brushColor;
            }));
        }

        private void _SetColorMotor2(Brush brushColor)
        {
            PolyMain1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyMain1.Fill = brushColor;
            }));
        }
    }
}
