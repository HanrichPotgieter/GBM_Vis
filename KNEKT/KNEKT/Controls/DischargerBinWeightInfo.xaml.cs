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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KNEKT.Controls
{
    /// <summary>
    /// Interaction logic for DischargerBinWeightInfo.xaml
    /// </summary>
    public partial class DischargerBinWeightInfo : UserControl
    {
        public DischargerBinWeightInfo()
        {
            InitializeComponent();
        }

        public string DischargerBinWeightInfo_Title
        {
            get
            {
                return lblTitle.Content.ToString();
            }
            set
            {
                lblTitle.Content = value;
            }
        }





        private double _DischargerBinWeightInfoValue;
        public double DischargerBinWeightInfoValue
        {
            get { return _DischargerBinWeightInfoValue; }
            set
            {
                if (value >= 0)
                {
                    _DischargerBinWeightInfoValue = value;
                    lblWeightValue.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        lblWeightValue.Content = value + " t";
                    }));
                }
            }
        }
    }
}
