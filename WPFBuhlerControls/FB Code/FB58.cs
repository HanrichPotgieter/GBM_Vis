using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{
    /// <summary>
    /// [Flaker Control MEAG]
    /// </summary>
    public class FB58
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

                case 3:                     //StStandby
                    Status = "StStandby";
                    return KNEKTColors.Lime;

                case 4:                     //StProduction
                    Status = "StProduction";
                    return KNEKTColors.Green;

                case 5:                     //StStopping
                    Status = "StStopping";
                    return KNEKTColors.Aqua;

                case 6:
                    Status = "StService";//StService
                    return KNEKTColors.Aqua;

                case 21:                     //StStoppedFaulted
                    Status = "StStoppedFaulted";
                    Fault = true;
                    return KNEKTColors.Red;

                case 22:                     //StStartingFaulted
                    Status = "StStartingFaulted";
                    Fault = true;
                    return KNEKTColors.Red;

                case 23:                     //StStandbyFaulted
                    Status = "StStandbyFaulted";
                    Fault = true;
                    return KNEKTColors.Red;

                case 24:                     //StProductionFaulted
                    Status = "StProductionFaulted";
                    Fault = true;
                    return KNEKTColors.Red;

                case 25:                     //StStoppingFaulted
                    Status = "StStoppingFaulted";
                    Fault = true;
                    return KNEKTColors.Red;

                case 26:                     //StServiceFaulted
                    Status = "StServiceFaulted";
                    Fault = true;
                    return KNEKTColors.Red;

                case 31:                     //StFaultCom.Local
                    Status = "StFaultCom.Local";
                    Fault = true;
                    return KNEKTColors.Yellow;

                case 32:                     //StFault
                    Status = "StFault";
                    return KNEKTColors.Red;

                case 513:
                    Status = "StStoppedPassive";//StStoppedPassive
                    return KNEKTColors.Gray;

                case 515:
                    Status = "StStandbySequenceStop";//StStandbySequenceStop
                    return KNEKTColors.Aqua;

                case 518:
                    Status = "StService";//StService
                    return KNEKTColors.Aqua;

                case 543:
                    Status = "StFaultNoComms";//StFaultNoComms
                    Fault = true;
                    return KNEKTColors.Red;

                case 16387:
                    Status = "StStandbyWarning";//StStandbyWarning
                    return KNEKTColors.Orange;

                case 16388:
                    Status = "StProductionWarning";//StProductionWarning
                    return KNEKTColors.Orange;  
                 
                case 16390:
                    Status = "StServiceWarning";//StServiceWarning
                    return KNEKTColors.Orange;   

                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_Flaker
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_Flaker
        {
            get
            {
                return this.Fault;
            }
        }
    }
}
