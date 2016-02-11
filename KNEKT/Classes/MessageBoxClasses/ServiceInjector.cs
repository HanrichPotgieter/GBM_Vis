using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KNEKT.Classes.MessageBoxClasses
{
    public static class ServiceInjector
    {
        // Loads service objects into the ServiceContainer on startup.
        public static void InjectServices()
        {
            ServiceContainer.Instance.AddService<IMessageBoxService>(new MessageBoxService());
        }
    }
}
