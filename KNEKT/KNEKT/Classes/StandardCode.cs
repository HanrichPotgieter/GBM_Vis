using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Security.AccessControl;
using System.Xml;
using System.Xml.Linq;
using System.Security.Principal;

namespace KNEKT.Classes
{
                                                                                        /// <summary>
                                                                                        /// This class contains all default code that is used in the application 
                                                                                        /// </summary>
    public class StandardCode
    {
        public ArrayList alNumbers = new ArrayList(){0,1,2,3,4,5,6,7,8,9};
        

        //------------------------------------------------------------------------------//
        //                              General Methods                                 //
        //------------------------------------------------------------------------------//


                                                                                        /// <summary>
                                                                                        /// Check if current Operating system is 32 Bit or 64 Bit
                                                                                        /// </summary>
                                                                                        /// <returns>True if 32 bit</returns>
        public bool IsOperatingSystem32Bit()
        {
            if (System.Environment.Is64BitOperatingSystem)
            {
                return false;
            }
            else
            {
                return true;
            }
        }     

                                                                                        /// <summary>
                                                                                        /// Splits a string into 2 parts using the supplied Character delimeter
                                                                                        /// </summary>
                                                                                        /// <param name="Delimeter">Character to use to split the string on</param>
                                                                                        /// <param name="Value">The string to split</param>
                                                                                        /// <returns>The split string</returns>
        public string[] SplitStringInto2(char Delimeter, string Value)
        {
            string[] words = new string[2];

            int indexDelimeter = Value.IndexOf(Delimeter);
            string LeftString = Value.Substring(0, indexDelimeter);
            int stringlength = Value.Length - 1;
            stringlength = stringlength - indexDelimeter;

            string RightString = Value.Substring((indexDelimeter + 1), stringlength);

            words[0] = LeftString;
            words[1] = RightString;

            return words;
        }
                                                                                        /// <summary>
                                                                                        /// Get exactly which bit is on in the DWORD and handle accordingly
                                                                                        /// </summary>
                                                                                        /// <param name="TagValue"></param>
                                                                                        /// <returns>State code</returns>
        public int GetRelevantBitValue(long TagValue)
        {
            int returnValue = 0;

            string s = Convert.ToString(TagValue, 2);               //Convert the Tags Value to a binary Number to see exactly which bits are on in the Double word
            string temp = s;

            for (int i = s.Length; i < 32; i++)                     //Fill the binary number with zero's from the front of the number to ensure the length is 32 bits(DWORD)
            {
                temp = "0" + temp;
            }

            char[] binaryValues = temp.ToCharArray();               //Split the string into a character array
            Array.Reverse(binaryValues);

            //
            //If any of these bits are on that means that there is a fault in the [PCREAD.FAULT] of the sections DB, therefore return 32 as the state code
            //
            if (binaryValues[0] == '1' || binaryValues[2] == '1' || binaryValues[3] == '1' ||
                binaryValues[4] == '1' || binaryValues[5] == '1' || binaryValues[6] == '1' ||
                binaryValues[8] == '1' || binaryValues[9] == '1' || binaryValues[10] == '1' || binaryValues[11] == '1' ||
                binaryValues[13] == '1' ||
                binaryValues[21] == '1' || binaryValues[22] == '1')
            {
                returnValue = 32;
            }

            if (binaryValues[1] == '1' || binaryValues[23] == '1')
            {
                returnValue = 20;
            }

            // [0] ErrEmpty
            // [1] ErrFull
            // [2] ErrWayConflict
            // [3] ErrWarning
            // [4] ErrMech
            // [5] ErrDosing
            // [6] ErrEmptyJobReq
            // [8] ErrBatchSize
            // [9] ErrRefilLevel
            // [10] ErrSectionTimeout
            // [11] ErrManualAdditionTimeout
            // [13] ErrProductMismatch
            // [21] ErrDischargeReq
            // [22] ErrStartupTimeout
            // [23] ErrNewBinTimeOut


            return returnValue;
        }


