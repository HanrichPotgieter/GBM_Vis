using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_MEAF_Bag [Bag Control]
    /// </summary>
    public class FB27
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

                case 4:                     //StStarted.Packing
                    Status = "StStarted.Packing";
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

        public string Status_BagControl
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_BagControl
        {
            get
            {
                return this.Fault;
            }
        }

    }
}


