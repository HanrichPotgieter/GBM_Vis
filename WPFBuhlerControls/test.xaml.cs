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

        public class Worker
        {
            S7Client plc;
            public Worker(S7Client tmp) {
                plc = tmp;
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
                            //Console.Out.WriteLine("Connected");
                        }
                        else
                        {
                            //Console.Out.WriteLine("Not Connected");
                        }
                    }
                }
                
            }
            public void RequestStop()
            {
                _shouldStop = true;
            }
            // Volatile is used as hint to the compiler that this data
            // member will be accessed by multiple threads.
            private volatile bool _shouldStop;
        }

        public test()
        {
            InitializeComponent();
            plc = Plc.Instance;
            Worker workerObject = new Worker(plc);
            Thread workerThread = new Thread(workerObject.DoWork);
            // Start the worker thread.
            workerThread.Start();

        }


    }
}
