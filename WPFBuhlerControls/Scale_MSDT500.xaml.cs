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
    /// Interaction logic for Scale_MSDT500.xaml
    /// </summary>
    public partial class Scale_MSDT500 : UserControl
    {
        private int _ScaleColor;
        private string DescriptionScale;
        private string StatusScale;
        private bool FaultScale;
        private string _ObjectNo;
        private string _PLCName;

        public Scale_MSDT500()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int ScaleColor
        {
            get
            {
                return _ScaleColor;
            }
            set
            {
                _ScaleColor = value;
                FB40 Scale = new FB40();
                _SetColor(Scale.SetColor(value));
                StatusScale = Scale.Status_Scale;
                FaultScale = Scale.Fault_Scale;
            }
        }

        [Category("Buhler")]
        public string Description_Scale
        {
            get
            {
                return this.DescriptionScale;
            }
            set
            {
                this.DescriptionScale = value;
            }
        }

        [Category("Buhler")]
        public string Status_Scale
        {
            get
            {
                return this.StatusScale;
            }
        }

        [Category("Buhler")]
        public bool Fault_Scale
        {
            get
            {
                return this.FaultScale;
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
            polyBottom.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyBottom.Fill = brushColor;
            }));

            polyTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyTop.Fill = brushColor;
            }));

            polyMid1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyMid1.Fill = brushColor;
            }));

            polyMid2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyMid2.Fill = brushColor;
            }));

            RectMid.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectMid.Fill = brushColor;
            }));
        }
    }
}
