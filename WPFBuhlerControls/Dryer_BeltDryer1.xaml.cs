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
    /// Interaction logic for Dryer_BeltDryer1.xaml
    /// </summary>
    public partial class Dryer_BeltDryer1 : UserControl
    {
        private string DescriptionBelt1;
        private string StatusBelt1;
        private bool FaultBelt1;
        private string _ObjectNo;
        private string _PLCName;

        public Dryer_BeltDryer1()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
        //------------------------------------------------------------------------------//

        public int ColorBelt1
        {
            set
            {
                FB12 Motor = new FB12();
                _SetColorBelt1(Motor.SetColor(value));
                StatusBelt1 = Motor.Status_Motor;
                FaultBelt1 = Motor.Fault_Motor;
            }
        }

        public string Description_Belt1
        {
            get
            {
                return this.DescriptionBelt1;
            }
            set
            {
                this.DescriptionBelt1 = value;
            }
        }

        public string Status_Belt1
        {
            get
            {
                return this.StatusBelt1;
            }
        }

        public bool Fault_Belt1
        {
            get
            {
                return this.FaultBelt1;
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
        //                             Set Color Methods                                //
        //------------------------------------------------------------------------------//
        private void _SetColorBelt1(Brush brushColor)
        {
            borderMainBelt.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        borderMainBelt.Background = brushColor;
                    }
            ));
        }
    }
}
