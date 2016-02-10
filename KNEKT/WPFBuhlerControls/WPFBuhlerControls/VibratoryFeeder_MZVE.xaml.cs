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
    /// Interaction logic for VibratoryFeeder_MZVE.xaml
    /// </summary>
    public partial class VibratoryFeeder_MZVE : UserControl
    {
        private int _FeederColor;
        private string DescriptionFeeder;
        private string StatusFeeder;
        private bool FaultFeeder;
        private string _ObjectNo;
        private string _PLCName;

        public VibratoryFeeder_MZVE()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int FeederColor
        {
            get
            {
                return _FeederColor;
            }
            set
            {
                _FeederColor = value;
                FB11 DO = new FB11();
                _SetColorMotor(DO.SetColor(value));
                StatusFeeder = DO.Status_DO_Element;
                FaultFeeder = DO.Fault_DO_Element;
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
        public string Description_Feeder
        {
            get
            {
                return this.DescriptionFeeder;
            }
            set
            {
                this.DescriptionFeeder = value;
            }
        }

        [Category("Buhler")]
        public string Status_Feeder
        {
            get
            {
                return this.StatusFeeder;
            }
        }

        [Category("Buhler")]
        public bool Fault_Feeder
        {
            get
            {
                return this.FaultFeeder;
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
            rectSmallTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectSmallTop.Fill = brushColor;
            }));

            rectMid.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectMid.Fill = brushColor;
            }));

            rectSmallBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectSmallBot.Fill = brushColor;
            }));

            polyBottom1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyBottom1.Fill = brushColor;
            }));

            polyBottom2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyBottom2.Fill = brushColor;
            }));
        }
    }
}
