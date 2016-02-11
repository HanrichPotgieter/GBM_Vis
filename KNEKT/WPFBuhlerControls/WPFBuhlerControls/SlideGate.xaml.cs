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
    /// Interaction logic for SlideGate.xaml
    /// </summary>
    public partial class SlideGate : UserControl
    {
        private int _SlideColor;
        private string DescriptionSlide;
        private string StatusSlide;
        private bool FaultSlide;
        private string _ObjectNo;
        private string _PLCName;
        
        public SlideGate()
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
        private void _SetColor(Brush brushColor)
        {     
            polyMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyMain.Fill = brushColor;      
            }));
        }
    }
}
