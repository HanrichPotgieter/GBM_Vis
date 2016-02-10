using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_Scale [Scale]
    /// </summary>
    public class FB40
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
                    return KNEKTColors.Lime;;

                case 3:                     //StDosing
                    Status = "StDosing";
                    return KNEKTColors.Green;
					
				case 9:                     //StEmtying
                    Status = "StEmtying";
                    return KNEKTColors.Aqua;
					
				case 10:                     //StEmptied
                    Status = "StEmptied";
                    return KNEKTColors.Gray;
					
				case 11:                     //StRefill
                    Status = "StRefill";
                    return KNEKTColors.Green;
					
				case 12:                     //StStartDelay
                    Status = "StStartDelay";
                    return KNEKTColors.Gray;
					
				case 23:                     //StDosing.Forced
                    Status = "StDosing.Forced";
                    return KNEKTColors.Yellow;
					
				case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;

                case 259:                     //StDosing
                    Status = "StDosing";
                    return KNEKTColors.Green; 

                case 513:                     //StPassiveStopped
                    Status = "StStoppedPassive";
                    return KNEKTColors.Gray;

                case 515:                     //StDosingPassive
                    Status = "StStoppedPassive";
                    return KNEKTColors.Green;


                case 2561:                  //StStopped.InHWStop
                    Status = "StStopped.InHWStop";
                    return KNEKTColors.Orange;

                case 16387:                     //StStarted.WarningNoProd
                    Status = "StStarted.WarningNoProd";
                    return KNEKTColors.Lime;

                case 16398:                     //StStopped.NIVEAULOW
                    Status = "StStopped.CWA";
                    return KNEKTColors.Gray;

                case 16408:                     //StStopped.NIVEAULOW
                    Status = "StDosing.CWA";
                    return KNEKTColors.Green;

                case 16910:                     //StStopped.NIVEAULOW
                    Status = "StStopped.CWA";
                    return KNEKTColors.Gray;
					
                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_Scale
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_Scale
        {
            get
            {
                return this.Fault;
            }
        }

    }
}


