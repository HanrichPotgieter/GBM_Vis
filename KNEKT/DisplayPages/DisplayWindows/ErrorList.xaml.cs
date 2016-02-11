using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.IO;

namespace KNEKT.DisplayPages.DisplayWindows
{
    /// <summary>
    /// Interaction logic for ErrorList.xaml
    /// </summary>
    public partial class ErrorList : Window
    {
        public ObservableCollection<Classes.Error> _ErrorCollection;

        public ErrorList(ObservableCollection<Classes.Error> err)
        {
            InitializeComponent();
            _ErrorCollection = err;
            this.DataContext = this;
        }

        public ObservableCollection<Classes.Error> ErrorCollection
        {
            get { return _ErrorCollection; }
        }


        //---------------------------------------------------------------------------------//
        //                                 Button Clicks                                   //
        //---------------------------------------------------------------------------------//
        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (Classes.Error err in _ErrorCollection)
                {
                    sb.Append(err.ErrorTag + "," + err.ErrorSource + "," + err.ErrorString + "\n");
                }

                Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
                sfd.FileName = "ErrorList";
                sfd.DefaultExt = ".csv";
                sfd.Filter = "Comma Separated Values (.csv)|*.csv";

                if (sfd.ShowDialog() == true)
                {
                    File.WriteAllText(sfd.FileName, sb.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not export error list.\n\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    } 
}
