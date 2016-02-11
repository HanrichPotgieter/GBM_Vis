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
    /// Interaction logic for Detacher_MJZF_Left.xaml
    /// </summary>
    public partial class Detacher_MJZF_Left : UserControl
    {
        private int _MotorColor;
        private string DescriptionDetacher;
        private string StatusDetacher;
        private bool FaultDetacher;
        private string _ObjectNo;
        private string _PLCName;

        public Detacher_MJZF_Left()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                   Properties                                 //
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
                StatusDetacher = Motor.Status_Motor;
                FaultDetacher = Motor.Fault_Motor;
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
        public string Description_Detacher
        {
            get
            {
                return this.DescriptionDetacher;
            }
            set
            {
                this.DescriptionDetacher = value;
            }
        }

        [Category("Buhler")]
        public string Status_Detacher
        {
            get
            {
                return this.StatusDetacher;
            }
        }

        [Category("Buhler")]
        public bool Fault_Detacher
        {
            get
            {
                return this.FaultDetacher;
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
            rectSmallTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                rectSmallTop.Fill = brushColor;
            }));

            rectMid.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                rectMid.Fill = brushColor;
            }));

            rectBottom.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                rectBottom.Fill = brushColor;
            }));
        }
    }
}
