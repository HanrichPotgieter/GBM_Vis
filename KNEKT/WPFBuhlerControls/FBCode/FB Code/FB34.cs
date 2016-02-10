using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_MYFC_MOZF [MYFC + MOZF Liquid Control]
    /// </summary>
    public class FB34
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
                
                case 3:                     //StDosing
                    Status = "StDosing";
                    return KNEKTColors.Green;

                case 4:                     //StDosingManual
                    Status = "StDosingManual";
                    return KNEKTColors.Green;

                case 5:                     //StWaiting
                    Status = "StWaiting";
                    return KNEKTColors.Lime;
                    
                case 6:                     //StEmptying
                    Status = "StEmptying";
                    return KNEKTColors.Aqua;
					
				case 7:                     //StStopping
                    Status = "StStopping";
                    return KNEKTColors.Aqua;
					
				case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;

                case 257:                     //StStopped-Manual
                    Status = "StStopped-Manual";
                    return KNEKTColors.Gray;

                case 261:                     //StWaiting
                    Status = "StWaiting";
                    return KNEKTColors.Lime;

                case 288:                     //InManualStFault
                    Status = "StFault-Manual";
                    Fault = true;
                    return KNEKTColors.Red;

                case 513:                     //StStoppedPassive
                    Status = "StStoppedPassive";
                    return KNEKTColors.Gray;

                case 16897:                   //StStoppedPassiveWarning
                    Status = "StStoppedPWarning";
                    return KNEKTColors.Gray;
	
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_MYFC
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_MYFC
        {
            get
            {
                return this.Fault;
            }
        }
    }
}


