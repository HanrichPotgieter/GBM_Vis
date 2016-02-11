using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KNEKT
{
                                                                                        /// <summary>
                                                                                        /// Used to store data that should be written to SQL perioicallyin an arraylist 
                                                                                        /// </summary>
    class LogItem
    {
        private DateTime tsDate;
        private double oaDate;
        private string objectName;
        private string objectDescription;
        private int code;

        private object _value;

        //------------------------------------------------------------------------------//
        //                                Constructor                                   //
        //------------------------------------------------------------------------------//

        public LogItem(DateTime EventDateTime, double OADateTime, string Object, string Action, int code)
        {
            ts_DateTime = EventDateTime;
            OADate = OADateTime;
            ObjectName = Object;
            ObjectAction = Action;
            Code = code;
        }

        public LogItem(DateTime EventDateTime, double OADateTime, string Tagname, object value)
        {
            ts_DateTime = EventDateTime;
            OADate = OADateTime;
            ObjectName = Tagname;
            Value = value;
        }


        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//
        public DateTime ts_DateTime
        {
            get
            {
                return this.tsDate;
            }
            set
            {
                this.tsDate = value;
            }
        }

        public double OADate
        {
            get
            {
                return this.oaDate;
            }
            set
            {
                oaDate = value;
            }
        }

        public string ObjectName
        {
            get
            {
                return this.objectName;
            }
            set
            {
                objectName = value;
            }
        }

        public string ObjectAction
        {
            get
            {
                return this.objectDescription;
            }
            set
            {
                objectDescription = value;
            }
        }

        public int Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }

        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }
}
