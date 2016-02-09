using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KNEKT.Classes;
using System.Windows.Data;

namespace KNEKT
{
    public class Recipe : INotifyPropertyChanged
    {
        public ObservableCollection<Ingredient> ocIngredients = new ObservableCollection<Ingredient>();
        public ObservableCollection<Ingredient> ocAvailableIngredients = new ObservableCollection<Ingredient>();

        public DelegateCommand cmdUseIngredient;
        public DelegateCommand cmdRemoveIngredient;
        public DelegateCommand cmdMoveIngredientUp;
        public DelegateCommand cmdMoveIngredientDown;

        public Recipe()
        {
            cmdUseIngredient = new DelegateCommand(cmdUseIngredientImplementation);
            cmdRemoveIngredient = new DelegateCommand(cmdRemoveIngredientImplementation);
            cmdMoveIngredientUp = new DelegateCommand(cmdMoveIngredientUpImplementation);
            cmdMoveIngredientDown = new DelegateCommand(cmdMoveIngredientDownImplementation);

            //Group the list by ScaleNumber
            var cvs = CollectionViewSource.GetDefaultView(ocIngredients);
            cvs.GroupDescriptions.Add(new PropertyGroupDescription("IngredientScaleNumber"));
        }

        //----------------------------------------------------------------------//
        //                             Properties                               //
        //----------------------------------------------------------------------//

        private int _RecipeId;
        public int RecipeId
        {
            get { return _RecipeId; }
            set { _RecipeId = value; }
        }

        private string _RecipeName;
        public string RecipeName
        {
            get { return _RecipeName; }
            set
            {
                _RecipeName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RecipeName"));
            }
        }

        private string _ProcessLineName;
        public string ProcessLineName
        {
            get { return _ProcessLineName; }
            set
            {
                _ProcessLineName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProcessLineName"));
            }
        }

        public ObservableCollection<Ingredient> UsedRecipeIngredients
        {
            get { return ocIngredients; }
            set
            {
                ocIngredients = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UsedRecipeIngredients"));
            }
        }

        public ObservableCollection<Ingredient> AvailableRecipeIngredients
        {
            get { return ocAvailableIngredients; }
            set
            {
                ocAvailableIngredients = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AvailableRecipeIngredients"));
            }
        }

        private Ingredient _SelectedAvailableIngredient;
        public Ingredient SelectedAvailableIngredient
        {
            get { return _SelectedAvailableIngredient; }
            set
            {
                _SelectedAvailableIngredient = value;
            }
        }

        private Ingredient _SelectedUsedIngredient;
        public Ingredient SelectedUsedIngredient
        {
            get { return _SelectedUsedIngredient; }
            set
            {
                _SelectedUsedIngredient = value;
            }
        }

        public static double _stat_RecipeBatchWeight;
        private double _RecipeBatchWeight;
        public double RecipeBatchWeight
        {
            get { return _RecipeBatchWeight; }
            set
            {
                _RecipeBatchWeight = value;
                _stat_RecipeBatchWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RecipeBatchWeight"));
                foreach (Ingredient i in this.UsedRecipeIngredients)
                {
                    i.CalculatePercentageFromBatchWeight();
                }
            }
        }


        private double _TotalWeightOfIngredients;
        public double TotalWeightOfIngredients
        {
            get { return _TotalWeightOfIngredients; }
            set
            {
                _TotalWeightOfIngredients = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalWeightOfIngredients"));

                if (_TotalWeightOfIngredients > IngredientsViewModel.MixerCapacity)
                    MixerCapacityBackGround = System.Windows.Visibility.Visible;
                else
                    MixerCapacityBackGround = System.Windows.Visibility.Hidden;
            }
        }

        private double _TotalPercentageOfIngredients;
        public double TotalPercentageOfIngredients
        {
            get { return _TotalPercentageOfIngredients; }
            set
            {
                _TotalPercentageOfIngredients = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalPercentageOfIngredients"));

                if (_TotalPercentageOfIngredients == 100)
                    TotalPercentageBackGround = System.Windows.Visibility.Hidden; //System.Windows.Media.Brushes.Gray;
                else
                    TotalPercentageBackGround = System.Windows.Visibility.Visible; //System.Windows.Media.Brushes.Red;
            }
        }

        private System.Windows.Visibility _MixerCapacityBackGround;
        public System.Windows.Visibility MixerCapacityBackGround
        {
            get { return _MixerCapacityBackGround; }
            set
            {
                _MixerCapacityBackGround = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MixerCapacityBackGround"));
            }
        }

        private System.Windows.Visibility _TotalPercentageBackGround;
        public System.Windows.Visibility TotalPercentageBackGround
        {
            get { return _TotalPercentageBackGround; }
            set
            {
                _TotalPercentageBackGround = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalPercentageBackGround"));
            }
        }

        //----------------------------------------------------------------------//
        //                              Commands                                //
        //----------------------------------------------------------------------//

        public ICommand UseIngredientCommand
        {
            get { return cmdUseIngredient; }
        }
        public void cmdUseIngredientImplementation()
        {
            if (SelectedAvailableIngredient != null)
            {
                UsedRecipeIngredients.Add(SelectedAvailableIngredient);
                AvailableRecipeIngredients.Remove(SelectedAvailableIngredient);
            }
        }


        public ICommand RemoveIngredientCommand
        {
            get { return cmdRemoveIngredient; }
        }
        public void cmdRemoveIngredientImplementation()
        {
            if (SelectedUsedIngredient != null)
            {
                SelectedUsedIngredient.IngredientPercentageValue = 0;
                AvailableRecipeIngredients.Add(SelectedUsedIngredient);
                UsedRecipeIngredients.Remove(SelectedUsedIngredient);
            }
        }

        public ICommand MoveUpCommand
        {
            get { return cmdMoveIngredientUp; }
        }
        public void cmdMoveIngredientUpImplementation()
        {
            //if (SelectedUsedIngredient != null)
            //{
            //    int iIndex = UsedRecipeIngredients.IndexOf(SelectedUsedIngredient);

            //    if (iIndex > 0)
            //    {
            //        Ingredient ingBot = UsedRecipeIngredients[iIndex];
            //        Ingredient ingTop = UsedRecipeIngredients[iIndex - 1];
            //        UsedRecipeIngredients[iIndex - 1] = ingBot;
            //        UsedRecipeIngredients[iIndex] = ingTop;
            //    }
            //}
        }

        public ICommand MoveDownCommand
        {
            get { return cmdMoveIngredientDown; }
        }
        public void cmdMoveIngredientDownImplementation()
        {
            //if (SelectedUsedIngredient != null)
            //{
            //    int iIndex = UsedRecipeIngredients.IndexOf(SelectedUsedIngredient);

            //    if (iIndex < UsedRecipeIngredients.Count)
            //    {
            //        Ingredient ingTop = UsedRecipeIngredients[iIndex];
            //        Ingredient ingBot = UsedRecipeIngredients[iIndex + 1];
            //        UsedRecipeIngredients[iIndex + 1] = ingTop;
            //        UsedRecipeIngredients[iIndex] = ingBot;
            //    }
            //}
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
