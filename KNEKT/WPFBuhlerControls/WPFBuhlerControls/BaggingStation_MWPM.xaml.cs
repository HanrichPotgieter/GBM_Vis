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
    /// Interaction logic for BaggingStation_MWPM.xaml
    /// </summary>
    public partial class BaggingStation_MWPM : UserControl
    {
        private int _BaggingStationColor;
        private string DescriptionBaggingStation;
        private string StatusBaggingStation;
        private bool FaultBaggingStation;

        public BaggingStation_MWPM()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int BaggingStationColor
        {
            get
            {
                return _BaggingStationColor;
            }
            set
            {
                _BaggingStationColor = value;
                FB14 monitor = new FB14();
                _SetColor(monitor.SetColor(value, 7147));
                StatusBaggingStation = monitor.Status_DI_Status;
                FaultBaggingStation = monitor.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public string Description_Monitor
        {
            get
            {
                return this.DescriptionBaggingStation;
            }
            set
            {
                this.DescriptionBaggingStation = value;
            }
        }

        [Category("Buhler")]
        public string Status_BaggingStation
        {
            get
            {
                return this.StatusBaggingStation;
            }
        }

        [Category("Buhler")]
        public bool Fault_BaggingStation
        {
            get
            {
                return this.FaultBaggingStation;
            }
        }

        //------------------------------------------------------------------------------//
        //                                 Methods                                      //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            rectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectMain.Fill = brushColor;
            }));

            rectMid1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectMid1.Fill = brushColor;
            }));

            rectMid2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectMid2.Fill = brushColor;
            }));

            rectMid3.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectMid3.Fill = brushColor;
            }));

            rectMid4.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectMid4.Fill = brushColor;
            }));

            rectMid5.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectMid5.Fill = brushColor;
            }));

            rectMid6.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectMid6.Fill = brushColor;
            }));

            ellipseMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                ellipseMain.Fill = brushColor;
            }));

            polyTriangle1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyTriangle1.Fill = brushColor;
            }));

            polyTriangle2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyTriangle2.Fill = brushColor;
            }));

            polyTriangle3.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyTriangle3.Fill = brushColor;
            }));
        }
    }
}
