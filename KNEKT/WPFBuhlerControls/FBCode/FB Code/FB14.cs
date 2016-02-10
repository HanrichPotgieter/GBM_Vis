using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{  
    /// <summary>
    /// SSW_EL_DI [Digital Input]
    /// </summary>
    public class FB14
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State, int PType)
        {

            if (PType == 7146)                  //[BIN LEVEL HIGH]
            {
                switch (State)
                {
                    case 513:                    //StUncovered 
                        Status = "StUncovered";
                        return KNEKTColors.White;

                    case 514:                    //StCtoCovered 
                        Status = "StCtoCovered";
                        return KNEKTColors.Lime;

                    case 515:                    //StCovered 
                        Status = "StCovered";
                        return KNEKTColors.Fuschia;

                    case 516:                    //StCtoUnCovered 
                        Status = "StCtoUnCovered";
                        return KNEKTColors.Lime;
                    
                    default:                    //State Not Included
                        Status = "State Not Included";
                        return KNEKTColors.Black;                                      
                }
            }

            if (PType == 0)                  //[PROFIBUS DEVICE (FLAKER ROLL ENGAGE)]
            {
                switch (State)
                {
                    case 513:                    //StStopped
                        Status = "StStopped";
                        return KNEKTColors.Gray;

                    case 514:                    //StStarting
                        Status = "StStarting";
                        return KNEKTColors.Lime;

                    case 515:                    //StStarted
                        Status = "StStarted";
                        return KNEKTColors.Green;

                    default:                    //State Not Included
                        Status = "State Not Included";
                        return KNEKTColors.Black;
                }
            }

            if (PType == 7135)                  //[DIG INP CONT SPEED MONITOR]
            {
                switch (State)
                {
                    case 1:                     //StFalse
                        Status = "StFalse";
                        return KNEKTColors.Gray;

                    case 3:                     //StTrue
                        Status = "StTrue";
                        return KNEKTColors.Green;

                   
                    case 32:                     //StFault
                        Status = "StFault";
                        Fault = true;
                        return KNEKTColors.Red;

                    case 513:                     //StFalsePassive
                        Status = "StFalse";
                        return KNEKTColors.Gray;

                    case 515:                    //StTrue
                        Status = "StTrue";
                        return KNEKTColors.Green;


                    default:                    //State Not Included
                        Status = "State Not Included";
                        return KNEKTColors.Black;
                }
            } 

            if (PType == 7147)                  //[DIG INP CONT PTYPE-ON/OFF]
            {
                switch (State)
                {
                    case 1:                     //StFalse
                        Status = "StFalse";
                        return KNEKTColors.Gray;

                    case 2:                     //StCtoTrue
                        Status = "StCtoTrue";
                        return KNEKTColors.Lime;

                    case 3:                     //StTrue
                        Status = "StTrue";
                        return KNEKTColors.Green;

                    case 4:                     //StCtoFalse
                        Status = "StCtoFalse";
                        return KNEKTColors.Lime;

                    case 11:                     //StStartwarning
                        Status = "StStartwarning";
                        return KNEKTColors.Gray;

                    case 257:                     //StStoppedManual
                        Status = "StStoppedManual";
                        return KNEKTColors.Gray;

                    case 513:                     //stfalsepassive
                        Status = "stFalsePassive";
                        return KNEKTColors.Gray;

                    case 514:                     //StCtoTruePassive
                        Status = "StCtoTruePassive";
                        return KNEKTColors.Lime;

                    case 515:                    //StTruePassive
                        Status = "StTruePassive";
                        return KNEKTColors.Green;

                    case 516:                     //StCtoFalsePassive
                        Status = "StCtoFalsePassive";
                        return KNEKTColors.Lime;

                    case 32:                     //StFault
                        Status = "StFault";
                        Fault = true;
                        return KNEKTColors.Red;

                    case 16387:                     //StStartedWarning
                        Status = "StStartedWarning";
                        Fault = true;
                        return KNEKTColors.Red;

                    default:                    //State Not Included
                        Status = "State Not Included";
                        return KNEKTColors.Black;
                }
            }    

            else if (PType == 7154)             //[GENERAL ELEMENT]
            {
                switch (State)
                {
                    case 1:                     //StFalse 
                        Status = "StFalse";
                        Fault = true;
                        return KNEKTColors.Red;

                    case 2:                     //StCtoTrue
                        Status = "StCtoTrue";
                        return KNEKTColors.Lime;

                    case 3:                     //StTrue
                        Status = "StTrue";
                        return KNEKTColors.Green;

                    case 4:                     //StCtoFalse
                        Status = "StCtoFalse";
                        return KNEKTColors.Lime;

                    case 513:                     //StFalsePassive
                        Status = "StFalsePassive";
                        return KNEKTColors.Gray;

                    case 515:                    //StTrue
                        Status = "StTruePassive";
                        return KNEKTColors.Green;

                    case 516:                    //StCToTruePassive
                        Status = "StCToTruePassive";
                        return KNEKTColors.Lime;

                    case 32:                     //StFault
                        Status = "StFault";
                        Fault = true;
                        return KNEKTColors.Red;

                    default:                    //State Not Included
                        Status = "State Not Included";
                        return KNEKTColors.Black;
                }
            }

            else if (PType == 7165)             //[MACHINE HIGH LEVEL]
            {
                switch (State)
                {
                    case 1:                     //StFalse 
                        Status = "StUncovered";
                        return KNEKTColors.White;

                    case 2:                     //StCtoTrue
                        Status = "StCtoCovered";
                        return KNEKTColors.Lime;

                    case 3:                     //StTrue
                        Status = "StCovered";
                        return KNEKTColors.Fuschia;

                    case 4:                     //StCtoFalse
                        Status = "StCtoUncovered";
                        return KNEKTColors.Lime;

                    case 513:                     //StFalsePassive
                        Status = "StUncovered";
                        return KNEKTColors.White;

                    case 514:                    //StCtoTrue
                        Status = "StCtoCovered";
                        return KNEKTColors.Lime;

                    case 515:                    //StCovered 
                        Status = "StCovered";
                        return KNEKTColors.Fuschia;

                    case 516:                    //StCtoFalse
                        Status = "StCtoUncovered";
                        return KNEKTColors.Lime;

                    case 32:                     //StFault
                        Status = "StFault";
                        Fault = true;
                        return KNEKTColors.Red;
                    
                    default:                    //State Not Included
                        Status = "State Not Included";
                        return KNEKTColors.Black;    
                }
            }

            else if (PType == 7170)                  //[BIN LEVEL LOW]
            {
                switch (State)
                {
                    case 1:                     //StFalse 
                        Status = "StUncovered";
                        return KNEKTColors.White;

                    case 2:                     //StCtoTrue
                        Status = "StCtoCovered";
                        return KNEKTColors.Lime;

                    case 3:                     //StTrue
                        Status = "StCovered";
                        return KNEKTColors.Fuschia;

                    case 4:                     //StCtoFalse
                        Status = "StCtoUnCovered";
                        return KNEKTColors.Lime;                    

                    case 32:                     //StFault
                        Status = "StFault";
                        Fault = true;
                        return KNEKTColors.Red;

                    case 513:                     //StFalsePassive
                        Status = "StFalsePassive";
                        return KNEKTColors.White;

                    case 514:                     //StCtoCovered
                        Status = "StCtoCovered";
                        return KNEKTColors.Lime;

                    case 515:                    //StCovered 
                        Status = "StCovered";
                        return KNEKTColors.Fuschia;

                    case 516:                     //StCtoUncovered
                        Status = "StCtoUncovered";
                        return KNEKTColors.Lime;

                    default:                    //State Not Included
                        Status = "State Not Included";
                        return KNEKTColors.Black;    
                }
            }

            //Default
            Status = "Default";
            return KNEKTColors.Black;
        }

        public string Status_DI_Status
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_DI_Element
        {
            get
            {
                return this.Fault;
            }
        }

    }
}
        

