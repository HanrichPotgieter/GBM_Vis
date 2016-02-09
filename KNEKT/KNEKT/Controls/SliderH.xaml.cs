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
using KNEKT.DisplayWindows;
using S7Link;

namespace KNEKT.Controls
{
    /// <summary>
    /// Interaction logic for SliderH.xaml
    /// </summary>
    public partial class SliderH : UserControl
    {
        private Controller PLC_W;
        private string _DBOffset;
        private bool _IsTagDataTypeReal;

        //------------------------------------------------------------------------------//
        //                              Constructors                                    //
        //------------------------------------------------------------------------------//

        public SliderH()
        {
            InitializeComponent();
        }
       

        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//

        public Controller Slider_WriteController
        {
            get
            {
                return this.PLC_W;
            }
            set
            {
                this.PLC_W = value;
            }
        }

        public string Slider_DBOffset
        {
            get
            {
                return this._DBOffset;
            }
            set
            {
                this._DBOffset = value;
            }
        }

        public string Slider_Title
        {
            get
            {
                return lblTitle.Content.ToString();
            }
            set
            {
                this.lblTitle.Content = value;
            }
        }

        public bool Slider_IsDataTypeReal
        {
            get
            {
                return _IsTagDataTypeReal;
            }
            set
            {
                _IsTagDataTypeReal = value;
            }
        }



        //------------------------------------------------------------------------------//
        //                                  Mehthods                                    //
        //------------------------------------------------------------------------------//


        /// <summary>
        /// Write the value of the slider to the DB
        /// </summary>
        public void WriteValue()
        {
            try
            {
                //Write the value of the Control box to the InFlowrate Address
                if (Slider_DBOffset != "DB0.DBD0")
                {

                    if (!PLC_W.IsConnected)
                        PLC_W.Connect();

                    if (PLC_W.IsConnected)
                    {
                        Tag t = new Tag(Slider_DBOffset);

                        if (Slider_IsDataTypeReal != true)
                        {
                            //
                            //Check datatype 
                            //
                            int iPosOfPoint = Slider_DBOffset.IndexOf('.');
                            string sRightOfPoint = Slider_DBOffset.Substring((iPosOfPoint + 1), 3);

                            if (sRightOfPoint.ToUpper() == "DBX")
                            {
                                t.DataType = S7Link.Tag.ATOMIC.BYTE;
                            }
                            else if (sRightOfPoint.ToUpper() == "DBB")
                            {
                                t.DataType = S7Link.Tag.ATOMIC.BOOL;
                            }
                            else if (sRightOfPoint.ToUpper() == "DBW")
                            {
                                t.DataType = S7Link.Tag.ATOMIC.WORD;
                            }
                            else if (sRightOfPoint.ToUpper() == "DBD")
                            {
                                t.DataType = S7Link.Tag.ATOMIC.DWORD;
                            }

                        }
                        else
                        {
                            t.DataType = S7Link.Tag.ATOMIC.REAL;
                        }
                        t.Value = slider1.Value;
                        t.Length = 1;
                        PLC_W.WriteTag(t);
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







        //------------------------------------------------------------------------------//
        //                                  Events                                      //
        //------------------------------------------------------------------------------//

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double dValue = Math.Round(slider1.Value, 2);
            lblPerentage.Content = dValue + "%";
            
        }

        private void contMen_EnterVal_Click(object sender, RoutedEventArgs e)
        {
            //NumericKeypad num = new NumericKeypad("0", this.lblTitle.ToString(), Slider_WriteController, Slider_DBOffset, false,true);
            //num.Show();            
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            if (slider1.Value > 0)
            {
                slider1.Value--;
                WriteValue();
            }
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            if (slider1.Value < 100)
            {
                slider1.Value++;
                WriteValue();
            }
        }

        private void slider1_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            WriteValue();
        }
      
    }
}
