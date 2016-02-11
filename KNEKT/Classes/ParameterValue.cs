using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KNEKT.Classes
{
    public class ParameterValue
    {
        private string _Value;
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        private string _DBOffset;
        public string DBOffset
        {
            get { return _DBOffset; }
            set { _DBOffset = value; }
        }

        private S7Link.Tag.ATOMIC _TagDataType;
        public S7Link.Tag.ATOMIC TagDataType
        {
            get { return _TagDataType; }
            set { _TagDataType = value; }
        }
    }
}
