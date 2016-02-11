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
    /// Interaction logic for Line_Arrow.xaml
    /// </summary>
    public partial class Line_Arrow : UserControl
    {
        private int Direction;

        public Line_Arrow()
        {
            InitializeComponent();

            polyTopLeft.Stroke = KNEKTColors.LineColor;
            polyTopRight.Stroke = KNEKTColors.LineColor;
            polyBotLeft.Stroke = KNEKTColors.LineColor;
            polyBotRight.Stroke = KNEKTColors.LineColor;

            polyTopLeft.Fill = KNEKTColors.LineColor;
            polyTopRight.Fill = KNEKTColors.LineColor;
            polyBotLeft.Fill = KNEKTColors.LineColor;
            polyBotRight.Fill = KNEKTColors.LineColor;
        }

        /// <summary>
        /// Set the direction of the Arrow. 1 = Up, 2 = Right, 3 = Down, 4 = Left
        /// </summary>
        
        [Category("Buhler")]
        public int Arrow_SetDirection
        {
            get
            {
                return this.Direction;
            }

            set
            {
                switch (value)
                {
                    case 1:                                             //UP                        
                        Direction = 1;

                        polyTopLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyTopLeft.Visibility = Visibility.Visible;
                        }));

                        polyTopRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyTopRight.Visibility = Visibility.Visible;
                        }));

                        polyBotLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyBotLeft.Visibility = Visibility.Hidden;
                        }));

                        polyBotRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyBotRight.Visibility = Visibility.Hidden;
                        }));
                        break;


                    case 2:                                             //RIGHT                       
                        Direction = 2;

                        polyTopLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyTopLeft.Visibility = Visibility.Hidden;
                        }));

                        polyTopRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyTopRight.Visibility = Visibility.Visible;
                        }));

                        polyBotLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyBotLeft.Visibility = Visibility.Hidden;
                        }));

                        polyBotRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyBotRight.Visibility = Visibility.Visible;
                        }));                       
                        break;


                    case 3:                                             //DOWN

                        Direction = 3;

                        polyTopLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyTopLeft.Visibility = Visibility.Hidden;
                        }));

                        polyTopRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyTopRight.Visibility = Visibility.Hidden;
                        }));

                        polyBotLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyBotLeft.Visibility = Visibility.Visible;
                        }));

                        polyBotRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            polyBotRight.Visibility = Visibility.Visible;
                        }));                       
                        break;


                    case 4:                                             //LEFT

                        Direction = 4;

                        polyTopLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyTopLeft.Visibility = Visibility.Visible;
                        }));

                        polyTopRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyTopRight.Visibility = Visibility.Hidden;
                        }));

                        polyBotLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyBotLeft.Visibility = Visibility.Visible;
                        }));

                        polyBotRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                        {
                            polyBotRight.Visibility = Visibility.Hidden;
                        }));                        
                        break;              
                }
            }
        }

    }
}
