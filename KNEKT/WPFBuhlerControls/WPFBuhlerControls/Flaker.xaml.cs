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
    /// Interaction logic for Flaker.xaml
    /// </summary>
    public partial class Flaker : UserControl
    {
        private int _ColorFlaker;
        private string DescriptionFlaker;
        private string StatusFlaker;
        private bool FaultFlaker;
        private string _ObjectNo1;
        private string _ObjectNo2;
        private string _ObjectNo3;
        private string _PLCName;

        public Flaker()
        {
            InitializeComponent();
        }


        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int ColorFlaker
        {
            get
            {
                return _ColorFlaker;
            }
            set
            {
                _ColorFlaker = value;
                FB58 flaker = new FB58();
                _SetFlakerColor(flaker.SetColor(value));
                StatusFlaker = flaker.Status_Flaker;
                FaultFlaker = flaker.Fault_Flaker;
            }
        }

        [Category("Buhler")]
        public int ColorFlakerMotor1
        {
            set
            {
                FB14 flaker = new FB14();
                _SetMotorColor(flaker.SetColor(value,0),1);
            }
        }

        [Category("Buhler")]
        public int ColorFlakerMotor2
        {
            set
            {
                FB14 flaker = new FB14();
                _SetMotorColor(flaker.SetColor(value,0),2);
            }
        }

        [Category("Buhler")]
        public string Description_Flaker
        {
            get
            {
                return this.DescriptionFlaker;
            }
            set
            {
                this.DescriptionFlaker = value;
            }
        }

        [Category("Buhler")]
        public string Status_Flaker
        {
            get
            {
                return this.StatusFlaker;
            }
        }

        [Category("Buhler")]
        public bool Fault_Flaker
        {
            get
            {
                return this.FaultFlaker;
            }
        }

        [Category("Buhler")]
        public string ObjectNumber1
        {
            get
            {
                return this._ObjectNo1;
            }
            set
            {
                this._ObjectNo1 = value;
            }
        }

        [Category("Buhler")]
        public string ObjectNumber2
        {
            get
            {
                return this._ObjectNo2;
            }
            set
            {
                this._ObjectNo2 = value;
            }
        }

        [Category("Buhler")]
        public string ObjectNumber3
        {
            get
            {
                return this._ObjectNo3;
            }
            set
            {
                this._ObjectNo3 = value;
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
        //                                  Methods                                     //
        //------------------------------------------------------------------------------//
        private void _SetFlakerColor(Brush brushColor)
        {
            polyBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyBot.Fill = brushColor;
            }));

            polyMid.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyMid.Fill = brushColor;
            }));

            polyTop1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyTop1.Fill = brushColor;
            }));

            polyTop2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyTop2.Fill = brushColor;
            }));
        }

        private void _SetMotorColor(Brush brushColor, int MotorNumber)
        {
            if (MotorNumber == 1)
            {
                elipseMain1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    elipseMain1.Fill = brushColor;
                }));

                elipseMain2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    elipseMain2.Fill = brushColor;
                }));
            }
            else if (MotorNumber == 2)
            {
                elipseFeed.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    elipseFeed.Fill = brushColor;
                }));
            }
        }
    }
}
