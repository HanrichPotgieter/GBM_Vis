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
using WPFBuhlerControls.FB_Code;
using System.ComponentModel;

namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for Airlock_MPSN.xaml
    /// </summary>
    public partial class Cyclone : UserControl
    {
        private bool CycloneIsLeft;

        public Cyclone()
        {
            InitializeComponent();
            PolyMainLeft.Fill = KNEKTColors.NoControlColor;
            PolyMainRight.Fill = KNEKTColors.NoControlColor;
            RectTop.Fill = KNEKTColors.NoControlColor;

            PolyMainRight.Visibility = Visibility.Hidden;
            CycloneIsLeft = true;
        }


        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
        //------------------------------------------------------------------------------//



        [Category("Buhler")]
        public bool Cyclone_IsSourceFromLeft
        {
            get
            {
                return CycloneIsLeft;
            }
            set
            {
                CycloneIsLeft = value;
                AirlockPosition();
            }
        }

        //------------------------------------------------------------------------------//
        //                                   Methods                                    //
        //------------------------------------------------------------------------------//

        //Changes the direction of the cyclones inlet
        private void AirlockPosition()
        {
            if (Cyclone_IsSourceFromLeft)
            {
                PolyMainLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    PolyMainLeft.Visibility = Visibility.Visible;
                }));
                PolyMainRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    PolyMainRight.Visibility = Visibility.Hidden;
                }));
            }
            else
            {
                PolyMainLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    PolyMainLeft.Visibility = Visibility.Hidden;
                }));
                PolyMainRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    PolyMainRight.Visibility = Visibility.Visible;
                }));
            }
        }
    }
}
