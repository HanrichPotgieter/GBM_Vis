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
    /// Interaction logic for HandAdditionsConrolDesk.xaml
    /// </summary>
    public partial class HandAdditionsConrolDesk : UserControl
    {
        public HandAdditionsConrolDesk() 
        {
            InitializeComponent();
        }

        public Brush GetColor(int state)
        {
            if (state == 3 || state == 515)
            {
                return Brushes.Green;
            }
            else
            {
                return Brushes.Gray;
            }

        }


        public int SetColor1
        {
            set
            {
                ellipse_1.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        ellipse_1.Fill = GetColor(value);
                    }
            ));
            }
        }

        //public int SetColor2
        //{
        //    set
        //    {
        //        ellipse_2.Dispatcher.BeginInvoke(
        //          System.Windows.Threading.DispatcherPriority.Normal,
        //          new Action(
        //            delegate()
        //            {
        //                ellipse_2.Fill = GetColor(value);
        //            }
        //    ));
        //    }
        //}

        //public int SetColor3
        //{
        //    set
        //    {
        //        ellipse_3.Dispatcher.BeginInvoke(
        //          System.Windows.Threading.DispatcherPriority.Normal,
        //          new Action(
        //            delegate()
        //            {
        //                ellipse_3.Fill = GetColor(value);
        //            }
        //    ));
        //    }
        //}

        
    }
}
