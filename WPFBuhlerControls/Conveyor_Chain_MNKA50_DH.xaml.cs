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
    /// Interaction logic for Conveyor_Chain_MNKA50_DH.xaml
    /// </summary>
    public partial class Conveyor_Chain_MNKA50_DH : UserControl
    {
        private int _MotorColor;
        private string DescriptionConveyor;
        private string StatusConveyor;
        private bool FaultConveyor;
        private bool _MotorOnLeft = false;
        private string _ObjectNo;
        private string _PLCName;

        public Conveyor_Chain_MNKA50_DH()
        {
            InitializeComponent();
        }

         //------------------------------------------------------------------------------//
        //                                 Properties                                   //
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
                FB12 Motor = new FB12();
                _SetColor(Motor.SetColor(value));
                StatusConveyor = Motor.Status_Motor;
                FaultConveyor = Motor.Fault_Motor;
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
        public string Description_Conveyor
        {
            get
            {
                return this.DescriptionConveyor;
            }
            set
            {
                this.DescriptionConveyor = value;
            }
        }

        [Category("Buhler")]
        public string Status_Conveyor
        {
            get
            {
                return this.StatusConveyor;
            }
        }

        [Category("Buhler")]
        public bool Fault_Conveyor
        {
            get
            {
                return this.FaultConveyor;
            }
        }

        //[Category("Buhler")]
        //public bool Conveyor_MotorOnLeft
        //{
        //    get
        //    {
        //        return _MotorOnLeft;
        //    }
        //    set
        //    {
        //        _MotorOnLeft = value;
        //        //SetMotorSide();
        //    }
        //}

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
        //                                 Methods                                      //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            PolyConveyor.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyConveyor.Fill = brushColor;
            }));

            RectLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectLeft.Fill = brushColor;
            }));

            RectRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectRight.Fill = brushColor;
            }));
        }


        //private void SetMotorSide()
        //{
        //    if (Conveyor_MotorOnLeft)
        //    {
        //        RectLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
        //        {
        //            RectLeft.Height = 25;
        //            Thickness margin = RectLeft.Margin;
        //            margin.Top = 0;
        //            RectLeft.Margin = margin;
        //        }));

        //        RectRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
        //        {
        //            RectRight.Height = 20;
        //            Thickness margin = RectRight.Margin;
        //            margin.Top = 5;
        //            RectRight.Margin = margin;
        //        }));
        //    }
        //    else
        //    {
        //        RectLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
        //        {
        //            RectLeft.Height = 20;
        //            Thickness margin = RectLeft.Margin;
        //            margin.Top = 5;
        //            RectLeft.Margin = margin;
        //        }));

        //        RectRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
        //        {
        //            RectRight.Height = 25;
        //            Thickness margin = RectRight.Margin;
        //            margin.Top = 0;
        //            RectRight.Margin = margin;
        //        }));
        //    }
        //}
    }
}