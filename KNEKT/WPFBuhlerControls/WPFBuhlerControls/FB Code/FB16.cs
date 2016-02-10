using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_PSRange [Pressure Range]
    /// </summary>
    public class FB16
    {
        private string Status;
        private bool Fault = false;
        private bool _IsMinPressureAlarm = false;
        private bool _IsHighPressureAlarm = true;

        public Brush SetColor(int State)
        {
            State = State % 256; //Get the lower byte information

            switch (State)
            {
                case 1:                     //StMinP
                    Status = "StMinP";

                    if (IsMinimumPressureAlarm)
                    {
                        Fault = true;
                        return KNEKTColors.Red;
                    }
                    Fault = false;
                    return KNEKTColors.Gray;

                case 2:                     //StCToUnderPressure
                    Status = "StCToUnderPressure";
                    Fault = false;
                    return KNEKTColors.Lime;
                
                case 3:                     //StNormalP.Ok
                    Status = "StNormalP.Ok";
                    Fault = false;
                    return KNEKTColors.Green;

                case 4:                     //StCtoOverP
                    Status = "StCtoOverP";
                    Fault = false;
                    return KNEKTColors.Orange;

                case 5:                     //StOverP
                    Status = "StOverP";
                    if (IsHighPressureAlarm)
                    {
                        Fault = true;
                        return KNEKTColors.Red;
                    }
                    else
                    {
                        Fault = false;
                        return KNEKTColors.Green;
                    }
                   
					
				case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;

                case 513:                     //StMinPPassive
                    Status = "StMinPPassive";
                    Fault = false;
                    return KNEKTColors.Gray;

                case 514:                     //StCToUnderPressure
                    Status = "StCToUnderPressure";
                    Fault = false;
                    return KNEKTColors.Lime;

                case 515:                     //StNormalPPassive
                    Status = "StNormalPPassive";
                    Fault = false;
                    return KNEKTColors.Green;

                case 516:                     //StCToOverPPassive
                    Status = "StCtoOverPPassive";
                    Fault = false;
                    return KNEKTColors.Orange;

                case 517:                     //StOverPPassive
                    Status = "StOverPPassive";
                    if (IsHighPressureAlarm)
                    {
                        Fault = true;
                        return KNEKTColors.Red;
                    }
                    else
                    {
                        Fault = false;
                        return KNEKTColors.Green;
                    }

                case 16387:                  //StWarning Normal Pressure warning
                    Status = "StWarning Normal Pressure warning";
                    Fault = false;
                    return KNEKTColors.Orange;

                case 16899:                  //StNormalIPWarning
                    Status = "StNormalIPWarning";
                    return KNEKTColors.Green;

                case 16901:                  //StWarningOverPressure
                    Status = "StWarningOverPressure";
                    return KNEKTColors.Orange;
					
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;
            }
        }

        public string Status_PressureSwitch
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_PressureSwitch
        {
            get
            {
                return this.Fault;
            }
        }

        public bool IsMinimumPressureAlarm
        {
            get
            {
                return this._IsMinPressureAlarm;
            }
            set
            {
                this._IsMinPressureAlarm = value;
            }
        }

        public bool IsHighPressureAlarm
        {
            get { return this._IsHighPressureAlarm; }
            set { this._IsHighPressureAlarm = value; }
        }

    }
}


