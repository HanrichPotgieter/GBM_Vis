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
    /// Interaction logic for Splitter1.xaml
    /// </summary>
    public partial class Splitter1 : UserControl
    {
        public Splitter1()
        {
            InitializeComponent();

            rectTop.Fill = KNEKTColors.NoControlColor;
            rectBottomLeft.Fill = KNEKTColors.NoControlColor;
            rectBottomRight.Fill = KNEKTColors.NoControlColor;

        }
    }
}
