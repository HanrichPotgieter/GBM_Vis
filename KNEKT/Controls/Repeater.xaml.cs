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
    /// Interaction logic for Repeater.xaml
    /// </summary>
    public partial class Repeater : UserControl
    {
        public Repeater()
        {
            InitializeComponent();

        }

        public string Repeater_Title
        {
            get
            {
                return lblTitle.Content.ToString();
            }
            set
            {
                lblTitle.Content = value;
            }
        }

        //public Brush GetColorAdditionalTags(int state)
        //{
        //    if (state == 1)
        //    {
        //        return Brushes.Red;
        //    }
        //    else
        //    {
        //        return Brushes.Green;
        //    }

        //}

        //public int SetColor1
        //{
        //    set
        //    {
        //        square_1.Dispatcher.BeginInvoke(
        //          System.Windows.Threading.DispatcherPriority.Normal,
        //          new Action(
        //            delegate()
        //            {
        //                square_1.Fill = GetColorAdditionalTags(value);
        //            }
        //    ));
        //    }
        //}

    }
}
