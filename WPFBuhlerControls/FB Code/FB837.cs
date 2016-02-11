using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WPFBuhlerControls;

namespace WPFBuhlerControls.FB_Code
{
    public class FB837
    {
        private string Status;
        private bool Fault = false;

        public Brush SetColor(int State)
        {          
            switch (State)
            {
                case 1:                     //StDisEngaged
                    Status = "StDisEngaged";
                    return KNEKTColors.Gray;             

                case 3:                     //StEngaged
                    Status = "StEngaged";
                    return KNEKTColors.Green;

                case 515:                    //StStopped.Engaged
                    Status = "StStopped.Engaged";
                    return KNEKTColors.Green;  

                case 11:                     //StDisEngaged.SuspendedLocal
                    Status = "StDisEngaged.SusLocal";
                    return KNEKTColors.Gray;

                case 31:                     //StEngaged.SusLocal
                    Status = "StEngaged.SusLocal";
                    return KNEKTColors.Green;

                case 32:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;

                //4128
                case 4128:                     //StFault
                    Status = "StFault";
                    Fault = true;
                    return KNEKTColors.Red;

                case 513:                    //StDisEngaged
                    Status = "StDisEngaged";
                    return KNEKTColors.Gray;             

                case 16837:                     //StEngaged.Warning
                    Status = "StEngaged.Warning";
                    return KNEKTColors.Yellow;

                case 16897:                //StDisengaged.Warning
                    Status = "StDisengaged.Warning";
                    return KNEKTColors.Yellow;

                case 16387:                //StStopped.Disengaged
                    Status = "StStopped.Disengaged";
                    return KNEKTColors.Yellow;

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
