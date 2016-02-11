using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_DFCO [Throughput Regulation]
    /// </summary>
    public class FB26
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
                
                case 3:                     //StStarted.Waiting
                    Status = "StStarted.Waiting";
                    return KNEKTColors.Green;

                case 4:                     //StStarted.Feeding
                    Status = "StStarted.Feeding";
                    return KNEKTColors.Green;
					
				case 7:                     //StStopping
                    Status = "StStopping";
                    return KNEKTColors.Lime;
					
				case 8:                     //StStartRequest
                    Status = "StStartRequest";
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

        public string Status_DFCO
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_DFCO
        {
            get
            {
                return this.Fault;
            }
        }

    }
}


