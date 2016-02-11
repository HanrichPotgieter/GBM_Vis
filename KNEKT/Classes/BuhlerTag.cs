using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S7Link;

namespace KNEKT
{
                                                                                        /// <summary>
                                                                                        /// This class is used in the Data Logger Page to allow the user to manage which tags are saved to the database. [OnRec, OnTick, OnChange]
                                                                                        /// </summary>
    class BuhlerTag
    {
        private string _Tagname;
        private string _TagDesc;
        private int _GcProTag;
        private int _UserTag;
        private int _RecTrend;
        private int _RecChange;
        private int _RecOnTick;
        private int _MeasurementID;


        //------------------------------------------------------------------------------//
        //                                  Constructor                                 //
        //------------------------------------------------------------------------------//   

        public BuhlerTag(string tagname, string tagDescription, int recTrend, int recOnChange, int recOnTick, int measurementID)
        {
            Tagname = tagname;
            TagDescription = tagDescription;
            RecordTrend = recTrend;
            RecordChange = recOnChange;
            RecordOnTick = recOnTick;
            MeasurementID = measurementID;
        }

        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//        


        public string Tagname
        {
            get
            {
                return _Tagname;
            }
            set
            {
                this._Tagname = value;
            }
        }

        public string TagDescription
        {
            get
            {
                return _TagDesc;
            }
            set
            {
                this._TagDesc = value;
            }
        }

        public int GcProTag
        {
            get
            {
                return _GcProTag;
            }
            set
            {
                _GcProTag = value;
            }
        }

        public int UserTag
        {
            get
            {
                return _UserTag;
            }
            set
            {
                _UserTag = value;
            }
        }

        public int RecordTrend
        {
            get
            {
                return _RecTrend;
            }
            set
            {
                _RecTrend = value;
            }
        }

        public int RecordChange
        {
            get
            {
                return _RecChange;
            }
            set
            {
                _RecChange = value;
            }
        }

        public int RecordOnTick
        {
            get
            {
                return _RecOnTick;
            }
            set
            {
                _RecOnTick = value;
            }
        }

        public int MeasurementID
        {
            get
            {
                return _MeasurementID;
            }
            set
            {
                _MeasurementID = value;
            }
        }

    }
}
