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
    /// Interaction logic for Cascade.xaml
    /// </summary>
    public partial class Cascade : UserControl
    {
        private string DescriptionCascade;

        public Cascade()
        {
            InitializeComponent();

            RectTop.Fill = KNEKTColors.NoControlColor;
        }

        public string Description_Cascade
        {
            get
            {
                return this.DescriptionCascade;
            }
            set
            {
                this.DescriptionCascade = value;
            }
        }
    }
}
