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
    /// Interaction logic for AirDryer1.xaml
    /// </summary>
    public partial class AirDryer1 : UserControl
    {
        private string DescriptionAirDryer1;

        public AirDryer1()
        {
            InitializeComponent();
            rectMain.Fill = KNEKTColors.NoControlColor;
            
        }

        public string Description_AirDryer1
        {
            get
            {
                return this.DescriptionAirDryer1;
            }
            set
            {
                this.DescriptionAirDryer1 = value;
            }
        }
    }
}
