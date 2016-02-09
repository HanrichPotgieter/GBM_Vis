using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KNEKT.Classes
{
    class MyObjectInfo
    {
        //------------------------------------------------------------------------------//
        //                                 Constructor                                  //
        //------------------------------------------------------------------------------//

        //Additional Elements
        public MyObjectInfo(string tagName, string dbOffset, int recOnTick, int recOnChange, int recTrend)
        {
            TagName = tagName;
            DBOffset = dbOffset;
            RecOnTick = recOnTick;
            RecOnChange = recOnChange;
            RecTrend = recTrend;
        }

        //GcPro Tags
        public MyObjectInfo(string objectNo, string tagName, string tagDescription, string dbOffset, string parMsgType, string fb, int recOnTick, int recOnChange, int recTrend, string linkedControlList)
        {
            ObjectNo = objectNo;
            TagName = tagName;
            TagDescription = tagDescription;
            DBOffset = dbOffset;
            ParMsgType = parMsgType;
            FB = fb;
            RecOnTick = recOnTick;
            RecOnChange = recOnChange;
            RecTrend = recTrend;
            LinkedControlList = linkedControlList;
        }

        //GcPro Tags - Added Group number for update element color
        public MyObjectInfo(string objectNo, string tagName, string tagDescription, string dbOffset, string parMsgType, string fb, int recOnTick, int recOnChange, int recTrend, string linkedControlList, string groupNumber)
        {
            ObjectNo = objectNo;
            TagName = tagName;
            TagDescription = tagDescription;
            DBOffset = dbOffset;
            ParMsgType = parMsgType;
            FB = fb;
            RecOnTick = recOnTick;
            RecOnChange = recOnChange;
            RecTrend = recTrend;
            LinkedControlList = linkedControlList;
            GroupNumber = groupNumber;
        }

        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
        //------------------------------------------------------------------------------//

        public string ObjectNo
        {
            get;
            set;
        }

        public string TagName
        {
            get;
            set;
        }

        public string TagDescription
        {
            get;
            set;
        }

        public string DBOffset
        {
            get;
            set;
        }

        public string ParMsgType
        {
            get;
            set;
        }

        public string FB
        {
            get;
            set;
        }

        public string GroupNumber
        {
            get;
            set;
        }

        public string ControlList
        {
            get;
            set;
        }

        public int RecTrend
        {
            get;
            set;
        }

        public int RecOnChange
        {
            get;
            set;
        }

        public int RecOnTick
        {
            get;
            set;
        }

        public string LinkedControlList
        {
            get;
            set;
        }
    }
 }
