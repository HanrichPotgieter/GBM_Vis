using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_MPAK [MPAK Sifter]
    /// </summary>
    public class FB40_MPAK
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
                    return KNEKTColors.Lime;;

                case 4:                     //StStarted
                    Status = "StStarted";
                    return KNEKTColors.Green;
					
				case 7:                     //StStopping
                    Status = "StStopping";
                    return KNEKTColors.Lime;
					
				case 11:                     //StStartingDelay
                    Status = "StStartingDelay";
                    return KNEKTColors.Gray;
					
				case 31:                     //StFault.FaultDev
                    Status = "StFault.FaultDev";
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
					
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_Sifter
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_Sifter
        {
            get
            {
                return this.Fault;
            }
        }

    }
}


