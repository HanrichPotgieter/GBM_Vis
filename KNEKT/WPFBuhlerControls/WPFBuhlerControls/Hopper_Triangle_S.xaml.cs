﻿using System;
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
    /// Interaction logic for Hopper_Triangle_S.xaml
    /// </summary>
    public partial class Hopper_Triangle_S : UserControl
    {
        public Hopper_Triangle_S()
        {
            InitializeComponent();

            polyMain.Fill = KNEKTColors.BinColor;
        }
    }
}
