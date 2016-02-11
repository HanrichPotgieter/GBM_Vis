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
    /// Interaction logic for Feeder_LossInWeight_MSDF.xaml
    /// </summary>
    public partial class Feeder_LossInWeight_MSDF : UserControl
    {
        private eScaleType _ScaleType;
        private int _MotorColor;
        private string DescriptionFeeder;
        private string StatusFeeder;
        private bool FaultFeeder;
        private string _ObjectNo;
        private string _PLCName;

        public Feeder_LossInWeight_MSDF()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int ColorFeeder
        {
            get
            {
                return _MotorColor;
            }
            set
            {
                if (Scale_ScaleType == eScaleType.CWA)
                {
                    _MotorColor = value;
                    FB97 scale = new FB97();
                    _SetColor(scale.SetColor(value));
                    StatusFeeder = scale.Status_CWA;
                    FaultFeeder = scale.Fault_CWA;
                }
                else
                {
                    _MotorColor = value;
                    FB12 Motor = new FB12();
                    _SetColor(Motor.SetColor(value));
                    StatusFeeder = Motor.Status_Motor;
                    FaultFeeder = Motor.Fault_Motor;
                }
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

        //
        //  MOTOR
        //
        [Category("Buhler")]
        public string Description_Feeder
        {
            get
            {
                return this.DescriptionFeeder;
            }
            set
            {
                this.DescriptionFeeder = value;
            }
        }

        [Category("Buhler")]
        public string Status_Feeder
        {
            get
            {
                return this.StatusFeeder;
            }
        }

        [Category("Buhler")]
        public bool Fault_Feeder
        {
            get
            {
                return this.FaultFeeder;
            }
        }

        [Category("Buhler")]
        public eScaleType Scale_ScaleType
        {
            get
            {
                return _ScaleType;
            }
            set
            {
                _ScaleType = value;
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
        private void _SetColor(Brush brushColor)
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
