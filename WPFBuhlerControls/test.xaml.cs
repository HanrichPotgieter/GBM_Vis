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


namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for test.xaml
    /// </summary>
    public partial class test : UserControl
    {
        
        public String name { get; set; }
        S7Client plc;
        // The worker threads runs in the background and updates our control
        #region[Worker Thread]
        public class Worker
        {
            S7Client plc;
            test parent;
            public Worker(S7Client tmp,test parent) {
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
                            parent.setConnected();
                            Console.Out.WriteLine("Connected");
                            S7Client.S7CpuInfo Info = new S7Client.S7CpuInfo();
                            
                            plc.GetCpuInfo(ref Info);
                            int i = 0;
                            plc.PlcGetStatus(ref i);
                            parent.setText(Info.SerialNumber);
                            parent.setStatus(i);
                       
                        }
                        else
                        {
                            parent.setDiconnected();
                            Console.Out.WriteLine("Not Connected");
                        }
                    }
                }
                Thread.Sleep(100);
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

        //Here is our actual control
        #region[test Control]
        public test()
        {
            InitializeComponent();
            plc = Plc.Instance;
            Worker workerObject = new Worker(plc,this);
            Thread workerThread = new Thread(workerObject.DoWork);
            // Start the worker thread.
            workerThread.Start();

        }

        public void setConnected()
        {
           // InnerBox.Dispatcher.Invoke((Action)(() => {
                //InnerBox.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
           // }));
        }
        public void setDiconnected()
        {
           // InnerBox.Dispatcher.Invoke((Action)(()=> {
                //InnerBox.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
           // }));
           
        }
        public void setText(String text)
        {
            InnerBox.Dispatcher.Invoke((Action)(() => {
                Box.Text = text;
            }));

        }
        public void setStatus(int status)
        {
            Console.Out.WriteLine(status);
            InnerBox.Dispatcher.Invoke((Action)(() => {
                if(status == 8)
                    InnerBox.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                if(status == 4)
                    InnerBox.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }));

        }
        #endregion



    }
}
