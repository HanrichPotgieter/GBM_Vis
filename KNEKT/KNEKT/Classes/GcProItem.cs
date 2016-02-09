using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KNEKT
{
                                                                                        /// <summary>
                                                                                        /// Used to store data retreived from the GcPro.mdb database when migrating the data to SQL
                                                                                        /// </summary>
    class GcProItem
    {
        private string _ObjectNo;
        private string _Tagname;
        private string _TagDesc;
        private string _GroupNo;
        private string _Address;
        private string _PType;
        private string _FB;

        public GcProItem(string ObjectNumber, string Tagname, string TagDescription, string GroupNumber, string Address, string ParMsgType, string FB)
        {
            pObjectNo = ObjectNumber;
            pTagname = Tagname;
            pTagDesc = TagDescription;
            pGroupNo = GroupNumber;
            pAddress = Address;
            pPType = ParMsgType;
            pFB = FB;
        }

        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//        
        public string pObjectNo
        {
            get
            {
                return _ObjectNo;
            }
            set
            {
                this._ObjectNo = value;
            }
        }

        public string pTagname
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

        public string pTagDesc
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

        public string pGroupNo
        {
            get
            {
                return _GroupNo;
            }
            set
            {
                this._GroupNo = value;
            }
        }

        public string pAddress
        {
            get
            {
                return _Address;
            }
            set
            {
                this._Address = value;
            }
        }

        public string pPType
        {
            get
            {
                return _PType;
            }
            set
            {
                this._PType = value;
            }
        }

        public string pFB
        {
            get
            {
                return _FB;
            }
            set
            {
                this._FB = value;
            }
        }
    }
}
