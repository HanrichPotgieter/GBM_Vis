using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_POWER_WAGO [Power Measurement Module]
    /// </summary>
    class FB803
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State)
        {
            switch (State)
            {
                case 1:                     //StOk
                    Status = "StOk";
                    return KNEKTColors.Green;
					
			    case 2:                     //StCosPhiLow
                    Status = "StCosPhiLow";
                    return KNEKTColors.Yellow;
                
                case 3:                     //StVoltageLow
                    Status = "StVoltageLow";
                    return KNEKTColors.Yellow;

                case 4:                     //StVoltageHigh
                    Status = "StVoltageHigh";
                    return KNEKTColors.Orange;

                case 5:                     //StCurrentHigh
                    Status = "StCurrentHigh";
                    return KNEKTColors.Orange;
                    
                case 6:                     //StCosPhiLowLow
                    Status = "StCosPhiLowLow";
                    return KNEKTColors.Red;
					
				case 7:                     //StVoltageLowLow
                    Status = "StVoltageLowLow";
                    return KNEKTColors.Red;
					
				case 8:                     //StVoltageHighHigh
                    Status = "StVoltageHighHigh";
                    return KNEKTColors.Red;
					
				case 9:                     //StCurrentHighHigh
                    Status = "StCurrentHighHigh";
                    return KNEKTColors.Red;
					
				case 22:                     //StCosPhiLow.Faulted
                    Status = "StCosPhiLow.Faulted";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 23:                     //StVoltageLow.Faulted
                    Status = "StVoltageLow.Faulted";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 24:                     //StVoltageHigh.Faulted
                    Status = "StVoltageHigh.Faulted";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 25:                     //StCurrentHigh.Faulted
                    Status = "StCurrentHigh.Faulted";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 26:                     //StCosPhiLowLow.Faulted
                    Status = "StCosPhiLowLow.Faulted";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 27:                     //StVoltageLowLow.Faulted
                    Status = "StVoltageLowLow.Faulted";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 28:                     //StVoltageHighHigh.Faulted
                    Status = "StVoltageHighHigh.Faulted";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 29:                     //StCurrentHighHigh.Faulted
                    Status = "StCurrentHighHigh.Faulted";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 30:                     //StFault.DPFault
                    Status = "StFault.DPFault";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;
					
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_WAGO
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_WAGO
        {
            get
            {
                return this.Fault;
            }
        }
    }
}


