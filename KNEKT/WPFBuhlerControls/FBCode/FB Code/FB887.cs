using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_SMKN [Discharger SMKN With Flow Contol]
    /// </summary>
    class FB887
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
					
			    case 2:                     //StCtoStartposition
                    Status = "StCtoStartposition";
                    return KNEKTColors.Lime;
                
                case 3:                     //StStartPosition
                    Status = "StStartPosition";
                    return KNEKTColors.Green;

                case 4:                     //StControl
                    Status = "StControl";
                    return KNEKTColors.Green;

                case 5:                     //StResidualDischarge
                    Status = "StResidualDischarge";
                    return KNEKTColors.Green;
                    
                case 7:                     //StClosing
                    Status = "StClosing";
                    return KNEKTColors.Lime;
					
				case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;
					
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_SMKN
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_SMKN
        {
            get
            {
                return this.Fault;
            }
        }
    }
}


