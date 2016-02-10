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
using System.ComponentModel;

namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for Arrow_Direction.xaml
    /// </summary>
    public partial class Arrow_Direction : UserControl
    {

        private bool bArrowRight;

        public Arrow_Direction()
        {
            InitializeComponent();
        }

        [Category("Buhler")]
        public bool Arrow_Direction_Right
        {
            get
            {
                return this.bArrowRight;
            }
            set
            {
                this.bArrowRight = value;

                if (value)
                {
                    polyS3.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        polyS3.Visibility = Visibility.Visible;
                    }));

                    polyS2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        polyS2.Visibility = Visibility.Hidden;
                    }));
                }
                else
                {
                    polyS3.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        polyS3.Visibility = Visibility.Hidden;
                    }));

                    polyS2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        polyS2.Visibility = Visibility.Visible;
                    }));
                }
            }
        }
    }
}
