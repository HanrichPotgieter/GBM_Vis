using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_AI [Analogue Input]
    /// </summary>
    public class FB19
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State)
        {
            State = State % 256;

            switch (State)
            {
                case 1:                     //StLowLow
                    Status = "StLowLow";
                    return KNEKTColors.Red;
					
			    case 2:                     //StLow
                    Status = "StLow";
                    return KNEKTColors.Yellow;
                
                case 3:                     //StMiddle
                    Status = "StMiddle";
                    return KNEKTColors.Green;

                case 4:                     //StHigh
                    Status = "StHigh";
                    return KNEKTColors.Orange;

                case 5:                     //StHighHigh
                    Status = "StHighHigh";
                    return KNEKTColors.Red;
                    
                case 6:                     //StDisable
                    Status = "StDisable";
                    return KNEKTColors.Fuschia;
					
				case 21:                     //StLowLow.Faulted
                    Status = "StLowLow.Faulted";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 22:                     //StLow.Faulted
                    Status = "StLow.Faulted";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 23:                     //StMiddle.Faulted
                    Status = "StMiddle.Faulted";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 24:                     //StHigh.Faulted
                    Status = "StHigh.Faulted";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 25:                     //StHighHigh.Faulted
                    Status = "StHighHigh.Faulted";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 30:                     //StFaultAI.Local
                    Status = "StFaultAI.Local";
                    Fault = true;
                    return KNEKTColors.Red;
                    
                case 32:                     //StFaultAI.Active
                    Status = "StFaultAI.Active";
                    Fault = true;
                    return KNEKTColors.Red;
						
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_AI_Element
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_AI_Element
        {
            get
            {
                return this.Fault;
            }
        }

    }
}


