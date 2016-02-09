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
using S7Link;

namespace KNEKT.Controls
{
    /// <summary>
    /// Interaction logic for ScaleInfo1.xaml
    /// </summary>
    public partial class ScaleInfo1 : UserControl
    {

        //------------------------------------------------------------------------------//
        //                                 Constructor                                  //
        //------------------------------------------------------------------------------//

        public ScaleInfo1()
        {
            InitializeComponent();
        }


        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//

        public string ScaleInfo_Title
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

        private void lblAlarmNo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Classes.MEAG m1 = new Classes.MEAG();
            m1.MEAG_AlarmCode = Convert.ToInt32(lblAlarmNo.Content);
            MainWindow.sElementDescription = m1.MEAG_AlarmDescritpion;
            MainWindow.sElementState = m1.MEAG_AlarmCodeDescription;
            //MessageBox.Show(m1.MEAG_AlarmCode + " - " + m1.MEAG_AlarmDescritpion, "MEAG Alarm Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
