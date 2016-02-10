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
    /// Interaction logic for Filter_MVTA1.xaml
    /// </summary>
    public partial class Filter_MVTA1 : UserControl
    {

        private int _FilterColor;
        private string DescriptionFilter;
        private string StatusFilter;
        private bool FaultFilter;
        private string _ObjectNo;
        private string _PLCName;

        public Filter_MVTA1()
        {
            InitializeComponent();
        }


        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int FilterColor
        {
            get
            {
                return _FilterColor;
            }
            set
            {
                _FilterColor = value;
                FB14 Filter = new FB14();
                _SetColor(Filter.SetColor(value, 7147));
                StatusFilter = Filter.Status_DI_Status;
                FaultFilter = Filter.Fault_DI_Element;
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
        public string Description_Filter
        {
            get
            {
                return this.DescriptionFilter;
            }
            set
            {
                this.DescriptionFilter = value;
            }
        }

        [Category("Buhler")]
        public string Status_Filter
        {
            get
            {
                return this.StatusFilter;
            }
        }

        [Category("Buhler")]
        public bool Fault_Filter
        {
            get
            {
                return this.FaultFilter;
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

            RectTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectTop.Fill = brushColor;
            }));

            RectBot1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectBot1.Fill = brushColor;
            }));

            RectBottom.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectBottom.Fill = brushColor;
            }));

            PolyBot1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyBot1.Fill = brushColor;
            }));

            PolyBot2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyBot2.Fill = brushColor;
            }));
            
            PolyBot2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyBot2.Fill = brushColor;
            }));

            PolyBot3.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyBot3.Fill = brushColor;
            }));

            PolyTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyTop.Fill = brushColor;
            }));
        }
    }
}
