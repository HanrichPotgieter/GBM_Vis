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
    /// Interaction logic for Sifter_DMHX.xaml
    /// </summary>
    public partial class Sifter_DMHX : UserControl
    {
        private int _MotorColor;
        private string DescriptionSifter;
        private string StatusSifter;
        private bool FaultSifter;
        private string _ObjectNo;
        private string _PLCName;

        public Sifter_DMHX()
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
        //                                   Methods                                    //
        //------------------------------------------------------------------------------//
        private void _SetColorMotor(Brush brushColor)
        {
            PolyRightInlet.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyRightInlet.Fill = brushColor;
            }));

            PolyLeftOutlet.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyLeftOutlet.Fill = brushColor;
            }));
            PolyMainRect.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyMainRect.Fill = brushColor;
            }));
            PolyBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyBot.Fill = brushColor;
            }));
            //PolyLeftOutlet.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            //{
            //    PolyLeftOutlet.Fill = brushColor;
            //}));
        }
    }
}
