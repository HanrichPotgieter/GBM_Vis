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
    /// Interaction logic for Magnet_MMUA_V2.xaml
    /// </summary>
    public partial class Magnet_MMUA_V2 : UserControl
    {
        private int _MagnetColor;
        private string DescriptionMagnet;
        private string StatusMagnet;
        private bool FaultMagnet;
        private string _ObjectNo;
        private string _PLCName;

        public Magnet_MMUA_V2()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int MagnetColor
        {
            get
            {
                return _MagnetColor;
            }
            set
            {
                _MagnetColor = value;
                FB14 magnet = new FB14();
                _SetColor(magnet.SetColor(value, 7154));
                StatusMagnet = magnet.Status_DI_Status;
                FaultMagnet = magnet.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public string Description_Magnet
        {
            get
            {
                return this.DescriptionMagnet;
            }
            set
            {
                this.DescriptionMagnet = value;
            }
        }

        [Category("Buhler")]
        public string Status_Magnet
        {
            get
            {
                return this.StatusMagnet;
            }
        }

        [Category("Buhler")]
        public bool Fault_Magnet
        {
            get
            {
                return this.FaultMagnet;
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
            RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectMain.Fill = brushColor;
            }));
        }
    }
}
