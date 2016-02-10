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
    /// Interaction logic for BaggingStation_BigBag.xaml
    /// </summary>
    public partial class BaggingStation_BigBag : UserControl
    {
        private int _BaggingStationColor;
        private string DescriptionBaggingStation;
        private string StatusBaggingStation;
        private eScaleType _ScaleType;
        private bool FaultBaggingStation;

        public BaggingStation_BigBag()
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
                if (Scale_ScaleType == eScaleType.CWA)
                {
                    FB97 cwa = new FB97();
                    _SetColor(cwa.SetColor(value));
                    StatusBaggingStation = cwa.Status_CWA;
                    FaultBaggingStation = cwa.Fault_CWA;
                }
                else if (Scale_ScaleType == eScaleType.CWA_DMST)
                {
                    FB83 cwa = new FB83();
                    _SetColor(cwa.SetColor(value));
                    StatusBaggingStation = cwa.Status_MEAF;
                    FaultBaggingStation = cwa.Fault_MEAF;
                }
            }
        }

        [Category("Buhler")]
        public eScaleType Scale_ScaleType
        {
            get
            {
                return _ScaleType;
            }
            set
            {
                _ScaleType = value;
            }
        }

        [Category("Buhler")]
        public string Description_BaggingStation
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
            rectTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectTop.Fill = brushColor;
            }));

            rectLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectLeft.Fill = brushColor;
            }));

            rectBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectBot.Fill = brushColor;
            }));

            rectLeftPillar.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectLeftPillar.Fill = brushColor;
            }));

            rectRightPillar.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectRightPillar.Fill = brushColor;
            }));

        }
    }
}
