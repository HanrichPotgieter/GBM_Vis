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

        public Brush SetColor(int State)
        {
            switch (State)
            {
                case 1:                     //StMinP
                    Status = "StMinP";

                    if (IsMinimumPressureAlarm)
                    {
                        Fault = true;
                        return KNEKTColors.Red;
                    }

                    return KNEKTColors.Gray;
                
                case 3:                     //StNormalP.Ok
                    Status = "StNormalP.Ok";
                    return KNEKTColors.Green;

                case 4:                     //StCtoOverP
                    Status = "StCtoOverP";
                    return KNEKTColors.Orange;

                case 5:                     //StOverP
                    Status = "StOverP";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;

                case 513:                     //StMinPPassive
                    Status = "StMinPPassive";
                    return KNEKTColors.Gray;

                case 515:                     //StNormalPPassive
                    Status = "StNormalPPassive";
                    return KNEKTColors.Green;

                case 516:                     //StCToOverPPassive
                    Status = "StCtoOverPPassive";
                    return KNEKTColors.Orange;

                case 517:                     //StOverPPassive
                    Status = "StOverPPassive";
                    Fault = true;
                    return KNEKTColors.Red;

                case 16387:                  //StWarning Normal Pressure warning
                    Status = "StWarning Normal Pressure warning";
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

    }
}


