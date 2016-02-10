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
    /// Interaction logic for MYFC.xaml
    /// </summary>
    public partial class MYFC : UserControl
    {
        private int _MYFCColor;
        private string DescriptionMYFC;
        private string StatusMYFC;
        private bool FaultMYFC;
        private string _ObjectNo;
        private string _PLCName;
        public MYFC()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int MYFCColor
        {
            get
            {
                return _MYFCColor;
            }
            set
            {
                _MYFCColor = value;
                FB34 MYFC = new FB34();
                _SetColor(MYFC.SetColor(value));
                StatusMYFC = MYFC.Status_MYFC;
                FaultMYFC = MYFC.Fault_MYFC;
            }
        }

        [Category("Buhler")]
        public string Description_MYFC
        {
            get
            {
                return this.DescriptionMYFC;
            }
            set
            {
                this.DescriptionMYFC = value;
            }
        }

        [Category("Buhler")]
        public string Status_MYFC
        {
            get
            {
                return this.StatusMYFC;
            }
        }

        [Category("Buhler")]
        public bool Fault_MYFC
        {
            get
            {
                return this.FaultMYFC;
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
            RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectMain.Fill = brushColor;      
            }));

            RectSmall.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectSmall.Fill = brushColor;      
            }));

            PolyBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                PolyBot.Fill = brushColor;      
            }));

            PolyTopLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                PolyTopLeft.Fill = brushColor;      
            }));

            PolyTopRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                PolyTopRight.Fill = brushColor;      
            }));            
        }
    }
}
