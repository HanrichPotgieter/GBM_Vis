﻿using System;
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
    /// Interaction logic for FlourOutloadControlDesk.xaml
    /// </summary>
    public partial class FlourOutloadControlDesk : UserControl
    {
        public FlourOutloadControlDesk()
        {
            InitializeComponent();
        }
        public Brush GetColor(int state)
        {
            if (state == 1)
            {
                return Brushes.Green;
            }
            else
            {
                return Brushes.Red;
            }

        }

        public Brush GetColorStopIdle(int state)
        {
            if (state == 1)
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

        public int SetColor2
        {
            set
            {
                ellipse_2.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        ellipse_2.Fill = GetColorStopIdle(value);
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
                        ellipse_3.Fill = GetColorStopIdle(value);
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
                ellipse_5.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        ellipse_5.Fill = GetColorStopIdle(value);
                    }
            ));
            }
        }

        public int SetColor6
        {
            set
            {
                ellipse_6.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        ellipse_6.Fill = GetColorStopIdle(value);
                    }
            ));
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

        //public int SetColor12
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
