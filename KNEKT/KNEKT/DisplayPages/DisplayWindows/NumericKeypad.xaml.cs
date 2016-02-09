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
    /// Interaction logic for NumericKeypad.xaml
    /// </summary>
    public partial class NumericKeypad : Window
    {
        Controller PLC_W = new Controller();
        private string _InFlowrateOffset = "DB0.DBD0";
        public static event EventHandler Text_Changed;
        private string _sText = "";
        private bool _PercentageInput;
        private bool _IsDataTypeReal = false;
        private bool _IsSignedValue = false;
        private int _MaxThroughput = 0;
        private bool _IsConvertPercentToLitres = false;
        private bool _IsValueMYFCInOffsetMoisture;
        private string _WriteToDBOffset = "DB0.DBW0";

        private int _MinLimit = 0;
        private int _MaxLimit = 0;

        //Used to manipulate the deciaml written to the individual MEAGs. If value is 100 But MEAG requires 1000, Multiply write value by 10
        private int _MultiplyWriteValue = 0;
        private int _DivideWriteValue = 0;

        //private double _NominalMoisture = 0;
        //private string _RawMoisture;
        //private string _ProductFlowrate;


        //------------------------------------------------------------------------------//
        //                              Constructors                                    //
        //------------------------------------------------------------------------------//

        public NumericKeypad(string CurrentValue)
        {
            InitializeComponent();

            if (CurrentValue != "-")
            {
                sText = CurrentValue;
            }
            else
            {
                sText = "0";
            }
            txt1.Text = sText;

            Text_Changed += new EventHandler(sText_Changed);
        }

        public NumericKeypad(string CurrentValue, string Title, Controller WriteController, string InFlowrateOffset)
        {
            InitializeComponent();
            this.Title = "" + Title;

            if (CurrentValue != "-")
            {
                sText = CurrentValue;
            }
            else
            {
                sText = "0";
            }
            txt1.Text = sText;

            Text_Changed += new EventHandler(sText_Changed);

            PLC_W = WriteController;
            _InFlowrateOffset = InFlowrateOffset;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CurrentValue">Current value of the label</param>
        /// <param name="Title">Title of the Numeric keypad</param>
        /// <param name="WriteController">S7 Link Write Controller</param>
        /// <param name="InFlowrateOffset">Address to write to</param>
        /// <param name="PercentageInput">Enter flowrate as percentage</param>
        public NumericKeypad(string CurrentValue, string Title, Controller WriteController, string DBOffset, bool PercentageInput)
        {
            InitializeComponent();
            this.Title = "" + Title;

            if (CurrentValue != "-")
            {
                sText = CurrentValue;
            }
            else
            {
                sText = "0";
            }
            txt1.Text = sText;

            Text_Changed += new EventHandler(sText_Changed);

            PLC_W = WriteController;
            _InFlowrateOffset = DBOffset;
            _PercentageInput = PercentageInput;

            if (PercentageInput)
            {
                button11.IsEnabled = true;
            }
            else
            {
                button11.IsEnabled = false;
            }

        }

        public NumericKeypad(string CurrentValue, string Title, Controller WriteController, string DBOffset, bool PercentageInput, bool IsDataTypeReal)
        {
            InitializeComponent();
            this.Title = "" + Title;

            if (CurrentValue != "-")
            {
                sText = CurrentValue;
            }
            else
            {
                sText = "0";
            }
            txt1.Text = sText;

            Text_Changed += new EventHandler(sText_Changed);

            PLC_W = WriteController;
            _InFlowrateOffset = DBOffset;
            _PercentageInput = PercentageInput;

            if (PercentageInput)
            {
                button11.IsEnabled = true;
            }
            else
            {
                button11.IsEnabled = false;
            }

            _IsDataTypeReal = IsDataTypeReal;
        }

        public NumericKeypad(string CurrentValue, string Title, Controller WriteController, string DBOffset, bool PercentageInput, bool IsDataTypeReal, bool IsSignedValue)
        {
            InitializeComponent();
            this.Title = "" + Title;

            if (CurrentValue != "-")
            {
                sText = CurrentValue;
            }
            else
            {
                sText = "0";
            }
            txt1.Text = sText;

            Text_Changed += new EventHandler(sText_Changed);

            PLC_W = WriteController;
            _InFlowrateOffset = DBOffset;
            _PercentageInput = PercentageInput;

            if (PercentageInput)
            {
                button11.IsEnabled = true;
            }
            else
            {
                button11.IsEnabled = false;
            }

            _IsDataTypeReal = IsDataTypeReal;
            _IsSignedValue = IsSignedValue;

            button10.IsEnabled = IsSignedValue; //true = Enables Negative values to be input

        }

        public NumericKeypad(string CurrentValue, string Title, Controller WriteController, string DBOffset, bool PercentageInput, bool IsDataTypeReal, bool IsSignedValue, int MaxThroughput)
        {
            InitializeComponent();
            this.Title = "" + Title;

            if (CurrentValue != "-")
            {
                sText = CurrentValue;
            }
            else
            {
                sText = "0";
            }
            txt1.Text = sText;

            Text_Changed += new EventHandler(sText_Changed);

            PLC_W = WriteController;
            _InFlowrateOffset = DBOffset;
            _PercentageInput = PercentageInput;

            if (PercentageInput)
            {
                button11.IsEnabled = true;
            }
            else
            {
                button11.IsEnabled = false;
            }

            _IsDataTypeReal = IsDataTypeReal;
            _IsSignedValue = IsSignedValue;

            NumericKeypad_MaxThroughput = MaxThroughput;

            button10.IsEnabled = IsSignedValue; //true = Enables Negative values to be input

        }

        //public NumericKeypad(string CurrentValue, string Title, Controller WriteController, string DBOffset, bool PercentageInput, bool IsConvertPercentToLitres, string RawMoisture, string ProductFlowrate)
        //{
        //    InitializeComponent();
        //    this.Title = "" + Title;

        //    if (CurrentValue != "-")
        //    {
        //        sText = CurrentValue;
        //    }
        //    else
        //    {
        //        sText = "0";
        //    }
        //    txt1.Text = sText;

        //    Text_Changed += new EventHandler(sText_Changed);

        //    PLC_W = WriteController;
        //    _InFlowrateOffset = DBOffset;
        //    _PercentageInput = PercentageInput;

        //    if (PercentageInput)
        //    {
        //        button11.IsEnabled = true;
        //    }
        //    else
        //    {
        //        button11.IsEnabled = false;
        //    }

        //    _IsDataTypeReal = false;
        //    _IsSignedValue = false;
        //    button10.IsEnabled = false; //true = Enables Negative values to be input

        //    _IsConvertPercentToLitres = IsConvertPercentToLitres;
        //    _RawMoisture = RawMoisture;
        //    _ProductFlowrate = ProductFlowrate;

        //}


        //------------------------------------------------------------------------------//
        //                              Event Handlers                                  //
        //------------------------------------------------------------------------------//

        public string sText
        {
            get
            {
                return _sText;
            }
            set
            {
                _sText = value;
                if (Text_Changed != null)
                {
                    Text_Changed(_sText, new EventArgs());
                }
            }
        }

        private void sText_Changed(object sender, EventArgs e)
        {
            char c = sText[0];
            if (sText.Length <= 2)
            {
                if (c == '0')
                {
                    string temp = sText;
                    if (temp != "0" && temp != "0.")
                    {
                        sText = temp.Remove(0, 1);
                    }
                }
            }

            txt1.Text = sText;
        }



        //------------------------------------------------------------------------------//
        //                              Button Clicks                                   //
        //------------------------------------------------------------------------------//


        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            sText += "1";
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            sText += "2";
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            sText += "3";
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            sText += "4";
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            sText += "5";
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            sText += "6";
        }

        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            sText += "7";
        }

        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            sText += "8";
        }

        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            sText += "9";
        }

        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            sText += "0";
        }

        //Point
        private void button11_Click(object sender, RoutedEventArgs e)
        {
            string temp = sText;
            bool containsPoint = false;

            foreach (char c in temp)
            {
                if (c == '.')
                {
                    containsPoint = true;
                    break;
                }
            }

            if (!containsPoint)
            {
                sText += ".";
            }
        }

        //Positive / Negative
        private void button10_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sText != null || sText != "0")
                {
                    decimal d = Convert.ToDecimal(sText);

                    if (d > 0)  //Number is positive and User wants it negative
                    {
                        string temp = sText;
                        sText = "-" + temp;
                    }
                    else if (d < 0)     //Number is negative and User wants it positive
                    {
                        string temp = sText.Remove(0, 1);
                        sText = temp;
                    }
                }
            }
            catch (Exception ex)
            {
                sText = "Error" + ex.Message;
            }
        }

        //Clear
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sText != null || sText != "0")
                {
                    int length = sText.Length;
                    if (length > 0)
                    {
                        string temp = sText;
                        string temp1 = "0" + temp;
                        string temp2 = temp1.Remove(1, length);
                        sText = temp2;
                    }
                }
            }
            catch (Exception ex)
            {
                sText = "Error" + ex.Message;
            }
        }


        //Backspace
        private void btnBackspace_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string temp = sText;
                if (temp != null & temp != "0")
                {
                    int length = sText.Length;
                    if (length > 0)
                    {
                        temp = temp.Remove(length - 1, 1);
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        sText = temp;

                    }
                }
            }
            catch (Exception ex)
            {
                sText = "Error" + ex.Message;
            }
        }

        //Enter
        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            bool bWithinLimits = false;

            try
            {
                //Write the value of the Control box to the InFlowrate Address
                if (_InFlowrateOffset != "DB0.DBD0")
                {

                    if (!PLC_W.IsConnected)
                        PLC_W.Connect();

                    if (PLC_W.IsConnected)
                    {
                        Tag t = new Tag(_InFlowrateOffset);

                        if (_IsDataTypeReal != true)
                        {
                            //
                            //Check datatype 
                            //
                            int iPosOfPoint = _InFlowrateOffset.IndexOf('.');
                            string sRightOfPoint = _InFlowrateOffset.Substring((iPosOfPoint + 1), 3);

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
                                if (_IsSignedValue)
                                {
                                    t.DataType = S7Link.Tag.ATOMIC.INT;
                                }
                                else
                                {
                                    t.DataType = S7Link.Tag.ATOMIC.WORD;
                                }
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


                        string sTemp = sText;

                        //
                        //  Decimal input ?
                        //
                        if (_PercentageInput)
                        {
                            if (sText.Contains('.'))
                            {
                                int iStart = sTemp.IndexOf('.');
                                sTemp = sTemp.Remove(iStart, 1);
                            }
                            else
                            {
                                sTemp += "0";
                            }

                            string sTempMinLimit = "" + NumericKeypad_MinLimit + "0";
                            string sTempMaxLimit = "" + NumericKeypad_MaxLimit + "0";

                            if (((Convert.ToInt32(sTemp) >= Convert.ToInt32(sTempMinLimit)) && (Convert.ToInt32(sTemp) <= Convert.ToInt32(sTempMaxLimit))) || (NumericKeypad_MinLimit == 0 && NumericKeypad_MaxLimit == 0))
                            {
                                bWithinLimits = true;

                                try
                                {
                                    //
                                    // Check if a conversion is required for percentage to litre dosing for the MOZF
                                    //
                                    if (Numerickeypad_IsConvertPercentDosingToLitreDosing)
                                    {
                                        //
                                        //Write the Percentage Value back to DB20. This will "hold" the value in % for display
                                        //
                                        Tag t2 = new Tag(Numerickeypad_WriteToDB20_DBOffset, S7Link.Tag.ATOMIC.WORD, 1);
                                        t2.Value = sTemp;
                                        PLC_W.WriteTag(t2);
                                    }
                                    else if (Numerickeypad_IsValue_MYFCInOffsetMoisture)
                                    {
                                        //If the value being set is the MYFC IN OFFSET MOISTURE
                                        //sText += "00";
                                        sTemp += "0";
                                        t.Value = sTemp;
                                    }
                                    else
                                    {
                                        t.Value = sTemp;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error writing Tag Value : " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }

                        }
                        else
                        {
                            if (((Convert.ToInt32(sTemp) >= NumericKeypad_MinLimit) && (Convert.ToInt32(sTemp) <= NumericKeypad_MaxLimit)) || (NumericKeypad_MinLimit == 0 && NumericKeypad_MaxLimit == 0))
                            {
                                bWithinLimits = true;

                                ////
                                ////MYFC
                                ////Add 2 Zeros to the end of the value as the MYFC InOffsetMoisture is 0.01. [Eg 1% = 0.01 on MYFC] therefore multiply by 100 [0.01 = 1.00]
                                ////
                                //if (Numerickeypad_IsValue_MYFCInOffsetMoisture)
                                //{
                                //    sText += "00";
                                //}

                                t.Value = Int32.Parse(sText);
                            }

                            //
                            // Work out the percentage to load when using Percentage dosing with multiple flowbalancers. Pass the percentage in but write the flowrate 
                            //
                            if (NumericKeypad_MaxThroughput > 0)
                            {
                                int iTempVal1 = Int32.Parse(sText);
                                int iTempVal2 = (iTempVal1 * NumericKeypad_MaxThroughput) / 100;
                                t.Value = iTempVal2;
                            }
                        }

                        t.Length = 1;

                        // Value is within range
                        if (bWithinLimits)
                        {
                            if (NumericKeypad_DivideyWriteValue > 0)
                            {
                                int sTempVal = Convert.ToInt32(t.Value);
                                sTempVal = sTempVal / NumericKeypad_MultiplyWriteValue;
                                t.Value = sTempVal;           
                            }

                            if (NumericKeypad_MultiplyWriteValue > 0)
                            {
                                int sTempVal = Convert.ToInt32(t.Value);
                                sTempVal = sTempVal * NumericKeypad_MultiplyWriteValue;
                                t.Value = sTempVal;                               
                            }

                            PLC_W.WriteTag(t);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("The value entered does not fall within the required limit.\nMinimum Allowed : " + NumericKeypad_MinLimit + "\nMaximum Allowed : " + NumericKeypad_MaxLimit, "Value Invalid", MessageBoxButton.OK, MessageBoxImage.Information);
                        }

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
                sText = "Error -->" + ex.Message;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//

        //
        //  Minimum allowable value for data entry in the numeric keypad
        //
        public int NumericKeypad_MinLimit
        {
            get
            {
                return _MinLimit;
            }
            set
            {
                _MinLimit = value;
                lblMinLimit.Content = value;
            }
        }

        //
        //  Maximum allowable value for data entry in the numeric keypad
        //
        public int NumericKeypad_MaxLimit
        {
            get
            {
                return _MaxLimit;
            }
            set
            {
                _MaxLimit = value;
                lblMaxLimit.Content = value;
            }
        }

        //
        //  
        //
        public int NumericKeypad_MaxThroughput
        {
            get
            {
                return _MaxThroughput;
            }
            set
            {
                _MaxThroughput = value;
            }
        }

        //
        //  Convert a percentage input for a MOZF into a litres per hour value for the dosing rate on the MOZF
        //
        public bool Numerickeypad_IsConvertPercentDosingToLitreDosing
        {
            get
            {
                return _IsConvertPercentToLitres;
            }
            set
            {
                _IsConvertPercentToLitres = value;
            }
        }

        //
        //  Is the entered value one for the InOffSetMoisture of the MYFC
        //
        public bool Numerickeypad_IsValue_MYFCInOffsetMoisture
        {
            get
            {
                return _IsValueMYFCInOffsetMoisture;
            }
            set
            {
                _IsValueMYFCInOffsetMoisture = value;
            }
        }

        //
        //  Used for Percentage to litre per hour dosing for the MOZF. Store the percentage moisture in DB20 and write the litre per hour value to the InMoisture
        //
        public string Numerickeypad_WriteToDB20_DBOffset
        {
            get
            {
                return _WriteToDBOffset;
            }
            set
            {
                _WriteToDBOffset = value;
            }
        }

        //
        //  Used to manipulate the value written to the MEAG control. If the MEAG requires a value of 1000 and the write value is 100, the NumericKeypad_MultiplyWriteValue would then = 10
        //
        public int NumericKeypad_MultiplyWriteValue
        {
            get
            {
                return _MultiplyWriteValue;
            }
            set
            {
                _MultiplyWriteValue = value;
            }
        }

        //
        //  Used to manipulate the value written to the MEAG control. If the MEAG requires a value of 1000 and the write value is 100, the NumericKeypad_MultiplyWriteValue would then = 10
        //
        public int NumericKeypad_DivideyWriteValue
        {
            get
            {
                return _DivideWriteValue;
            }
            set
            {
                _DivideWriteValue = value;
            }
        }
     
    }
}
