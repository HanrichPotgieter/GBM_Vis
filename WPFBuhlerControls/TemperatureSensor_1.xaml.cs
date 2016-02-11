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
    public enum eSwitchType
    {
        DIGITAL,
        ANALOG
    }


    /// <summary>
    /// Interaction logic for TemperatureSensor_1.xaml
    /// </summary>
    public partial class TemperatureSensor_1 : UserControl
    {
        private bool _IsLowLowAlarm;
        private int _SensorColor;
        private string DescriptionSwitch;
        private string StatusSwitch;
        private bool FaultSwitch;

        public TemperatureSensor_1()
        {
            InitializeComponent();

            SwitchType = eSwitchType.DIGITAL;
        }

        /// <summary>
        /// Set the color of the motor
        /// </summary>
        /// <param name="StateCode">State Code of the element</param>
        [Obsolete("SetColor is Obsolete. Please use SensorColor instead."), Category("Buhler")]
        public void SetColor(int StateCode)
        {
            FB14 HL = new FB14();
            _SetColor(HL.SetColor(StateCode, 7131));
            StatusSwitch = HL.Status_DI_Status;
            FaultSwitch = HL.Fault_DI_Element;
        }



        //------------------------------------------------------------------------------//
        //                                   Properties                                 //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]     
        public int SensorColor
        {
            get
            {
                return _SensorColor;
            }
            set
            {
                if (SwitchType == eSwitchType.DIGITAL)
                {
                    _SensorColor = value;
                    FB14 HL = new FB14();
                    _SetColor(HL.SetColor(value, 7131));
                    StatusSwitch = HL.Status_DI_Status;
                    FaultSwitch = HL.Fault_DI_Element;
                }
                else
                {
                    _SensorColor = value;
                    FB19 sensor = new FB19();
                    _SetColor(sensor.SetColor(value));
                    StatusSwitch = sensor.Status_AI_Element;

                    if (Status_Switch == "StLowLow")
                    {
                        if (IsLowLowAlarm)
                            FaultSwitch = sensor.Fault_AI_Element;
                        else
                        {
                            FaultSwitch = false;
                            _SetColor(sensor.SetColor(2));
                        }
                    }
                    else
                    {
                        FaultSwitch = sensor.Fault_AI_Element;
                    }
                }
            }
        }

        [Category("Buhler")]
        public string Description_Switch
        {
            get
            {
                return this.DescriptionSwitch;
            }
            set
            {
                this.DescriptionSwitch = value;
            }
        }

        [Category("Buhler")]
        public string Status_Switch
        {
            get
            {
                return this.StatusSwitch;
            }
        }

        [Category("Buhler")]
        public bool Fault_Switch
        {
            get
            {
                return this.FaultSwitch;
            }
        }

        [Category("Buhler")]
        public eSwitchType SwitchType
        {
            get;
            set;
        }

        [Category("Buhler")]
        public bool IsLowLowAlarm
        {
            get
            {
                return this._IsLowLowAlarm;
            }
            set
            {
                this._IsLowLowAlarm = value;                
            }
        }

        //------------------------------------------------------------------------------//
        //                                     Methods                                  //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            elipseMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                elipseMain.Fill = brushColor;
            }));
        }       
    }
}
