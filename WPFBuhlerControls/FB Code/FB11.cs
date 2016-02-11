using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_DO [Digital Output]
    /// </summary>
    public class FB11
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State)
        {

            if (State < 0 || State > 32767)
            {                     //StFault
                Status = "StFault";
                Fault = true;
                return KNEKTColors.Red;
            }
            else if (State / 256 == 64)     //Get high byte info. Bit 15 is error, 14 is warning, if 14 is on value is 64. if 15 is on value is 128
            {
                Status = "StFault";
                Fault = true;
                return KNEKTColors.Red;
            }
            else
            {
                State = State % 256; //Get the lower byte information

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

                    case 5:                     //StFalse
                        Status = "StFalse";
                        return KNEKTColors.Gray;

                    case 7:                     //StStartRequest
                        Status = "StStartRequest";
                        return KNEKTColors.Gray;

                    case 11:                     //StStopped.StartDelay
                        Status = "StStopped.StartDelay";
                        return KNEKTColors.Gray;

                    case 32:                     //StFault
                        Status = "StFault";
                        Fault = true;
                        return KNEKTColors.Red;

                    case 257:                    //StStoppedManualOn
                        Status = "StStoppedManualEnabled";
                        Fault = false;
                        return KNEKTColors.Gray;

                    case 259:                    //StStartedManualOn
                        Status = "StStartedManualEnabled";
                        Fault = false;
                        return KNEKTColors.Green;

                    case 260:                     //StCtoLN
                        Status = "StCtoLN";
                        Fault = true;
                        return KNEKTColors.Lime;

                    case 258:                     //StCtoHN
                        Status = "StCtoHN";
                        Fault = true;
                        return KNEKTColors.Lime;

                    case 513:                    //StStopedPassive
                        Status = "StStoppedPassive";
                        return KNEKTColors.Gray;

                    case 16387:                     //StWarning
                        Status = "StWarning";
                        Fault = true;
                        return KNEKTColors.Orange;

                    default:                    //State Not Included
                        Status = "State Not Included";
                        return KNEKTColors.Black;
                }
            }
        }

        public string Status_DO_Element
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_DO_Element
        {
            get
            {
                return this.Fault;
            }
        }
    }
}


