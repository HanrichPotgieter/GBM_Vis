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
    /// Interaction logic for Line_Bend.xaml
    /// </summary>
    public partial class Line_Bend : UserControl
    {
        private int Direction;
        private int DashStyle = 4;
        private double LineThickness = 1.2;

        public Line_Bend()
        {
            InitializeComponent();

            polyMain.Stroke = KNEKTColors.LineColor;
            polyMain.StrokeThickness = 1.2;
        }

        /// <summary>
        /// Set the direction of the Bend. 1 = TopLeft, 2 = TopRight, 3 = BottomLeft, 4 = BottomRight
        /// </summary>
        [Category("Buhler")]
        public int Bend_Direction
        {
            get
            {
                return this.Direction;
            }

            set
            {
                switch (value)
                {
                    case 1:
                        Direction = 1;
                        polyMain.Points.Clear();
                        polyMain.Points.Add(new Point(3, 20));
                        polyMain.Points.Add(new Point(3, 8));
                        polyMain.Points.Add(new Point(4.5, 4.5));
                        polyMain.Points.Add(new Point(8, 3));
                        polyMain.Points.Add(new Point(20, 3));

                        break;
                    case 2:
                        Direction = 2;
                        polyMain.Points.Clear();
                        polyMain.Points.Add(new Point(0, 3));
                        polyMain.Points.Add(new Point(12, 3));
                        polyMain.Points.Add(new Point(15.5, 4.5));
                        polyMain.Points.Add(new Point(17, 8));
                        polyMain.Points.Add(new Point(17, 20));

                        break;
                    case 3:
                        Direction = 3;
                        polyMain.Points.Clear();
                        polyMain.Points.Add(new Point(3, 0));
                        polyMain.Points.Add(new Point(3, 12));
                        polyMain.Points.Add(new Point(4.5, 15.5));
                        polyMain.Points.Add(new Point(8, 17));
                        polyMain.Points.Add(new Point(20, 17));

                        break;
                    case 4:
                        Direction = 4;
                        polyMain.Points.Clear();
                        polyMain.Points.Add(new Point(17, 0));
                        polyMain.Points.Add(new Point(17, 12));
                        polyMain.Points.Add(new Point(15.5, 15.5));
                        polyMain.Points.Add(new Point(12, 17));
                        polyMain.Points.Add(new Point(0, 17));

                        break;
                }
            }
        }

        [Category("Buhler")]
        public int Bend_DashStyle
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
                        polyMain.StrokeDashArray = dashes;
                        break;

                    case 2:
                        DashStyle = 2;
                        dashes = new DoubleCollection();
                        dashes.Add(3);
                        dashes.Add(3);
                        polyMain.StrokeDashArray = dashes;
                        break;

                    case 3:
                        DashStyle = 3;
                        dashes = new DoubleCollection();
                        dashes.Add(1);
                        polyMain.StrokeDashArray = dashes;
                        break;

                    case 4:
                        DashStyle = 4;
                        dashes = new DoubleCollection();
                        polyMain.StrokeDashArray = dashes;
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
                    polyMain.StrokeThickness = LineThickness;
                }
            }
        }

    }
}
