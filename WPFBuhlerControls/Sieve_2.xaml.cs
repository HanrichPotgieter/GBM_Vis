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
    /// Interaction logic for Sieve_2.xaml
    /// </summary>
    public partial class Sieve_2 : UserControl
    {
        public Sieve_2()
        {
            InitializeComponent();

            PolyMain.Fill = KNEKTColors.NoControlColor;
            RectMain.Fill = KNEKTColors.NoControlColor;
        }
    }
}
