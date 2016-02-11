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
    /// Interaction logic for Sifter_MPAK_1024.xaml
    /// </summary>
    public partial class Sifter_MPAK_1024 : UserControl
    {
        private int _MotorColor;
        private string DescriptionSifter;
        private string StatusSifter;
        private bool FaultSifter;
        private string _ObjectNo;
        private string _PLCName;

        public Sifter_MPAK_1024()
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
                _SetColorMotor(Motor.SetColor(value));
                StatusSifter = Motor.Status_Motor;
                FaultSifter = Motor.Fault_Motor;
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

        [Category("Buhler")]
        public string Status_Sifter
        {
            get
            {
                return this.StatusSifter;
            }
        }

        [Category("Buhler")]
        public bool Fault_Sifter
        {
            get
            {
                return this.FaultSifter;
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
        private void _SetColorMotor(Brush brushColor)
        {
            RectLeft1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectLeft1.Fill = brushColor;
            }));

            RectLeft2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectLeft2.Fill = brushColor;
            }));

            RectLeft3.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectLeft3.Fill = brushColor;
            }));

            RectMid.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectMid.Fill = brushColor;
            }));

            RectRight1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectRight1.Fill = brushColor;
            }));

            RectRight2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectRight2.Fill = brushColor;
            }));

            RectRight3.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectRight3.Fill = brushColor;
            }));
        }
    }
}
