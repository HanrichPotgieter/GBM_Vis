﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snap7;


    public sealed class Plc
    {
        private static S7Client instance = null;
        private static readonly object padlock = new object();
        private static readonly object padlockDBRead = new object();

        Plc()
        {
        }

        public bool allowCreate = false;

        public static S7Client Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        try
                        {
                            instance = new S7Client();
                        }
                        catch (Exception ex)
                        {
                            Console.Out.WriteLine(ex);
                        }

                    }
                    return instance;
                }
            }
        }

        public void connect(String IP, int Rack, int Slot)
        {
            Plc.instance.ConnectTo(IP, Rack, Slot);
        }
        // Enables multiple threads to read from the plc at the smae time.
        public static int DBRead(int DB, int Offset, int length, byte[] Buffer)
        {
            if (instance != null)
            {
                lock (padlockDBRead)
                {
                    return instance.DBRead(DB, Offset, length, Buffer);
                }
            }
            else
            {
                return 0;
            }

        }
        //Enables multiple threads to write to the driver at the same time.
        public static int WriteArea(int S7area, int dbnumber, int offset,int length,int S7type , byte[] Buffer)
        {
            if (instance != null)
            {
                lock (padlockDBRead)
                {
                    return instance.WriteArea(S7area, dbnumber, offset, length, S7type, Buffer);
                }
            }
            else
            {
                return 0;
            }

        }
}


