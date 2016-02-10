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
    public enum eAirValve1FB
    {
        FB11,
        FB13
    }

    /// <summary>
    /// Interaction logic for AirValve1.xaml
    /// </summary>
    public partial class AirValve1 : UserControl
    {
        private int _ValveColor;
        private string DescriptionValve;
        private string StatusValve;
        private bool FaultValve;
        private eAirValve1FB _FB;
        private string _ObjectNo;
        private string _PLCName;

        public AirValve1()
        {
            InitializeComponent();

            FB = eAirValve1FB.FB11;
        }

        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int ValveColor
        {
            get
            {
                return _ValveColor;
            }
            set
            {
                _ValveColor = value;

                if (FB == eAirValve1FB.FB11)
                {
                    FB11 Valve = new FB11();
                    _SetColorMotor(Valve.SetColor(value));
                    StatusValve = Valve.Status_DO_Element;
                    FaultValve = Valve.Fault_DO_Element;
                }
                else if (FB == eAirValve1FB.FB13)
                {
                    FB13 Valve = new FB13();
                    _SetColorMotor(Valve.SetColor(value));
                    StatusValve = Valve.Status_Slide;
                    FaultValve = Valve.Fault_Slide;
                }
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
        public string Description_Valve
        {
            get
            {
                return this.DescriptionValve;
            }
            set
            {
                this.DescriptionValve = value;
            }
        }

        [Category("Buhler")]
        public eAirValve1FB FB
        {
            get
            {
                return this._FB;
            }
            set
            {
                this._FB = value;
            }
        }

        [Category("Buhler")]
        public string Status_Valve
        {
            get
            {
                return this.StatusValve;
            }
        }

        [Category("Buhler")]
        public bool Fault_Valve
        {
            get
            {
                return this.FaultValve;
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
            rectMain.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        rectMain.Fill = brushColor;
                    }
            ));
        }
    }
}
