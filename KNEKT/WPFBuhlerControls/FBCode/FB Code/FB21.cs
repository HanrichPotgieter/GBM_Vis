using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_DMDS [Slide DMDS]
    /// </summary>
    public class FB21
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
					
			    case 2:                     //StOpening
                    Status = "StOpening";
                    return KNEKTColors.Lime;
                
                case 3:                     //StCoarse
                    Status = "StCoarse";
                    return KNEKTColors.Green;

                case 4:                     //StClosing
                    Status = "StClosing";
                    return KNEKTColors.Lime;

                case 10:                     //StFine
                    Status = "StFine";
                    return KNEKTColors.Green;
                    
                case 11:                     //StOpen
                    Status = "StOpen";
                    return KNEKTColors.Green;
					
				case 21:                     //StStartingDelay
                    Status = "StStartingDelay";
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

        public string Status_Slide
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_Slide
        {
            get
            {
                return this.Fault;
            }
        }

    }
}


