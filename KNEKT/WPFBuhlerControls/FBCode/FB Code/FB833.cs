using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_MHSA [MHSA Impact Huller]
    /// </summary>
    class FB833
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

                case 4:                     //StStartedFwd
                    Status = "StStartedFwd";
                    return KNEKTColors.Green;
                    
                case 6:                     //StStartedRev
                    Status = "StStartedRev";
                    return KNEKTColors.Green;
					
				case 7:                     //StStopping
                    Status = "StStopping";
                    return KNEKTColors.Lime;
					
				case 9:                     //StStartRequest
                    Status = "StStartRequest";
                    return KNEKTColors.Yellow;
					
				case 11:                     //StStartingDelay
                    Status = "StStartingDelay";
                    return KNEKTColors.Gray;
					
				case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 33:                     //StFaultBackupProbe
                    Status = "StFaultBackupProbe";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 35:                     //StFaultComunication
                    Status = "StFaultComunication";
                    Fault = true;
                    return KNEKTColors.Red;
					
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_ImpactHuller
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_ImpactHuller
        {
            get
            {
                return this.Fault;
            }
        }
    }
}