                                                                                        /// <summary>
                                                                                        /// Returns the amount of water required (in litres per hour) from the given target moisture (in percentage)
                                                                                        /// </summary>
                                                                                        /// <param name="NominalMoisture">Percentage of water to add. eg 2.5%</param>
                                                                                        /// <param name="RawMoisture">Actual Moisture</param>
                                                                                        /// <param name="ProductFlowrate">Product Flowrate</param>
                                                                                        /// <returns>litres of water per hour required. Value is -1 if there was an exception</returns>
        public double ConvertPercentageDosingToLitreDosing(double NominalMoisture, double RawMoisture, double ProductFlowrate)
        {
            double dResult = 0;

            try
            {
                /*
                 * Qh20 = Qproduct * ( (fNominal - fRaw) / 100% - fNominal) )
                 * 
                 * Qh20     = Dosing Rate in Litres per Hour
                 * Qproduct = productRate in kg/h
                 * fRaw     = Raw moisutre in %
                 * fNominal = Nominal Moisture in %
                 * 
                 */

                double d1 = (NominalMoisture / 10);// - RawMoisture);
                double d2 = (100 - (d1 + RawMoisture));
                double d3 = (d1 / d2);
                double d4 = ProductFlowrate * d3;
                dResult = d4;

            }
            catch (Exception)
            {                
                dResult = -1;
            }

            return dResult;
        }



                                                                                        /// <summary>
                                                                                        /// Checks that PLC Settings are in correct format before proceeding
                                                                                        /// </summary>
                                                                                        /// <returns>true if the IP, Rack and Slot are in the correct format</returns>
        public bool ValidatePLCConfiguration(string IPAddress, string RackNumber, string SlotNumber)
        {
            bool valid = false;
            bool bIP = false;
            bool bRack = false;
            bool bSlot = false;

            //
            // IP ADDRESS
            //
            int iPointCount = 0;
            foreach (char c in IPAddress)
            {
                if (c == '.')
                {
                    iPointCount++;
                }
                else if (!alNumbers.Contains(c))
                {
                    //iPointCount = 0;
                  //  break;
                }
            }
            if (iPointCount == 3)   //Must be 3 [.]s 
            {
                bIP = true;
            }

            //
            // RACK
            //
            foreach (char c in RackNumber)
            {
                if (alNumbers.Contains(c))
                {
                    bRack = false;
                    break;
                }
                bRack = true;
            }

            //
            // SLOT
            //
            foreach (char c in SlotNumber)
            {
                if (alNumbers.Contains(c))
                {
                    bSlot = false;
                    break;
                }
                bSlot = true;
            }

            if (!bIP || !bRack || !bSlot)
            {
                valid = false;
            }
            else
            {
                valid = true;
            }

            return valid;
        }


