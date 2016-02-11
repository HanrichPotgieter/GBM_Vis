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

namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for Bin_ConeBottom.xaml
    /// </summary>
    public partial class Bin_ConeBottom : UserControl
    {
        private bool _BinLocked = false;
        private Brush _BinSelectionBrush;

        public Bin_ConeBottom()
        {
            InitializeComponent();
            _BinLocked = false;
        }


        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public string Bin_BinText
        {
            get
            {
                return txtBin.Text;
            }
            set
            {
                txtBin.Text = value;
            }
        }

        [Category("Buhler")]
        public bool BinLocked
        {
            get
            {
                return _BinLocked;
            }
            set
            {
                _BinLocked = value;

                if (_BinLocked)
                {
                    polyMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                    {
                        polyMain.Fill = Brushes.Tomato;
                    }));
                }
                else
                {
                    polyMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                    {
                        polyMain.Fill = KNEKTColors.BinColor;
                    }));
                }
            }
        }

        [Category("Buhler")]
        public Brush BinSelectionBrush
        {
            get
            {
                return _BinSelectionBrush;
            }
            set
            {
                _BinSelectionBrush = value;

                rectTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    rectTop.Fill = value;
                }));
            }
        }
    }
}
