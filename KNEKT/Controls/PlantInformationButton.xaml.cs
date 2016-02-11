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
    /// Interaction logic for PlantInformationButton.xaml
    /// </summary>
    public partial class PlantInformationButton : UserControl
    {
        private bool bFault = false;

        public PlantInformationButton()
        {
            InitializeComponent();
        }

        private void btnPlantInformation_Click(object sender, RoutedEventArgs e)
        {
            //Navigate to PlantInformation Page here
        }

        public bool PlantInformation_Fault
        {
            get
            {
                return bFault;
            }
            set
            {
                bFault = value;

                LinearGradientBrush lgb = new LinearGradientBrush();
                lgb.StartPoint = new Point(1, 0);
                lgb.EndPoint = new Point(0, 0);

                if (bFault)
                {
                    lgb.GradientStops.Add(new GradientStop(Colors.DarkRed, 0.0D));
                    lgb.GradientStops.Add(new GradientStop(Colors.Red, 0.4D));
                    lgb.GradientStops.Add(new GradientStop(Colors.White, 1.0D));
                }
                else
                {
                    lgb.GradientStops.Add(new GradientStop(Colors.LimeGreen, 0.0D));
                    lgb.GradientStops.Add(new GradientStop(Colors.Green, 0.4D));
                    lgb.GradientStops.Add(new GradientStop(Colors.White, 1.0D));
                }

                btnPlantInformation.Background = lgb;
            }
        }
    }
}
