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
    /// Interaction logic for Switch_Pressure.xaml
    /// </summary>
    public partial class Switch_Pressure : UserControl
    {
        private int _Color;
        private string DescriptionPressureSwitch;
        private string StatusPressureSwitch;
        private bool FaultPressureSwitch;
        private bool _IsMinPressureAlarm;
        private bool _IsHighPressureAlarm;
        private bool _IsAnalogPressureSwitch = true;

        FB16 Switch;

        public Switch_Pressure()
        {
            InitializeComponent();
            Switch = new FB16();
        }

        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int PressureSwitchColor
        {
            get
            {
                return _Color;
            }
            set
            {
                _Color = value;
                if (PressureSwitch_IsAnalogPressureSwitch)
                {
                    Switch.IsMinimumPressureAlarm = IsMinimumPressureAlarm;
                    _SetColor(Switch.SetColor(value));
                    StatusPressureSwitch = Switch.Status_PressureSwitch;
                    FaultPressureSwitch = Switch.Fault_PressureSwitch;
                }
            }
        }

        [Category("Buhler")]
        public string Description_PressureSwitch
        {
            get
            {
                return this.DescriptionPressureSwitch;
            }
            set
            {
                this.DescriptionPressureSwitch = value;
            }
        }

        /// <summary>
        /// If true, Pressure switch will be Red when in Minimum pressure
        /// </summary>
        [Category("Buhler")]
        public bool IsMinimumPressureAlarm
        {
            get
            {
                return this._IsMinPressureAlarm;
            }
            set
            {
                this._IsMinPressureAlarm = value;
                Switch.IsMinimumPressureAlarm = value;
            }
        }

        /// <summary>
        /// If true, Pressure switch will be Red when in Minimum pressure
        /// </summary>
        [Category("Buhler")]
        public bool IsHighPressureAlarm
        {
            get
            {
                return this._IsHighPressureAlarm;
            }
            set
            {
                this._IsHighPressureAlarm = value;
                Switch.IsHighPressureAlarm = value;
            }
        }

        [Category("Buhler")]
        public string Status_PressureSwitch
        {
            get
            {
                return this.StatusPressureSwitch;
            }
        }

        [Category("Buhler")]
        public bool Fault_PressureSwitch
        {
            get
            {
                return this.FaultPressureSwitch;
            }
        }

        [Category("Buhler")]
        public bool PressureSwitch_IsAnalogPressureSwitch
        {
            get
            {
                return this._IsAnalogPressureSwitch;
            }
            set
            {
                _IsAnalogPressureSwitch = value;
            }
        }
         

        //------------------------------------------------------------------------------//
        //                                   Methods                                    //
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
