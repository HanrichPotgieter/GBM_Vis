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
    /// Interaction logic for Magnet_Rotary1.xaml
    /// </summary>
    public partial class Magnet_Rotary1 : UserControl
    {
        private int _MotorColor;
        private string DescriptionMagnet_Rotary;
        private string StatusMagnet_Rotary;
        private bool FaultMagnet_Rotary;
        private string _ObjectNo;
        private string _PLCName;

        public Magnet_Rotary1()
        {
            InitializeComponent();

            //RectMain.Fill = KNEKTColors.NoControlColor;
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
                StatusMagnet_Rotary = Motor.Status_Motor;
                FaultMagnet_Rotary = Motor.Fault_Motor;
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


        [Category("Buhler")]
        public string Description_Magnet_Rotary
        {
            get
            {
                return this.DescriptionMagnet_Rotary;
            }
            set
            {
                this.DescriptionMagnet_Rotary = value;
            }
        }

        [Category("Buhler")]
        public string Status_Magnet_Rotary
        {
            get
            {
                return this.StatusMagnet_Rotary;
            }
        }

        [Category("Buhler")]
        public bool Fault_Magnet_Rotary
        {
            get
            {
                return this.FaultMagnet_Rotary;
            }
        }

        //------------------------------------------------------------------------------//
        //                                 Methods                                      //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectMain.Fill = brushColor;
            }));
        }
    }
}
