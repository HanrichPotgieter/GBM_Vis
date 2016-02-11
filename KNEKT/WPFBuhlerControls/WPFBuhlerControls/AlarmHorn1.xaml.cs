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
    /// Interaction logic for AlarmHorn1.xaml
    /// </summary>
    public partial class AlarmHorn1 : UserControl
    {
        private int _AlarmHornColor;

        public AlarmHorn1()
        {
            InitializeComponent();
        }

        [Category("Buhler")]
        public int AlarmHornColor
        {
            get
            {
                return _AlarmHornColor;
            }
            set
            {
                _AlarmHornColor = value;
                if (value == 1) //On
                {
                    _SetColorAlarm(KNEKTColors.Orange);
                    _SetColorLines(KNEKTColors.Orange);
                }
                else //Off
                {
                    _SetColorAlarm(Brushes.DarkGray);
                    _SetColorLines(Brushes.Transparent);
                }
            }
        }

        [Category("Buhler")]
        public Visibility AlarmHorn_Visibility
        {
            get
            {
                return canvasMain.Visibility;
            }
            set
            {
                canvasMain.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        canvasMain.Visibility = value;
                    }
                ));   
               
            }
        }

        //------------------------------------------------------------------------------//
        //                                   Methods                                    //
        //------------------------------------------------------------------------------//
        private void _SetColorAlarm(Brush brushColor)
        {
            polyAlarmHorn.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyAlarmHorn.Fill = brushColor;
            }));            
        }
        

        private void _SetColorLines(Brush brushColor)
        {            
            polyLine1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyLine1.Stroke = brushColor;
            }));

            polyLine2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyLine2.Stroke = brushColor;
            }));

            polyLine3.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyLine3.Stroke = brushColor;
            }));
        }
    }
}
