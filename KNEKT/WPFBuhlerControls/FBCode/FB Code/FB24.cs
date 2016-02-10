using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_MOZE [Liquids Flow Control Unit]
    /// </summary>
    public class FB24
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
                
                case 3:                     //StStarted
                    Status = "StStarted";
                    return KNEKTColors.Green;

                case 5:                     //StFalse
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

        public string Status_LiquidFlowControlUnit
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_LiquidFlowControlUnit
        {
            get
            {
                return this.Fault;
            }
        }
    }
}


