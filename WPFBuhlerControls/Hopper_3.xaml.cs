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
    /// Interaction logic for Hopper_3.xaml
    /// </summary>
    public partial class Hopper_3 : UserControl
    {
        private bool _HasKnockingHammer;
        
        public Hopper_3()
        {
            InitializeComponent();

            polyMain.Fill = KNEKTColors.BinColor;
            Hopper_HasKnockingHammer = true;
        }

        public bool Hopper_HasKnockingHammer
        {
            get { return _HasKnockingHammer; }
            set
            {
                _HasKnockingHammer = value;

                if (value)
                {
                    polyHammer1.Visibility = Visibility.Visible;
                    polyHammer2.Visibility = Visibility.Visible;
                }
                else
                {
                    polyHammer1.Visibility = Visibility.Hidden;
                    polyHammer2.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
