using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_Motor [Motor]
    /// </summary>
    public class FB12
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
                
                case 3:                     //StStartedFwdSlow
                    Status = "StStartedFwdSlow";
                    return KNEKTColors.Green;

                case 4:                     //StStartedFwdFast
                    Status = "StStartedFwdFast";
                    return KNEKTColors.Green;

                case 5:                     //StStartedRevSlow
                    Status = "StStartedRevSlow";
                    return KNEKTColors.Green;
                    
                case 6:                     //StStartedRevFast
                    Status = "StStartedRevFast";
                    return KNEKTColors.Green;
					
				case 7:                     //StStopping
                    Status = "StStopping";
                    return KNEKTColors.Aqua;
					
				case 9:                     //StStartRequest
                    Status = "StStartRequest";
                    return KNEKTColors.Gray;
					
				case 11:                     //StStartingDelay
                    Status = "StStartingDelay";
                    return KNEKTColors.Gray;
					
				case 31:                     //StFault.FaultDev
                    Status = "StFault.FaultDev";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 33:                     //StFault.Isolated
                    Status = "StFault.Isolated";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 34:                     //StFault.Overload
                    Status = "StFault.Overload";
                    Fault = true;
                    return KNEKTColors.Red;
					
				case 35:                     //StFault.Service
                    Status = "StFault.Service";
                    Fault = true;
                    return KNEKTColors.Red;

                case 513:                     //StStoppedPassive
                    Status = "StStoppedPassive";
                    return KNEKTColors.Gray;

                case 514:                     //St-----
                    Status = "St-----";
                    return KNEKTColors.Yellow;

                case 515:                     //St-----
                    Status = "St-----";
                    return KNEKTColors.Gray;

                case 516:                     //StStoppingManual
                    Status = "StStStoppingManual";
                    return KNEKTColors.Aqua;

                case 258:                     //StStartingManual
                    Status = "StStartingManual";
                    return KNEKTColors.Lime;

                case 259:                     //StStartedManual
                    Status = "StStartedManual";
                    return KNEKTColors.Green;

                case 265:                     //StStopepdManual
                    Status = "StStopepdManual";
                    return KNEKTColors.Gray;

                case 267:                     //StStartWarningManual
                    Status = "StStartWarningManual";
                    return KNEKTColors.Gray;

                case 288:                     //StFaultManual
                    Status = "StFaultManual";
                    Fault = true;
                    return KNEKTColors.Red;

                case 1537:                     //StStoppedManualForce
                    Status = "StStoppedManualForce";
                    return KNEKTColors.Gray;

                case 2049:                     //StStoppedHardware
                    Status = "StStStoppedHardware";
                    return KNEKTColors.Red;

                case 8223:                     //StFaultDevice
                    Status = "StFaultDevice";
                    return KNEKTColors.FaultDevice;

                case 16387:                     //StStartedWarning
                    Status = "StStartedWarning";
                    return KNEKTColors.Green;

                case 24:                     
                    Status = "StDosing.NIVEAULOW";
                    return KNEKTColors.Green;

                case 14:
                    Status = "StStopped.NIVEAULOW";
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


