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

namespace KNEKT.Controls
{
    /// <summary>
    /// Interaction logic for EmptyingTimer.xaml
    /// </summary>
    public partial class EmptyingTimer : UserControl
    {
        public EmptyingTimer()
        {
            InitializeComponent();
            progressBarEmptying.Maximum = 10;
        }

        private void progressBarEmptying_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //MessageBox.Show("" + e.OldValue + "   " + e.NewValue);
            lblProgress.Content = "" + e.NewValue;
        }



    }
}
