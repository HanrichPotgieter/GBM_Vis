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
    /// Interaction logic for MYFD.xaml
    /// </summary>
    public partial class MYFD : UserControl
    {
        private int _MYFDColor;
        private string DescriptionMYFD;
        private string StatusMYFD;
        private bool FaultMYFD;
        private string _ObjectNo;
        private string _PLCName;
        public MYFD()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int MYFDColor
        {
            get
            {
                return _MYFDColor;
            }
            set
            {
                _MYFDColor = value;
                FB34 MYFD = new FB34();
                _SetColor(MYFD.SetColor(value));
                StatusMYFD = MYFD.Status_MYFC;
                FaultMYFD = MYFD.Fault_MYFC;
            }
        }

        [Category("Buhler")]
        public string Description_MYFD
        {
            get
            {
                return this.DescriptionMYFD;
            }
            set
            {
                this.DescriptionMYFD = value;
            }
        }

        [Category("Buhler")]
        public string Status_MYFD
        {
            get
            {
                return this.StatusMYFD;
            }
        }

        [Category("Buhler")]
        public bool Fault_MYFD
        {
            get
            {
                return this.FaultMYFD;
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
        //                                     Methods                                  //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            RectOuter.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectOuter.Fill = brushColor;
            }));

            PolyBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyBot.Fill = brushColor;
            }));
        }
    }
}
