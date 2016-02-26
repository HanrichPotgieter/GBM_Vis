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
    public class FB93
    {
        public string Status;
        public string StatusError;
        private bool Fault = false;

        public Color SetColorStatus(int State) //State is a HEXADECIMAL value
        {
           
            if (State < 0 || State > 32767)
            {                     
                Status = "StFault";
                Fault = true;
                return Colors.Red;
            }
            else
            {
                State = State % 256;  
                
                switch (State)
                {
                    case 1:                    
                        Status = "Passive";
                        return Colors.White;

                    case 2:                     
                        Status = "Waiting";
                        return Colors.Orange;

                    case 4:                     
                        Status = "Active";
                        return Colors.Green;

                    case 8:                   
                        Status = "Ready";
                        return Colors.Lime;

                    case 16:                     
                        Status = "Emptying";
                        return Colors.Aqua;

                    case 32:               
                        Status = "Emptied";
                        return Colors.Aqua;

                    case 64:                     
                        Status = "Idling";
                        return Colors.Aqua;
                    default:                  
                        Status = "State Not Included";
                        return Colors.Gray;
                }
            }
        }

        public Color SetColorError(int State) //State is a HEXADECIMAL value
        {

            if (State < 0 || State > 32767)
            {                     //StFault
                StatusError = "StFault";
                Fault = true;
                return Colors.Red;
            }
            else
            {
                State = State % 256;

                switch (State)
                {
                    case 0:
                        StatusError = "No Errors";
                        return Colors.Green;

                    case 1:                    
                        StatusError = "Empty";
                        return Colors.Red;

                    case 2:                     
                        StatusError = "Full";
                        return Colors.Red;

                    case 4:                     
                        StatusError = "Way Conflict";
                        return Colors.Red;

                    case 8:                   
                        StatusError = "Warning";
                        return Colors.Red;

                    case 16:                   
                        StatusError = "Mechanical Error";
                        return Colors.Red;

                    case 32:                   
                        StatusError = "Dosing Error";
                        return Colors.Red;

                    case 64:                  
                        StatusError = "Empty Job";
                        return Colors.Red;
                    default:                   
                        StatusError = "State Not Included";
                        return Colors.Gray;
                }
            }
        }
    }
}
