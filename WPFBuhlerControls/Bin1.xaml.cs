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
    /// Interaction logic for Bin1.xaml
    /// </summary>
    public partial class Bin1 : UserControl
    {
        private bool _BinLocked = false;
        private Brush _BinSelectionBrush;
        private Brush _BinActiveColorBrush;

        public Bin1()
        {
            InitializeComponent();        
            BinLocked = false;
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
                    polybin.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                    {
                        polybin.Fill = KNEKTColors.BinLockedColor;
                    }));                
                }
                else
                {
                    polybin.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
                    {
                        // Create a linear gradient brush with five stops 
                        LinearGradientBrush BinConeGradient = new LinearGradientBrush();
                        BinConeGradient.StartPoint = new Point(0, 0);
                        BinConeGradient.EndPoint = new Point(1, 0);

                        // Create and add Gradient stops
                        GradientStop Yellow = new GradientStop();
                        Yellow.Color = Colors.DarkGoldenrod;
                        Yellow.Offset = 0.0;
                        BinConeGradient.GradientStops.Add(Yellow);

                        // Create and add Gradient stops
                        GradientStop White1 = new GradientStop();
                        White1.Color = Colors.LightGoldenrodYellow;
                        White1.Offset = 0.3;
                        BinConeGradient.GradientStops.Add(White1);

                        // Create and add Gradient stops
                        GradientStop White2 = new GradientStop();
                        White2.Color = Colors.LightGoldenrodYellow;
                        White2.Offset = 0.7;
                        BinConeGradient.GradientStops.Add(White2);

                        // Create and add Gradient stops
                        GradientStop YellowR = new GradientStop();
                        YellowR.Color = Colors.DarkGoldenrod;
                        YellowR.Offset = 1.0;
                        BinConeGradient.GradientStops.Add(YellowR);

                        polybin.Fill = BinConeGradient;
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


        [Category("Buhler")]
        public Brush BinActiveColor
        {
            get
            {
                return _BinActiveColorBrush;
            }
            set
            {
                _BinActiveColorBrush = value;

                polybin.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    polybin.Fill = value;
                }));

            }
        }

    }
}
