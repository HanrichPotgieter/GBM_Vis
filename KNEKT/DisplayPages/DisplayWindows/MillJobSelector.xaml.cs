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
using System.Windows.Shapes;

namespace KNEKT.DisplayPages
{
    /// <summary>
    /// Interaction logic for MillJobSelector.xaml
    /// </summary>
    public partial class MillJobSelector : Window
    {
        public MillJobSelector(bool IsButton1Enabled, bool IsButton2Enabled, bool IsButton3Enabled, bool IsButton4Enabled, bool IsButton5Enabled)
        {
            InitializeComponent();
            btnMilRec34.IsEnabled = IsButton1Enabled;
            btnMilRec35.IsEnabled = IsButton2Enabled;
            btnMilRec36.IsEnabled = IsButton3Enabled;
            btnMilRec37.IsEnabled = IsButton4Enabled;
            btnMilRec38.IsEnabled = IsButton5Enabled;
        }

        private void btnMilRec34_Click(object sender, RoutedEventArgs e)
        {
            iWindowResult = 34;
            this.DialogResult = true;
        }

        private void btnMilRec35_Click(object sender, RoutedEventArgs e)
        {
            iWindowResult = 35;
            this.DialogResult = true;
        }


        private void btnMilRec36_Click(object sender, RoutedEventArgs e)
        {
            iWindowResult = 36;
            this.DialogResult = true;
        }

        private void btnMilRec37_Click(object sender, RoutedEventArgs e)
        {
            iWindowResult = 37;
            this.DialogResult = true;
        }

        private void btnMilRec38_Click(object sender, RoutedEventArgs e)
        {
            iWindowResult = 38;
            this.DialogResult = true;
        }

        public static int iWindowResult
        {
            get;
            set;
        }
       
    }
}
