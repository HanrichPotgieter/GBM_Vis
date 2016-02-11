using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;



namespace KNEKT.Classes.MicroIngredients
{
    public class MicroIngredientViewModel : INotifyPropertyChanged
    {
        ObservableCollection<MicroIngredientModel> oc = new ObservableCollection<MicroIngredientModel>();
        string sqlConn = MainWindow.SqlConnectionString;

        public DelegateCommand microIngredient1UpdateClickCommand;
        public DelegateCommand microIngredient2UpdateClickCommand;
        public DelegateCommand microIngredient3UpdateClickCommand;
        public DelegateCommand microIngredient4UpdateClickCommand;
        public DelegateCommand addNewMicroIngredientClickCommand;
        

        public MicroIngredientViewModel()
        {
            LoadAllMicroIngredients();
            LoadUsedMicroIngredients();
            microIngredient1UpdateClickCommand = new DelegateCommand(microIngredient1UpdateClickCommandImplementation);
            microIngredient2UpdateClickCommand = new DelegateCommand(microIngredient2UpdateClickCommandImplementation);
            microIngredient3UpdateClickCommand = new DelegateCommand(microIngredient3UpdateClickCommandImplementation);
            microIngredient4UpdateClickCommand = new DelegateCommand(microIngredient4UpdateClickCommandImplementation);
            addNewMicroIngredientClickCommand = new DelegateCommand(addNewMicroIngredientClickCommandImplementation);
            ClearUpdateResultMessageAndVisibility();
        }

