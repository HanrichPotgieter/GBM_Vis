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
    /// Interaction logic for DivideBox.xaml
    /// </summary>
    public partial class DivideBox : UserControl
    {
        public DivideBox()
        {
            InitializeComponent();

            RectTop.Fill = KNEKTColors.NoControlColor;
            RectLeft.Fill = KNEKTColors.NoControlColor;
            RectRight.Fill = KNEKTColors.NoControlColor;
        }
    }
}

