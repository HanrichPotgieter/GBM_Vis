using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_FBAL [Flowbalancer]
    /// </summary>
    public class FB28
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State)
        {
            State = State % 256; //Get the lower byte information
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
					
				case 7:                     //StStopping
                    Status = "StStopping";
                    return KNEKTColors.Lime;
					
				case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;

                case 513:                     //StStoppedPassive
                    Status = "StStoppedPassive";
                    return KNEKTColors.Gray;

                //case 263:                     //StStarted
                //    Status = "StStarted";
                //    return KNEKTColors.Green;

                case 259:                     //StStarted
                    Status = "StDosing-Manual";
                    return KNEKTColors.Green;
					
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_Flowbalancer
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_Flowbalancer
        {
            get
            {
                return this.Fault;
            }
        }

    }
}


