using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KNEKT.Classes.plc
{
    public sealed class Plc
    {
        private static readonly Lazy<Plc> lazy =
        new Lazy<Plc>(() => new Plc());
        public static Plc Instance { get { return lazy.Value; } }
        private Plc()
        {
        }

    }
}
