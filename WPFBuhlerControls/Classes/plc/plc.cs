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
                    catch(Exception ex)
                    {
                        Console.Out.WriteLine(ex);
                    }
                   
                }
                return instance;
            }
        }
    }



}

