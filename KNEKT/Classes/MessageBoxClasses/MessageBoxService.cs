using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace KNEKT.Classes.MessageBoxClasses
{
    /// <summary>
    /// A service that shows message boxes.
    /// </summary>
    internal class MessageBoxService : IMessageBoxService
    {
        MessageBoxResult IMessageBoxService.Show(
            string text,
            string caption,
            MessageBoxButton buttons,
            MessageBoxImage image)
        {
            return MessageBox.Show(text, caption, buttons, image);
        }
    }
}
