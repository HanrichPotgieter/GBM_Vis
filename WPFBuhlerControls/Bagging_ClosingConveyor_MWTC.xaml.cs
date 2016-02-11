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
    /// Interaction logic for Bagging_ClosingConveyor_MWTC.xaml
    /// </summary>
    public partial class Bagging_ClosingConveyor_MWTC : UserControl
    {
        private int _MotorColor;
        private string DescriptionConveyor;
        private string StatusConveyor;
        private bool FaultConveyor;
        private string _ObjectNo;

        public Bagging_ClosingConveyor_MWTC()
        {
            InitializeComponent();
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


        //------------------------------------------------------------------------------//
        //                                 Methods                                      //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            BordermainBelt.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                BordermainBelt.Background = brushColor;
            }));

            ellipseLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                ellipseLeft.Fill = brushColor;
            }));

            ellipseRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                ellipseRight.Fill = brushColor;
            }));

            rectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectMain.Fill = brushColor;
            }));

            rectSmall.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectSmall.Fill = brushColor;
            }));

            rectPole.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectPole.Fill = brushColor;
            }));
        }    
    }
}
