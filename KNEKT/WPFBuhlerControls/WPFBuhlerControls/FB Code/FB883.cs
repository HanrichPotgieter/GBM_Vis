using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_MRMA [Mixer Outlet]
    /// </summary>
    class FB883
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
                
                case 3:                     //StOpen
                    Status = "StOpen";
                    return KNEKTColors.Green;

                case 4:                     //StClosing
                    Status = "StClosing";
                    return KNEKTColors.Lime;

                case 5:                     //StNoPosition
                    Status = "StNoPosition";
                    Fault = true;
                    return KNEKTColors.Red;
                    
                case 21:                     //StClosed.Faulted
                    Status = "StClosed.Faulted";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 22:                     //StOpening.Faulted
                    Status = "StOpening.Faulted";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 23:                     //StOpen.Faulted
                    Status = "StOpen.Faulted";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 24:                     //StClosed.Faulted
                    Status = "StClosed.Faulted";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 25:                     //StNoPosition.Faulted
                    Status = "StNoPosition.Faulted";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;
					
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_MRMA
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_MRMA
        {
            get
            {
                return this.Fault;
            }
        }

    }
}


