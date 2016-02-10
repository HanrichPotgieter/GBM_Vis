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
    /// Interaction logic for DampenerTurbolizer_MOZL.xaml
    /// </summary>
    public partial class DampenerTurbolizer_MOZL : UserControl
    {
        private int _MotorColor;
        private string DescriptionDampener;
        private string StatusDampener;
        private bool FaultDampener;
        private string _ObjectNo;
        private string _PLCName;

        public DampenerTurbolizer_MOZL()
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
                FB12 motor = new FB12();
                _SetColor(motor.SetColor(value));
                StatusDampener = motor.Status_Motor;
                FaultDampener = motor.Fault_Motor;
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
        public string Description_Dampener
        {
            get
            {
                return this.DescriptionDampener;
            }
            set
            {
                this.DescriptionDampener = value;
            }
        }

        [Category("Buhler")]
        public string Status_Dampener
        {
            get
            {
                return this.StatusDampener;
            }
        }

        [Category("Buhler")]
        public bool Fault_Dampener
        {
            get
            {
                return this.FaultDampener;
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
            RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectMain.Fill = brushColor;
            }));

            RectSmall1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectSmall1.Fill = brushColor;
            }));

            RectSmall2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectSmall2.Fill = brushColor;
            }));

            PolyMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                PolyMain.Fill = brushColor;
            }));
        }
    }
}
