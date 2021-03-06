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
    /// Interaction logic for RollerStand_Engage.xaml
    /// </summary>
    public partial class RollerStand_Engage : UserControl
    {
        private int _EngageColor;
        private string DescriptionEngage;
        private string StatusEngage;
        private bool FaultEngage;
        private string _ObjectNo;
        private string _PLCName;
        Worker workerObject;

        // The worker threads runs in the background and updates our control
        #region[Worker Thread]
        public class Worker
        {
            S7Client plc;
            RollerStand_Engage parent;
            private int updateTime = 100;
            public int dbnumber { get; set; }
            public int dboffset { get; set; }
            public int dboffsetSpeedMonitor { get; set; }

            public Worker(S7Client tmp, RollerStand_Engage parent)
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
                            parent.EngageColor = BitConverter.ToUInt16(buffer, 0);
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

        public RollerStand_Engage()
        {
            InitializeComponent();

            polyLeftClosed.Visibility = Visibility.Hidden;
            polyLeftOpen.Visibility = Visibility.Hidden;
            polyRightClosed.Visibility = Visibility.Hidden;
            polyRightOpen.Visibility = Visibility.Hidden;

            rectMain.Fill = KNEKTColors.Gray;
        }

        //------------------------------------------------------------------------------//
        //                                Properties                                    //
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
        public int EngageColor
        {
            get
            {
                return _EngageColor;
            }
            set
            {
                _EngageColor = value;
                FB11 engage = new FB11();
                _SetColor(engage.SetColor(value));
                StatusEngage = engage.Status_DO_Element;
                FaultEngage = engage.Fault_DO_Element;


                if (value == 3) //Engaged
                {
                    _SetEngage(true);
                }
                else//DisEngaged
                {
                    _SetEngage(false);
                }
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
        public string Description_Engage
        {
            get
            {
                return this.DescriptionEngage;
            }
            set
            {
                this.DescriptionEngage = value;
            }
        }
        
        [Category("Buhler")]
        public string Status_Engage
        {
            get
            {
                return this.StatusEngage;
            }
        }

        [Category("Buhler")]
        public bool Fault_Engage
        {
            get
            {
                return this.FaultEngage;
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
        private void _SetColor(Brush brushColor)
        {
            elipseLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                elipseLeft.Fill = brushColor;
            }));

            elipseRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                elipseRight.Fill = brushColor;
            }));
        }

        private void _SetEngage(bool Engage)
        {
            if (Engage)
            {
                polyLeftClosed.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    polyLeftClosed.Visibility = Visibility.Visible;
                }));

                polyRightClosed.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    polyRightClosed.Visibility = Visibility.Visible;
                }));

                polyLeftOpen.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    polyLeftOpen.Visibility = Visibility.Hidden;
                }));

                polyRightOpen.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    polyRightOpen.Visibility = Visibility.Hidden;
                }));
            }
            else
            {
                polyLeftClosed.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    polyLeftClosed.Visibility = Visibility.Hidden;
                }));

                polyRightClosed.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    polyRightClosed.Visibility = Visibility.Hidden;
                }));

                polyLeftOpen.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    polyLeftOpen.Visibility = Visibility.Visible;
                }));

                polyRightOpen.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    polyRightOpen.Visibility = Visibility.Visible;
                }));
            }
        }
    }
}
