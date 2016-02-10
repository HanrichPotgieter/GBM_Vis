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
    /// Interaction logic for SpeedMonitor.xaml
    /// </summary>
    public partial class SpeedMonitor : UserControl
    {
        private int _MonitorColor;
        private string DescriptionSpeedMon;
        private string StatusSpeedMon;
        private bool FaultSpeedMon;
        private string _ObjectNo;
        private string _PLCName;

        public SpeedMonitor()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int MonitorColor
        {
            get
            {
                return _MonitorColor;
            }
            set
            {
                _MonitorColor = value;
                FB14 monitor = new FB14();
                _SetColor(monitor.SetColor(value, 7147));
                StatusSpeedMon = monitor.Status_DI_Status;
                FaultSpeedMon = monitor.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public string Description_SpeedMonitor
        {
            get
            {
                return this.DescriptionSpeedMon;
            }
            set
            {
                this.DescriptionSpeedMon = value;
            }
        }

        [Category("Buhler")]
        public string Status_SpeedMonitor
        {
            get
            {
                return this.StatusSpeedMon;
            }
        }

        [Category("Buhler")]
        public bool Fault_SpeedMonitor
        {
            get
            {
                return this.FaultSpeedMon;
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
        //                                 Methods                                      //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            elipseMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                elipseMain.Fill = brushColor;
            }));
        }
    }
}
