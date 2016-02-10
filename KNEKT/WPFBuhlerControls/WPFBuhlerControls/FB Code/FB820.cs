using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_Flowmeter [Flowmeter]
    /// </summary>
    class FB820
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
                
                case 3:                     //StDosing
                    Status = "StDosing";
                    return KNEKTColors.Green;

                case 4:                     //StDosing.StopPoint
                    Status = "StDosing.StopPoint";
                    return KNEKTColors.Green;

                case 5:                     //StDosingFinished
                    Status = "StDosingFinished";
                    return KNEKTColors.Green;
                    
                case 6:                     //StStopped.Finished
                    Status = "StStopped.Finished";
                    return KNEKTColors.Gray;
					
				case 7:                     //StStopped.Leak
                    Status = "StStopped.Leak";
                    return KNEKTColors.Orange;
					
				case 14:                     //StDosing.Min
                    Status = "StDosing.Min";
                    return KNEKTColors.Green;
					
				case 15:                     //StDosing.Max
                    Status = "StDosing.Max";
                    return KNEKTColors.Green;
					
				case 23:                     //StDosing.WarningMin
                    Status = "StDosing.WarningMin";
                    return KNEKTColors.Yellow;
					
				case 24:                     //StDosing.WarningMax
                    Status = "StDosing.WarningMax";
                    return KNEKTColors.Yellow;
					
				case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 33:                     //StFault.AI
                    Status = "StFault.AI";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 34:                     //StFault.DryRun
                    Status = "StFault.DryRun";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 35:                     //StFault.FaultDev
                    Status = "StFault.FaultDev";
                    Fault = true;
                    return KNEKTColors.Red;
						
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_Flowmeter
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_Flowmeter
        {
            get
            {
                return this.Fault;
            }
        }
    }
}


