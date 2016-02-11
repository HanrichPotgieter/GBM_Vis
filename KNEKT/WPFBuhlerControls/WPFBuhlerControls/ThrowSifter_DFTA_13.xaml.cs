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
    /// Interaction logic for ThrowSifter_DFTA.xaml
    /// </summary>
    public partial class ThrowSifter_DFTA_13 : UserControl
    {
        private int _MotorColor;
        private string DescriptionThrowSifter;
        private string StatusThrowSifter;
        private bool FaultThrowSifter;
        private string _ObjectNo;
        private string _PLCName;

        public ThrowSifter_DFTA_13()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int MotorColor
        {
            get
            {
                return _MotorColor;
            }
            set
            {
                _MotorColor = value;
                FB12 motor = new FB12();
                _SetColor(motor.SetColor(value));
                StatusThrowSifter = motor.Status_Motor;
                FaultThrowSifter = motor.Fault_Motor;
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
        public string Description_ThrowSifter
        {
            get
            {
                return this.DescriptionThrowSifter;
            }
            set
            {
                this.DescriptionThrowSifter = value;
            }
        }

        [Category("Buhler")]
        public string Status_ThrowSifter
        {
            get
            {
                return this.StatusThrowSifter;
            }
        }

        [Category("Buhler")]
        public bool Fault_ThrowSifter
        {
            get
            {
                return this.FaultThrowSifter;
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
            RectMain1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectMain1.Fill = brushColor;
            }));

            RectMain2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectMain2.Fill = brushColor;
            }));

            RectMain3.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectMain3.Fill = brushColor;
            }));

            RectMain4.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectMain4.Fill = brushColor;
            }));

            RectMain5.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectMain5.Fill = brushColor;
            }));

            RectMain6.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectMain6.Fill = brushColor;
            }));

            PolyMain1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                PolyMain1.Fill = brushColor;
            }));

            PolyMain2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                PolyMain2.Fill = brushColor;
            }));
        }
    }
}
