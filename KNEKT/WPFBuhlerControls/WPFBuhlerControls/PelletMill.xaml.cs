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
    /// Interaction logic for PelletMill.xaml
    /// </summary>
    public partial class PelletMill : UserControl
    {
        private int _PelletMillColor;
        private string DescriptionPelletMill;
        private string StatusPelletMill;
        private bool FaultPelletMill;
        private string _ObjectNo;
        private string _PLCName;

        public PelletMill()
        {
            InitializeComponent();
        }

        public int PelletMillColor
        {
            get
            {
                return _PelletMillColor;
            }
            set
            {
                _PelletMillColor = value;
                FB73 PelletMill = new FB73();
                _SetColor(PelletMill.SetColor(value));
                StatusPelletMill = PelletMill.Status_PelletMill;
                FaultPelletMill = PelletMill.Fault_PelletMill;
            }
        }

        [Category("Buhler")]
        public string Description_PelletMill
        {
            get
            {
                return this.DescriptionPelletMill;
            }
            set
            {
                this.DescriptionPelletMill = value;
            }
        }

        [Category("Buhler")]
        public string Status_PelletMill
        {
            get
            {
                return this.StatusPelletMill;
            }
            set
            {
                StatusPelletMill = value;
            }
        }

        [Category("Buhler")]
        public bool Fault_PelletMill
        {
            get
            {
                return this.FaultPelletMill;
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
        //                                   Methods                                    //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            PolyL.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyL.Fill = brushColor;
            }));

            RectLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectLeft.Fill = brushColor;
            }));

            RectRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectRight.Fill = brushColor;
            }));
        }
    }
}
