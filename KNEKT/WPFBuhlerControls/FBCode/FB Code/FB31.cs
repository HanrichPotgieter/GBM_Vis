using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_Maintanace2 [Monitor maintenace of Object - Advanced]
    /// </summary>
    public class FB31
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State)
        {
            switch (State)
            {					
			    case 2:                     //StWarning
                    Status = "StWarning";
                    return KNEKTColors.Yellow;
                
                case 3:                     //StNormal
                    Status = "StNormal";
                    return KNEKTColors.Green;
					
				case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;
					
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_MotorMaintenance
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_MotorMaintenance
        {
            get
            {
                return this.Fault;
            }
        }
    }
}


