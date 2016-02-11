using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace KNEKT.Classes
{
    public class Bin : INotifyPropertyChanged
    {
        private int _BinId;
        public int BinId
        {
            get { return _BinId; }
            set { _BinId = value; }
        }

        private string _BinDescription;
        public string BinDescription
        {
            get { return _BinDescription; }
            set
            {
                _BinDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BinDescription"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}
