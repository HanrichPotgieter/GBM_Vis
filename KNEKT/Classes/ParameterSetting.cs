using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KNEKT.Classes
{
    public class ParameterSetting
    {
        private int _RevisionID;
        public int RevisionID
        {
            get { return _RevisionID; }
            set { _RevisionID = value; }
        }

        private int _ParameterID;
        public int ParameterID
        {
            get { return _ParameterID; }
            set { _ParameterID = value; }
        }

        private string _ParameterName;
        public string ParameterName
        {
            get { return _ParameterName; }
            set { _ParameterName = value; }
        }

        private string _DBOffset;
        public string DBOffset
        {
            get { return _DBOffset; }
            set { _DBOffset = value; }
        }

        private string _Value;
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        private DateTime _DateSaved;
        public DateTime DateSaved
        {
            get { return _DateSaved; }
            set { _DateSaved = value; }
        }

    }
}
