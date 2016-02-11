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
    /// Interaction logic for Magnet_MMUD.xaml
    /// </summary>
    public partial class Magnet_MMUD : UserControl
    {
        public Magnet_MMUD()
        {
            InitializeComponent();
            polyTop.Fill = KNEKTColors.NoControlColor;
            Rectmain.Fill = KNEKTColors.NoControlColor;
            polyBot.Fill = KNEKTColors.NoControlColor;
        }
    }
}
