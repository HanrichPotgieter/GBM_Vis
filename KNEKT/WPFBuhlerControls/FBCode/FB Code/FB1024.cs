using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{
    /// <summary>
    /// AEROGLIDE DRYER
    /// </summary>
    public class FB1024
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

                case 3:                     //StStarted
                    Status = "StStarted";
                    return KNEKTColors.Green;

                case 4:                     //StCooling / StStopping
                    Status = "StCooling";
                    return KNEKTColors.Aqua;

                case 5:                     //StWarning
                    Status = "StWarning";
                    return KNEKTColors.Purple;

                case 20:                     //StEStop
                    Status = "StEStop";
                    return KNEKTColors.Gray;

                case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;                

                default:                    //State Not Included
                    Status = "NoCommsToDryer";
                    return KNEKTColors.Red;
            }
        }

        public string Status_Dryer
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_Dryer
        {
            get
            {
                return this.Fault;
            }
        }
    }
}
