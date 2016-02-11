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
    /// Interaction logic for Hopper_ChainConveyor.xaml
    /// </summary>
    public partial class Hopper_ChainConveyor : UserControl
    {
        private string DescriptionHopperChainConveyor;

        public Hopper_ChainConveyor()
        {
            InitializeComponent();
            RectMain.Fill = KNEKTColors.NoControlColor;
        }

        public string DescriptionHopper_ChainConveyor
        {
            get
            {
                return this.DescriptionHopperChainConveyor;
            }
            set
            {
                this.DescriptionHopperChainConveyor = value;
            }
        }
    }
}
