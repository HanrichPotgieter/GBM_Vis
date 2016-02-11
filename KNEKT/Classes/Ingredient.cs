using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KNEKT.Classes
{
    public class Ingredient : INotifyPropertyChanged
    {
        public Ingredient()
        {
            ErrorIngredientWeight = System.Windows.Media.Brushes.Transparent;
        }


        private int _IngredientID;
        public int IngredientID
        {
            get { return _IngredientID; }
            set
            {
                if (_IngredientID != value)
                {
                    _IngredientID = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IngredientID"));
                }
            }
        }

        private string _IngredientName;
        public string IngredientName
        {
            get { return _IngredientName; }
            set
            {
                if (_IngredientName != value)
                {
                    _IngredientName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IngredientName"));
                }
            }
        }

        private double _IngredientPercentageValue;
        public double IngredientPercentageValue
        {
            get { return _IngredientPercentageValue; }
            set
            {
                if (_IngredientPercentageValue != value)
                {
                    _IngredientPercentageValue = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IngredientPercentageValue"));
                    CalculatePercentageFromBatchWeight();

                    IngredientsViewModel.IngredientTotalChanged = value;
                    IngredientsViewModel.IngredientTotalPercentageChanged = value;
                }
            }
        }

        private double _IngredientKilogramValue;
        public double IngredientKilogramValue
        {
            get { return _IngredientKilogramValue; }
            set
            {
                if (_IngredientKilogramValue != value)
                {
                    _IngredientKilogramValue = value;

                    if (!(this.MinimumValue == 0 & this.MaximumValue == 0)) //No Limits set
                    {
                        if (value < this.MinimumValue || value > this.MaximumValue)
                            ErrorIngredientWeight = System.Windows.Media.Brushes.Red;
                        else
                            ErrorIngredientWeight = System.Windows.Media.Brushes.Transparent;
                    }

                    OnPropertyChanged(new PropertyChangedEventArgs("IngredientKilogramValue"));
                }
            }
        }

        private int _IngredientBinNumber;
        public int IngredientBinNumber
        {
            get { return _IngredientBinNumber; }
            set
            {
                if (_IngredientBinNumber != value)
                {
                    _IngredientBinNumber = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IngredientBinNumber"));
                }
            }
        }

        private int _IngredientType;
        public int IngredientType
        {
            get { return _IngredientType; }
            set
            {
                if (_IngredientType != value)
                {
                    _IngredientType = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IngredientType"));
                }
            }
        }

        private int _IngredientScaleNumber;
        public int IngredientScaleNumber
        {
            get { return _IngredientScaleNumber; }
            set
            {
                if (_IngredientScaleNumber != value)
                {
                    _IngredientScaleNumber = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IngredientScaleNumber"));
                }
            }
        }

        private System.Windows.Media.Brush _ErrorIngredientWeight;
        public System.Windows.Media.Brush ErrorIngredientWeight
        {
            get { return _ErrorIngredientWeight; }
            set
            {
                _ErrorIngredientWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ErrorIngredientWeight"));
            }
        }


        private double _MinimumValue;
        public double MinimumValue
        {
            get { return _MinimumValue; }
            set
            {
                if (_MinimumValue != value)
                {
                    _MinimumValue = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("MinimumValue"));
                }
            }
        }

        private double _MaximumValue;
        public double MaximumValue
        {
            get { return _MaximumValue; }
            set
            {
                if (_MinimumValue != value)
                {
                    _MaximumValue = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("MaximumValue"));
                }
            }
        }



        public void CalculatePercentageFromBatchWeight()
        {
            IngredientKilogramValue = IngredientPercentageValue * Recipe._stat_RecipeBatchWeight / 100;
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
