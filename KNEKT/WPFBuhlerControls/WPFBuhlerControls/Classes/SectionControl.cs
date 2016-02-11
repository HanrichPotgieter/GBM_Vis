using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace WPFBuhlerControls
{
    public class SectionControl
    {
        private string Status;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SectionState">Current value of the Section Tag</param>
        /// <param name="State">The value of either the section state[Which has a "0" added to the end] or the value of the sectionFault</param>
        /// <returns></returns>
        public LinearGradientBrush SetColor(int SectionState, int State)
        {
            Color BottomGradientColor = Colors.Black;  

            //If there is no fault (0) in the DBD, then make the state of the section = the section state passed in
            if (State == 0)
            {
                State = SectionState;
            }

            switch (State)
            {
                case 0:                     //StPassive [Blue passive]
                    Status = "StPassive [Blue]";
                    BottomGradientColor = Colors.White;
                    break;

                case 10:                     //StPassive
                    Status = "StPassive";
                    BottomGradientColor = Colors.White;
                    break;

                case 20:                     //StWaiting
                    Status = "StWaiting";
                    BottomGradientColor = Colors.Fuchsia;
                    break;

                case 40:                     //StActive
                    Status = "StActive";
                    BottomGradientColor = Colors.Green;
                    break;

                case 80:                     //StReady
                    Status = "StReady";
                    BottomGradientColor = Colors.Lime;
                    break;

                case 160:                     //StEmptying
                    Status = "StEmptying";
                    BottomGradientColor = Colors.Aqua;
                    break;

                case 320:                     //StEmptied
                    Status = "StEmptied";
                    BottomGradientColor = Colors.Yellow;
                    break;

                case 640:                     //StIdling
                    Status = "StIdling";
                    BottomGradientColor = Colors.LightGreen;
                    break;

                //case 2:                     //ErrFull
                //    Status = "ErrFull";
                //    BottomGradientColor = Colors.Red;
                //    break;

                //case 16:                     //ErrMech
                //    Status = "StIdling";
                //    BottomGradientColor = Colors.Red;
                //    break;

                case 32:                     //Fault
                    Status = "Fault";
                    BottomGradientColor = Colors.Red;
                    break;

                default:                    //State Not Included
                    Status = "State Not Included";
                    BottomGradientColor = Colors.Black;
                    break;
            }

            LinearGradientBrush lgb = new LinearGradientBrush();
            lgb.StartPoint = new Point(0, 0);
            lgb.EndPoint = new Point(0, 10);
            lgb.GradientStops.Add(new GradientStop(Colors.White, 0.0));
            lgb.GradientStops.Add(new GradientStop(BottomGradientColor, 0.1));

            return lgb;
        }

        //------------------------------------------------------------------------------//
        //                              Line Properties                                 //
        //------------------------------------------------------------------------------//

        public string Status_Section
        {
            get
            {
                return this.Status;
            }
        }
    }
}
