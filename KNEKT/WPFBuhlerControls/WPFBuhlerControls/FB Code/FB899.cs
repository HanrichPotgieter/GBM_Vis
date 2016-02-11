using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_ASi [For WAGO AS-Interface Master]
    /// </summary>
    class FB899
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State)
        {
            switch (State)
            {
                case 0:                     //StNotUsed
                    Status = "StNotUsed";
                    return KNEKTColors.Fuschia;
					
			    case 2:                     //StOk
                    Status = "StOk";
                    return KNEKTColors.Green;
                
                case 33:                     //StFault.DPMaster
                    Status = "StFault.DPMaster";
                    Fault = true;
                    return KNEKTColors.Red;

                case 34:                     //StFault.ASiMaster
                    Status = "StFault.ASiMaster";
                    Fault = true;
                    return KNEKTColors.Red;

                case 38:                     //StFault.Overflow
                    Status = "StFault.Overflow";
                    Fault = true;
                    return KNEKTColors.Red;
                    
                case 39:                     //StFault.ASiInactive
                    Status = "StFault.ASiInactive";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 40:                     //StFault.ErrPower
                    Status = "StFault.ErrPower";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 41:                     //StFault.ErrMapping
                    Status = "StFault.ErrMapping";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 42:                     //StFault.ErrGeneral
                    Status = "StFault.ErrGeneral";
                    Fault = true;
                    return KNEKTColors.Red;
				
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_ASI
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_ASI
        {
            get
            {
                return this.Fault;
            }
        }

    }
}


