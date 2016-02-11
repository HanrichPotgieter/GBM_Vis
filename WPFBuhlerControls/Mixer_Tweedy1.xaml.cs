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
    /// Interaction logic for Mixer_Tweedy1.xaml
    /// </summary>
    public partial class Mixer_Tweedy1 : UserControl
    {
        private bool _IsControlled = false;

        public Mixer_Tweedy1()
        {
            InitializeComponent();
            Mixer_IsControlled = false;       
        }



        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//
        public bool Mixer_IsControlled
        {
            get
            {
                return _IsControlled;
            }
            set
            {
                _IsControlled = value;
                _SetDefaultColor(value);
            }
        }

        //------------------------------------------------------------------------------//
        //                                   Methods                                    //
        //------------------------------------------------------------------------------//
        private void _SetDefaultColor(bool Value)
        {
            if (Value)
            {
                RectBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectBot.Fill = Brushes.Transparent; }));
                RectBracketL.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectBracketL.Fill = Brushes.Transparent; }));
                RectBracketR.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectBracketR.Fill = Brushes.Transparent; }));                
                RectLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectLeft.Fill = Brushes.Transparent; }));               
                RectRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectRight.Fill = Brushes.Transparent; }));
                borderMid.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { borderMid.Background = Brushes.Transparent; }));
            }
            else
            {
                RectBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectBot.Fill = KNEKTColors.Gray; }));
                RectBracketL.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectBracketL.Fill = KNEKTColors.Gray; }));
                RectBracketR.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectBracketR.Fill = KNEKTColors.Gray; }));                
                RectLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectLeft.Fill = KNEKTColors.Gray; }));                
                RectRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectRight.Fill = KNEKTColors.Gray; }));
                borderMid.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { borderMid.Background = KNEKTColors.Gray; }));
            }
        }
              
    }
}
