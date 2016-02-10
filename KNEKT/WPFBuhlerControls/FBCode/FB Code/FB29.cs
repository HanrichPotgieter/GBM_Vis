using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_Sampler [MZER + MZEP]
    /// </summary>
    public class FB29
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State)
        {
            switch (State)
            {
                case 1:                     //StOff
                    Status = "StOff";
                    return KNEKTColors.Gray;

                case 2:                     //StWait
                    Status = "StWait";
                    return KNEKTColors.Lime;
                
                case 3:                     //StSampling
                    Status = "StSampling";
                    return KNEKTColors.Green;

                case 4:                     //StBottleChange
                    Status = "StBottleChange";
                    return KNEKTColors.Green;

                case 5:                     //StWarning
                    Status = "StWarning";
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

        public string Status_Sampler
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_Sampler
        {
            get
            {
                return this.Fault;
            }
        }
    }
}


