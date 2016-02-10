using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_MDDx [MDDM 4 Rolls + MDDO 8 Rolls Newtronic Roller Stands]
    /// </summary>
    public class FB35
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

                case 5:                     //StWarning
                    Status = "StWarning";
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

        public string Status_MDDM
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_MDDM
        {
            get
            {
                return this.Fault;
            }
        }

    }
}


