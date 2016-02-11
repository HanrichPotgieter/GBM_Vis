using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snap7;


public sealed class Plc
{
    private static S7Client instance = null;
    private static readonly object padlock = new object();

    Plc()
    {
    }

    public static S7Client Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new S7Client();
                }
                return instance;
            }
        }
    }



}

