using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFBuhlerControls;
using System.Windows.Media;

namespace WPFBuhlerControls.FB_Code
{
    /// <summary>
    /// SSW_EL_DFCQ [Hammer Mill]
    /// </summary>
    class FB826
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State)
        {
            State = State % 256;

            switch (State)
            {
                case 1:                     //StStopped
                    Status = "StStopped";
                    return KNEKTColors.Gray;

                case 2:                     //StStarting
                    Status = "StStarting";
                    return KNEKTColors.Lime;

                case 3:                     //StStarted.Waiting
                    Status = "Waiting";
                    return KNEKTColors.Aqua;

                case 4:                     //StStarted.Feeding
                    Status = "Feeding";
                    return KNEKTColors.Green;

                case 7:                     //StStopping
                    Status = "StStopping";
                    return KNEKTColors.Lime;

                case 8:                     //StStartRequest
                    Status = "StStartRequest";
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

        public string Status_MEAG
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_MEAG
        {
            get
            {
                return this.Fault;
            }
        }
    }
}
