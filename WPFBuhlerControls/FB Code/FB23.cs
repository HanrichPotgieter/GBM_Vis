using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_MYEB [Moisture Control Unit]
    /// </summary>
    public class FB23
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State)
        {
            switch (State)
            {					
			    case 1:                     //StOk
                    Status = "StOk";
                    return KNEKTColors.Green;
                
                case 3:                     //StFalse
                    Status = "StFalse";
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

        public string Status_MoistureControUnit
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_MoistureControUnit
        {
            get
            {
                return this.Fault;
            }
        }
    }
}


