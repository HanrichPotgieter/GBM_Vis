using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KNEKT.Classes
{
                                                        /// <summary>
                                                        /// Used for Component arts charting, primarily for trending wih component art .dll
                                                        /// </summary>
    public class ChartData
    {
        public DateTime X { get; set; }
        public double Y { get; set; }

        public string XName { get; set; }
    }
}
