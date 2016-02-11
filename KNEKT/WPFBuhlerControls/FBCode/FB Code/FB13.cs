using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_VLS [Valve Control]
    /// </summary>
    public class FB13
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State)
        {
            switch (State)
            {
                case 1:                     //StLN
                    Status = "StLN";
                    return KNEKTColors.Gray;
					
			    case 2:                     //StCtoHN
                    Status = "StCtoHN";
                    return KNEKTColors.Lime;
                
                case 3:                     //StHN
                    Status = "StHN";
                    return KNEKTColors.Green;

                case 4:                     //StCtoLN
                    Status = "StCtoLN";
                    return KNEKTColors.Lime;

                case 5:                     //StNoPosition
                    Status = "StNoPosition";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 11:                     //StLN.StartDelay
                    Status = "StLN.StartDelay";
                    return KNEKTColors.Gray;
					
				case 13:                     //StLN.StartDelay
                    Status = "StLN.StartDelay";
                    return KNEKTColors.Gray;
					
				case 15:                     //StNoPosition.StartDelay
                    Status = "StNoPosition.StartDelay";
                    Fault = true;
                    return KNEKTColors.Red;

                case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;

                case 257:                     //StStoppedManualOn
                    Status = "StStoppedManualOn";
                    return KNEKTColors.Gray;

                case 259:                     //StStartedManualOff
                    Status = "StStartedManualOff";
                    return KNEKTColors.Green;

                case 288:                     //StFaultManual
                    Status = "StFaultManual";
                    Fault = true;
                    return KNEKTColors.Red;

                case 513:                     //StLNPassive
                    Status = "StLNPassive";
                    return KNEKTColors.Gray;

                case 515:                     //StHNPassive
                    Status = "StHNPassive";
                    return KNEKTColors.Green;

                case 517:                     //StNoPositionPassive
                    Status = "StNoPositionPassive";
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


