﻿using System;
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
    /// Interaction logic for Airlock_MPSN.xaml
    /// </summary>
    public partial class Airlock_MPSN_Cyclone : UserControl
    {
        private int _MotorColor;
        private string DescriptionMotor;
        private string StatusMotor;
        private bool FaultMotor;
        private bool AirlockIsLeft;
        private string _ObjectNo;
        private string _PLCName;
        Worker workerObject;

        // The worker threads runs in the background and updates our control
        #region[Worker Thread]
        public class Worker
        {
            S7Client plc;
            Airlock_MPSN_Cyclone parent;
            private int updateTime = 100;
            public int dbnumber { get; set; }
            public int dboffset { get; set; }
            public int dboffsetSpeedMonitor { get; set; }

            public Worker(S7Client tmp, Airlock_MPSN_Cyclone parent)
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

        public Airlock_MPSN_Cyclone()
        {

                InitializeComponent();
                if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Runtime)
                {
                    workerObject = new Worker(Plc.Instance, this);
                    Thread workerThread = new Thread(workerObject.DoWork);
                    workerThread.Start();
                
            }
            PolyMainLeft.Fill = KNEKTColors.NoControlColor;
            PolyMainRight.Fill = KNEKTColors.NoControlColor;
            RectTop.Fill = KNEKTColors.NoControlColor;
            Airlock_IsSourceFromLeft = true;
        }


        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int dbnumber
        {
            get
            {
                return dbnumber;
            }
            set
            {
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
                _SetColorMotor(Motor.SetColor(value));
                StatusMotor = Motor.Status_Motor;
                FaultMotor = Motor.Fault_Motor;
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

        [Category("Buhler")]
        public bool Airlock_IsSourceFromLeft    
        {
            get
            {
                return AirlockIsLeft;
            }
            set
            {
                AirlockIsLeft = value;
                AirlockPosition();
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

        //Changes the direction of the cyclones inlet
        private void AirlockPosition()
        {
            if (Airlock_IsSourceFromLeft)
            {
                PolyMainLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    PolyMainLeft.Visibility = Visibility.Visible;
                }));
                PolyMainRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    PolyMainRight.Visibility = Visibility.Hidden;
                }));
            }
            else
            {
                PolyMainLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    PolyMainLeft.Visibility = Visibility.Hidden;
                }));
                PolyMainRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    PolyMainRight.Visibility = Visibility.Visible;
                }));
            }
        }
        

        private void _SetColorMotor(Brush brushColor)
        {
            ElipseBottom.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                ElipseBottom.Fill = brushColor;      
            }));

            ElipseBottomCenter.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                ElipseBottomCenter.Fill = brushColor;      
            }));
        }
    }
}
