using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_DO [Digital Output]
    /// </summary>
    public class FB11
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
                
                case 3:                     //StStarted
                    Status = "StStarted";
                    return KNEKTColors.Green;

                case 5:                     //StFalse
                    Status = "StFalse";
                    return KNEKTColors.Gray;
					
				case 11:                     //StStopped.StartDelay
                    Status = "StStopped.StartDelay";
                    return KNEKTColors.Gray;
					
				case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;

                case 257:                    //StStoppedManualOn
                    Status = "StStoppedManualEnabled";
                    Fault = false;
                    return KNEKTColors.Gray;

                case 259:                    //StStartedManualOn
                    Status = "StStartedManualEnabled";
                    Fault = false;
                    return KNEKTColors.Green;

                case 513:                    //StStopedPassive
                    Status = "StStoppedPassive";                    
                    return KNEKTColors.Gray;

                case 16387:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;

                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;
            }
        }

        public string Status_DO_Element
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_DO_Element
        {
            get
            {
                return this.Fault;
            }
        }
    }
}


