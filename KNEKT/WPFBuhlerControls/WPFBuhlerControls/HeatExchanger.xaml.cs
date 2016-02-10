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
    /// Interaction logic for HeatExchanger.xaml
    /// </summary>
    public partial class HeatExchanger : UserControl
    {
        public HeatExchanger()
        {
            InitializeComponent();
            polyTop.Fill = KNEKTColors.Gray;
            rectBottom.Fill = KNEKTColors.Gray;
            rectMiddle.Fill = KNEKTColors.Gray;
        }
    }
}
