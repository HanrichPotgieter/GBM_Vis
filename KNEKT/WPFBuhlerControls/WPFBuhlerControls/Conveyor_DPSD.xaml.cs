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
    /// Interaction logic for Conveyor_DPSD.xaml
    /// </summary>
    public partial class Conveyor_DPSD : UserControl
    {
        private int _MotorColor;
        private string DescriptionConveyor;
        private string StatusConveyor;
        private bool FaultConveyor;
        private string _ObjectNo;
        private string _PLCName;

        public Conveyor_DPSD()
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
                FB12 Motor = new FB12();
                _SetColor(Motor.SetColor(value));
                StatusConveyor = Motor.Status_Motor;
                FaultConveyor = Motor.Fault_Motor;
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
        public string Description_Conveyor
        {
            get
            {
                return this.DescriptionConveyor;
            }
            set
            {
                this.DescriptionConveyor = value;
            }
        }

        [Category("Buhler")]
        public string Status_Conveyor
        {
            get
            {
                return this.StatusConveyor;
            }
        }

        [Category("Buhler")]
        public bool Fault_Conveyor
        {
            get
            {
                return this.FaultConveyor;
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
            polyShell.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyShell.Fill = brushColor;
            }));
        }

    }
}
