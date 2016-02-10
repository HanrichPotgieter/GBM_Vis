using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_VLS4 [Valve Control 4 Position]
    /// </summary>
    public class FB20
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
					
				case 6:                     //StMiddle1
                    Status = "StMiddle1";
                    return KNEKTColors.Green;
					
				case 7:                     //StMiddle2
                    Status = "StMiddle2";
                    return KNEKTColors.Green;
					
				case 11:                     //StLN.StartDelay
                    Status = "StLN.StartDelay";
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

        public string Status_Valve
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_Valve
        {
            get
            {
                return this.Fault;
            }
        }

    }
}


