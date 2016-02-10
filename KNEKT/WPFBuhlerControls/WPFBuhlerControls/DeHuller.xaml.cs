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
    /// Interaction logic for DeHuller.xaml
    /// </summary>
    public partial class DeHuller : UserControl
    {
        private int _DehullerColor;
        private string DescriptionDehuller;
        private string StatusDehuller;
        private bool FaultDehuller;
        private string _ObjectNo;
        private string _PLCName;

        public DeHuller()
        {
            InitializeComponent();
        }

        [Category("Buhler")]
        public int DeHullerColor
        {
            get
            {
                return _DehullerColor;
            }
            set
            {
                _DehullerColor = value;
                FB833 dehuller = new FB833();
                _SetColor(dehuller.SetColor(value));
                StatusDehuller = dehuller.Status_ImpactHuller;
                FaultDehuller = dehuller.Fault_ImpactHuller;
            }
        }

        [Category("Buhler")]
        public string Description_Dehuller
        {
            get
            {
                return this.DescriptionDehuller;
            }
            set
            {
                this.DescriptionDehuller = value;
            }
        }

        [Category("Buhler")]
        public string Status_Dehuller
        {
            get
            {
                return this.StatusDehuller;
            }
        }

        [Category("Buhler")]
        public bool Fault_Dehuller
        {
            get
            {
                return this.FaultDehuller;
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
            polyLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyLeft.Fill = brushColor;
            }));

            polyTopLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyTopLeft.Fill = brushColor;
            }));

            polyMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyMain.Fill = brushColor;
            }));
        }
    }
}
