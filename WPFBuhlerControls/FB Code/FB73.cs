using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WPFBuhlerControls.FB_Code
{
    //
    // Pellet Mill
    //
    public class FB73
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

                case 2:                     //StStarted
                    Status = "StStarted";
                    return KNEKTColors.Green;

                default:                    //State Not Included
                    Status = "State Not Included";
                    return KNEKTColors.Black;

            }
        }

        public string Status_PelletMill
        {
            get
            {
                return this.Status;
            }
        }

        public bool Fault_PelletMill
        {
            get
            {
                return this.Fault;
            }
        }
    }
}
