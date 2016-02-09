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

namespace KNEKT.Controls.LevelIndicators
{
    /// <summary>
    /// Interaction logic for BinLevel.xaml
    /// </summary>
    public partial class BinLevel : UserControl
    {
        public BinLevel()
        {
            InitializeComponent();
        }

        public Brush GetColor(int state)
        {
            if (state == 513)
            {
                return Brushes.Aqua;
            }
            if (state == 514)
            {
                return Brushes.LightGreen;
            }
            if (state == 515)
            {
                return Brushes.Green;
            }
            else
            {
                return Brushes.Red;
            }

        }


        public int SetBarColor
        {
            set
            {
                Progressbar_1.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        Progressbar_1.Foreground = GetColor(value);
                    }
            ));
            }
        }

        public int SetBarMin
        {
            set
            {
                Progressbar_1.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        Progressbar_1.Minimum = value;
                        //Progressbar_1.Foreground = GetColor(value);
                    }
            ));
            }
        }

        public int SetBarMax
        {
            set
            {
                Progressbar_1.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        Progressbar_1.Maximum = value;
                        //Progressbar_1.Foreground = GetColor(value);
                    }
            ));
            }
        }

        public int SetBarValue
        {
            set
            {
                Progressbar_1.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        Progressbar_1.Value = value;
                        //Progressbar_1.Foreground = GetColor(value);
                    }
            ));
            }
        }
    }
}
