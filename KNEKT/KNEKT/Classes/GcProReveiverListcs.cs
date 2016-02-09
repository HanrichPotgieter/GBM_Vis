using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KNEKT.Classes
{
    public class GcProReceiverData
    {
        public GcProReceiverData()
        {

        }

        private int _SlotNumber;
        public int SlotNumber
        {
            get { return _SlotNumber; }
            set { _SlotNumber = value; }
        }

        private string _ReceiverDBOffset;
        public string ReceiverDBOffset
        {
            get { return _ReceiverDBOffset; }
            set { _ReceiverDBOffset = value; }
        }
    }
}
