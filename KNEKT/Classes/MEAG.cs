using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KNEKT.Classes
{
                                                                                        /// <summary>
                                                                                        /// Contains the Alarms and Warnings that could appear on the MEAG controller
                                                                                        /// </summary>
    class MEAG
    {
        private int _AlarmCode;
        private string _AlarmCodeDescription;
        private string _AlarmDescription;


        //------------------------------------------------------------------------------//
        //                                  Constructor                                 //
        //------------------------------------------------------------------------------//

        public MEAG()
        {

        }


        //------------------------------------------------------------------------------//
        //                                  Methods                                     //
        //------------------------------------------------------------------------------//

        private void GetAlarmDescriptionFromCode(int code)
        {
            switch (code)
            {
                case 0:
                    MEAG_AlarmCodeDescription_Internal = "A000";
                    MEAG_AlarmDescription_Internal = "No Alarm";
                    break;

                case 1:
                    MEAG_AlarmCodeDescription_Internal = "A001";
                    MEAG_AlarmDescription_Internal = "EPROM - The flash is defective";
                    break;

                case 2:
                    MEAG_AlarmCodeDescription_Internal = "A002";
                    MEAG_AlarmDescription_Internal = "RAM - The RAM is defective";
                    break;

                case 3:
                    MEAG_AlarmCodeDescription_Internal = "A003";
                    MEAG_AlarmDescription_Internal = "RUNTIME - Program runtime error occurred";
                    break;

                case 4:
                    MEAG_AlarmCodeDescription_Internal = "A004";
                    MEAG_AlarmDescription_Internal = "WATCHDOG - Program cycle exceeded its normal time limit";
                    break;

                case 5:
                    MEAG_AlarmCodeDescription_Internal = "A005";
                    MEAG_AlarmDescription_Internal = "24V PSU - Voltage is too low (<18V)";
                    break;

                case 6:
                    MEAG_AlarmCodeDescription_Internal = "A006";
                    MEAG_AlarmDescription_Internal = "DATA LOSS - Loss of data in battery buffered RAM";
                    break;

                case 7:
                    MEAG_AlarmCodeDescription_Internal = "A007";
                    MEAG_AlarmDescription_Internal = "RANGE - Measuring range is outside of the limits";
                    break;

                case 8:
                    MEAG_AlarmCodeDescription_Internal = "A008";
                    MEAG_AlarmDescription_Internal = "ADCAL - AD values are insufficient for calibration";
                    break;

                case 9:
                    MEAG_AlarmCodeDescription_Internal = "A009";
                    MEAG_AlarmDescription_Internal = "ADFCT - The AD converter module supplies no values";
                    break;

                case 10:
                    MEAG_AlarmCodeDescription_Internal = "A010";
                    MEAG_AlarmDescription_Internal = "AD10V - The sensor supply is outside the permissible tolerance";
                    break;

                case 11:
                    MEAG_AlarmCodeDescription_Internal = "A011";
                    MEAG_AlarmDescription_Internal = "LOW4MA - Setpoint less thatn 4 mA";
                    break;

                case 12:
                    MEAG_AlarmCodeDescription_Internal = "A012";
                    MEAG_AlarmDescription_Internal = "HOSTCOM - Host system unable to communicate with device";
                    break;

                case 13:
                    MEAG_AlarmCodeDescription_Internal = "W013";
                    MEAG_AlarmDescription_Internal = "EXTDISP - Serial Communication to remote control has been interrupted";
                    break;

                case 14:
                    MEAG_AlarmCodeDescription_Internal = "A014";
                    MEAG_AlarmDescription_Internal = "PROFIDP - Profibus communication interrupted";
                    break;

                case 16:
                    MEAG_AlarmCodeDescription_Internal = "W016";
                    MEAG_AlarmDescription_Internal = "PRINTER - Printer Fault";
                    break;

                case 17:
                    MEAG_AlarmCodeDescription_Internal = "W017";
                    MEAG_AlarmDescription_Internal = "DISPLAY - Communication with primary display has been interrupted";
                    break;

                case 18:
                    MEAG_AlarmCodeDescription_Internal = "W018";
                    MEAG_AlarmDescription_Internal = "BATTERY - Buffer battery has insufficient voltage";
                    break;

                case 20:
                    MEAG_AlarmCodeDescription_Internal = "A020";
                    MEAG_AlarmDescription_Internal = "MOVE - The scale weight is not steady";
                    break;

                case 21:
                    MEAG_AlarmCodeDescription_Internal = "A021";
                    MEAG_AlarmDescription_Internal = "DISCTIME - The scale is not emptied within the given time";
                    break;

                case 22:
                    MEAG_AlarmCodeDescription_Internal = "A022";
                    MEAG_AlarmDescription_Internal = "DISCOPEN - Outlet slide gate is open when metering starts";
                    break;

                case 23:
                    MEAG_AlarmCodeDescription_Internal = "A023";
                    MEAG_AlarmDescription_Internal = "HOPPER - The probe is still covered after emptying";
                    break;

                case 26:
                    MEAG_AlarmCodeDescription_Internal = "A026";
                    MEAG_AlarmDescription_Internal = "TOLERANCE - The controller continously works outside the bulk tolerance";
                    break;

                case 29:
                    MEAG_AlarmCodeDescription_Internal = "A029";
                    MEAG_AlarmDescription_Internal = "PTIME - Dosing Time exceeded";
                    break;

                case 30:
                    MEAG_AlarmCodeDescription_Internal = "A030";
                    MEAG_AlarmDescription_Internal = "ZERO - Excessive weight deviation when zeroing";
                    break;

                case 32:
                    MEAG_AlarmCodeDescription_Internal = "A032";
                    MEAG_AlarmDescription_Internal = "INOPEN - The inlet slide gate is open during standby";
                    break;

                case 33:
                    MEAG_AlarmCodeDescription_Internal = "A033";
                    MEAG_AlarmDescription_Internal = "PRESSURE - Drop in air pressure";
                    break;

                case 37:
                    MEAG_AlarmCodeDescription_Internal = "A037";
                    MEAG_AlarmDescription_Internal = "BIN EMPTY - Secure the product feed";
                    break;

                case 38:
                    MEAG_AlarmCodeDescription_Internal = "A038";
                    MEAG_AlarmDescription_Internal = "SLIDE GATE - Metering slide gate is not closing properly";
                    break;

                case 39:
                    MEAG_AlarmCodeDescription_Internal = "A039";
                    MEAG_AlarmDescription_Internal = "LEAK - Leak in pneumatic control system when operating with product";
                    break;

                case 42:
                    MEAG_AlarmCodeDescription_Internal = "ERR 42";
                    MEAG_AlarmDescription_Internal = "LIMIT - Maximum dosing rate reached for 1 minute";
                    break;

                case 54:
                    MEAG_AlarmCodeDescription_Internal = "ERR 54";
                    MEAG_AlarmDescription_Internal = "REL - Relay on MOZF does not work correctly";
                    break;

                case 55:
                    MEAG_AlarmCodeDescription_Internal = "ERR 55";
                    MEAG_AlarmDescription_Internal = "MOIST - Calculated moisture is outside of tolerance range for more than 1 minute";
                    break;

                case 56:
                    MEAG_AlarmCodeDescription_Internal = "ERR 56";
                    MEAG_AlarmDescription_Internal = "INTT - Internal temperature outside of tolerance range";
                    break;

                case 57:
                    MEAG_AlarmCodeDescription_Internal = "ERR 57";
                    MEAG_AlarmDescription_Internal = "EXTT - External temperature outside of tolerance range";
                    break;

                case 58:
                    MEAG_AlarmCodeDescription_Internal = "ERR 58";
                    MEAG_AlarmDescription_Internal = "HF - Measured attenuation lies outside tolerance range";
                    break;

                case 59:
                    MEAG_AlarmCodeDescription_Internal = "ERR 59";
                    MEAG_AlarmDescription_Internal = "WT - Measured bulk density lies outside of tolerance range";
                    break;

                case 60:
                    MEAG_AlarmCodeDescription_Internal = "ERR 60";
                    MEAG_AlarmDescription_Internal = "NIV - Meaured level lies outside of tolerance";
                    break;

                case 61:
                    MEAG_AlarmCodeDescription_Internal = "ERR 61";
                    MEAG_AlarmDescription_Internal = "SWERR - Internal security monitoring has reacted";
                    break;

                case 62:
                    MEAG_AlarmCodeDescription_Internal = "ERR 62";
                    MEAG_AlarmDescription_Internal = "BOARD - Error on extension board";
                    break;

                case 63:
                    MEAG_AlarmCodeDescription_Internal = "ERR 63";
                    MEAG_AlarmDescription_Internal = "FLOWD - MOZL probe has detected error";
                    break;

                case 64:
                    MEAG_AlarmCodeDescription_Internal = "ERR 64";
                    MEAG_AlarmDescription_Internal = "MOZE - A MOZE Alarm has occurred";
                    break;

                case 65:
                    MEAG_AlarmCodeDescription_Internal = "ERR 65";
                    MEAG_AlarmDescription_Internal = "QMIN - Minimum permissible water dosing rate";
                    break;

                case 66:
                    MEAG_AlarmCodeDescription_Internal = "ERR 66";
                    MEAG_AlarmDescription_Internal = "DMAX - Maximum water dosing rate";
                    break;

                case 67:
                    MEAG_AlarmCodeDescription_Internal = "A067";
                    MEAG_AlarmDescription_Internal = "QTOL - MOZF Error";
                    break;

                case 70:
                    MEAG_AlarmCodeDescription_Internal = "A070";
                    MEAG_AlarmDescription_Internal = "CANCOM - Can bus comminucation disrupted";
                    break;

                case 71:
                    MEAG_AlarmCodeDescription_Internal = "A071";
                    MEAG_AlarmDescription_Internal = "CANMOD - CAN module error";
                    break;

                case 72:
                    MEAG_AlarmCodeDescription_Internal = "A072";
                    MEAG_AlarmDescription_Internal = "CANDOUT - CAN module output error";
                    break;

                case 73:
                    MEAG_AlarmCodeDescription_Internal = "A073";
                    MEAG_AlarmDescription_Internal = "DEVNR - Incorrect LC module device number";
                    break;

                case 75:
                    MEAG_AlarmCodeDescription_Internal = "W075";
                    MEAG_AlarmDescription_Internal = "WARNTEMP - The temperature in the device is high";
                    break;

                case 76:
                    MEAG_AlarmCodeDescription_Internal = "A076";
                    MEAG_AlarmDescription_Internal = "HIGHTEMP - The temperature in the device has exceeded the permissible threshold values";
                    break;

                case 77:
                    MEAG_AlarmCodeDescription_Internal = "A077";
                    MEAG_AlarmDescription_Internal = "DIGOUT - One or more outputs deviate from expected behaviour";
                    break;

                case 94:
                    MEAG_AlarmCodeDescription_Internal = "A094";
                    MEAG_AlarmDescription_Internal = "EN24V - 24V Enable Lost";
                    break;

                case 96:
                    MEAG_AlarmCodeDescription_Internal = "A096";
                    MEAG_AlarmDescription_Internal = "EN24V - 24V Enable Lost";
                    break;

                default:
                    MEAG_AlarmCodeDescription_Internal = "Axxx";
                    MEAG_AlarmDescription_Internal = "Unknown - Please check manual for more information";
                    break;
            }
        }


        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        public int MEAG_AlarmCode
        {
            get
            {
                return _AlarmCode;
            }
            set
            {
                _AlarmCode = value;
                GetAlarmDescriptionFromCode(value);
            }
        }

        public string MEAG_AlarmCodeDescription
        {
            get
            {
                return _AlarmCodeDescription;
            }
        }

        private string MEAG_AlarmCodeDescription_Internal
        {
            set
            {
                _AlarmCodeDescription = value;
            }
        }

        public string MEAG_AlarmDescritpion
        {
            get
            {
                return _AlarmDescription;
            }
        }

        public string MEAG_AlarmDescription_Internal
        {
            set
            {
                _AlarmDescription = value;
            }
        }
    }
}