        private void LoadAllMicroIngredients()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT ProductID, ProductName from Products WHERE ProductType = 2 order by ProductName asc";
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        oc.Add(new MicroIngredientModel() { ComponentID = reader.GetInt32(0), ComponentName = reader.GetString(1) });
                        Components = oc;

                    }
                    conn.Close();
                }
            }
            catch (Exception ae)
            {
                //   MessageBox.Show("RefreshComponents: " + ae.Message, "Get Components", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadUsedMicroIngredients()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = @"SELECT a.BinID, a.ProductName 
                                    FROM (SELECT bpcl.BinID, p.ProductName,ROW_NUMBER() OVER (PARTITION BY bpcl.BinID ORDER BY bpcl.ts_datetime DESC) AS rn 
											FROM binproductchangelog bpcl inner join Products p on bpcl.BinProductID = p.ProductID 
											WHERE BinID in (23,24,25,26)
										)a
									WHERE a.rn = 1";
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int microdoserID = reader.GetInt32(0);

                        if (microdoserID == 23)
                        {
                            MicroIngredient1Name = reader.GetString(1);
                        }
                        else if (microdoserID == 24)
                        {
                            MicroIngredient2Name = reader.GetString(1);
                        }
                        else if (microdoserID == 25)
                        {
                            MicroIngredient3Name = reader.GetString(1);
                        }
                        else if (microdoserID == 26)
                        {
                            MicroIngredient4Name = reader.GetString(1);
                        }

                    }
                    conn.Close();
                }
            }
            catch (Exception ae)
            {
                //   MessageBox.Show("RefreshComponents: " + ae.Message, "Get Components", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private ObservableCollection<MicroIngredientModel> _Components;
        public ObservableCollection<MicroIngredientModel> Components
        {
            get { return _Components; }
            set
            {
                _Components = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Components"));
            }
        }


        private string _MicroIngredient1Name;
        public string MicroIngredient1Name
        {
            get { return _MicroIngredient1Name; }
            set
            {
                _MicroIngredient1Name = value;
                if (_MicroIngredient1Name != null)
                {
                    _MicroIngredient1Name = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("MicroIngredient1Name"));
                }
            }
        }

        private string _MicroIngredient2Name;
        public string MicroIngredient2Name
        {
            get { return _MicroIngredient2Name; }
            set
            {
                _MicroIngredient2Name = value;
                if (_MicroIngredient2Name != null)
                {
                    _MicroIngredient2Name = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("MicroIngredient2Name"));
                }
            }


        }

        private string _MicroIngredient3Name;
        public string MicroIngredient3Name
        {
            get { return _MicroIngredient3Name; }
            set
            {
                _MicroIngredient3Name = value;
                if (_MicroIngredient3Name != null)
                {
                    _MicroIngredient3Name = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("MicroIngredient3Name"));
                }
            }
        }

        private string _MicroIngredient4Name;
        public string MicroIngredient4Name
        {
            get { return _MicroIngredient4Name; }
            set
            {
                _MicroIngredient4Name = value;
                if (_MicroIngredient4Name != null)
                {
                    _MicroIngredient4Name = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("MicroIngredient4Name"));
                }
            }
        }

        private int _ComponentID;
        public int ComponentID
        {
            get { return _ComponentID; }
            set
            {
                _ComponentID = value;

            }
        }

        private string _NewMicroIngredientName;
        public string NewMicroIngredientName
        {
            get { return _NewMicroIngredientName; }
            set
            {
                _NewMicroIngredientName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewMicroIngredientName"));
            }
        }

        private string _UpdateResult;
        public string UpdateResult
        {
            get { return _UpdateResult; }
            set { 
                _UpdateResult = value; 
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateResult"));
            }
        }

        private Color _UpdateResultColor;
        public Color UpdateResultColor
        {
            get { return _UpdateResultColor; }
            set 
            { 
                _UpdateResultColor = value; 
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateResultColor"));
            }
        }

        private Visibility _UpdateMessageVisibility;
        public Visibility UpdateMessageVisibility
        {
            get 
            {
                return _UpdateMessageVisibility; 
            }
            set 
            {
                _UpdateMessageVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateMessageVisibility"));
            }
        }

        private Visibility _UpdateMessageFailureImageVisibility;
        public Visibility UpdateMessageFailureImageVisibility
        {
            get
            {
                return _UpdateMessageFailureImageVisibility;
            }
            set
            {
                _UpdateMessageFailureImageVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateMessageFailureImageVisibility"));
            }
        }

        private Visibility _UpdateMessageSuccessImageVisibility;
        public Visibility UpdateMessageSuccessImageVisibility
        {
            get
            {
                return _UpdateMessageSuccessImageVisibility;
            }
            set
            {
                _UpdateMessageSuccessImageVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateMessageSuccessImageVisibility"));
            }
        }

        public ICommand UpdateButton1ClickCommand
        {
            get { return microIngredient1UpdateClickCommand; }
        }

        public ICommand UpdateButton2ClickCommand
        {
            get { return microIngredient2UpdateClickCommand; }
        }

        public ICommand UpdateButton3ClickCommand
        {
            get { return microIngredient3UpdateClickCommand; }
        }

        public ICommand UpdateButton4ClickCommand
        {
            get { return microIngredient4UpdateClickCommand; }
        }

        public ICommand AddNewComponent
        {
            get { return addNewMicroIngredientClickCommand; }
        }

        public void microIngredient1UpdateClickCommandImplementation()
        {
            
            ClearUpdateResultMessageAndVisibility();
            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(23," + SelectedIngredientMicroIngredient1.ComponentID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MicroIngredient1Name = SelectedIngredientMicroIngredient1.ComponentName;
                        UpdateSuccessfullMessageDisplay();
                    }
                    else
                    {
                        ClearUpdateResultMessageAndVisibility();
                    }


                    conn.Close();
                }
            }
            catch (NullReferenceException)
            {
                UpdateResultNoProductSelected();
            }
            catch (Exception ae)
            {
                UpdateFailedMessageDisplay();
            }
        }

        public void microIngredient2UpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(24," + SelectedIngredientMicroIngredient2.ComponentID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MicroIngredient1Name = SelectedIngredientMicroIngredient2.ComponentName;
                        UpdateSuccessfullMessageDisplay();
                    }
                    else
                    {
                        ClearUpdateResultMessageAndVisibility();
                    }

                    conn.Close();
                }
            }
            catch(NullReferenceException)
            {
                UpdateResultNoProductSelected();
            }
            catch (Exception ae)
            {
                UpdateFailedMessageDisplay();
            }
        }

        public void microIngredient3UpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(25," + SelectedIngredientMicroIngredient3.ComponentID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MicroIngredient1Name = SelectedIngredientMicroIngredient3.ComponentName;
                        UpdateSuccessfullMessageDisplay();
                    }
                    else
                    {
                        ClearUpdateResultMessageAndVisibility();
                    }

                    conn.Close();
                }
            }
            catch (NullReferenceException)
            {
                UpdateResultNoProductSelected();
            }
            catch (Exception ae)
            {
                UpdateFailedMessageDisplay();
            }
        }

        public void microIngredient4UpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(26," + SelectedIngredientMicroIngredient4.ComponentID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MicroIngredient1Name = SelectedIngredientMicroIngredient4.ComponentName;
                        UpdateSuccessfullMessageDisplay();
                    }
                    else
                    {
                        ClearUpdateResultMessageAndVisibility();
                    }


                    conn.Close();
                }
            }
            catch (NullReferenceException)
            {
                UpdateResultNoProductSelected();
            }
            catch (Exception ae)
            {
                UpdateFailedMessageDisplay();
            }
        }

        public void addNewMicroIngredientClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            if (NewMicroIngredientName != null)
            {
                try
                {

                    using (SqlConnection conn = new SqlConnection(sqlConn))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand("INSERT INTO Products (ProductName,ProductAbbreviation, ProductType) VALUES('" + NewMicroIngredientName + "',' '," + 2 + ")", conn);
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            Components.Clear();
                            LoadAllMicroIngredients();
                        }
                        conn.Close();
                    }
                }
                catch (Exception ae)
                {

                }
            }
            else
            {
                UpdateResultNoNewMicroIngredientName();
            }
        }


        private MicroIngredientModel _SelectedIngredientMicroIngredient1;
        public MicroIngredientModel SelectedIngredientMicroIngredient1
        {
            get { return _SelectedIngredientMicroIngredient1; }
            set
            {

                _SelectedIngredientMicroIngredient1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIngredientMicroIngredient1"));
            }
        }

        private MicroIngredientModel _SelectedIngredientMicroIngredient2;
        public MicroIngredientModel SelectedIngredientMicroIngredient2
        {
            get { return _SelectedIngredientMicroIngredient2; }
            set
            {
                _SelectedIngredientMicroIngredient2 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIngredientMicroIngredient2"));
            }
        }

        private MicroIngredientModel _SelectedIngredientMicroIngredient3;
        public MicroIngredientModel SelectedIngredientMicroIngredient3
        {
            get { return _SelectedIngredientMicroIngredient3; }
            set
            {
                _SelectedIngredientMicroIngredient3 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIngredientMicroIngredient3"));
            }
        }

        private MicroIngredientModel _SelectedIngredientMicroIngredient4;
        public MicroIngredientModel SelectedIngredientMicroIngredient4
        {
            get { return _SelectedIngredientMicroIngredient4; }
            set
            {
                _SelectedIngredientMicroIngredient4 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIngredientMicroIngredient4"));
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

        private void UpdateResultNoNewMicroIngredientName()
        {
            UpdateResult = "Please enter a product name and try again.";
            UpdateResultColor = Colors.Red;
            UpdateMessageFailureImageVisibility = Visibility.Visible;
            UpdateMessageVisibility = Visibility.Visible;
        }

        private void UpdateResultNoProductSelected()
        {
            UpdateResult = "Please select a product name and try again.";
            UpdateResultColor = Colors.Red;
            UpdateMessageFailureImageVisibility = Visibility.Visible;
            UpdateMessageVisibility = Visibility.Visible;
        }

        private void UpdateSuccessfullMessageDisplay()
        {
            UpdateResult = "Ingredient Updated Successfully!";
            UpdateResultColor = Colors.LimeGreen;
            UpdateMessageSuccessImageVisibility = Visibility.Visible;
            UpdateMessageVisibility = Visibility.Visible;
        }

        private void UpdateFailedMessageDisplay()
        {
            UpdateResult = "Update Failed. Please try again.";
            UpdateResultColor = Colors.Red;
            UpdateMessageFailureImageVisibility = Visibility.Visible;
            UpdateMessageVisibility = Visibility.Visible;
        }

        private void ClearUpdateResultMessageAndVisibility()
        {
            UpdateMessageVisibility = Visibility.Hidden;
            UpdateResult = "";
            UpdateResultColor = Colors.Transparent;
            UpdateMessageFailureImageVisibility = Visibility.Hidden;
            UpdateMessageSuccessImageVisibility = Visibility.Hidden;
        }
    }
}
