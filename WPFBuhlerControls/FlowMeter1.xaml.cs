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
    /// Interaction logic for FlowMeter1.xaml
    /// </summary>
    public partial class FlowMeter1 : UserControl
    {        
        private int _MeterColor;
        private string DescriptionMeter;
        private string StatusMeter;
        private bool FaultMeter;
        private string _ObjectNo;
        private string _PLCName;

        public FlowMeter1()
        {
            InitializeComponent();
        }


        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int MeterColor
        {
            get
            {
                return _MeterColor;
            }
            set
            {
                _MeterColor = value;
                FB97 cwa = new FB97();
                _SetColor(cwa.SetColor(value));
                StatusMeter = cwa.Status_CWA;
                FaultMeter = cwa.Fault_CWA;

            }
        }

        [Category("Buhler")]
        public string Description_Meter
        {
            get
            {
                return this.DescriptionMeter;
            }
            set
            {
                this.DescriptionMeter = value;
            }
        }

        [Category("Buhler")]
        public string Status_Meter
        {
            get
            {
                return this.StatusMeter;
            }
        }

        [Category("Buhler")]
        public bool Fault_Meter
        {
            get
            {
                return this.FaultMeter;
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

        //------------------------------------------------------------------------------//
        //                                   Methods                                    //
        //------------------------------------------------------------------------------//

        private void _SetColor(Brush brushColor)
        {
            ellipseMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                ellipseMain.Fill = brushColor;
            }));
        }              
    }
}
