using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;

namespace KNEKT.Classes
{
    public class SectionInfoViewModel : INotifyPropertyChanged
    {
        public SectionInfoViewModel()
        {
            //SectionState = "DEFAULT!";
        }

        public void UpdateSectionControl()
        {
            //switch (SectionStateColor.ToString())
            //{
            //    case Brushes.White.ToString():
            //        SectionState = "Passive";
            //        break;

            //    case "Fuchsia":
            //        SectionState = "Waiting";
            //        break;

            //    case "Green":
            //        SectionState = "Active";
            //        break;

            //    case "Lime":
            //        SectionState = "Ready";
            //        break;

            //    case "Aqua":
            //        SectionState = "Emptying";
            //        break;

            //    case "Yellow":
            //        SectionState = "Emptied";
            //        break;

            //    case "LightGreen":
            //        SectionState = "Idling";
            //        break;

            //    case "Red":
            //        SectionState = "Fault";
            //        break;
            //}

            if (SectionStateColor.ToString() == Brushes.White.ToString())
            {
                SectionState = "Passive";
            }
            else if (SectionStateColor.ToString() == Brushes.Fuchsia.ToString())
            {
                SectionState = "Waiting";
            }
            else if (SectionStateColor.ToString() == Brushes.Green.ToString())
            {
                SectionState = "Active";
            }
            else if (SectionStateColor.ToString() == Brushes.Lime.ToString())
            {
                SectionState = "Ready";
            }
            else if (SectionStateColor.ToString() == Brushes.Aqua.ToString())
            {
                SectionState = "Emptying";
            }
            else if (SectionStateColor.ToString() == Brushes.Yellow.ToString())
            {
                SectionState = "Emptied";
            }
            else if (SectionStateColor.ToString() == Brushes.LightGreen.ToString())
            {
                SectionState = "Idling";
            }
            else if (SectionStateColor.ToString() == Brushes.Red.ToString())
            {
                SectionState = "Fault";
            }
        }

        private string _SectionName;
        public string SectionName
        {
            get { return _SectionName; }
            set
            {
                if (_SectionName != value)
                {
                    _SectionName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SectionName"));
                }
            }
        }

        private int _SectionCurrentBatchNumber;
        public int SectionCurrentBatchNumber
        {
            get { return _SectionCurrentBatchNumber; }
            set
            {
                if (_SectionCurrentBatchNumber != value)
                {
                    _SectionCurrentBatchNumber = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SectionCurrentBatchNumber"));
                }
            }
        }

        private int _SectionMaxBatches;
        public int SectionMaxBatches
        {
            get { return _SectionMaxBatches; }
            set
            {
                if (_SectionMaxBatches != value)
                {
                    _SectionMaxBatches = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SectionMaxBatches"));
                }
            }
        }

        private string _SectionState;
        public string SectionState
        {
            get { return _SectionState; }
            set
            {
                _SectionState = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SectionState"));
            }
        }

        private int _SectionStateCode;
        public int SectionStateCode
        {
            get
            {
                return _SectionStateCode;
            }
            set
            {
                _SectionStateCode = value;

                OnPropertyChanged(new PropertyChangedEventArgs("SectionStateCode"));
            }
        }

        private int _SectionJobNumber;
        public int SectionJobNumber
        {
            get { return _SectionJobNumber; }
            set
            {
                _SectionJobNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SectionJobNumber"));
            }
        }

        private Brush _LabelColour;
        public Brush LabelColour
        {
            get
            {
                return _LabelColour;
            }
            set
            {
                _LabelColour = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LabelColour"));
            }
        }


        private Brush _SectionStateColor;
        public Brush SectionStateColor
        {
            get
            {
                return _SectionStateColor;
            }
            set
            {
                _SectionStateColor = value;
                UpdateSectionControl();
                OnPropertyChanged(new PropertyChangedEventArgs("SectionStateColor"));
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
