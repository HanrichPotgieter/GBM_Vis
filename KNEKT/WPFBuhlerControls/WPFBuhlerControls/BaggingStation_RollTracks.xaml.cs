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
    /// Interaction logic for BaggingStation_RollTracks.xaml
    /// </summary>
    public partial class BaggingStation_RollTracks : UserControl
    {
        private int _MotorColor;
        private string DescriptionMotor;
        private string StatusMotor;
        private bool FaultMotor;
        private string _ObjectNo;

        public BaggingStation_RollTracks()
        {
            InitializeComponent();

            rectBag.Fill = KNEKTColors.BinColor;
            rectTop1.Fill = KNEKTColors.Gray;
            rectTop2.Fill = KNEKTColors.Gray;
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
                StatusMotor = motor.Status_Motor;
                FaultMotor = motor.Fault_Motor;
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
        public string Description_Motor
        {
            get
            {
                return this.DescriptionMotor;
            }
            set
            {
                this.DescriptionMotor = value;
            }
        }

        [Category("Buhler")]
        public string Status_Motor
        {
            get
            {
                return this.StatusMotor;
            }
        }

        [Category("Buhler")]
        public bool Fault_Motor
        {
            get
            {
                return this.FaultMotor;
            }
        }


        //------------------------------------------------------------------------------//
        //                                    Methods                                   //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            rectTopPacker.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectTopPacker.Fill = brushColor;
            }));

            rectBotPacker.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectBotPacker.Fill = brushColor;
            }));

            //ellipse1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            //{
            //    ellipse1.Fill = brushColor;
            //}));

            //ellipse2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            //{
            //    ellipse2.Fill = brushColor;
            //}));

            //ellipse3.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            //{
            //    ellipse3.Fill = brushColor;
            //}));

            //ellipse4.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            //{
            //    ellipse4.Fill = brushColor;
            //}));

            //ellipse5.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            //{
            //    ellipse5.Fill = brushColor;
            //}));

            //ellipse6.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            //{
            //    ellipse6.Fill = brushColor;
            //}));

            //ellipse7.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            //{
            //    ellipse7.Fill = brushColor;
            //}));

            //ellipse8.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            //{
            //    ellipse8.Fill = brushColor;
            //}));
        }
    }
}
