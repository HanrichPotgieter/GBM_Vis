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
    /// Interaction logic for Conveyor_SpeedMonitor_R.xaml
    /// </summary>
    public partial class Conveyor_SpeedMonitor_R : UserControl
    {
        private int _SpeedMonitorColor;
        private string DescriptionSpeedMonitor;
        private string StatusSpeedMonitor;
        private bool FaultSpeedMonitor;
        private string _ObjectNo;
        private string _PLCName;

        public Conveyor_SpeedMonitor_R()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int SpeedMonitorColor
        {
            get
            {
                return _SpeedMonitorColor;
            }
            set
            {
                _SpeedMonitorColor = value;
                FB14 monitor = new FB14();
                _SetColor(monitor.SetColor(value, 7135));
                StatusSpeedMonitor = monitor.Status_DI_Status;
                FaultSpeedMonitor = monitor.Fault_DI_Element;
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
        public string Description_SpeedMonitor
        {
            get
            {
                return this.DescriptionSpeedMonitor;
            }
            set
            {
                this.DescriptionSpeedMonitor = value;
            }
        }

        [Category("Buhler")]
        public string Status_SpeedMonitor
        {
            get
            {
                return this.StatusSpeedMonitor;
            }
        }

        [Category("Buhler")]
        public bool Fault_SpeedMonitor
        {
            get
            {
                return this.FaultSpeedMonitor;
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
            ellipseBig.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                ellipseBig.Fill = brushColor;
            }));
        }
    }
}
