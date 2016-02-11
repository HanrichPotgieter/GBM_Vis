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

namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for Line_Inlet_Pneumatic.xaml
    /// </summary>
    public partial class Line_Inlet_Pneumatic : UserControl
    {
        public Line_Inlet_Pneumatic()
        {
            InitializeComponent();

            polyMain.Stroke = KNEKTColors.LineColor;
            polyMain.Fill = KNEKTColors.LineColor;
        }
    }
}
