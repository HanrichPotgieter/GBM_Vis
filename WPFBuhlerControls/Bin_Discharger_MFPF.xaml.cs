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
    /// Interaction logic for Bin_Discharger_MFPF.xaml
    /// </summary>
    public partial class Bin_Discharger_MFPF : UserControl
    {
        private int _DischargerColor;
        private string DescriptionDischarger;
        private string StatusDischarger;
        private bool FaultDischarger;
        private string _ObjectNo;
        private string _PLCName;

        public Bin_Discharger_MFPF()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int DischargerColor
        {
            get
            {
                return _DischargerColor;
            }
            set
            {
                _DischargerColor = value;
                FB11 discharger = new FB11();
                _SetMotorColor(discharger.SetColor(value));
                StatusDischarger = discharger.Status_DO_Element;
                FaultDischarger = discharger.Fault_DO_Element;
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
        public string Description_Discharger
        {
            get
            {
                return this.DescriptionDischarger;
            }
            set
            {
                this.DescriptionDischarger = value;
            }
        }

        [Category("Buhler")]
        public string Status_Discharger
        {
            get
            {
                return this.StatusDischarger;
            }
        }

        [Category("Buhler")]
        public bool Fault_Discharger
        {
            get
            {
                return this.FaultDischarger;
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
        private void _SetMotorColor(Brush brushColor)
        {           
            polyMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyMain.Fill = brushColor;
            }));

            polySmall.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polySmall.Fill = brushColor;
            }));
        }
    }
}
