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

namespace KNEKT.Controls
{
    /// <summary>
    /// Interaction logic for CustomStoppingTimer.xaml
    /// </summary>
    public partial class CountdownTimer : UserControl
    {
        //private string customTimerName;
        public CountdownTimer()
        {
            InitializeComponent();
            progressBarStopping.Maximum = 10;
            lblStopping.Content = CustomTimerName;
        }

        private void progressBarStopping_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //MessageBox.Show("" + e.OldValue + "   " + e.NewValue);
            lblProgress.Content = "" + e.NewValue + " Sec.";
        }

        public string CustomTimerName
        {
            get
            {
                return lblStopping.Content.ToString();
            }
            set
            {
                lblStopping.Content  = value;
            }
        }
    }
}
