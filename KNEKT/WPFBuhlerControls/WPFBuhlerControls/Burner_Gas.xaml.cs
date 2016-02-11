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
    /// Interaction logic for Burner_Gas.xaml
    /// </summary>
    public partial class Burner_Gas : UserControl
    {
        private int _MotorColor;
        private string DescriptionMotor;
        private string StatusMotor;
        private bool FaultMotor;
        private string _ObjectNo;
        private string _PLCName;

        public Burner_Gas()
        {
            InitializeComponent();
            //elipseMain.Fill = KNEKTColors.Gray;
            //polyMain.Fill = KNEKTColors.Gray;
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
        //                                   Methods                                    //
        //------------------------------------------------------------------------------//
        private void _SetMotorColor(Brush brushColor)
        {
            elipseMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                elipseMain.Fill = brushColor;
            }));

            elipseInner.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                elipseInner.Fill = brushColor;
            }));

            polyMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyMain.Fill = brushColor;
            }));
        }
    }
}
