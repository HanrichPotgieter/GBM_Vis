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
using S7Link;

namespace KNEKT.DisplayWindows
{
    /// <summary>
    /// Interaction logic for ManualControl.xaml
    /// </summary>
    public partial class ManualControl : Window
    {
        private Controller PLC_W;
        private string sStartAddress = "DB0.DBX0.0";
        private string sStopAddress = "DB0.DBX0.0";


        public ManualControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a new instance of the Manual Control Form
        /// </summary>
        /// <param name="controller">Pass the S7 Controller from the Main program</param>
        /// <param name="dbxStartAddress">Byte Number to switch on in the PLC (DB---[.]DBX---)</param>
        /// <param name="dbxStopAddress">Byte Number to switch off in the PLC (DB---[.]DBX---)</param>
        public ManualControl(Controller controller, string dbxStartAddress, string dbxStopAddress)
        {
            InitializeComponent();

            PLC_W = controller;
            this.sStartAddress = dbxStartAddress;
            this.sStopAddress = dbxStopAddress;
        }



        //------------------------------------------------------------------------------//
        //                                  Event Handlers                              //
        //------------------------------------------------------------------------------//

        private void ButtonOn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!PLC_W.IsConnected)
                    PLC_W.Connect();

                if (PLC_W.IsConnected)
                {
                    Tag t1 = new Tag(sStartAddress, S7Link.Tag.ATOMIC.BOOL, 1);
                    t1.Value = true;
                    PLC_W.WriteTag(t1);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Write Channel is Not Connected");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("WriteOn >> " + ex.Message);
            }
        }

        private void ButtonOff_Click(object sender, EventArgs e)
        {
            try
            {
                if (!PLC_W.IsConnected)
                    PLC_W.Connect();

                if (PLC_W.IsConnected)
                {
                    Tag t1 = new Tag(sStopAddress, S7Link.Tag.ATOMIC.BOOL, 1);
                    t1.Value = false;
                    PLC_W.WriteTag(t1);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Write Channel is Not Connected");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("WriteOff >> " + ex.Message);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }        
    }
}
