using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_MTRI [Remote Adjusting Through Angle Af a Trieur]
    /// </summary>
    public class FB57
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
					
			    case 2:                     //StAdjusting
                    Status = "StAdjusting";
                    return KNEKTColors.Lime;
                
                case 3:                     //StAdjusted.Automatic
                    Status = "StAdjusted.Automatic";
                    return KNEKTColors.Green;

                case 4:                     //StEmptying
                    Status = "StEmptying";
                    return KNEKTColors.Aqua;
					
				case 11:                     //StStartingDelay
                    Status = "StStartingDelay";
                    return KNEKTColors.Gray;
					
				case 12:                     //StAdjusting.Calibration
                    Status = "StAdjusting.Calibration";
                    return KNEKTColors.Yellow;
					
				case 13:                     //StAdjusted.Hand
                    Status = "StAdjusted.Hand";
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

        public string Status_MTRI
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_MTRI
        {
            get
            {
                return this.Fault;
            }
        }

    }
}


