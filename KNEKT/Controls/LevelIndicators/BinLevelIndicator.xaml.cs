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

namespace KNEKT.Controls.LevelIndicators
{
    /// <summary>
    /// Interaction logic for BinLevelIndicator.xaml
    /// </summary>
    public partial class BinLevelIndicator : UserControl
    {
        private double dValue;
        private Color FillColor;
        private int LevelMax;
        //    pageINT1.binLevelIndicator4Actual.Value = Convert.ToDouble(t.Value)/1000;

        public BinLevelIndicator()
        {
            InitializeComponent();
            BinLevelIndicator_FillColor = Colors.Brown;
            BinLevelIndicator_MaximumLevelValue = 100;
        }

        private void UpdateBinLevel()
        {
            double dMax = UserControl.Height;
            double i = dMax / BinLevelIndicator_MaximumLevelValue;
            double dActualValue = Math.Round(dValue * i, 1);
            recProgress.Height = dActualValue;
        }


        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
        //------------------------------------------------------------------------------//

        public double Value
        {
            get
            {
                return dValue;
            }
            set
            {
                if (value >= 0)
                {
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        dValue = value;
                        UpdateBinLevel();
                    }));
                }
                else
                {
                    dValue = 0;
                }
            }
        }

        public int BinLevelIndicator_MaximumLevelValue
        {
            get
            {
                return LevelMax;
            }
            set
            {
                LevelMax = value;
            }
        }

        public Color BinLevelIndicator_FillColor
        {
            get
            {
                return FillColor;
            }
            set
            {
                FillColor = value;
                LinearGradientBrush lgb = new LinearGradientBrush();
                lgb.StartPoint = new Point(0, 0);
                lgb.EndPoint = new Point(1, 0);
                lgb.Opacity = 0.6;
                GradientStop gs1 = new GradientStop(Colors.Transparent, 0.0);
                GradientStop gs2 = new GradientStop(FillColor, 0.1);
                GradientStop gs3 = new GradientStop(FillColor, 0.5);
                GradientStop gs4 = new GradientStop(FillColor, 0.9);
                GradientStop gs5 = new GradientStop(Colors.Transparent, 1.0);

                lgb.GradientStops.Add(gs1);
                lgb.GradientStops.Add(gs2);
                lgb.GradientStops.Add(gs3);
                lgb.GradientStops.Add(gs4);
                lgb.GradientStops.Add(gs5);

                recProgress.Background = lgb;
            }
        }

    }
}
