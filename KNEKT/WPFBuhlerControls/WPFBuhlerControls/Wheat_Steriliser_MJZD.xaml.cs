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
    /// Interaction logic for Wheat_Steriliser_MJZD.xaml
    /// </summary>
    public partial class Wheat_Steriliser_MJZD : UserControl
    {
        private int _MotorColor;
        private string _DescriptionWheat_Steriliser;
        private string _StatusWheat_Steriliser;
        private bool _FaultWheat_Steriliser;
        private string _ObjectNo;
        private string _PLCName;

        public Wheat_Steriliser_MJZD()
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
                FB12 Wheat_Steriliser = new FB12();
                _SetColor(Wheat_Steriliser.SetColor(value));
                _StatusWheat_Steriliser = Wheat_Steriliser.Status_Motor;
                _FaultWheat_Steriliser = Wheat_Steriliser.Fault_Motor;
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
        public string DescriptionWheat_Steriliser
        {
            get
            {
                return this._DescriptionWheat_Steriliser;
            }
            set
            {
                this._DescriptionWheat_Steriliser = value;
            }
        }

        [Category("Buhler")]
        public string StatusWheat_Steriliser
        {
            get
            {
                return this._StatusWheat_Steriliser;
            }
        }

        [Category("Buhler")]
        public bool FaultWheat_Steriliser
        {
            get
            {
                return this._FaultWheat_Steriliser;
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
            RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectMain.Fill = brushColor;
            }));

            RectSmall.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectSmall.Fill = brushColor;
            }));

            PolyBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyBot.Fill = brushColor;
            }));
        }
    }
}
