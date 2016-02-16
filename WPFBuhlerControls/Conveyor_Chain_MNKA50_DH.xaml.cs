using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using WPFBuhlerControls.FB_Code;
using Snap7;
using System.Threading;
using System.Windows.Media.Animation;
using System.Windows;

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
        private string _ObjectNo;
        private string _PLCName;
        Worker workerObject;

        // The worker threads runs in the background and updates our control
        #region[Worker Thread]
        public class Worker
        {
            S7Client plc;
            Conveyor_Chain_MNKA50_DH parent;
            private int updateTime = 100;
            public int dbnumber { get; set; }
            public int dboffset { get; set; }

            public Worker(S7Client tmp, Conveyor_Chain_MNKA50_DH parent)
            {
                plc = tmp;
                this.parent = parent;
            }
            // This method will be called when the thread is started.
            public void DoWork()
            {
                while (!_shouldStop)
                {
                    if (plc != null)
                    {
                        if (plc.Connected())
                        {
                            byte[] buffer = new byte[2];
                            Plc.DBRead(dbnumber, dboffset, 2, buffer);
                            Array.Reverse(buffer);
                            //Console.Out.WriteLine(BitConverter.ToUInt16(buffer, 0));
                            parent.MotorColor = BitConverter.ToUInt16(buffer, 0);
                            Thread.Sleep(200);   
                        }
                    }
                }
                Thread.Sleep(updateTime);
            }
            public void RequestStop()
            {
                _shouldStop = true;
            }
            // Volatile is used as hint to the compiler that this data
            // member will be accessed by multiple threads.
            private volatile bool _shouldStop;
        }
        #endregion

        public Conveyor_Chain_MNKA50_DH()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Runtime)
            {
                workerObject = new Worker(Plc.Instance, this);
                Thread workerThread = new Thread(workerObject.DoWork);
                workerThread.Start();
            }
          
        }

        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
        //------------------------------------------------------------------------------//


        [Category("Buhler")]
        public int dbnumber {
            get {
                return dbnumber;
            }
            set {
                workerObject.dbnumber = value;
            }
        }

        [Category("Buhler")]
        public int dboffset
        {
            get
            {
                return dbnumber;
            }
            set
            {
                workerObject.dboffset = value;
            }
        }

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
                _SetColor(Motor.SetColorAnimated(value),Motor);
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


        private void _SetColor(Color brushColor,FB12 Motor)
        {
            
            

            PolyConveyor.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                ColorAnimation animation;
                animation = new ColorAnimation();
                SolidColorBrush oldBrush;
                try
                {
                    oldBrush = PolyConveyor.Fill as SolidColorBrush;
                    animation.From = oldBrush.Color;
                }
                catch
                {
                    animation.From = Colors.White;
                }
                animation.To = brushColor;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1));
                if (PolyConveyor.Fill != null)
                {
                    PolyConveyor.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
                else
                {
                    PolyConveyor.Fill = new SolidColorBrush(Colors.White);
                    PolyConveyor.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
            }));

            RectLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                ColorAnimation animation;
                animation = new ColorAnimation();
                SolidColorBrush oldBrush;
                try
                {
                    oldBrush = PolyConveyor.Fill as SolidColorBrush;
                    animation.From = oldBrush.Color;
                }
                catch
                {
                    animation.From = Colors.White;
                }
                animation.To = brushColor;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1));
                if (RectLeft.Fill != null)
                {
                    RectLeft.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
                else
                {
                    RectLeft.Fill = new SolidColorBrush(Colors.White);
                    RectLeft.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
            }));

            RectRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                ColorAnimation animation;
                animation = new ColorAnimation();
                SolidColorBrush oldBrush;
                try
                {
                    oldBrush = PolyConveyor.Fill as SolidColorBrush;
                    animation.From = oldBrush.Color;
                }
                catch
                {
                    animation.From = Colors.White;
                }
                animation.To = brushColor;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1));
                if (RectRight.Fill != null)
                {
                    RectRight.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
                else
                {
                    RectRight.Fill = new SolidColorBrush(Colors.White);
                    RectRight.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
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