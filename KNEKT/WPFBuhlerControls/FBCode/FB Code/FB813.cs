using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_VLS_AO_AI [Valves With Analouge Output]
    /// </summary>
    class FB813
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State)
        {
            switch (State)
            {
                case 1:                     //StClosed
                    Status = "StClosed";
                    return KNEKTColors.Gray;
					
			    case 2:                     //StStarting
                    Status = "StStarting";
                    return KNEKTColors.Lime;
                
                case 3:                     //StStarted
                    Status = "StStarted";
                    return KNEKTColors.Green;

                case 7:                     //StStopping
                    Status = "StStopping";
                    return KNEKTColors.Lime;
					
				case 11:                     //StLN.StartDelay
                    Status = "StLN.StartDelay";
                    return KNEKTColors.Gray;
					
				case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;
					
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_Valve
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_Valve
        {
            get
            {
                return this.Fault;
            }
        }

    }
}


