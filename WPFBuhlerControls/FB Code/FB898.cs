using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_MEAG_VFC [Interface To MEAG-CANOpen-Gateway]
    /// </summary>
    class FB898
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State)
        {
            switch (State)
            {
                case 1:                     //StFalse
                    Status = "StFalse";
                    return KNEKTColors.Gray;
                
                case 3:                     //StOk
                    Status = "StOk";
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

        public string Status_MEAG_VFC
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_MEAG_VFC
        {
            get
            {
                return this.Fault;
            }
        }
    }
}


