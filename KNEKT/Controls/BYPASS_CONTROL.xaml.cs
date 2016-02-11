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
using S7Link;
using System.Windows.Threading;

namespace KNEKT.Controls
{
    /// <summary>
    /// Interaction logic for BYPASS_CONTROL.xaml
    /// </summary>
    public partial class BYPASS_CONTROL : UserControl
    {

        public Controller PLC_W;
        private string _DBOffset = "DBX0.0";
        DispatcherTimer timerWriteBYPASS = new DispatcherTimer(); //The timer is pulsed, turned on for 500ms and then turned off
        private int _TimerMaximum;


        public BYPASS_CONTROL()
        {
            InitializeComponent();
            timerWriteBYPASS.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timerWriteBYPASS.Tick += new EventHandler(timerWriteBYPASS_Tick);
            PLC_W = BypassControl_WriteController;


        }

        public Controller BypassControl_WriteController
        {
            set
            {
                PLC_W = value;
            }
            get
            {
                return PLC_W;
            }
        }

        public string BYPASS_DBOffset
        {
            set
            {
                _DBOffset = value;
            }
            get
            {
                return _DBOffset;
            }
        }

        public string BYPASS_ELEMENT_NAME
        {
            set
            {
                lblBYPASS.Content = value;
            }
            get
            {
                return lblBYPASS.Content.ToString();
            }
        }

        public int BYPASS_TimerMaximum
        {
            get
            {
                return _TimerMaximum;
            }
            set
            {
                _TimerMaximum = value;
                BYPASSprogressbarTimer.Maximum = value;
            }
        }

        private void btnBYPASS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tag t1 = new Tag(BYPASS_DBOffset, S7Link.Tag.ATOMIC.BOOL, 1);
                t1.Value = true;

                if (!PLC_W.IsConnected)
                    PLC_W.Connect();

                PLC_W.WriteTag(t1);

                LogItem li = new LogItem(DateTime.Now, DateTime.Now.ToOADate(), "btn4034Reset", "Bypass Enabled", 30);
                MainWindow.alLoggerToSQL.Add(li);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Write Error --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            timerWriteBYPASS.Start();
        }

        public void timerWriteBYPASS_Tick(object sender, EventArgs e)
        {
            //Reset the start timer bit            

            try
            {
                Tag t1 = new Tag(BYPASS_DBOffset, S7Link.Tag.ATOMIC.BOOL, 1);
                t1.Value = false;

                if (!PLC_W.IsConnected)
                    PLC_W.Connect();

                PLC_W.WriteTag(t1);


            }
            catch (Exception ex)
            {
                MessageBox.Show("Write Error --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            timerWriteBYPASS.Stop();
        }



        public double BypassTimerValue
        {
            set
            {
                BYPASSprogressbarTimer.Value = value;
                lblBYPASSTimerValue.Content = "" + value + " Sec";
            }
        }

    }
}
