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
    /// Interaction logic for Mixer_Tweedy2.xaml
    /// </summary>
    public partial class Mixer_Tweedy2 : UserControl
    {
        private int _SlideColor;
        private string DescriptionSlide;
        private string StatusSlide;
        private bool FaultSlide;
        private string _ObjectNo;
        private string _PLCName;

        private bool _IsControlled = false;

        public Mixer_Tweedy2()
        {
            InitializeComponent();
            Mixer_IsControlled = false;       
        }

        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//
         [Category("Buhler")]
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



        [Category("Buhler")]
        public int SlideColor
        {
            get
            {
                return _SlideColor;
            }
            set
            {
                _SlideColor = value;
                FB13 Slide = new FB13();
                _SetColor(Slide.SetColor(value));
                StatusSlide = Slide.Status_Slide;
                FaultSlide = Slide.Fault_Slide;

                if (Status_Slide.Contains("LN"))
                {
                    RectTop1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        RectTop1.Visibility = Visibility.Visible;
                    }));

                    RectTop2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        RectTop2.Visibility = Visibility.Hidden;
                    }));
                }
                else if (Status_Slide.Contains("HN"))
                {
                    RectTop1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        RectTop1.Visibility = Visibility.Hidden;
                    }));

                    RectTop2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        RectTop2.Visibility = Visibility.Visible;
                    }));
                }
                else
                {
                    RectTop1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        RectTop1.Visibility = Visibility.Visible;
                    }));

                    RectTop2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        RectTop2.Visibility = Visibility.Visible;
                    }));
                }
            }
        }

        [Category("Buhler")]
        public string ObjectNumber
        {
            get
            {
                return this._ObjectNo;
            }
            set
            {
                this._ObjectNo = value;
            }
        }

        [Category("Buhler")]
        public string Description_Slide
        {
            get
            {
                return this.DescriptionSlide;
            }
            set
            {
                this.DescriptionSlide = value;
            }
        }

        [Category("Buhler")]
        public string Status_Slide
        {
            get
            {
                return this.StatusSlide;
            }
        }

        [Category("Buhler")]
        public bool Fault_Slide
        {
            get
            {
                return this.FaultSlide;
            }
        }

        [Category("Buhler")]
        public string PLCName
        {
            get
            {
                return this._PLCName;
            }
            set
            {
                this._PLCName = value;
            }
        }

        //------------------------------------------------------------------------------//
        //                                   Methods                                    //
        //------------------------------------------------------------------------------//
        private void _SetDefaultColor(bool Value)
        {
            if (Value)
            {
                RectBot1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectBot1.Fill = KNEKTColors.Gray; }));
                RectBot2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectBot2.Fill = KNEKTColors.Gray; }));
                RectTop1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectTop1.Fill = KNEKTColors.Red; }));
                RectTop2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectTop2.Fill = KNEKTColors.Red; }));
                RectBotLeft1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectBotLeft1.Fill = KNEKTColors.Gray; }));
                RectBotRight1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectBotRight1.Fill = KNEKTColors.Gray; }));
                borderMid.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { borderMid.Background = KNEKTColors.Gray; }));
            }
            else
            {
                RectBot1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectBot1.Fill = KNEKTColors.Gray; }));
                RectBot2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectBot2.Fill = KNEKTColors.Gray; }));
                RectTop1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectTop1.Fill = KNEKTColors.Gray; }));
                RectTop2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectTop2.Fill = KNEKTColors.Gray; }));
                RectBotLeft1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectBotLeft1.Fill = KNEKTColors.Gray; }));
                RectBotRight1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { RectBotRight1.Fill = KNEKTColors.Gray; }));
                borderMid.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { borderMid.Background = KNEKTColors.Gray; }));
            }
        }


        private void _SetColor(Brush brushColor)
        {
            RectTop1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectTop1.Fill = brushColor;
            }));

            RectTop2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectTop2.Fill = brushColor;
            }));
        }
    }
}
