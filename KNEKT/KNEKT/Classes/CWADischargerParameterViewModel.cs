using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Input;
using S7Link;
using System.Reflection;
using KNEKT.Classes.MessageBoxClasses;
using System.Windows;
using System.Windows.Media;

namespace KNEKT.Classes
{
    public class CWADischargerParameterViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public Hashtable htParameters = new Hashtable();
        public DelegateCommand cmdSaveParameters;
        public DelegateCommand cmdDownloadParameters;

        //----------------------------------------------------------------------//
        //                               Constructor                            //
        //----------------------------------------------------------------------//
        public CWADischargerParameterViewModel(int scaleNumber, string ScaleDBnumber, string SwitchOverWeightDBoffset, Controller ReadCont, Controller WriteCont)
        {
            ScaleNumber = scaleNumber;
            ScaleDBNumber = ScaleDBnumber;
            SwitchOverWeightDBOffset = SwitchOverWeightDBoffset;
            PLCR = ReadCont;
            PLCW = WriteCont;
            cmdSaveParameters = new DelegateCommand(cmdSaveParametersImplementation);
            cmdDownloadParameters = new DelegateCommand(cmdDownloadParametersImplementation);

            GetSavedParameters();
            UpdateProperties();
            ReadParameterValuesFromPLC();
        }

        public string sqlConnection = MainWindow.SqlConnectionString;//"Data Source=JHBY03\\SQLEXPRESS;Initial Catalog=KNEKT_AFGRI;User Id=sa;Password=SQLpassword1234;Connection Timeout=10;";


        //----------------------------------------------------------------------//
        //                                Methods                               //
        //----------------------------------------------------------------------//

