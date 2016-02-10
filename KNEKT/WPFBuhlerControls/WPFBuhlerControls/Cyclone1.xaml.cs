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
    /// Interaction logic for Airlock_MPSN.xaml
    /// </summary>
    public partial class Airlock_MPSN_Cyclone : UserControl
    {
        private string DescriptionMotor;
        private string StatusMotor;
        private bool FaultMotor;

        public Airlock_MPSN_Cyclone()
        {
            InitializeComponent();
            PolyMain.Fill = KNEKTColors.NoControlColor;
            RectTop.Fill = KNEKTColors.NoControlColor;
        }


        //------------------------------------------------------------------------------//
        //                             Airlock Properties                               //
        //------------------------------------------------------------------------------//

        public int MotorColor
        {
            set
            {
                FB12 Motor = new FB12();
                _SetColorMotor(Motor.SetColor(value));
                StatusMotor = Motor.Status_Motor;
                FaultMotor = Motor.Fault_Motor;
            }
        }

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

        public string Status_Motor
        {
            get
            {
                return this.StatusMotor;
            }
        }

        public bool Fault_Motor
        {
            get
            {
                return this.FaultMotor;
            }
        }


        //------------------------------------------------------------------------------//
        //                        Aspirator Set Color Methods                           //
        //------------------------------------------------------------------------------//
        private void _SetColorMotor(Brush brushColor)
        {
            ElipseBottom.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        ElipseBottom.Fill = brushColor;      
                    }
            ));

            ElipseBottomCenter.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        ElipseBottomCenter.Fill = brushColor;      
                    }
            ));

            //ElipseBottom.Fill = brushColor;
            //ElipseBottomCenter.Fill = brushColor;
        }
    }
}