                                                                                        /// <summary>
                                                                                        /// Tests the connection to SQL Server
                                                                                        /// </summary>
                                                                                        /// <returns>true if connection succeeds</returns>
        public bool TestSQLDatabaseConnection(string sqlConnectionString)
        {
            bool bValidConnection = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConnectionString))
                {
                    conn.Open();
                    conn.Close();
                    bValidConnection = true;
                }
            }
            catch
            {
                bValidConnection = false;
            }

            return bValidConnection;
        }

                                                                                        /// <summary>
                                                                                        /// Remove all entries in the arraylist that fall out of bounds of the limit
                                                                                        /// </summary>
                                                                                        /// <returns>Bool, true if no exception has occured</returns>
        public bool CleanUpLogAL()
        {
            bool bSuccess = false;

            int itemLimit = 5000;
            int itemCount = MainWindow.alLoggerToUI.Count;
            int itemsToRemove = 0;

            try
            {
                if (itemCount > itemLimit)
                {
                    itemsToRemove = itemCount - itemLimit;

                    //
                    //  Start at zero (Removes oldest entries)
                    //
                    for (int i = itemsToRemove; i > 0; i--)
                    {
                        if (i <= 0)
                        {
                            break;
                        }
                        else
                        {
                            MainWindow.alLoggerToUI.RemoveAt(i);
                        }
                    }
                }
                bSuccess = true;
            }
            catch (Exception)
            {
                bSuccess = false;
            }

            return bSuccess;
        }




                                                                                        


        //------------------------------------------------------------------------------//
        //        ***          General Static Methods             ***                   //
        //------------------------------------------------------------------------------//
                                                                                        /// <summary>
                                                                                        /// Closes all other open windows, all except the main application window
                                                                                        /// </summary>
                                                                                        /// <param name="windowCollection">Collection of open windows from the main application</param>
        public static void CloseAllOpenWindows(WindowCollection windowCollection)
        {
            foreach (Window window in windowCollection)
            {
                string str = window.Title;
                if (str != "MainWindow")
                {
                    window.Close();
                }
            }
        }

                                                                                        /// <summary>
                                                                                        /// Closes the specified open window
                                                                                        /// </summary>
                                                                                        /// <param name="windowCollection">Collection of open windows from the main application</param>
        public static void CloseSpecificWindow(WindowCollection windowCollection, string WindowTitle)
        {
            foreach (Window window in windowCollection)
            {
                string str = window.Title;
                if (str == WindowTitle)
                {
                    window.Close();
                }
            }
        }

                                                                                        /// <summary>
                                                                                        /// Check if a window with the specified name is open
                                                                                        /// </summary>
                                                                                        /// <param name="windowCollection">Collection of open windows from the main application</param>
        public static bool IsSpecificWindowOpen(WindowCollection windowCollection, string WindowTitle)
        {
            bool bOpen = false;
            foreach (Window window in windowCollection)
            {
                string str = window.Title;
                if (str == WindowTitle)
                {
                    bOpen = true;
                }
            }
            return bOpen;
        }





        //------------------------------------------------------------------------------//
        //                              File Handling                                   //
        //------------------------------------------------------------------------------//
                                                                                        /// <summary>
                                                                                        /// Creates the required directory structure for the application
                                                                                        /// </summary>
                                                                                        /// <returns></returns>
        public bool CreateDirectoryStructure()
        {
            bool bCreated = false;

            string sKNEKTDirectory = "";
            string sDLLDirectory = @"";
            string sResourceFolder = @"Resources\";
            string sManualFolder = @"Manuals\";
            string sVideoFolder = @"Videos\";
            string sBinFolder = @"Bin\";
            string sBin32Folder = @"x86\";
            string sBin64Folder = @"x64\";

            try
            {
                if (IsOperatingSystem32Bit())
                {
                    sKNEKTDirectory = @"C:\Program Files\Buhler AE\KNEKT\";
                    sDLLDirectory = @"C:\Program Files\Buhler AE\Common\";
                    MainWindow.stat_OSKPath = sKNEKTDirectory + sBinFolder + sBin32Folder;
                    MainWindow.sVNCDirectory = @"C:\Program Files\UltraVNC\";
                }
                else
                {
                    sKNEKTDirectory = @"C:\Program Files (x86)\Buhler AE\KNEKT\";
                    sDLLDirectory = @"C:\Program Files (x86)\Buhler AE\Common\";
                    MainWindow.stat_OSKPath = sKNEKTDirectory + sBinFolder + sBin64Folder;
                    MainWindow.sVNCDirectory = @"C:\Program Files (x86)\UltraVNC\";
                }


                if (!Directory.Exists(sKNEKTDirectory))
                {
                    Directory.CreateDirectory(sKNEKTDirectory);
                    DirectoryInfo dInfo = new DirectoryInfo(@"" + sKNEKTDirectory);
                    DirectorySecurity dSecurity = dInfo.GetAccessControl();
                    dSecurity.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));
                    Directory.SetAccessControl(@"" + sKNEKTDirectory, dSecurity);
                }
                
                if (!Directory.Exists(sKNEKTDirectory + sResourceFolder))
                {
                    string currentResDir = sKNEKTDirectory + sResourceFolder;
                    Directory.CreateDirectory(currentResDir);
                    DirectoryInfo dInfo = new DirectoryInfo(@"" + currentResDir);
                    DirectorySecurity dSecurity = dInfo.GetAccessControl();
                    dSecurity.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));
                    Directory.SetAccessControl(@"" + currentResDir, dSecurity);
                }

                if (!Directory.Exists(sKNEKTDirectory + sResourceFolder + sVideoFolder))
                {
                    string currentResVidDir = sKNEKTDirectory + sResourceFolder + sVideoFolder;
                   // Directory.CreateDirectory(sKNEKTDirectory + sResourceFolder + sVideoFolder);
                    Directory.CreateDirectory(currentResVidDir);
                    DirectoryInfo dInfo = new DirectoryInfo(@"" + currentResVidDir);
                    DirectorySecurity dSecurity = dInfo.GetAccessControl();
                    dSecurity.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));
                    Directory.SetAccessControl(@"" + currentResVidDir, dSecurity);
                }

                if (!Directory.Exists(sKNEKTDirectory + sResourceFolder + sManualFolder))
                {
                    string currentResManDir = sKNEKTDirectory + sResourceFolder + sManualFolder;
//                    Directory.CreateDirectory(sKNEKTDirectory + sResourceFolder + sManualFolder);
                    Directory.CreateDirectory(currentResManDir);
                    DirectoryInfo dInfo = new DirectoryInfo(@"" + currentResManDir);
                    DirectorySecurity dSecurity = dInfo.GetAccessControl();
                    dSecurity.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));
                    Directory.SetAccessControl(@"" + currentResManDir, dSecurity);
                }

                if (!Directory.Exists(sKNEKTDirectory + sBinFolder))
                {
                    string currentBinDir = sKNEKTDirectory + sBinFolder;
                    Directory.CreateDirectory(currentBinDir);
                    DirectoryInfo dInfo = new DirectoryInfo(@"" + currentBinDir);
                    DirectorySecurity dSecurity = dInfo.GetAccessControl();
                    dSecurity.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));
                    Directory.SetAccessControl(@"" + currentBinDir, dSecurity);
                }

                if (!Directory.Exists(sKNEKTDirectory + sBinFolder + sBin32Folder))
                {
                    string currentBin32Dir = sKNEKTDirectory + sBinFolder + sBin32Folder;
                    Directory.CreateDirectory(currentBin32Dir);
                    DirectoryInfo dInfo = new DirectoryInfo(@"" + currentBin32Dir);
                    DirectorySecurity dSecurity = dInfo.GetAccessControl();
                    dSecurity.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));
                    Directory.SetAccessControl(@"" + currentBin32Dir, dSecurity);
                }

                if (!Directory.Exists(sKNEKTDirectory + sBinFolder + sBin64Folder))
                {
                    string currentBin64Dir = sKNEKTDirectory + sBinFolder + sBin64Folder;
                    Directory.CreateDirectory(currentBin64Dir);
                    DirectoryInfo dInfo = new DirectoryInfo(@"" + currentBin64Dir);
                    DirectorySecurity dSecurity = dInfo.GetAccessControl();
                    dSecurity.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));
                    Directory.SetAccessControl(@"" + currentBin64Dir, dSecurity);
                }




                MainWindow.KNEKTDirectory = sKNEKTDirectory;
                MainWindow.XMLSettingFile = sKNEKTDirectory + MainWindow.XMLSettingFile;
                MainWindow.DllDirectory = sDLLDirectory;
                MainWindow.stat_HelpManualFolder = sKNEKTDirectory + sResourceFolder + sManualFolder;
                MainWindow.stat_HelpVideoFolder = sKNEKTDirectory + sResourceFolder + sVideoFolder;

                //if(!File.Exists(sKNEKTDirectory + ))
                //{

                //}

                //
                //Copy the OSK.exe into the relevant folder
                //
                if (!File.Exists(MainWindow.stat_OSKPath + "osk.exe"))
                    File.Copy(@"C:\Windows\System32\osk.exe", MainWindow.stat_OSKPath + "osk.exe");

                bCreated = true;
            }
            catch (Exception)
            {
                bCreated = false;
            }

            return bCreated;
        }


        //------------------------------------------------------------------------------//
        //                              Database Methods                                //
        //------------------------------------------------------------------------------//

                                                                                        /// <summary>
                                                                                        /// Load all application settings from SQL
                                                                                        /// </summary>
                                                                                        /// <param name="sqlConnectionString">Connection string to use to connect to the database</param>
                                                                                        /// <returns>True if all settings were loaded without exception</returns>
        public bool LoadApplicationSettings(string sqlConnectionString)
        {
            bool bValid = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT SettingID, SettingValue FROM ApplicationSettings";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string sID = reader.GetString(0);
                        string sValue = reader.GetString(1);

                        switch (sID)
                        {
                            case "PLC_IP":
                                MainWindow.PLCIpAddress = sValue;
                                break;

                            case "PLC_RACK":
                                MainWindow.PLCRackNo = sValue;
                                break;

                            case "PLC_SLOT":
                                MainWindow.PLCSlotNum = sValue;
                                break;

                            case "USR_LOGOFF":
                                int seconds = Int32.Parse(sValue);
                                int minutes = 0;
                                if (seconds > 0)
                                {
                                    minutes = seconds / 60;
                                }
                                else
                                {
                                    minutes = 0;
                                }
                                MainWindow.stat_iLogOffTime = minutes;
                                break;

                            case "VIS_TAGNAMES":
                                MainWindow.bShowTagnames = Convert.ToBoolean(sValue);
                                break;

                            case "APP_HINTS":
                                MainWindow.stat_bShowApplicationHints = Convert.ToBoolean(sValue);

                                //if (MainWindow.stat_bShowApplicationHints == true)
                                //{
                                //    LoadApplicationHints(sqlConnectionString);
                                    
                                //}

                                break;

                            case "SYS_MULTITOUCH_S1":
                                MainWindow.stat_bMultitouchS1 = Convert.ToBoolean(sValue);
                                break;

                            case "APP_REPORTVIEWPATH":
                                MainWindow.stat_ReportViewerAddress = sValue;
                                break;
                        }
                    }
                }

                //GetSettingsFromXMLFile();

                bValid = true;
            }
            catch (Exception)
            {
                bValid = false;
            }

            return bValid;
        }


                                                                                        /// <summary>
                                                                                        /// Load application hints from SQL
                                                                                        /// </summary>
                                                                                        /// <param name="sqlConnectionString">Connection string to use to connect to the database</param>
                                                                                        /// <returns>Arraylist containing data type string. Each string is a hint. If list is empty an exception was caught</returns>
        public ArrayList LoadApplicationHints(string sqlConnectionString)
        {
            ArrayList alhints = new ArrayList();
            try
            {

                using (SqlConnection sqlConn = new SqlConnection(sqlConnectionString))  //Return All lines that have values in their lineDB
                {
                    sqlConn.Open();
                    SqlCommand cmd = sqlConn.CreateCommand();
                    cmd.CommandText = "SELECT HintText FROM ApplicationHints";
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        alhints.Add(reader.GetString(0));
                    }
                    sqlConn.Close();
                }
            }
            catch (Exception)
            {
                alhints.Clear();
            }

            return alhints;
        }



                                                                                        /// <summary>
                                                                                        /// Returns the application log for the past 2 days
                                                                                        /// </summary>
                                                                                        /// <param name="sqlConnectionString">Connection string to use to connect to the database</param>
                                                                                        /// <returns>Arraylist containing data type LogItem. If list is empty an exception was caught</returns>
        public ArrayList GetUILogItems(string sqlConnectionString)
        {
            ArrayList alLogItems = new ArrayList();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM applicationlog WHERE ts_DateTime >= DATEADD(dd,-2,GETDATE()) AND Code <> 30 ORDER BY OADate ASC";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        LogItem li = new LogItem(reader.GetDateTime(0), reader.GetDouble(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4));
                        alLogItems.Add(li);
                    }
                }
            }
            catch (Exception)
            {
                alLogItems.Clear();
            }

            return alLogItems;
        }


                                                                                        /// <summary>
                                                                                        /// Saves the zoom value of the selected page to the database
                                                                                        /// </summary>
                                                                                        /// <param name="sqlConnectionString">Connection string to use to connect to the database</param>
                                                                                        /// <param name="SettingID">Setting ID in the ApplicationSettings table to update</param>
                                                                                        /// <param name="Value">Value of the slider at its current position</param>
                                                                                        /// 
        public bool SaveZoomValue(string sqlConnectionString, string SettingID, string Value)
        {
            bool bSettingSaved = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE ApplicationSettings SET SettingValue = '" + Value + "' WHERE SettingID = '" + SettingID + "'";
                    int rowsUpdated = cmd.ExecuteNonQuery();
                    if (rowsUpdated > 0)
                    {
                        bSettingSaved = true;
                    }
                }
            }
            catch (Exception)
            {
                bSettingSaved = false;
            }

            return bSettingSaved;
        }


                                                                                        /// <summary>
                                                                                        /// Return the database credentials that are encrypted
                                                                                        /// </summary>
        internal void GetDatabaseCredentials()
        {
            FileStream fs = null;
            StreamReader sr = null;

            if (File.Exists(@"" + MainWindow.dllName))
            {
                try
                {
                    fs = new FileStream(@"" + MainWindow.dllName, FileMode.Open, FileAccess.Read);
                    sr = new StreamReader(fs);
                    int lineCount = 1;

                    SimpleAES aes = new SimpleAES();

                    string s = sr.ReadLine();

                    //Is no data in newly created file
                    if (s != null)
                    {
                        string read = aes.DecryptString(s);
                        while (read != null)
                        {
                            //MessageBox.Show(read.ToString());
                            if (lineCount == 1)
                            {
                                MainWindow.sqlServername = read.ToString();
                            }
                            else if (lineCount == 2)
                            {
                                MainWindow.sqlDatabase = read.ToString();
                            }
                            else if (lineCount == 3)
                            {
                                MainWindow.sqlUsername = read.ToString();
                            }
                            else if (lineCount == 4)
                            {
                                MainWindow.sqlPassword = read.ToString();
                                sr.Close();
                                fs.Close();
                                break;
                            }
                            read = aes.DecryptString(sr.ReadLine());
                            lineCount++;
                        }
                    }
                    sr.Close();
                    fs.Close();
                }
                catch (Exception)
                {
                    MainWindow.sqlServername = "";
                    MainWindow.sqlDatabase = "";
                    MainWindow.sqlUsername = "";
                    MainWindow.sqlPassword = "";
                }
                finally
                {
                    sr.Close();
                    fs.Close();
                }
            }
            else
            {
                try
                {
                    if (!File.Exists(@"" + MainWindow.dllName))
                    {
                        FileStream fs1 = File.Create(@"" + MainWindow.dllName);
                        fs1.Close();
                    }
                }
                catch (Exception)
                {
                    MainWindow.dllName = "";
                }
            }

        }

                                                                                        /// <summary>
                                                                                        /// This method gets the application settings from the XML configuration file
                                                                                        /// </summary>
        public void GetSettingsFromXMLFile()
        {
            bool bSettingsRead = false;
            int iSettingCount = 0;
            string xmlFileName = MainWindow.XMLSettingFile;
            if(File.Exists(xmlFileName))
            {
                try
                {
                    ArrayList al = ReadXMLFile(MainWindow.XMLSettingFile, "setting", "name", "value");
                    
                    foreach (string[] s in al)
                    {
                        iSettingCount++;
                        switch (s[0])
                        {
                            case "PANEL_ID":
                                MainWindow.PanelID = Convert.ToInt32(s[1]);
                                break;
                        }
                    }

                    
                }
                catch (Exception ae)
                {
                    MessageBox.Show("GetSettingsFromXML -->" + ae.ToString(), "Get Settings From Configuration File", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                try
                {
                    if (!File.Exists(@"" + xmlFileName))
                    {
                        CreateXMLSettingsFile();
                    }
                }
                catch (Exception)
                {
                    
                }
            }
            //return bSettingsRead;
        }

        

                                                                                /// <summary>
                                                                                /// Returns the Attributes along with their values from the given XML file
                                                                                /// </summary>
                                                                                /// <param name="XmlFilePath">The physical path of the XML file</param>
                                                                                /// <param name="ParentNodeName">The node to read in the XML file</param>
                                                                                /// <param name="ParentNodeAttributeName">The Nodes attribute name to search for</param>
                                                                                /// <param name="ChildNodeName">The Child Node to search for the value</param>
                                                                                /// <returns>ArrayList of two dimensional string arrays</returns>
        public ArrayList ReadXMLFile(string XmlFilePath, string ParentNodeName, string ParentNodeAttributeName, string ChildNodeName)
        {
            ArrayList alValues = new ArrayList();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(XmlFilePath);
                XmlNodeList settingsList = xmlDoc.SelectNodes("//" + ParentNodeName);

                foreach (XmlNode setting in settingsList)
                {
                    string sNodeAttribute = setting.Attributes[ParentNodeAttributeName].Value;
                    string sNodeValue = "";

                    XmlNode xmlValue = setting.SelectSingleNode(ChildNodeName);

                    if (xmlValue != null)
                    {
                        sNodeValue = xmlValue.InnerText;
                    }

                    alValues.Add(new string[2] { sNodeAttribute, sNodeValue });
                }
            }
            catch (Exception ex)
            {
                alValues.Add(new string[2] { "Error", ex.Message });
            }

            return alValues;
        }

        /// <summary>
        /// Updates the value of the given XML node
        /// </summary>
        /// <param name="XmlFilePath">The physical path of the XML file</param>
        /// <param name="ParentNodeName">The node to read in the XML file</param>
        /// <param name="ParentNodeAttributeName">The Nodes attribute name to search for</param>
        /// <param name="ChildNodeName">The Child Node to search for to perform an update on</param>
        /// <param name="SettingNameToUpdate">The Setting name to update</param>
        /// <param name="Value">New value of the child node</param>
        /// <returns>bool value, true if success, false if failed</returns>
        public static bool WriteXMLValue(string XmlFilePath, string ParentNodeName, string ParentNodeAttributeName, string ChildNodeName, string SettingNameToUpdate, string Value)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(XmlFilePath);

                XmlNodeList settingsList = xmlDoc.SelectNodes("//" + ParentNodeName);

                foreach (XmlNode setting in settingsList)
                {
                    if (setting.Attributes[ParentNodeAttributeName].Value == SettingNameToUpdate)
                    {
                        XmlNode node1 = setting.SelectSingleNode(ChildNodeName);

                        if (node1 != null)
                        {
                            node1.InnerText = Value;
                            //node1.AppendChild(xmlDoc.CreateElement("subnode1"));
                        }
                    }
                }

                xmlDoc.Save(XmlFilePath);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

                                                                                                    /// <summary>
                                                                                                    /// This method creates the XML settings file if it does not exist
                                                                                                    /// </summary>
        private void CreateXMLSettingsFile()
        {
            //MainWindow.XMLSettingFile = MainWindow.KNEKTDirectory + MainWindow.XMLSettingFile;
            if (!File.Exists(@"" + MainWindow.XMLSettingFile))
            {
                try
                {
                    XDocument XDocNewSettingsFile = new XDocument(
                        new XElement("KNEKT",
                            new XElement("applicationSettings",
                                new XElement("setting", new XAttribute("name", "PANEL_ID"),
                                    new XElement("value", "1"))
                                )));
                    XDocNewSettingsFile.Save(MainWindow.XMLSettingFile);

                    FileSecurity fSecurity = File.GetAccessControl(@"" + MainWindow.XMLSettingFile);
                    fSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, AccessControlType.Allow));
                    File.SetAccessControl(MainWindow.XMLSettingFile, fSecurity);
                  //  bXMLSettingsRead = GetSettingsFromXMLFile();
                }
                catch (Exception ae)
                {
                    MessageBox.Show("CreateXMLSettingsFile -->> " + ae.Message.ToString());
                }
            }
           // return bXMLSettingsRead;
        }



    }
}
