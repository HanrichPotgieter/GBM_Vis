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
    /// Interaction logic for Monitor_SifterOutlet.xaml
    /// </summary>
    public partial class Monitor_SifterOutlet : UserControl
    {
        private int _MonitorColor;
        private string DescriptionMonitor;
        private string StatusMonitor;
        private bool FaultMonitor;
        private string _ObjectNo;
        private string _PLCName;

        public Monitor_SifterOutlet()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                Properties                                    //
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
                _SetColor(monitor.SetColor(value, 7154));
                StatusMonitor = monitor.Status_DI_Status;
                FaultMonitor = monitor.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public string Description_Monitor
        {
            get
            {
                return this.DescriptionMonitor;
            }
            set
            {
                this.DescriptionMonitor = value;
            }
        }

        [Category("Buhler")]
        public string Status_Monitor
        {
            get
            {
                return this.StatusMonitor;
            }
        }

        [Category("Buhler")]
        public bool Fault_Monitor
        {
            get
            {
                return this.FaultMonitor;
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
        //                                    Methods                                   //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            rectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                rectMain.Fill = brushColor;
            }));
        }
    }
}
