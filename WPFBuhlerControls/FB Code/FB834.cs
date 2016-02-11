using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_MYFD_MOZG [MYFD Dampening + MOZG Water Dosing]
    /// </summary>
    class FB834
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
                    return KNEKTColors.Lime;
                
                case 3:                     //StDosing
                    Status = "StDosing";
                    return KNEKTColors.Green;

                case 5:                     //StWaiting
                    Status = "StWaiting";
                    return KNEKTColors.Green;
					
				case 7:                     //StStopping
                    Status = "StStopping";
                    return KNEKTColors.Lime;
					
				case 31:                     //StDosing.Local.MOZx
                    Status = "StDosing.Local.MOZx";
                    return KNEKTColors.Green;
					
				case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 33:                     //StDosing.Manual
                    Status = "StDosing.Manual";
                    return KNEKTColors.Green;
					
				case 34:                     //StDosing.Local.MYFD
                    Status = "StDosing.Local.MYFD";
                    return KNEKTColors.Green;
					
				case 51:                     //StWaiting.WaterDisabled
                    Status = "StWaiting.WaterDisabled";
                    return KNEKTColors.Yellow;
					
				case 52:                     //StDosing.Local.MOZx
                    Status = "StDosing.Local.MOZx";
                    return KNEKTColors.Green;
					
				case 53:                     //StDosing.Manual
                    Status = "StDosing.Manual";
                    return KNEKTColors.Green;
					
				case 54:                     //StDosing.Local.MYFD
                    Status = "StDosing.Local.MYFD";
                    return KNEKTColors.Green;
					
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_MOZG
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_MOZG
        {
            get
            {
                return this.Fault;
            }
        }

    }
}


