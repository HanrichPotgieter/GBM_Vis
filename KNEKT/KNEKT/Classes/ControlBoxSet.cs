using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KNEKT
{
                                                                                                /// <summary>
                                                                                                /// This class is used to hold the data retrieved from the lineParameters table. It contains all commands that can be executed on each line and section 
                                                                                                /// </summary>

    class ControlBoxSet
    {
        private int tLineNumber;
        private string lineDB;
        private string tPlay1;
        private string tPlay2;
        private string tPlay3;
        private string tPlay4;
        private string tSuspend1;
        private string tMute1;
        private string tAccept1;
        private string tStop1;
        private string tEStop1;

        private string L_StateCode;
        private string L_S1DB;
        private string L_S1StateCode;
        private string L_S1ParEmptying;
        private string L_S1OutEmptying;

        private string L_S2DB;
        private string L_S2StateCode;
        private string L_S2ParEmptying;
        private string L_S2OutEmptying;

        private string L_S3DB;
        private string L_S3StateCode;
        private string L_S3ParEmptying;
        private string L_S3OutEmptying;

        private string tCmdReset;
        private string tRequestModify;
        private string tRequestDefine;

    
        public ControlBoxSet(int lineNumber, string lineDB, string cmdEStop, string cmdHornOff, string cmdSeqStop, string cmdFaultReset, string cmdFeedOn, string cmdStart, string cmdTransferOn, string cmdReqExecute, string cmdFeedOff, string lineStateCode, string s1_DB, string s1_StateCode, string s1_parEmptyingTime, string s1_outEmptyingTime, string s2_DB, string s2_StateCode, string s2_parEmptyingTime, string s2_outEmptyingTime, string s3_DB, string s3_StateCode, string s3_parEmptyingTime, string s3_outEmptyingTime, string cmdReset, string cmdRequestModify, string cmdRequestDefine)
        {
            LineNumber = lineNumber;
            LineDB = lineDB;
            CmdEStop = cmdEStop;
            CmdMute = cmdHornOff;
            CmdSequenceStop = cmdSeqStop;
            CmdFaultReset = cmdFaultReset;
            CmdFeedOn = cmdFeedOn;
            CmdStart = cmdStart;
            CmdTransferOn = cmdTransferOn;
            CmdRequestExecute = cmdReqExecute;
            CmdFeedOff = cmdFeedOff;
            CmdReset = cmdReset;
            CmdRequestModify = cmdRequestModify;
            CmdRequestDefine = cmdRequestDefine;


            LineStateCode = lineStateCode;
            LineS1DB = s1_DB;
            LineS1StateCode = s1_StateCode;
            LineS1outEmptying = s1_outEmptyingTime;
            LineS1parEmptying = s1_parEmptyingTime;

            LineS2DB = s2_DB;
            LineS2StateCode = s2_StateCode;
            LineS2outEmptying = s2_outEmptyingTime;
            LineS2parEmptying = s2_parEmptyingTime;

            LineS3DB = s3_DB;
            LineS3StateCode = s3_StateCode;
            LineS3outEmptying = s3_outEmptyingTime;
            LineS3parEmptying = s3_parEmptyingTime;
        }

        public int LineNumber
        {
            get
            {
                return tLineNumber;
            }
            set
            {
                tLineNumber = value;
            }
        }

        public string LineDB
        {
            get
            {
                return lineDB;
            }
            set
            {
                lineDB = value;
            }
        }

        public string CmdEStop
        {
            get
            {
                return tEStop1;
            }
            set
            {
                tEStop1 = value;
            }
        }

        public string CmdMute
        {
            get
            {
                return tMute1;
            }
            set
            {
                tMute1 = value;
            }
        }

        public string CmdSequenceStop
        {
            get
            {
                return tStop1;
            }
            set
            {
                tStop1 = value;
            }
        }

        public string CmdFaultReset
        {
            get
            {
                return tAccept1;
            }
            set
            {
                tAccept1 = value;
            }
        }
       
        public string CmdFeedOn
        {
            get
            {
                return tPlay1;
            }
            set
            {
                tPlay1 = value;
            }
        }

        public string CmdStart
        {
            get
            {
                return tPlay2;
            }
            set
            {
                tPlay2 = value;
            }
        }

        public string CmdTransferOn
        {
            get
            {
                return tPlay3;
            }
            set
            {
                tPlay3 = value;
            }
        }

        public string CmdRequestExecute
        {
            get
            {
                return tPlay4;
            }
            set
            {
                tPlay4 = value;
            }
        }

        public string CmdFeedOff
        {
            get
            {
                return tSuspend1;
            }
            set
            {
                tSuspend1 = value;
            }
        }

        public string LineStateCode
        {
            get
            {
                return L_StateCode;
            }
            set
            {
                L_StateCode = value;
            }
        }

        public string LineS1DB
        {
            get
            {
                return L_S1DB;
            }
            set
            {
                L_S1DB = value;
            }
        }

        public string LineS1StateCode
        {
            get
            {
                return L_S1StateCode;
            }
            set
            {
                L_S1StateCode = value;
            }
        }

        public string LineS1parEmptying
        {
            get
            {
                return L_S1ParEmptying;
            }
            set
            {
                L_S1ParEmptying = value;
            }
        }

        public string LineS1outEmptying
        {
            get
            {
                return L_S1OutEmptying;
            }
            set
            {
                L_S1OutEmptying = value;
            }
        }

        public string LineS2DB
        {
            get
            {
                return L_S2DB;
            }
            set
            {
                L_S2DB = value;
            }
        }

        public string LineS2StateCode
        {
            get
            {
                return L_S2StateCode;
            }
            set
            {
                L_S2StateCode = value;
            }
        }

        public string LineS2parEmptying
        {
            get
            {
                return L_S2ParEmptying;
            }
            set
            {
                L_S2ParEmptying = value;
            }
        }

        public string LineS2outEmptying
        {
            get
            {
                return L_S2OutEmptying;
            }
            set
            {
                L_S2OutEmptying = value;
            }
        }

        public string LineS3DB
        {
            get
            {
                return L_S3DB;
            }
            set
            {
                L_S3DB = value;
            }
        }

        public string LineS3StateCode
        {
            get
            {
                return L_S3StateCode;
            }
            set
            {
                L_S3StateCode = value;
            }
        }

        public string LineS3parEmptying
        {
            get
            {
                return L_S3ParEmptying;
            }
            set
            {
                L_S3ParEmptying = value;
            }
        }

        public string LineS3outEmptying
        {
            get
            {
                return L_S3OutEmptying;
            }
            set
            {
                L_S3OutEmptying = value;
            }
        }

        public string CmdReset
        {
            get
            {
                return tCmdReset;
            }
            set
            {
                tCmdReset = value;
            }
        }

        public string CmdRequestModify
        {
            get
            {
                return tRequestModify;
            }
            set
            {
                tRequestModify = value;
            }
        }

        public string CmdRequestDefine
        {
            get
            {
                return tRequestDefine;
            }
            set
            {
                tRequestDefine = value;
            }
        }
    }
}
