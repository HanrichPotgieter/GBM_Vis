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
    /// Interaction logic for HammerMillInfo.xaml
    /// </summary>
    public partial class HammerMillInfo : UserControl
    {
        public HammerMillInfo()
        {
            InitializeComponent();
        }
        public string HammerMillInfo_Title
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
    }
}
