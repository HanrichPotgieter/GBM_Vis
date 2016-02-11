using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_DCOS [DCOS (Dosing Control System)]
    /// </summary>
    public class FB43
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State)
        {
            switch (State)
            {
                case 1:                     //StPassive
                    Status = "StPassive";
                    return KNEKTColors.Gray;
					
			    case 2:                     //StStarting
                    Status = "StStarting";
                    return KNEKTColors.Lime;
                
                case 3:                     //StDosing
                    Status = "StDosing";
                    return KNEKTColors.Green;

                case 4:                     //StSuspended
                    Status = "StSuspended";
                    return KNEKTColors.Yellow;

                case 5:                     //StWaiting
                    Status = "StWaiting";
                    return KNEKTColors.Green;
					
				case 9:                     //StEmptying
                    Status = "StEmptying";
                    return KNEKTColors.Aqua;
					
				case 10:                     //StEmptied
                    Status = "StEmptied";
                    return KNEKTColors.Gray;
					
				case 14:                     //StRegistering
                    Status = "StRegistering";
                    return KNEKTColors.Orange;
					
				case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;
				
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_DCOS
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_DCOS
        {
            get
            {
                return this.Fault;
            }
        }
    }
}


