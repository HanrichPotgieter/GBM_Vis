using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// MYRO OPTOC
    /// </summary>
    public class FB74
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State)
        {
            State = State % 256; //Get the lower byte information

            switch (State)
            {
                case 1:                     //StStopped
                    Status = "StStopped";
                    return KNEKTColors.Gray;
					
			    case 2:                     //StWaiting
                    Status = "StWaiting";
                    return KNEKTColors.Lime;
                
                case 3:                     //StDosing
                    Status = "StDosing";
                    return KNEKTColors.Green;
					
				case 31:                     //StFault.Device
                    Status = "StFault.Device";
                    return KNEKTColors.Red;
					
				case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;
					
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_Device
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_Device
        {
            get
            {
                return this.Fault;
            }
        }
    }
}


