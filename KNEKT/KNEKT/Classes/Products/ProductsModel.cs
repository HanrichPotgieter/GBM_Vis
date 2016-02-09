using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KNEKT.Classes.Products
{
    public class ProductsModel : INotifyPropertyChanged
    {
        private int _ProductID;
        public int ProductID
        {
            get { return _ProductID; }
            set
            {
                _ProductID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductID"));
            }
        }

        private string _ProductName;
        public string ProductName
        {
            get { return _ProductName; }
            set
            {
                _ProductName = value;

                OnPropertyChanged(new PropertyChangedEventArgs("ProductName"));
            }
        }

        private string _ProductAbbreviation;
        public string ProductAbbreviation
        {
            get { return _ProductAbbreviation; }
            set
            {
                _ProductAbbreviation = value;
                //ProductName = ProductName + " - " + _ProductAbbreviation;
                //_ProductName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductAbbreviation"));

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
