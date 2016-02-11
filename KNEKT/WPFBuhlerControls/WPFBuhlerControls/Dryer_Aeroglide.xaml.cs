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
    /// Interaction logic for Dryer_Aeroglide.xaml
    /// </summary>
    public partial class Dryer_Aeroglide : UserControl
    {
        private int _ColorDryer;
        private string DescriptionDryer;
        private string StatusDryer;
        private bool FaultDryer;
        private string _ObjectNo;
        private string _PLCName;

        public Dryer_Aeroglide()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int ColorDryer
        {
            get
            {
                return _ColorDryer;
            }
            set
            {
                _ColorDryer = value;
                FB1024 dryer = new FB1024();
                _SetColor(dryer.SetColor(value));
                StatusDryer = dryer.Status_Dryer;
                FaultDryer = dryer.Fault_Dryer;
            }
        }

        [Category("Buhler")]
        public string Description_Dryer
        {
            get
            {
                return this.DescriptionDryer;
            }
            set
            {
                this.DescriptionDryer = value;
            }
        }

        [Category("Buhler")]
        public string Status_Dryer
        {
            get
            {
                return this.StatusDryer;
            }
        }

        [Category("Buhler")]
        public bool Fault_Dryer
        {
            get
            {
                return this.FaultDryer;
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
            polyDryer.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyDryer.Fill = brushColor;
            }));

            polyTopLeftSpout.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyTopLeftSpout.Fill = brushColor;
            }));
        }
    }
}
