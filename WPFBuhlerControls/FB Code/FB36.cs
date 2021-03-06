using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_MYRA_MDEx [MYRA + MYAG + MDEx]
    /// </summary>
    public class FB36
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State)
        {
            switch (State)
            {
                case 1:                     //StOff
                    Status = "StOff";
                    return KNEKTColors.Gray;
					
			    case 2:                     //StWaitForResponse
                    Status = "StWaitForResponse";
                    return KNEKTColors.Lime;
                
                case 3:                     //StOn.Ok
                    Status = "StOn.Ok";
                    return KNEKTColors.Green;
					
				case 4:                     //StOn.Warning
                    Status = "StOn.Warning";
                    return KNEKTColors.Orange;
					
				case 5:                     //StCtoFault
                    Status = "StCtoFault";
                    return KNEKTColors.Yellow;
					
				case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;
					
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_MYRA_MDEx
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_MYRA_MDEx
        {
            get
            {
                return this.Fault;
            }
        }

    }
}


