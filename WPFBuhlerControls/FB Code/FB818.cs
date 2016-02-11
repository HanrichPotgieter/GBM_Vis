using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_VFC_Adaptor [Adaptor For Variable Speed Drives]
    /// </summary>
    class FB818
    {
        private string Status;
        //private bool Fault = false;

        public Brush SetColor(int State)
        {
            switch (State)
            {
                case 1:                     //StFalse
                    Status = "StFalse";
                    return KNEKTColors.Gray;
                
                case 3:                     //StOk
                    Status = "StFalse";
                    return KNEKTColors.Green;
					
                default:                    //State Not Included
                    Status = "StFalse";
                    return KNEKTColors.Black;

            }
        }

        public string Status_Adaptor
        {
            get
            {
                return this.Status;
            }
        }
    }
}


