using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KNEKT.Classes
{
                                                                /// <summary>
                                                                /// This class is used when performing KPIs on the given objects. It holds the start time that the object entered into a new state, as well as the end time of when the object exited the state.
                                                                /// The Calculate time difference is used to return the total amount of time that the object was in the given state
                                                                /// </summary>
    class ElementUptime
    {
        private DateTime _dtStartPeriodTime;
        private DateTime _dtEndPeriodTime;

        public ElementUptime()
        {
            _dtStartPeriodTime = new DateTime(2000, 1, 1);
            _dtEndPeriodTime = new DateTime(2000, 1, 1);
        }

        public DateTime StartPeriodTime
        {
            get
            {
                return _dtStartPeriodTime;
            }
            set
            {
                _dtStartPeriodTime = value;
            }
        }

        public DateTime EndPeriodTime
        {
            get
            {
                return _dtEndPeriodTime;
            }
            set
            {
                _dtEndPeriodTime = value;
            }
        }

                                                                /// <summary>
                                                                /// Calculates the difference between the Endtime and starttime
                                                                /// </summary>
                                                                /// <returns>Number of seconds</returns>
        public double CalculateTimeDifference()
        {
            double numberOfSeconds = 0;

            if (StartPeriodTime.Year == 2000 && StartPeriodTime.Month == 1 && StartPeriodTime.Day == 1)
            {
                numberOfSeconds = 0;
            }
            else
            {
                TimeSpan ts = EndPeriodTime - StartPeriodTime;

                numberOfSeconds = ts.TotalSeconds;
            }

            return numberOfSeconds;
        }
    }
}
