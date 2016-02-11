using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KNEKT.Classes.MicroIngredients
{
    public class MicroIngredientModel : INotifyPropertyChanged
    {

        private int _ComponentID;
        public int ComponentID
        {
            get { return _ComponentID; }
            set
            {
                _ComponentID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ComponentID"));
            }
        }

        private string _ComponentName;
        public string ComponentName
        {
            get { return _ComponentName; }
            set
            {
                _ComponentName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ComponentName"));
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
