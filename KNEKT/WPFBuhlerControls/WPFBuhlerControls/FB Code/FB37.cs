using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_MDDx_DP [MDDR 4 Rolls + MDDT 8 Rolls Roller Stands]
    /// </summary>
    public class FB37
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State)
        {
            switch (State)
            {                
                case 3:                     //StOk
                    Status = "StOk";
                    return KNEKTColors.Green;
					
				case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;
					
                default:                    //State Not Included
                    Status = "StOk";
                    return KNEKTColors.Black;

            }
        }

        public string Status_MDDR
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_MDDR
        {
            get
            {
                return this.Fault;
            }
        }

    }
}


