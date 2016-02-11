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
    /// Interaction logic for Magnet_MMUA.xaml
    /// </summary>
    public partial class Magnet_MMUA_H : UserControl
    {
        private string DescriptionMagnet;

        public Magnet_MMUA_H()
        {
            InitializeComponent();
            RectMain.Fill = KNEKTColors.NoControlColor;
        }

        public string Description_Magnet
        {
            get
            {
                return this.DescriptionMagnet;
            }
            set
            {
                this.DescriptionMagnet = value;
            }
        }
    }
}
