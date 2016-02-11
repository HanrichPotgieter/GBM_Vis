using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_DFCI [DFCI Pellet Press]
    /// </summary>
    class FB873
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
                
                case 3:                     //StSettingModeRight
                    Status = "StSettingModeRight";
                    return KNEKTColors.Green;

                case 4:                     //StStarted
                    Status = "StStarted";
                    return KNEKTColors.Green;

                case 5:                     //StSettingModeLeft
                    Status = "StSettingModeLeft";
                    return KNEKTColors.Green;
                    
                case 6:                     //StBypassed
                    Status = "StBypassed";
                    return KNEKTColors.Gray;
					
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

                case 16897:                     //StDisengaged
                    Status = "StDisengaged";
                    return KNEKTColors.Gray;
					
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_DFCI
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_DFCI
        {
            get
            {
                return this.Fault;
            }
        }
    }
}


