using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{
    /// <summary>
    /// SSW_MA_Motor6Mon    [Motor With 6 DI]
    /// </summary>
    public class FB50
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

                case 3:                     //StStartedFwd.Slow
                    Status = "StStartedFwd.Slow";
                    return KNEKTColors.Green;

                case 4:                     //StStartedFwd.Fast
                    Status = "StStartedFwd.Fast";
                    return KNEKTColors.Green;

                case 5:                     //StStartedRev.Slow
                    Status = "StStartedRev.Slow";
                    return KNEKTColors.Green;

                case 6:                     //StStartedRev.Fast
                    Status = "StStartedRev.Fast";
                    return KNEKTColors.Green;

                case 7:                     //StStopping
                    Status = "StStopping";
                    return KNEKTColors.Aqua;

                case 8:                     //StLocked
                    Status = "StLocked";
                    return KNEKTColors.Yellow;

                case 11:                     //StartDelay
                    Status = "StartDelay";
                    return KNEKTColors.Yellow;

                case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;

                case 513:                     //StStoppedPassive
                    Status = "StStoppedPassive";
                    return KNEKTColors.Gray;

                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_Motor
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_Motor
        {
            get
            {
                return this.Fault;
            }
        }
    }
}
