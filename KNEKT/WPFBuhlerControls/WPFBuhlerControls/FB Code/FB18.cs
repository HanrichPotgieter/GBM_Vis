using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_ATV [Altivar Variable Speed Drive]
    /// </summary>
    public class FB18
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State)
        {
            switch (State)
            {
                case 1:                     //StStopped
                    Status = "StStopped";
                    return KNEKTColors.Gray;
					
			    case 2:                     //StStarting
                    Status = "StStarting";
                    return KNEKTColors.Lime;
                
                case 3:                     //StStartedFwdSlow
                    Status = "StStartedFwdSlow";
                    return KNEKTColors.Green;

                case 4:                     //StStartedFwdFast
                    Status = "StStartedFwdFast";
                    return KNEKTColors.Green;

                case 5:                     //StStartedRevSlow
                    Status = "StStartedRevSlow";
                    return KNEKTColors.Green;
                    
                case 6:                     //StStartedRevFast
                    Status = "StStartedRevFast";
                    return KNEKTColors.Green;
					
				case 7:                     //StStopping
                    Status = "StStopping";
                    return KNEKTColors.Lime;
					
				case 9:                     //StStartRequest
                    Status = "StStartRequest";
                    return KNEKTColors.Gray;
					
				case 11:                     //StStartingDelay
                    Status = "StStartingDelay";
                    return KNEKTColors.Gray;
					
				case 31:                     //StFault.FaultATV
                    Status = "StFault.FaultATV";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 33:                     //StFault.Isolated
                    Status = "StFault.Isolated";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 34:                     //StFault.Overload
                    Status = "StFault.Overload";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 35:                     //StFault.Service
                    Status = "StFault.Service";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 36:                     //StFault.OutFaultCfgLine
                    Status = "StFault.OutFaultCfgLine";
                    Fault = true;
                    return KNEKTColors.Red;

                case 513:                     //StStoppedPassive
                    Status = "StStoppedPassive";
                    return KNEKTColors.Gray;

                case 515:                     //StStartedPassive
                    Status = "StStartedPassive";
                    return KNEKTColors.Green;

                case 4128:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;

                case 4608:                     //StStopped
                    Status = "StStopped";
                    return KNEKTColors.Gray;

                case 8223:                     //StFaultDevice
                    Status = "StFaultDevice";
                    Fault = true;
                    return KNEKTColors.FaultDevice;

                case 8705:                     //StStoppedFault
                    Status = "StStoppedFault";
                    Fault = true;
                    return KNEKTColors.FaultDevice;


					
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_VSD
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_VSD
        {
            get
            {
                return this.Fault;
            }
        }

    }
}


