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
    /// Interaction logic for BaggingSystem.xaml
    /// </summary>
    public partial class BaggingSystem : UserControl
    {
        public BaggingSystem()
        {
            InitializeComponent();

            borderMain.Background = KNEKTColors.BinColor;
            rectMidCover1.Fill = KNEKTColors.BinColor;
            rectMidBottom.Fill = KNEKTColors.BinColor;
        }
    }
}
