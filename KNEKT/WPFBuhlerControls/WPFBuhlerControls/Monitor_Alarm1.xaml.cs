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

namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for Monitor_Alarm1.xaml
    /// </summary>
    public partial class Monitor_Alarm1 : UserControl
    {
        private int _MonitorColor;
        private string DescriptionAlarm;
        private string StatusAlarm;
        private bool FaultAlarm;
        private string _FB = "FB14";
        private string _ObjectNo;
        private string _PLCName;

        public Monitor_Alarm1()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int MonitorColor
        {
            get
            {
                return _MonitorColor;
            }
            set
            {
                _MonitorColor = value;

                if (Alarm_FB == "FB11")
                {
                    FB11 alarm = new FB11();
                    _SetColor(alarm.SetColor(value));
                    StatusAlarm = alarm.Status_DO_Element;
                    FaultAlarm = alarm.Fault_DO_Element;
                }
                else if (Alarm_FB == "FB18")
                {
                    FB18 alarm = new FB18();
                    _SetColor(alarm.SetColor(value));
                    StatusAlarm = alarm.Status_VSD;
                    FaultAlarm = alarm.Fault_VSD;
                }
                else if (Alarm_FB == "FB40")
                {
                    FB40 alarm = new FB40();
                    _SetColor(alarm.SetColor(value));
                    StatusAlarm = alarm.Status_Scale;
                    FaultAlarm = alarm.Fault_Scale;
                }
                else if (Alarm_FB == "FB837")
                {
                    FB837 alarm = new FB837();
                    _SetColor(alarm.SetColor(value));
                    StatusAlarm = alarm.Status_MEAG;
                    FaultAlarm = alarm.Fault_MEAG;
                }
                else
                {
                    FB14 Alarm = new FB14();
                    _SetColor(Alarm.SetColor(value, 7147));
                    StatusAlarm = Alarm.Status_DI_Status;
                    FaultAlarm = Alarm.Fault_DI_Element;
                }
            }
        }

        [Category("Buhler")]
        public string Alarm_FB
        {
            get
            {
                return _FB;
            }
            set
            {
                _FB = value;
            }
        }

        [Category("Buhler")]
        public string Description_Alarm
        {
            get
            {
                return this.DescriptionAlarm;
            }
            set
            {
                this.DescriptionAlarm = value;
            }
        }

        [Category("Buhler")]
        public string Status_Alarm
        {
            get
            {
                return this.StatusAlarm;
            }
        }

        [Category("Buhler")]
        public bool Fault_Alarm
        {
            get
            {
                return this.FaultAlarm;
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
        //                                 Methods                                      //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {            
            rectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                rectMain.Fill = brushColor;
            }));
        }
    }
}
