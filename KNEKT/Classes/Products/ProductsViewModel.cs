using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace KNEKT.Classes.Products
{
    public class ProductsViewModel : INotifyPropertyChanged
    {
        ObservableCollection<ProductsModel> oc = new ObservableCollection<ProductsModel>();
        string sqlConn = MainWindow.SqlConnectionString;

        //public DelegateCommand bin21ProductUpdateClickCommand;
        //public DelegateCommand bin22ProductUpdateClickCommand;
        //public DelegateCommand bin23ProductUpdateClickCommand;
        //public DelegateCommand bin24ProductUpdateClickCommand;
        public DelegateCommand bin19ProductUpdateClickCommand;
        public DelegateCommand bin20ProductUpdateClickCommand;
        public DelegateCommand bin21ProductUpdateClickCommand;
        public DelegateCommand bin22ProductUpdateClickCommand;
        public DelegateCommand bin201ProductUpdateClickCommand;
        public DelegateCommand bin202ProductUpdateClickCommand;
        public DelegateCommand bin203ProductUpdateClickCommand;
        public DelegateCommand bin401ProductUpdateClickCommand;
        public DelegateCommand bin402ProductUpdateClickCommand;
        public DelegateCommand bin403ProductUpdateClickCommand;
        public DelegateCommand bin404ProductUpdateClickCommand;
        public DelegateCommand bin405ProductUpdateClickCommand;
        public DelegateCommand bin406ProductUpdateClickCommand;
        public DelegateCommand bin407ProductUpdateClickCommand;
        public DelegateCommand bin411ProductUpdateClickCommand;
        public DelegateCommand bin501ProductUpdateClickCommand;
        public DelegateCommand bin502ProductUpdateClickCommand;
        public DelegateCommand bin503ProductUpdateClickCommand;
        public DelegateCommand bin504ProductUpdateClickCommand;
        public DelegateCommand bin601ProductUpdateClickCommand;
        public DelegateCommand bin602ProductUpdateClickCommand;
        
        public DelegateCommand addNewProductClickCommand;
        public DelegateCommand binProductManagementCloseCommand;
        public bool[] binButtonEnable = new bool[21];
        DispatcherTimer timerHideUpdateMessage = new DispatcherTimer();

        //public event PropertyChangedEventHandler  PropertyChanged;

        public ProductsViewModel()
        {

            LoadAllProducts();
            LoadUsedProducts();

            //bin21ProductUpdateClickCommand = new DelegateCommand(bin21ProductUpdateClickCommandImplementation);
            //bin22ProductUpdateClickCommand = new DelegateCommand(bin22ProductUpdateClickCommandImplementation);
            //bin23ProductUpdateClickCommand = new DelegateCommand(bin23ProductUpdateClickCommandImplementation);
            //bin24ProductUpdateClickCommand = new DelegateCommand(bin24ProductUpdateClickCommandImplementation);
            bin19ProductUpdateClickCommand = new DelegateCommand(bin19ProductUpdateClickCommandImplementation);
            bin20ProductUpdateClickCommand = new DelegateCommand(bin20ProductUpdateClickCommandImplementation);
            bin21ProductUpdateClickCommand = new DelegateCommand(bin21ProductUpdateClickCommandImplementation);
            bin22ProductUpdateClickCommand = new DelegateCommand(bin22ProductUpdateClickCommandImplementation);
            bin201ProductUpdateClickCommand = new DelegateCommand(bin201ProductUpdateClickCommandImplementation);
            bin202ProductUpdateClickCommand = new DelegateCommand(bin202ProductUpdateClickCommandImplementation);
            bin203ProductUpdateClickCommand = new DelegateCommand(bin203ProductUpdateClickCommandImplementation);
            bin401ProductUpdateClickCommand = new DelegateCommand(bin401ProductUpdateClickCommandImplementation);
            bin402ProductUpdateClickCommand = new DelegateCommand(bin402ProductUpdateClickCommandImplementation);
            bin403ProductUpdateClickCommand = new DelegateCommand(bin403ProductUpdateClickCommandImplementation);
            bin404ProductUpdateClickCommand = new DelegateCommand(bin404ProductUpdateClickCommandImplementation);
            bin405ProductUpdateClickCommand = new DelegateCommand(bin405ProductUpdateClickCommandImplementation);
            bin406ProductUpdateClickCommand = new DelegateCommand(bin406ProductUpdateClickCommandImplementation);
            bin407ProductUpdateClickCommand = new DelegateCommand(bin407ProductUpdateClickCommandImplementation);
            bin411ProductUpdateClickCommand = new DelegateCommand(bin411ProductUpdateClickCommandImplementation);
            bin501ProductUpdateClickCommand = new DelegateCommand(bin501ProductUpdateClickCommandImplementation);
            bin502ProductUpdateClickCommand = new DelegateCommand(bin502ProductUpdateClickCommandImplementation);
            bin503ProductUpdateClickCommand = new DelegateCommand(bin503ProductUpdateClickCommandImplementation);
            bin504ProductUpdateClickCommand = new DelegateCommand(bin504ProductUpdateClickCommandImplementation);
            bin601ProductUpdateClickCommand = new DelegateCommand(bin601ProductUpdateClickCommandImplementation);
            bin602ProductUpdateClickCommand = new DelegateCommand(bin602ProductUpdateClickCommandImplementation);

        


            addNewProductClickCommand = new DelegateCommand(addNewProductClickCommandImplementation);
            binProductManagementCloseCommand = new DelegateCommand(binProductManagementCloseCommandImplementation);
            ClearUpdateResultMessageAndVisibility();

            timerHideUpdateMessage.Tick += new EventHandler(timerHideUpdateMessage_Tick);
            timerHideUpdateMessage.Interval = new TimeSpan(0, 0, 0, 3, 0);
            
        }

        private void LoadAllProducts()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM Products WHERE ProductType = 1 order by ProductName asc";
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        // string me = reader.GetString(2);
                        oc.Add(new ProductsModel() { ProductID = reader.GetInt32(0), ProductName = reader.GetString(1) + " - " + reader.GetString(2), ProductAbbreviation = reader.GetString(2) });
                        Products = oc;

                    }
                    conn.Close();
                }
            }
            catch (Exception ae)
            {
                //   MessageBox.Show("RefreshComponents: " + ae.Message, "Get Components", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadUsedProducts()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = @"SELECT a.BinID, a.ProductName, a.ProductAbbreviation 
                                    FROM (SELECT *,ROW_NUMBER() OVER (PARTITION BY BinID ORDER BY ts_datetime DESC) AS rn 
                                    FROM BinProductChangeLog bpcl inner join Products p on bpcl.BinProductID = p.ProductID)a
                                        WHERE a.rn = 1";
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int binID = reader.GetInt32(0);

                        //if (binID == 1)
                        //{
                        //    Bin21ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                        //    Bin21ProductNameAbbreviation = reader.GetString(2);
                        //}
                        //else if (binID == 2)
                        //{
                        //    Bin22ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                        //    Bin22ProductNameAbbreviation = reader.GetString(2);
                        //}
                        //else if (binID == 3)
                        //{
                        //    Bin23ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                        //    Bin23ProductNameAbbreviation = reader.GetString(2);
                        //}
                        //else if (binID == 4)
                        //{
                        //    Bin24ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                        //    Bin24ProductNameAbbreviation = reader.GetString(2);
                        //}
                        if (binID == 1)
                        {
                            Bin19ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin19ProductNameAbbreviation = reader.GetString(2);
                        }
                        else if (binID == 2)
                        {
                            Bin20ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin20ProductNameAbbreviation = reader.GetString(2);
                        }
                        else if (binID == 3)
                        {
                            Bin21ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin21ProductNameAbbreviation = reader.GetString(2);
                        }
                        else if (binID == 4)
                        {
                            Bin22ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin22ProductNameAbbreviation = reader.GetString(2);
                        }
                        else if (binID == 5)
                        {
                            Bin201ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin201ProductNameAbbreviation = reader.GetString(2);
                        }
                        else if (binID == 6)
                        {
                            Bin202ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin202ProductNameAbbreviation = reader.GetString(2);
                        }
                        else if (binID == 7)
                        {
                            Bin203ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin203ProductNameAbbreviation = reader.GetString(2);
                        }
                        else if (binID == 8)
                        {
                            Bin401ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin401ProductNameAbbreviation = reader.GetString(2);
                        }
                        else if (binID == 9)
                        {
                            Bin402ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin402ProductNameAbbreviation = reader.GetString(2);
                        }
                        else if (binID == 10)
                        {
                            Bin403ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin403ProductNameAbbreviation = reader.GetString(2);
                        }
                        else if (binID == 11)
                        {
                            Bin404ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin404ProductNameAbbreviation = reader.GetString(2);
                        }
                        else if (binID == 12)
                        {
                            Bin405ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin405ProductNameAbbreviation = reader.GetString(2);
                        }
                        else if (binID == 13)
                        {
                            Bin406ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin406ProductNameAbbreviation = reader.GetString(2);
                        }
                        else if (binID == 14)
                        {
                            Bin407ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin407ProductNameAbbreviation = reader.GetString(2);
                        }
                        else if (binID == 15)
                        {
                            Bin411ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin411ProductNameAbbreviation = reader.GetString(2);
                        }
                        else if (binID == 16)
                        {
                            Bin501ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin501ProductNameAbbreviation = reader.GetString(2);
                        }
                        else if (binID == 17)
                        {
                            Bin502ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin502ProductNameAbbreviation = reader.GetString(2);
                        }
                        else if (binID == 18)
                        {
                            Bin503ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin503ProductNameAbbreviation = reader.GetString(2);
                        }
                        else if (binID == 19)
                        {
                            Bin504ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin504ProductNameAbbreviation = reader.GetString(2);
                        }
                        else if (binID == 20)
                        {
                            Bin601ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin601ProductNameAbbreviation = reader.GetString(2);
                        }
                        else if (binID == 21)
                        {
                            Bin602ProductName = reader.GetString(1) + " - " + reader.GetString(2);
                            Bin602ProductNameAbbreviation = reader.GetString(2);
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

        private void timerHideUpdateMessage_Tick(object sender, EventArgs e)
        {
            ClearUpdateResultMessageAndVisibility();
            timerHideUpdateMessage.Stop();
        }

        private ObservableCollection<ProductsModel> _Products;
        public ObservableCollection<ProductsModel> Products
        {
            get { return _Products; }
            set
            {
                _Products = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Products"));
            }
        }

        #region Product Name Properties

        //private string _Bin21ProductName;
        //public string Bin21ProductName
        //{
        //    get { return _Bin21ProductName; }
        //    set
        //    {
        //        _Bin21ProductName = value;
        //        if (_Bin21ProductName != null)
        //        {
        //            _Bin21ProductName = value;
        //            OnPropertyChanged(new PropertyChangedEventArgs("Bin21ProductName"));
        //        }
        //    }
        //}

        //private string _Bin22ProductName;
        //public string Bin22ProductName
        //{
        //    get { return _Bin22ProductName; }
        //    set
        //    {
        //        _Bin22ProductName = value;
        //        if (_Bin22ProductName != null)
        //        {
        //            _Bin22ProductName = value;
        //            OnPropertyChanged(new PropertyChangedEventArgs("Bin22ProductName"));
        //        }
        //    }
        //}

        //private string _Bin23ProductName;
        //public string Bin23ProductName
        //{
        //    get { return _Bin23ProductName; }
        //    set
        //    {
        //        _Bin23ProductName = value;
        //        if (_Bin23ProductName != null)
        //        {
        //            _Bin23ProductName = value;
        //            OnPropertyChanged(new PropertyChangedEventArgs("Bin23ProductName"));
        //        }
        //    }
        //}

        //private string _Bin24ProductName;
        //public string Bin24ProductName
        //{
        //    get { return _Bin24ProductName; }
        //    set
        //    {
        //        _Bin24ProductName = value;
        //        if (_Bin24ProductName != null)
        //        {
        //            _Bin24ProductName = value;
        //            OnPropertyChanged(new PropertyChangedEventArgs("Bin24ProductName"));
        //        }
        //    }
        //}

        private string _Bin19ProductName;
        public string Bin19ProductName
        {
            get { return _Bin19ProductName; }
            set
            {
                _Bin19ProductName = value;
                if (_Bin19ProductName != null)
                {
                    _Bin19ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin19ProductName"));
                }
            }
        }

        private string _Bin20ProductName;
        public string Bin20ProductName
        {
            get { return _Bin20ProductName; }
            set
            {
                _Bin20ProductName = value;
                if (_Bin20ProductName != null)
                {
                    _Bin20ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin20ProductName"));
                }
            }
        }

        private string _Bin21ProductName;
        public string Bin21ProductName
        {
            get { return _Bin21ProductName; }
            set
            {
                _Bin21ProductName = value;
                if (_Bin21ProductName != null)
                {
                    _Bin21ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin21ProductName"));
                }
            }
        }

        private string _Bin22ProductName;
        public string Bin22ProductName
        {
            get { return _Bin22ProductName; }
            set
            {
                _Bin22ProductName = value;
                if (_Bin22ProductName != null)
                {
                    _Bin22ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin22ProductName"));
                }
            }
        }

        //
        private string _Bin201ProductName;
        public string Bin201ProductName
        {
            get { return _Bin201ProductName; }
            set
            {
                _Bin201ProductName = value;
                if (_Bin201ProductName != null)
                {
                    _Bin201ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin201ProductName"));
                }
            }
        }

        private string _Bin202ProductName;
        public string Bin202ProductName
        {
            get { return _Bin202ProductName; }
            set
            {
                _Bin202ProductName = value;
                if (_Bin202ProductName != null)
                {
                    _Bin202ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin202ProductName"));
                }
            }
        }

        private string _Bin203ProductName;
        public string Bin203ProductName
        {
            get { return _Bin203ProductName; }
            set
            {
                _Bin203ProductName = value;
                if (_Bin203ProductName != null)
                {
                    _Bin203ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin203ProductName"));
                }
            }
        }

        private string _Bin401ProductName;
        public string Bin401ProductName
        {
            get { return _Bin401ProductName; }
            set
            {
                _Bin401ProductName = value;
                if (_Bin401ProductName != null)
                {
                    _Bin401ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin401ProductName"));
                }
            }
        }

        private string _Bin402ProductName;
        public string Bin402ProductName
        {
            get { return _Bin402ProductName; }
            set
            {
                _Bin402ProductName = value;
                if (_Bin402ProductName != null)
                {
                    _Bin402ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin402ProductName"));
                }
            }
        }

        private string _Bin403ProductName;
        public string Bin403ProductName
        {
            get { return _Bin403ProductName; }
            set
            {
                _Bin403ProductName = value;
                if (_Bin403ProductName != null)
                {
                    _Bin403ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin403ProductName"));
                }
            }
        }

        private string _Bin404ProductName;
        public string Bin404ProductName
        {
            get { return _Bin404ProductName; }
            set
            {
                _Bin404ProductName = value;
                if (_Bin404ProductName != null)
                {
                    _Bin404ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin404ProductName"));
                }
            }
        }

        private string _Bin405ProductName;
        public string Bin405ProductName
        {
            get { return _Bin405ProductName; }
            set
            {
                _Bin405ProductName = value;
                if (_Bin405ProductName != null)
                {
                    _Bin405ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin405ProductName"));
                }
            }
        }

        private string _Bin406ProductName;
        public string Bin406ProductName
        {
            get { return _Bin406ProductName; }
            set
            {
                _Bin406ProductName = value;
                if (_Bin406ProductName != null)
                {
                    _Bin406ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin406ProductName"));
                }
            }
        }

        private string _Bin407ProductName;
        public string Bin407ProductName
        {
            get { return _Bin407ProductName; }
            set
            {
                _Bin407ProductName = value;
                if (_Bin407ProductName != null)
                {
                    _Bin407ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin407ProductName"));
                }
            }
        }

        private string _Bin411ProductName;
        public string Bin411ProductName
        {
            get { return _Bin411ProductName; }
            set
            {
                _Bin411ProductName = value;
                if (_Bin411ProductName != null)
                {
                    _Bin411ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin411ProductName"));
                }
            }
        }

        private string _Bin501ProductName;
        public string Bin501ProductName
        {
            get { return _Bin501ProductName; }
            set
            {
                _Bin501ProductName = value;
                if (_Bin501ProductName != null)
                {
                    _Bin501ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin501ProductName"));
                }
            }
        }

        private string _Bin502ProductName;
        public string Bin502ProductName
        {
            get { return _Bin502ProductName; }
            set
            {
                _Bin502ProductName = value;
                if (_Bin502ProductName != null)
                {
                    _Bin502ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin502ProductName"));
                }
            }
        }

        private string _Bin503ProductName;
        public string Bin503ProductName
        {
            get { return _Bin503ProductName; }
            set
            {
                _Bin503ProductName = value;
                if (_Bin503ProductName != null)
                {
                    _Bin503ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin503ProductName"));
                }
            }
        }

        private string _Bin504ProductName;
        public string Bin504ProductName
        {
            get { return _Bin504ProductName; }
            set
            {
                _Bin504ProductName = value;
                if (_Bin504ProductName != null)
                {
                    _Bin504ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin504ProductName"));
                }
            }
        }

        private string _Bin601ProductName;
        public string Bin601ProductName
        {
            get { return _Bin601ProductName; }
            set
            {
                _Bin601ProductName = value;
                if (_Bin601ProductName != null)
                {
                    _Bin601ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin601ProductName"));
                }
            }
        }

        private string _Bin602ProductName;
        public string Bin602ProductName
        {
            get { return _Bin602ProductName; }
            set
            {
                _Bin602ProductName = value;
                if (_Bin602ProductName != null)
                {
                    _Bin602ProductName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin602ProductName"));
                }
            }
        }

        private string _NewProductName;
        public string NewProductName
        {
            get { return _NewProductName; }
            set
            {
                _NewProductName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewProductName"));
            }
        }

        #endregion


        #region Product Name Abbreviation Properties

        //private string _Bin21ProductNameAbbreviation;
        //public string Bin21ProductNameAbbreviation
        //{
        //    get { return _Bin21ProductNameAbbreviation; }
        //    set
        //    {
        //        _Bin21ProductNameAbbreviation = value;
        //        if (_Bin21ProductNameAbbreviation != null)
        //        {
        //            _Bin21ProductNameAbbreviation = value;
        //            //Bin21ProductName = Bin21ProductName + " - " + _Bin21ProductNameAbbreviation;
        //            OnPropertyChanged(new PropertyChangedEventArgs("Bin21ProductNameAbbreviation"));
        //        }
        //    }
        //}

        //private string _Bin22ProductNameAbbreviation;
        //public string Bin22ProductNameAbbreviation
        //{
        //    get { return _Bin22ProductNameAbbreviation; }
        //    set
        //    {
        //        _Bin22ProductNameAbbreviation = value;
        //        if (_Bin22ProductNameAbbreviation != null)
        //        {
        //            _Bin22ProductNameAbbreviation = value;
        //            OnPropertyChanged(new PropertyChangedEventArgs("Bin22ProductNameAbbreviation"));
        //        }
        //    }
        //}

        //private string _Bin23ProductNameAbbreviation;
        //public string Bin23ProductNameAbbreviation
        //{
        //    get { return _Bin23ProductNameAbbreviation; }
        //    set
        //    {
        //        _Bin23ProductNameAbbreviation = value;
        //        if (_Bin23ProductNameAbbreviation != null)
        //        {
        //            _Bin23ProductNameAbbreviation = value;
        //            OnPropertyChanged(new PropertyChangedEventArgs("Bin23ProductNameAbbreviation"));
        //        }
        //    }
        //}

        //private string _Bin24ProductNameAbbreviation;
        //public string Bin24ProductNameAbbreviation
        //{
        //    get { return _Bin24ProductNameAbbreviation; }
        //    set
        //    {
        //        _Bin24ProductNameAbbreviation = value;
        //        if (_Bin24ProductNameAbbreviation != null)
        //        {
        //            _Bin24ProductNameAbbreviation = value;
        //            OnPropertyChanged(new PropertyChangedEventArgs("Bin24ProductNameAbbreviation"));
        //        }
        //    }
        //}

        private string _Bin19ProductNameAbbreviation;
        public string Bin19ProductNameAbbreviation
        {
            get { return _Bin19ProductNameAbbreviation; }
            set
            {
                _Bin19ProductNameAbbreviation = value;
                if (_Bin19ProductNameAbbreviation != null)
                {
                    _Bin19ProductNameAbbreviation = value;
                    //Bin19ProductName = Bin19ProductName + " - " + _Bin19ProductNameAbbreviation;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin19ProductNameAbbreviation"));
                }
            }
        }

        private string _Bin20ProductNameAbbreviation;
        public string Bin20ProductNameAbbreviation
        {
            get { return _Bin20ProductNameAbbreviation; }
            set
            {
                _Bin20ProductNameAbbreviation = value;
                if (_Bin20ProductNameAbbreviation != null)
                {
                    _Bin20ProductNameAbbreviation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin20ProductNameAbbreviation"));
                }
            }
        }

        private string _Bin21ProductNameAbbreviation;
        public string Bin21ProductNameAbbreviation
        {
            get { return _Bin21ProductNameAbbreviation; }
            set
            {
                _Bin21ProductNameAbbreviation = value;
                if (_Bin21ProductNameAbbreviation != null)
                {
                    _Bin21ProductNameAbbreviation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin21ProductNameAbbreviation"));
                }
            }
        }

        private string _Bin22ProductNameAbbreviation;
        public string Bin22ProductNameAbbreviation
        {
            get { return _Bin22ProductNameAbbreviation; }
            set
            {
                _Bin22ProductNameAbbreviation = value;
                if (_Bin22ProductNameAbbreviation != null)
                {
                    _Bin22ProductNameAbbreviation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin22ProductNameAbbreviation"));
                }
            }
        }

        private string _Bin201ProductNameAbbreviation;
        public string Bin201ProductNameAbbreviation
        {
            get { return _Bin201ProductNameAbbreviation; }
            set
            {
                _Bin201ProductNameAbbreviation = value;
                if (_Bin201ProductNameAbbreviation != null)
                {
                    _Bin201ProductNameAbbreviation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin201ProductNameAbbreviation"));
                }
            }
        }

        private string _Bin202ProductNameAbbreviation;
        public string Bin202ProductNameAbbreviation
        {
            get { return _Bin202ProductNameAbbreviation; }
            set
            {
                _Bin202ProductNameAbbreviation = value;
                if (_Bin202ProductNameAbbreviation != null)
                {
                    _Bin202ProductNameAbbreviation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin202ProductNameAbbreviation"));
                }
            }
        }

        private string _Bin203ProductNameAbbreviation;
        public string Bin203ProductNameAbbreviation
        {
            get { return _Bin203ProductNameAbbreviation; }
            set
            {
                _Bin203ProductNameAbbreviation = value;
                if (_Bin203ProductNameAbbreviation != null)
                {
                    _Bin203ProductNameAbbreviation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin203ProductNameAbbreviation"));
                }
            }
        }

        private string _Bin401ProductNameAbbreviation;
        public string Bin401ProductNameAbbreviation
        {
            get { return _Bin401ProductNameAbbreviation; }
            set
            {
                _Bin401ProductNameAbbreviation = value;
                if (_Bin401ProductNameAbbreviation != null)
                {
                    _Bin401ProductNameAbbreviation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin401ProductNameAbbreviation"));
                }
            }
        }

        private string _Bin402ProductNameAbbreviation;
        public string Bin402ProductNameAbbreviation
        {
            get { return _Bin402ProductNameAbbreviation; }
            set
            {
                _Bin402ProductNameAbbreviation = value;
                if (_Bin402ProductNameAbbreviation != null)
                {
                    _Bin402ProductNameAbbreviation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin402ProductNameAbbreviation"));
                }
            }
        }

        private string _Bin403ProductNameAbbreviation;
        public string Bin403ProductNameAbbreviation
        {
            get { return _Bin403ProductNameAbbreviation; }
            set
            {
                _Bin403ProductNameAbbreviation = value;
                if (_Bin403ProductNameAbbreviation != null)
                {
                    _Bin403ProductNameAbbreviation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin403ProductNameAbbreviation"));
                }
            }
        }

        private string _Bin404ProductNameAbbreviation;
        public string Bin404ProductNameAbbreviation
        {
            get { return _Bin404ProductNameAbbreviation; }
            set
            {
                _Bin404ProductNameAbbreviation = value;
                if (_Bin404ProductNameAbbreviation != null)
                {
                    _Bin404ProductNameAbbreviation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin404ProductNameAbbreviation"));
                }
            }
        }

        private string _Bin405ProductNameAbbreviation;
        public string Bin405ProductNameAbbreviation
        {
            get { return _Bin405ProductNameAbbreviation; }
            set
            {
                _Bin405ProductNameAbbreviation = value;
                if (_Bin405ProductNameAbbreviation != null)
                {
                    _Bin405ProductNameAbbreviation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin405ProductNameAbbreviation"));
                }
            }
        }

        private string _Bin406ProductNameAbbreviation;
        public string Bin406ProductNameAbbreviation
        {
            get { return _Bin406ProductNameAbbreviation; }
            set
            {
                _Bin406ProductNameAbbreviation = value;
                if (_Bin406ProductNameAbbreviation != null)
                {
                    _Bin406ProductNameAbbreviation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin406ProductNameAbbreviation"));
                }
            }
        }

        private string _Bin407ProductNameAbbreviation;
        public string Bin407ProductNameAbbreviation
        {
            get { return _Bin407ProductNameAbbreviation; }
            set
            {
                _Bin407ProductNameAbbreviation = value;
                if (_Bin407ProductNameAbbreviation != null)
                {
                    _Bin407ProductNameAbbreviation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin407ProductNameAbbreviation"));
                }
            }
        }

        private string _Bin411ProductNameAbbreviation;
        public string Bin411ProductNameAbbreviation
        {
            get { return _Bin411ProductNameAbbreviation; }
            set
            {
                _Bin411ProductNameAbbreviation = value;
                if (_Bin411ProductNameAbbreviation != null)
                {
                    _Bin411ProductNameAbbreviation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin411ProductNameAbbreviation"));
                }
            }
        }

        private string _Bin501ProductNameAbbreviation;
        public string Bin501ProductNameAbbreviation
        {
            get { return _Bin501ProductNameAbbreviation; }
            set
            {
                _Bin501ProductNameAbbreviation = value;
                if (_Bin501ProductNameAbbreviation != null)
                {
                    _Bin501ProductNameAbbreviation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin501ProductNameAbbreviation"));
                }
            }
        }

        private string _Bin502ProductNameAbbreviation;
        public string Bin502ProductNameAbbreviation
        {
            get { return _Bin502ProductNameAbbreviation; }
            set
            {
                _Bin502ProductNameAbbreviation = value;
                if (_Bin502ProductNameAbbreviation != null)
                {
                    _Bin502ProductNameAbbreviation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin502ProductNameAbbreviation"));
                }
            }
        }

        private string _Bin503ProductNameAbbreviation;
        public string Bin503ProductNameAbbreviation
        {
            get { return _Bin503ProductNameAbbreviation; }
            set
            {
                _Bin503ProductNameAbbreviation = value;
                if (_Bin503ProductNameAbbreviation != null)
                {
                    _Bin503ProductNameAbbreviation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin503ProductNameAbbreviation"));
                }
            }
        }

        private string _Bin504ProductNameAbbreviation;
        public string Bin504ProductNameAbbreviation
        {
            get { return _Bin504ProductNameAbbreviation; }
            set
            {
                _Bin504ProductNameAbbreviation = value;
                if (_Bin504ProductNameAbbreviation != null)
                {
                    _Bin504ProductNameAbbreviation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin504ProductNameAbbreviation"));
                }
            }
        }

        private string _Bin601ProductNameAbbreviation;
        public string Bin601ProductNameAbbreviation
        {
            get { return _Bin601ProductNameAbbreviation; }
            set
            {
                _Bin601ProductNameAbbreviation = value;
                if (_Bin601ProductNameAbbreviation != null)
                {
                    _Bin601ProductNameAbbreviation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin601ProductNameAbbreviation"));
                }
            }
        }

        private string _Bin602ProductNameAbbreviation;
        public string Bin602ProductNameAbbreviation
        {
            get { return _Bin602ProductNameAbbreviation; }
            set
            {
                _Bin602ProductNameAbbreviation = value;
                if (_Bin602ProductNameAbbreviation != null)
                {
                    _Bin602ProductNameAbbreviation = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Bin602ProductNameAbbreviation"));
                }
            }
        }

        private string _NewProductAbbreviation;
        public string NewProductAbbreviation
        {
            get { return _NewProductAbbreviation; }
            set
            {
                _NewProductAbbreviation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewProductAbbreviation"));
            }
        }

        #endregion

        private int _Product;
        public int Product
        {
            get { return _Product; }
            set
            {
                _Product = value;

            }
        }

        private string _UpdateResult;
        public string UpdateResult
        {
            get { return _UpdateResult; }
            set
            {
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

       

        #region Button Commands

        //public ICommand UpdateButtonBin21ClickCommand
        //{
        //    get { return bin21ProductUpdateClickCommand; }
        //}

        //public ICommand UpdateButtonBin22ClickCommand
        //{
        //    get { return bin22ProductUpdateClickCommand; }
        //}

        //public ICommand UpdateButtonBin23ClickCommand
        //{
        //    get { return bin23ProductUpdateClickCommand; }
        //}

        //public ICommand UpdateButtonBin24ClickCommand
        //{
        //    get { return bin24ProductUpdateClickCommand; }
        //}

        public ICommand UpdateButtonBin19ClickCommand
        {
            get { return bin19ProductUpdateClickCommand; }
        }

        public ICommand UpdateButtonBin20ClickCommand
        {
            get { return bin20ProductUpdateClickCommand; }
        }

        public ICommand UpdateButtonBin21ClickCommand
        {
            get { return bin21ProductUpdateClickCommand; }
        }

        public ICommand UpdateButtonBin22ClickCommand
        {
            get { return bin22ProductUpdateClickCommand; }
        }

        public ICommand UpdateButtonBin201ClickCommand
        {
            get { return bin201ProductUpdateClickCommand; }
        }

        public ICommand UpdateButtonBin202ClickCommand
        {
            get { return bin202ProductUpdateClickCommand; }
        }

        public ICommand UpdateButtonBin203ClickCommand
        {
            get { return bin203ProductUpdateClickCommand; }
        }

        public ICommand UpdateButtonBin401ClickCommand
        {
            get { return bin401ProductUpdateClickCommand; }
        }

        public ICommand UpdateButtonBin402ClickCommand
        {
            get { return bin402ProductUpdateClickCommand; }
        }

        public ICommand UpdateButtonBin403ClickCommand
        {
            get { return bin403ProductUpdateClickCommand; }
        }

        public ICommand UpdateButtonBin404ClickCommand
        {
            get { return bin404ProductUpdateClickCommand; }
        }

        public ICommand UpdateButtonBin405ClickCommand
        {
            get { return bin405ProductUpdateClickCommand; }
        }

        public ICommand UpdateButtonBin406ClickCommand
        {
            get { return bin406ProductUpdateClickCommand; }
        }

        public ICommand UpdateButtonBin407ClickCommand
        {
            get { return bin407ProductUpdateClickCommand; }
        }

        public ICommand UpdateButtonBin411ClickCommand
        {
            get { return bin411ProductUpdateClickCommand; }
        }

        public ICommand UpdateButtonBin501ClickCommand
        {
            get { return bin501ProductUpdateClickCommand; }
        }

        public ICommand UpdateButtonBin502ClickCommand
        {
            get { return bin502ProductUpdateClickCommand; }
        }

        public ICommand UpdateButtonBin503ClickCommand
        {
            get { return bin503ProductUpdateClickCommand; }
        }

        public ICommand UpdateButtonBin504ClickCommand
        {
            get { return bin504ProductUpdateClickCommand; }
        }

        public ICommand UpdateButtonBin601ClickCommand
        {
            get { return bin601ProductUpdateClickCommand; }
        }

        public ICommand UpdateButtonBin602ClickCommand
        {
            get { return bin602ProductUpdateClickCommand; }
        }

        public ICommand AddNewProduct
        {
            get { return addNewProductClickCommand; }
        }

        public ICommand BinProductManagementCloseCommand
        {
            get { return binProductManagementCloseCommand; }
        }

        #endregion


        #region Button Enables

        //private bool _bin21UpdateButtonEnable;
        //public bool bin21UpdateButtonEnable
        //{
        //    get
        //    {
        //        return _bin21UpdateButtonEnable;
        //    }
        //    set
        //    {
        //        _bin21UpdateButtonEnable = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("bin21UpdateButtonEnable"));
        //    }
        //}

        //private bool _bin22UpdateButtonEnable;
        //public bool bin22UpdateButtonEnable
        //{
        //    get
        //    {
        //        return _bin22UpdateButtonEnable;
        //    }
        //    set
        //    {
        //        _bin22UpdateButtonEnable = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("bin22UpdateButtonEnable"));
        //    }
        //}

        //private bool _bin23UpdateButtonEnable;
        //public bool bin23UpdateButtonEnable
        //{
        //    get
        //    {
        //        return _bin23UpdateButtonEnable;
        //    }
        //    set
        //    {
        //        _bin23UpdateButtonEnable = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("bin23UpdateButtonEnable"));
        //    }
        //}

        //private bool _bin24UpdateButtonEnable;
        //public bool bin24UpdateButtonEnable
        //{
        //    get
        //    {
        //        return _bin24UpdateButtonEnable;
        //    }
        //    set
        //    {
        //        _bin24UpdateButtonEnable = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("bin24UpdateButtonEnable"));
        //    }
        //}

        private bool _bin19UpdateButtonEnable;
        public bool bin19UpdateButtonEnable
        {
            get
            {
                return _bin19UpdateButtonEnable;
            }
            set
            {
                _bin19UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bin19UpdateButtonEnable"));
            }
        }

        private bool _bin20UpdateButtonEnable;
        public bool bin20UpdateButtonEnable
        {
            get
            {
                return _bin20UpdateButtonEnable;
            }
            set
            {
                _bin20UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bin20UpdateButtonEnable"));
            }
        }

        private bool _bin21UpdateButtonEnable;
        public bool bin21UpdateButtonEnable
        {
            get
            {
                return _bin21UpdateButtonEnable;
            }
            set
            {
                _bin21UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bin21UpdateButtonEnable"));
            }
        }

        private bool _bin22UpdateButtonEnable;
        public bool bin22UpdateButtonEnable
        {
            get
            {
                return _bin22UpdateButtonEnable;
            }
            set
            {
                _bin22UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bin22UpdateButtonEnable"));
            }
        }

        private bool _bin201UpdateButtonEnable;
        public bool bin201UpdateButtonEnable
        {
            get
            {
                return _bin201UpdateButtonEnable;
            }
            set
            {
                _bin201UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bin201UpdateButtonEnable"));
            }
        }

        private bool _bin202UpdateButtonEnable;
        public bool bin202UpdateButtonEnable
        {
            get
            {
                return _bin202UpdateButtonEnable;
            }
            set
            {
                _bin202UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bin202UpdateButtonEnable"));
            }
        }

        private bool _bin203UpdateButtonEnable;
        public bool bin203UpdateButtonEnable
        {
            get
            {
                return _bin203UpdateButtonEnable;
            }
            set
            {
                _bin203UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bin203UpdateButtonEnable"));
            }
        }

        private bool _bin401UpdateButtonEnable;
        public bool bin401UpdateButtonEnable
        {
            get
            {
                return _bin401UpdateButtonEnable;
            }
            set
            {
                _bin401UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bin401UpdateButtonEnable"));
            }
        }

        private bool _bin402UpdateButtonEnable;
        public bool bin402UpdateButtonEnable
        {
            get
            {
                return _bin402UpdateButtonEnable;
            }
            set
            {
                _bin402UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bin402UpdateButtonEnable"));
            }
        }

        private bool _bin403UpdateButtonEnable;
        public bool bin403UpdateButtonEnable
        {
            get
            {
                return _bin403UpdateButtonEnable;
            }
            set
            {
                _bin403UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bin403UpdateButtonEnable"));
            }
        }

        private bool _bin404UpdateButtonEnable;
        public bool bin404UpdateButtonEnable
        {
            get
            {
                return _bin404UpdateButtonEnable;
            }
            set
            {
                _bin404UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bin404UpdateButtonEnable"));
            }
        }

        private bool _bin405UpdateButtonEnable;
        public bool bin405UpdateButtonEnable
        {
            get
            {
                return _bin405UpdateButtonEnable;
            }
            set
            {
                _bin405UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bin405UpdateButtonEnable"));
            }
        }

        private bool _bin406UpdateButtonEnable;
        public bool bin406UpdateButtonEnable
        {
            get
            {
                return _bin406UpdateButtonEnable;
            }
            set
            {
                _bin406UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("_bin406UpdateButtonEnable"));
            }
        }

        private bool _bin407UpdateButtonEnable;
        public bool bin407UpdateButtonEnable
        {
            get
            {
                return _bin407UpdateButtonEnable;
            }
            set
            {
                _bin407UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bin407UpdateButtonEnable"));
            }
        }

        private bool _bin411UpdateButtonEnable;
        public bool bin411UpdateButtonEnable
        {
            get
            {
                return _bin411UpdateButtonEnable;
            }
            set
            {
                _bin411UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bin411UpdateButtonEnable"));
            }
        }

        private bool _bin501UpdateButtonEnable;
        public bool bin501UpdateButtonEnable
        {
            get
            {
                return _bin501UpdateButtonEnable;
            }
            set
            {
                _bin501UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bin501UpdateButtonEnable"));
            }
        }

        private bool _bin502UpdateButtonEnable;
        public bool bin502UpdateButtonEnable
        {
            get
            {
                return _bin502UpdateButtonEnable;
            }
            set
            {
                _bin502UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bin502UpdateButtonEnable"));
            }
        }

        private bool _bin503UpdateButtonEnable;
        public bool bin503UpdateButtonEnable
        {
            get
            {
                return _bin503UpdateButtonEnable;
            }
            set
            {
                _bin503UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bin503UpdateButtonEnable"));
            }
        }

        private bool _bin504UpdateButtonEnable;
        public bool bin504UpdateButtonEnable
        {
            get
            {
                return _bin504UpdateButtonEnable;
            }
            set
            {
                _bin504UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bin504UpdateButtonEnable"));
            }
        }

        private bool _bin601UpdateButtonEnable;
        public bool bin601UpdateButtonEnable
        {
            get
            {
                return _bin601UpdateButtonEnable;
            }
            set
            {
                _bin601UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bin601UpdateButtonEnable"));
            }
        }

        private bool _bin602UpdateButtonEnable;
        public bool bin602UpdateButtonEnable
        {
            get
            {
                return _bin602UpdateButtonEnable;
            }
            set
            {
                _bin602UpdateButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("bin602UpdateButtonEnable"));
            }
        }

        #endregion


        #region Click Command Implementations

        //public void bin21ProductUpdateClickCommandImplementation()
        //{
        //    ClearUpdateResultMessageAndVisibility();

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(sqlConn))
        //        {
        //            conn.Open();

        //            // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

        //            SqlCommand cmd = conn.CreateCommand();
        //            cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(1," + SelectedProductBin21.ProductID + ",'" + DateTime.Now + "')";
        //            int result = cmd.ExecuteNonQuery();

        //            if (result > 0)
        //            {
        //                Bin21ProductName = SelectedProductBin21.ProductName;
        //                Bin21ProductNameAbbreviation = SelectedProductBin21.ProductAbbreviation;
        //                UpdateSuccessfullMessageDisplay();
                        
        //            }
        //            else
        //            {
        //                ClearUpdateResultMessageAndVisibility();
        //            }


        //            conn.Close();
        //        }
        //    }
        //    catch (NullReferenceException)
        //    {
        //        UpdateResultNoProductSelected();
                
        //    }
        //    catch (Exception ae)
        //    {
        //        UpdateFailedMessageDisplay();
                
        //    }
        //}

        //public void bin22ProductUpdateClickCommandImplementation()
        //{
        //    ClearUpdateResultMessageAndVisibility();

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(sqlConn))
        //        {
        //            conn.Open();

        //            // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

        //            SqlCommand cmd = conn.CreateCommand();
        //            cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(2," + SelectedProductBin22.ProductID + ",'" + DateTime.Now + "')";
        //            int result = cmd.ExecuteNonQuery();

        //            if (result > 0)
        //            {
        //                Bin22ProductName = SelectedProductBin22.ProductName;
        //                Bin22ProductNameAbbreviation = SelectedProductBin22.ProductAbbreviation;
        //                UpdateSuccessfullMessageDisplay();
                        
        //            }
        //            else
        //            {
        //                ClearUpdateResultMessageAndVisibility();
        //            }

        //            conn.Close();
        //        }
        //    }
        //    catch (NullReferenceException)
        //    {
        //        UpdateResultNoProductSelected();
                
        //    }
        //    catch (Exception ae)
        //    {
        //        UpdateFailedMessageDisplay();
                
        //    }
        //}

        //public void bin23ProductUpdateClickCommandImplementation()
        //{
        //    ClearUpdateResultMessageAndVisibility();

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(sqlConn))
        //        {
        //            conn.Open();

        //            // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

        //            SqlCommand cmd = conn.CreateCommand();
        //            cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(3," + SelectedProductBin23.ProductID + ",'" + DateTime.Now + "')";
        //            int result = cmd.ExecuteNonQuery();

        //            if (result > 0)
        //            {
        //                Bin23ProductName = SelectedProductBin23.ProductName;
        //                Bin23ProductNameAbbreviation = SelectedProductBin23.ProductAbbreviation;
        //                UpdateSuccessfullMessageDisplay();
                        
        //            }
        //            else
        //            {
        //                ClearUpdateResultMessageAndVisibility();
        //            }

        //            conn.Close();
        //        }
        //    }
        //    catch (NullReferenceException)
        //    {
        //        UpdateResultNoProductSelected();
                
        //    }
        //    catch (Exception ae)
        //    {
        //        UpdateFailedMessageDisplay();
                
        //    }
        //}

        //public void bin24ProductUpdateClickCommandImplementation()
        //{
        //    ClearUpdateResultMessageAndVisibility();
            
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(sqlConn))
        //        {
        //            conn.Open();

        //            // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

        //            SqlCommand cmd = conn.CreateCommand();
        //            cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(4," + SelectedProductBin24.ProductID + ",'" + DateTime.Now + "')";
        //            int result = cmd.ExecuteNonQuery();

        //            if (result > 0)
        //            {
        //                Bin24ProductName = SelectedProductBin24.ProductName;
        //                Bin24ProductNameAbbreviation = SelectedProductBin24.ProductAbbreviation;
        //                UpdateSuccessfullMessageDisplay();
                        
        //            }
        //            else
        //            {
        //                ClearUpdateResultMessageAndVisibility();
        //            }

        //            conn.Close();
        //        }
        //    }
        //    catch (NullReferenceException)
        //    {
        //        UpdateResultNoProductSelected();
                
        //    }
        //    catch (Exception ae)
        //    {
        //        UpdateFailedMessageDisplay();
                
        //    }
        //}

        //public void bin201ProductUpdateClickCommandImplementation()
        //{
        //    ClearUpdateResultMessageAndVisibility();

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(sqlConn))
        //        {
        //            conn.Open();

        //            // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

        //            SqlCommand cmd = conn.CreateCommand();
        //            cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(5," + SelectedProductBin201.ProductID + ",'" + DateTime.Now + "')";
        //            int result = cmd.ExecuteNonQuery();

        //            if (result > 0)
        //            {
        //                Bin201ProductName = SelectedProductBin201.ProductName;
        //                Bin201ProductNameAbbreviation = SelectedProductBin201.ProductAbbreviation;
        //                UpdateSuccessfullMessageDisplay();
                        
        //            }
        //            else
        //            {
        //                ClearUpdateResultMessageAndVisibility();
        //            }


        //            conn.Close();
        //        }
        //    }
        //    catch (NullReferenceException)
        //    {
        //        UpdateResultNoProductSelected();
                
        //    }
        //    catch (Exception ae)
        //    {
        //        UpdateFailedMessageDisplay();
                
        //    }
        //}

        //public void bin202ProductUpdateClickCommandImplementation()
        //{
        //    ClearUpdateResultMessageAndVisibility();

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(sqlConn))
        //        {
        //            conn.Open();

        //            // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

        //            SqlCommand cmd = conn.CreateCommand();
        //            cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(6," + SelectedProductBin202.ProductID + ",'" + DateTime.Now + "')";
        //            int result = cmd.ExecuteNonQuery();

        //            if (result > 0)
        //            {
        //                Bin202ProductName = SelectedProductBin202.ProductName;
        //                Bin202ProductNameAbbreviation = SelectedProductBin202.ProductAbbreviation;
        //                UpdateSuccessfullMessageDisplay();
                        
        //            }
        //            else
        //            {
        //                ClearUpdateResultMessageAndVisibility();
        //            }


        //            conn.Close();
        //        }
        //    }
        //    catch (NullReferenceException)
        //    {
        //        UpdateResultNoProductSelected();
                
        //    }
        //    catch (Exception ae)
        //    {
        //        UpdateFailedMessageDisplay();
                
        //    }
        //}

        //public void bin203ProductUpdateClickCommandImplementation()
        //{
        //    ClearUpdateResultMessageAndVisibility();

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(sqlConn))
        //        {
        //            conn.Open();

        //            // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

        //            SqlCommand cmd = conn.CreateCommand();
        //            cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(7," + SelectedProductBin203.ProductID + ",'" + DateTime.Now + "')";
        //            int result = cmd.ExecuteNonQuery();

        //            if (result > 0)
        //            {
        //                Bin203ProductName = SelectedProductBin203.ProductName;
        //                Bin203ProductNameAbbreviation = SelectedProductBin203.ProductAbbreviation;
        //                UpdateSuccessfullMessageDisplay();
                        
        //            }
        //            else
        //            {
        //                ClearUpdateResultMessageAndVisibility();
        //            }

        //            conn.Close();
        //        }
        //    }
        //    catch (NullReferenceException)
        //    {
        //        UpdateResultNoProductSelected();
                
        //    }
        //    catch (Exception ae)
        //    {
        //        UpdateFailedMessageDisplay();
                
        //    }
        //}

        public void bin19ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    //cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(1," + SelectedProductBin19.ProductID + ",'" + DateTime.Now + "')";
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(1," + SelectedProductBin19.ProductID + ",'" + DateTime.Now + "')";
                    
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin19ProductName = SelectedProductBin19.ProductName;
                        Bin19ProductNameAbbreviation = SelectedProductBin19.ProductAbbreviation;
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

        public void bin20ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(2," + SelectedProductBin20.ProductID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin20ProductName = SelectedProductBin20.ProductName;
                        Bin20ProductNameAbbreviation = SelectedProductBin20.ProductAbbreviation;
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

        public void bin21ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(3," + SelectedProductBin21.ProductID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin21ProductName = SelectedProductBin21.ProductName;
                        Bin21ProductNameAbbreviation = SelectedProductBin21.ProductAbbreviation;
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

        public void bin22ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(4," + SelectedProductBin22.ProductID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin22ProductName = SelectedProductBin22.ProductName;
                        Bin22ProductNameAbbreviation = SelectedProductBin22.ProductAbbreviation;
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

        public void bin201ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(5," + SelectedProductBin201.ProductID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin201ProductName = SelectedProductBin201.ProductName;
                        Bin201ProductNameAbbreviation = SelectedProductBin201.ProductAbbreviation;
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

        public void bin202ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(6," + SelectedProductBin202.ProductID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin202ProductName = SelectedProductBin202.ProductName;
                        Bin202ProductNameAbbreviation = SelectedProductBin202.ProductAbbreviation;
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

        public void bin203ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(7," + SelectedProductBin203.ProductID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin203ProductName = SelectedProductBin203.ProductName;
                        Bin203ProductNameAbbreviation = SelectedProductBin203.ProductAbbreviation;
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

        public void bin401ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(8," + SelectedProductBin401.ProductID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin401ProductName = SelectedProductBin401.ProductName;
                        Bin401ProductNameAbbreviation = SelectedProductBin401.ProductAbbreviation;
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

        public void bin402ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(9," + SelectedProductBin402.ProductID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin402ProductName = SelectedProductBin402.ProductName;
                        Bin402ProductNameAbbreviation = SelectedProductBin402.ProductAbbreviation;
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

        public void bin403ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(10," + SelectedProductBin403.ProductID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin403ProductName = SelectedProductBin403.ProductName;
                        Bin403ProductNameAbbreviation = SelectedProductBin403.ProductAbbreviation;
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

        public void bin404ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(11," + SelectedProductBin404.ProductID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin404ProductName = SelectedProductBin404.ProductName;
                        Bin404ProductNameAbbreviation = SelectedProductBin404.ProductAbbreviation;
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

        public void bin405ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(12," + SelectedProductBin405.ProductID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin405ProductName = SelectedProductBin405.ProductName;
                        Bin405ProductNameAbbreviation = SelectedProductBin405.ProductAbbreviation;
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

        public void bin406ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(13," + SelectedProductBin406.ProductID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin406ProductName = SelectedProductBin406.ProductName;
                        Bin406ProductNameAbbreviation = SelectedProductBin406.ProductAbbreviation;
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

        public void bin407ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(14," + SelectedProductBin407.ProductID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin407ProductName = SelectedProductBin407.ProductName;
                        Bin407ProductNameAbbreviation = SelectedProductBin407.ProductAbbreviation;
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

        public void bin411ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(15," + SelectedProductBin411.ProductID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin411ProductName = SelectedProductBin411.ProductName;
                        Bin411ProductNameAbbreviation = SelectedProductBin411.ProductAbbreviation;
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

        public void bin501ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(16," + SelectedProductBin501.ProductID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin501ProductName = SelectedProductBin501.ProductName;
                        Bin501ProductNameAbbreviation = SelectedProductBin501.ProductAbbreviation;
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

        public void bin502ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(17," + SelectedProductBin502.ProductID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin502ProductName = SelectedProductBin502.ProductName;
                        Bin502ProductNameAbbreviation = SelectedProductBin502.ProductAbbreviation;
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

        public void bin503ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(18," + SelectedProductBin503.ProductID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin503ProductName = SelectedProductBin503.ProductName;
                        Bin503ProductNameAbbreviation = SelectedProductBin503.ProductAbbreviation;
                        UpdateSuccessfullMessageDisplay();
                        
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

        public void bin504ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(19," + SelectedProductBin504.ProductID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin504ProductName = SelectedProductBin504.ProductName;
                        Bin504ProductNameAbbreviation = SelectedProductBin504.ProductAbbreviation;
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

        public void bin601ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();
            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(20," + SelectedProductBin601.ProductID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin601ProductName = SelectedProductBin601.ProductName;
                        Bin601ProductNameAbbreviation = SelectedProductBin601.ProductAbbreviation;
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

        public void bin602ProductUpdateClickCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();

                    // MicroIngredientModel mim = (MicroIngredientModel)cmbBxMicroIngredient1Components.SelectedItem;

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO BinProductChangeLog VALUES(21," + SelectedProductBin602.ProductID + ",'" + DateTime.Now + "')";
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Bin602ProductName = SelectedProductBin602.ProductName;
                        Bin602ProductNameAbbreviation = SelectedProductBin602.ProductAbbreviation;
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

        public void addNewProductClickCommandImplementation()
        {
            if (NewProductName != null)
            {
                if (NewProductAbbreviation != null)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(sqlConn))
                        {
                            conn.Open();

                            SqlCommand cmd = new SqlCommand("INSERT INTO Products (ProductName, ProductAbbreviation, ProductType) VALUES('" + NewProductName + "', '" + NewProductAbbreviation + "', 1)", conn);
                            int result = cmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                                if (Products.Count > 1)
                                {
                                    Products.Clear();
                                }
                                LoadAllProducts();
                                UpdateResultNewProductAddedSuccesfully();
                            }
                            conn.Close();
                        }
                    }
                    catch (SqlException se)
                    {
                        if (se.Number == 2627)
                        {
                            UpdateResultDuplicateProduct();
                            
                        }
                    }
                    catch (Exception ae)
                    {
                        UpdateResultProductNotAdded();
                        
                    }
                }
                else
                {
                    UpdateResultNoProductAbbreviation();
                    
                }
            }
            else
            {
                UpdateResultNoNewProductName();
                
            }
        }

        public void binProductManagementCloseCommandImplementation()
        {
            ClearUpdateResultMessageAndVisibility();
            
        }

        #endregion


        #region Selected Product Product Models

        //private ProductsModel _SelectedProductBin21;
        //public ProductsModel SelectedProductBin21
        //{
        //    get { return _SelectedProductBin21; }
        //    set
        //    {
        //        _SelectedProductBin21 = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin21"));
        //    }
        //}

        //private ProductsModel _SelectedProductBin22;
        //public ProductsModel SelectedProductBin22
        //{
        //    get { return _SelectedProductBin22; }
        //    set
        //    {
        //        _SelectedProductBin22 = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin22"));
        //    }
        //}

        //private ProductsModel _SelectedProductBin23;
        //public ProductsModel SelectedProductBin23
        //{
        //    get { return _SelectedProductBin23; }
        //    set
        //    {
        //        _SelectedProductBin23 = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin23"));
        //    }
        //}

        //private ProductsModel _SelectedProductBin24;
        //public ProductsModel SelectedProductBin24
        //{
        //    get { return _SelectedProductBin24; }
        //    set
        //    {
        //        _SelectedProductBin24 = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin24"));
        //    }
        //}

        private ProductsModel _SelectedProductBin19;
        public ProductsModel SelectedProductBin19
        {
            get { return _SelectedProductBin19; }
            set
            {
                _SelectedProductBin19 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin19"));
            }
        }

        private ProductsModel _SelectedProductBin20;
        public ProductsModel SelectedProductBin20
        {
            get { return _SelectedProductBin20; }
            set
            {
                _SelectedProductBin20 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin20"));
            }
        }

        private ProductsModel _SelectedProductBin21;
        public ProductsModel SelectedProductBin21
        {
            get { return _SelectedProductBin21; }
            set
            {
                _SelectedProductBin21 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin21"));
            }
        }

        private ProductsModel _SelectedProductBin22;
        public ProductsModel SelectedProductBin22
        {
            get { return _SelectedProductBin22; }
            set
            {
                _SelectedProductBin22 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin22"));
            }
        }


        private ProductsModel _SelectedProductBin201;
        public ProductsModel SelectedProductBin201
        {
            get { return _SelectedProductBin201; }
            set
            {
                _SelectedProductBin201 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin201"));
            }
        }

        private ProductsModel _SelectedProductBin202;
        public ProductsModel SelectedProductBin202
        {
            get { return _SelectedProductBin202; }
            set
            {
                _SelectedProductBin202 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin202"));
            }
        }

        private ProductsModel _SelectedProductBin203;
        public ProductsModel SelectedProductBin203
        {
            get { return _SelectedProductBin203; }
            set
            {
                _SelectedProductBin203 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin203"));
            }
        }

        private ProductsModel _SelectedProductBin401;
        public ProductsModel SelectedProductBin401
        {
            get { return _SelectedProductBin401; }
            set
            {
                _SelectedProductBin401 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin401"));
            }
        }

        private ProductsModel _SelectedProductBin402;
        public ProductsModel SelectedProductBin402
        {
            get { return _SelectedProductBin402; }
            set
            {
                _SelectedProductBin402 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin402"));
            }
        }

        private ProductsModel _SelectedProductBin403;
        public ProductsModel SelectedProductBin403
        {
            get { return _SelectedProductBin403; }
            set
            {
                _SelectedProductBin403 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin403"));
            }
        }

        private ProductsModel _SelectedProductBin404;
        public ProductsModel SelectedProductBin404
        {
            get { return _SelectedProductBin404; }
            set
            {
                _SelectedProductBin404 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin404"));
            }
        }

        private ProductsModel _SelectedProductBin405;
        public ProductsModel SelectedProductBin405
        {
            get { return _SelectedProductBin405; }
            set
            {
                _SelectedProductBin405 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin405"));
            }
        }

        private ProductsModel _SelectedProductBin406;
        public ProductsModel SelectedProductBin406
        {
            get { return _SelectedProductBin406; }
            set
            {
                _SelectedProductBin406 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin406"));
            }
        }

        private ProductsModel _SelectedProductBin407;
        public ProductsModel SelectedProductBin407
        {
            get { return _SelectedProductBin407; }
            set
            {
                _SelectedProductBin407 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin407"));
            }
        }

        private ProductsModel _SelectedProductBin411;
        public ProductsModel SelectedProductBin411
        {
            get { return _SelectedProductBin411; }
            set
            {
                _SelectedProductBin411 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin411"));
            }
        }

        private ProductsModel _SelectedProductBin501;
        public ProductsModel SelectedProductBin501
        {
            get { return _SelectedProductBin501; }
            set
            {
                _SelectedProductBin501 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin501"));
            }
        }

        private ProductsModel _SelectedProductBin502;
        public ProductsModel SelectedProductBin502
        {
            get { return _SelectedProductBin502; }
            set
            {
                _SelectedProductBin502 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin502"));
            }
        }

        private ProductsModel _SelectedProductBin503;
        public ProductsModel SelectedProductBin503
        {
            get { return _SelectedProductBin503; }
            set
            {
                _SelectedProductBin503 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin503"));
            }
        }

        private ProductsModel _SelectedProductBin504;
        public ProductsModel SelectedProductBin504
        {
            get { return _SelectedProductBin504; }
            set
            {
                _SelectedProductBin504 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin504"));
            }
        }

        private ProductsModel _SelectedProductBin601;
        public ProductsModel SelectedProductBin601
        {
            get { return _SelectedProductBin601; }
            set
            {
                _SelectedProductBin601 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin601"));
            }
        }

        private ProductsModel _SelectedProductBin602;
        public ProductsModel SelectedProductBin602
        {
            get { return _SelectedProductBin602; }
            set
            {
                _SelectedProductBin602 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductBin602"));
            }
        }

        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }



        private void UpdateResultNoNewProductName()
        {
            UpdateResult = "Please enter a product name and try again.";
            UpdateResultColor = Colors.Red;
            UpdateMessageFailureImageVisibility = Visibility.Visible;
            UpdateMessageVisibility = Visibility.Visible;
            timerHideUpdateMessage.Start();
        }

        private void UpdateResultNoProductAbbreviation()
        {
            UpdateResult = "Please enter product abbreviation!";
            UpdateResultColor = Colors.Red;
            UpdateMessageFailureImageVisibility = Visibility.Visible;
            UpdateMessageVisibility = Visibility.Visible;
            timerHideUpdateMessage.Start();
        }

        private void UpdateResultProductNotAdded()
        {
            UpdateResult = "Failed to insert new product!";
            UpdateResultColor = Colors.Red;
            UpdateMessageFailureImageVisibility = Visibility.Visible;
            UpdateMessageVisibility = Visibility.Visible;
            timerHideUpdateMessage.Start();
        }

        private void UpdateResultNoProductSelected()
        {
            UpdateResult = "Please select a product and try again.";
            UpdateResultColor = Colors.Red;
            UpdateMessageFailureImageVisibility = Visibility.Visible;
            UpdateMessageVisibility = Visibility.Visible;
            timerHideUpdateMessage.Start();
        }

        private void UpdateResultDuplicateProduct()
        {
            UpdateResult = "The product entered already exists!";
            UpdateResultColor = Colors.Red;
            UpdateMessageFailureImageVisibility = Visibility.Visible;
            UpdateMessageVisibility = Visibility.Visible;
            timerHideUpdateMessage.Start();
        }

        private void UpdateSuccessfullMessageDisplay()
        {
            UpdateResult = "Bin updated successfully!";
            UpdateResultColor = Colors.LimeGreen;
            UpdateMessageSuccessImageVisibility = Visibility.Visible;
            UpdateMessageVisibility = Visibility.Visible;
            timerHideUpdateMessage.Start();
        }

        private void UpdateResultNewProductAddedSuccesfully()
        {
            UpdateResult = "New product added successfully!";
            UpdateResultColor = Colors.LimeGreen;
            UpdateMessageSuccessImageVisibility = Visibility.Visible;
            UpdateMessageVisibility = Visibility.Visible;
            timerHideUpdateMessage.Start();
        }

        private void UpdateFailedMessageDisplay()
        {
            UpdateResult = "Update Failed. Please try again.";
            UpdateResultColor = Colors.Red;
            UpdateMessageFailureImageVisibility = Visibility.Visible;
            UpdateMessageVisibility = Visibility.Visible;
            timerHideUpdateMessage.Start();
        }

        private void ClearUpdateResultMessageAndVisibility()
        {
            UpdateMessageVisibility = Visibility.Hidden;
            UpdateResult = "";
            UpdateResultColor = Colors.Transparent;
            UpdateMessageFailureImageVisibility = Visibility.Hidden;
            UpdateMessageSuccessImageVisibility = Visibility.Hidden;
        }


        /// <summary>
        /// This method enables the update button for the bin if the status of the bin is empty based on the value of the WORD in the plc
        /// </summary>
        public void UpdateBinUpdateButtonEnabeledState(long binStatusWordValue)
        {
            //int iIndex1 = 0;

            //Status DWord [4 Bytes, Ingear Word is read, 1 Length]
            string t = Convert.ToString(binStatusWordValue, 2);  //Convert to a Word in Binary (16 Bits)
            string ttemp = t;
            for (int i = t.Length; i < 32; i++)                     //Fill the binary number with zero's from the front of the number to ensure the length is 16 bits(WORD)            
                ttemp = "0" + ttemp;

            char[] binaryValuesW1 = ttemp.ToCharArray();
            char[] bitSwap = new char[32];

            char[] binByte1 = new char[8];
            char[] binByte2 = new char[8];
            char[] binByte3 = new char[8];
            char[] binByte4 = new char[8];
            Array.Copy(binaryValuesW1, 0, binByte1, 0, 8);
            Array.Copy(binaryValuesW1, 8, binByte2, 0, 8);
            Array.Copy(binaryValuesW1, 16, binByte3, 0, 8);
            Array.Copy(binaryValuesW1, 24, binByte4, 0, 8);

            Array.Reverse(binByte1);
            Array.Reverse(binByte2);
            Array.Reverse(binByte3);
            Array.Reverse(binByte4);

            for (int i = 0; i < 8; i++)
                binaryValuesW1[i] = binByte1[i];

            int j = 0;
            for (int i = 8; i < 16; i++)
                binaryValuesW1[i] = binByte2[j++];

            j = 0;
            for (int i = 16; i < 24; i++)
                binaryValuesW1[i] = binByte3[j++];

            j = 0;
            for (int i = 24; i < 32; i++)
                binaryValuesW1[i] = binByte4[j++];


            string sSwappedBinaryValue = "";
            for (int i = 0; i < binaryValuesW1.Length; i++)
                sSwappedBinaryValue += binaryValuesW1[i];

            //foreach (char c in sSwappedBinaryValue)
            //{

            //    //bool currentBin = binButtonEnable[iIndex1];
            //    int bitValue = (Convert.ToInt32(c.ToString()));

            //    binButtonEnable[iIndex1] = Convert.ToBoolean(bitValue);
            //    //cb.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            //    //{
            //    //    cb.IsChecked = Convert.ToBoolean(bitValue);

            //    //    if (cb.IsChecked == true)
            //    //    {
            //    //        cb.Foreground = Brushes.Green;
            //    //        cb.FontWeight = FontWeights.DemiBold;
            //    //        cb.FontSize = 12;
            //    //    }
            //    //    else
            //    //    {
            //    //        cb.Foreground = Brushes.Black;
            //    //        cb.FontWeight = FontWeights.Normal;
            //    //        cb.FontSize = 10;
            //    //    }
            //    //    cb.IsEnabled = false;

            //    ////}));
            //    iIndex1++;
            //}

            

            char[] bits = sSwappedBinaryValue.ToCharArray();

            //bin21UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[0].ToString()));
            //bin22UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[1].ToString()));
            //bin23UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[2].ToString()));
            //bin24UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[3].ToString()));
            bin19UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[0].ToString()));
            bin20UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[1].ToString()));
            bin21UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[2].ToString()));
            bin22UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[3].ToString()));
            bin201UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[4].ToString()));
            bin202UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[5].ToString()));
            bin203UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[6].ToString()));
            bin401UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[12].ToString()));
            bin402UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[13].ToString()));
            bin403UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[14].ToString()));
            bin404UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[15].ToString()));
            bin405UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[16].ToString()));
            bin406UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[17].ToString()));
            bin407UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[18].ToString()));
            bin411UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[19].ToString()));
            bin501UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[20].ToString()));
            bin502UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[21].ToString()));
            bin503UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[22].ToString()));
            bin504UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[23].ToString()));
            bin601UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[24].ToString()));
            bin602UpdateButtonEnable = Convert.ToBoolean(int.Parse(bits[25].ToString()));
        }

        private long _BinStatusWordValue;
        public long BinStatusWordValue
        {
            get
            {
                return _BinStatusWordValue;
            }
            set
            {
                _BinStatusWordValue = value;
                UpdateBinUpdateButtonEnabeledState(BinStatusWordValue);
            }
        }
    }
}
