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
    /// Interaction logic for Line_Horizontal.xaml
    /// </summary>
    public partial class Line_Horizontal : UserControl
    {

        private int DashStyle = 4;
        private double LineThickness = 1.2;

        public Line_Horizontal()
        {
            InitializeComponent();

            polyLineH.Points.Add(new Point(0, 5));
            polyLineH.Points.Add(new Point(300, 5));
            polyLineH.StrokeThickness = 1.2;
            polyLineH.Stroke = KNEKTColors.LineColor;
        }

        [Category("Buhler")]
        public int Line_DashStyle
        {
            get
            {
                return DashStyle;
            }
            set
            {
                DoubleCollection dashes;

                switch (value)
                {
                    case 1:
                        DashStyle = 1;
                        dashes = new DoubleCollection();
                        dashes.Add(2);
                        dashes.Add(2);
                        polyLineH.StrokeDashArray = dashes;
                        break;

                    case 2:
                        DashStyle = 2;
                        dashes = new DoubleCollection();
                        dashes.Add(3);
                        dashes.Add(3);
                        polyLineH.StrokeDashArray = dashes;
                        break;

                    case 3:
                        DashStyle = 3;
                        dashes = new DoubleCollection();
                        dashes.Add(1);
                        polyLineH.StrokeDashArray = dashes;
                        break;

                    case 4:
                        DashStyle = 4;
                        dashes = new DoubleCollection();
                        polyLineH.StrokeDashArray = dashes;
                        break;
                }
            }
        }

        [Category("Buhler")]
        public double Line_Thickness
        {
            get
            {
                return LineThickness;
            }
            set
            {
                if (value <= 5)
                {
                    LineThickness = value;
                    polyLineH.StrokeThickness = LineThickness;
                }
            }
        }
        

    }
}
