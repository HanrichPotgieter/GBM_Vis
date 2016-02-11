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
using System.Windows.Threading;

namespace KNEKT.Controls
{
    /// <summary>
    /// Interaction logic for ByPassHammerButton.xaml
    /// </summary>
    public partial class ByPassHammerButton : UserControl
    {

        private string _OnDBAddress = "DB0.DBD0";
        private Controller PLC_W;
        //private bool _MillerCallStarted = false;

        //------------------------------------------------------------------------------//
        //                                 Constructor                                  //
        //------------------------------------------------------------------------------//
        public ByPassHammerButton()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
        //------------------------------------------------------------------------------//

        public string HammerBypass_Title
        {
            get
            {
                return lblHammerByPass.Content.ToString();
            }
            set
            {
                lblHammerByPass.Content = value;
            }
        }

        public Controller ByPassHammerButton_WriteController
        {
            set
            {
                PLC_W = value;
            }
        }

        /// <summary>
        /// DB Offset to read for display of which button is on
        /// </summary>
        public string ByPassHammerButton_OnBit
        {
            get
            {
                return _OnDBAddress;
            }
            set
            {
                _OnDBAddress = value;
            }
        }

        private void btnBypassHamer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_OnDBAddress != "DB0.DBD0")
                {

                    if (!PLC_W.IsConnected)
                        PLC_W.Connect();

                    if (PLC_W.IsConnected)
                    {
                        Tag t = new Tag(_OnDBAddress);
                        t.DataType = S7Link.Tag.ATOMIC.BOOL;
                        t.Length = 1;
                        t.Value = true;
                        PLC_W.WriteTag(t);

                        LogItem li = new LogItem(DateTime.Now, DateTime.Now.ToOADate(), "btnOn", this.lblHammerByPass.Content.ToString(), 30);
                        MainWindow.alLoggerToSQL.Add(li);

                    }
                    else
                    {
                        MessageBox.Show("Write --> Not Connected");
                    }
                }
                else
                {
                    MessageBox.Show("Value has not been set", "Invalid Setting", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error -->" + ex.Message);
            }
        }


        private void btnStartHammer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_OnDBAddress != "DB0.DBD0")
                {

                    if (!PLC_W.IsConnected)
                        PLC_W.Connect();

                    if (PLC_W.IsConnected)
                    {
                        Tag t = new Tag(_OnDBAddress);
                        t.DataType = S7Link.Tag.ATOMIC.BOOL;
                        t.Length = 1;
                        t.Value = false;
                        PLC_W.WriteTag(t);

                        LogItem li = new LogItem(DateTime.Now, DateTime.Now.ToOADate(), "btnOff", this.lblHammerByPass.Content.ToString(), 30);
                        MainWindow.alLoggerToSQL.Add(li);
                    }
                    else
                    {
                        MessageBox.Show("Write --> Not Connected");
                    }
                }
                else
                {
                    MessageBox.Show("Value has not been set", "Invalid Setting", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error -->" + ex.Message);
            }
        }



    }
}
