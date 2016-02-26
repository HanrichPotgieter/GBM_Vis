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
using Snap7;
using System.Threading;
using WPFBuhlerControls.FB_Code;
using System.Windows.Media.Animation;

namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for SectionState.xaml
    /// </summary>
    public partial class SectionState : UserControl
    {
        private int _dbnumber;
        private int _dboffset;
        Worker workerObject;
        private int _SectionColor;
        private int _SectionErrorColor;
        // The worker threads runs in the background and updates our control
        #region[Worker Thread]
        public class Worker
        {
            S7Client plc;
            SectionState parent;
            private int updateTime = 100;
            public int dbnumber { get; set; }
            public int dboffset { get; set; }

            public Worker(S7Client tmp, SectionState parent)
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
                            parent.SectionColor = BitConverter.ToUInt16(buffer, 0);

                            buffer = new byte[2];
                            Plc.DBRead(dbnumber, 105, 1, buffer);
                            //Array.Reverse(buffer);
                            parent.SectionErrorColor = BitConverter.ToUInt16(buffer, 0);
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

        public SectionState()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Runtime)
            {
                workerObject = new Worker(Plc.Instance, this);
                Thread workerThread = new Thread(workerObject.DoWork);
                workerThread.Start();
            }
        }

        public int dbnumber
        {
            get
            {
                return _dbnumber;
            }
            set
            {
                _dbnumber = value;
                workerObject.dbnumber = _dbnumber;
            }
        }

        public int dboffset
        {
            get
            {
                return _dboffset;
            }
            set
            {
                _dboffset = value;
                workerObject.dboffset = _dboffset;
            }
        }


        //------------------------------------------------------------------------------//
        //                                   Methods                                    //
        //------------------------------------------------------------------------------//

        public int SectionColor
        {
            get
            {
                return _SectionColor;
            }
            set
            {
                _SectionColor = value;
                FB93 Section = new FB93();
                _SetSectionColor(Section.SetColorStatus(value));
                StatusText.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
                {
                    StatusText.Content = Section.Status;
                }));
               
            }
        }
        private void _SetSectionColor(Color brushColor)
        {
           
            RectStatus.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
            {
                ColorAnimation animation;
                animation = new ColorAnimation();
                SolidColorBrush oldBrush;
                try
                {
                    oldBrush = RectStatus.Fill as SolidColorBrush;
                    animation.From = oldBrush.Color;
                }
                catch
                {
                    animation.From = Colors.White;
                }
                animation.To = brushColor;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1));
                if (RectStatus.Fill != null)
                {
                    RectStatus.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
                else
                {
                    RectStatus.Fill = new SolidColorBrush(Colors.White);
                    RectStatus.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
            }));
        }

        public int SectionErrorColor
        {
            get
            {
                return _SectionErrorColor;
            }
            set
            {
                _SectionErrorColor = value;
                FB93 Section = new FB93();
                _SetSectionErrorColor(Section.SetColorError(value));
                StatusErrorText.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
                {
                    StatusErrorText.Content = Section.StatusError;
                }));
            }
        }
        private void _SetSectionErrorColor(Color brushColor)
        {
            RectError.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate ()
            {
                ColorAnimation animation;
                animation = new ColorAnimation();
                SolidColorBrush oldBrush;
                try
                {
                    oldBrush = RectError.Fill as SolidColorBrush;
                    animation.From = oldBrush.Color;
                }
                catch
                {
                    animation.From = Colors.White;
                }
                animation.To = brushColor;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1));
                if (RectError.Fill != null)
                {
                    RectError.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
                else
                {
                    RectError.Fill = new SolidColorBrush(Colors.White);
                    RectError.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
            }));
        }
    }
}
