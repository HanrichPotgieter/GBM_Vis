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
using System.Windows.Shapes;
using System.Threading;

namespace KNEKT.DisplayPages.DisplayWindows
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreenWindow : Window
    {


        public SplashScreenWindow()
        {
            InitializeComponent();
            lblKNEKTVersionNumber.Content = "V " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString() + "." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build.ToString();
        }

        private string versionNumber;


        public string VersionNumber
        {
            get { return versionNumber; }
            set
            {
                versionNumber = value;
                lblKNEKTVersionNumber.Content = versionNumber;
            }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                lblKNEKTLoadingProgress.Content = value;
            }
        }

        private double value;
        public double Value
        {
            get { return this.value; }
            set
            {
                this.value = value;
                pgbKNEKTLoadingProgress.Value = value;
                //doubleAnimation = new DoubleAnimation(progressBar1.Value, duration);
                //progressBar1.BeginAnimation(ProgressBar.ValueProperty, doubleAnimation);
            }
        }



        private static SplashScreenWindow splash = new SplashScreenWindow();

        // To refresh the UI immediately
        private delegate void RefreshDelegate();
        private static void Refresh(DependencyObject obj)
        {
            obj.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, (RefreshDelegate)delegate { });
        }

        public static void BeginDisplay()
        {
            splash.Show();
        }

        public static void EndDisplay()
        {
            splash.Close();
        }

        public static void CurrentLoadingStatus(string test, double progressValue)
        {
            splash.Text = test;
            splash.Value = progressValue;
            Refresh(splash.lblKNEKTLoadingProgress);
            Refresh(splash.pgbKNEKTLoadingProgress);
        }
    }
}
