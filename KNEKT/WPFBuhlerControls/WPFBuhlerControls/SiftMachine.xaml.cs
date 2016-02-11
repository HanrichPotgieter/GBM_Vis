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
    /// Interaction logic for SiftMachine.xaml
    /// </summary>
    public partial class SiftMachine : UserControl
    {
      private int _MotorColor;
        private string DescriptionSiftMachine;
        private string StatusSiftMachine;
        private bool FaultSiftMachine;
        private string _ObjectNo;
        private string _PLCName;

        public SiftMachine()
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
                StatusSiftMachine = Motor.Status_Motor;
                FaultSiftMachine = Motor.Fault_Motor;
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
        public string Description_SiftMachine
        {
            get
            {
                return this.DescriptionSiftMachine;
            }
            set
            {
                this.DescriptionSiftMachine = value;
            }
        }

        [Category("Buhler")]
        public string Status_SiftMachine
        {
            get
            {
                return this.StatusSiftMachine;
            }
        }

        [Category("Buhler")]
        public bool Fault_SiftMachine
        {
            get
            {
                return this.FaultSiftMachine;
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
            polySift.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polySift.Fill = brushColor;
            }));
        }
    }
}
