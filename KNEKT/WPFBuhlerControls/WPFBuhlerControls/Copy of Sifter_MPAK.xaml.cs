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

namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for Sifter_MPAK.xaml
    /// </summary>
    public partial class Sifter_MPAK : UserControl
    {
        private string DescriptionSifter;
        private string StatusSifter;
        private bool FaultSifter;

        public Sifter_MPAK()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                             Sifter Properties                                //
        //------------------------------------------------------------------------------//

        public int MotorColor
        {
            set
            {
                FB12 Motor = new FB12();
                _SetColorMotor(Motor.SetColor(value));
                StatusSifter = Motor.Status_Motor;
                FaultSifter = Motor.Fault_Motor;
            }
        }

        public string Description_Sifter
        {
            get
            {
                return this.DescriptionSifter;
            }
            set
            {
                this.DescriptionSifter = value;
            }
        }

        public string Status_Sifter
        {
            get
            {
                return this.StatusSifter;
            }
        }

        public bool Fault_Sifter
        {
            get
            {
                return this.FaultSifter;
            }
        }

        //------------------------------------------------------------------------------//
        //                        Sifter Set Color Methods                              //
        //------------------------------------------------------------------------------//
        private void _SetColorMotor(Brush brushColor)
        {
            RectLeft.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        RectLeft.Fill = brushColor;
                    }
            ));

            RectRight.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        RectRight.Fill = brushColor;
                    }
            ));
            //RectLeft.Fill = brushColor;
            //RectRight.Fill = brushColor;
        }

    }
}
