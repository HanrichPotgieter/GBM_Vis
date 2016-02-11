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
    /// Interaction logic for ControlSieve_MKSA1.xaml
    /// </summary>
    public partial class ControlSieve_MKSA1 : UserControl
    {
        private int _SieveColor;
        private string DescriptionSieve;
        private string StatusSieve;
        private bool FaultSieve;
        private string _ObjectNo;
        private string _PLCName;

        public ControlSieve_MKSA1()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//
        [Category("Buhler")]
        public int SieveColor
        {
            get
            {
                return _SieveColor;
            }
            set
            {
                _SieveColor = value;
                FB12 motor = new FB12();
                _SetColor(motor.SetColor(value));
                StatusSieve = motor.Status_Motor;
                FaultSieve = motor.Fault_Motor;
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
        public string Description_Sieve
        {
            get
            {
                return this.DescriptionSieve;
            }
            set
            {
                this.DescriptionSieve = value;
            }
        }

        [Category("Buhler")]
        public string Status_Sieve
        {
            get
            {
                return this.StatusSieve;
            }
        }

        [Category("Buhler")]
        public bool Fault_Sieve
        {
            get
            {
                return this.FaultSieve;
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
            polyBin.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyBin.Fill = brushColor;
            }));

            polyLeftFlap.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyLeftFlap.Fill = brushColor;
            }));           

            polyRightFlap.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyRightFlap.Fill = brushColor;
            })); 

            polyTopCone.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyTopCone.Fill = brushColor;
            })); 
        }    
    }
}
