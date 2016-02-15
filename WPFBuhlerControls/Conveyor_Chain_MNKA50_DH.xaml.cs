using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using WPFBuhlerControls.FB_Code;
using Snap7;
using System.Threading;
using System.Collections;

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
        private S7Client PLC;
       

        // The worker threads runs in the background and updates our control
        #region[Worker Thread]
        public class Worker
        {
            S7Client plc;
            Conveyor_Chain_MNKA50_DH parent;
            private int updateTime = 100;
            int dbnumber = 161;
            int dboffset = 439;

            public Worker(S7Client tmp, Conveyor_Chain_MNKA50_DH parent,int dbnumber,int dboffset)
            {
                plc = tmp;
                this.parent = parent;
                //this.dbnumber = dbnumber;
                //this.dboffset = dboffset;
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
                            byte[] buffer = new byte[1];
                            plc.DBRead(dbnumber, dboffset, 1, buffer);
                            parent.MotorColor = buffer[0];
                            /*
                            bool StStopped = (buffer[0] & 0x01) != 0;
                            bool StStarted = (buffer[0] & 0x02) != 0;
                            bool StStartedFwd = (buffer[0] & 0x04) != 0;
                            bool StFault = (buffer[0] & 0x08) != 0;
                            bool StStrtedRev = (buffer[0] & 0x16) != 0;
                            Console.Out.WriteLine("DB number:" + dbnumber.ToString() + " Offset:" + dboffset.ToString());
                            Console.Out.WriteLine("=====================");
                            Console.Out.WriteLine(buffer[0]);
                            Console.Out.WriteLine(StStopped);
                            Console.Out.WriteLine(StStarted);
                            Console.Out.WriteLine(StStartedFwd);
                            Console.Out.WriteLine(StFault);
                            Console.Out.WriteLine(StStrtedRev);
                            Console.Out.WriteLine("=====================");
                            */
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
                Worker workerObject = new Worker(Plc.Instance, this, dbnumber, dboffset);
                Thread workerThread = new Thread(workerObject.DoWork);
                workerThread.Start();
            }
          
        }

        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
        //------------------------------------------------------------------------------//

        public int dbnumber { get; set; }
        public int dboffset { get; set; }

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