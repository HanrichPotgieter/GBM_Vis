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
using WPFBuhlerControls.FB_Code;

namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for MixerDoor1.xaml
    /// </summary>
    public partial class MixerDoor1 : UserControl
    {
        private int _SlideColor;
        private string DescriptionSlide;
        private string StatusSlide;
        private bool FaultSlide;
        private string _ObjectNo;
        private string _PLCName;

        public MixerDoor1()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

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

                if (StatusSlide.ToString().Contains("StHN"))
                    Slide_IsOpen = true;
                else if (StatusSlide.ToString().Contains("StLN"))
                    Slide_IsOpen = false;
                

                SetVisibility();
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

        public bool Slide_IsOpen
        {
            get;
            set;
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
        private void _SetColor(Brush brushColor)
        {
            polyClosed.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyClosed.Stroke = brushColor;
            }));

            polyOpenLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyOpenLeft.Stroke = brushColor;
            }));

            polyOpenDownLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyOpenDownLeft.Stroke = brushColor;
            }));

            polyOpenRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyOpenRight.Stroke = brushColor;
            }));

            polyOpenDownRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyOpenDownRight.Stroke = brushColor;
            }));          
        }

        private void SetVisibility()
        {
            Visibility v1 = System.Windows.Visibility.Hidden;
            Visibility v2 = System.Windows.Visibility.Visible;
            if (Slide_IsOpen)
            {
                v1 = System.Windows.Visibility.Hidden;
                v2 = System.Windows.Visibility.Visible;
            }
            else
            {
                v1 = System.Windows.Visibility.Visible;
                v2 = System.Windows.Visibility.Hidden;
            }

            polyClosed.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyClosed.Visibility = v1;
            }));

            polyOpenLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyOpenLeft.Visibility = v2;
            }));

            polyOpenDownLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyOpenDownLeft.Visibility = v2;
            }));

            polyOpenRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyOpenRight.Visibility = v2;
            }));

            polyOpenDownRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyOpenDownRight.Visibility = v2;
            }));
        }
    }
}