        private void GetSavedParameters()
        {
            try
            {
                htParameters.Clear();

                using (SqlConnection sqlConn = new SqlConnection(sqlConnection))
                {
                    sqlConn.Open();
                    SqlCommand cmd = sqlConn.CreateCommand();
                    cmd.CommandText = "Select * from ParameterSettings Where Revision = " + ScaleNumber;
                    SqlDataReader reader = cmd.ExecuteReader();
                    bool bHasRows = false;

                    while (reader.Read())
                    {
                        htParameters.Add(reader["Name"],
                                            new ParameterSetting()
                                            {
                                                RevisionID = Convert.ToInt32(reader["Revision"]),
                                                ParameterID = Convert.ToInt32(reader["ParameterID"]),
                                                ParameterName = reader["Name"].ToString(),
                                                DBOffset = reader["DBOffset"].ToString(),
                                                Value = reader["Value"].ToString(),
                                                DateSaved = Convert.ToDateTime(reader["DateSaved"])
                                            });
                        bHasRows = true;
                    }

                    if (!bHasRows)
                    {
                        SwitchOverWeight = 100;
                        CutOffWeight = 0;
                        TolerancePositiveWeight = 0;
                        ToleranceNegativeWeight = 0;
                        CutOffWeightCorrectionMax = 0;
                        DosingTimeSlow = 0;
                        SwitchOverTimeFastToSlow = 0;
                        TipPulseLength = 0;
                        DosingTimeMax = 0;
                        TolerancePositiveProportion = 0;
                        ToleranceNegativeProportion = 0;
                        AutoFaultResetWeight = 0;
                        DosingSpeedFast = 0;
                        DosingSpeedSlow = 0;
                        FineDosingWeight = 0;
                    }

                    sqlConn.Close();
                }

            }
            catch (Exception ex)
            {
                var msgBox = base.GetService<IMessageBoxService>();
                if (msgBox != null)
                    msgBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateProperties()
        {
            //Loop through ht and set properties
            foreach (object key in htParameters.Keys)
            {
                var ps = (ParameterSetting)htParameters[key];

                object objDataContext = this;
                PropertyInfo[] piProperties = objDataContext.GetType().GetProperties();

                foreach (PropertyInfo pi in piProperties)
                {
                    if (pi.Name == key.ToString())
                        pi.SetValue(objDataContext, Convert.ToDouble(ps.Value), null);
                }
            }

            var ps1 = (ParameterSetting)htParameters["SwitchOverWeight"];
            if(ps1 != null)
            SwitchOverWeight = Convert.ToDouble(ps1.Value) / 1000;

            var ps2 = (ParameterSetting)htParameters["CutOffWeight"];
            if (ps2 != null)
            CutOffWeight = Convert.ToDouble(ps2.Value) / 1000;

            var ps3 = (ParameterSetting)htParameters["TolerancePositiveWeight"];
            if (ps3 != null)
            TolerancePositiveWeight = Convert.ToDouble(ps3.Value) / 1000;

            var ps4 = (ParameterSetting)htParameters["ToleranceNegativeWeight"];
            if (ps4 != null)
            ToleranceNegativeWeight = Convert.ToDouble(ps4.Value) / 1000;

            var ps5 = (ParameterSetting)htParameters["CutOffWeightCorrectionMax"];
            if (ps5 != null)
            CutOffWeightCorrectionMax = Convert.ToDouble(ps5.Value) / 1000;

            var ps6 = (ParameterSetting)htParameters["DosingTimeSlow"];
            if (ps6 != null)
            DosingTimeSlow = Convert.ToDouble(ps6.Value) / 10;

            var ps7 = (ParameterSetting)htParameters["SwitchOverTimeFastToSlow"];
            if (ps7 != null)
            SwitchOverTimeFastToSlow = Convert.ToDouble(ps7.Value) / 10;

            var ps8 = (ParameterSetting)htParameters["TipPulseLength"];
            if (ps8 != null)
            TipPulseLength = Convert.ToDouble(ps8.Value) / 10;

            var ps9 = (ParameterSetting)htParameters["DosingTimeMax"];
            if (ps9 != null)
            DosingTimeMax = Convert.ToDouble(ps9.Value) / 10;

            var ps10 = (ParameterSetting)htParameters["TolerancePositiveProportion"];
            if (ps10 != null)
            TolerancePositiveProportion = Convert.ToDouble(ps10.Value);

            var ps11 = (ParameterSetting)htParameters["ToleranceNegativeProportion"];
            if (ps11 != null)
            ToleranceNegativeProportion = Convert.ToDouble(ps11.Value);

            var ps12 = (ParameterSetting)htParameters["AutoFaultResetWeight"];
            if (ps12 != null)
            AutoFaultResetWeight = Convert.ToDouble(ps12.Value) / 1000;

            var ps13 = (ParameterSetting)htParameters["AutoFaultResetPercent"];
            if (ps13 != null)
            AutoFaultResetPercent = Convert.ToDouble(ps13.Value);

            var ps14 = (ParameterSetting)htParameters["DosingSpeedFast"];
            if (ps14 != null)
            DosingSpeedFast = Convert.ToDouble(ps14.Value);

            var ps15 = (ParameterSetting)htParameters["DosingSpeedSlow"];
            if (ps15 != null)
            DosingSpeedSlow = Convert.ToDouble(ps15.Value);

            var ps16 = (ParameterSetting)htParameters["FineDosingWeight"];
            if (ps16 != null)
            FineDosingWeight = Convert.ToDouble(ps16.Value) / 1000;
        }

        private void ReadParameterValuesFromPLC()
        {
            try
            {
                //Calculate what DB Offsets to Read From
                int iStart = SwitchOverWeightDBOffset.LastIndexOf('D');
                int iLength = SwitchOverWeightDBOffset.Length;
                string s = (SwitchOverWeightDBOffset.Substring(iStart + 1, iLength - iStart - 1));
                int i1 = Convert.ToInt32(s);
                int i2 = i1 + 4;
                int i3 = i2 + 4;
                int i4 = i3 + 4;
                int i5 = i4 + 4;
                int i6 = i5 + 4;
                int i7 = i6 + 2;
                int i8 = i7 + 2;
                int i9 = i8 + 2;
                int i10 = i9 + 2;
                int i11 = i10 + 2;
                int i12 = i11 + 2;
                int i13 = i12 + 4;
                int i14 = i13 + 2;
                int i15 = i14 + 2;
                int i16 = i15 + 2;

                TagGroup tg = new TagGroup();
                Tag t1 = new Tag(ScaleDBNumber + ".DBD" + i1, S7Link.Tag.ATOMIC.DWORD, 1);
                Tag t2 = new Tag(ScaleDBNumber + ".DBD" + i2, S7Link.Tag.ATOMIC.DWORD, 1);
                Tag t3 = new Tag(ScaleDBNumber + ".DBD" + i3, S7Link.Tag.ATOMIC.DWORD, 1);
                Tag t4 = new Tag(ScaleDBNumber + ".DBD" + i4, S7Link.Tag.ATOMIC.DWORD, 1);
                Tag t5 = new Tag(ScaleDBNumber + ".DBD" + i5, S7Link.Tag.ATOMIC.DWORD, 1);
                Tag t6 = new Tag(ScaleDBNumber + ".DBW" + i6, S7Link.Tag.ATOMIC.WORD, 1);
                Tag t7 = new Tag(ScaleDBNumber + ".DBW" + i7, S7Link.Tag.ATOMIC.WORD, 1);
                Tag t8 = new Tag(ScaleDBNumber + ".DBW" + i8, S7Link.Tag.ATOMIC.WORD, 1);
                Tag t9 = new Tag(ScaleDBNumber + ".DBW" + i9, S7Link.Tag.ATOMIC.WORD, 1);
                Tag t10 = new Tag(ScaleDBNumber + ".DBW" + i10, S7Link.Tag.ATOMIC.WORD, 1);
                Tag t11 = new Tag(ScaleDBNumber + ".DBW" + i11, S7Link.Tag.ATOMIC.WORD, 1);
                Tag t12 = new Tag(ScaleDBNumber + ".DBD" + i12, S7Link.Tag.ATOMIC.DWORD, 1);
                Tag t13 = new Tag(ScaleDBNumber + ".DBW" + i13, S7Link.Tag.ATOMIC.WORD, 1);
                Tag t14 = new Tag(ScaleDBNumber + ".DBW" + i14, S7Link.Tag.ATOMIC.WORD, 1);
                Tag t15 = new Tag(ScaleDBNumber + ".DBW" + i15, S7Link.Tag.ATOMIC.WORD, 1);
                Tag t16 = new Tag(ScaleDBNumber + ".DBD" + i16, S7Link.Tag.ATOMIC.DWORD, 1);

                tg.AddTag(t1);
                tg.AddTag(t2);
                tg.AddTag(t3);
                tg.AddTag(t4);
                tg.AddTag(t5);
                tg.AddTag(t6);
                tg.AddTag(t7);
                tg.AddTag(t8);
                tg.AddTag(t9);
                tg.AddTag(t10);
                tg.AddTag(t11);
                tg.AddTag(t12);
                tg.AddTag(t13);
                tg.AddTag(t14);
                tg.AddTag(t15);
                tg.AddTag(t16);

                if (!PLCR.IsConnected)
                    PLCR.Connect();

                PLCR.GroupRead(tg);

                SwitchOverWeight_Online = Convert.ToDouble(t1.Value) / 1000;
                CutOffWeight_Online = Convert.ToDouble(t2.Value) / 1000;
                TolerancePositiveWeight_Online = Convert.ToDouble(t3.Value) / 1000;
                ToleranceNegativeWeight_Online = Convert.ToDouble(t4.Value) / 1000;
                CutOffWeightCorrectionMax_Online = Convert.ToDouble(t5.Value) / 1000;
                DosingTimeSlow_Online = Convert.ToDouble(t6.Value) / 10;
                SwitchOverTimeFastToSlow_Online = Convert.ToDouble(t7.Value) / 10;
                TipPulseLength_Online = Convert.ToDouble(t8.Value) / 10;
                DosingTimeMax_Online = Convert.ToDouble(t9.Value) / 10;
                TolerancePositiveProportion_Online = Convert.ToDouble(t10.Value);
                ToleranceNegativeProportion_Online = Convert.ToDouble(t11.Value);
                AutoFaultResetWeight_Online = Convert.ToDouble(t12.Value) / 1000;
                AutoFaultResetPercent_Online = Convert.ToDouble(t13.Value);
                DosingSpeedFast_Online = Convert.ToDouble(t14.Value);
                DosingSpeedSlow_Online = Convert.ToDouble(t15.Value);
                FineDosingWeight_Online = Convert.ToDouble(t16.Value) / 1000;
            }
            catch (Exception ex)
            {
                var msgBox = base.GetService<IMessageBoxService>();
                if (msgBox != null)
                    msgBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //----------------------------------------------------------------------//
        //                               Properties                             //
        //----------------------------------------------------------------------//

        private int _ScaleNumber;
        public int ScaleNumber
        {
            get { return _ScaleNumber; }
            set { _ScaleNumber = value; }
        }

        private string _ScaleDBNumber;
        public string ScaleDBNumber
        {
            get { return _ScaleDBNumber; }
            set { _ScaleDBNumber = value; }
        }

        private string _SwitchOverWeightDBOffset;
        public string SwitchOverWeightDBOffset
        {
            get { return _SwitchOverWeightDBOffset; }
            set { _SwitchOverWeightDBOffset = value; }
        }

        private Controller _PLCW;
        public Controller PLCW
        {
            get { return _PLCW; }
            set { _PLCW = value; }
        }

        private Controller _PLCR;
        public Controller PLCR
        {
            get { return _PLCR; }
            set { _PLCR = value; }
        }



        private double _SwitchOverWeight;
        public double SwitchOverWeight
        {
            get { return _SwitchOverWeight; }
            set
            {
                _SwitchOverWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SwitchOverWeight"));
            }
        }

        private double _CutOffWeight;
        public double CutOffWeight
        {
            get { return _CutOffWeight; }
            set
            {
                _CutOffWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CutOffWeight"));
            }
        }

        private double _TolerancePositiveWeight;
        public double TolerancePositiveWeight
        {
            get { return _TolerancePositiveWeight; }
            set
            {
                _TolerancePositiveWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TolerancePositiveWeight"));
            }
        }

        private double _ToleranceNegativeWeight;
        public double ToleranceNegativeWeight
        {
            get { return _ToleranceNegativeWeight; }
            set
            {
                _ToleranceNegativeWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToleranceNegativeWeight"));
            }
        }

        private double _CutOffWeightCorrectionMax;
        public double CutOffWeightCorrectionMax
        {
            get { return _CutOffWeightCorrectionMax; }
            set
            {
                _CutOffWeightCorrectionMax = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CutOffWeightCorrectionMax"));
            }
        }

        private double _DosingTimeSlow;
        public double DosingTimeSlow
        {
            get { return _DosingTimeSlow; }
            set
            {
                _DosingTimeSlow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DosingTimeSlow"));
            }
        }

        private double _SwitchOverTimeFastToSlow;
        public double SwitchOverTimeFastToSlow
        {
            get { return _SwitchOverTimeFastToSlow; }
            set
            {
                _SwitchOverTimeFastToSlow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SwitchOverTimeFastToSlow"));
            }
        }

        private double _TipPulseLength;
        public double TipPulseLength
        {
            get { return _TipPulseLength; }
            set
            {
                _TipPulseLength = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TipPulseLength"));
            }
        }

        private double _DosingTimeMax;
        public double DosingTimeMax
        {
            get { return _DosingTimeMax; }
            set
            {
                _DosingTimeMax = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DosingTimeMax"));
            }
        }

        private double _TolerancePositiveProportion;
        public double TolerancePositiveProportion
        {
            get { return _TolerancePositiveProportion; }
            set
            {
                _TolerancePositiveProportion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TolerancePositiveProportion"));
            }
        }

        private double _ToleranceNegativeProportion;
        public double ToleranceNegativeProportion
        {
            get { return _ToleranceNegativeProportion; }
            set
            {
                _ToleranceNegativeProportion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToleranceNegativeProportion"));
            }
        }

        private double _AutoFaultResetWeight;
        public double AutoFaultResetWeight
        {
            get { return _AutoFaultResetWeight; }
            set
            {
                _AutoFaultResetWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AutoFaultResetWeight"));
            }
        }

        private double _AutoFaultResetPercent;
        public double AutoFaultResetPercent
        {
            get { return _AutoFaultResetPercent; }
            set
            {
                _AutoFaultResetPercent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AutoFaultResetPercent"));
            }
        }

        private double _DosingSpeedFast;
        public double DosingSpeedFast
        {
            get { return _DosingSpeedFast; }
            set
            {
                _DosingSpeedFast = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DosingSpeedFast"));
            }
        }

        private double _DosingSpeedSlow;
        public double DosingSpeedSlow
        {
            get { return _DosingSpeedSlow; }
            set
            {
                _DosingSpeedSlow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DosingSpeedSlow"));
            }
        }

        private double _FineDosingWeight;
        public double FineDosingWeight
        {
            get { return _FineDosingWeight; }
            set
            {
                _FineDosingWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FineDosingWeight"));
            }
        }


        
        //Online
        
        private double _SwitchOverWeight_Online;
        public double SwitchOverWeight_Online
        {
            get { return _SwitchOverWeight_Online; }
            set
            {
                _SwitchOverWeight_Online = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SwitchOverWeight_Online"));

                TextBox1Color = SwitchOverWeight_Online != SwitchOverWeight ? Brushes.LightBlue : Brushes.White;
            }
        }

        private double _CutOffWeight_Online;
        public double CutOffWeight_Online
        {
            get { return _CutOffWeight_Online; }
            set
            {
                _CutOffWeight_Online = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CutOffWeight_Online"));

                TextBox2Color = CutOffWeight_Online != CutOffWeight ? Brushes.LightBlue : Brushes.White;
            }
        }

        private double _TolerancePositiveWeight_Online;
        public double TolerancePositiveWeight_Online
        {
            get { return _TolerancePositiveWeight_Online; }
            set
            {
                _TolerancePositiveWeight_Online = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TolerancePositiveWeight_Online"));

                TextBox3Color = TolerancePositiveWeight_Online != TolerancePositiveWeight ? Brushes.LightBlue : Brushes.White;
            }
        }

        private double _ToleranceNegativeWeight_Online;
        public double ToleranceNegativeWeight_Online
        {
            get { return _ToleranceNegativeWeight_Online; }
            set
            {
                _ToleranceNegativeWeight_Online = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToleranceNegativeWeight_Online"));

                TextBox4Color = ToleranceNegativeWeight_Online != ToleranceNegativeWeight ? Brushes.LightBlue : Brushes.White;
            }
        }

        private double _CutOffWeightCorrectionMax_Online;
        public double CutOffWeightCorrectionMax_Online
        {
            get { return _CutOffWeightCorrectionMax_Online; }
            set
            {
                _CutOffWeightCorrectionMax_Online = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CutOffWeightCorrectionMax_Online"));

                TextBox5Color = CutOffWeightCorrectionMax_Online != CutOffWeightCorrectionMax ? Brushes.LightBlue : Brushes.White;
            }
        }

        private double _DosingTimeSlow_Online;
        public double DosingTimeSlow_Online
        {
            get { return _DosingTimeSlow_Online; }
            set
            {
                _DosingTimeSlow_Online = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DosingTimeSlow_Online"));

                TextBox6Color = DosingTimeSlow_Online != DosingTimeSlow ? Brushes.LightBlue : Brushes.White;
            }
        }

        private double _SwitchOverTimeFastToSlow_Online;
        public double SwitchOverTimeFastToSlow_Online
        {
            get { return _SwitchOverTimeFastToSlow_Online; }
            set
            {
                _SwitchOverTimeFastToSlow_Online = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SwitchOverTimeFastToSlow_Online"));

                TextBox7Color = SwitchOverTimeFastToSlow_Online != SwitchOverTimeFastToSlow ? Brushes.LightBlue : Brushes.White;
            }
        }

        private double _TipPulseLength_Online;
        public double TipPulseLength_Online
        {
            get { return _TipPulseLength_Online; }
            set
            {
                _TipPulseLength_Online = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TipPulseLength_Online"));

                TextBox8Color = TipPulseLength_Online != TipPulseLength ? Brushes.LightBlue : Brushes.White;
            }
        }

        private double _DosingTimeMax_Online;
        public double DosingTimeMax_Online
        {
            get { return _DosingTimeMax_Online; }
            set
            {
                _DosingTimeMax_Online = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DosingTimeMax_Online"));

                TextBox9Color = DosingTimeMax_Online != DosingTimeMax ? Brushes.LightBlue : Brushes.White;
            }
        }

        private double _TolerancePositiveProportion_Online;
        public double TolerancePositiveProportion_Online
        {
            get { return _TolerancePositiveProportion_Online; }
            set
            {
                _TolerancePositiveProportion_Online = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TolerancePositiveProportion_Online"));

                TextBox10Color = TolerancePositiveProportion_Online != TolerancePositiveProportion ? Brushes.LightBlue : Brushes.White;
            }
        }

        private double _ToleranceNegativeProportion_Online;
        public double ToleranceNegativeProportion_Online
        {
            get { return _ToleranceNegativeProportion_Online; }
            set
            {
                _ToleranceNegativeProportion_Online = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToleranceNegativeProportion_Online"));

                TextBox11Color = ToleranceNegativeProportion_Online != ToleranceNegativeProportion ? Brushes.LightBlue : Brushes.White;
            }
        }

        private double _AutoFaultResetWeight_Online;
        public double AutoFaultResetWeight_Online
        {
            get { return _AutoFaultResetWeight_Online; }
            set
            {
                _AutoFaultResetWeight_Online = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AutoFaultResetWeight_Online"));

                TextBox12Color = AutoFaultResetWeight_Online != AutoFaultResetWeight ? Brushes.LightBlue : Brushes.White;
            }
        }

        private double _AutoFaultResetPercent_Online;
        public double AutoFaultResetPercent_Online
        {
            get { return _AutoFaultResetPercent_Online; }
            set
            {
                _AutoFaultResetPercent_Online = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AutoFaultResetPercent_Online"));

                TextBox13Color = AutoFaultResetPercent_Online != AutoFaultResetPercent ? Brushes.LightBlue : Brushes.White;
            }
        }

        private double _DosingSpeedFast_Online;
        public double DosingSpeedFast_Online
        {
            get { return _DosingSpeedFast_Online; }
            set
            {
                _DosingSpeedFast_Online = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DosingSpeedFast_Online"));

                TextBox14Color = DosingSpeedFast_Online != DosingSpeedFast ? Brushes.LightBlue : Brushes.White;
            }
        }

        private double _DosingSpeedSlow_Online;
        public double DosingSpeedSlow_Online
        {
            get { return _DosingSpeedSlow_Online; }
            set
            {
                _DosingSpeedSlow_Online = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DosingSpeedSlow_Online"));

                TextBox15Color = DosingSpeedSlow_Online != DosingSpeedSlow ? Brushes.LightBlue : Brushes.White;
            }
        }

        private double _FineDosingWeight_Online;
        public double FineDosingWeight_Online
        {
            get { return _FineDosingWeight_Online; }
            set
            {
                _FineDosingWeight_Online = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FineDosingWeight_Online"));

                TextBox16Color = FineDosingWeight_Online != FineDosingWeight ? Brushes.LightBlue : Brushes.White;
            }
        }

        //
        //Text Box Colors
        //
        private Brush _TextBox1Color;
        public Brush TextBox1Color
        {
            get { return _TextBox1Color; }
            set
            {
                _TextBox1Color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TextBox1Color"));
            }
        }

        private Brush _TextBox2Color;
        public Brush TextBox2Color
        {
            get { return _TextBox2Color; }
            set
            {
                _TextBox2Color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TextBox2Color"));
            }
        }

        private Brush _TextBox3Color;
        public Brush TextBox3Color
        {
            get { return _TextBox3Color; }
            set
            {
                _TextBox3Color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TextBox3Color"));
            }
        }

        private Brush _TextBox4Color;
        public Brush TextBox4Color
        {
            get { return _TextBox4Color; }
            set
            {
                _TextBox4Color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TextBox4Color"));
            }
        }

        private Brush _TextBox5Color;
        public Brush TextBox5Color
        {
            get { return _TextBox5Color; }
            set
            {
                _TextBox5Color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TextBox5Color"));
            }
        }

        private Brush _TextBox6Color;
        public Brush TextBox6Color
        {
            get { return _TextBox6Color; }
            set
            {
                _TextBox6Color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TextBox6Color"));
            }
        }

        private Brush _TextBox7Color;
        public Brush TextBox7Color
        {
            get { return _TextBox7Color; }
            set
            {
                _TextBox7Color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TextBox7Color"));
            }
        }

        private Brush _TextBox8Color;
        public Brush TextBox8Color
        {
            get { return _TextBox8Color; }
            set
            {
                _TextBox8Color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TextBox8Color"));
            }
        }

        private Brush _TextBox9Color;
        public Brush TextBox9Color
        {
            get { return _TextBox9Color; }
            set
            {
                _TextBox9Color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TextBox9Color"));
            }
        }

        private Brush _TextBox10Color;
        public Brush TextBox10Color
        {
            get { return _TextBox10Color; }
            set
            {
                _TextBox10Color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TextBox10Color"));
            }
        }

        private Brush _TextBox11Color;
        public Brush TextBox11Color
        {
            get { return _TextBox11Color; }
            set
            {
                _TextBox11Color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TextBox11Color"));
            }
        }

        private Brush _TextBox12Color;
        public Brush TextBox12Color
        {
            get { return _TextBox12Color; }
            set
            {
                _TextBox12Color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TextBox12Color"));
            }
        }

        private Brush _TextBox13Color;
        public Brush TextBox13Color
        {
            get { return _TextBox13Color; }
            set
            {
                _TextBox13Color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TextBox13Color"));
            }
        }

        private Brush _TextBox14Color;
        public Brush TextBox14Color
        {
            get { return _TextBox14Color; }
            set
            {
                _TextBox14Color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TextBox14Color"));
            }
        }

        private Brush _TextBox15Color;
        public Brush TextBox15Color
        {
            get { return _TextBox15Color; }
            set
            {
                _TextBox15Color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TextBox15Color"));
            }
        }

        private Brush _TextBox16Color;
        public Brush TextBox16Color
        {
            get { return _TextBox16Color; }
            set
            {
                _TextBox16Color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TextBox16Color"));
            }
        }


        //----------------------------------------------------------------------//
        //                               Events                                 //
        //----------------------------------------------------------------------//

        public new event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }


        //----------------------------------------------------------------------//
        //                              Commands                                //
        //----------------------------------------------------------------------//


        public ICommand SaveParameters
        {
            get { return cmdSaveParameters; }
        }
        public void cmdSaveParametersImplementation()
        {
            try
            {
                var msgBox1 = base.GetService<IMessageBoxService>();
                if (msgBox1 != null)
                {
                    var result = msgBox1.Show("Are you sure that you want to save these parameters? Existing values will be overwritten", "Confirmation Required", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        if (SwitchOverWeightDBOffset != "")
                        {

                            using (SqlConnection conn = new SqlConnection(sqlConnection))
                            {
                                conn.Open();
                                SqlCommand cmd = conn.CreateCommand();
                                cmd.CommandText = "DELETE ParameterSettings WHERE Revision = " + ScaleNumber;
                                int iRowsUpdated = cmd.ExecuteNonQuery();
                                conn.Close();
                            }

                            //Calculate what DB Offsets to write to
                            int iStart = SwitchOverWeightDBOffset.LastIndexOf('D');
                            int iLength = SwitchOverWeightDBOffset.Length;
                            string s = (SwitchOverWeightDBOffset.Substring(iStart + 1, iLength - iStart - 1));
                            int i1 = Convert.ToInt32(s);
                            int i2 = i1 + 4;
                            int i3 = i2 + 4;
                            int i4 = i3 + 4;
                            int i5 = i4 + 4;
                            int i6 = i5 + 4;
                            int i7 = i6 + 2;
                            int i8 = i7 + 2;
                            int i9 = i8 + 2;
                            int i10 = i9 + 2;
                            int i11 = i10 + 2;
                            int i12 = i11 + 2;
                            int i13 = i12 + 4;
                            int i14 = i13 + 2;
                            int i15 = i14 + 2;
                            int i16 = i15 + 2;


                            //Insert Updated records
                            using (SqlConnection conn = new SqlConnection(sqlConnection))
                            {
                                conn.Open();
                                SqlCommand cmd = conn.CreateCommand();
                                StringBuilder sbCommandText = new StringBuilder("INSERT INTO ParameterSettings VALUES ");

                                sbCommandText.Append("(" + ScaleNumber + ", 1, 'SwitchOverWeight', '" + ScaleDBNumber + ".DBD" + i1 + "'," + (SwitchOverWeight * 1000) + ", '" + DateTime.Now + "'),");
                                sbCommandText.Append("(" + ScaleNumber + ", 2, 'CutOffWeight', '" + ScaleDBNumber + ".DBD" + i2 + "'," + (CutOffWeight * 1000) + ", '" + DateTime.Now + "'),");
                                sbCommandText.Append("(" + ScaleNumber + ", 3, 'TolerancePositiveWeight', '" + ScaleDBNumber + ".DBD" + i3 + "'," + (TolerancePositiveWeight * 1000) + ", '" + DateTime.Now + "'),");
                                sbCommandText.Append("(" + ScaleNumber + ", 4, 'ToleranceNegativeWeight', '" + ScaleDBNumber + ".DBD" + i4 + "'," + (ToleranceNegativeWeight * 1000) + ", '" + DateTime.Now + "'),");
                                sbCommandText.Append("(" + ScaleNumber + ", 5, 'CutOffWeightCorrectionMax', '" + ScaleDBNumber + ".DBD" + i5 + "'," + (CutOffWeightCorrectionMax * 1000) + ", '" + DateTime.Now + "'),");
                                sbCommandText.Append("(" + ScaleNumber + ", 6, 'DosingTimeSlow', '" + ScaleDBNumber + ".DBW" + i6 + "'," + (DosingTimeSlow * 10) + ", '" + DateTime.Now + "'),");
                                sbCommandText.Append("(" + ScaleNumber + ", 7, 'SwitchOverTimeFastToSlow', '" + ScaleDBNumber + ".DBW" + i7 + "'," + (SwitchOverTimeFastToSlow * 10) + ", '" + DateTime.Now + "'),");
                                sbCommandText.Append("(" + ScaleNumber + ", 8, 'TipPulseLength', '" + ScaleDBNumber + ".DBW" + i8 + "'," + (TipPulseLength * 10) + ", '" + DateTime.Now + "'),");
                                sbCommandText.Append("(" + ScaleNumber + ", 9, 'DosingTimeMax', '" + ScaleDBNumber + ".DBW" + i9 + "'," + (DosingTimeMax * 10) + ", '" + DateTime.Now + "'),");
                                sbCommandText.Append("(" + ScaleNumber + ", 10, 'TolerancePositiveProportion', '" + ScaleDBNumber + ".DBW" + i10 + "'," + TolerancePositiveProportion + ", '" + DateTime.Now + "'),");
                                sbCommandText.Append("(" + ScaleNumber + ", 11, 'ToleranceNegativeProportion', '" + ScaleDBNumber + ".DBW" + i11 + "'," + ToleranceNegativeProportion + ", '" + DateTime.Now + "'),");
                                sbCommandText.Append("(" + ScaleNumber + ", 12, 'AutoFaultResetWeight', '" + ScaleDBNumber + ".DBD" + i12 + "'," + (AutoFaultResetWeight * 1000) + ", '" + DateTime.Now + "'),");
                                sbCommandText.Append("(" + ScaleNumber + ", 13, 'AutoFaultResetPercent', '" + ScaleDBNumber + ".DBW" + i13 + "'," + AutoFaultResetPercent + ", '" + DateTime.Now + "'),");
                                sbCommandText.Append("(" + ScaleNumber + ", 14, 'DosingSpeedFast', '" + ScaleDBNumber + ".DBW" + i14 + "'," + DosingSpeedFast + ", '" + DateTime.Now + "'),");
                                sbCommandText.Append("(" + ScaleNumber + ", 15, 'DosingSpeedSlow', '" + ScaleDBNumber + ".DBW" + i15 + "'," + DosingSpeedSlow + ", '" + DateTime.Now + "'),");
                                sbCommandText.Append("(" + ScaleNumber + ", 16, 'FineDosingWeight', '" + ScaleDBNumber + ".DBD" + i16 + "'," + (FineDosingWeight * 1000) + ", '" + DateTime.Now + "')");

                                cmd.CommandText = sbCommandText.ToString();
                                int iRowsInserted = cmd.ExecuteNonQuery();

                                if (iRowsInserted > 0)
                                {
                                    var msgBox = base.GetService<IMessageBoxService>();
                                    if (msgBox != null)
                                        msgBox.Show("Discharger parameters saved", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                conn.Close();
                            }
                        }
                        else
                        {
                            var msgBox = base.GetService<IMessageBoxService>();
                            if (msgBox != null)
                                msgBox.Show("No DB Offset has been supplied for the SwitchOverWeight Parameter", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msgBox = base.GetService<IMessageBoxService>();
                if (msgBox != null)
                    msgBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand DownloadParameters
        {
            get { return cmdDownloadParameters; }
        }
        public void cmdDownloadParametersImplementation()
        {
            try
            {
                var msgBox = base.GetService<IMessageBoxService>();
                if (msgBox != null)
                {
                    var result = msgBox.Show("Are you sure you want to download the OFFLINE values to the PLC? This will OVERWRITE ONLINE values.", "Confrim Download. OFFLINE --> ONLINE", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        //Calculate what DB Offsets to Read From
                        int iStart = SwitchOverWeightDBOffset.LastIndexOf('D');
                        int iLength = SwitchOverWeightDBOffset.Length;
                        string s = (SwitchOverWeightDBOffset.Substring(iStart + 1, iLength - iStart - 1));
                        int i1 = Convert.ToInt32(s);
                        int i2 = i1 + 4;
                        int i3 = i2 + 4;
                        int i4 = i3 + 4;
                        int i5 = i4 + 4;
                        int i6 = i5 + 4;
                        int i7 = i6 + 2;
                        int i8 = i7 + 2;
                        int i9 = i8 + 2;
                        int i10 = i9 + 2;
                        int i11 = i10 + 2;
                        int i12 = i11 + 2;
                        int i13 = i12 + 4;
                        int i14 = i13 + 2;
                        int i15 = i14 + 2;
                        int i16 = i15 + 2;

                        TagGroup tg = new TagGroup();
                        Tag t1 = new Tag(ScaleDBNumber + ".DBD" + i1, S7Link.Tag.ATOMIC.DWORD, 1);
                        Tag t2 = new Tag(ScaleDBNumber + ".DBD" + i2, S7Link.Tag.ATOMIC.DWORD, 1);
                        Tag t3 = new Tag(ScaleDBNumber + ".DBD" + i3, S7Link.Tag.ATOMIC.DWORD, 1);
                        Tag t4 = new Tag(ScaleDBNumber + ".DBD" + i4, S7Link.Tag.ATOMIC.DWORD, 1);
                        Tag t5 = new Tag(ScaleDBNumber + ".DBD" + i5, S7Link.Tag.ATOMIC.DWORD, 1);
                        Tag t6 = new Tag(ScaleDBNumber + ".DBW" + i6, S7Link.Tag.ATOMIC.WORD, 1);
                        Tag t7 = new Tag(ScaleDBNumber + ".DBW" + i7, S7Link.Tag.ATOMIC.WORD, 1);
                        Tag t8 = new Tag(ScaleDBNumber + ".DBW" + i8, S7Link.Tag.ATOMIC.WORD, 1);
                        Tag t9 = new Tag(ScaleDBNumber + ".DBW" + i9, S7Link.Tag.ATOMIC.WORD, 1);
                        Tag t10 = new Tag(ScaleDBNumber + ".DBW" + i10, S7Link.Tag.ATOMIC.WORD, 1);
                        Tag t11 = new Tag(ScaleDBNumber + ".DBW" + i11, S7Link.Tag.ATOMIC.WORD, 1);
                        Tag t12 = new Tag(ScaleDBNumber + ".DBD" + i12, S7Link.Tag.ATOMIC.DWORD, 1);
                        Tag t13 = new Tag(ScaleDBNumber + ".DBW" + i13, S7Link.Tag.ATOMIC.WORD, 1);
                        Tag t14 = new Tag(ScaleDBNumber + ".DBW" + i14, S7Link.Tag.ATOMIC.WORD, 1);
                        Tag t15 = new Tag(ScaleDBNumber + ".DBW" + i15, S7Link.Tag.ATOMIC.WORD, 1);
                        Tag t16 = new Tag(ScaleDBNumber + ".DBD" + i16, S7Link.Tag.ATOMIC.DWORD, 1);


                        t1.Value = SwitchOverWeight * 1000;
                        t2.Value = CutOffWeight * 1000;
                        t3.Value = TolerancePositiveWeight * 1000;
                        t4.Value = ToleranceNegativeWeight * 1000;
                        t5.Value = CutOffWeightCorrectionMax * 1000;
                        t6.Value = DosingTimeSlow * 10;
                        t7.Value = SwitchOverTimeFastToSlow * 10;
                        t8.Value = TipPulseLength * 10;
                        t9.Value = DosingTimeMax * 10;
                        t10.Value = TolerancePositiveProportion;
                        t11.Value = ToleranceNegativeProportion;
                        t12.Value = AutoFaultResetWeight * 1000;
                        t13.Value = AutoFaultResetPercent;
                        t14.Value = DosingSpeedFast;
                        t15.Value = DosingSpeedSlow;
                        t16.Value = FineDosingWeight * 1000;

                        tg.AddTag(t1);
                        tg.AddTag(t2);
                        tg.AddTag(t3);
                        tg.AddTag(t4);
                        tg.AddTag(t5);
                        tg.AddTag(t6);
                        tg.AddTag(t7);
                        tg.AddTag(t8);
                        tg.AddTag(t9);
                        tg.AddTag(t10);
                        tg.AddTag(t11);
                        tg.AddTag(t12);
                        tg.AddTag(t13);
                        tg.AddTag(t14);
                        tg.AddTag(t15);
                        tg.AddTag(t16);

                        if (!PLCW.IsConnected)
                            PLCW.Connect();

                        PLCR.GroupWrite(tg);

                        ReadParameterValuesFromPLC();
                    }
                }

            }
            catch (Exception ex)
            {
                var msgBox = base.GetService<IMessageBoxService>();
                if (msgBox != null)
                    msgBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
