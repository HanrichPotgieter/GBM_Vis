using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_Sortex [Sortex]
    /// </summary>
    class FB805
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State)
        {
            State = State % 256;

            switch (State)
            {
                case 1:                     //StOff
                    Status = "StOff";
                    return KNEKTColors.Gray;
					
			    case 2:                     //StStarting
                    Status = "StStarting";
                    return KNEKTColors.Lime;
                
                case 3:                     //StSuspended
                    Status = "StSuspended";
                    return KNEKTColors.Aqua;

                case 5:                     //StSWaking
                    Status = "StSWaking";
                    return KNEKTColors.Lime;
                    
                case 6:                     //StWorking
                    Status = "StWorking";
                    return KNEKTColors.Green;
					
				case 7:                     //StEmptying
                    Status = "StEmptying";
                    return KNEKTColors.Aqua;
					
				case 8:                     //StEmptied
                    Status = "StEmptied";
                    return KNEKTColors.Gray;
					
				case 9:                     //StWaiting
                    Status = "StWaiting";
                    return KNEKTColors.Lime;
					
				case 11:                     //StRestartDelay
                    Status = "StRestartDelay";
                    return KNEKTColors.Lime;
					
				case 12:                     //StStartDelay
                    Status = "StStartDelay";
                    return KNEKTColors.Lime;
					
				case 13:                     //StIdling
                    Status = "StIdling";
                    return KNEKTColors.Lime;
					
				case 14:                     //StEmptying.Waiting
                    Status = "StEmptying.Waiting";
                    return KNEKTColors.Aqua;
					
				case 15:                     //StInitialCalibration
                    Status = "StInitialCalibration";
                    return KNEKTColors.Green;
					
				case 31:                     //StFault.Device
                    Status = "StFault.Device";
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

        public string Status_Sortex
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_Sortex
        {
            get
            {
                return this.Fault;
            }
        }
    }
}


