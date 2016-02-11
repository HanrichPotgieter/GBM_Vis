using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_ScaleAdaptor_Scale [Adapt Third Party Scale To WinCos]
    /// </summary>
    class FB801
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
					
			    case 2:                     //StStarting
                    Status = "StStarting";
                    return KNEKTColors.Lime;;

                case 3:                     //StDosing
                    Status = "StDosing";
                    return KNEKTColors.Green;
					
				case 9:                     //StEmtying
                    Status = "StEmtying";
                    return KNEKTColors.Aqua;
					
				case 10:                     //StEmptied
                    Status = "StEmptied";
                    return KNEKTColors.Gray;
					
				case 11:                     //StRefill
                    Status = "StRefill";
                    return KNEKTColors.Green;
					
				case 12:                     //StStartDelay
                    Status = "StStartDelay";
                    return KNEKTColors.Gray;
					
				case 23:                     //StDosing.Forced
                    Status = "StDosing.Forced";
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

        public string Status_ScaleAdaptor
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_ScaleAdaptor
        {
            get
            {
                return this.Fault;
            }
        }
    }
}


