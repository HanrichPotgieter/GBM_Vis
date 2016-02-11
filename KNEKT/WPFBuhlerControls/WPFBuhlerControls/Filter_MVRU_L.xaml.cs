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
    /// Interaction logic for Filter_MVRU_L.xaml
    /// </summary>
    public partial class Filter_MVRU_L : UserControl
    {
        private string DescriptionFilter;
        private string StatusFilter;
        private bool FaultFilter;
        private string _ObjectNo;
        private string _PLCName;

        public Filter_MVRU_L()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                             Airlock Properties                               //
        //------------------------------------------------------------------------------//

        public int FilterColor
        {
            set
            {
                FB11 filter = new FB11();
                _SetColor(filter.SetColor(value));
                StatusFilter = filter.Status_DO_Element;
                FaultFilter = filter.Fault_DO_Element;
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
        //                               Color Methods                                  //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            rectMain.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        rectMain.Fill = brushColor;
                    }
            ));

            rectTop.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        rectTop.Fill = brushColor;
                    }
            ));

            rectBot1.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        rectBot1.Fill = brushColor;
                    }
            ));

            rectBot2.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        rectBot2.Fill = brushColor;
                    }
            ));

            PolyBot.Dispatcher.BeginInvoke(
                 System.Windows.Threading.DispatcherPriority.Normal,
                 new Action(
                   delegate()
                   {
                       PolyBot.Fill = brushColor;
                   }
           ));
        }
    }
}
