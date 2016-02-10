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

namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for Label_T1.xaml
    /// </summary>
    public partial class Label_T1 : UserControl
    {
        public Label_T1()
        {
            InitializeComponent();
        }

        public string Label_Text
        {
            get
            {
                return label1.Content.ToString();
            }
            set
            {
                label1.Content = value;
            }
        }
    }
}
