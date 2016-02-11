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
using Snap7;


namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for test.xaml
    /// </summary>
    public partial class test : UserControl
    {
        public String name { get; set; }

        public test()
        {
            InitializeComponent();
            name = "hanrich";
            S7Client test = Plc.Instance;
            if (test.Connected())
            {
                Console.WriteLine("Control is connected");
            }
            else
            {
                Console.WriteLine("no connection");
            }
        }

    }
}
