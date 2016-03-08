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
    /// Interaction logic for MOZF.xaml
    /// </summary>
    public partial class MOZF : UserControl
    {
        private int _Color;
        private string DescriptionMOZF;
        private string StatusMOZF;
        private bool FaultMOZF;
        private bool _IsOnProfibus;
        private string _ObjectNo;
        private string _PLCName;
        Worker workerObject;

        // The worker threads runs in the background and updates our control
        #region[Worker Thread]
        public class Worker
        {
            S7Client plc;
            MOZF parent;
            private int updateTime = 100;
            public int dbnumber { get; set; }
            public int dboffset { get; set; }
            public int dboffsetSpeedMonitor { get; set; }

            public Worker(S7Client tmp, MOZF parent)
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
                            parent.MOZFColor = BitConverter.ToUInt16(buffer, 0);
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

        public MOZF()
        {
            InitializeComponent();
            MOZF_IsOnProfibus = false;
        }


        //------------------------------------------------------------------------------//
        //                                   Properties                                 //
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

        [Obsolete("SetColor is Obsolete. Please Use MOZFColor instead."), Category("Buhler")]
        public int SetColor
        {
            set
            {
                FB11 MOZF = new FB11();
                _SetColor(MOZF.SetColor(value));
                StatusMOZF = MOZF.Status_DO_Element;
                FaultMOZF = MOZF.Fault_DO_Element;
            }
        }

        [Category("Buhler")]
        public int MOZFColor
        {
            get
            {
                return _Color;
            }
            set
            {
                if (MOZF_IsOnProfibus)
                {
                    _Color = value;
                    FB34 MYFC = new FB34();
                    _SetColor(MYFC.SetColor(value));
                    StatusMOZF = MYFC.Status_MYFC;
                    FaultMOZF = MYFC.Fault_MYFC;
                }
                else
                {
                    _Color = value;
                    FB11 MOZF = new FB11();
                    _SetColor(MOZF.SetColor(value));
                    StatusMOZF = MOZF.Status_DO_Element;
                    FaultMOZF = MOZF.Fault_DO_Element;
                }
            }
        }

        //
        //  MOZF
        //
        [Category("Buhler")]
        public string Description_MOZF
        {
            get
            {
                return this.DescriptionMOZF;
            }
            set
            {
                this.DescriptionMOZF = value;
            }
        }

        [Category("Buhler")]
        public string Status_MOZF
        {
            get
            {
                return this.StatusMOZF;
            }
        }

        [Category("Buhler")]
        public bool Fault_MOZF
        {
            get
            {
                return this.FaultMOZF;
            }
        }

        [Category("Buhler")]
        public bool MOZF_IsOnProfibus
        {
            get
            {
                return _IsOnProfibus;
            }
            set
            {
                _IsOnProfibus = value;
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
        }
    }
}
