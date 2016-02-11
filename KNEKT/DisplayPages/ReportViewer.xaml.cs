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
using SHDocVw;

namespace KNEKT.DisplayPages
{
    /// <summary>
    /// Interaction logic for ReportViewer.xaml
    /// </summary>
    public partial class ReportViewer : Page
    {
        public ReportViewer()
        {
            InitializeComponent();

            try
            {
                webBrowser1.Navigate(new Uri(MainWindow.stat_ReportViewerAddress + "?u=" + MainWindow.sCurrentUsername + "&p=" + MainWindow.sCurrentPassword + "&bFK=true"));
            }
            catch (Exception)
            { }
            //    (webBrowser1.ActiveXInstance as SHDocVw.WebBrowser).NewWindow3 += new SHDocVw.DWebBrowserEvents2_NewWindow3EventHandler(Browser_NewWindow3);


            //    SHDocVw.WebBrowser axBrowser;// = (webBrowser1.ActiveXInstance as SHDocVw.WebBrowser);
            //    axBrowser.NewWindow3 += new SHDocVw.DWebBrowserEvents2_NewWindow3EventHandler(axBrowser_NewWindow3);

            //}

            ////void webBrowser1_NewWindow(object sender, CancelEventArgs e)
            ////{
            ////    e.Cancel = true; //cancel the navigating
            ////}

            //void axBrowser_NewWindow3(ref object ppDisp, ref bool Cancel, uint dwFlags, string bstrUrlContext, string bstrUrl)
            //{
            //    // access the web page with the URL bstrUrl
            //}
        }
    }
}
