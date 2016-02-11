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
    /// Interaction logic for CerealCooker1.xaml
    /// </summary>
    public partial class CerealCooker1 : UserControl
    {
        public CerealCooker1()
        {
            InitializeComponent();
            polyTop1.Fill = KNEKTColors.NoControlColor;
            borderMain.Background = KNEKTColors.NoControlColor;
        }
    }
}
