using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WPFBuhlerControls.FB_Code
{
    /// <summary>
    /// CWA SCALE
    /// </summary>
    public class FB97
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State) //State is a HEXADECIMAL value
        {
            //State = Convert.ToInt32(""+State, 16);  //Convert from Hex to Decimal
           //State = State % 256;                    //Get the lower byte information

            if (State < 0 || State > 32767)
            {                     //StFault
                Status = "StFault";
                Fault = true;
                return KNEKTColors.Red;
            }
            else
            {
                State = State % 256;  

                switch (State)
                {
                    case 1:                     //StPassive
                        Status = "StPassive";
                        return KNEKTColors.Gray;

                    case 2:                     //StWaiting
                        Status = "StWaiting";
                        return KNEKTColors.Fuschia;

                    case 4:                     //StDosing
                        Status = "StDosing";
                        return KNEKTColors.Green;

                    case 8:                     //StEmptying
                        Status = "StEmptying";
                        return KNEKTColors.Aqua;

                    case 16:                     //StSuspended
                        Status = "StSuspended";
                        return KNEKTColors.Aqua;

                    case 32:                     //StCompleted
                        Status = "StCompleted";
                        return KNEKTColors.Aqua;

                    case 64:                     //StEmptying
                        Status = "StEmptying";
                        return KNEKTColors.Lime;

                    case 128:                     //StRegister
                        Status = "StRegister";
                        return KNEKTColors.Aqua;

                    case 16408:                     
                        Status = "StDosing.MIVEAULOW";
                        return KNEKTColors.Green;

                    case 16398:
                        Status = "StStopped.MIVEAULOW";
                        return KNEKTColors.Gray;


                    case 16910:
                        Status = "StStopped.NIVEAULOW";
                        return KNEKTColors.Gray;

                    default:                    //State Not Included
                        Status = "State Not Included";
                        return KNEKTColors.Gray;
                }
            }
        }

        public string Status_CWA
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_CWA
        {
            get
            {
                return this.Fault;
            }
        }
    }
}
