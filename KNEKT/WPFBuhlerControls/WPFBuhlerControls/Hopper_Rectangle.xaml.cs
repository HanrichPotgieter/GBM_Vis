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
using WPFBuhlerControls.FB_Code;

namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for Hopper_Rectangle.xaml
    /// </summary>
    public partial class Hopper_Rectangle : UserControl
    {
        public Hopper_Rectangle()
        {
            InitializeComponent();

            RectMain.Fill = KNEKTColors.BinColor;
        }
    }
}
