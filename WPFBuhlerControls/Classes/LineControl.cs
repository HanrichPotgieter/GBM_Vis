using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;

namespace WPFBuhlerControls
{
    public class LineControl
    {
        private string Status;

        public LinearGradientBrush SetColor(int State)
        {
            Color BottomGradientColor;            

            switch (State)
            {
                case 1:                     //StPassive
                    Status = "StPassive";
                    BottomGradientColor = Colors.LightGray;
                    break;

                case 2:                     //Spare
                    Status = "Spare";
                    BottomGradientColor = Colors.Black;
                    break;

                case 4:                     //StActive
                    Status = "StActive";
                    BottomGradientColor = Colors.Green;
                    break;

                case 8:                     //StReady
                    Status = "StReady";
                    BottomGradientColor = Colors.Lime;
                    break;

                case 16:                     //Spare
                    Status = "Spare";
                    BottomGradientColor = Colors.Black;
                    break;

                case 32:                     //Spare
                    Status = "Spare";
                    BottomGradientColor = Colors.Black;
                    break;

                case 64:                     //StIdling
                    Status = "StIdling";
                    BottomGradientColor = Colors.LightGreen;
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

        public string Status_Line
        {
            get
            {
                return this.Status;
            }
        }
    }
}
