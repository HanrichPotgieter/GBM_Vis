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
using Snap7;
using System.Threading;

namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for Separator_MTRC.xaml
    /// </summary>
    public partial class Separator_MTRC : UserControl
    {
        private int _MotorColor;
        private string DescriptionSeparator;
        private string StatusSeparator;
        private bool FaultSeparator;
        private string _ObjectNo;
        private string _PLCName;

        public Separator_MTRC()
        {
            InitializeComponent();
        }

        // The worker threads runs in the background and updates our control
        #region[Worker Thread]
        public class Worker
        {
            S7Client plc;
            Scale_Dosing1 parent;
            private int updateTime = 100;
            public int dbnumber { get; set; }
            public int dboffset { get; set; }
            public int dboffsetSpeedMonitor { get; set; }

            public Worker(S7Client tmp, Scale_Dosing1 parent)
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
                            parent.ScaleColor = BitConverter.ToUInt16(buffer, 0);
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

        public Separator_MTRC()
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
                StatusSeparator = motor.Status_Motor;
                FaultSeparator = motor.Fault_Motor;
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
        public string Description_Separator
        {
            get
            {
                return this.DescriptionSeparator;
            }
            set
            {
                this.DescriptionSeparator = value;
            }
        }

        [Category("Buhler")]
        public string Status_Separator
        {
            get
            {
                return this.StatusSeparator;
            }
        }

        [Category("Buhler")]
        public bool Fault_Separator
        {
            get
            {
                return this.FaultSeparator;
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
        //                                     Methods                                  //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectMain.Fill = brushColor;
            }));

            PolyMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                PolyMain.Fill = brushColor;
            }));
        }
    }
}
