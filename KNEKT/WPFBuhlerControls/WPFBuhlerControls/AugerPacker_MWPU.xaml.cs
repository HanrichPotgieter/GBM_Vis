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
    /// Interaction logic for AugerPacker_MWPU.xaml
    /// </summary>
    public partial class AugerPacker_MWPU : UserControl
    {
        private string DescriptionBaggingMachine;
        private int _BaggingStationColor;
        private string StatusBaggingStation;
        private bool FaultBaggingStation;

        public AugerPacker_MWPU()
        {
            InitializeComponent();
        }


        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
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
                _SetMotorColor(monitor.SetColor(value, 7147));
                StatusBaggingStation = monitor.Status_DI_Status;
                FaultBaggingStation = monitor.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public string Description_BaggingMachine
        {
            get
            {
                return this.DescriptionBaggingMachine;
            }
            set
            {
                this.DescriptionBaggingMachine = value;
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
        //                             Set Color Methods                                //
        //------------------------------------------------------------------------------//
        private void _SetMotorColor(Brush brushColor)
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

            //ellipseMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            //{
            //    ellipseMain.Fill = brushColor;
            //}));

            polyTriangle.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyTriangle.Fill = brushColor;
            }));
        }
    }
}
