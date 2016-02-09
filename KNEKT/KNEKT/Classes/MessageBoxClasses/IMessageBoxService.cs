using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace KNEKT.Classes.MessageBoxClasses
{
    public interface IMessageBoxService
    {
        MessageBoxResult Show(string message, string title, MessageBoxButton buttons, MessageBoxImage image);
    }
}
