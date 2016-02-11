using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_RotEncoderBa_Fct [...]
    /// </summary>
    class FB806
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State)
        {
            switch (State)
            {
                case 1:                     //StNotInPosition
                    Status = "StNotInPosition";
                    return KNEKTColors.Red;
					
				case 2:                     //StWaiting
                    Status = "StWaiting";
                    return KNEKTColors.Lime;
                
                case 3:                     //StMoving
                    Status = "StMoving";
                    return KNEKTColors.Lime;

                case 4:                     //StInTargetPosition
                    Status = "StInTargetPosition";
                    return KNEKTColors.Green;

                case 5:                     //StInPosition
                    Status = "StInPosition";
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

        public string Status_Encoder
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_Encoder
        {
            get
            {
                return this.Fault;
            }
        }

    }
}


