using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_PID [PID Control For Fluidlift Airlock]
    /// </summary>
    class FB802
    {
        private string Status;
        //private bool Fault = false;

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
                
                case 3:                     //StRunning
                    Status = "StRunning";
                    return KNEKTColors.Green;
					
				case 4:                     //StHold
                    Status = "StHold";
                    return KNEKTColors.Yellow;
					
				case 5:                     //StLimiting
                    Status = "StLimiting";
                    return KNEKTColors.Orange;
					
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_PID
        {
            get
            {
                return this.Status;
            }
        }
    }
}


