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
using System.Threading;
using Snap7;
using System.Windows.Media.Animation;

namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for Elevator_L.xaml
    /// </summary>
    public partial class Elevator_L : UserControl
    {
        private int _MotorColor;
        private int _SpeedMonitorColorTop;
        private int _SpeedMonitorColorBot;
        private int _BeltAlignmentColor1;
        private int _BeltAlignmentColor2;
        private int _BeltAlignmentColor3;
        private int _BeltAlignmentColor4;
        private int _BeltAlignmentColor5;
        private string _ObjectNo;
        private bool _IsTopOutletLeft;
        private bool _IsBottomOutletLeft;
        private bool _IsBottomOutletLeftAndRight;

        private string DescriptionMotor;
        private string StatusMotor;
        private bool FaultMotor;
        
        private string DescriptionSpeedMonitorTop;
        private string StatusSpeedMonitorTop;
        private bool FaultSpeedMonitorTop;

        private string DescriptionSpeedMonitorBottom;
        private string StatusSpeedMonitorBottom;
        private bool FaultSpeedMonitorBottom;

        private string StatusBeltAlignment1;
        private string StatusBeltAlignment2;
        private string StatusBeltAlignment3;
        private string StatusBeltAlignment4;
        private string StatusBeltAlignment5;

        private bool FaultBeltAlignment1;
        private bool FaultBeltAlignment2;
        private bool FaultBeltAlignment3;
        private bool FaultBeltAlignment4;
        private bool FaultBeltAlignment5;
        private string _PLCName;
        Worker workerObject;

        // The worker threads runs in the background and updates our control
        #region[Worker Thread]
        public class Worker
        {
            S7Client plc;
            Elevator_L parent;
            private int updateTime = 100;
            public int dbnumber { get; set; }
            public int dboffset { get; set; }
            public int dboffsetSpeedMonitor { get; set; }

            public Worker(S7Client tmp, Elevator_L parent)
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

                            buffer = new byte[2];
                            Plc.DBRead(dbnumber, dboffsetSpeedMonitor, 2, buffer);
                            Array.Reverse(buffer);
                            //Console.Out.WriteLine(BitConverter.ToUInt16(buffer, 0));
                            parent.SpeedMonitorColorBottom = BitConverter.ToUInt16(buffer, 0);
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


        public Elevator_L()
        {
            InitializeComponent();

            Elevator_IsTopOutletLeft = false;
            Elevator_HasBeltAlignMiddle = false;
            Elevator_IsBottomOutletLeft = true;

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
        public int dboffsetSpeedMonitor
        {
            get
            {
                return dboffsetSpeedMonitor;
            }
            set
            {
                workerObject.dboffsetSpeedMonitor = value;
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
                _SetColorMotor(Motor.SetColorAnimated(value));
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
        public int SpeedMonitorColorTop
        {
            get
            {
                return _SpeedMonitorColorTop;
            }
            set
            {
                _SpeedMonitorColorTop = value;

                FB14 Monitor = new FB14();
                _SetColorSpeedMonitor(Monitor.SetColor(value, 7135), true);
                StatusSpeedMonitorTop = Monitor.Status_DI_Status;
                FaultSpeedMonitorTop = Monitor.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public int SpeedMonitorColorBottom
        {
            get
            {
                return _SpeedMonitorColorBot;
            }
            set
            {
                _SpeedMonitorColorBot = value;

                FB14 Monitor = new FB14();
                _SetColorSpeedMonitor(Monitor.SetColor(value, 7135), false);
                StatusSpeedMonitorBottom = Monitor.Status_DI_Status;
                FaultSpeedMonitorBottom = Monitor.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public int BeltAlignmentColor1
        {
            get
            {
                return _BeltAlignmentColor1;
            }
            set
            {
                _BeltAlignmentColor1 = value;

                FB14 Monitor = new FB14();
                _SetColorBeltAlignment(Monitor.SetColor(value, 7135), 1);
                StatusBeltAlignment1 = Monitor.Status_DI_Status;
                FaultBeltAlignment1 = Monitor.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public int BeltAlignmentColor2
        {
            get
            {
                return _BeltAlignmentColor2;
            }
            set
            {
                _BeltAlignmentColor2 = value;

                FB14 Monitor = new FB14();
                _SetColorBeltAlignment(Monitor.SetColor(value, 7135), 2);
                StatusBeltAlignment2 = Monitor.Status_DI_Status;
                FaultBeltAlignment2 = Monitor.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public int BeltAlignmentColor3
        {
            get
            {
                return _BeltAlignmentColor3;
            }
            set
            {
                _BeltAlignmentColor3 = value;

                FB14 Monitor = new FB14();
                _SetColorBeltAlignment(Monitor.SetColor(value, 7135), 3);
                StatusBeltAlignment3 = Monitor.Status_DI_Status;
                FaultBeltAlignment3 = Monitor.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public int BeltAlignmentColor4
        {
            get
            {
                return _BeltAlignmentColor4;
            }
            set
            {
                _BeltAlignmentColor4 = value;

                FB14 Monitor = new FB14();
                _SetColorBeltAlignment(Monitor.SetColor(value, 7135), 4);
                StatusBeltAlignment4 = Monitor.Status_DI_Status;
                FaultBeltAlignment4 = Monitor.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public int BeltAlignmentColor5
        {
            get
            {
                return _BeltAlignmentColor5;
            }
            set
            {
                _BeltAlignmentColor5 = value;

                FB14 Monitor = new FB14();
                _SetColorBeltAlignment(Monitor.SetColor(value, 7135), 5);
                StatusBeltAlignment5 = Monitor.Status_DI_Status;
                FaultBeltAlignment5 = Monitor.Fault_DI_Element;
            }
        }



        [Category("Buhler")]
        public bool Elevator_HasSpeedMonitorTop
        {
            get
            {
                return Convert.ToBoolean(SpeedMon_Top.Visibility);
            }
            set
            {
                bool visible = Convert.ToBoolean(value);
                Visibility v = visible == true ? Visibility.Visible : Visibility.Hidden;
                SpeedMon_Top.Visibility = v;
            }
        }

        [Category("Buhler")]
        public bool Elevator_HasSpeedMonitorBottom
        {
            get
            {
                return Convert.ToBoolean(SpeedMon_Bot.Visibility);
            }
            set
            {
                bool visible = Convert.ToBoolean(value);
                Visibility v = visible == true ? Visibility.Visible : Visibility.Hidden;
                SpeedMon_Bot.Visibility = v;
            }
        }

        [Category("Buhler")]
        public bool Elevator_HasBeltAlignTop
        {
            get
            {
                return Convert.ToBoolean(BeltAlign_RectTop1.Visibility);
            }
            set
            {
                bool visible = Convert.ToBoolean(value);
                Visibility v = visible == true ? Visibility.Visible : Visibility.Hidden;
                BeltAlign_RectTop1.Visibility = v;
                BeltAlign_RectTop2.Visibility = v;
            }
        }

        [Category("Buhler")]
        public bool Elevator_HasBeltAlignBottom
        {
            get
            {
                return Convert.ToBoolean(BeltAlign_RectBot1.Visibility);
            }
            set
            {
                bool visible = Convert.ToBoolean(value);
                Visibility v = visible == true ? Visibility.Visible : Visibility.Hidden;
                BeltAlign_RectBot1.Visibility = v;
                BeltAlign_RectBot2.Visibility = v;
            }
        }

        [Category("Buhler")]
        public bool Elevator_HasBeltAlignMiddle
        {
            get
            {
                return Convert.ToBoolean(BeltAlign_Middle.Visibility);
            }
            set
            {
                bool visible = Convert.ToBoolean(value);
                Visibility v = visible == true ? Visibility.Visible : Visibility.Hidden;
                BeltAlign_Middle.Visibility = v;
                
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
        public string Description_SpeedMonitorTop
        {
            get
            {
                return this.DescriptionSpeedMonitorTop;
            }
            set
            {
                this.DescriptionSpeedMonitorTop = value;
            }
        }

        [Category("Buhler")]
        public string Status_SpeedMonitorTop
        {
            get
            {
                return this.StatusSpeedMonitorTop;
            }
        }

        [Category("Buhler")]
        public bool Fault_SpeedMonitorTop
        {
            get
            {
                return this.FaultSpeedMonitorTop;
            }
        }

        [Category("Buhler")]
        public string Description_SpeedMonitorBottom
        {
            get
            {
                return this.DescriptionSpeedMonitorBottom;
            }
            set
            {
                this.DescriptionSpeedMonitorBottom = value;
            }
        }

        [Category("Buhler")]
        public string Status_SpeedMonitorBottom
        {
            get
            {
                return this.StatusSpeedMonitorBottom;
            }
        }

        [Category("Buhler")]
        public bool Fault_SpeedMonitorBottom
        {
            get
            {
                return this.FaultSpeedMonitorBottom;
            }
        }

        [Category("Buhler")]
        public bool Elevator_IsTopOutletLeft
        {
            get
            {
                return this._IsTopOutletLeft;
            }
            set
            {
                this._IsTopOutletLeft = value;

                if (value)
                {
                    PolyTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        PolyTop.Visibility = Visibility.Hidden;
                    }));
                    PolyTopLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        PolyTopLeft.Visibility = Visibility.Visible;
                    }));
                }
                else
                {
                    PolyTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        PolyTop.Visibility = Visibility.Visible;
                    }));
                    PolyTopLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        PolyTopLeft.Visibility = Visibility.Hidden;
                    }));
                }
            }
        }

        [Category("Buhler")]
        public bool Elevator_IsBottomOutletLeft
        {
            get
            {
                return this._IsBottomOutletLeft;
            }
            set
            {
                this._IsBottomOutletLeft = value;

                if (value)
                {
                    PolyBotLeftAndRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        PolyBotLeftAndRight.Visibility = Visibility.Hidden;
                    }));
                    PolyBotRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        PolyBotRight.Visibility = Visibility.Hidden;
                    }));
                    PolyBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        PolyBot.Visibility = Visibility.Visible;
                    }));
                }
                else
                {
                    PolyBotRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        PolyBotRight.Visibility = Visibility.Visible;
                    }));
                    PolyBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        PolyBot.Visibility = Visibility.Hidden;
                    }));
                    PolyBotLeftAndRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        PolyBotLeftAndRight.Visibility = Visibility.Hidden;
                    }));
                }
            }
        }

        [Category("Buhler")]
        public bool Elevator_IsBottomOutletLeftAndRight
        {
            get
            {
                return this._IsBottomOutletLeftAndRight;
            }
            set
            {
                this._IsBottomOutletLeftAndRight = value;

                if (value)
                {
                    PolyBotLeftAndRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        PolyBotLeftAndRight.Visibility = Visibility.Visible;
                    }));
                    PolyBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        PolyBot.Visibility = Visibility.Hidden;
                    }));
                    PolyBotRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        PolyBotRight.Visibility = Visibility.Hidden;
                    }));
                }
                else
                {
                    PolyBotLeftAndRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        PolyBotLeftAndRight.Visibility = Visibility.Hidden;
                    }));
                    PolyBotRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        PolyBotRight.Visibility = Visibility.Hidden;
                    }));
                    PolyBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        PolyBot.Visibility = Visibility.Visible;
                    }));
                }
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
        //                              public Methods                                  //
        //------------------------------------------------------------------------------//

        [Obsolete("SpeedMonitorColor is Obsolete. Please use SpeedMonitorColor Instead.")]
        public void SpeedMonitorColor(int value, bool TopSpeedMonitor)
        {
            FB14 Monitor = new FB14();
            _SetColorSpeedMonitor(Monitor.SetColor(value, 7135), TopSpeedMonitor);


            if (TopSpeedMonitor)
            {
                StatusSpeedMonitorTop = Monitor.Status_DI_Status;
                FaultSpeedMonitorTop = Monitor.Fault_DI_Element;
            }
            else
            {
                StatusSpeedMonitorBottom = Monitor.Status_DI_Status;
                FaultSpeedMonitorBottom = Monitor.Fault_DI_Element;
            }
        }

        [Obsolete("BeltAlignmentColor is Obsolete. Please use BeltAlignmentColor Instead.")]
        public void BeltAlignmentColor(int value, int MonitorNumber)
        {
            FB14 Monitor = new FB14();
            _SetColorBeltAlignment(Monitor.SetColor(value, 7135), MonitorNumber);


            if (MonitorNumber == 1)
            {
                FaultBeltAlignment1 = Monitor.Fault_DI_Element;
                //BeltAlign_RectTop1.Fill
            }
            else if (MonitorNumber == 2)
            {
                FaultBeltAlignment2 = Monitor.Fault_DI_Element;
            }
            else if (MonitorNumber == 3)
            {
                FaultBeltAlignment3 = Monitor.Fault_DI_Element;
            }
            else if (MonitorNumber == 4)
            {
                FaultBeltAlignment4 = Monitor.Fault_DI_Element;
            }
            else if (MonitorNumber == 5)
            {
                FaultBeltAlignment5 = Monitor.Fault_DI_Element;
            }

        }


        //------------------------------------------------------------------------------//
        //                            Private Methods                                   //
        //------------------------------------------------------------------------------//

        private void _SetColorMotor(Color brushColor)
        {
            RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                ColorAnimation animation;
                animation = new ColorAnimation();
                SolidColorBrush oldBrush;
                try
                {
                    oldBrush = RectMain.Fill as SolidColorBrush;
                    animation.From = oldBrush.Color;
                }
                catch
                {
                    animation.From = Colors.White;
                }
                animation.To = brushColor;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1));
                if (RectMain.Fill != null)
                {
                    RectMain.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
                else
                {
                    RectMain.Fill = new SolidColorBrush(Colors.White);
                    RectMain.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
            }));

            PolyTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                ColorAnimation animation;
                animation = new ColorAnimation();
                SolidColorBrush oldBrush;
                try
                {
                    oldBrush = PolyTop.Fill as SolidColorBrush;
                    animation.From = oldBrush.Color;
                }
                catch
                {
                    animation.From = Colors.White;
                }
                animation.To = brushColor;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1));
                if (PolyTop.Fill != null)
                {
                    PolyTop.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
                else
                {
                    PolyTop.Fill = new SolidColorBrush(Colors.White);
                    PolyTop.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
            }));

            PolyTopLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                ColorAnimation animation;
                animation = new ColorAnimation();
                SolidColorBrush oldBrush;
                try
                {
                    oldBrush = PolyTopLeft.Fill as SolidColorBrush;
                    animation.From = oldBrush.Color;
                }
                catch
                {
                    animation.From = Colors.White;
                }
                animation.To = brushColor;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1));
                if (PolyTopLeft.Fill != null)
                {
                    PolyTopLeft.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
                else
                {
                    PolyTopLeft.Fill = new SolidColorBrush(Colors.White);
                    PolyTopLeft.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
            }));

            PolyBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                ColorAnimation animation;
                animation = new ColorAnimation();
                SolidColorBrush oldBrush;
                try
                {
                    oldBrush = PolyBot.Fill as SolidColorBrush;
                    animation.From = oldBrush.Color;
                }
                catch
                {
                    animation.From = Colors.White;
                }
                animation.To = brushColor;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1));
                if (PolyBot.Fill != null)
                {
                    PolyBot.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
                else
                {
                    PolyBot.Fill = new SolidColorBrush(Colors.White);
                    PolyBot.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }

            }));

            PolyBotRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                ColorAnimation animation;
                animation = new ColorAnimation();
                SolidColorBrush oldBrush;
                try
                {
                    oldBrush = PolyBotRight.Fill as SolidColorBrush;
                    animation.From = oldBrush.Color;
                }
                catch
                {
                    animation.From = Colors.White;
                }
                animation.To = brushColor;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1));
                if (PolyBotRight.Fill != null)
                {
                    PolyBotRight.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
                else
                {
                    PolyBotRight.Fill = new SolidColorBrush(Colors.White);
                    PolyBotRight.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
            }));
            PolyBotLeftAndRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                ColorAnimation animation;
                animation = new ColorAnimation();
                SolidColorBrush oldBrush;
                try
                {
                    oldBrush = PolyBotLeftAndRight.Fill as SolidColorBrush;
                    animation.From = oldBrush.Color;
                }
                catch
                {
                    animation.From = Colors.White;
                }
                animation.To = brushColor;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1));
                if (PolyBotLeftAndRight.Fill != null)
                {
                    PolyBotLeftAndRight.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
                else
                {
                    PolyBotLeftAndRight.Fill = new SolidColorBrush(Colors.White);
                    PolyBotLeftAndRight.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
            }));
        }


        private void _SetColorBeltAlignment(Brush brushColor, int MonitorNumber)
        {
            switch (MonitorNumber)
            {
                case 1:
                    RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        BeltAlign_RectTop1.Fill = brushColor;
                    }));
                    break;
                case 2:
                    RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        BeltAlign_RectTop2.Fill = brushColor;
                    }));
                    break;
                case 3:
                    RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        BeltAlign_RectBot1.Fill = brushColor;
                    }));
                    break;
                case 4:
                    RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        BeltAlign_RectBot2.Fill = brushColor;
                    }));
                    break;

                case 5:
                    RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        BeltAlign_Middle.Fill = brushColor;
                    }));
                    break;
            }
        }


        private void _SetColorSpeedMonitor(Brush brushColor, bool TopSpeedMonitor)
        {
            if (TopSpeedMonitor)
            {
                RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    SpeedMon_Top.Fill = brushColor;
                }));
            }
            else
            {
                RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    SpeedMon_Bot.Fill = brushColor;
                }));
            }
        }
    }
}
