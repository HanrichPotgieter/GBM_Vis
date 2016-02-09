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
    /// Interaction logic for PlantSwitches.xaml
    /// </summary>
    public partial class PlantSwitches : UserControl
    {
        public PlantSwitches()
        {
            InitializeComponent();
        }
        //Digital Inputs
        public Brush GetColor(int state)
        {
            if (state == 3 || state == 515)
            {
                return Brushes.Green;
            }
            else
            {
                return Brushes.Red;
            }

        }
        //Analog Inputs
        public Brush GetAnalogColor(int state)
        {
            if (state == 1 || state == 513) //LowLow
            {
                return Brushes.Red;
            }
            else if (state == 2)            //Low
            {
                return Brushes.LightGreen;
            }
            else if (state == 3)            //StMiddle
            {
                return Brushes.Green;
            }
            else if (state == 4)            //High
            {
                return Brushes.Orange;
            }
            else if (state == 5)            //HighHigh
            {
                return Brushes.Red;
            }
            else
            {
                return Brushes.Red;
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

        public int SetColor2
        {
            set
            {
                ellipse_2.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        ellipse_2.Fill = GetColor(value);
                    }
            ));
            }
        }

        public int SetColor3
        {
            set
            {
                ellipse_3.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        ellipse_3.Fill = GetColor(value);
                    }
            ));
            }
        }

        public int SetColor4
        {
            set
            {
                ellipse_4.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        ellipse_4.Fill = GetColor(value);
                    }
            ));
            }
        }

        public int SetColor5
        {
            set
            {
                ellipse_5.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    ellipse_5.Fill = GetAnalogColor(value);
                }));
                progressBar1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    progressBar1.Foreground = GetAnalogColor(value);
                }));
            }
        }

        public int SetColor6
        {
            set
            {
                ellipse_6.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    ellipse_6.Fill = GetAnalogColor(value);
                }));
                progressBar2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    progressBar2.Foreground = GetAnalogColor(value);
                }));
            }
        }


        //public int SetColor7
        //{
        //    set
        //    {
        //        ellipse_7.Dispatcher.BeginInvoke(
        //          System.Windows.Threading.DispatcherPriority.Normal,
        //          new Action(
        //            delegate()
        //            {
        //                ellipse_7.Fill = GetColor(value);
        //            }
        //    ));
        //    }
        //}

        //public int SetColor8
        //{
        //    set
        //    {
        //        ellipse_8.Dispatcher.BeginInvoke(
        //          System.Windows.Threading.DispatcherPriority.Normal,
        //          new Action(
        //            delegate()
        //            {
        //                ellipse_8.Fill = GetColor(value);
        //            }
        //    ));
        //    }
        //}

        //public int SetColor9
        //{
        //    set
        //    {
        //        ellipse_9.Dispatcher.BeginInvoke(
        //          System.Windows.Threading.DispatcherPriority.Normal,
        //          new Action(
        //            delegate()
        //            {
        //                ellipse_9.Fill = GetColor(value);
        //            }
        //    ));
        //    }
        //}

        //public int SetColor10
        //{
        //    set
        //    {
        //        ellipse_10.Dispatcher.BeginInvoke(
        //          System.Windows.Threading.DispatcherPriority.Normal,
        //          new Action(
        //            delegate()
        //            {
        //                ellipse_10.Fill = GetColor(value);
        //            }
        //    ));
        //    }
        //}

        //public int SetColor11
        //{
        //    set
        //    {
        //        ellipse_11.Dispatcher.BeginInvoke(
        //          System.Windows.Threading.DispatcherPriority.Normal,
        //          new Action(
        //            delegate()
        //            {
        //                ellipse_11.Fill = GetColor(value);
        //            }
        //    ));
        //    }
        //}

        ////public int SetColor12
        //{
        //    set
        //    {
        //        ellipse_12.Dispatcher.BeginInvoke(
        //          System.Windows.Threading.DispatcherPriority.Normal,
        //          new Action(
        //            delegate()
        //            {
        //                ellipse_12.Fill = GetColor(value);
        //            }
        //    ));
        //    }
        //}

        //public int SetColor13
        //{
        //    set
        //    {
        //        ellipse_13.Dispatcher.BeginInvoke(
        //          System.Windows.Threading.DispatcherPriority.Normal,
        //          new Action(
        //            delegate()
        //            {
        //                ellipse_13.Fill = GetColor(value);
        //            }
        //    ));
        //    }
        //}

    }
}
