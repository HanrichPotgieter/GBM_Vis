﻿using System;
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
    /// Interaction logic for Bin.xaml
    /// </summary>
    public partial class Bin : UserControl
    {
        private bool _BinLocked = false;
        private Brush _BinSelectionBrush;

        public Bin()
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
                    RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
                     {
                         RectMain.Fill = Brushes.Tomato;
                     }));
                }
                else
                {
                    RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
                    {
                        // Create a linear gradient brush with five stops 
                        LinearGradientBrush BinGradient = new LinearGradientBrush();
                        BinGradient.StartPoint = new Point(0, 0);
                        BinGradient.EndPoint = new Point(1, 0);

                        // Create and add Gradient stops
                        GradientStop Yellow = new GradientStop();
                        Yellow.Color = Colors.DarkGoldenrod;
                        Yellow.Offset = 0.0;
                        BinGradient.GradientStops.Add(Yellow);

                        // Create and add Gradient stops
                        GradientStop White1 = new GradientStop();
                        White1.Color = Colors.LightGoldenrodYellow;
                        White1.Offset = 0.3;
                        BinGradient.GradientStops.Add(White1);

                        // Create and add Gradient stops
                        GradientStop White2 = new GradientStop();
                        White2.Color = Colors.LightGoldenrodYellow;
                        White2.Offset = 0.7;
                        BinGradient.GradientStops.Add(White2);

                        // Create and add Gradient stops
                        GradientStop YellowR = new GradientStop();
                        YellowR.Color = Colors.DarkGoldenrod;
                        YellowR.Offset = 1.0;
                        BinGradient.GradientStops.Add(YellowR);


                        RectMain.Fill = BinGradient;
                    }));

                    RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
                     {
                         RectMain.Fill = KNEKTColors.BinColor;
                     }));
                }
            }
        }

    }

}