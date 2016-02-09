/*
 * 
 * 
 */
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
using System.Net.NetworkInformation;
using WPFBuhlerControls;
using Microsoft.Win32;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Collections;
using S7Link;
using System.Windows.Threading;
using System.Threading;
using System.ComponentModel;
using KNEKT.Classes;
using System.IO;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
using Snap7;


namespace KNEKT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //..............................................................................//
        //                                  Variables                                   //
        //..............................................................................// 

        #region Class Variables [Do Not Change]

        S7Client PLC;
        //
        //Event Handlers 
        //
        public static event EventHandler sElementDescription_Changed;
        public static event EventHandler sElementStatus_Changed;
        public static event EventHandler sActiveLine_Changed;
        public static event EventHandler bFault_Changed;
        public static event EventHandler iActiveLine_Changed;
        public static event EventHandler bLoggedIn_Changed;
        public static event EventHandler bCmdModify_Changed;


        //Class Variables used on window (Template) for all pages(in Frame)
        public static string stat_ElementDescription = "";                                      //Show selected elements description
        public static string stat_ElementState = "";                                            //Show selected elements state
        public static string sActiveControlName;                                                //The active control that has been selected/clicked
        public static string stat_ActiveLineName;                                               //The line name of the currently selected page
        public static int stat_iActiveLineNumber = 0;                                           //The line number of the currently selected page
        public static bool stat_bFault;                                                         //True if active element is in fault
        public static string stat_sFault;                                                       //Element Description that is in fault
        public static string stat_sPlantName;                                                   //PLant Name
        public static string stat_sPlantLocation;                                               //Location of plant
        public static bool stat_bShowApplicationHints = false;                                  //Display application hints 
        public static string sCurrentUsername = "";
        public static string sCurrentPassword = "";

        //
        //User Management
        //
        public static int stat_iUserLevel = 0;                                                  //Integer Value for current users access level
        public static string stat_sLoggedInUser;                                                //User First and last name
        public static bool stat_bLoggedIn;                                                      //Logged in True/False
        public static bool stat_bCmdModify = false;                                             //When true, CmdModify is fired on current line
        public static int stat_iLogOffTime = 0;                                                 //Automatically Log the user off after this time [0=Deactivated]
        public static int stat_iLogOffCounter = 0;                                              //Counter that starts counting when the UI is inactive  
        public static bool stat_bMultitouchS1;                                                  //Screen 1 supports multitouch?
        public static string stat_HelpVideoFolder;                                              //The folder where the help page should look for training videos
        public static string stat_ReportViewerAddress = "";                                     //The web address to navigate to for the report viewer
        public static string stat_HelpManualFolder;                                             //Folder that contains all Machine Manuals
        public static string stat_OSKPath;                                                      //Path to the On Screen Keyboard
        public static bool bThreadsToRun = false;                                               //True if all threads should continue running


        public static string sqlServername = "";
        public static string sqlDatabase = "";
        public static string sqlUsername = "";
        public static string sqlPassword = "";
        public static string SqlConnectionString;

        public static string sActiveControl;
        public bool bRunSQLCleanup = false;                                                     //Prevents the data logger from writing to HistoricLog
        DateTime dtHistoricLogCleanupTime = DateTime.Parse("01:00");                            //The time when historiclog needs to be cleaned up of old records (greater than 3 1M)

        bool[] bSectionSTAFeedPriority = new bool[200];

        //
        //Collections
        //
        private ArrayList alLineTypes = new ArrayList();                                        //1 = Normal Line (1 Section), 2 = Mill Line (3 sections)
        public static ArrayList alLoggerToSQL = new ArrayList();                                //Contents to write to SQL
        public static ArrayList alLoggerToUI = new ArrayList();                                 //Contents to write to the Alarm/Event Viewer
        private ArrayList alControlBoxSet = new ArrayList();                                    //Contains DB addresses for each line to use control buttons
        private ArrayList alBadTags = new ArrayList();                                          //Contains all bad read tags
        private ArrayList alReportingLog = new ArrayList();                                     //Contains all items that should be written to the database for historical storage
        private Hashtable htRecTickTagValues = new Hashtable();                                 //Contains all records marked RecOnTick = 1 along with their current values
        ArrayList alHints = new ArrayList();

        public static int i2048PercentMoisture = 0;                                     //The percentage of water to add to the second dampening screw
        public static int i2041OutFlowrate = 0;                                         //The outflowrate for CON2s flowbalancer
        public static int i2042OutFlowrate = 0;                                         //The outflowrate for CON2s flowbalancer
        public static double d2027ActualMoisture = 0;                                   //Actual Moisture in Bin 201

        //
        //General
        //

        public static string KNEKTDirectory = "";
        public static string DllDirectory = "";
        public static string dllName = "DBCredential.dll";
        public static string sVNCDirectory = "";

        public static string PLCIpAddress = "10.0.0.210";                                                 //IP address of the PLC
        public static string PLCRackNo = "0";                                                    //Rack Nunmber of the PLC
        public static string PLCSlotNum = "2";                                                   //Slot Number of the PLC

        bool bFirstTagRead = true;                                                              //True on start up of application, after first tag read, false
        bool bFirstAdditionalTagRead = true;                                                    //True on start up of application, after first tag read, false
        public static bool bPLCCommsGood = false;                                               //Are PLC comms good? If Not dont attempt to read tags
        public static bool bShowTagnames = true;                                                //Display tagnames next to elements        
        public bool bAlarmHornFlash = false;

        public static int iTransferBinNumber = 0;                                               //This is used to track which bin is being used for transfer

        //User access
        bool bOperatorAccess = false;
        bool bAdministratorAccess = false;

        DispatcherTimer timerWriteLog = new DispatcherTimer();                                  //Writes data to SQL every x seconds
        DispatcherTimer timerAlarmFlash = new DispatcherTimer();                                //Flashes when there is an active alarm
        DispatcherTimer timerNoCommFlash = new DispatcherTimer();                               //Flashes when there are no comms to the PLC
        DispatcherTimer timerAutoLogOff = new DispatcherTimer();                                //Automatically Logs user off
        DispatcherTimer timerApplicationHints = new DispatcherTimer();                          //Loads a new hint every x minutes
        DispatcherTimer timerTrends = new DispatcherTimer();                                    //Updates the trend page
        DispatcherTimer timerAlarmHorn = new DispatcherTimer();                                 //Updates the Alarm Horn Element
        DispatcherTimer timerWriteReporting = new DispatcherTimer();                            //Logs the current value of all tags that need to be reported on   
        DispatcherTimer timerCleanHistoricLog = new DispatcherTimer();                          //Runs the query to clean historiclog every night

        //
        //Threads
        //

        Thread threadTagRead;                                                                   //Thread to read tags from PLC every x milliseconds
        Thread threadPLCComms;                                                                  //Thread to check comms to PLC every x seconds
        Thread threadWriteToSQL;

        StandardCode standardCode;

        #endregion


        #region Software Toolbox S7 Link
        //
        //S7LINK Variables
        //
        Controller PLC1_R;                                                                      //Read instance        
        public static Controller PLC1_W;                                                                      //Write instance
        public TagGroup tagroupSmartTags;                                                       //All SQL SmartTags
        public TagGroup tagroupAdditionalSmartTags;                                             //All Additional Element SmartTags
        TagGroup tagroupSecStates;                                                              //
        TagGroup tagroupSecStatesFAULT;                                                         //
        TagGroup tagroupSecParEmptying;                                                         //
        TagGroup tagroupSecOutEmptying;                                                         //
        TagGroup tagroupSecStaFeedOff;
        //
        //Control Button Tags
        //
        Tag tControl_CmdFeedOn = new Tag();
        Tag tControl_CmdStart = new Tag();
        Tag tControl_CmdTransferOn = new Tag();
        Tag tControl_CmdRequestExecute = new Tag();
        Tag tControl_CmdRequestModify = new Tag();
        Tag tControl_CmdFeedOff = new Tag();
        Tag tControl_CmdHornOff = new Tag();
        Tag tControl_CmdFaultReset = new Tag();
        Tag tControl_CmdSeqStop = new Tag();
        Tag tControl_CmdEStop = new Tag();
        Tag tControl_CmdReset = new Tag();
        Tag tControl_CmdRequestDefine = new Tag();

        #endregion

        //
        //Display Pages Instances
        //
        DisplayPages.INT1 pageINT1;
        DisplayPages.FCL1 pageFCL1;
        DisplayPages.MTR1 pageMTR1;
        DisplayPages.MIL1A pageMIL1;
        DisplayPages.MIL1B pageMIL2;
     



        //public static string sActiveControl;                                                    //
        public static string stat_sActiveObjectNo;                                              // Used for manual start / stop commands
        public static bool isValve; 
      

        DisplayPages.Settings pageSettings;
        DisplayPages.ReportViewer pageReportViewer;
        DisplayPages.ProfibusNetwork pageProfibusNetwork;                                                                               //
        //Senders and Receivers
        //
        // *********** INT1 Senders and Receivers ************* //
        SortedList slJobList_INT1SenderDBs = new SortedList();
        SortedList slJobList_INT1SenderBins = new SortedList();
        SortedList slJobList_INT1ReceiverDBs = new SortedList();
        SortedList slJobList_INT1ReceiverBins = new SortedList();
       
        // *********** FCL1 Senders and Receivers ************* //
        SortedList slJobList_FCL1SenderDBs = new SortedList();
        SortedList slJobList_FCL1SenderBins = new SortedList();
        SortedList slJobList_FCL1ReceiverDBs = new SortedList();
        SortedList slJobList_FCL1ReceiverBins = new SortedList();

        // *********** TRF1 Senders and Receivers ************* //
        SortedList slJobList_MTR1SenderDBs = new SortedList();
        SortedList slJobList_MTR1SenderBins = new SortedList();
        SortedList slJobList_MTR1ReceiverDBs = new SortedList();
        SortedList slJobList_MTR1ReceiverBins = new SortedList();

        // *********** SCL1 Senders and Receivers ************* //
        SortedList slJobList_MIL1SenderDBs = new SortedList();
        SortedList slJobList_MIL1SenderBins = new SortedList();
        SortedList slJobList_MIL1ReceiverDBs = new SortedList();
        SortedList slJobList_MIL1ReceiverBins = new SortedList();

        // *********** FPH1 Senders and Receivers ************* //
        SortedList slJobList_MIL2SenderDBs = new SortedList();
        SortedList slJobList_MIL2SenderBins = new SortedList();
        SortedList slJobList_MIL2ReceiverDBs = new SortedList();
        SortedList slJobList_MIL2ReceiverBins = new SortedList();

        ////**************SCG1 Screenings and Grnding *************//
        //SortedList slJoblist_SCG1SenderDBs = new SortedList();
        //SortedList slJoblist_SCG1SenderBins = new SortedList();
        //SortedList slJoblist_SCG1ReceiverDBs = new SortedList();
        //SortedList slJoblist_SCG1ReceiverBins = new SortedList();


        ////**************BRN1 BRAN HANDLING *************//
        //SortedList slJoblist_BRN1SenderDBs = new SortedList();
        //SortedList slJoblist_BRN1SenderBins = new SortedList();
        //SortedList slJoblist_BRN1ReceiverDBs = new SortedList();
        //SortedList slJoblist_BRN1ReceiverBins = new SortedList();

        ////<----
        //// *********** MIL1 Senders and Receivers ************* //
        //SortedList slJoblist_MIL1SND35Bins = new SortedList();
        //SortedList slJoblist_MIL1SND35DBs = new SortedList();
        //SortedList slJobList_MIL1RCV36DBs = new SortedList();
        //SortedList slJobList_MIL1RCV36Bins = new SortedList();
        //SortedList slJobList_MIL1RCV37Bins = new SortedList();
        //SortedList slJobList_MIL1RCV37DBs = new SortedList();
        //SortedList slJobList_MIL1RCV40Bins = new SortedList();
        //SortedList slJobList_MIL1RCV40DBs = new SortedList();

      
       

        /*******************************************************************************/
        /*                  TAG VALUE VARIABLES USED FOR GLOBAL ACCESS                 */
        /*******************************************************************************/
        //Yeild Calculator
        //public double d4004B1OutFlowrate = 0;
        //public double d4425CakeFlourOutFlowrate = 0;
        //public double d4409WhiteBreadFlourOutFlowrate = 0;
        //public double d4415LowgradeFlourOutFlowrate = 0;
        //public double d4437BranOutFlowrate = 0;
        //public double d5102MixbackFlourOutFlowrate = 0;
        //public bool bBin404ContainsWBF = false;
        //public bool bBin404ContainsCF = false;
        //public bool bBin405ContainsWBF = false;
        //public bool bBin405ContainsCF = false;
        //public bool bBin406ContainsWBF = false;
        //public bool bBin406ContainsCF = false;
        //public bool bBin407ContainsWBF = false;
        //public bool bBin407ContainsCF = false;
        //public bool bProductionReady = false;
        //public bool bFCL1StatusActive = false;
        public double _d1011_Flour1Flowrate;
        public double _d1012_Flour1Flowrate;
        public double _d2007_Flour1Flowrate;
        public double _d4002B1OutFlowrate;


        string sRecipeFailSafeFile = KNEKTDirectory + @"\RecipeFailSafe.csv";

        public static string XMLSettingFile = KNEKTDirectory + @"Binaries\AdminSettings.xml";
        public static int PanelID = 0;
        public KNEKT.Classes.Products.ProductsViewModel ProductsViewModelDataContext;
        ObservableCollection<Classes.Error> errorList = new ObservableCollection<Classes.Error>();
        Hashtable htMIX1SectionStates = new Hashtable();




        //------------------------------------------------------------------------------//
        //                                  Constructor                                 //
        //------------------------------------------------------------------------------//        


        public MainWindow()
        {
            InitializeComponent();

            DisplayPages.DisplayWindows.SplashScreenWindow.CurrentLoadingStatus("Initializing Objects...", 10);
            //Thread.Sleep(500);

            standardCode = new StandardCode();

            DisplayPages.DisplayWindows.SplashScreenWindow.CurrentLoadingStatus("Checking Directories...", 20);
            //Thread.Sleep(500);

            bool bExists = standardCode.CreateDirectoryStructure();

            if (bExists)
            {
                dllName = DllDirectory + dllName;

                DisplayPages.DisplayWindows.SplashScreenWindow.CurrentLoadingStatus("Loading Credentials...", 30);
                Thread.Sleep(500);
                standardCode.GetDatabaseCredentials();

                SqlConnectionString = "Data Source=" + sqlServername + ";Initial Catalog=" + sqlDatabase + ";User Id=" + sqlUsername + ";Password=" + sqlPassword + "; Connection Timeout=10;";

                DisplayPages.DisplayWindows.SplashScreenWindow.CurrentLoadingStatus("Testing Connection...", 40);
                //Thread.Sleep(500);
                bool validConnection = standardCode.TestSQLDatabaseConnection(SqlConnectionString);

                if (validConnection)
                {
                    DisplayPages.DisplayWindows.SplashScreenWindow.CurrentLoadingStatus("Validating Settings...", 50);
                    //Thread.Sleep(500);
                    bool bSettingsLoaded = standardCode.LoadApplicationSettings(SqlConnectionString);
                    //standardCode.GetSettingsFromXMLFile();




                    DisplayPages.DisplayWindows.SplashScreenWindow.CurrentLoadingStatus("Connecting To PLC...", 60);
                    //Thread.Sleep(500);

                    bool validFormat = standardCode.ValidatePLCConfiguration(PLCIpAddress, PLCRackNo, PLCSlotNum);  //Validates the loaded settings
           
                    if (validFormat)
                    {
                        bool bContinue = false;
                        string sRackSlot = PLCRackNo.Trim() + "." + PLCSlotNum.Trim();

                        try
                        {
                            PLC = new S7Client();
                            PLC.ConnectTo(PLCIpAddress, 0, 2);
                            //PLC1_R = new Controller(PLCIpAddress, Controller.CPU.S7300, sRackSlot);         //Set PLC Rack and Slot number
                            //PLC1_W = new Controller(PLCIpAddress, Controller.CPU.S7300, sRackSlot);         //Set PLC Rack and Slot number
                            bContinue = true;
                        }
                        catch (Exception ex)
                        {
                            bContinue = false;
                            MessageBox.Show("Error creating PLC connection object!\n" + ex.Message, "Check PLC Config Settings", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                        DisplayPages.DisplayWindows.SplashScreenWindow.CurrentLoadingStatus("PLC Objects Created...", 80);
                        //Thread.Sleep(500);
                        if (bContinue)
                        {
                            timerWriteLog.Tick += new EventHandler(timerWriteLog_Tick);
                            timerWriteLog.Interval = new TimeSpan(0, 0, 0, 10, 0);
                            timerAlarmFlash.Tick += new EventHandler(timerAlarmFlash_Tick);
                            timerAlarmFlash.Interval = new TimeSpan(0, 0, 0, 1, 0);
                            timerNoCommFlash.Tick += new EventHandler(timerNoCommFlash_Tick);
                            timerNoCommFlash.Interval = new TimeSpan(0, 0, 0, 2, 0);
                            timerAutoLogOff.Tick += new EventHandler(timerAutoLogOff_Tick);
                            timerAutoLogOff.Interval = new TimeSpan(0, 0, 1, 0, 0);
                            timerApplicationHints.Tick += new EventHandler(timerApplicationHints_Tick);
                            timerApplicationHints.Interval = new TimeSpan(0, 0, 10, 0, 0);
                            timerAlarmHorn.Tick += new EventHandler(timerAlarmHorn_Tick);
                            timerAlarmHorn.Interval = new TimeSpan(0, 0, 0, 1, 0);
                            timerWriteReporting.Tick += new EventHandler(timerWriteReporting_Tick);
                            timerWriteReporting.Interval = new TimeSpan(0, 0, 1, 0, 0);
                            timerCleanHistoricLog.Tick += new EventHandler(timerCleanHistoricLog_Tick);
                            timerCleanHistoricLog.Interval = new TimeSpan(0, 1, 0, 0, 0);


                            sElementDescription_Changed += new EventHandler(MainWindow_sElementDescription_Changed);
                            sElementStatus_Changed += new EventHandler(MainWindow_sElementStatus_Changed);
                            sActiveLine_Changed += new EventHandler(MainWindow_sActiveLine_Changed);
                            bFault_Changed += new EventHandler(MainWindow_bFault_Changed);
                            iActiveLine_Changed += new EventHandler(MainWindow_iActiveLine_Changed);
                            bLoggedIn_Changed += new EventHandler(MainWindow_bLoggedIn_Changed);
                            bCmdModify_Changed += new EventHandler(MainWindow_bCmdModify_Changed);

                            if (MainWindow.stat_bShowApplicationHints == true)
                            {
                                alHints = standardCode.LoadApplicationHints(SqlConnectionString);
                                timerApplicationHints.IsEnabled = true;
                                timerApplicationHints.Start();
                            }

                            #region S7 Link Initialization


                            //tagroupSecStaFeedOff = new TagGroup(PLC1_R);
                            //tagroupSmartTags = new TagGroup(PLC1_R);
                            //tagroupAdditionalSmartTags = new TagGroup(PLC1_R);
                            //tagroupSecStates = new TagGroup();
                            //tagroupSecStatesFAULT = new TagGroup();
                            //tagroupSecParEmptying = new TagGroup(PLC1_R);
                            //tagroupSecOutEmptying = new TagGroup(PLC1_R);


                            #endregion

                            DisplayPages.DisplayWindows.SplashScreenWindow.CurrentLoadingStatus("Finalizing Parameters...", 90);
                            //Thread.Sleep(500);
                            //------------------------------------------------------------------------------//
                            //                             -->* CHANGE *<--                                 //
                            //------------------------------------------------------------------------------//
                            stat_sPlantName = "African Milling - Maize Mill 168 t/h";
                            stat_sPlantLocation = "Lusaka, Zambia";

                            //MainWindow mw;

                            //ProductsViewModelDataContext = new KNEKT.Classes.Products.ProductsViewModel();

                            //Create instance of each display page to navigate to using buttons
                            //pageINT1 = new DisplayPages.INT1(PLC);
                            pageFCL1 = new DisplayPages.FCL1(PLC);
                            //pageMTR1 = new DisplayPages.MTR1(PLC1_W);
                            //pageMIL1 = new DisplayPages.MIL1A(PLC1_W);
                            //pageMIL2 = new DisplayPages.MIL1B(PLC1_W);
                           
                            //binProductManagement = new DisplayPages.DisplayWindows.BinProductManagement() { DataContext = ProductsViewModelDataContext };

                            pageSettings = new DisplayPages.Settings();
                            pageReportViewer = new DisplayPages.ReportViewer();
                            pageProfibusNetwork = new DisplayPages.ProfibusNetwork();

                            //Assign the PLC_W Write instance to each manual control and scale info on every display page
                            //pageINT1._Setpoint_A1001.SpeedControl_WriteController = PLC1_W;
                            //pageINT1.manualControl_A1059.ManualControl_WriteController = PLC1_W;
                            //pageINT1.manualControl_A1060.ManualControl_WriteController = PLC1_W;
                            //pageINT1.manualControl_A1061.ManualControl_WriteController = PLC1_W;
                            //pageINT1.manualControl_A1062.ManualControl_WriteController = PLC1_W;
                            //pageTRF1.manualControl_A1059.ManualControl_WriteController = PLC1_W;
                            //pageTRF1.manualControl_A1060.ManualControl_WriteController = PLC1_W;
                            //pageTRF1.manualControl_A1061.ManualControl_WriteController = PLC1_W;
                            //pageTRF1.manualControl_A1062.ManualControl_WriteController = PLC1_W;
                            ////pageFCL1.FlowbalancerInfo_A2026.FlowbalancerInfo3_WriteController = PLC1_W;
                            ////pageFCL1.FlowbalancerInfo_A2027.FlowbalancerInfo3_WriteController = PLC1_W;
                            ////pageFCL1.FlowbalancerInfo_A2028.FlowbalancerInfo3_WriteController = PLC1_W;
                            //pageFCL1.MYFCInfo_A2039.MYFCInfo_WriteController = PLC1_W;
                            //pageSCL1.FlowbalancerInfo_A2053.FlowbalancerInfo1_WriteController = PLC1_W;
                            //pageSCL1.FlowbalancerInfo_A2054.FlowbalancerInfo1_WriteController = PLC1_W;
                            //pageSCL1.FlowbalancerInfo_A2055.FlowbalancerInfo1_WriteController = PLC1_W;
                            //pageSCL1.MYFCInfo_A2039.MYFCInfo_WriteController = PLC1_W;
                            //pageMIL1.ScaleInfo_A0150.ScaleInfo_Controller_W = PLC1_W;
                            //pageMIL1.manualControl_A0150.ManualControl_WriteController = PLC1_W;
                            //pageMTR1.manualControl_A0150.ManualControl_WriteController = PLC1_W;
                            //pageSCG1._Setpoint_A3010.SpeedControl_WriteController = PLC1_W;

                            //-->
                            
                            
                            //MillerCallButton.MillerCallButton_WriteController = PLC1_W;
                            
                 
                            
                           
                            //pageMIL1A._4073SC.SpeedControl_WriteController = PLC1_W;
                            //pageTRF1.ScaleInfo_1019.ScaleInfo_Controller_W = PLC1_W;
                           


                            //Set the type of line for each line in the relevant order (1 = normal line, 2 = Milling line)
                            //alLineTypes.Add(1); //INT1
                            alLineTypes.Add(1); //FCL1
                            //alLineTypes.Add(1); //MTR1
                            //alLineTypes.Add(2); //MIL1                            
                            //alLineTypes.Add(2); //MIL2
                            //alLineTypes.Add(2); //MIL1B
                            
                            DisplayPages.DisplayWindows.SplashScreenWindow.CurrentLoadingStatus("Application Starting...", 100);
                            //Thread.Sleep(500);
                            DisplayPages.DisplayWindows.SplashScreenWindow.EndDisplay();

                            _mainFrame.Navigate(new DisplayPages.StartPage());                          //--> *Navigate to the start page

                            LoadVisualApplicationSettings();                                           //Loads the visual application settings                            
                            StartCommunicationThread();                                                 //Start the commincation threads
                            //InitSendersAndRecievers();                                                  //Initialize all senders and recievers
                            bLoggedIn = false;                                                          //Set LoggedIn status to False     


                            #region Visibilities (Labels buttons and timers)
                            btnJob.Visibility = Visibility.Hidden;
                            #endregion
                        }
                    }
                    else //End PLC configuration settings
                    {
                        DisplayPages.DisplayWindows.SplashScreenWindow.EndDisplay();
                        MessageBox.Show("Some of the PLC settings are not correct. The program will not start until these settings are corrected", "Format of Settings Incorrect", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else //End connect to Database
                {
                    stat_bLoggedIn = false;
                    stat_iActiveLineNumber = 0;
                    SetControlVisibilityOnLineChange();

                    _mainFrame.Navigate(new _PagesSettings.SQLSettings(this));
                    HideLineButtons();
                    HideControlsOnException();
                    DisplayPages.DisplayWindows.SplashScreenWindow.EndDisplay();
                    MessageBox.Show("Please update the database connection details on the settings page to continue", "Invalid Database credentials", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else //End Create Directories
            {
                DisplayPages.DisplayWindows.SplashScreenWindow.EndDisplay();
                MessageBox.Show("The required directory structure has not been created.\nPlease run the program as an Administrator to create the directories.\nThe program will not start until these directories are created", "Directories do not Exist", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }


        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        #region Properties [Standard]


        public static string sElementDescription
        {
            get
            {
                return MainWindow.stat_ElementDescription;
            }
            set
            {
                MainWindow.stat_ElementDescription = value;
                if (MainWindow.sElementDescription_Changed != null)
                {
                    sElementDescription_Changed(MainWindow.stat_ElementDescription, new EventArgs());
                }
            }
        }

        public static string sElementState
        {
            get
            {
                return MainWindow.stat_ElementState;
            }
            set
            {
                MainWindow.stat_ElementState = value;
                if (MainWindow.sElementStatus_Changed != null)
                {
                    sElementStatus_Changed(MainWindow.stat_ElementState, new EventArgs());
                }
            }
        }

        public static string sActiveLineName
        {
            get
            {
                return MainWindow.stat_ActiveLineName;
            }
            set
            {
                MainWindow.stat_ActiveLineName = value;
                if (MainWindow.sActiveLine_Changed != null)
                {
                    sActiveLine_Changed(MainWindow.stat_ActiveLineName, new EventArgs());
                }
            }
        }

        public static bool bFault
        {
            get
            {
                return MainWindow.stat_bFault;
            }
            set
            {
                MainWindow.stat_bFault = value;
                if (MainWindow.bFault_Changed != null)
                {
                    bFault_Changed(MainWindow.stat_bFault, new EventArgs());
                }
            }
        }

        public static int iActiveLineNumber
        {
            get
            {
                return MainWindow.stat_iActiveLineNumber;
            }
            set
            {
                MainWindow.stat_iActiveLineNumber = value;
                if (MainWindow.iActiveLine_Changed != null)
                {
                    iActiveLine_Changed(MainWindow.stat_iActiveLineNumber, new EventArgs());
                }
            }
        }

        public static bool bLoggedIn
        {
            get
            {
                return MainWindow.stat_bLoggedIn;
            }
            set
            {
                MainWindow.stat_bLoggedIn = value;
                if (MainWindow.bLoggedIn_Changed != null)
                {
                    bLoggedIn_Changed(MainWindow.stat_bLoggedIn, new EventArgs());
                }
            }
        }

        public static bool bCmdModify
        {
            get
            {
                return MainWindow.stat_bCmdModify;
            }
            set
            {
                MainWindow.stat_bCmdModify = value;
                if (MainWindow.bCmdModify_Changed != null)
                {
                    bCmdModify_Changed(MainWindow.stat_bCmdModify, new EventArgs());
                }
            }
        }

        #endregion

        //------------------------------------------------------------------------------//
        //                                  Threads                                     //
        //------------------------------------------------------------------------------//

        #region Threads [Standard]

        /// <summary>
        /// Creates all SmartTags and Reads them on their own thread to prevent the UI from freezing
        /// </summary>
        public void InitializeTagUpdateThread()
        {
            //
            //  INITIALIZE THE TAGS
            //
            LoadTagsOnlineFromSQL();
            LoadControlButtonsOnline();
            LoadLineAndSectionStatesOnline();
            LoadAdditionalTagsOnlineFromSQL();
            alLoggerToUI = standardCode.GetUILogItems(SqlConnectionString);

        ThreadLoop:

            if (Thread.CurrentThread.IsAlive && bThreadsToRun)
            {
                try
                {
                    if (bPLCCommsGood)
                    {
                        //
                        //  READ THE TAGS
                        //
                        if (!PLC1_R.IsConnected)
                        {
                            PLC1_R.Connect();
                        }
                       
                        PLC1_R.GroupRead(tagroupSmartTags);
                        PLC1_R.GroupRead(tagroupSecStates);
                        PLC1_R.GroupRead(tagroupSecStatesFAULT);
                        PLC1_R.GroupRead(tagroupSecStaFeedOff);
                        PLC1_R.GroupRead(tagroupSecOutEmptying);
                        PLC1_R.GroupRead(tagroupSecParEmptying);
                        PLC1_R.GroupRead(tagroupAdditionalSmartTags);

                        lblLastReadTime.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            lblLastReadTime.Content = "PLCR >> " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "." + DateTime.Now.Millisecond;
                        }));

                        Thread.Sleep(500);

                        if (bFirstTagRead == true)
                        {
                            bFirstTagRead = false;
                        }

                        if (bFirstAdditionalTagRead == true)
                        {
                            bFirstAdditionalTagRead = false;
                        }

                        goto ThreadLoop;
                    }
                    else
                    {
                        if (PLC1_R.IsConnected)
                        {
                            PLC1_R.Disconnect();
                        }

                        Thread.Sleep(5000);
                        goto ThreadLoop;
                    }
                }
                catch (Exception exc)
                {
                    if (exc.Message.ToLower() != "thread was being aborted.")
                    {
                        MessageBox.Show("TagUpdate 1 >> " + exc.Message);
                    }

                }
            }
            else
            {
                threadTagRead.Join(1000);
            }
        }


        /// <summary>
        /// Pings the PLC every 1 second to check if the communication link is still active
        /// </summary>
        public void CheckPLCComms()
        {
            Brush brColor = Brushes.Black;
            string sPLCCommText = "";


        ThreadLoop:

            if (Thread.CurrentThread.IsAlive && bThreadsToRun)
            {
                try
                {
                    if (NetworkInterface.GetIsNetworkAvailable())
                    {
                        Ping pingPLC = new Ping();
                        PingReply pingReply = pingPLC.Send(PLCIpAddress);

                        if (pingReply.Status == IPStatus.Success)                               //Ping reply from PLC
                        {
                            brColor = Brushes.Green;
                            sPLCCommText = "GOOD";
                            MainWindow.bPLCCommsGood = true;

                            if (!PLC1_R.IsConnected)
                            {
                                PLC1_R.Connect();
                            }
                        }
                        else                                                                    //Active interface on local machine, no ping reply from plc
                        {
                            brColor = Brushes.Red;
                            sPLCCommText = "BAD PLC";
                            MainWindow.bPLCCommsGood = false;
                        }
                    }
                    else                                                                        //No active interface on local machine
                    {
                        brColor = Brushes.Orange;
                        sPLCCommText = "HMI INT FAULT";
                        MainWindow.bPLCCommsGood = false;
                    }


                    //
                    //  GET DISPATCHER ON UI THREAD
                    //                                                    
                    txtPLCComm.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        txtPLCComm.Background = brColor;
                        txtPLCComm.Text = sPLCCommText;
                    }));

                    txtDateTime.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        txtDateTime.Text = DateTime.Now.ToString();
                    }));


                    if (bPLCCommsGood)
                    {
                        txtNoComm.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            txtNoComm.Opacity = 0;
                        }));

                        timerNoCommFlash.Stop();

                        timerWriteReporting.Start();

                    }
                    else
                    {
                        if (!timerNoCommFlash.IsEnabled)
                            timerNoCommFlash.Start();
                        timerWriteReporting.Stop();
                    }

                    Thread.Sleep(1000);
                    goto ThreadLoop;
                }
                catch (Exception ex)
                {
                    MainWindow.bPLCCommsGood = false;

                    if (ex.Message.ToLower() != "thread was being aborted.")
                    {
                        if (ex.Message.ToLower() != "an exception occurred during a ping request.")
                        {
                            MessageBox.Show("CheckPLCComms 1 >> " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                threadPLCComms.Join(1000);
            }
        }


        #endregion


        //------------------------------------------------------------------------------//
        //                                  Timers                                      //
        //------------------------------------------------------------------------------//

        #region Timers [Standard]

        private void timerWriteLog_Tick(object sender, EventArgs e)
        {
            try
            {
                ArrayList alLoggerToSQLLOCAL = new ArrayList();
                alLoggerToSQLLOCAL.AddRange(alLoggerToSQL);
                alLoggerToSQL.Clear();

                for (int i = 0; i < alLoggerToSQLLOCAL.Count; i++)
                {
                    LogItem li = (LogItem)alLoggerToSQLLOCAL[i];
                    InsertLogValue(li.ts_DateTime, li.OADate, li.ObjectName, li.ObjectAction, li.Code);

                }
                alLoggerToSQLLOCAL.Clear();
                standardCode.CleanUpLogAL(); //Clean up UI Log
            }
            catch (Exception ex)
            {
                MessageBox.Show("Insert To AppLog --> " + ex.Message, "Error Adding Log Item", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            standardCode.CleanUpLogAL(); //Clean up UI Log

        }

        private void timerAlarmFlash_Tick(object sender, EventArgs e)
        {
            Brush b = txtAlarms.Foreground;

            if (b == Brushes.Black)
            {
                txtAlarms.Foreground = Brushes.White;
            }
            else
            {
                txtAlarms.Foreground = Brushes.Black;
            }
        }

        private void timerAlarmHorn_Tick(object sender, EventArgs e)
        {
            bAlarmHornFlash = !bAlarmHornFlash;

            if (bAlarmHornFlash)
            {
                AlarmHorn1.AlarmHornColor = 1;
            }
            else
            {
                AlarmHorn1.AlarmHornColor = 0;
            }
        }

        private void timerNoCommFlash_Tick(object sender, EventArgs e)
        {
            txtNoComm.Visibility = Visibility.Visible;
            if (txtNoComm.Opacity >= 1.0)
            {
                DoubleAnimation da = new DoubleAnimation(1.0, 0.0, new Duration(new TimeSpan(0, 0, 0, 1)));
                txtNoComm.BeginAnimation(OpacityProperty, da);
            }
            else
            {
                DoubleAnimation da = new DoubleAnimation(0.0, 1.0, new Duration(new TimeSpan(0, 0, 0, 1)));
                txtNoComm.BeginAnimation(OpacityProperty, da);
            }
        }

        private void timerAutoLogOff_Tick(object sender, EventArgs e)
        {
            stat_iLogOffCounter += 1;

            if (stat_iLogOffCounter >= stat_iLogOffTime)
            {
                //Log the active user off
                if (stat_iUserLevel > 0)
                {
                    UIInteraction_Change(btnLogOff, "AutoLogOff (" + stat_sLoggedInUser + ")");
                    MainWindow.stat_sLoggedInUser = "";
                    MainWindow.stat_iUserLevel = 0;
                    MainWindow.bLoggedIn = false;
                    MainWindow.stat_iActiveLineNumber = -1;
                    PlantSwitches.Visibility = Visibility.Hidden;
                    MaintenanceSwitches.Visibility = Visibility.Hidden;
                    gbTipBox.Visibility = Visibility.Hidden;

                    _mainFrame.Navigate(new DisplayPages.StartPage());
                }
            }
        }

        private void timerApplicationHints_Tick(object sender, EventArgs e)
        {
            try
            {
                if (stat_iActiveLineNumber > 0 && alHints.Count > 0)
                {
                    Random r = new Random();
                    int iRandom = r.Next(0, alHints.Count - 1);
                    string sHint = alHints[iRandom].ToString();
                    gbTipBox.Visibility = System.Windows.Visibility.Visible;
                    txtBlockTipBox.Text = sHint;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hint --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Writes the current value of the selected tags to DB on tick
        /// </summary>
        private void timerWriteReporting_Tick(object sender, EventArgs e)
        {
            if (!bRunSQLCleanup)
            {
                threadWriteToSQL = new Thread(new ThreadStart(WriteReportingLogToSQL));
                threadWriteToSQL.Start();
            }

        }

        private void timerCleanHistoricLog_Tick(object sender, EventArgs e)
        {
            if (DateTime.Parse(DateTime.Now.ToString("HH:mm")) <= dtHistoricLogCleanupTime.AddMinutes(31) || DateTime.Parse(DateTime.Now.ToString("HH:mm")) >= dtHistoricLogCleanupTime.AddMinutes(-31))
            {
                try
                {
                    bRunSQLCleanup = true;
                    using (SqlConnection sqlconn = new SqlConnection(SqlConnectionString))
                    {
                        sqlconn.Open();
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.CommandText = "EXECUTE CLEAR_HISTORIC_LOG";
                        int result = sqlcmd.ExecuteNonQuery();
                    }
                }
                catch (Exception)
                {
                    bRunSQLCleanup = false;
                }
                finally
                {
                    bRunSQLCleanup = false;
                }
            }
            else
            {
                bRunSQLCleanup = false;
            }
        }

        #endregion


        //------------------------------------------------------------------------------//
        //                              Database Methods                                //
        //------------------------------------------------------------------------------//

        #region Database Methods [Standard]


        /// <summary>
        /// Loads all Tags from the SmartTag Table in SQL into Memory by creating a S7LINK.Tag for each SmartTag entry
        /// </summary>
        public void LoadTagsOnlineFromSQL()
        {
            try
            {
                //
                //  Select all tags from SQL
                //
                using (SqlConnection sqlConn = new SqlConnection(SqlConnectionString))
                {
                    sqlConn.Open();
                    SqlCommand cmd = sqlConn.CreateCommand();
                    cmd.CommandText = "SELECT ObjectNo, st.Tagname, TagDescription, DBOffset, ParMsgType, FB, RecOnTick, RecOnChange, RecTrend, ISNULL(Property,'-') Property, GroupNo FROM SmartTags st LEFT OUTER JOIN TagLinks tl on st.Tagname = tl.TagName WHERE st.GcProTag = 1 ORDER BY st.TagName";
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        MyObjectInfo moi = new MyObjectInfo(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetInt32(6), reader.GetInt32(7), reader.GetInt32(8), reader.GetString(9), reader.GetString(10));

                        Tag t = new Tag();
                        t.Name = reader.GetString(3);
                        t.DataType = S7Link.Tag.ATOMIC.WORD;
                        t.MyObject = moi;
                        t.Controller = PLC1_R;
                        t.Changed += new EventHandler(INGEARS7_Tag_Changed);                             //Adds event handler to each tag for "Tag Value Changed Event"
                        tagroupSmartTags.AddTag(t);

                        if (moi.RecOnTick == 1)
                        {
                            htRecTickTagValues.Add(moi.TagName, 0);
                        }
                    }
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load SmartTags --> " + ex.Message, "Error Loading Tags", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Inserts a Log item into the application log table
        /// </summary>
        /// <param name="Date">Datetime of the event</param>
        /// <param name="OADate">OLE Automation date</param>
        /// <param name="ObjectName">Object name that raised the event</param>
        /// <param name="ObjectAction">Description of the action</param>
        /// <param name="Code">10 Event, 20 Fault, 30 User Interaction</param>
        public void InsertLogValue(DateTime Date, double OADate, string ObjectName, string ObjectAction, int Code)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SqlConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO ApplicationLog VALUES ('" + Date + "'," + OADate + ",'" + ObjectName + "','" + ObjectAction + "'," + Code + ")";
                    int rowsInserted = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Insert To Log --> " + ex.Message, "Error Adding Log Item", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Loads all SmartTags that are marked as a User tag from SQL into memory
        /// </summary>
        /// <summary>
        /// Loads all SmartTags that are marked as a User tag from SQL into memory
        /// </summary>
        public void LoadAdditionalTagsOnlineFromSQL()
        {
            string thisCurrentTag = "";
            try
            {

                //
                //  Select all tags from SQL
                //
                using (SqlConnection sqlConn = new SqlConnection(SqlConnectionString))
                {

                    sqlConn.Open();
                    SqlCommand cmd = sqlConn.CreateCommand();
                    cmd.CommandText = "SELECT st.Tagname, DBOffset, RecOnTick, RecOnChange, RecTrend  FROM SmartTags st WHERE UserTag = 1 ORDER BY TagName";
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        MyObjectInfo moi = new MyObjectInfo(reader.GetString(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4));

                        Tag t = new Tag();
                        thisCurrentTag = t.Name;
                        //
                        // Check special features of tag
                        //
                        bool IsRealDataType = false;
                        bool IsIntDataType = false;
                        bool IsDoubleIntDataType = false;
                        string tmpSpecialTagName = "";

                        if (moi.DBOffset.ToString().Contains("{R}")) //This tag contains special features
                        {
                            IsRealDataType = true;
                            tmpSpecialTagName = "" + moi.DBOffset.ToString().Remove(moi.DBOffset.ToString().IndexOf('{'), 3);
                        }

                        if (moi.DBOffset.ToString().Contains("{I}")) //This tag contains special features
                        {
                            IsIntDataType = true;
                            tmpSpecialTagName = "" + moi.DBOffset.ToString().Remove(moi.DBOffset.ToString().IndexOf('{'), 3);
                        }

                        if (moi.DBOffset.ToString().Contains("{D}")) //This tag contains special features
                        {
                            IsDoubleIntDataType = true;
                            tmpSpecialTagName = "" + moi.DBOffset.ToString().Remove(moi.DBOffset.ToString().IndexOf('{'), 3);
                        }

                        if (IsRealDataType || IsIntDataType || IsDoubleIntDataType)
                        {
                            t.Name = tmpSpecialTagName;
                        }
                        else
                        {
                            t.Name = moi.DBOffset.ToString();
                        }


                        //
                        // Check datatype of tag
                        //
                        int iPosOfPoint = t.Name.IndexOf('.');
                        string sRightOfPoint = t.Name.Substring((iPosOfPoint + 1), 3);

                        if (sRightOfPoint.ToUpper() == "DBX")
                        {
                            t.DataType = S7Link.Tag.ATOMIC.BOOL;
                        }
                        else if (sRightOfPoint.ToUpper() == "DBB")
                        {
                            t.DataType = S7Link.Tag.ATOMIC.BYTE;
                        }
                        else if (sRightOfPoint.ToUpper() == "DBW")
                        {
                            if (IsIntDataType)
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
                            //
                            // DBD is used to read Double Word as well as Real
                            //
                            if (IsRealDataType)
                            {
                                t.DataType = S7Link.Tag.ATOMIC.REAL;
                            }
                            else if (IsDoubleIntDataType)
                            {
                                t.DataType = S7Link.Tag.ATOMIC.DINT;
                            }
                            else
                            {
                                t.DataType = S7Link.Tag.ATOMIC.DWORD;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Datatype : " + sRightOfPoint + " is not supported!", "Invalid Datatype!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        t.MyObject = moi;
                        t.Controller = PLC1_R;
                        t.Changed += new EventHandler(INGEARS7_AdditionalTag_Changed);
                        tagroupAdditionalSmartTags.AddTag(t);

                        //htAdditionalTagCurrentValues.Add(moi.TagName, 0); //Default value of 0;

                        if (moi.RecOnTick == 1)
                        {
                            htRecTickTagValues.Add(moi.TagName, 0);
                        }
                    }
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load AdditionalTags --> " + ex.Message, "Error Loading Tags", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }



        /// <summary>
        /// Load the values from the LineParameter table into memory for the control box buttons and Line and Section States
        /// </summary>
        public void LoadControlButtonsOnline()
        {
            try
            {

                using (SqlConnection sqlConn = new SqlConnection(SqlConnectionString))  //Return All lines that have values in their lineDB
                {
                    sqlConn.Open();
                    SqlCommand cmd = sqlConn.CreateCommand();
                    cmd.CommandText = "SELECT LineNumber, LineDB, cmdImmediateStop, cmdHornOff, cmdSequenceStop, cmdfaultReset, cmdFeedOn, cmdStart, cmdTransferOn, RequestExecute, cmdFeedOff, LineStateCode, S1_DB, S1_StateCode, S1_parEmptyingTime, S1_outEmptyingTime, S2_DB, S2_StateCode, S2_parEmptyingTime, S2_OutEmptyingTime, S3_DB, S2_StateCode, S3_parEmptyingTime, S3_OutEmptyingTime, CmdReset, RequestModify, RequestDefine FROM LineParameters WHERE LineDB != '' ORDER BY LineNumber";
                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        ControlBoxSet cbs = new ControlBoxSet(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetString(7), reader.GetString(8), reader.GetString(9), reader.GetString(10), reader.GetString(11), reader.GetString(12), reader.GetString(13), reader.GetString(14), reader.GetString(15), reader.GetString(16), reader.GetString(17), reader.GetString(18), reader.GetString(19), reader.GetString(20), reader.GetString(21), reader.GetString(22), reader.GetString(23), reader.GetString(24), reader.GetString(25), reader.GetString(26));
                        alControlBoxSet.Add(cbs);
                    }
                    sqlConn.Close();
                }

                //Setup INGEAR Control Box Tags
                tControl_CmdFeedOn.DataType = S7Link.Tag.ATOMIC.BOOL;
                tControl_CmdStart.DataType = S7Link.Tag.ATOMIC.BOOL;
                tControl_CmdTransferOn.DataType = S7Link.Tag.ATOMIC.BOOL;
                tControl_CmdRequestExecute.DataType = S7Link.Tag.ATOMIC.BOOL;
                tControl_CmdRequestModify.DataType = S7Link.Tag.ATOMIC.BOOL;
                tControl_CmdFeedOff.DataType = S7Link.Tag.ATOMIC.BOOL;
                tControl_CmdHornOff.DataType = S7Link.Tag.ATOMIC.BOOL;
                tControl_CmdFaultReset.DataType = S7Link.Tag.ATOMIC.BOOL;
                tControl_CmdSeqStop.DataType = S7Link.Tag.ATOMIC.BOOL;
                tControl_CmdEStop.DataType = S7Link.Tag.ATOMIC.BOOL;
                tControl_CmdReset.DataType = S7Link.Tag.ATOMIC.BOOL;
                tControl_CmdRequestDefine.DataType = S7Link.Tag.ATOMIC.BOOL;

                tControl_CmdFeedOn.Length = 1;
                tControl_CmdStart.Length = 1;
                tControl_CmdTransferOn.Length = 1;
                tControl_CmdRequestExecute.Length = 1;
                tControl_CmdRequestModify.Length = 1;
                tControl_CmdFeedOff.Length = 1;
                tControl_CmdHornOff.Length = 1;
                tControl_CmdFaultReset.Length = 1;
                tControl_CmdSeqStop.Length = 1;
                tControl_CmdEStop.Length = 1;
                tControl_CmdReset.Length = 1;
                tControl_CmdRequestDefine.Length = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load ControlButtons --> " + ex.Message, "Error Loading Buttons", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        /// <summary>
        /// Writes all data in the Arraylist to SQL
        /// </summary>
        public void WriteReportingLogToSQL()
        {
            try
            {
                ArrayList alReportingLogLOCAL = new ArrayList();
                alReportingLogLOCAL.AddRange(alReportingLog);
                alReportingLog.Clear();

                Hashtable htLOCAL = new Hashtable(htRecTickTagValues);

                foreach (string sKey in htLOCAL.Keys)
                {
                    LogItem li = new LogItem(DateTime.Now, DateTime.Now.ToOADate(), sKey, htLOCAL[sKey]);
                    alReportingLogLOCAL.Add(li);
                }

                if (alReportingLogLOCAL.Count > 0 && bRunSQLCleanup == false)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("INSERT INTO HistoricLog VALUES ");

                    for (int i = 0; i < alReportingLogLOCAL.Count; i++)
                    {
                        LogItem li = (LogItem)alReportingLogLOCAL[i];
                        sb.Append("('" + li.ts_DateTime + "'," + li.OADate + ",'" + li.ObjectName + "','" + li.Value + "'),");
                    }

                    sb.Remove(sb.Length - 1, 1); //remove Last ,

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(SqlConnectionString))
                        {
                            conn.Open();
                            SqlCommand cmd = conn.CreateCommand();
                            cmd.CommandText = sb.ToString();
                            int rowsInserted = cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Insert To HistoricLog --> " + ex.Message, "Error Adding Log Item", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                alReportingLogLOCAL.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Writing Log --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion



        /// <summary>
        /// Load all Visual related application settings from SQL
        /// </summary>
        public void LoadVisualApplicationSettings()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SqlConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT SettingID, SettingValue FROM ApplicationSettings";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string sID = reader.GetString(0);
                        string sValue = reader.GetString(1);
                        //double dValue = 0;

                        switch (sID)
                        {
                            case "VIS_ZOOML1":
                                //dValue = Convert.ToDouble(sValue);
                                //pageINT1.uiScaleSlider.Value = dValue;
                                DisplayPages.INT1.sMatrixTransformValue = sValue;
                                break;
                                ///
                            case "VIS_ZOOML2":
                                //dValue = Convert.ToDouble(sValue);
                                //pageTRF1.uiScaleSlider.Value = dValue;
                                DisplayPages.FCL1.sMatrixTransformValue = sValue;
                                break;

                            case "VIS_ZOOML3":
                                //dValue = Convert.ToDouble(sValue);
                                //pageTRF1.uiScaleSlider.Value = dValue;
                                DisplayPages.MTR1.sMatrixTransformValue = sValue;
                                break;

                            case "VIS_ZOOML4":
                                //dValue = Convert.ToDouble(sValue);
                                //pageFCL1.uiScaleSlider.Value = dValue;
                                DisplayPages.MIL1A.sMatrixTransformValue = sValue;
                                break;
                   
                            case "VIS_ZOOML5":
                                //dValue = Convert.ToDouble(sValue);
                                //pageSCL1.uiScaleSlider.Value = dValue;
                                DisplayPages.MIL1B.sMatrixTransformValue = sValue;
                                break;

                      
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Vis Application Settings --> " + ex.Message, "Error Loading Settings", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Sets the visibility of the given elements/controls each time the iLineNumber changes (On changing display pages)
        /// </summary>
        public void SetControlVisibilityOnLineChange()
        {
            if (stat_iActiveLineNumber <= 0)
            {
                gbTipBox.Visibility = Visibility.Hidden;
                PlantSwitches.Visibility = Visibility.Hidden;
                MaintenanceSwitches.Visibility = Visibility.Hidden;
                lblLineName.Visibility = Visibility.Hidden;
                btnDisableZoom.IsEnabled = false;
                btnEnableZoom.IsEnabled = false;
                btnSaveZoom.IsEnabled = false;
                AlarmHorn1.Visibility = Visibility.Hidden;
                EmptyingTimer1.Visibility = Visibility.Hidden;
                //imageCustomerLogo.Visibility = Visibility.Hidden;
                imageBuhlerLogo.Visibility = Visibility.Hidden;
                btnControlAcknowledge.Visibility = Visibility.Hidden;
                btnControlStart.Visibility = Visibility.Hidden;
                btnControlSuspend.Visibility = Visibility.Hidden;
                btnControlMuteSiren.Visibility = Visibility.Hidden;
                btnControlSeqStop.Visibility = Visibility.Hidden;
                btnControlEmergencyStop.Visibility = Visibility.Hidden;
                btnTaglinks.Visibility = Visibility.Hidden;
                btnErrors.Visibility = Visibility.Hidden;
            }
            else
            {
                PlantSwitches.Visibility = Visibility.Visible;
                MaintenanceSwitches.Visibility = Visibility.Visible;
                lblLineName.Visibility = Visibility.Visible;
                AlarmHorn1.Visibility = Visibility.Visible;
                imageBuhlerLogo.Visibility = Visibility.Visible;

                int i = stat_ActiveLineName.IndexOf('-') + 1;
                int l = stat_ActiveLineName.Length;
                int r = l - i;

                lblLineName.Content = stat_ActiveLineName.ToString().Substring(i, r);
                btnDisableZoom.IsEnabled = true;
                btnEnableZoom.IsEnabled = true;
                btnSaveZoom.IsEnabled = true;
                //imageCustomerLogo.Visibility = Visibility.Visible;

            }

            if (stat_iActiveLineNumber > 0)
            {
                btnControlAcknowledge.Visibility = bOperatorAccess == true ? Visibility.Visible : Visibility.Hidden;
                btnControlStart.Visibility = bOperatorAccess == true ? Visibility.Visible : Visibility.Hidden;
                btnControlSuspend.Visibility = bOperatorAccess == true ? Visibility.Visible : Visibility.Hidden;
                btnControlMuteSiren.Visibility = bOperatorAccess == true ? Visibility.Visible : Visibility.Hidden;
                btnControlSeqStop.Visibility = bOperatorAccess == true ? Visibility.Visible : Visibility.Hidden;
                btnControlEmergencyStop.Visibility = bOperatorAccess == true ? Visibility.Visible : Visibility.Hidden;
                btnTaglinks.Visibility = bAdministratorAccess == true ? Visibility.Visible : Visibility.Hidden;
                btnErrors.Visibility = bAdministratorAccess == true ? Visibility.Visible : Visibility.Hidden;

            }

//elements unique to specific pages
            //if (stat_iActiveLineNumber == 8)
            //{
            //    FlourOutloadControlDesk.Visibility = Visibility.Visible;
            //    MixBackFlourControlDesk.Visibility = Visibility.Hidden;
            //}

             
            //Job List Button
            if ((stat_iActiveLineNumber == 1 || stat_iActiveLineNumber == 2 || stat_iActiveLineNumber == 3 || stat_iActiveLineNumber == 4 || stat_iActiveLineNumber == 5 || stat_iActiveLineNumber == 6 || stat_iActiveLineNumber == 7 || stat_iActiveLineNumber == 8 || stat_iActiveLineNumber == 9) && bLoggedIn)
            {
                btnJob.Visibility = Visibility.Visible;
            }
            else
            {
                btnJob.Visibility = Visibility.Hidden;
            }
        }



        /// <summary>
        /// This method loads the last used products in a bin from SQL
        /// </summary>
//        public void LoadBinProductsFromSQL()
//        {
//            try
//            {
//                using (SqlConnection conn = new SqlConnection(SqlConnectionString))
//                {
//                    conn.Open();
//                    SqlCommand cmd = conn.CreateCommand();
//                    cmd.CommandText = @"SELECT BinNumber, ProductAbbreviation FROM
//	                                        (SELECT a.BinID, a.ProductName, a.ProductAbbreviation 
//                                                FROM (SELECT *,ROW_NUMBER() OVER (PARTITION BY BinID ORDER BY ts_datetime DESC) AS rn 
//                                                        FROM BinProductChangeLog bpcl inner join Products p on bpcl.BinProductID = p.ProductID)a
//                                                             WHERE a.rn = 1)b
//                                                JOIN Bins on b.binID = Bins.binID";
//                    SqlDataReader reader = cmd.ExecuteReader();

//                    while (reader.Read())
//                    {
//                        if (slJobList_TRF1SenderBins[reader.GetInt32(0)] != null)
//                        {
//                            slJobList_TRF1SenderBins[reader.GetInt32(0)] = "Bin " + reader.GetInt32(0).ToString() + " - " + reader.GetString(1).ToUpper();
//                        }

//                        if (slJobList_TRF1ReceiverBins[reader.GetInt32(0)] != null)
//                        {
//                            slJobList_TRF1ReceiverBins[reader.GetInt32(0)] = "Bin " + reader.GetInt32(0).ToString() + " - " + reader.GetString(1).ToUpper();
//                        }

//                        if (slJobList_MIL1REC34ReceiverBins[reader.GetInt32(0)] != null)
//                        {
//                            slJobList_MIL1REC34ReceiverBins[reader.GetInt32(0)] = "Bin " + reader.GetInt32(0).ToString() + " - " + reader.GetString(1).ToUpper();
//                        }

//                        if (slJobList_MIL1REC35ReceiverBins[reader.GetInt32(0)] != null)
//                        {
//                            slJobList_MIL1REC35ReceiverBins[reader.GetInt32(0)] = "Bin " + reader.GetInt32(0).ToString() + " - " + reader.GetString(1).ToUpper();
//                        }

//                        if (slJobList_MIL1REC36ReceiverBins[reader.GetInt32(0)] != null)
//                        {
//                            slJobList_MIL1REC36ReceiverBins[reader.GetInt32(0)] = "Bin " + reader.GetInt32(0).ToString() + " - " + reader.GetString(1).ToUpper();
//                        }

//                        if (slJobList_MIL1REC38ReceiverBins[reader.GetInt32(0)] != null)
//                        {
//                            slJobList_MIL1REC38ReceiverBins[reader.GetInt32(0)] = "Bin " + reader.GetInt32(0).ToString() + " - " + reader.GetString(1).ToUpper();
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Load Bin Products From SQL --> " + ex.Message, "Error Loading Bin Products", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//        }


        //------------------------------------------------------------------------------//
        //                              Event Handlers                                  //
        //------------------------------------------------------------------------------//



        #region EventHandlers [Standard]


        /// <summary>
        /// Handles SmartTag state change
        /// </summary>        
        private void INGEARS7_Tag_Changed(object sender, EventArgs e)
        {
            if (bPLCCommsGood & PLC1_R.IsConnected)
            {


                Tag t = sender as Tag;

                MyObjectInfo MOI = (MyObjectInfo)t.MyObject;
                string MO = MOI.LinkedControlList;
                string[] sTagLinks = MO.Split(';');

                if (MOI.GroupNumber == "G002G" || MOI.GroupNumber == "G003G")
                {
                    UpdateElementColor(t);
                }
                else
                {
                    bool bLogFirstControlStateInList = true; //Used to remove the duplicate insert into the historiclog table if an element changes over multiple display pages (only log ONE state change, but update the element on all pages)

                    //Update element on every page that it is linked to
                    foreach (string myObject in sTagLinks)
                    {
                        if (myObject == "-")
                        {
                            break;
                        }
                        string sPageName = myObject.Substring(0, myObject.IndexOf("."));
                        string sControlType = myObject.Substring(myObject.IndexOf("(") + 1, myObject.IndexOf(")") - myObject.IndexOf("(") - 1);
                        string sControlName = myObject.Substring(myObject.IndexOf(")") + 1, myObject.LastIndexOf(".") - myObject.IndexOf(")") - 1);
                        string sPropertyName = myObject.Substring(myObject.LastIndexOf(".") + 1, myObject.Length - myObject.LastIndexOf(".") - 1);



                        //
                        //CHECK WHICH PAGE TO USE!------------------------------------------------------------------------------------------------------------TODO********************************************
                        //
                        Page p1 = null;

                        if (sPageName == "pageINT1")
                            p1 = pageINT1;
                        else if (sPageName == "pageFCL1")
                            p1 = pageFCL1;
                        else if (sPageName == "pageMTR1")
                            p1 = pageMTR1;
                        else if (sPageName == "pageMIL1")
                            p1 = pageMIL1;
                        else if (sPageName == "pageMIL2")
                            p1 = pageMIL2;
                        
                        else
                            errorList.Add(new Error() { ErrorTag = "PAGENAME", ErrorSource = "INGEARS7_Tag_Changed", ErrorString = sPageName + " has not been added to the list of pages" });

                        if (p1 != null)
                            UpdateControlState(p1, MOI.TagName, sControlType, sControlName, sPropertyName, Convert.ToInt16(t.Value), Convert.ToInt16(MOI.ParMsgType), t, bLogFirstControlStateInList);
                        else
                            AddError(new Error { ErrorTag = "PAGENAME", ErrorSource = "INGEARS7_Tag_Changed", ErrorString = "Pagename has not been set in INGEARS7_Tag_Changed." });

                        bLogFirstControlStateInList = false;
                    }
                }
            }
        }


        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------




        //---------------------------------------------------------------------------------//
        //                             Functionality Methods                               //
        //---------------------------------------------------------------------------------//

        #region UpdateControlState [Don't Change]
        private void UpdateControlState(Page p1, string Tagname, string sControlType, string sControlName, string sPropertyName, int iValue, int pType, Tag t, bool bLogThisChange)
        {

            string sStatus = "";
            string sName = ""; //Used for error class
            bool bConfigError = false;
            bool bControlDoesntExist = false;

            MyObjectInfo moi = t.MyObject as MyObjectInfo;

            switch (sControlType)
            {
                #region Airlock_MPSN
                case "Airlock_MPSN":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myAirlock_MPSN = (Airlock_MPSN)p1.FindName(sControlName);
                            if (myAirlock_MPSN == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myAirlock_MPSN.MotorColor = iValue;
                                sStatus = myAirlock_MPSN.Status_Motor;
                                sName = myAirlock_MPSN.Name;
                                myAirlock_MPSN.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myAirlock_MPSN.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myAirlock_MPSN.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Airlock_MPSN_Cyclone
                case "Airlock_MPSN_Cyclone":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myAirlock_MPSN_Cyclone = (Airlock_MPSN_Cyclone)p1.FindName(sControlName);
                            if (myAirlock_MPSN_Cyclone == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myAirlock_MPSN_Cyclone.MotorColor = iValue;
                                sStatus = myAirlock_MPSN_Cyclone.Status_Motor;
                                sName = myAirlock_MPSN_Cyclone.Name;
                                myAirlock_MPSN_Cyclone.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myAirlock_MPSN_Cyclone.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myAirlock_MPSN_Cyclone.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region AirValve1
                case "AirValve1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myAirValve1 = (AirValve1)p1.FindName(sControlName);
                            if (myAirValve1 == null) bControlDoesntExist = true;
                            if (sPropertyName == "ValveColor")
                            {
                                myAirValve1.ValveColor = iValue;
                                sStatus = myAirValve1.Status_Valve;
                                sName = myAirValve1.Name;
                                myAirValve1.Description_Valve = moi.TagName + " " + moi.TagDescription;
                                myAirValve1.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myAirValve1.Fault_Valve;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Aspirator_MVSB
                case "Aspirator_MVSB":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myAspirator_MVSB = (Aspirator_MVSB)p1.FindName(sControlName);
                            if (myAspirator_MVSB == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor1")
                            {
                                myAspirator_MVSB.MotorColor1 = iValue;
                                sStatus = myAspirator_MVSB.Status_Motor1;
                                sName = myAspirator_MVSB.Name;
                                myAspirator_MVSB.Description_Motor1 = moi.TagName + " " + moi.TagDescription;
                                myAspirator_MVSB.ObjectNumber1 = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myAspirator_MVSB.Fault_Motor1;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "MotorColor2")
                            {
                                myAspirator_MVSB.MotorColor2 = iValue;
                                sStatus = myAspirator_MVSB.Status_Motor2;
                                sName = myAspirator_MVSB.Name;
                                myAspirator_MVSB.Description_Motor2 = moi.TagName + " " + moi.TagDescription;
                                myAspirator_MVSB.ObjectNumber2 = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myAspirator_MVSB.Fault_Motor2;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Aspirator_MVSI
                case "Aspirator_MVSI":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myAspirator_MVSI = (Aspirator_MVSI)p1.FindName(sControlName);
                            if (myAspirator_MVSI == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myAspirator_MVSI.MotorColor = iValue;
                                sStatus = myAspirator_MVSI.Status_Motor;
                                sName = myAspirator_MVSI.Name;
                                myAspirator_MVSI.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myAspirator_MVSI.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myAspirator_MVSI.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Auger_InSilo1
                case "Auger_InSilo1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myAuger_InSilo1 = (Auger_InSilo1)p1.FindName(sControlName);
                            if (myAuger_InSilo1 == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myAuger_InSilo1.MotorColor = iValue;
                                sStatus = myAuger_InSilo1.Status_Conveyor;
                                sName = myAuger_InSilo1.Name;
                                myAuger_InSilo1.Description_Conveyor = moi.TagName + " " + moi.TagDescription;
                                myAuger_InSilo1.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myAuger_InSilo1.Fault_Conveyor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Auger_Screw1
                case "Auger_Screw1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myAuger_Screw1 = (Auger_Screw1)p1.FindName(sControlName);
                            if (myAuger_Screw1 == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myAuger_Screw1.MotorColor = iValue;
                                sStatus = myAuger_Screw1.Status_Conveyor;
                                sName = myAuger_Screw1.Name;
                                myAuger_Screw1.Description_Conveyor = moi.TagName + " " + moi.TagDescription;
                                myAuger_Screw1.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myAuger_Screw1.Fault_Conveyor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region BaggingStation_BigBag
                case "BaggingStation_BigBag":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myBaggingStation_BigBag = (BaggingStation_BigBag)p1.FindName(sControlName);
                            if (myBaggingStation_BigBag == null) bControlDoesntExist = true;
                            if (sPropertyName == "BaggingStationColor")
                            {
                                myBaggingStation_BigBag.BaggingStationColor = iValue;
                                sStatus = myBaggingStation_BigBag.Status_BaggingStation;
                                sName = myBaggingStation_BigBag.Name;
                                myBaggingStation_BigBag.Description_BaggingStation = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myBaggingStation_BigBag.Fault_BaggingStation;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Bagging_ClosingConveyor_MWTC
                case "Bagging_ClosingConveyor_MWTC":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myBagging_ClosingConveyor_MWTC = (Bagging_ClosingConveyor_MWTC)p1.FindName(sControlName);
                            if (myBagging_ClosingConveyor_MWTC == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myBagging_ClosingConveyor_MWTC.MotorColor = iValue;
                                sStatus = myBagging_ClosingConveyor_MWTC.Status_Conveyor;
                                sName = myBagging_ClosingConveyor_MWTC.Name;
                                myBagging_ClosingConveyor_MWTC.Description_Conveyor = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myBagging_ClosingConveyor_MWTC.Fault_Conveyor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region BagginStation_MWPE
                case "BagginStation_MWPE":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myBagginStation_MWPE = (BagginStation_MWPE)p1.FindName(sControlName);
                            if (myBagginStation_MWPE == null) bControlDoesntExist = true;
                            if (sPropertyName == "BaggingStationColor")
                            {
                                myBagginStation_MWPE.BaggingStationColor = iValue;
                                sStatus = myBagginStation_MWPE.Status_BaggingStation;
                                sName = myBagginStation_MWPE.Name;
                                myBagginStation_MWPE.Description_BaggingMachine = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myBagginStation_MWPE.Fault_BaggingStation;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region BaggingStation_MWPM
                case "BaggingStation_MWPM":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myBaggingStation_MWPM = (BaggingStation_MWPM)p1.FindName(sControlName);
                            if (myBaggingStation_MWPM == null) bControlDoesntExist = true;
                            if (sPropertyName == "BaggingStationColor")
                            {
                                myBaggingStation_MWPM.BaggingStationColor = iValue;
                                sStatus = myBaggingStation_MWPM.Status_BaggingStation;
                                sName = myBaggingStation_MWPM.Name;
                                myBaggingStation_MWPM.Description_Monitor = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myBaggingStation_MWPM.Fault_BaggingStation;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region BaggingStation_RollTracks
                case "BaggingStation_RollTracks":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myBaggingStation_RollTracks = (BaggingStation_RollTracks)p1.FindName(sControlName);
                            if (myBaggingStation_RollTracks == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myBaggingStation_RollTracks.MotorColor = iValue;
                                sStatus = myBaggingStation_RollTracks.Status_Motor;
                                sName = myBaggingStation_RollTracks.Name;
                                myBaggingStation_RollTracks.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myBaggingStation_RollTracks.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion


                #region Bin_Vibro
                case "Bin_Vibro":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myBin_Vibro = (Bin_Vibro)p1.FindName(sControlName);
                            if (myBin_Vibro == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myBin_Vibro.MotorColor = iValue;
                                sStatus = myBin_Vibro.Status_Motor;
                                sName = myBin_Vibro.Name;
                                myBin_Vibro.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myBin_Vibro.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myBin_Vibro.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region BlowThroughSievingMachine_MKZG
                case "BlowThroughSievingMachine_MKZG":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myBlowThroughSievingMachine_MKZG = (BlowThroughSievingMachine_MKZG)p1.FindName(sControlName);
                            if (myBlowThroughSievingMachine_MKZG == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myBlowThroughSievingMachine_MKZG.MotorColor = iValue;
                                sStatus = myBlowThroughSievingMachine_MKZG.Status_Motor;
                                sName = myBlowThroughSievingMachine_MKZG.Name;
                                myBlowThroughSievingMachine_MKZG.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myBlowThroughSievingMachine_MKZG.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myBlowThroughSievingMachine_MKZG.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region BranFinisher_MKLA
                case "BranFinisher_MKLA":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myBranFinisher_MKLA = (BranFinisher_MKLA)p1.FindName(sControlName);
                            if (myBranFinisher_MKLA == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myBranFinisher_MKLA.MotorColor = iValue;
                                sStatus = myBranFinisher_MKLA.Status_BranFinisher;
                                sName = myBranFinisher_MKLA.Name;
                                myBranFinisher_MKLA.Description_BranFinisher = moi.TagName + " " + moi.TagDescription;
                                myBranFinisher_MKLA.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myBranFinisher_MKLA.Fault_BranFinisher;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Bin_Discharger_MFPF
                case "Bin_Discharger_MFPF":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myBin_Discharger_MFPF = (Bin_Discharger_MFPF)p1.FindName(sControlName);
                            if (myBin_Discharger_MFPF == null) bControlDoesntExist = true;
                            if (sPropertyName == "DischargerColor")
                            {
                                myBin_Discharger_MFPF.DischargerColor = iValue;
                                sStatus = myBin_Discharger_MFPF.Status_Discharger;
                                sName = myBin_Discharger_MFPF.Name;
                                myBin_Discharger_MFPF.Description_Discharger = moi.TagName + " " + moi.TagDescription;
                                myBin_Discharger_MFPF.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myBin_Discharger_MFPF.Fault_Discharger;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region CombiCleaner_MTKB
                case "CombiCleaner_MTKB":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myCombiCleaner_MTKB = (CombiCleaner_MTKB)p1.FindName(sControlName);
                            if (myCombiCleaner_MTKB == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myCombiCleaner_MTKB.MotorColor = iValue;
                                sStatus = myCombiCleaner_MTKB.Status_Motor;
                                sName = myCombiCleaner_MTKB.Name;
                                myCombiCleaner_MTKB.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myCombiCleaner_MTKB.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myCombiCleaner_MTKB.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region CompressedAir_SprayNozzle
                case "CompressedAir_SprayNozzle":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myCompressedAir_SprayNozzle = (CompressedAir_SprayNozzle)p1.FindName(sControlName);
                            if (myCompressedAir_SprayNozzle == null) bControlDoesntExist = true;
                            if (sPropertyName == "NozzleColor")
                            {
                                myCompressedAir_SprayNozzle.NozzleColor = iValue;
                                sStatus = myCompressedAir_SprayNozzle.Status_Nozzle;
                                sName = myCompressedAir_SprayNozzle.Name;
                                myCompressedAir_SprayNozzle.Description_Nozzle = moi.TagName + " " + moi.TagDescription;
                                myCompressedAir_SprayNozzle.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myCompressedAir_SprayNozzle.Fault_Nozzle;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Compressor
                case "Compressor":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myCompressor = (Compressor)p1.FindName(sControlName);
                            if (myCompressor == null) bControlDoesntExist = true;
                            if (sPropertyName == "CompressorColor")
                            {
                                myCompressor.CompressorColor = iValue;
                                sStatus = myCompressor.Status_Compressor;
                                sName = myCompressor.Name;
                                myCompressor.Description_Compressor = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myCompressor.Fault_Compressor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Concentrator_MTCB
                case "Concentrator_MTCB":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConcentrator_MTCB = (Concentrator_MTCB)p1.FindName(sControlName);
                            if (myConcentrator_MTCB == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myConcentrator_MTCB.MotorColor = iValue;
                                sStatus = myConcentrator_MTCB.Status_Motor;
                                sName = myConcentrator_MTCB.Name;
                                myConcentrator_MTCB.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myConcentrator_MTCB.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConcentrator_MTCB.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion
                //ControlSieve_MKSA1
                #region ControlSieve_MKSA1
                case "ControlSieve_MKSA1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myControlSieve_MKSA1 = (ControlSieve_MKSA1)p1.FindName(sControlName);
                            if (myControlSieve_MKSA1 == null) bControlDoesntExist = true;
                            if (sPropertyName == "SieveColor")
                            {
                                myControlSieve_MKSA1.SieveColor = iValue;
                                sStatus = myControlSieve_MKSA1.Status_Sieve;
                                sName = myControlSieve_MKSA1.Name;
                                myControlSieve_MKSA1.Description_Sieve = moi.TagName + " " + moi.TagDescription;
                                myControlSieve_MKSA1.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myControlSieve_MKSA1.Fault_Sieve;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_BCTC
                case "Conveyor_BCTC":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_BCTC = (Conveyor_BCTC)p1.FindName(sControlName);
                            if (myConveyor_BCTC == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor1")
                            {
                                myConveyor_BCTC.MotorColor1 = iValue;
                                sStatus = myConveyor_BCTC.Status_Motor1;
                                sName = myConveyor_BCTC.Name;
                                myConveyor_BCTC.Description_Motor1 = moi.TagName + " " + moi.TagDescription;
                                myConveyor_BCTC.ObjectNumber1 = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_BCTC.Fault_Motor1;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "MotorColor2")
                            {
                                myConveyor_BCTC.MotorColor2 = iValue;
                                sStatus = myConveyor_BCTC.Status_Motor2;
                                sName = myConveyor_BCTC.Name;
                                myConveyor_BCTC.Description_Motor2 = moi.TagName + " " + moi.TagDescription;
                                myConveyor_BCTC.ObjectNumber2 = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_BCTC.Fault_Motor2;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_Belt_XS
                case "Conveyor_Belt_XS":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_Belt_XS = (Conveyor_Belt_XS)p1.FindName(sControlName);
                            if (myConveyor_Belt_XS == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myConveyor_Belt_XS.MotorColor = iValue;
                                sStatus = myConveyor_Belt_XS.Status_Conveyor;
                                sName = myConveyor_Belt_XS.Name;
                                myConveyor_Belt_XS.Description_Conveyor = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_Belt_XS.Fault_Conveyor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_Belt_S
                case "Conveyor_Belt_S":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_Belt_S = (Conveyor_Belt_S)p1.FindName(sControlName);
                            if (myConveyor_Belt_S == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myConveyor_Belt_S.MotorColor = iValue;
                                sStatus = myConveyor_Belt_S.Status_Conveyor;
                                sName = myConveyor_Belt_S.Name;
                                myConveyor_Belt_S.Description_Conveyor = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_Belt_S.Fault_Conveyor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_Belt_M
                case "Conveyor_Belt_M":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_Belt_M = (Conveyor_Belt_M)p1.FindName(sControlName);
                            if (myConveyor_Belt_M == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myConveyor_Belt_M.MotorColor = iValue;
                                sStatus = myConveyor_Belt_M.Status_Conveyor;
                                sName = myConveyor_Belt_M.Name;
                                myConveyor_Belt_M.Description_Conveyor = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_Belt_M.Fault_Conveyor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_Chain_AHKA_L
                case "Conveyor_Chain_AHKA_L":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_Chain_AHKA_L = (Conveyor_Chain_AHKA_L)p1.FindName(sControlName);
                            if (myConveyor_Chain_AHKA_L == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myConveyor_Chain_AHKA_L.MotorColor = iValue;
                                sStatus = myConveyor_Chain_AHKA_L.Status_Conveyor;
                                sName = myConveyor_Chain_AHKA_L.Name;
                                myConveyor_Chain_AHKA_L.Description_Conveyor = moi.TagName + " " + moi.TagDescription;
                                myConveyor_Chain_AHKA_L.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_Chain_AHKA_L.Fault_Conveyor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_Chain_AHKA_M
                case "Conveyor_Chain_AHKA_M":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_Chain_AHKA_M = (Conveyor_Chain_AHKA_M)p1.FindName(sControlName);
                            if (myConveyor_Chain_AHKA_M == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myConveyor_Chain_AHKA_M.MotorColor = iValue;
                                sStatus = myConveyor_Chain_AHKA_M.Status_Conveyor;
                                sName = myConveyor_Chain_AHKA_M.Name;
                                myConveyor_Chain_AHKA_M.Description_Conveyor = moi.TagName + " " + moi.TagDescription;
                                myConveyor_Chain_AHKA_M.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_Chain_AHKA_M.Fault_Conveyor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_Chain_AHKA_S
                case "Conveyor_Chain_AHKA_S":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_Chain_AHKA_S = (Conveyor_Chain_AHKA_S)p1.FindName(sControlName);
                            if (myConveyor_Chain_AHKA_S == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myConveyor_Chain_AHKA_S.MotorColor = iValue;
                                sStatus = myConveyor_Chain_AHKA_S.Status_Conveyor;
                                sName = myConveyor_Chain_AHKA_S.Name;
                                myConveyor_Chain_AHKA_S.Description_Conveyor = moi.TagName + " " + moi.TagDescription;
                                myConveyor_Chain_AHKA_S.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_Chain_AHKA_S.Fault_Conveyor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_Chain_AHKA_XL
                case "Conveyor_Chain_AHKA_XL":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_Chain_AHKA_XL = (Conveyor_Chain_AHKA_XL)p1.FindName(sControlName);
                            if (myConveyor_Chain_AHKA_XL == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myConveyor_Chain_AHKA_XL.MotorColor = iValue;
                                sStatus = myConveyor_Chain_AHKA_XL.Status_Conveyor;
                                sName = myConveyor_Chain_AHKA_XL.Name;
                                myConveyor_Chain_AHKA_XL.Description_Conveyor = moi.TagName + " " + moi.TagDescription;
                                myConveyor_Chain_AHKA_XL.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_Chain_AHKA_XL.Fault_Conveyor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_Chain_AHKA_XXL
                case "Conveyor_Chain_AHKA_XXL":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_Chain_AHKA_XXL = (Conveyor_Chain_AHKA_XXL)p1.FindName(sControlName);
                            if (myConveyor_Chain_AHKA_XXL == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myConveyor_Chain_AHKA_XXL.MotorColor = iValue;
                                sStatus = myConveyor_Chain_AHKA_XXL.Status_Conveyor;
                                sName = myConveyor_Chain_AHKA_XXL.Name;
                                myConveyor_Chain_AHKA_XXL.Description_Conveyor = moi.TagName + " " + moi.TagDescription;
                                myConveyor_Chain_AHKA_XXL.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_Chain_AHKA_XXL.Fault_Conveyor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_LimitSwitch
                case "Conveyor_LimitSwitch":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_LimitSwitch = (Conveyor_LimitSwitch)p1.FindName(sControlName);
                            if (myConveyor_LimitSwitch == null) bControlDoesntExist = true;
                            if (sPropertyName == "LimitColor")
                            {
                                myConveyor_LimitSwitch.LimitColor = iValue;
                                sStatus = myConveyor_LimitSwitch.Status_LimitSwitch;
                                sName = myConveyor_LimitSwitch.Name;
                                myConveyor_LimitSwitch.Description_LimitSwitch = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_LimitSwitch.Fault_LimitSwitch;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_MNSG_250
                case "Conveyor_MNSG_250":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_MNSG_250 = (Conveyor_MNSG_250)p1.FindName(sControlName);
                            if (myConveyor_MNSG_250 == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myConveyor_MNSG_250.MotorColor = iValue;
                                sStatus = myConveyor_MNSG_250.Status_Conveyor;
                                sName = myConveyor_MNSG_250.Name;
                                myConveyor_MNSG_250.Description_Conveyor = moi.TagName + " " + moi.TagDescription;
                                myConveyor_MNSG_250.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_MNSG_250.Fault_Conveyor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_MZMA
                case "Conveyor_MZMA":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_MZMA = (Conveyor_MZMA)p1.FindName(sControlName);
                            if (myConveyor_MZMA == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myConveyor_MZMA.MotorColor = iValue;
                                sStatus = myConveyor_MZMA.Status_Conveyor;
                                sName = myConveyor_MZMA.Name;
                                myConveyor_MZMA.Description_Conveyor = moi.TagName + " " + moi.TagDescription;
                                myConveyor_MZMA.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_MZMA.Fault_Conveyor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_NFAS
                case "Conveyor_NFAS":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_NFAS = (Conveyor_NFAS)p1.FindName(sControlName);
                            if (myConveyor_NFAS == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myConveyor_NFAS.MotorColor = iValue;
                                sStatus = myConveyor_NFAS.Status_Conveyor;
                                sName = myConveyor_NFAS.Name;
                                myConveyor_NFAS.Description_Conveyor = moi.TagName + " " + moi.TagDescription;
                                myConveyor_NFAS.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_NFAS.Fault_Conveyor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_NFAS_M
                case "Conveyor_NFAS_M":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_NFAS_M = (Conveyor_NFAS_M)p1.FindName(sControlName);
                            if (myConveyor_NFAS_M == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myConveyor_NFAS_M.MotorColor = iValue;
                                sStatus = myConveyor_NFAS_M.Status_Conveyor;
                                sName = myConveyor_NFAS_M.Name;
                                myConveyor_NFAS_M.Description_Conveyor = moi.TagName + " " + moi.TagDescription;
                                myConveyor_NFAS_M.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_NFAS_M.Fault_Conveyor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_NFAS_L
                case "Conveyor_NFAS_L":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_NFAS_L = (Conveyor_NFAS_L)p1.FindName(sControlName);
                            if (myConveyor_NFAS_L == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myConveyor_NFAS_L.MotorColor = iValue;
                                sStatus = myConveyor_NFAS_L.Status_Conveyor;
                                sName = myConveyor_NFAS_L.Name;
                                myConveyor_NFAS_L.Description_Conveyor = moi.TagName + " " + moi.TagDescription;
                                myConveyor_NFAS_L.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_NFAS_L.Fault_Conveyor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_NFAS_250
                case "Conveyor_NFAS_250":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_NFAS_250 = (Conveyor_NFAS_250)p1.FindName(sControlName);
                            if (myConveyor_NFAS_250 == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myConveyor_NFAS_250.MotorColor = iValue;
                                sStatus = myConveyor_NFAS_250.Status_Conveyor;
                                sName = myConveyor_NFAS_250.Name;
                                myConveyor_NFAS_250.Description_Conveyor = moi.TagName + " " + moi.TagDescription;
                                myConveyor_NFAS_250.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_NFAS_250.Fault_Conveyor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_NFAS_250Medium
                case "Conveyor_NFAS_250Medium":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_NFAS_250Medium = (Conveyor_NFAS_250Medium)p1.FindName(sControlName);
                            if (myConveyor_NFAS_250Medium == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myConveyor_NFAS_250Medium.MotorColor = iValue;
                                sStatus = myConveyor_NFAS_250Medium.Status_Conveyor;
                                sName = myConveyor_NFAS_250Medium.Name;
                                myConveyor_NFAS_250Medium.Description_Conveyor = moi.TagName + " " + moi.TagDescription;
                                myConveyor_NFAS_250Medium.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_NFAS_250Medium.Fault_Conveyor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_NFAS_250_L
                case "Conveyor_NFAS_250_L":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_NFAS_250_L = (Conveyor_NFAS_250_L)p1.FindName(sControlName);
                            if (myConveyor_NFAS_250_L == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myConveyor_NFAS_250_L.MotorColor = iValue;
                                sStatus = myConveyor_NFAS_250_L.Status_Conveyor;
                                sName = myConveyor_NFAS_250_L.Name;
                                myConveyor_NFAS_250_L.Description_Conveyor = moi.TagName + " " + moi.TagDescription;
                                myConveyor_NFAS_250_L.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_NFAS_250_L.Fault_Conveyor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_NFAS_250_XL
                case "Conveyor_NFAS_250_XL":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_NFAS_250_XL = (Conveyor_NFAS_250_XL)p1.FindName(sControlName);
                            if (myConveyor_NFAS_250_XL == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myConveyor_NFAS_250_XL.MotorColor = iValue;
                                sStatus = myConveyor_NFAS_250_XL.Status_Conveyor;
                                sName = myConveyor_NFAS_250_XL.Name;
                                myConveyor_NFAS_250_XL.Description_Conveyor = moi.TagName + " " + moi.TagDescription;
                                myConveyor_NFAS_250_XL.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_NFAS_250_XL.Fault_Conveyor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_OverflowFlap
                case "Conveyor_OverflowFlap":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_OverflowFlap = (Conveyor_OverflowFlap)p1.FindName(sControlName);
                            if (myConveyor_OverflowFlap == null) bControlDoesntExist = true;
                            if (sPropertyName == "OverFlowColor")
                            {
                                myConveyor_OverflowFlap.OverFlowColor = iValue;
                                sStatus = myConveyor_OverflowFlap.Status_Overflow;
                                sName = myConveyor_OverflowFlap.Name;
                                myConveyor_OverflowFlap.Description_Overflow = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_OverflowFlap.Fault_Overflow;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_SpeedMonitor_L
                case "Conveyor_SpeedMonitor_L":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_SpeedMonitor_L = (Conveyor_SpeedMonitor_L)p1.FindName(sControlName);
                            if (myConveyor_SpeedMonitor_L == null) bControlDoesntExist = true;
                            if (sPropertyName == "SpeedMonitorColor")
                            {
                                myConveyor_SpeedMonitor_L.SpeedMonitorColor = iValue;
                                sStatus = myConveyor_SpeedMonitor_L.Status_SpeedMonitor;
                                sName = myConveyor_SpeedMonitor_L.Name;
                                myConveyor_SpeedMonitor_L.Description_SpeedMonitor = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_SpeedMonitor_L.Fault_SpeedMonitor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Conveyor_SpeedMonitor_R
                case "Conveyor_SpeedMonitor_R":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myConveyor_SpeedMonitor_R = (Conveyor_SpeedMonitor_R)p1.FindName(sControlName);
                            if (myConveyor_SpeedMonitor_R == null) bControlDoesntExist = true;
                            if (sPropertyName == "SpeedMonitorColor")
                            {
                                myConveyor_SpeedMonitor_R.SpeedMonitorColor = iValue;
                                sStatus = myConveyor_SpeedMonitor_R.Status_SpeedMonitor;
                                sName = myConveyor_SpeedMonitor_R.Name;
                                myConveyor_SpeedMonitor_R.Description_SpeedMonitor = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myConveyor_SpeedMonitor_R.Fault_SpeedMonitor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region DampenerTurbolizer_MOZL
                case "DampenerTurbolizer_MOZL":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myDampenerTurbolizer_MOZL = (DampenerTurbolizer_MOZL)p1.FindName(sControlName);
                            if (myDampenerTurbolizer_MOZL == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myDampenerTurbolizer_MOZL.MotorColor = iValue;
                                sStatus = myDampenerTurbolizer_MOZL.Status_Dampener;
                                sName = myDampenerTurbolizer_MOZL.Name;
                                myDampenerTurbolizer_MOZL.Description_Dampener = moi.TagName + " " + moi.TagDescription;
                                myDampenerTurbolizer_MOZL.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myDampenerTurbolizer_MOZL.Fault_Dampener;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region DeHuller
                case "DeHuller":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myDeHuller = (DeHuller)p1.FindName(sControlName);
                            if (myDeHuller == null) bControlDoesntExist = true;
                            if (sPropertyName == "DeHullerColor")
                            {
                                myDeHuller.DeHullerColor = iValue;
                                sStatus = myDeHuller.Status_Dehuller;
                                sName = myDeHuller.Name;
                                myDeHuller.Description_Dehuller = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myDeHuller.Fault_Dehuller;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Degerminator_MHXM
                case "Degerminator_MHXM":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myDegerminator_MHXM = (Degerminator_MHXM)p1.FindName(sControlName);
                            if (myDegerminator_MHXM == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColorMain")
                            {
                                myDegerminator_MHXM.MotorColorMain = iValue;
                                sStatus = myDegerminator_MHXM.Status_MainMotor;
                                sName = myDegerminator_MHXM.Name;
                                myDegerminator_MHXM.Description_MainMotor = moi.TagName + " " + moi.TagDescription;
                                myDegerminator_MHXM.ObjectNumber1 = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myDegerminator_MHXM.Fault_MainMotor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "MotorColorFan")
                            {
                                myDegerminator_MHXM.MotorColorFan = iValue;
                                sStatus = myDegerminator_MHXM.Status_Fan;
                                sName = myDegerminator_MHXM.Name;
                                myDegerminator_MHXM.Description_Fan = moi.TagName + " " + moi.TagDescription;
                                myDegerminator_MHXM.ObjectNumber2 = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myDegerminator_MHXM.Fault_Fan;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Destoner_MTSD
                case "Destoner_MTSD":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myDestoner_MTSD = (Destoner_MTSD)p1.FindName(sControlName);
                            if (myDestoner_MTSD == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myDestoner_MTSD.MotorColor = iValue;
                                sStatus = myDestoner_MTSD.Status_Destoner;
                                sName = myDestoner_MTSD.Name;
                                myDestoner_MTSD.Description_Destoner = moi.TagName + " " + moi.TagDescription;
                                myDestoner_MTSD.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myDestoner_MTSD.Fault_Destoner;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Detacher_300G
                case "Detacher_300G":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myDetacher_300G = (Detacher_300G)p1.FindName(sControlName);
                            if (myDetacher_300G == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myDetacher_300G.MotorColor = iValue;
                                sStatus = myDetacher_300G.Status_Motor;
                                sName = myDetacher_300G.Name;
                                myDetacher_300G.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myDetacher_300G.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myDetacher_300G.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Detacher_MJZF
                case "Detacher_MJZF":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myDetacher_MJZF = (Detacher_MJZF)p1.FindName(sControlName);
                            if (myDetacher_MJZF == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myDetacher_MJZF.MotorColor = iValue;
                                sStatus = myDetacher_MJZF.Status_Detacher;
                                sName = myDetacher_MJZF.Name;
                                myDetacher_MJZF.Description_Detacher = moi.TagName + " " + moi.TagDescription;
                                myDetacher_MJZF.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myDetacher_MJZF.Fault_Detacher;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Detacher_MJZE
                case "Detacher_MJZE":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myDetacher_MJZE = (Detacher_MJZE)p1.FindName(sControlName);
                            if (myDetacher_MJZE == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myDetacher_MJZE.MotorColor = iValue;
                                sStatus = myDetacher_MJZE.Status_Detacher;
                                sName = myDetacher_MJZE.Name;
                                myDetacher_MJZE.Description_Detacher = moi.TagName + " " + moi.TagDescription;
                                myDetacher_MJZE.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myDetacher_MJZE.Fault_Detacher;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Detacher_MJZF_Left
                case "Detacher_MJZF_Left":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myDetacher_MJZF_Left = (Detacher_MJZF_Left)p1.FindName(sControlName);
                            if (myDetacher_MJZF_Left == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myDetacher_MJZF_Left.MotorColor = iValue;
                                sStatus = myDetacher_MJZF_Left.Status_Detacher;
                                sName = myDetacher_MJZF_Left.Name;
                                myDetacher_MJZF_Left.Description_Detacher = moi.TagName + " " + moi.TagDescription;
                                myDetacher_MJZF_Left.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myDetacher_MJZF_Left.Fault_Detacher;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region DrumSieve_AHCY
                case "DrumSieve_AHCY":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myDrumSieve_AHCY = (DrumSieve_AHCY)p1.FindName(sControlName);
                            if (myDrumSieve_AHCY == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myDrumSieve_AHCY.MotorColor = iValue;
                                sStatus = myDrumSieve_AHCY.Status_DrumSieve;
                                sName = myDrumSieve_AHCY.Name;
                                myDrumSieve_AHCY.Description_DrumSieve = moi.TagName + " " + moi.TagDescription;
                                myDrumSieve_AHCY.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myDrumSieve_AHCY.Fault_DrumSieve;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Dryer_Aeroglide
                case "Dryer_Aeroglide":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myDryer_Aeroglide = (Dryer_Aeroglide)p1.FindName(sControlName);
                            if (myDryer_Aeroglide == null) bControlDoesntExist = true;
                            if (sPropertyName == "ColorDryer")
                            {
                                myDryer_Aeroglide.ColorDryer = iValue;
                                sStatus = myDryer_Aeroglide.Status_Dryer;
                                sName = myDryer_Aeroglide.Name;
                                myDryer_Aeroglide.Description_Dryer = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myDryer_Aeroglide.Fault_Dryer;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Dryer_BeltDryer1
                case "Dryer_BeltDryer1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myDryer_BeltDryer1 = (Dryer_BeltDryer1)p1.FindName(sControlName);
                            if (myDryer_BeltDryer1 == null) bControlDoesntExist = true;
                            if (sPropertyName == "ColorBelt1")
                            {
                                myDryer_BeltDryer1.ColorBelt1 = iValue;
                                sStatus = myDryer_BeltDryer1.Status_Belt1;
                                sName = myDryer_BeltDryer1.Name;
                                myDryer_BeltDryer1.Description_Belt1 = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myDryer_BeltDryer1.Fault_Belt1;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region DustDetector1
                case "DustDetector1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myDustDetector1 = (DustDetector1)p1.FindName(sControlName);
                            if (myDustDetector1 == null) bControlDoesntExist = true;
                            if (sPropertyName == "DustDetectorColor")
                            {
                                myDustDetector1.DustDetectorColor = iValue;
                                sStatus = myDustDetector1.Status_DustDetector;
                                sName = myDustDetector1.Name;
                                myDustDetector1.Description_DustDetector = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myDustDetector1.Fault_DustDetector;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Elevator_L
                case "Elevator_L":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myElevator_L = (Elevator_L)p1.FindName(sControlName);
                            if (myElevator_L == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myElevator_L.MotorColor = iValue;
                                sStatus = myElevator_L.Status_Motor;
                                sName = myElevator_L.Name;
                                myElevator_L.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myElevator_L.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myElevator_L.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "SpeedMonitorColorBottom")
                            {
                                myElevator_L.SpeedMonitorColorBottom = iValue;
                                sStatus = myElevator_L.Status_SpeedMonitorBottom;
                                sName = myElevator_L.Name;
                                myElevator_L.Description_SpeedMonitorBottom = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myElevator_L.Fault_SpeedMonitorBottom;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "SpeedMonitorColorTop")
                            {
                                myElevator_L.SpeedMonitorColorTop = iValue;
                                sStatus = myElevator_L.Status_SpeedMonitorTop;
                                sName = myElevator_L.Name;
                                myElevator_L.Description_SpeedMonitorTop = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myElevator_L.Fault_SpeedMonitorTop;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "BeltAlignmentColor1")
                            {
                                myElevator_L.BeltAlignmentColor1 = iValue;
                                sName = myElevator_L.Name;
                            }
                            else if (sPropertyName == "BeltAlignmentColor2")
                            {
                                myElevator_L.BeltAlignmentColor2 = iValue;
                                sName = myElevator_L.Name;
                            }
                            else if (sPropertyName == "BeltAlignmentColor3")
                            {
                                myElevator_L.BeltAlignmentColor3 = iValue;
                                sName = myElevator_L.Name;
                            }
                            else if (sPropertyName == "BeltAlignmentColor4")
                            {
                                myElevator_L.BeltAlignmentColor4 = iValue;
                                sName = myElevator_L.Name;
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Elevator_S
                case "Elevator_S":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myElevator_S = (Elevator_S)p1.FindName(sControlName);
                            if (myElevator_S == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myElevator_S.MotorColor = iValue;
                                sStatus = myElevator_S.Status_Motor;
                                sName = myElevator_S.Name;
                                myElevator_S.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myElevator_S.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myElevator_S.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "SpeedMonitorColorBottom")
                            {
                                myElevator_S.SpeedMonitorColorBottom = iValue;
                                sStatus = myElevator_S.Status_SpeedMonitorBottom;
                                sName = myElevator_S.Name;
                                myElevator_S.Description_SpeedMonitorBottom = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myElevator_S.Fault_SpeedMonitorBottom;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "SpeedMonitorColorTop")
                            {
                                myElevator_S.SpeedMonitorColorTop = iValue;
                                sStatus = myElevator_S.Status_SpeedMonitorTop;
                                sName = myElevator_S.Name;
                                myElevator_S.Description_SpeedMonitorTop = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myElevator_S.Fault_SpeedMonitorTop;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "BeltAlignmentColor1")
                            {
                                myElevator_S.BeltAlignmentColor1 = iValue;
                                sName = myElevator_S.Name;
                            }
                            else if (sPropertyName == "BeltAlignmentColor2")
                            {
                                myElevator_S.BeltAlignmentColor2 = iValue;
                                sName = myElevator_S.Name;
                            }
                            else if (sPropertyName == "BeltAlignmentColor3")
                            {
                                myElevator_S.BeltAlignmentColor3 = iValue;
                                sName = myElevator_S.Name;
                            }
                            else if (sPropertyName == "BeltAlignmentColor4")
                            {
                                myElevator_S.BeltAlignmentColor4 = iValue;
                                sName = myElevator_S.Name;
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Elevator_M
                case "Elevator_M":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myElevator_M = (Elevator_M)p1.FindName(sControlName);
                            if (myElevator_M == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myElevator_M.MotorColor = iValue;
                                sStatus = myElevator_M.Status_Motor;
                                sName = myElevator_M.Name;
                                myElevator_M.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myElevator_M.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myElevator_M.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "SpeedMonitorColorBottom")
                            {
                                myElevator_M.SpeedMonitorColorBottom = iValue;
                                sStatus = myElevator_M.Status_SpeedMonitorBottom;
                                sName = myElevator_M.Name;
                                myElevator_M.Description_SpeedMonitorBottom = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myElevator_M.Fault_SpeedMonitorBottom;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "SpeedMonitorColorTop")
                            {
                                myElevator_M.SpeedMonitorColorTop = iValue;
                                sStatus = myElevator_M.Status_SpeedMonitorTop;
                                sName = myElevator_M.Name;
                                myElevator_M.Description_SpeedMonitorTop = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myElevator_M.Fault_SpeedMonitorTop;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "BeltAlignmentColor1")
                            {
                                myElevator_M.BeltAlignmentColor1 = iValue;
                                sName = myElevator_M.Name;
                            }
                            else if (sPropertyName == "BeltAlignmentColor2")
                            {
                                myElevator_M.BeltAlignmentColor2 = iValue;
                                sName = myElevator_M.Name;
                            }
                            else if (sPropertyName == "BeltAlignmentColor3")
                            {
                                myElevator_M.BeltAlignmentColor3 = iValue;
                                sName = myElevator_M.Name;
                            }
                            else if (sPropertyName == "BeltAlignmentColor4")
                            {
                                myElevator_M.BeltAlignmentColor4 = iValue;
                                sName = myElevator_M.Name;
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Fan_Blower
                case "Fan_Blower":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myFan_Blower = (Fan_Blower)p1.FindName(sControlName);
                            if (myFan_Blower == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myFan_Blower.MotorColor = iValue;
                                sStatus = myFan_Blower.Status_Motor;
                                sName = myFan_Blower.Name;
                                myFan_Blower.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myFan_Blower.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myFan_Blower.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Fan_HP
                case "Fan_HP":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myFan_HP = (Fan_HP)p1.FindName(sControlName);
                            if (myFan_HP == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myFan_HP.MotorColor = iValue;
                                sStatus = myFan_HP.Status_Motor;
                                sName = myFan_HP.Name;
                                myFan_HP.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myFan_HP.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myFan_HP.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Fan_Normal
                case "Fan_Normal":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myFan_Normal = (Fan_Normal)p1.FindName(sControlName);
                            if (myFan_Normal == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myFan_Normal.MotorColor = iValue;
                                sStatus = myFan_Normal.Status_Motor;
                                sName = myFan_Normal.Name;
                                myFan_Normal.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myFan_Normal.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myFan_Normal.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Feeder_LossInWeight_MSDF
                case "Feeder_LossInWeight_MSDF":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myFeeder_LossInWeight_MSDF = (Feeder_LossInWeight_MSDF)p1.FindName(sControlName);
                            if (myFeeder_LossInWeight_MSDF == null) bControlDoesntExist = true;
                            if (sPropertyName == "ColorFeeder")
                            {
                                myFeeder_LossInWeight_MSDF.ColorFeeder = iValue;
                                sStatus = myFeeder_LossInWeight_MSDF.Status_Feeder;
                                sName = myFeeder_LossInWeight_MSDF.Name;
                                myFeeder_LossInWeight_MSDF.Description_Feeder = moi.TagName + " " + moi.TagDescription;
                                myFeeder_LossInWeight_MSDF.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myFeeder_LossInWeight_MSDF.Fault_Feeder;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Feeder_MicroDifferential_1
                case "Feeder_MicroDifferential_1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myFeeder_MicroDifferential_1 = (Feeder_MicroDifferential_1)p1.FindName(sControlName);
                            if (myFeeder_MicroDifferential_1 == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myFeeder_MicroDifferential_1.MotorColor = iValue;
                                sStatus = myFeeder_MicroDifferential_1.Status_Motor;
                                sName = myFeeder_MicroDifferential_1.Name;
                                myFeeder_MicroDifferential_1.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myFeeder_MicroDifferential_1.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myFeeder_MicroDifferential_1.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Filter_MVRT
                case "Filter_MVRT":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myFilter_MVRT = (Filter_MVRT)p1.FindName(sControlName);
                            if (myFilter_MVRT == null) bControlDoesntExist = true;
                            if (sPropertyName == "FilterColor")
                            {
                                myFilter_MVRT.FilterColor = iValue;
                                sStatus = myFilter_MVRT.Status_Filter;
                                sName = myFilter_MVRT.Name;
                                myFilter_MVRT.Description_Filter = moi.TagName + " " + moi.TagDescription;
                                myFilter_MVRT.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myFilter_MVRT.Fault_Filter;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Filter_MVRU
                case "Filter_MVRU":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myFilter_MVRU = (Filter_MVRU)p1.FindName(sControlName);
                            if (myFilter_MVRU == null) bControlDoesntExist = true;
                            if (sPropertyName == "FilterColor")
                            {
                                myFilter_MVRU.FilterColor = iValue;
                                sStatus = myFilter_MVRU.Status_Filter;
                                sName = myFilter_MVRU.Name;
                                myFilter_MVRU.Description_Filter = moi.TagName + " " + moi.TagDescription;
                                myFilter_MVRU.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myFilter_MVRU.Fault_Filter;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Filter_MVRU_L
                case "Filter_MVRU_L":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myFilter_MVRU_L = (Filter_MVRU_L)p1.FindName(sControlName);
                            if (myFilter_MVRU_L == null) bControlDoesntExist = true;
                            if (sPropertyName == "FilterColor")
                            {
                                myFilter_MVRU_L.FilterColor = iValue;
                                sStatus = myFilter_MVRU_L.Status_Filter;
                                sName = myFilter_MVRU_L.Name;
                                myFilter_MVRU_L.Description_Filter = moi.TagName + " " + moi.TagDescription;
                                myFilter_MVRU_L.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myFilter_MVRU_L.Fault_Filter;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Filter_MVRU1
                case "Filter_MVRU1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myFilter_MVRU1 = (Filter_MVRU1)p1.FindName(sControlName);
                            if (myFilter_MVRU1 == null) bControlDoesntExist = true;
                            if (sPropertyName == "FilterColor")
                            {
                                myFilter_MVRU1.FilterColor = iValue;
                                sStatus = myFilter_MVRU1.Status_Filter;
                                sName = myFilter_MVRU1.Name;
                                myFilter_MVRU1.Description_Filter = moi.TagName + " " + moi.TagDescription;
                                myFilter_MVRU1.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myFilter_MVRU1.Fault_Filter;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Flaker
                case "Flaker":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myFlaker = (Flaker)p1.FindName(sControlName);
                            if (myFlaker == null) bControlDoesntExist = true;
                            if (sPropertyName == "ColorFlaker")
                            {
                                myFlaker.ColorFlaker = iValue;
                                sStatus = myFlaker.Status_Flaker;
                                sName = myFlaker.Name;
                                myFlaker.Description_Flaker = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myFlaker.Fault_Flaker;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "ColorFlakerMotor1")
                            {
                                myFlaker.ColorFlakerMotor1 = iValue;
                                sStatus = myFlaker.Status_Flaker;
                                sName = myFlaker.Name;
                                myFlaker.Description_Flaker = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myFlaker.Fault_Flaker;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "ColorFlakerMotor2")
                            {
                                myFlaker.ColorFlakerMotor2 = iValue;
                                sStatus = myFlaker.Status_Flaker;
                                sName = myFlaker.Name;
                                myFlaker.Description_Flaker = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myFlaker.Fault_Flaker;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region FlapBox
                case "FlapBox":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myFlapBox = (FlapBox)p1.FindName(sControlName);
                            if (myFlapBox == null) bControlDoesntExist = true;
                            if (sPropertyName == "FlapState")
                            {
                                myFlapBox.FlapState = iValue;
                                sStatus = myFlapBox.Status_Flap;
                                sName = myFlapBox.Name;
                                myFlapBox.Description_Flap = moi.TagName + " " + moi.TagDescription;
                                myFlapBox.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myFlapBox.Fault_Flap;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Flowbalancer
                case "Flowbalancer":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myFlowbalancer = (Flowbalancer)p1.FindName(sControlName);
                            if (myFlowbalancer == null) bControlDoesntExist = true;
                            if (sPropertyName == "FlowbalancerColor")
                            {
                                myFlowbalancer.FlowbalancerColor = iValue;
                                sStatus = myFlowbalancer.Status_Flowbalancer;
                                sName = myFlowbalancer.Name;
                                myFlowbalancer.Description_Flowbalancer = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myFlowbalancer.Fault_Flowbalancer;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region FlowMeter1
                case "FlowMeter1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myFlowMeter1 = (FlowMeter1)p1.FindName(sControlName);
                            if (myFlowMeter1 == null) bControlDoesntExist = true;
                            if (sPropertyName == "MeterColor")
                            {
                                myFlowMeter1.MeterColor = iValue;
                                sStatus = myFlowMeter1.Status_Meter;
                                sName = myFlowMeter1.Name;
                                myFlowMeter1.Description_Meter = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myFlowMeter1.Fault_Meter;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region HammerMill_DFCQ
                case "HammerMill_DFCQ":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myHammerMill_DFCQ = (HammerMill_DFCQ)p1.FindName(sControlName);
                            if (myHammerMill_DFCQ == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myHammerMill_DFCQ.MotorColor = iValue;
                                sStatus = myHammerMill_DFCQ.Status_Motor;
                                sName = myHammerMill_DFCQ.Name;
                                myHammerMill_DFCQ.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myHammerMill_DFCQ.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myHammerMill_DFCQ.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region HammerMill_DFCQ2
                case "HammerMill_DFCQ2_Motor":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myHammerMill_DFCQ2_Motor = (HammerMill_DFCQ2_Motor)p1.FindName(sControlName);
                            if (myHammerMill_DFCQ2_Motor == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myHammerMill_DFCQ2_Motor.MotorColor = iValue;
                                sStatus = myHammerMill_DFCQ2_Motor.Status_Motor;
                                sName = myHammerMill_DFCQ2_Motor.Name;
                                myHammerMill_DFCQ2_Motor.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myHammerMill_DFCQ2_Motor.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myHammerMill_DFCQ2_Motor.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region HammerMill_MJSA_MTSN
                case "HammerMill_MJSA_MTSN":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myHammerMill_MJSA_MTSN = (HammerMill_MJSA_MTSN)p1.FindName(sControlName);
                            if (myHammerMill_MJSA_MTSN == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myHammerMill_MJSA_MTSN.MotorColor = iValue;
                                sStatus = myHammerMill_MJSA_MTSN.Status_Motor;
                                sName = myHammerMill_MJSA_MTSN.Name;
                                myHammerMill_MJSA_MTSN.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myHammerMill_MJSA_MTSN.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myHammerMill_MJSA_MTSN.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion


                #region Hopper_BagIntake
                case "Hopper_BagIntake":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myHopperBagIntake = (Hopper_BagIntake)p1.FindName(sControlName);
                            if (myHopperBagIntake == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myHopperBagIntake.MotorColor = iValue;
                                sStatus = myHopperBagIntake.Status_Motor;
                                sName = myHopperBagIntake.Name;
                                myHopperBagIntake.ObjectNumber = moi.ObjectNo;
                                myHopperBagIntake.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myHopperBagIntake.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region IndentedSeperator_Mini
                case "IndentedSeperator_Mini":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myIndentedSeperator_Mini = (IndentedSeperator_Mini)p1.FindName(sControlName);
                            if (myIndentedSeperator_Mini == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myIndentedSeperator_Mini.MotorColor = iValue;
                                sStatus = myIndentedSeperator_Mini.Status_Motor;
                                sName = myIndentedSeperator_Mini.Name;
                                myIndentedSeperator_Mini.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myIndentedSeperator_Mini.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myIndentedSeperator_Mini.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region IndentedSeparator_Mini1
                case "IndentedSeparator_Mini1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myIndentedSeparator_Mini1 = (IndentedSeparator_Mini1)p1.FindName(sControlName);
                            if (myIndentedSeparator_Mini1 == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myIndentedSeparator_Mini1.MotorColor = iValue;
                                sStatus = myIndentedSeparator_Mini1.Status_Motor;
                                sName = myIndentedSeparator_Mini1.Name;
                                myIndentedSeparator_Mini1.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myIndentedSeparator_Mini1.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myIndentedSeparator_Mini1.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region IndentedSeparator_MTRI
                case "IndentedSeparator_MTRI":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myIndentedSeparator_MTRI = (IndentedSeparator_MTRI)p1.FindName(sControlName);
                            if (myIndentedSeparator_MTRI == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myIndentedSeparator_MTRI.MotorColor = iValue;
                                sStatus = myIndentedSeparator_MTRI.Status_Motor;
                                sName = myIndentedSeparator_MTRI.Name;
                                myIndentedSeparator_MTRI.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myIndentedSeparator_MTRI.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myIndentedSeparator_MTRI.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region KnockingHammer1
                case "KnockingHammer1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myKnockingHammer1 = (KnockingHammer1)p1.FindName(sControlName);
                            if (myKnockingHammer1 == null) bControlDoesntExist = true;
                            if (sPropertyName == "HammerColor")
                            {
                                myKnockingHammer1.HammerColor = iValue;
                                sStatus = myKnockingHammer1.Status_Hammer;
                                sName = myKnockingHammer1.Name;
                                myKnockingHammer1.Description_Hammer = moi.TagName + " " + moi.TagDescription;
                                myKnockingHammer1.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myKnockingHammer1.Fault_Hammer;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Label_M
                case "Label_M":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myLabel_M = (Label_M)p1.FindName(sControlName);
                            if (myLabel_M == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myLabel_M.MotorColor = iValue;
                                sName = myLabel_M.Name;
                                myLabel_M.Description_Motor = moi.TagName + " " + moi.TagDescription;
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Level_High_Bin
                case "Level_High_Bin":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myLevel_High_Bin = (Level_High_Bin)p1.FindName(sControlName);
                            if (myLevel_High_Bin == null) bControlDoesntExist = true;
                            if (sPropertyName == "LevelColor")
                            {
                                myLevel_High_Bin.LevelColor = iValue;
                                sStatus = myLevel_High_Bin.Status_HighLevel;
                                sName = myLevel_High_Bin.Name;
                                myLevel_High_Bin.Description_HighLevel = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myLevel_High_Bin.Fault_HighLevel;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Level_High_Machine
                case "Level_High_Machine":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myLevel_High_Machine = (Level_High_Machine)p1.FindName(sControlName);
                            if (myLevel_High_Machine == null) bControlDoesntExist = true;
                            if (sPropertyName == "LevelColor")
                            {
                                myLevel_High_Machine.LevelColor = iValue;
                                sStatus = myLevel_High_Machine.Status_HighLevel;
                                sName = myLevel_High_Machine.Name;
                                myLevel_High_Machine.Description_HighLevel = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myLevel_High_Machine.Fault_HighLevel;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Level_Mid
                case "Level_Mid":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myLevel_Mid = (Level_Mid)p1.FindName(sControlName);
                            if (myLevel_Mid == null) bControlDoesntExist = true;
                            if (sPropertyName == "SetColor")
                            {
                                myLevel_Mid.SetColor(iValue, pType);
                                sStatus = myLevel_Mid.Status_MidLevel;
                                sName = myLevel_Mid.Name;
                                myLevel_Mid.Description_MidLevel = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myLevel_Mid.Fault_MidLevel;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Level_Low_Bin
                case "Level_Low_Bin":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myLevel_Low_Bin = (Level_Low_Bin)p1.FindName(sControlName);
                            if (myLevel_Low_Bin == null) bControlDoesntExist = true;
                            if (sPropertyName == "LevelColor")
                            {
                                myLevel_Low_Bin.LevelColor = iValue;
                                sStatus = myLevel_Low_Bin.Status_LowLevel;
                                sName = myLevel_Low_Bin.Name;
                                myLevel_Low_Bin.Description_LowLevel = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myLevel_Low_Bin.Fault_LowLevel;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Level_Low_Machine
                case "Level_Low_Machine":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myLevel_Low_Machine = (Level_Low_Machine)p1.FindName(sControlName);
                            if (myLevel_Low_Machine == null) bControlDoesntExist = true;
                            if (sPropertyName == "LevelColor")
                            {
                                myLevel_Low_Machine.LevelColor = iValue;
                                sStatus = myLevel_Low_Machine.Status_LowLevel;
                                sName = myLevel_Low_Machine.Name;
                                myLevel_Low_Machine.Description_LowLevel = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myLevel_Low_Machine.Fault_LowLevel;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Valve_Dosing1
                case "Valve_Dosing1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myValve_Dosing1 = (Valve_Dosing1)p1.FindName(sControlName);
                            if (myValve_Dosing1 == null) bControlDoesntExist = true;
                            if (sPropertyName == "ValveColor")
                            {
                                myValve_Dosing1.ValveColor = iValue;
                                sStatus = myValve_Dosing1.Status_Valve;
                                sName = myValve_Dosing1.Name;
                                myValve_Dosing1.Description_Valve = moi.TagName + " " + moi.TagDescription;
                                myValve_Dosing1.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myValve_Dosing1.Fault_Valve;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Valve_MAUB
                case "Valve_MAUB":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myValve_MAUB = (Valve_MAUB)p1.FindName(sControlName);
                            if (myValve_MAUB == null) bControlDoesntExist = true;
                            if (sPropertyName == "ValveColor")
                            {
                                myValve_MAUB.ValveColor = iValue;
                                sStatus = myValve_MAUB.Status_Valve;
                                sName = myValve_MAUB.Name;
                                myValve_MAUB.Description_Valve = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myValve_MAUB.Fault_Valve;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Magnet_Rotary1
                case "Magnet_Rotary1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myMagnet_Rotary1 = (Magnet_Rotary1)p1.FindName(sControlName);
                            if (myMagnet_Rotary1 == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myMagnet_Rotary1.MotorColor = iValue;
                                sStatus = myMagnet_Rotary1.Status_Magnet_Rotary;
                                sName = myMagnet_Rotary1.Name;
                                myMagnet_Rotary1.Description_Magnet_Rotary = moi.TagName + " " + moi.TagDescription;
                                myMagnet_Rotary1.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myMagnet_Rotary1.Fault_Magnet_Rotary;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Mixer_DFML1000
                case "Mixer_DFML1000":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myMixer_DFML1000 = (Mixer_DFML1000)p1.FindName(sControlName);
                            if (myMixer_DFML1000 == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myMixer_DFML1000.MotorColor = iValue;
                                sStatus = myMixer_DFML1000.Status_Motor;
                                sName = myMixer_DFML1000.Name;
                                myMixer_DFML1000.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myMixer_DFML1000.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myMixer_DFML1000.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region MixerDoor1
                case "MixerDoor1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myMixerDoor1 = (MixerDoor1)p1.FindName(sControlName);
                            if (myMixerDoor1 == null) bControlDoesntExist = true;
                            if (sPropertyName == "SlideColor")
                            {
                                myMixerDoor1.SlideColor = iValue;
                                sStatus = myMixerDoor1.Status_Slide;
                                sName = myMixerDoor1.Name;
                                myMixerDoor1.Description_Slide = moi.TagName + " " + moi.TagDescription;
                                myMixerDoor1.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myMixerDoor1.Fault_Slide;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Monitor_Alarm1
                case "Monitor_Alarm1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myMonitor_Alarm1 = (Monitor_Alarm1)p1.FindName(sControlName);
                            if (myMonitor_Alarm1 == null) bControlDoesntExist = true;
                            if (sPropertyName == "MonitorColor")
                            {
                                myMonitor_Alarm1.MonitorColor = iValue;
                                sStatus = myMonitor_Alarm1.Status_Alarm;
                                sName = myMonitor_Alarm1.Name;
                                myMonitor_Alarm1.Description_Alarm = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myMonitor_Alarm1.Fault_Alarm;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Monitor_SifterOutlet
                case "Monitor_SifterOutlet":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myMonitor_SifterOutlet = (Monitor_SifterOutlet)p1.FindName(sControlName);
                            if (myMonitor_SifterOutlet == null) bControlDoesntExist = true;
                            if (sPropertyName == "MonitorColor")
                            {
                                myMonitor_SifterOutlet.MonitorColor = iValue;
                                sStatus = myMonitor_SifterOutlet.Status_Monitor;
                                sName = myMonitor_SifterOutlet.Name;
                                myMonitor_SifterOutlet.Description_Monitor = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myMonitor_SifterOutlet.Fault_Monitor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Monitor_Stroke
                case "Monitor_Stroke":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myMonitor_Stroke = (Monitor_Stroke)p1.FindName(sControlName);
                            if (myMonitor_Stroke == null) bControlDoesntExist = true;
                            if (sPropertyName == "MonitorColor")
                            {
                                myMonitor_Stroke.MonitorColor = iValue;
                                sStatus = myMonitor_Stroke.Status_Monitor;
                                sName = myMonitor_Stroke.Name;
                                myMonitor_Stroke.Description_Monitor = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myMonitor_Stroke.Fault_Monitor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region MOZF
                case "MOZF":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myMOZF = (MOZF)p1.FindName(sControlName);
                            if (myMOZF == null) bControlDoesntExist = true;
                            if (sPropertyName == "MOZFColor")
                            {
                                myMOZF.MOZFColor = iValue;
                                sStatus = myMOZF.Status_MOZF;
                                sName = myMOZF.Name;
                                myMOZF.Description_MOZF = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myMOZF.Fault_MOZF;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region MYFC
                case "MYFC":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myMYFC = (MYFC)p1.FindName(sControlName);
                            if (myMYFC == null) bControlDoesntExist = true;
                            if (sPropertyName == "MYFCColor")
                            {
                                myMYFC.MYFCColor = iValue;
                                sStatus = myMYFC.Status_MYFC;
                                sName = myMYFC.Name;
                                myMYFC.Description_MYFC = moi.TagName + " " + moi.TagDescription;
                                myMYFC.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myMYFC.Fault_MYFC;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region OTWDryer
                case "OTWDryer":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myOTWDryer = (OTWDryer)p1.FindName(sControlName);
                            if (myOTWDryer == null) bControlDoesntExist = true;
                            if (sPropertyName == "BeltMotorColor")
                            {
                                myOTWDryer.BeltMotorColor = iValue;
                                sStatus = myOTWDryer.Status_BeltMotor;
                                sName = myOTWDryer.Name;
                                myOTWDryer.Description_BeltMotor = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myOTWDryer.Fault_BeltMotor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "PulsatorMotorColor")
                            {
                                myOTWDryer.PulsatorMotorColor = iValue;
                                sStatus = myOTWDryer.Status_PulsatorMotor;
                                sName = myOTWDryer.Name;
                                myOTWDryer.Description_PulsatorMotor = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myOTWDryer.Fault_PulsatorMotor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion


                #region PowerMill_MDGB
                case "PowerMill_MDGB":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myPowerMill_MDGB = (PowerMill_MDGB)p1.FindName(sControlName);
                            if (myPowerMill_MDGB == null) bControlDoesntExist = true;
                            if (sPropertyName == "DetacherColor")
                            {
                                myPowerMill_MDGB.DetacherColor = iValue;
                                sStatus = myPowerMill_MDGB.Status_Detacher;
                                sName = myPowerMill_MDGB.Name;
                                myPowerMill_MDGB.Description_Detacher = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myPowerMill_MDGB.Fault_Detacher;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "HydraulicPumpColor")
                            {
                                myPowerMill_MDGB.HydraulicPumpColor = iValue;
                                sStatus = myPowerMill_MDGB.Status_HydraulicPump;
                                sName = myPowerMill_MDGB.Name;
                                myPowerMill_MDGB.Description_HydraulicPump = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myPowerMill_MDGB.Fault_HydraulicPump;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "LeftRollColor")
                            {
                                myPowerMill_MDGB.LeftRollColor = iValue;
                                sStatus = myPowerMill_MDGB.Status_LeftRoll;
                                sName = myPowerMill_MDGB.Name;
                                myPowerMill_MDGB.Description_LeftRoll = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myPowerMill_MDGB.Fault_LeftRoll;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "RightRollColor")
                            {
                                myPowerMill_MDGB.RightRollColor = iValue;
                                sStatus = myPowerMill_MDGB.Status_RightRoll;
                                sName = myPowerMill_MDGB.Name;
                                myPowerMill_MDGB.Description_RightRoll = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myPowerMill_MDGB.Fault_RightRoll;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Purifier_MQRF
                case "Purifier_MQRF":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myPurifier_MQRF = (Purifier_MQRF)p1.FindName(sControlName);
                            if (myPurifier_MQRF == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myPurifier_MQRF.MotorColor = iValue;
                                sStatus = myPurifier_MQRF.Status_Purifier;
                                sName = myPurifier_MQRF.Name;
                                myPurifier_MQRF.Description_Purifier = moi.TagName + " " + moi.TagDescription;
                                myPurifier_MQRF.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myPurifier_MQRF.Fault_Purifier;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Purifier_MQRF1
                case "Purifier_MQRF1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myPurifier_MQRF1 = (Purifier_MQRF1)p1.FindName(sControlName);
                            if (myPurifier_MQRF1 == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myPurifier_MQRF1.MotorColor = iValue;
                                sStatus = myPurifier_MQRF1.Status_Purifier;
                                sName = myPurifier_MQRF1.Name;
                                myPurifier_MQRF1.Description_Purifier = moi.TagName + " " + moi.TagDescription;
                                myPurifier_MQRF1.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myPurifier_MQRF1.Fault_Purifier;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Purifier_MQRF_SideView
                case "Purifier_MQRF_SideView":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myPurifier_MQRF_SideView = (Purifier_MQRF_SideView)p1.FindName(sControlName);
                            if (myPurifier_MQRF_SideView == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myPurifier_MQRF_SideView.MotorColor = iValue;
                                sStatus = myPurifier_MQRF_SideView.Status_Purifier;
                                sName = myPurifier_MQRF_SideView.Name;
                                myPurifier_MQRF_SideView.Description_Purifier = moi.TagName + " " + moi.TagDescription;
                                myPurifier_MQRF_SideView.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myPurifier_MQRF_SideView.Fault_Purifier;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region RollerStand_Engage
                case "RollerStand_Engage":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myRollerStand_Engage = (RollerStand_Engage)p1.FindName(sControlName);
                            if (myRollerStand_Engage == null) bControlDoesntExist = true;
                            if (sPropertyName == "EngageColor")
                            {
                                myRollerStand_Engage.EngageColor = iValue;
                                sStatus = myRollerStand_Engage.Status_Engage;
                                sName = myRollerStand_Engage.Name;
                                myRollerStand_Engage.Description_Engage = moi.TagName + " " + moi.TagDescription;
                                myRollerStand_Engage.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myRollerStand_Engage.Fault_Engage;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion


                #region RollerStand_MDDQ
                case "RollerStand_MDDQ":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myRollerStand_MDDQ = (RollerStand_MDDQ)p1.FindName(sControlName);
                            if (myRollerStand_MDDQ == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor1")
                            {
                                myRollerStand_MDDQ.MotorColor1 = iValue;
                                sStatus = myRollerStand_MDDQ.Status_Motor1;
                                sName = myRollerStand_MDDQ.Name;
                                myRollerStand_MDDQ.Description_Motor1 = moi.TagName + " " + moi.TagDescription;
                                myRollerStand_MDDQ.ObjectNumber1 = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myRollerStand_MDDQ.Fault_Motor1;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "MotorColor2")
                            {
                                myRollerStand_MDDQ.MotorColor2 = iValue;
                                sStatus = myRollerStand_MDDQ.Status_Motor2;
                                sName = myRollerStand_MDDQ.Name;
                                myRollerStand_MDDQ.Description_Motor2 = moi.TagName + " " + moi.TagDescription;
                                myRollerStand_MDDQ.ObjectNumber2 = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myRollerStand_MDDQ.Fault_Motor2;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            if (sPropertyName == "MotorColor3")
                            {
                                myRollerStand_MDDQ.MotorColor3 = iValue;
                                sStatus = myRollerStand_MDDQ.Status_Motor3;
                                sName = myRollerStand_MDDQ.Name;
                                myRollerStand_MDDQ.Description_Motor3 = moi.TagName + " " + moi.TagDescription;
                                myRollerStand_MDDQ.ObjectNumber3 = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myRollerStand_MDDQ.Fault_Motor3;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "MotorColor4")
                            {
                                myRollerStand_MDDQ.MotorColor4 = iValue;
                                sStatus = myRollerStand_MDDQ.Status_Motor4;
                                sName = myRollerStand_MDDQ.Name;
                                myRollerStand_MDDQ.Description_Motor4 = moi.TagName + " " + moi.TagDescription;
                                myRollerStand_MDDQ.ObjectNumber4 = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myRollerStand_MDDQ.Fault_Motor4;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "FrequencyConverter_ColorSide1")
                            {
                                myRollerStand_MDDQ.FrequencyConverter_ColorSide1 = iValue;
                                sStatus = myRollerStand_MDDQ.Status_FrequencyConverter1;
                                sName = myRollerStand_MDDQ.Name;
                                myRollerStand_MDDQ.Description_FrequencyConverter1 = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myRollerStand_MDDQ.Fault_FrequencyConverter1;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "FrequencyConverter_ColorSide2")
                            {
                                myRollerStand_MDDQ.FrequencyConverter_ColorSide2 = iValue;
                                sStatus = myRollerStand_MDDQ.Status_FrequencyConverter2;
                                sName = myRollerStand_MDDQ.Name;
                                myRollerStand_MDDQ.Description_FrequencyConverter2 = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myRollerStand_MDDQ.Fault_FrequencyConverter2;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region RollerStand_MDDR
                case "RollerStand_MDDR":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myRollerStand_MDDR = (RollerStand_MDDR)p1.FindName(sControlName);
                            if (myRollerStand_MDDR == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor1")
                            {
                                myRollerStand_MDDR.MotorColor1 = iValue;
                                sStatus = myRollerStand_MDDR.Status_Motor1;
                                sName = myRollerStand_MDDR.Name;
                                myRollerStand_MDDR.Description_Motor1 = moi.TagName + " " + moi.TagDescription;
                                myRollerStand_MDDR.ObjectNumber1 = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myRollerStand_MDDR.Fault_Motor1;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "MotorColor2")
                            {
                                myRollerStand_MDDR.MotorColor2 = iValue;
                                sStatus = myRollerStand_MDDR.Status_Motor2;
                                sName = myRollerStand_MDDR.Name;
                                myRollerStand_MDDR.Description_Motor2 = moi.TagName + " " + moi.TagDescription;
                                myRollerStand_MDDR.ObjectNumber2 = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myRollerStand_MDDR.Fault_Motor2;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "FrequencyConverter_ColorSide1")
                            {
                                myRollerStand_MDDR.FrequencyConverter_ColorSide1 = iValue;
                                sStatus = myRollerStand_MDDR.Status_FrequencyConverter1;
                                sName = myRollerStand_MDDR.Name;
                                myRollerStand_MDDR.Description_FrequencyConverter1 = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myRollerStand_MDDR.Fault_FrequencyConverter1;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "FrequencyConverter_ColorSide2")
                            {
                                myRollerStand_MDDR.FrequencyConverter_ColorSide2 = iValue;
                                sStatus = myRollerStand_MDDR.Status_FrequencyConverter2;
                                sName = myRollerStand_MDDR.Name;
                                myRollerStand_MDDR.Description_FrequencyConverter2 = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myRollerStand_MDDR.Fault_FrequencyConverter2;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region RotarySpout_MAYV
                case "RotarySpout_MAYV":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myRotarySpout_MAYV = (RotarySpout_MAYV)p1.FindName(sControlName);
                            if (myRotarySpout_MAYV == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myRotarySpout_MAYV.MotorColor = iValue;
                                sStatus = myRotarySpout_MAYV.Status_RotarySpout;
                                sName = myRotarySpout_MAYV.Name;
                                myRotarySpout_MAYV.Description_RotarySpout = moi.TagName + " " + moi.TagDescription;
                                myRotarySpout_MAYV.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myRotarySpout_MAYV.Fault_RotarySpout;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Scale_Dosing
                case "Scale_Dosing":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myScale_Dosing = (Scale_Dosing)p1.FindName(sControlName);
                            if (myScale_Dosing == null) bControlDoesntExist = true;
                            if (sPropertyName == "ScaleColor")
                            {
                                myScale_Dosing.ScaleColor = iValue;
                                sStatus = myScale_Dosing.Status_Scale;
                                sName = myScale_Dosing.Name;
                                myScale_Dosing.Description_Scale = moi.TagName + " " + moi.TagDescription;
                                myScale_Dosing.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myScale_Dosing.Fault_Scale;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Scale_Dosing1
                case "Scale_Dosing1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myScale_Dosing1 = (Scale_Dosing1)p1.FindName(sControlName);
                            if (myScale_Dosing1 == null) bControlDoesntExist = true;
                            if (sPropertyName == "ScaleColor")
                            {
                                myScale_Dosing1.ScaleColor = iValue;
                                sStatus = myScale_Dosing1.Status_Scale;
                                sName = myScale_Dosing1.Name;
                                myScale_Dosing1.Description_Scale = moi.TagName + " " + moi.TagDescription;
                                myScale_Dosing1.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myScale_Dosing1.Fault_Scale;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Scale_MSDM
                case "Scale_MSDM":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myScale_MSDM = (Scale_MSDM)p1.FindName(sControlName);
                            if (myScale_MSDM == null) bControlDoesntExist = true;
                            if (sPropertyName == "ScaleColor")
                            {
                                myScale_MSDM.ScaleColor = iValue;
                                sStatus = myScale_MSDM.Status_Scale;
                                sName = myScale_MSDM.Name;
                                myScale_MSDM.Description_Scale = moi.TagName + " " + moi.TagDescription;
                                myScale_MSDM.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myScale_MSDM.Fault_Scale;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Scale_MSDT500
                case "Scale_MSDT500":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myScale_MSDT500 = (Scale_MSDT500)p1.FindName(sControlName);
                            if (myScale_MSDT500 == null) bControlDoesntExist = true;
                            if (sPropertyName == "ScaleColor")
                            {
                                myScale_MSDT500.ScaleColor = iValue;
                                sStatus = myScale_MSDT500.Status_Scale;
                                sName = myScale_MSDT500.Name;
                                myScale_MSDT500.Description_Scale = moi.TagName + " " + moi.TagDescription;
                                myScale_MSDT500.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myScale_MSDT500.Fault_Scale;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region ScaleCentral_MSBA_A500
                case "ScaleCentral_MSBA_A500":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myScaleCentral_MSBA_A500 = (ScaleCentral_MSBA_A500)p1.FindName(sControlName);
                            if (myScaleCentral_MSBA_A500 == null) bControlDoesntExist = true;
                            if (sPropertyName == "ScaleColor")
                            {
                                myScaleCentral_MSBA_A500.ScaleColor = iValue;
                                sStatus = myScaleCentral_MSBA_A500.Status_Scale;
                                sName = myScaleCentral_MSBA_A500.Name;
                                myScaleCentral_MSBA_A500.Description_Scale = moi.TagName + " " + moi.TagDescription;
                                myScaleCentral_MSBA_A500.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myScaleCentral_MSBA_A500.Fault_Scale;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Scourer_MHXT
                case "Scourer_MHXT":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myScourer_MHXT = (Scourer_MHXT)p1.FindName(sControlName);
                            if (myScourer_MHXT == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myScourer_MHXT.MotorColor = iValue;
                                sStatus = myScourer_MHXT.Status_Scourer;
                                sName = myScourer_MHXT.Name;
                                myScourer_MHXT.Description_Scourer = moi.TagName + " " + moi.TagDescription;
                                myScourer_MHXT.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myScourer_MHXT.Fault_Scourer;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Separator_MANB
                case "Separator_MANB":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var mySeparator_MANB = (Separator_MANB)p1.FindName(sControlName);
                            if (mySeparator_MANB == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                mySeparator_MANB.MotorColor = iValue;
                                sStatus = mySeparator_MANB.Status_Motor;
                                sName = mySeparator_MANB.Name;
                                mySeparator_MANB.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                mySeparator_MANB.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = mySeparator_MANB.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Separator_MTRC
                case "Separator_MTRC":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var mySeparator_MTRC = (Separator_MTRC)p1.FindName(sControlName);
                            if (mySeparator_MTRC == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                mySeparator_MTRC.MotorColor = iValue;
                                sStatus = mySeparator_MTRC.Status_Separator;
                                sName = mySeparator_MTRC.Name;
                                mySeparator_MTRC.Description_Separator = moi.TagName + " " + moi.TagDescription;
                                mySeparator_MTRC.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = mySeparator_MTRC.Fault_Separator;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Sieve_1
                case "Sieve_1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var mySieve_1 = (Sieve_1)p1.FindName(sControlName);
                            if (mySieve_1 == null) bControlDoesntExist = true;
                            if (sPropertyName == "SieveColor")
                            {
                                mySieve_1.SieveColor = iValue;
                                sStatus = mySieve_1.Status_Sieve;
                                sName = mySieve_1.Name;
                                mySieve_1.Description_Sieve = moi.TagName + " " + moi.TagDescription;
                                mySieve_1.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = mySieve_1.Fault_Sieve;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region SievingMachine_MKZF
                case "SievingMachine_MKZF":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var mySievingMachine_MKZF = (SievingMachine_MKZF)p1.FindName(sControlName);
                            if (mySievingMachine_MKZF == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                mySievingMachine_MKZF.MotorColor = iValue;
                                sStatus = mySievingMachine_MKZF.Status_Motor;
                                sName = mySievingMachine_MKZF.Name;
                                mySievingMachine_MKZF.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                mySievingMachine_MKZF.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = mySievingMachine_MKZF.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion



                #region Sifter_MPAK
                case "Sifter_MPAK":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var mySifter_MPAK = (Sifter_MPAK)p1.FindName(sControlName);
                            if (mySifter_MPAK == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                mySifter_MPAK.MotorColor = iValue;
                                sStatus = mySifter_MPAK.Status_Sifter;
                                sName = mySifter_MPAK.Name;
                                mySifter_MPAK.Description_Sifter = moi.TagName + " " + moi.TagDescription;
                                mySifter_MPAK.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = mySifter_MPAK.Fault_Sifter;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Sifter_MPAK2
                case "Sifter_MPAK2":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var mySifter_MPAK2 = (Sifter_MPAK2)p1.FindName(sControlName);
                            if (mySifter_MPAK2 == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                mySifter_MPAK2.MotorColor = iValue;
                                sStatus = mySifter_MPAK2.Status_Sifter;
                                sName = mySifter_MPAK2.Name;
                                mySifter_MPAK2.Description_Sifter = moi.TagName + " " + moi.TagDescription;
                                mySifter_MPAK2.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = mySifter_MPAK2.Fault_Sifter;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Sifter_MPAP3
                case "Sifter_MPAP3":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var mySifter_MPAP3 = (Sifter_MPAP3)p1.FindName(sControlName);
                            if (mySifter_MPAP3 == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                mySifter_MPAP3.MotorColor = iValue;
                                sStatus = mySifter_MPAP3.Status_Sifter;
                                sName = mySifter_MPAP3.Name;
                                mySifter_MPAP3.Description_Sifter = moi.TagName + " " + moi.TagDescription;
                                mySifter_MPAP3.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = mySifter_MPAP3.Fault_Sifter;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Sifter_MPAP4
                case "Sifter_MPAP4":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var mySifter_MPAP4 = (Sifter_MPAP4)p1.FindName(sControlName);
                            if (mySifter_MPAP4 == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                mySifter_MPAP4.MotorColor = iValue;
                                sStatus = mySifter_MPAP4.Status_Sifter;
                                sName = mySifter_MPAP4.Name;
                                mySifter_MPAP4.Description_Sifter = moi.TagName + " " + moi.TagDescription;
                                mySifter_MPAP4.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = mySifter_MPAP4.Fault_Sifter;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Sifter_MPAR
                case "Sifter_MPAR":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var mySifter_MPAR = (Sifter_MPAR)p1.FindName(sControlName);
                            if (mySifter_MPAR == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                mySifter_MPAR.MotorColor = iValue;
                                sStatus = mySifter_MPAR.Status_Sifter;
                                sName = mySifter_MPAR.Name;
                                mySifter_MPAR.Description_Sifter = moi.TagName + " " + moi.TagDescription;
                                mySifter_MPAR.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = mySifter_MPAR.Fault_Sifter;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Sifter_MTZA1
                case "Sifter_MTZA1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var mySifter_MTZA1 = (Sifter_MTZA1)p1.FindName(sControlName);
                            if (mySifter_MTZA1 == null) bControlDoesntExist = true;
                            if (sPropertyName == "ColorAspirationMotor")
                            {
                                mySifter_MTZA1.ColorAspirationMotor = iValue;
                                sStatus = mySifter_MTZA1.Status_AspirationMotor;
                                sName = mySifter_MTZA1.Name;
                                mySifter_MTZA1.Description_AspirationMotor = moi.TagName + " " + moi.TagDescription;
                                mySifter_MTZA1.ObjectNumber1 = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = mySifter_MTZA1.Fault_AspirationMotor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else if (sPropertyName == "ColorFeedMotor")
                            {
                                mySifter_MTZA1.ColorFeedMotor = iValue;
                                sStatus = mySifter_MTZA1.Status_FeedMotor;
                                sName = mySifter_MTZA1.Name;
                                mySifter_MTZA1.Description_FeedMotor = moi.TagName + " " + moi.TagDescription;
                                mySifter_MTZA1.ObjectNumber2 = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = mySifter_MTZA1.Fault_FeedMotor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Silo_Door1
                case "Silo_Door1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var mySilo1_Door = (Silo_Door1)p1.FindName(sControlName);
                            if (mySilo1_Door == null) bControlDoesntExist = true;
                            if (sPropertyName == "DoorColor")
                            {
                                mySilo1_Door.DoorColor = iValue;
                                sStatus = mySilo1_Door.Status_Door;
                                sName = mySilo1_Door.Name;
                                mySilo1_Door.Description_Door = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = mySilo1_Door.Fault_Door;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region SlideGate
                case "SlideGate":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var mySlideGate = (SlideGate)p1.FindName(sControlName);
                            if (mySlideGate == null) bControlDoesntExist = true;
                            if (sPropertyName == "SlideColor")
                            {
                                mySlideGate.SlideColor = iValue;
                                sStatus = mySlideGate.Status_Slide;
                                sName = mySlideGate.Name;
                                mySlideGate.Description_Slide = moi.TagName + " " + moi.TagDescription;
                                mySlideGate.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = mySlideGate.Fault_Slide;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Solenoid_RinsingAir
                case "Solenoid_RinsingAir":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var mySolenoid_RinsingAir = (Solenoid_RinsingAir)p1.FindName(sControlName);
                            if (mySolenoid_RinsingAir == null) bControlDoesntExist = true;
                            if (sPropertyName == "ValveColor")
                            {
                                mySolenoid_RinsingAir.ValveColor = iValue;
                                sStatus = mySolenoid_RinsingAir.Status_Valve;
                                sName = mySolenoid_RinsingAir.Name;
                                mySolenoid_RinsingAir.Description_Valve = moi.TagName + " " + moi.TagDescription;
                                mySolenoid_RinsingAir.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = mySolenoid_RinsingAir.Fault_Valve;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Sortex1
                case "Sortex1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var mySortex1 = (Sortex1)p1.FindName(sControlName);
                            if (mySortex1 == null) bControlDoesntExist = true;
                            if (sPropertyName == "SortexColor")
                            {
                                mySortex1.SortexColor = iValue;
                                sStatus = mySortex1.Status_Sortex;
                                sName = mySortex1.Name;
                                mySortex1.Description_Sortex = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = mySortex1.Fault_Sortex;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region SpeedMonitor
                case "SpeedMonitor":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var mySpeedMonitor = (SpeedMonitor)p1.FindName(sControlName);
                            if (mySpeedMonitor == null) bControlDoesntExist = true;
                            if (sPropertyName == "MonitorColor")
                            {
                                mySpeedMonitor.MonitorColor = iValue;
                                sStatus = mySpeedMonitor.Status_SpeedMonitor;
                                sName = mySpeedMonitor.Name;
                                mySpeedMonitor.Description_SpeedMonitor = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = mySpeedMonitor.Fault_SpeedMonitor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region Switch_Pressure
                case "Switch_Pressure":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var mySwitch_Pressure = (Switch_Pressure)p1.FindName(sControlName);
                            if (mySwitch_Pressure == null) bControlDoesntExist = true;
                            if (sPropertyName == "PressureSwitchColor")
                            {
                                mySwitch_Pressure.PressureSwitchColor = iValue;
                                sStatus = mySwitch_Pressure.Status_PressureSwitch;
                                sName = mySwitch_Pressure.Name;
                                mySwitch_Pressure.Description_PressureSwitch = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = mySwitch_Pressure.Fault_PressureSwitch;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region TemperatureSensor_1
                case "TemperatureSensor_1":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myTemperatureSensor = (TemperatureSensor_1)p1.FindName(sControlName);
                            if (myTemperatureSensor == null) bControlDoesntExist = true;
                            if (sPropertyName == "SensorColor")
                            {
                                myTemperatureSensor.SensorColor = iValue;
                                sStatus = myTemperatureSensor.Status_Switch;
                                sName = myTemperatureSensor.Name;
                                myTemperatureSensor.Description_Switch = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myTemperatureSensor.Fault_Switch;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region ThrowSifter_DFTA_13
                case "ThrowSifter_DFTA_13":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myThrowSifter_DFTA_13 = (ThrowSifter_DFTA_13)p1.FindName(sControlName);
                            if (myThrowSifter_DFTA_13 == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myThrowSifter_DFTA_13.MotorColor = iValue;
                                sStatus = myThrowSifter_DFTA_13.Status_ThrowSifter;
                                sName = myThrowSifter_DFTA_13.Name;
                                myThrowSifter_DFTA_13.Description_ThrowSifter = moi.TagName + " " + moi.TagDescription;
                                myThrowSifter_DFTA_13.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myThrowSifter_DFTA_13.Fault_ThrowSifter;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion



                #region Valve_WaterShutoff
                case "Valve_WaterShutoff":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myValve_WaterShutoff = (Valve_WaterShutoff)p1.FindName(sControlName);
                            if (myValve_WaterShutoff == null) bControlDoesntExist = true;
                            if (sPropertyName == "VavleColor")
                            {
                                myValve_WaterShutoff.VavleColor = iValue;
                                sStatus = myValve_WaterShutoff.Status_Vavle;
                                sName = myValve_WaterShutoff.Name;
                                myValve_WaterShutoff.Description_Vavle = moi.TagName + " " + moi.TagDescription;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myValve_WaterShutoff.Fault_Vavle;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion


                #region VibratoryFeeder_MZVE
                case "VibratoryFeeder_MZVE":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myVibratoryFeeder_MZVE = (VibratoryFeeder_MZVE)p1.FindName(sControlName);
                            if (myVibratoryFeeder_MZVE == null) bControlDoesntExist = true;
                            if (sPropertyName == "FeederColor")
                            {
                                myVibratoryFeeder_MZVE.FeederColor = iValue;
                                sStatus = myVibratoryFeeder_MZVE.Status_Feeder;
                                sName = myVibratoryFeeder_MZVE.Name;
                                myVibratoryFeeder_MZVE.Description_Feeder = moi.TagName + " " + moi.TagDescription;
                                myVibratoryFeeder_MZVE.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myVibratoryFeeder_MZVE.Fault_Feeder;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region VibroSieve_MKZH
                case "VibroSieve_MKZH":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myVibroSieve_MKZH = (VibroSieve_MKZH)p1.FindName(sControlName);
                            if (myVibroSieve_MKZH == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myVibroSieve_MKZH.MotorColor = iValue;
                                sStatus = myVibroSieve_MKZH.Status_Motor;
                                sName = myVibroSieve_MKZH.Name;
                                myVibroSieve_MKZH.Description_Motor = moi.TagName + " " + moi.TagDescription;
                                myVibroSieve_MKZH.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myVibroSieve_MKZH.Fault_Motor;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                #region VibroSifter_MKZH
                case "VibroSifter_MKZH":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myVibroSifter_MKZH = (VibroSifter_MKZH)p1.FindName(sControlName);
                            if (myVibroSifter_MKZH == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myVibroSifter_MKZH.MotorColor = iValue;
                                sStatus = myVibroSifter_MKZH.Status_VibroSifter;
                                sName = myVibroSifter_MKZH.Name;
                                myVibroSifter_MKZH.Description_VibroSifter = moi.TagName + " " + moi.TagDescription;
                                myVibroSifter_MKZH.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myVibroSifter_MKZH.Fault_VibroSifter;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion



                #region Wheat_Steriliser_MJZD
                case "Wheat_Steriliser_MJZD":
                    p1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        try
                        {
                            var myWheat_Steriliser_MJZD = (Wheat_Steriliser_MJZD)p1.FindName(sControlName);
                            if (myWheat_Steriliser_MJZD == null) bControlDoesntExist = true;
                            if (sPropertyName == "MotorColor")
                            {
                                myWheat_Steriliser_MJZD.MotorColor = iValue;
                                sStatus = myWheat_Steriliser_MJZD.StatusWheat_Steriliser;
                                sName = myWheat_Steriliser_MJZD.Name;
                                myWheat_Steriliser_MJZD.DescriptionWheat_Steriliser = moi.TagName + " " + moi.TagDescription;
                                myWheat_Steriliser_MJZD.ObjectNumber = moi.ObjectNo;
                                string sFaultString = moi.TagDescription + " (" + sStatus + ")";
                                bool bIsInFault = myWheat_Steriliser_MJZD.FaultWheat_Steriliser;
                                RegisterEvent(bIsInFault, sFaultString, moi.TagName, iValue, bLogThisChange);
                            }
                            else bConfigError = true;
                        }
                        catch (NullReferenceException)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Link Error", ErrorString = "Control " + sControlName + " does not exist on display page. Links exist in database. [" + Tagname + "]" });
                            btnErrors.Background = Brushes.Red;
                        }
                        catch (Exception ae)
                        {
                            AddError(new Error { ErrorTag = sControlName, ErrorSource = "Control Type", ErrorString = ae.Message });
                            btnErrors.Background = Brushes.Red;
                        }
                    }));
                    break;
                #endregion

                case "...":
                    break;

                default:
                    AddError(new Error { ErrorTag = Tagname, ErrorSource = "STATE", ErrorString = "UserControl <" + sControlType + "> was not found" });
                    break;

            }



            //
            //Update the Element status event if the selected control has changed state
            // 
            if (sActiveControl == sName)
                sElementState = sStatus;

            if (bControlDoesntExist)
                AddError(new Error { ErrorTag = Tagname, ErrorSource = "CONTROL", ErrorString = sControlName + "<" + sControlType + "> does not exist. There are taglinks configured." });
            else if (bConfigError)
                AddError(new Error { ErrorTag = Tagname, ErrorSource = "STATE", ErrorString = "UserControl <" + sControlType + "> does not contain property : [" + sPropertyName + "]" });

        }
        #endregion



        /// <summary>
        /// Adds a new Error Object to the list
        /// </summary>
        private void AddError(Error eNew)
        {
            bool bExists = false;
            foreach (Error err in errorList)
            {
                if (err.ErrorSource == eNew.ErrorSource && err.ErrorString == eNew.ErrorString)
                    bExists = true;
            }

            if (!bExists)

                errorList.Add(eNew);
        }





        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Handles additional Element Tag Change events
        /// </summary>
        private void INGEARS7_AdditionalTag_Changed(object sender, EventArgs e)
        {
            if (bPLCCommsGood & PLC1_R.IsConnected)
            {
                Tag t = sender as Tag;
                MyObjectInfo moi = (MyObjectInfo)t.MyObject;

                //
                //Tag is a bad read
                //
                if (t.QualityCode == 0)
                {
                    if (!alBadTags.Contains(moi.TagName))
                    {
                        lblBadTags.Dispatcher.BeginInvoke(
                          System.Windows.Threading.DispatcherPriority.Normal,
                          new Action(
                            delegate()
                            {
                                lblBadTags.Content += "  *" + moi.TagName;
                            }
                          ));

                        alBadTags.Add(moi.TagName);
                    }
                }
                else
                {
                    UpdateAdditionalElementColor(t);

                    //
                    //Update the values in the Reporting OnTick hashtable
                    //
                    if (moi.RecOnTick == 1)
                    {
                        htRecTickTagValues[moi.TagName] = t.Value.ToString();
                    }

                    //
                    //Only start logging after all smart tags have been loaded for the first time
                    //
                    if (!bFirstAdditionalTagRead)
                    {
                        if (moi.RecOnChange == 1)
                        {
                            LogItem li = new LogItem(DateTime.Now, DateTime.Now.ToOADate(), moi.TagName, t.Value);
                            alReportingLog.Add(li);
                        }
                    }
                }
            }
        }



        //
        //  sElementDescription
        //
        private void MainWindow_sElementDescription_Changed(object sender, EventArgs e)
        {
            txtElementDescription.Text = sElementDescription;
        }


        //
        //  bLoggedIn
        //
        private void MainWindow_bLoggedIn_Changed(object sender, EventArgs e)
        {
            if (bLoggedIn == true)
            {
                txtLoggedInUser.Text = stat_sLoggedInUser;

                if (stat_iLogOffTime > 0)
                {
                    timerAutoLogOff.Start();
                }
            }
            else
            {
                txtLoggedInUser.Text = "";
                timerAutoLogOff.Stop();
                stat_iLogOffCounter = 0;
            }

            if (!bFirstTagRead & bLoggedIn == true)
            {
                pageSettings = new DisplayPages.Settings(this, PLC1_R, PLC1_W);
            }

            SetUserAccess();
            progBarUserLevel.Value = stat_iUserLevel;
        }


        //
        //  bCmdModify
        //
        private void MainWindow_bCmdModify_Changed(object sender, EventArgs e)
        {
            if (stat_bCmdModify == true)        //If the Modify Job bit is on
            {
                ModifyCurrentJob();             //Execute the modify
                bCmdModify = false;
            }
        }


        //
        //  sElementStatus
        //
        private void MainWindow_sElementStatus_Changed(object sender, EventArgs e)
        {
            txtElementStatus.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new Action(
                    delegate()
                    {
                        txtElementStatus.Text = sElementState;
                    }
                  ));

        }


        //
        //  
        //
        private void MainWindow_sActiveLine_Changed(object sender, EventArgs e)
        {
            txtLineName.Text = stat_ActiveLineName;
            HideEmptyingTimer();
        }


        private void MainWindow_bFault_Changed(object sender, EventArgs e)
        {
            if (stat_bFault)
            {
                txtAlarms.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();
                    myLinearGradientBrush.StartPoint = new Point(0, 0);
                    myLinearGradientBrush.EndPoint = new Point(0, 10);
                    myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.White, 0.0));
                    myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.Red, 0.1));

                    txtAlarms.Background = myLinearGradientBrush;

                    txtAlarms.Text = stat_sFault;
                    txtAlarms.FontSize = 12;
                }));

                timerAlarmFlash.Start();
            }
        }

        /// <summary>
        /// Registers the Tag that has just changed to the Logger (UI and SQL)
        /// </summary>        
        public void RegisterEvent(bool bIsInFault, string sFaultString, string Tagname, int iStateCode, bool bLogThisChange)
        {
            if (bIsInFault)
            {
                txtAlarms.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();
                    myLinearGradientBrush.StartPoint = new Point(0, 0);
                    myLinearGradientBrush.EndPoint = new Point(0, 10);
                    myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.White, 0.0));
                    myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.Red, 0.1));

                    txtAlarms.Background = myLinearGradientBrush;

                    txtAlarms.Text = sFaultString;
                    txtAlarms.FontSize = 12;
                }));

                timerAlarmFlash.Start();
            }

            if (!bFirstTagRead && bLogThisChange)
            {
                int ErrorCode;

                //Check if error code is fault or event
                if (!bIsInFault)
                {
                    ErrorCode = 10; //Event
                }
                else
                {
                    ErrorCode = 20; //Fault                    
                }

                LogItem li = new LogItem(DateTime.Now, DateTime.Now.ToOADate(), Tagname, sFaultString + "(" + iStateCode + ")", ErrorCode);
                alLoggerToSQL.Add(li);
                alLoggerToUI.Add(li);

            }
        }


        private void WindowMain_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                //cwa5033ScaleDetails.ScaleDetailsWindowCanClose = true;
                //cwa5033ScaleDetails.Close();

                DisplayPages.DisplayWindows.SplashScreenWindow.EndDisplay();
                timerApplicationHints.Stop();
                timerAutoLogOff.Stop();
                timerTrends.Stop();
                timerWriteLog.Stop();
                timerWriteReporting.Stop();

                threadTagRead.Abort();                                          //Abort the Tag read thread
                threadPLCComms.Abort();                                         //Abort the PLC Comms thread

                MainWindow.bThreadsToRun = false;

                //
                //Save all Data in memory to SQL
                //
                //WriteReportingLogToSQL();
                threadWriteToSQL = new Thread(new ThreadStart(WriteReportingLogToSQL));
                threadWriteToSQL.Start();
            }
            catch { }
        }





        /// <summary>
        /// Loads the relevant DBs and Offsets into the Control boxes tags for the Active line (Each time the line is changed)
        /// </summary>        
        private void MainWindow_iActiveLine_Changed(object sender, EventArgs e)
        {
            int itemIndex = 0;

            foreach (ControlBoxSet cbs in alControlBoxSet)
            {
                if (cbs.LineNumber == stat_iActiveLineNumber)
                {
                    itemIndex = stat_iActiveLineNumber;
                    break;
                }
                else
                {
                    itemIndex = -1;
                }
            }

            if (itemIndex > 0)     //Not negative means that the selected line number clicked on the window has configured DB Values
            {
                ControlBoxSet cbsi = (ControlBoxSet)alControlBoxSet[itemIndex - 1];

                //Assign the relevant DB numbers to the control box tags
                tControl_CmdFeedOn.Name = cbsi.LineDB + "." + cbsi.CmdFeedOn;
                tControl_CmdStart.Name = cbsi.LineDB + "." + cbsi.CmdStart;
                tControl_CmdTransferOn.Name = cbsi.LineDB + "." + cbsi.CmdTransferOn;
                tControl_CmdRequestExecute.Name = cbsi.LineDB + "." + cbsi.CmdRequestExecute;
                tControl_CmdRequestModify.Name = cbsi.LineDB + "." + cbsi.CmdRequestModify;
                tControl_CmdFeedOff.Name = cbsi.LineDB + "." + cbsi.CmdFeedOff;
                tControl_CmdHornOff.Name = cbsi.LineDB + "." + cbsi.CmdMute;
                tControl_CmdFaultReset.Name = cbsi.LineDB + "." + cbsi.CmdFaultReset;
                tControl_CmdSeqStop.Name = cbsi.LineDB + "." + cbsi.CmdSequenceStop;
                tControl_CmdEStop.Name = cbsi.LineDB + "." + cbsi.CmdEStop;
                tControl_CmdReset.Name = cbsi.LineDB + "." + cbsi.CmdReset;
                tControl_CmdRequestDefine.Name = cbsi.LineDB + "." + cbsi.CmdRequestDefine;

            }
            else
            {
                //Set the Control box tags to "DB0.DBX0.0" [As a defualt value] so that no line can be controlled on the button click event
                tControl_CmdFeedOn.Name = "DB0.DBX0.0";
                tControl_CmdStart.Name = "DB0.DBX0.0";
                tControl_CmdTransferOn.Name = "DB0.DBX0.0";
                tControl_CmdRequestExecute.Name = "DB0.DBX0.0";
                tControl_CmdRequestModify.Name = "DB0.DBX0.0";
                tControl_CmdFeedOff.Name = "DB0.DBX0.0";
                tControl_CmdHornOff.Name = "DB0.DBX0.0";
                tControl_CmdFaultReset.Name = "DB0.DBX0.0";
                tControl_CmdSeqStop.Name = "DB0.DBX0.0";
                tControl_CmdEStop.Name = "DB0.DBX0.0";
                tControl_CmdReset.Name = "DB0.DBX0.0";
                tControl_CmdRequestDefine.Name = "DB0.DBX0.0";
            }

            SetControlVisibilityOnLineChange();
        }

        /// <summary>
        /// Log any change that is made by the user of the system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UIInteraction_Change(object sender, RoutedEventArgs e)
        {
            Control c = (Control)sender;

            string ControlName = c.Name;
            string ControlSource = e.Source.ToString();
            string ControlEvent = e.RoutedEvent.ToString();
            DateTime EventTime = DateTime.Now;

            if (ControlName == "btnControlAcknowledge")                                 //Stop the alarm flash timer and set the active alarm tab to green
            {
                LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();
                myLinearGradientBrush.StartPoint = new Point(0, 0);
                myLinearGradientBrush.EndPoint = new Point(0, 10);
                myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.White, 0.0));
                myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.Lime, 0.1));
                txtAlarms.Background = myLinearGradientBrush;

                txtAlarms.Text = "No Active Alarms";
                txtAlarms.FontSize = 10;

                timerAlarmFlash.Stop();
                txtAlarms.Foreground = Brushes.Black;
            }


            LogItem li = new LogItem(DateTime.Now, DateTime.Now.ToOADate(), ControlName, "Button Pressed [" + stat_iActiveLineNumber + "]", 30);
            alLoggerToSQL.Add(li);
        }


        //OVERLOAD
        public void UIInteraction_Change(object sender, RoutedEventArgs e, string Message)
        {
            Control c = (Control)sender;

            string ControlName = c.Name;
            string ControlSource = e.Source.ToString();
            string ControlEvent = e.RoutedEvent.ToString();
            DateTime EventTime = DateTime.Now;

            if (ControlName == "btnControlAcknowledge")
            {
                LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();
                myLinearGradientBrush.StartPoint = new Point(0, 0);
                myLinearGradientBrush.EndPoint = new Point(0, 10);
                myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.White, 0.0));
                myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.Lime, 0.1));
                txtAlarms.Background = myLinearGradientBrush;

                txtAlarms.Text = "No Active Alarms";
                txtAlarms.FontSize = 10;

                timerAlarmFlash.Stop();
                txtAlarms.Foreground = Brushes.Black;
            }


            LogItem li = new LogItem(DateTime.Now, DateTime.Now.ToOADate(), ControlName, Message, 30);
            alLoggerToSQL.Add(li);
        }


        //OVERLOAD
        public void UIInteraction_Change(object sender, string Message)
        {
            Control c = (Control)sender;

            string ControlName = c.Name;
            DateTime EventTime = DateTime.Now;

            if (ControlName == "btnControlAcknowledge")
            {
                LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();
                myLinearGradientBrush.StartPoint = new Point(0, 0);
                myLinearGradientBrush.EndPoint = new Point(0, 10);
                myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.White, 0.0));
                myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.Lime, 0.1));
                txtAlarms.Background = myLinearGradientBrush;

                txtAlarms.Text = "No Active Alarms";
                txtAlarms.FontSize = 10;

                timerAlarmFlash.Stop();
                txtAlarms.Foreground = Brushes.Black;
            }


            LogItem li = new LogItem(DateTime.Now, DateTime.Now.ToOADate(), ControlName, Message, 30);
            alLoggerToSQL.Add(li);
        }

        /// <summary>
        /// Handle the state change of a Line
        /// </summary>      
        //NOT IN USE
        private void LineState_Changed(object sender, EventArgs e)
        {

        }

        //NOT IN USE
        private void ParEmpty_Changed(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// The OutEmpty of each line gets handled here by updating the line emptying timer that runs 
        /// </summary>
        private void OutEmpty_Changed(object sender, EventArgs e)
        {
            if (stat_iActiveLineNumber > 0)
            {
                Tag tOutEmpty = sender as Tag;
                Tag tParEmpty = tagroupSecParEmptying.Tags[stat_iActiveLineNumber - 1] as Tag;              //Only get the active lines Emtpying Time Tag              
                string[] words = standardCode.SplitStringInto2(')', tOutEmpty.MyObject.ToString());
                string lineNumber = words[0];                                                               //Get The line Number of the current Tag that is changing

                //else if (Int32.Parse(lineNumber) == stat_iActiveLineNumber)                                      //If the Active line has an emptying time Running
                if (Int32.Parse(lineNumber) == stat_iActiveLineNumber)                                      //If the Active line has an emptying time Running
                {

                    EmptyingTimer1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        EmptyingTimer1.Visibility = System.Windows.Visibility.Visible;
                        EmptyingTimer1.progressBarEmptying.Maximum = Convert.ToDouble(tParEmpty.Value);                                     //Set the maximum of the progress bar to the ParEmpty value
                        EmptyingTimer1.progressBarEmptying.Value = (Convert.ToDouble(tParEmpty.Value) - Convert.ToDouble(tOutEmpty.Value)); //Change the value of the emptyingtimer
                    }
                    ));

                    if ((Convert.ToDouble(tOutEmpty.Value) == 0) || (Convert.ToDouble(tOutEmpty.Value) == Convert.ToDouble(tParEmpty.Value)))                                             //Hide the timer if the value is < 1
                    {
                        EmptyingTimer1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            EmptyingTimer1.Visibility = System.Windows.Visibility.Hidden;
                        }
                        ));
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Handle the state change of a section
        /// </summary>        
        private void SectionState_Changed(object sender, EventArgs e)
        {

            if (MainWindow.bPLCCommsGood & PLC1_R.IsConnected)
            {
                Tag t = sender as Tag;

                if (t.Value != null)
                {

                    string[] Words = standardCode.SplitStringInto2(')', t.MyObject.ToString());              //Get the value of the Tag, This could be the state of the section OR a Fault of the section
                    int iLineNo = Convert.ToInt32(Words[0]);                                               //2 Separate words are being read for each [Section] Button
                    string MyObj = Words[1];
                    string TagValue = t.Value.ToString();


                    if (MyObj == "SECTION_STATE")                                               //Add a zero to the section's state, used in WPFBuhlerControls.SectionControl if this tag is a SECTION_STATE tag
                    {
                        TagValue += "0";
                    }
                    else if (MyObj == "SECTION_FAULT")                     //Check which bit is on in the DWORD
                    {
                        TagValue = "" + standardCode.GetRelevantBitValue(Convert.ToInt64(t.Value));
                    }


                    Color col = Colors.White;

                    if (TagValue == "20")  //Bits to exclude as errors (StandardCode.GetRelevantBitValue)
                    {
                        col = Colors.Fuchsia;
                        bSectionSTAFeedPriority[iLineNo - 1] = true;
                        UpdateLineButtonColor(iLineNo, col);
                    }
                    else if (TagValue == "32") //One of the relevant fault bits are [TRUE]
                    {
                        col = Colors.Red;
                        bSectionSTAFeedPriority[iLineNo - 1] = true;
                        UpdateLineButtonColor(iLineNo, col);
                    }
                    else
                    {
                        bSectionSTAFeedPriority[iLineNo - 1] = false;

                        //PCREAD.STEP--------------------------------------------------------------------------
                        //Bit 0 = StPassive     [White]
                        //Bit 1 = StWaiting     [Purple]
                        //Bit 2 = StActive      [Green]
                        //Bit 3 = StReady       [Lime]
                        //Bit 4 = StEmptying    [Aqua]
                        //Bit 5 = StEmptied     [Yellow]
                        //Bit 6 = StIdling      [LightGreen]

                        if (MyObj == "SECTION_STATE")
                        {
                            if (TagValue == "10")
                                col = Colors.White;
                            else if (TagValue == "20")
                                col = Colors.Fuchsia;
                            else if (TagValue == "40")
                                col = Colors.Green;
                            else if (TagValue == "80")
                                col = Colors.Lime;
                            else if (TagValue == "160")
                                col = Colors.Aqua;
                            else if (TagValue == "320")
                                col = Colors.Yellow;
                            else if (TagValue == "640")
                                col = Colors.LightGreen;

                            UpdateLineButtonColor(iLineNo, col);
                        }
                        else if (MyObj == "SECTION_FAULT" && TagValue == "0") //Revert back to previous color
                        {
                            try
                            {
                                Tag t1 = tagroupSecStaFeedOff.Tags[iLineNo - 1] as Tag;
                                //string sSecTagValue = t1.Value.ToString() + "0";
                                //if (sSecTagValue == "10")
                                //    br = Brushes.White;
                                //else if (sSecTagValue == "20")
                                //    br = Brushes.Fuchsia;
                                //else if (sSecTagValue == "40")
                                //    br = Brushes.Green;
                                //else if (sSecTagValue == "80")
                                //    br = Brushes.Lime;
                                //else if (sSecTagValue == "160")
                                //    br = Brushes.Aqua;
                                //else if (sSecTagValue == "320")
                                //    br = Brushes.Yellow;
                                //else if (sSecTagValue == "640")
                                //    br = Brushes.LightGreen;

                                Tag tSectionState = tagroupSecStates.Tags[iLineNo - 1] as Tag;

                                if (Convert.ToBoolean(t1.Value) && bSectionSTAFeedPriority[iLineNo - 1] == false && Convert.ToInt32(tSectionState.Value) == 4)   //StaFeedOff = 1, Prority = 0, SectionState = Active
                                {
                                    col = Colors.Lime;
                                    UpdateLineButtonColor(iLineNo, col);
                                }
                                else if (!Convert.ToBoolean(t1.Value) && bSectionSTAFeedPriority[iLineNo - 1] == false && Convert.ToInt32(tSectionState.Value) == 4)   //StaFeedOff = 1, Prority = 0, SectionState = Active
                                {
                                    col = Colors.Green;
                                    UpdateLineButtonColor(iLineNo, col);
                                }
                                //--
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        //Needed because when feed is off, section state does not change
        private void tStaFeedOff_Changed(object sender, EventArgs e)
        {
            if (MainWindow.bPLCCommsGood & PLC1_R.IsConnected)
            {
                Tag t = sender as Tag;
                Color col = Colors.White;
                int iLineNo = Convert.ToInt32(t.MyObject);
                Tag tSectionState = tagroupSecStates.Tags[iLineNo - 1] as Tag;

                if (Convert.ToBoolean(t.Value) && bSectionSTAFeedPriority[iLineNo - 1] == false)
                {
                    col = Colors.Lime;
                    UpdateLineButtonColor(iLineNo, col);
                }
                else if (!Convert.ToBoolean(t.Value) && bSectionSTAFeedPriority[iLineNo - 1] == false && Convert.ToInt32(tSectionState.Value) == 4) //StaFeedOff = False; Proirity = 0; Section is Active
                {
                    col = Colors.Green;
                    UpdateLineButtonColor(iLineNo, col);
                }
            }
        }


        private void UpdateLineButtonColor(int LineNumber, Color col)
        {

            if (LineNumber == 1)
            {
                btnLine1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    LinearGradientBrush lgb = new LinearGradientBrush();
                    lgb.StartPoint = new Point(0, 0);
                    lgb.EndPoint = new Point(0, 10);
                    lgb.GradientStops.Add(new GradientStop(Colors.White, 0.0));
                    lgb.GradientStops.Add(new GradientStop(col, 0.1));
                    btnLine1.Background = lgb;
                }));
            }
            else if (LineNumber == 2)
            {
                btnLine2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    LinearGradientBrush lgb = new LinearGradientBrush();
                    lgb.StartPoint = new Point(0, 0);
                    lgb.EndPoint = new Point(0, 10);
                    lgb.GradientStops.Add(new GradientStop(Colors.White, 0.0));
                    lgb.GradientStops.Add(new GradientStop(col, 0.1));
                    btnLine2.Background = lgb;
                }));
            }
            else if (LineNumber == 3)
            {
                btnLine3.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    LinearGradientBrush lgb = new LinearGradientBrush();
                    lgb.StartPoint = new Point(0, 0);
                    lgb.EndPoint = new Point(0, 10);
                    lgb.GradientStops.Add(new GradientStop(Colors.White, 0.0));
                    lgb.GradientStops.Add(new GradientStop(col, 0.1));
                    btnLine3.Background = lgb;
                }));
            }
            else if (LineNumber == 4)
            {
                btnLine4.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    LinearGradientBrush lgb = new LinearGradientBrush();
                    lgb.StartPoint = new Point(0, 0);
                    lgb.EndPoint = new Point(0, 10);
                    lgb.GradientStops.Add(new GradientStop(Colors.White, 0.0));
                    lgb.GradientStops.Add(new GradientStop(col, 0.1));
                    btnLine4.Background = lgb;
                }));
            }
            else if (LineNumber == 5)
            {
                btnLine5.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                {
                    LinearGradientBrush lgb = new LinearGradientBrush();
                    lgb.StartPoint = new Point(0, 0);
                    lgb.EndPoint = new Point(0, 10);
                    lgb.GradientStops.Add(new GradientStop(Colors.White, 0.0));
                    lgb.GradientStops.Add(new GradientStop(col, 0.1));
                    btnLine5.Background = lgb;
                }));
            }
            
        }

        //------------------------------------------------------------------------------//
        //                           Functionality Methods                              //
        //------------------------------------------------------------------------------//        


        /// <summary>
        /// Reads all Tags from the LineParameter Table and creates the tags for them
        /// </summary>
        public void LoadLineAndSectionStatesOnline()
        {
            string sErrorLineNumber = "";
            string sErrorLineType = "";
            try
            {
                //Create The line and section Tags for the [Line Buttons]

                for (int i = 0; i < alControlBoxSet.Count; i++)
                {
                    ControlBoxSet cbsItem = (ControlBoxSet)alControlBoxSet[i];

                    sErrorLineNumber = "" + i;

                    //______________________________________________________________________//ADDRESS OFFSETS
                    //SECTION 1
                    string LineStateName = cbsItem.LineDB + "." + cbsItem.LineStateCode;
                    string Section1StateName = cbsItem.LineS1DB + "." + cbsItem.LineS1StateCode;
                    string Section1StateFaultName = cbsItem.LineS1DB + ".DBD102";                           //Default R2 PCRead.Fault Double Word
                    string Section1ParEmptyingName = cbsItem.LineS1DB + "." + cbsItem.LineS1parEmptying;
                    string Section1OutEmptyingName = cbsItem.LineS1DB + "." + cbsItem.LineS1outEmptying;

                    //sTempCurrentDB1 = LineStateName;

                    //SECTION 2
                    string Section2StateName = "";
                    string Section2StateFaultName = "";
                    string Section2ParEmptyingName = "";
                    string Section2OutEmptyingName = "";

                    sErrorLineType = Convert.ToInt32(alLineTypes[i]).ToString();

                    if (Convert.ToInt32(alLineTypes[i]) == 2)                                               //2 = Mill section
                    {
                        Section2StateName = cbsItem.LineS2DB + "." + cbsItem.LineS2StateCode;
                        Section2StateFaultName = cbsItem.LineS2DB + ".DBD102";                              //Default R2 PCRead.Fault Double Word
                        Section2ParEmptyingName = cbsItem.LineS2DB + "." + cbsItem.LineS2parEmptying;
                        Section2OutEmptyingName = cbsItem.LineS2DB + "." + cbsItem.LineS2outEmptying;
                    }

                    //sTempCurrentDB2 = Section2StateName;

                    //______________________________________________________________________                //CREATE TAGS
                    //SECTION 1
                    Tag tLine = new Tag(LineStateName, S7Link.Tag.ATOMIC.WORD, 1);                          //Line and section
                    Tag tSect = new Tag(Section1StateName, S7Link.Tag.ATOMIC.WORD, 1);
                    Tag tSectFault = new Tag(Section1StateFaultName, S7Link.Tag.ATOMIC.DWORD, 1);
                    Tag tParEmpty_S1 = new Tag(Section1ParEmptyingName, S7Link.Tag.ATOMIC.WORD, 1);         //Create Emptying Time Tags
                    Tag tOutEmpty_S1 = new Tag(Section1OutEmptyingName, S7Link.Tag.ATOMIC.WORD, 1);
                    Tag tStaFeedOff = new Tag(cbsItem.LineS1DB + ".DBX109.2", S7Link.Tag.ATOMIC.BOOL, 1);

                    //SECTION 2                
                    //int iDbgtemp = Convert.ToInt32(alLineTypes[i]);
                    if (Convert.ToInt32(alLineTypes[i]) == 2)                                               //If this line is a mill line
                    {
                        tSect = new Tag(Section2StateName, S7Link.Tag.ATOMIC.WORD, 1);
                        tSectFault = new Tag(Section2StateFaultName, S7Link.Tag.ATOMIC.DWORD, 1);
                        tParEmpty_S1 = new Tag(Section2ParEmptyingName, S7Link.Tag.ATOMIC.WORD, 1);
                        tOutEmpty_S1 = new Tag(Section2OutEmptyingName, S7Link.Tag.ATOMIC.WORD, 1);
                        tStaFeedOff = new Tag(cbsItem.LineS2DB + ".DBX109.2", S7Link.Tag.ATOMIC.BOOL, 1);
                    }



                    //______________________________________________________________________//ADD MY OBJECTS
                    //tLine.MyObject = "LINE_" + (i + 1);                                   //Assign the Line Number to the Tag
                    //Used to identify which Word to look at when determining section color [State or Fault]
                    tSect.MyObject = (i + 1) + ")SECTION_STATE";
                    tSectFault.MyObject = (i + 1) + ")SECTION_FAULT";
                    //***CALCULATE SECTION NUMBER
                    tParEmpty_S1.MyObject = (i + 1) + ")PAR_EMPTY(1)";                      //(x) = Section number
                    tOutEmpty_S1.MyObject = (i + 1) + ")OUT_EMPTY(1)";
                    tStaFeedOff.MyObject = i + 1;


                    //______________________________________________________________________//EVENT HANDLERS
                    //tLine.Changed += new EventHandler(LineState_Changed);
                    tSect.Changed += new EventHandler(SectionState_Changed);
                    tSectFault.Changed += new EventHandler(SectionState_Changed);
                    tParEmpty_S1.Changed += new EventHandler(ParEmpty_Changed);
                    tOutEmpty_S1.Changed += new EventHandler(OutEmpty_Changed);
                    tStaFeedOff.Changed += new EventHandler(tStaFeedOff_Changed);


                    //______________________________________________________________________//ADD TAGGROUPS
                    tagroupSecStaFeedOff.AddTag(tStaFeedOff);
                    //tagroupLineStates.AddTag(tLine);
                    tagroupSecStates.AddTag(tSect);
                    tagroupSecStatesFAULT.AddTag(tSectFault);
                    tagroupSecParEmptying.AddTag(tParEmpty_S1);
                    tagroupSecOutEmptying.AddTag(tOutEmpty_S1);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Line States --> " + ex.Message + "\nLine Number --> " + sErrorLineNumber + "\nLineType --> " + sErrorLineType, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// Load the Senders and Recievers for each line
        /// </summary>
        public void InitSendersAndRecievers()
        {
            #region INTAKE 1

            //slJobList_INT1SenderDBs.Add("DB610.DBW816", "DB610.DBW816");

            slJobList_INT1ReceiverDBs.Add("DB610.DBW846", "DB610.DBW846");
            slJobList_INT1ReceiverDBs.Add("DB610.DBW854", "DB610.DBW854");
            slJobList_INT1ReceiverDBs.Add("DB610.DBW862", "DB610.DBW862");
            slJobList_INT1ReceiverDBs.Add("DB610.DBW870", "DB610.DBW870");
            slJobList_INT1ReceiverDBs.Add("DB610.DBW878", "DB610.DBW878");
            slJobList_INT1ReceiverDBs.Add("DB610.DBW886", "DB610.DBW886");
            //slJobList_INT1ReceiverDBs.Add("DB610.DBW924", "DB610.DBW924");

            //slJobList_INT1SenderBins.Add(100, "TRUCK INTAKE");
            slJobList_INT1ReceiverBins.Add(101, "SILO 1");
            slJobList_INT1ReceiverBins.Add(102, "SILO 2");
            slJobList_INT1ReceiverBins.Add(103, "SILO 3");
            slJobList_INT1ReceiverBins.Add(104, "SILO 4");
            slJobList_INT1ReceiverBins.Add(105, "BULK OUTLOAD");
            slJobList_INT1ReceiverBins.Add(203, "CLEANING BINS");
            //slJobList_INT1ReceiverBins.Add(500, "SCREENINGS BIN");

            #endregion

            #region FIRST CLEANING 1
            slJobList_FCL1SenderDBs.Add("DB630.DBW824", "DB630.DBW824");
            slJobList_FCL1SenderDBs.Add("DB630.DBW832", "DB630.DBW832");
            slJobList_FCL1SenderDBs.Add("DB630.DBW840", "DB630.DBW840");

            //slJobList_FCL1ReceiverDBs.Add("DB630.DBW862", "DB630.DBW862");
            //slJobList_FCL1ReceiverDBs.Add("DB630.DBW900", "DB630.DBW900");

            slJobList_FCL1SenderBins.Add(201, "BIN 201");
            slJobList_FCL1SenderBins.Add(202, "BIN 202");
            slJobList_FCL1SenderBins.Add(203, "BIN 203");

            //slJobList_FCL1ReceiverBins.Add(206, "3rd TEMP BIN");
            //slJobList_FCL1ReceiverBins.Add(500, "SCREENINGS BIN");

            #endregion

            #region WHEAT TRANSFER 1
            slJobList_MTR1SenderDBs.Add("DB620.DBW816", "DB620.DBW816");
            slJobList_MTR1SenderDBs.Add("DB620.DBW824", "DB620.DBW824");
            slJobList_MTR1SenderDBs.Add("DB620.DBW832", "DB620.DBW832");
            slJobList_MTR1SenderDBs.Add("DB620.DBW840", "DB620.DBW840");

            slJobList_MTR1ReceiverDBs.Add("DB620.DBW894", "DB620.DBW894");
            slJobList_MTR1ReceiverDBs.Add("DB620.DBW902", "DB620.DBW902");
            slJobList_MTR1ReceiverDBs.Add("DB620.DBW910", "DB620.DBW910");
            slJobList_MTR1ReceiverDBs.Add("DB620.DBW918", "DB620.DBW918");
            slJobList_MTR1ReceiverDBs.Add("DB620.DBW926", "DB620.DBW926");
            slJobList_MTR1ReceiverDBs.Add("DB620.DBW934", "DB620.DBW934");



            slJobList_MTR1SenderBins.Add(101, "SILO 1");
            slJobList_MTR1SenderBins.Add(102, "SILO 2");
            slJobList_MTR1SenderBins.Add(103, "SILO 3");
            slJobList_MTR1SenderBins.Add(104, "SILO 4");

            slJobList_MTR1ReceiverBins.Add(101, "SILO 1");
            slJobList_MTR1ReceiverBins.Add(102, "SILO 2");
            slJobList_MTR1ReceiverBins.Add(103, "SILO 3");
            slJobList_MTR1ReceiverBins.Add(104, "SILO 4");
            slJobList_MTR1ReceiverBins.Add(105, "BULK OUTLOAD");
            slJobList_MTR1ReceiverBins.Add(203, "CLEANING BINS");
          //  slJobList_MTR1ReceiverBins.Add(500, "SCREENINGS BIN");
            

            #endregion

//            #region SECOND CLEANING
//            slJobList_SCL1SenderDBs.Add("DB650.DBW816", "DB650.DBW816");
//            slJobList_SCL1SenderDBs.Add("DB650.DBW824", "DB650.DBW824");
//            slJobList_SCL1SenderDBs.Add("DB650.DBW832", "DB650.DBW832");

//            slJobList_SCL1ReceiverDBs.Add("DB650.DBW894", "DB650.DBW894");
//            slJobList_SCL1ReceiverDBs.Add("DB650.DBW902", "DB650.DBW902");
//            //slJobList_SCL1ReceiverDBs.Add("DB650.DBW932", "DB650.DBW932");


//            slJobList_SCL1SenderBins.Add(204, "1st TEMP BIN");
//            slJobList_SCL1SenderBins.Add(205, "2nd TEMP BIN");
//            slJobList_SCL1SenderBins.Add(206, "3rd TEMP BIN");

//            slJobList_SCL1ReceiverBins.Add(206, "3rd TEMP BIN");
//            slJobList_SCL1ReceiverBins.Add(207, "B1 Scale Hopper");
//            //slJobList_SCL1ReceiverBins.Add(500, "SCREENINGS BIN");
        

//#endregion

            
            //#region MIL1

            //slJoblist_MIL1SND35DBs.Add("DB800.DBW834", "DB800.DBW834");

            //slJobList_MIL1RCV36DBs.Add("DB800.DBW912", "DB800.DBW912");
            //slJobList_MIL1RCV36DBs.Add("DB800.DBW920", "DB800.DBW920");
            //slJobList_MIL1RCV36DBs.Add("DB800.DBW928", "DB800.DBW928");

            //slJobList_MIL1RCV37DBs.Add("DB800.DBW950", "DB800.DBW950");

            //slJobList_MIL1RCV40DBs.Add("DB800.DBW988", "DB800.DBW988");
            //slJobList_MIL1RCV40DBs.Add("DB800.DBW996", "DB800.DBW996");



            //slJoblist_MIL1SND35Bins.Add(207, "B1 Scale Hopper");

            //slJobList_MIL1RCV36Bins.Add(501, "FLOUR 1 BIN");
            //slJobList_MIL1RCV36Bins.Add(502, "FLOUR 2 BIN");
            //slJobList_MIL1RCV36Bins.Add(503, "FLOUR 3 BIN");

            //slJobList_MIL1RCV37Bins.Add(500, "SCREENINGS BIN");

            //slJobList_MIL1RCV40Bins.Add(601, "BRAN HOLDING BIN");
            //slJobList_MIL1RCV40Bins.Add(602, "BRAN PACKING BIN");


          

            //#endregion



            //#region FINAL PRODUCT HANDLING 1

            //slJobList_FPH1SenderDBs.Add("DB660.DBW816", "DB660.DBW816");
            //slJobList_FPH1SenderDBs.Add("DB660.DBW824", "DB660.DBW824");
            //slJobList_FPH1SenderDBs.Add("DB660.DBW832", "DB660.DBW832");

            //slJobList_FPH1ReceiverDBs.Add("DB660.DBW894", "DB660.DBW894");
            //slJobList_FPH1ReceiverDBs.Add("DB660.DBW902", "DB660.DBW902");
            //slJobList_FPH1ReceiverDBs.Add("DB660.DBW910", "DB660.DBW910");
            //slJobList_FPH1ReceiverDBs.Add("DB660.DBW918", "DB660.DBW918");
            //slJobList_FPH1ReceiverDBs.Add("DB660.DBW926", "DB660.DBW926");
            //slJobList_FPH1ReceiverDBs.Add("DB660.DBW934", "DB660.DBW934");
          
           

            //slJobList_FPH1SenderBins.Add(501, "FLOUR 1 BIN");
            //slJobList_FPH1SenderBins.Add(502, "FLOUR 2 BIN");
            //slJobList_FPH1SenderBins.Add(503, "FLOUR 3 BIN");

            //slJobList_FPH1ReceiverBins.Add(501, "FLOUR 1 BIN");
            //slJobList_FPH1ReceiverBins.Add(502, "FLOUR 2 BIN");
            //slJobList_FPH1ReceiverBins.Add(503, "FLOUR 3 BIN");
            //slJobList_FPH1ReceiverBins.Add(504, "3RD PACKER");
            //slJobList_FPH1ReceiverBins.Add(505, "2ND PACKER");
            //slJobList_FPH1ReceiverBins.Add(506, "1ST PACKER");

            //#endregion

           // #region Screenings Grinding

           //slJoblist_SCG1SenderDBs.Add("DB680.DBW816", "DB680.DBW816");

           // slJoblist_SCG1ReceiverDBs.Add("DB680.DBW894", "DB680.DBW894");
           // slJoblist_SCG1ReceiverDBs.Add("DB680.DBW902", "DB680.DBW902");

           // slJoblist_SCG1SenderBins.Add(500, "SCREENINGS BIN");

           // slJoblist_SCG1ReceiverBins.Add(601, "BRAN HOLDING BIN");
           // slJoblist_SCG1ReceiverBins.Add(602, "BRAN PACKING BIN");

           // #endregion

            //#region BRAN HANDLING
            //slJoblist_BRN1SenderDBs.Add("DB690.DBW816", "DB690.DBW816");

            //slJoblist_BRN1ReceiverDBs.Add("DB690.DBW894", "DB690.DBW894");
            //slJoblist_BRN1ReceiverDBs.Add("DB690.DBW902", "DB690.DBW902");

            //slJoblist_BRN1SenderBins.Add(601, "BRAN HOLDING BIN");

            //slJoblist_BRN1ReceiverBins.Add(601, "BRAN HOLDING BIN");
            //slJoblist_BRN1ReceiverBins.Add(602, "BRAN PACKING BIN");
            //#endregion
        }



        /// <summary>
        /// Set the visibility of controls based on user permission levels
        /// </summary>
        public void SetUserAccess()
        {
            //            bool bOperatorAccess = false;
            bool bSupervisorAccess = false;


            // 0     = Logged Out
            // 2     = Operator
            // 5     = Supervisor
            // 10    = Administrator

            if (stat_iUserLevel == 0)
            {
                bOperatorAccess = false;
                bSupervisorAccess = false;
                bAdministratorAccess = false;
            }
            else if (stat_iUserLevel == 2)
            {
                bOperatorAccess = true;
                bSupervisorAccess = false;
                bAdministratorAccess = false;
            }
            else if (stat_iUserLevel == 5)
            {
                bOperatorAccess = true;
                bSupervisorAccess = true;
                bAdministratorAccess = false;
            }
            else if (stat_iUserLevel == 10)
            {
                bOperatorAccess = true;
                bSupervisorAccess = true;
                bAdministratorAccess = true;
            }


            //
            //Control Buttons
            //
            btnControlAcknowledge.Visibility = bOperatorAccess == true ? Visibility.Visible : Visibility.Hidden;
            btnControlStart.Visibility = bOperatorAccess == true ? Visibility.Visible : Visibility.Hidden;
            btnControlSuspend.Visibility = bOperatorAccess == true ? Visibility.Visible : Visibility.Hidden;
            btnControlMuteSiren.Visibility = bOperatorAccess == true ? Visibility.Visible : Visibility.Hidden;
            btnControlSeqStop.Visibility = bOperatorAccess == true ? Visibility.Visible : Visibility.Hidden;
            btnControlEmergencyStop.Visibility = bOperatorAccess == true ? Visibility.Visible : Visibility.Hidden;
            



            ////
            ////CWA Scale Details Buttons
            ////
            //cwa5039ScaleDetails.btnReset.IsEnabled = bSupervisorAccess == true ? true : false


            //
            //Taglinks
            //
            btnErrors.Visibility = bAdministratorAccess == true ? Visibility.Visible : Visibility.Hidden;
            btnTaglinks.Visibility = bAdministratorAccess == true ? Visibility.Visible : Visibility.Hidden;

            //
            //Switches and Control Desks
            //
            MaintenanceSwitches.Visibility = bOperatorAccess == true ? Visibility.Visible : Visibility.Hidden;
            PlantSwitches.Visibility = bOperatorAccess == true ? Visibility.Visible : Visibility.Hidden;
          
            //
            //Settings Page
            //

            btnSettings.Visibility = bSupervisorAccess == true ? Visibility.Visible : Visibility.Hidden;
            btnRestart.Visibility = bSupervisorAccess == true ? Visibility.Visible : Visibility.Hidden;

            //
            //Parameter Settings Page
            //
            // btnParameterSettings.Visibility = bSupervisorAccess == true ? Visibility.Visible : Visibility.Hidden;
            //Profibus Network Page
            //
            btnProfibusNetwork.Visibility = bOperatorAccess == true ? Visibility.Visible : Visibility.Hidden;
            
            //
            //Debug Settings
            //
            btnStart.Visibility = bAdministratorAccess == true ? Visibility.Visible : Visibility.Hidden;
            btnStop.Visibility = bAdministratorAccess == true ? Visibility.Visible : Visibility.Hidden;
            btnExit.Visibility = bAdministratorAccess == true ? Visibility.Visible : Visibility.Hidden;
            lblLastReadTime.Visibility = bAdministratorAccess == true ? Visibility.Visible : Visibility.Hidden;
            lblBadTags.Visibility = bAdministratorAccess == true ? Visibility.Visible : Visibility.Hidden;
        
            //
            //Administrative Tools (On Settings Page)
            //
            pageSettings.gbConfiguration.Visibility = bAdministratorAccess == true ? Visibility.Visible : Visibility.Hidden;
            pageSettings.gbAdminTools.Visibility = bAdministratorAccess == true ? Visibility.Visible : Visibility.Hidden;
            pageSettings.gbUserEvents.Visibility = bAdministratorAccess == true ? Visibility.Visible : Visibility.Hidden;

            //
            //Zoom Controls
            //            
            btnEnableZoom.Visibility = bAdministratorAccess == true ? Visibility.Visible : Visibility.Hidden;
            btnDisableZoom.Visibility = bAdministratorAccess == true ? Visibility.Visible : Visibility.Hidden;
            btnSaveZoom.Visibility = bAdministratorAccess == true ? Visibility.Visible : Visibility.Hidden;

            //btnTaglinks.Visibility = bAdministratorAccess == true ? Visibility.Visible : Visibility.Hidden;
            //btnErrors.Visibility = bAdministratorAccess == true ? Visibility.Visible : Visibility.Hidden;

            //
            //Speed Control Visibility
            //


        }



        //------------------------------------------------------------------------------//
        //                          Update Element Methods                              //
        //------------------------------------------------------------------------------//  

        private void UpdateElementColor(Tag t)
        {
            MyObjectInfo moi = (MyObjectInfo)t.MyObject;

            switch (moi.TagName)
            {
                //=====================
                //Maintenance Switches
                //=====================

                case "G_ES_GENERAL":
                    MaintenanceSwitches.SetColor1 = Convert.ToInt32(t.Value);
                    break;

                case "=A-0011-SHE01":
                    MaintenanceSwitches.SetColor2 = Convert.ToInt32(t.Value);
                    break;

                case "=A-0013-SHE01":
                    MaintenanceSwitches.SetColor3 = Convert.ToInt32(t.Value);
                    break;

                case "=A-0014-SHE01":
                    MaintenanceSwitches.SetColor4 = Convert.ToInt32(t.Value);
                    break;

                //case "175S2":
                //    MaintenanceSwitches.SetColor5 = Convert.ToInt32(t.Value);
                //    break;

                //case "175S3":
                //    MaintenanceSwitches.SetColor6 = Convert.ToInt32(t.Value);
                //    break;

                //=====================
                //Plant Switches
                //=====================

                case "G_FUSE_SWITCHED":
                    if (Convert.ToInt32(t.Value) == 32)
                    {
                        PlantSwitches.SetColor1 = Convert.ToInt32(t.Value);
                        LogItem li = new LogItem(DateTime.Now, DateTime.Now.ToOADate(), moi.TagName, moi.TagDescription + "   " + "(" + t.Value + ")", 20);
                        alLoggerToSQL.Add(li);
                        alLoggerToUI.Add(li);

                        stat_sFault = "+C110 FUSE SWITCH INTERRUPTED!!!";

                        bFault = true;
                    }
                    else
                    {
                        PlantSwitches.SetColor1 = Convert.ToInt32(t.Value);
                    }
                    break;

                case "G_FUSE_NSWITCHED":
                    if (Convert.ToInt32(t.Value) == 32)
                    {
                        PlantSwitches.SetColor2 = Convert.ToInt32(t.Value);
                        LogItem li = new LogItem(DateTime.Now, DateTime.Now.ToOADate(), moi.TagName, moi.TagDescription + "   " + "(" + t.Value + ")", 20);
                        alLoggerToSQL.Add(li);
                        alLoggerToUI.Add(li);

                        stat_sFault = "+C110 FUSE SWITCH NOT INTERRUPTED!!!";

                        bFault = true;
                    }
                    else
                    {
                        PlantSwitches.SetColor2 = Convert.ToInt32(t.Value);
                    }
                    break;

                case "+C110:OMF":
                    if (Convert.ToInt32(t.Value) == 32)
                    {
                        PlantSwitches.SetColor3 = Convert.ToInt32(t.Value);
                        LogItem li = new LogItem(DateTime.Now, DateTime.Now.ToOADate(), moi.TagName, moi.TagDescription + "   " + "(" + t.Value + ")", 20);
                        alLoggerToSQL.Add(li);
                        alLoggerToUI.Add(li);

                        stat_sFault = "+C110 OVERVOLTAGE MONITOR FAILURE!!!";

                        bFault = true;
                    }
                    else
                    {
                        PlantSwitches.SetColor3 = Convert.ToInt32(t.Value);
                    }
                    break;

                case "+C120:OMF":
                    if (Convert.ToInt32(t.Value) == 32)
                    {
                        PlantSwitches.SetColor4 = Convert.ToInt32(t.Value);
                        LogItem li = new LogItem(DateTime.Now, DateTime.Now.ToOADate(), moi.TagName, moi.TagDescription + "   " + "(" + t.Value + ")", 20);
                        alLoggerToSQL.Add(li);
                        alLoggerToUI.Add(li);

                        stat_sFault = "+C120 OVERVOLTAGE MONITOR FAILURE!!!";

                        bFault = true;
                    }
                    else
                    {
                        PlantSwitches.SetColor4 = Convert.ToInt32(t.Value);
                    }
                    break;

                case "+C110":
                    if (Convert.ToInt32(t.Value) == 32)
                    {
                        PlantSwitches.SetColor5 = Convert.ToInt32(t.Value);
                        LogItem li = new LogItem(DateTime.Now, DateTime.Now.ToOADate(), moi.TagName, moi.TagDescription + "   " + "(" + t.Value + ")", 20);
                        alLoggerToSQL.Add(li);
                        alLoggerToUI.Add(li);

                        stat_sFault = "+C110 POWER COMSUMPTION MEASUREMENT!!!";

                        bFault = true;
                    }
                    else
                    {
                        PlantSwitches.SetColor5 = Convert.ToInt32(t.Value);
                    }
                    break;

                case "+C120":
                    if (Convert.ToInt32(t.Value) == 32)
                    {
                        PlantSwitches.SetColor6 = Convert.ToInt32(t.Value);
                        LogItem li = new LogItem(DateTime.Now, DateTime.Now.ToOADate(), moi.TagName, moi.TagDescription + "   " + "(" + t.Value + ")", 20);
                        alLoggerToSQL.Add(li);
                        alLoggerToUI.Add(li);

                        stat_sFault = "+C120 POWER COMSUMPTION MEASUREMENT!!!";

                        bFault = true;
                    }
                    else
                    {
                        PlantSwitches.SetColor6 = Convert.ToInt32(t.Value);
                    }
                    break;

                ////=====================
                ////Bag Packer Bin 411
                ////=====================

                //case "=A-5074-KCL10SA":
                //    BagPacker411.SetColor1 = Convert.ToInt32(t.Value);
                //    break;

                //case "=A-5074-KCL10RBP":
                //    BagPacker411.SetColor2 = Convert.ToInt32(t.Value);
                //    break;

                //case "=A-5074-KCL10RBC":
                //    BagPacker411.SetColor3 = Convert.ToInt32(t.Value);
                //    break;

                //case "=A-5074-KCL10PO":
                //    BagPacker411.SetColor4 = Convert.ToInt32(t.Value);

                //    //If the signal is on, colour all other packing machines green, else gray
                //    if (Convert.ToInt32(t.Value) == 3)
                //    {
                //        pageBPL2._5069.MotorColor = 3;
                //        pageBPL2._5070_1.MotorColor = 3;
                //        pageBPL2._5070_2.MotorColor = 3;
                //        pageBPL2._5075.ScaleColor = 3;
                //        pageBPL2._5071.BaggingStationColor = 3;
                //        pageBPL2._5072.MotorColor = 3;
                //    }
                //    else
                //    {
                //        pageBPL2._5069.MotorColor = 1;
                //        pageBPL2._5070_1.MotorColor = 1;
                //        pageBPL2._5070_2.MotorColor = 1;
                //        pageBPL2._5075.ScaleColor = 1;
                //        pageBPL2._5071.BaggingStationColor = 1;
                //        pageBPL2._5072.MotorColor = 1;
                //    }

                //    break;

                //case "=A-5074-KCL10SE":
                //    BagPacker411.SetColor5 = Convert.ToInt32(t.Value);
                //    break;

                //case "=A-5074-KCL10A":
                //    pageBPL2._5074A.MonitorColor = Convert.ToInt32(t.Value);
                //    string sStatus2 = pageBPL2._5074A.Status_Alarm;
                //    pageBPL2._5074A.Description_Alarm = moi.TagName + " " + moi.TagDescription;
                //    string sFaultString2 = moi.TagDescription + " (" + sStatus2 + ")";
                //    bool bIsInFault2 = pageBPL2._5074A.Fault_Alarm;
                //    RegisterEvent(bIsInFault2, sFaultString2, moi.TagName, Convert.ToInt32(t.Value), !bFirstTagRead);
                //    break;

            }
        }


        public void UpdateAdditionalElementColor(Tag t)
        {

            MyObjectInfo moi = (MyObjectInfo)t.MyObject;

            string tagname = moi.TagName;
            //string sStatusDescription = "";
            //bool bLogThisTag = true;

            int iTrend = moi.RecTrend;
            int iRecOnChange = moi.RecOnChange;

            if (t.QualityCode == 0)
            {
                MessageBox.Show("BAD " + t.Name);
            }
            else if (t.QualityCode == 64)
            {
                MessageBox.Show("UNCERTAIN " + t.Name);
            }

            switch (tagname)
            {
                case "_Alarm_Horn":

                    //int alarmState = Convert.ToInt32(t.Value);
                    bool alarmState = Convert.ToBoolean(t.Value);
                    if (alarmState)
                    {
                        timerAlarmHorn.Start();
                        AlarmHorn1.AlarmHorn_Visibility = Visibility.Visible;
                    }
                    else
                    {
                        timerAlarmHorn.Stop();
                        AlarmHorn1.AlarmHornColor = 0;
                        AlarmHorn1.AlarmHorn_Visibility = Visibility.Hidden;
                    }
                    break;

                //==================
                //Plant Information
                //==================

                case "_Profibus_Fault":
                    PlantSwitches.SetColor6 = Convert.ToInt32(t.Value);
                    break;

                //=======================
                //Profibus Network Nodes
                //======================

                //case "_PBDP_6016TSC01":
                //    bool Status6016 = Convert.ToBoolean(t.Value);
                //    pageProfibusNetwork.VSD_6016.SetColor1 = Convert.ToInt32(t.Value);
                //    if (Status6016)
                //    {
                //        pageProfibusNetwork.VSD_6016.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //        {
                //            pageProfibusNetwork.VSD_6016.TxtOnline.Visibility = Visibility.Hidden;
                //            pageProfibusNetwork.VSD_6016.TxtOffline.Visibility = Visibility.Visible;
                //            pageProfibusNetwork.VSD_6016.ImgALTIVAR.Visibility = Visibility.Visible;
                //            pageProfibusNetwork.VSD_6016.ImgWAGO.Visibility = Visibility.Hidden;
                //            pageProfibusNetwork.VSD_6016.ImgMEAG.Visibility = Visibility.Hidden;
                //        }));
                //    }
                //    else
                //    {
                //        pageProfibusNetwork.VSD_6016.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //        {
                //            pageProfibusNetwork.VSD_6016.TxtOnline.Visibility = Visibility.Visible;
                //            pageProfibusNetwork.VSD_6016.TxtOffline.Visibility = Visibility.Hidden;
                //            pageProfibusNetwork.VSD_6016.ImgALTIVAR.Visibility = Visibility.Visible;
                //            pageProfibusNetwork.VSD_6016.ImgWAGO.Visibility = Visibility.Hidden;
                //            pageProfibusNetwork.VSD_6016.ImgMEAG.Visibility = Visibility.Hidden;
                //        }));
                //    }
                //    break;

                //case "_PBDP_5019TSC01":
                //    bool Status5019 = Convert.ToBoolean(t.Value);
                //    pageProfibusNetwork.VSD_5019.SetColor1 = Convert.ToInt32(t.Value);
                //    if (Status5019)
                //    {
                //        pageProfibusNetwork.VSD_5019.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //        {
                //            pageProfibusNetwork.VSD_5019.TxtOnline.Visibility = Visibility.Hidden;
                //            pageProfibusNetwork.VSD_5019.TxtOffline.Visibility = Visibility.Visible;
                //            pageProfibusNetwork.VSD_5019.ImgALTIVAR.Visibility = Visibility.Visible;
                //            pageProfibusNetwork.VSD_5019.ImgWAGO.Visibility = Visibility.Hidden;
                //            pageProfibusNetwork.VSD_5019.ImgMEAG.Visibility = Visibility.Hidden;
                //        }));
                //    }
                //    else
                //    {
                //        pageProfibusNetwork.VSD_5019.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //        {
                //            pageProfibusNetwork.VSD_5019.TxtOnline.Visibility = Visibility.Visible;
                //            pageProfibusNetwork.VSD_5019.TxtOffline.Visibility = Visibility.Hidden;
                //            pageProfibusNetwork.VSD_5019.ImgALTIVAR.Visibility = Visibility.Visible;
                //            pageProfibusNetwork.VSD_5019.ImgWAGO.Visibility = Visibility.Hidden;
                //            pageProfibusNetwork.VSD_5019.ImgMEAG.Visibility = Visibility.Hidden;
                //        }));
                //    }
                //    break;

                case "_PBDP_C110A":
                    bool StatusC110A = Convert.ToBoolean(t.Value);
                    pageProfibusNetwork.PANEL_C110A.SetColor1 = Convert.ToInt32(t.Value);
                    if (StatusC110A)
                    {
                        pageProfibusNetwork.PANEL_C110A.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.PANEL_C110A.TxtOnline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_C110A.TxtOffline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_C110A.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_C110A.ImgWAGO.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_C110A.ImgMEAG.Visibility = Visibility.Hidden;
                        }));
                    }
                    else
                    {
                        pageProfibusNetwork.PANEL_C110A.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.PANEL_C110A.TxtOnline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_C110A.TxtOffline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_C110A.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_C110A.ImgWAGO.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_C110A.ImgMEAG.Visibility = Visibility.Hidden;
                        }));
                    }
                    break;

                case "_PBDP_C110B":
                    bool StatusC110B = Convert.ToBoolean(t.Value);
                    pageProfibusNetwork.PANEL_C110B.SetColor1 = Convert.ToInt32(t.Value);
                    if (StatusC110B)
                    {
                        pageProfibusNetwork.PANEL_C110B.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.PANEL_C110B.TxtOnline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_C110B.TxtOffline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_C110B.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_C110B.ImgWAGO.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_C110B.ImgMEAG.Visibility = Visibility.Hidden;
                        }));
                    }
                    else
                    {
                        pageProfibusNetwork.PANEL_C110B.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.PANEL_C110B.TxtOnline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_C110B.TxtOffline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_C110B.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_C110B.ImgWAGO.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_C110B.ImgMEAG.Visibility = Visibility.Hidden;
                        }));
                    }
                    break;

                case "_PBDP_C120A":
                    bool StatusC120A = Convert.ToBoolean(t.Value);
                    pageProfibusNetwork.PANEL_C120A.SetColor1 = Convert.ToInt32(t.Value);
                    if (StatusC120A)
                    {
                        pageProfibusNetwork.PANEL_C120A.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.PANEL_C120A.TxtOnline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_C120A.TxtOffline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_C120A.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_C120A.ImgWAGO.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_C120A.ImgMEAG.Visibility = Visibility.Hidden;
                        }));
                    }
                    else
                    {
                        pageProfibusNetwork.PANEL_C120A.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.PANEL_C120A.TxtOnline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_C120A.TxtOffline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_C120A.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_C120A.ImgWAGO.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_C120A.ImgMEAG.Visibility = Visibility.Hidden;
                        }));
                    }
                    break;

                case "_PBDP_C120B":
                    bool StatusC120B = Convert.ToBoolean(t.Value);
                    pageProfibusNetwork.PANEL_C120B.SetColor1 = Convert.ToInt32(t.Value);
                    if (StatusC120B)
                    {
                        pageProfibusNetwork.PANEL_C120B.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.PANEL_C120B.TxtOnline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_C120B.TxtOffline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_C120B.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_C120B.ImgWAGO.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_C120B.ImgMEAG.Visibility = Visibility.Hidden;
                        }));
                    }
                    else
                    {
                        pageProfibusNetwork.PANEL_C120B.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.PANEL_C120B.TxtOnline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_C120B.TxtOffline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_C120B.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_C120B.ImgWAGO.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_C120B.ImgMEAG.Visibility = Visibility.Hidden;
                        }));
                    }
                    break;

                case "_PBDP_F130":
                    bool StatusF130 = Convert.ToBoolean(t.Value);
                    pageProfibusNetwork.PANEL_F130.SetColor1 = Convert.ToInt32(t.Value);
                    if (StatusF130)
                    {
                        pageProfibusNetwork.PANEL_F130.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.PANEL_F130.TxtOnline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_F130.TxtOffline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_F130.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_F130.ImgWAGO.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_F130.ImgMEAG.Visibility = Visibility.Hidden;
                        }));
                    }
                    else
                    {
                        pageProfibusNetwork.PANEL_F130.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.PANEL_F130.TxtOnline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_F130.TxtOffline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_F130.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_F130.ImgWAGO.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_F130.ImgMEAG.Visibility = Visibility.Hidden;
                        }));
                    }
                    break;

                case "_PBDP_C140":
                    bool StatusC140 = Convert.ToBoolean(t.Value);
                    pageProfibusNetwork.PANEL_C140.SetColor1 = Convert.ToInt32(t.Value);
                    if (StatusC140)
                    {
                        pageProfibusNetwork.PANEL_C140.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.PANEL_C140.TxtOnline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_C140.TxtOffline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_C140.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_C140.ImgWAGO.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_C140.ImgMEAG.Visibility = Visibility.Hidden;
                        }));
                    }
                    else
                    {
                        pageProfibusNetwork.PANEL_C140.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.PANEL_C140.TxtOnline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_C140.TxtOffline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_C140.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.PANEL_C140.ImgWAGO.Visibility = Visibility.Visible;
                            pageProfibusNetwork.PANEL_C140.ImgMEAG.Visibility = Visibility.Hidden;
                        }));
                    }
                    break;

                case "_PBDP_1011":
                    bool Status1011 = Convert.ToBoolean(t.Value);
                    pageProfibusNetwork.MEAG_1011.SetColor1 = Convert.ToInt32(t.Value);
                    if (Status1011)
                    {
                        pageProfibusNetwork.MEAG_1011.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_1011.TxtOnline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_1011.TxtOffline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_1011.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_1011.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_1011.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    else
                    {
                        pageProfibusNetwork.MEAG_1011.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_1011.TxtOnline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_1011.TxtOffline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_1011.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_1011.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_1011.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    break;

                case "_PBDP_1012":
                    bool Status1012 = Convert.ToBoolean(t.Value);
                    pageProfibusNetwork.MEAG_1012.SetColor1 = Convert.ToInt32(t.Value);
                    if (Status1012)
                    {
                        pageProfibusNetwork.MEAG_1012.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_1012.TxtOnline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_1012.TxtOffline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_1012.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_1012.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_1012.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    else
                    {
                        pageProfibusNetwork.MEAG_1012.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_1012.TxtOnline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_1012.TxtOffline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_1012.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_1012.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_1012.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    break;

                case "_PBDP_2007":
                    bool Status2007 = Convert.ToBoolean(t.Value);
                    pageProfibusNetwork.MEAG_2007.SetColor1 = Convert.ToInt32(t.Value);
                    if (Status2007)
                    {
                        pageProfibusNetwork.MEAG_2007.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_2007.TxtOnline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2007.TxtOffline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_2007.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2007.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2007.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    else
                    {
                        pageProfibusNetwork.MEAG_2007.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_2007.TxtOnline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_2007.TxtOffline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2007.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2007.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2007.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    break;

                case "_PBDP_2026":
                    bool Status2026 = Convert.ToBoolean(t.Value);
                    pageProfibusNetwork.MEAG_2026.SetColor1 = Convert.ToInt32(t.Value);
                    if (Status2026)
                    {
                        pageProfibusNetwork.MEAG_2026.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_2026.TxtOnline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2026.TxtOffline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_2026.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2026.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2026.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    else
                    {
                        pageProfibusNetwork.MEAG_2026.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_2026.TxtOnline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_2026.TxtOffline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2026.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2026.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2026.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    break;

                case "_PBDP_2027":
                    bool Status2027 = Convert.ToBoolean(t.Value);
                    pageProfibusNetwork.MEAG_2027.SetColor1 = Convert.ToInt32(t.Value);
                    if (Status2027)
                    {
                        pageProfibusNetwork.MEAG_2027.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_2027.TxtOnline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2027.TxtOffline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_2027.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2027.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2027.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    else
                    {
                        pageProfibusNetwork.MEAG_2027.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_2027.TxtOnline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_2027.TxtOffline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2027.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2027.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2027.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    break;

                case "_PBDP_2028":
                    bool Status2028 = Convert.ToBoolean(t.Value);
                    pageProfibusNetwork.MEAG_2028.SetColor1 = Convert.ToInt32(t.Value);
                    if (Status2028)
                    {
                        pageProfibusNetwork.MEAG_2028.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_2028.TxtOnline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2028.TxtOffline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_2028.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2028.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2028.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    else
                    {
                        pageProfibusNetwork.MEAG_2028.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_2028.TxtOnline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_2028.TxtOffline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2028.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2028.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2028.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    break;

                case "_PBDP_2039":
                    bool Status2039 = Convert.ToBoolean(t.Value);
                    pageProfibusNetwork.MEAG_2039.SetColor1 = Convert.ToInt32(t.Value);
                    if (Status2039)
                    {
                        pageProfibusNetwork.MEAG_2039.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_2039.TxtOnline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2039.TxtOffline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_2039.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2039.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2039.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    else
                    {
                        pageProfibusNetwork.MEAG_2039.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_2039.TxtOnline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_2039.TxtOffline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2039.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2039.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2039.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    break;

                case "_PBDP_2053":
                    bool Status2053 = Convert.ToBoolean(t.Value);
                    pageProfibusNetwork.MEAG_2053.SetColor1 = Convert.ToInt32(t.Value);
                    if (Status2053)
                    {
                        pageProfibusNetwork.MEAG_2053.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_2053.TxtOnline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2053.TxtOffline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_2053.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2053.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2053.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    else
                    {
                        pageProfibusNetwork.MEAG_2053.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_2053.TxtOnline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_2053.TxtOffline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2053.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2053.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2053.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    break;

                case "_PBDP_2054":
                    bool Status2054 = Convert.ToBoolean(t.Value);
                    pageProfibusNetwork.MEAG_2054.SetColor1 = Convert.ToInt32(t.Value);
                    if (Status2054)
                    {
                        pageProfibusNetwork.MEAG_2054.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_2054.TxtOnline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2054.TxtOffline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_2054.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2054.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2054.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    else
                    {
                        pageProfibusNetwork.MEAG_2054.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_2054.TxtOnline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_2054.TxtOffline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2054.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2054.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2054.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    break;

                case "_PBDP_2055":
                    bool Status2055 = Convert.ToBoolean(t.Value);
                    pageProfibusNetwork.MEAG_2055.SetColor1 = Convert.ToInt32(t.Value);
                    if (Status2055)
                    {
                        pageProfibusNetwork.MEAG_2055.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_2055.TxtOnline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2055.TxtOffline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_2055.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2055.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2055.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    else
                    {
                        pageProfibusNetwork.MEAG_2055.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_2055.TxtOnline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_2055.TxtOffline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2055.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2055.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_2055.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    break;

                case "_PBDP_4002":
                    bool Status4002 = Convert.ToBoolean(t.Value);
                    pageProfibusNetwork.MEAG_4002.SetColor1 = Convert.ToInt32(t.Value);
                    if (Status4002)
                    {
                        pageProfibusNetwork.MEAG_4002.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_4002.TxtOnline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_4002.TxtOffline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_4002.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_4002.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_4002.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    else
                    {
                        pageProfibusNetwork.MEAG_4002.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_4002.TxtOnline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_4002.TxtOffline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_4002.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_4002.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_4002.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    break;

                case "_PBDP_4404":
                    bool Status4404 = Convert.ToBoolean(t.Value);
                    pageProfibusNetwork.MEAG_4404.SetColor1 = Convert.ToInt32(t.Value);
                    if (Status4404)
                    {
                        pageProfibusNetwork.MEAG_4404.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_4404.TxtOnline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_4404.TxtOffline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_4404.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_4404.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_4404.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    else
                    {
                        pageProfibusNetwork.MEAG_4404.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_4404.TxtOnline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_4404.TxtOffline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_4404.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_4404.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_4404.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    break;

                case "_PBDP_6023":
                    bool Status6023 = Convert.ToBoolean(t.Value);
                    pageProfibusNetwork.MEAG_6023.SetColor1 = Convert.ToInt32(t.Value);
                    if (Status6023)
                    {
                        pageProfibusNetwork.MEAG_6023.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_6023.TxtOnline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_6023.TxtOffline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_6023.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_6023.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_6023.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    else
                    {
                        pageProfibusNetwork.MEAG_6023.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageProfibusNetwork.MEAG_6023.TxtOnline.Visibility = Visibility.Visible;
                            pageProfibusNetwork.MEAG_6023.TxtOffline.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_6023.ImgALTIVAR.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_6023.ImgWAGO.Visibility = Visibility.Hidden;
                            pageProfibusNetwork.MEAG_6023.ImgMEAG.Visibility = Visibility.Visible;
                        }));
                    }
                    break;

               
                //============================
                //MOISTURE CONTROLLERS (MYFCs)
                //============================

                //case "_2027FCL1_NEWater":
                //    pageFCL1.MYFCInfo_2027.lblNEWater.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageFCL1.MYFCInfo_2027.lblNEWater.Content = t.Value.ToString();
                //    }));
                //    break;

                //case "_2027FCL1_NEProduct":
                //    pageFCL1.MYFCInfo_2027.lblNEProduct.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 1000), 2);
                //        pageFCL1.MYFCInfo_2027.lblNEProduct.Content = currentTonnage.ToString();
                //    }));
                //    break;

                //case "_2027FCL1_Out_Water_Flowrate":
                //    pageFCL1.MYFCInfo_2027.lblWaterFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageFCL1.MYFCInfo_2027.lblWaterFlowrate.Content = Math.Round((Convert.ToDecimal(t.Value) / 10), 2);
                //        //pageFCL1.MYFCInfo_2027.lblWaterFlowrate.Content = t.Value.ToString();
                //    }));
                //    break;

                //case "_2027FCL1_Out_Product_Flowrate":
                //    pageFCL1.MYFCInfo_2027.lblProductFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 1000), 2);
                //        pageFCL1.MYFCInfo_2027.lblProductFlowrate.Content = currentTonnage.ToString();
                //    }));
                //    break;

                //case "_2027FCL1_Out_Moisture":
                //    pageFCL1.MYFCInfo_2027.lblActualMoisture.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageFCL1.MYFCInfo_2027.lblActualMoisture.Content = Math.Round((Convert.ToDecimal(t.Value) / 10), 2);
                //        //pageFCL1.MYFCInfo_2027.lblActualMoisture.Content = t.Value.ToString();
                //    }));
                //    break;



                //case "_2027FCL1_ErasableProduct":
                //    pageFCL1.MYFCInfo_2027.lblErasableProduct.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 1000), 2);
                //        pageFCL1.MYFCInfo_2027.lblErasableProduct.Content = currentTonnage.ToString();
                //    }));
                //    break;

                //case "_2027FCL1_AlarmNo_MOZF":
                //    pageFCL1.MYFCInfo_2027.lblAlarmMOZF.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageFCL1.MYFCInfo_2027.lblAlarmMOZF.Content = t.Value.ToString();
                //    }));
                //    break;

                //case "_2027FCL1_AlarmNo_MYFC":
                //    pageFCL1.MYFCInfo_2027.lblAlarmMYFC.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageFCL1.MYFCInfo_2027.lblAlarmMYFC.Content = t.Value.ToString();
                //    }));
                //    break;

                //case "_2027FCL1_ErasableWater":
                //    pageFCL1.MYFCInfo_2027.lblErasableWater.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageFCL1.MYFCInfo_2027.lblErasableWater.Content = t.Value.ToString();
                //    }));
                //    break;

                //------------------------------------------------------------------------------------------------------------------------------

                

                ////==============
                ////Profibus MOZF
                ////==============
                //case "_2039_InMoisture":
                //    pageSCL1.MYFCInfo_A2039.lblTargetMoisture.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageSCL1.MYFCInfo_A2039.lblTargetMoisture.Content = (Math.Round(Convert.ToDouble(t.Value) / 10, 3)).ToString();
                //        pageFCL1.MYFCInfo_A2039.lblTargetMoisture.Content = (Math.Round(Convert.ToDouble(t.Value) / 10, 3)).ToString();
                //    }));
                //    break;

                //case "_2039_InOffsetMoisture":
                //    pageSCL1.MYFCInfo_A2039.lblOffsetMoisture.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageSCL1.MYFCInfo_A2039.lblOffsetMoisture.Content = (Math.Round(Convert.ToDouble(t.Value) / 100, 3)).ToString();
                //        pageFCL1.MYFCInfo_A2039.lblOffsetMoisture.Content = (Math.Round(Convert.ToDouble(t.Value) / 100, 3)).ToString();
                //    }));
                //    break;

                //case "_2039_OutMoisture":
                //    pageSCL1.MYFCInfo_A2039.lblActualMoisture.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageSCL1.MYFCInfo_A2039.lblActualMoisture.Content = (Math.Round(Convert.ToDouble(t.Value) / 10, 2)).ToString();
                //        pageFCL1.MYFCInfo_A2039.lblActualMoisture.Content = (Math.Round(Convert.ToDouble(t.Value) / 10, 2)).ToString();
                //    }));
                //    break;

                //case "_2039_OutFlowrateProduct":
                //    pageSCL1.MYFCInfo_A2039.lblProductFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 1000), 2);
                //        pageSCL1.MYFCInfo_A2039.lblProductFlowrate.Content = currentTonnage.ToString();
                //        pageFCL1.MYFCInfo_A2039.lblProductFlowrate.Content = currentTonnage.ToString();
                //    }));
                //    break;

                //case "_2039_OutFlowrateWater":
                //    pageSCL1.MYFCInfo_A2039.lblWaterFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 10), 2);
                //        pageSCL1.MYFCInfo_A2039.lblWaterFlowrate.Content = currentTonnage.ToString();
                //        pageFCL1.MYFCInfo_A2039.lblWaterFlowrate.Content = currentTonnage.ToString();
                //    }));
                //    break;

                //case "_2039_OutAlarmNoMYFC":
                //    pageSCL1.MYFCInfo_A2039.lblAlarmMYFC.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageSCL1.MYFCInfo_A2039.lblAlarmMYFC.Content = t.Value.ToString();
                //        pageFCL1.MYFCInfo_A2039.lblAlarmMYFC.Content = t.Value.ToString();
                //    }));
                //    break;

                //case "_2039_OutAlarmNoMOZF":
                //    pageSCL1.MYFCInfo_A2039.lblAlarmMOZF.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageSCL1.MYFCInfo_A2039.lblAlarmMOZF.Content = t.Value.ToString();
                //        pageFCL1.MYFCInfo_A2039.lblAlarmMOZF.Content = t.Value.ToString();
                //    }));
                //    break;

                //case "_2039_OutTotalProduct":
                //    pageSCL1.MYFCInfo_A2039.lblErasableProduct.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 1000), 2);
                //        pageSCL1.MYFCInfo_A2039.lblErasableProduct.Content = currentTonnage.ToString();
                //        pageFCL1.MYFCInfo_A2039.lblErasableProduct.Content = currentTonnage.ToString();
                //    }));
                //    break;

                //case "_2039_OutTotalWater":
                //    pageSCL1.MYFCInfo_A2039.lblErasableWater.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 1000), 2);
                //        pageSCL1.MYFCInfo_A2039.lblErasableWater.Content = currentTonnage.ToString();
                //        pageFCL1.MYFCInfo_A2039.lblErasableWater.Content = currentTonnage.ToString();
                //    }));
                //    break;

                //case "_2039_OutNotErasableWater":
                //    pageSCL1.MYFCInfo_A2039.lblNEWater.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 1000), 2);
                //        pageSCL1.MYFCInfo_A2039.lblNEWater.Content = currentTonnage.ToString();
                //        pageFCL1.MYFCInfo_A2039.lblNEWater.Content = currentTonnage.ToString();
                //    }));
                //    break;

                //case "_2039_OutNotErasableProduct":
                //    pageSCL1.MYFCInfo_A2039.lblNEProduct.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 1000), 2);
                //        pageSCL1.MYFCInfo_A2039.lblNEProduct.Content = currentTonnage.ToString();
                //        pageFCL1.MYFCInfo_A2039.lblNEProduct.Content = currentTonnage.ToString();
                //    }));
                    break;

                //=========
                //Sifters
                //=========

                case "_A0340_OutTransitionTime":
                    Tag tA0340 = null;
                    ArrayList alTagsA0340 = tagroupAdditionalSmartTags.Tags;

                    foreach (Tag ta in alTagsA0340)
                    {
                        if (((MyObjectInfo)ta.MyObject).TagName == "_A0340_ParStoppingTime")              //Get the Stopping time tag in the collection and assign to T1
                        {
                            tA0340 = ta;
                            break;
                        }
                    }

                    int iStoppingTimeA0340 = Convert.ToInt32(tA0340.Value) / 10;
                    int countDownA0340 = (Convert.ToInt32(t.Value) / 10);
                    if (pageMIL1._A0340.Status_Sifter == "StStopping" && countDownA0340 > 0)
                    {
                        pageMIL1._A0340StoppingTimer.Dispatcher.BeginInvoke(
                        System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(
                        delegate()
                        {
                            pageMIL1._A0340StoppingTimer.Visibility = System.Windows.Visibility.Visible;
                            pageMIL1._A0340StoppingTimer.progressBarStopping.Maximum = Convert.ToInt32(iStoppingTimeA0340);
                            pageMIL1._A0340StoppingTimer.progressBarStopping.Value = countDownA0340;
                        }
                        ));
                    }

                    else
                    {
                        pageMIL1._A0340StoppingTimer.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                     new Action(
                    delegate()
                    {
                        pageMIL1._A0340StoppingTimer.Visibility = System.Windows.Visibility.Hidden;
                    }
                    ));
                    }

                    break;

                //=============
                // Buttons
                //==============


                case "_0150_start":
                    pageMIL1.manualControl_A0150.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        pageMIL1.manualControl_A0150.ManualControl_SetSelectedButton = Convert.ToBoolean(t.Value);
                    }));
                    pageMTR1.manualControl_A0150.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        pageMTR1.manualControl_A0150.ManualControl_SetSelectedButton = Convert.ToBoolean(t.Value);
                    }));
                    break;
//                case "_Enable_1061":
//                    pageINT1.manualControl_A1061.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
//                    {
//                        pageINT1.manualControl_A1061.ManualControl_SetSelectedButton = Convert.ToBoolean(t.Value);
//                        pageTRF1.manualControl_A1061.ManualControl_SetSelectedButton = Convert.ToBoolean(t.Value);

//                    }));

//                    break;
//                case "_Enable_1062":
//                    pageINT1.manualControl_A1062.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
//                    {
//                        pageINT1.manualControl_A1062.ManualControl_SetSelectedButton = Convert.ToBoolean(t.Value);
//                        pageTRF1.manualControl_A1062.ManualControl_SetSelectedButton = Convert.ToBoolean(t.Value);

//                    }));

//                    break;
//                case "_Enable_1059":
//                    pageINT1.manualControl_A1059.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
//                    {
//                        pageINT1.manualControl_A1059.ManualControl_SetSelectedButton = Convert.ToBoolean(t.Value);
//                        pageTRF1.manualControl_A1059.ManualControl_SetSelectedButton = Convert.ToBoolean(t.Value);

//                    }));

//                    break;
//                //============
//                //Intake Scales
//                //===========


//                case "_1011_OutFlowrate":
//                    pageINT1.ScaleInfo_A1011.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
//                    {
//                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
//                        pageINT1.ScaleInfo_A1011.lblOutFlowrate.Content = currentTonnage.ToString();
//                        pageTRF1.ScaleInfo_A1011.lblOutFlowrate.Content = currentTonnage.ToString();
//                        _d1011_Flour1Flowrate = currentTonnage;
//                        CalculateYield();
//                    }));
//                    break;

//                case "_1011_OutAlarmNo":
//                    pageINT1.ScaleInfo_A1011.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
//                    {
//                        pageINT1.ScaleInfo_A1011.lblAlarmNo.Content = t.Value.ToString();
//                        pageTRF1.ScaleInfo_A1011.lblAlarmNo.Content = t.Value.ToString();
//                    }));
//                    break;

//                case "_1011_OutNotErasableWeight":
//                    pageINT1.ScaleInfo_A1011.lblNEWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
//                    {
//                        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 1000), 2);
//                        pageINT1.ScaleInfo_A1011.lblNEWeight.Content = currentTonnage.ToString();
//                        pageTRF1.ScaleInfo_A1011.lblNEWeight.Content = currentTonnage.ToString();
//                    }));
//                    break;

//                case "_1011_OutJobWeight":
//                    pageINT1.ScaleInfo_A1011.lblJobWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
//                    {
//                        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 1000), 2);
//                        pageINT1.ScaleInfo_A1011.lblJobWeight.Content = currentTonnage.ToString();
//                        pageTRF1.ScaleInfo_A1011.lblJobWeight.Content = currentTonnage.ToString();
//                    }));
//                    break;

//                case "_1012_OutFlowrate":
//                    pageINT1.scaleInfo_A1012.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
//                    {
//                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
//                        pageINT1.scaleInfo_A1012.lblOutFlowrate.Content = currentTonnage.ToString();
//                        pageTRF1.scaleInfo_A1012.lblOutFlowrate.Content = currentTonnage.ToString();
//                        _d1012_Flour1Flowrate = currentTonnage;
//                        CalculateYield();
//                    }));
//                    break;

//                case "_1012_OutAlarmNo":
//                    pageINT1.scaleInfo_A1012.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
//                    {
//                        pageINT1.scaleInfo_A1012.lblAlarmNo.Content = t.Value.ToString();
//                        pageTRF1.scaleInfo_A1012.lblAlarmNo.Content = t.Value.ToString();
//                    }));
//                    break;

//                case "_1012_OutNotErasableWeight":
//                    pageINT1.scaleInfo_A1012.lblNEWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
//                    {
//                        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 1000), 2);
//                        pageINT1.scaleInfo_A1012.lblNEWeight.Content = currentTonnage.ToString();
//                        pageTRF1.scaleInfo_A1012.lblNEWeight.Content = currentTonnage.ToString();
//                    }));
//                    break;

//                case "_1012_OutJobWeight":
//                    pageINT1.scaleInfo_A1012.lblJobWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
//                    {
//                        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 1000), 2);
//                        pageINT1.scaleInfo_A1012.lblJobWeight.Content = currentTonnage.ToString();
//                        pageTRF1.scaleInfo_A1012.lblJobWeight.Content = currentTonnage.ToString();
//                    }));
//                    break;

//                case "_2007_OutFlowrate":
//                    pageINT1.scaleInfo_A2007.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
//                    {
//                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
//                        pageINT1.scaleInfo_A2007.lblOutFlowrate.Content = currentTonnage.ToString();
//                        pageTRF1.scaleInfo_A2007.lblOutFlowrate.Content = currentTonnage.ToString();
//                        _d2007_Flour1Flowrate = currentTonnage;
//                        CalculateYield();
//                    }));
//                    break;

//                    //case "_2007_InFlowrate"
//                    //pageINT1.scaleInfo_A2007.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
//                    //{
//                    //    pageINT1.scaleInfo_A2007.lblAlarmNo.Content = t.Value.ToString();
//                    //    pageTRF1.scaleInfo_A2007.lblAlarmNo.Content = t.Value.ToString();
//                    //}));
//                    //break;

//                case "_2007_OutAlarmNo":
//                    pageINT1.scaleInfo_A2007.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
//                    {
//                        pageINT1.scaleInfo_A2007.lblAlarmNo.Content = t.Value.ToString();
//                        pageTRF1.scaleInfo_A2007.lblAlarmNo.Content = t.Value.ToString();
//                    }));
//                    break;

//                case "_2007_OutNotErasableWeight":
//                    pageINT1.scaleInfo_A2007.lblNEWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
//                    {
//                        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 1000), 2);
//                        pageINT1.scaleInfo_A2007.lblNEWeight.Content = currentTonnage.ToString();
//                        pageTRF1.scaleInfo_A2007.lblNEWeight.Content = currentTonnage.ToString();
//                    }));
//                    break;

//                case "_2007_OutJobWeight":
//                    pageINT1.scaleInfo_A2007.lblJobWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
//                    {
//                        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 1000), 2);
//                        pageINT1.scaleInfo_A2007.lblJobWeight.Content = currentTonnage.ToString();
//                       pageTRF1.scaleInfo_A2007.lblJobWeight.Content = currentTonnage.ToString();
//                    }));
//                    break;
//  //      =============
//// Transfer Flowbalancers
////=======================

                case "_0125_InFlowrate":
                    pageMTR1.FlowbalancerInfo_A0125.lblInFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 1000), 2);
                        pageMTR1.FlowbalancerInfo_A0125.lblInFlowrate.Content = currentTonnage.ToString();
                    }));
                    break;

                case "_0125_OutFlowrate":
                    pageMTR1.FlowbalancerInfo_A0125.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageMTR1.FlowbalancerInfo_A0125.lblOutFlowrate.Content = currentTonnage.ToString();
                        //_d4005_Flowbalancer1Flowrate = currentTonnage;
                        //_dIntakeFlowrate = _d4005_Flowbalancer1Flowrate + _d4006_Flowbalancer2Flowrate + _d4007_Flowbalancer3Flowrate;
                        CalculateYield();
                        //Update4014LitreDosingValue();
                    }));
                    break;

                case "_0125_OutAlarmNo":
                    pageMTR1.FlowbalancerInfo_A0125.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        pageMTR1.FlowbalancerInfo_A0125.lblAlarmNo.Content = t.Value.ToString();
                    }));
                    break;

 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
               
                case "_0130_InFlowrate":
                    pageMTR1.FlowbalancerInfo_A0130.lblInFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 1000), 2);
                        pageMTR1.FlowbalancerInfo_A0130.lblInFlowrate.Content = currentTonnage.ToString();
                    }));
                    break;

                case "_0130_OutFlowrate":
                    pageMTR1.FlowbalancerInfo_A0130.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageMTR1.FlowbalancerInfo_A0130.lblOutFlowrate.Content = currentTonnage.ToString();
                        //_d4005_Flowbalancer1Flowrate = currentTonnage;
                        //_dIntakeFlowrate = _d4005_Flowbalancer1Flowrate + _d4006_Flowbalancer2Flowrate + _d4007_Flowbalancer3Flowrate;
                        CalculateYield();
                        //Update4014LitreDosingValue();
                    }));
                    break;

                case "_0130_OutAlarmNo":
                    pageMTR1.FlowbalancerInfo_A0130.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        pageMTR1.FlowbalancerInfo_A0130.lblAlarmNo.Content = t.Value.ToString();
                    }));
                    break;

                //case "_2028_InFlowrate":
                //    pageFCL1.FlowbalancerInfo_A2028.lblInFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 1000), 2);
                //        pageFCL1.FlowbalancerInfo_A2028.lblInFlowrate.Content = currentTonnage.ToString();
                //    }));
                //    break;

                //case "_2028_OutFlowrate":
                //    pageFCL1.FlowbalancerInfo_A2028.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //        pageFCL1.FlowbalancerInfo_A2028.lblOutFlowrate.Content = currentTonnage.ToString();
                //        //_d4005_Flowbalancer1Flowrate = currentTonnage;
                //        //_dIntakeFlowrate = _d4005_Flowbalancer1Flowrate + _d4006_Flowbalancer2Flowrate + _d4007_Flowbalancer3Flowrate;
                //        CalculateYield();
                //        //Update4014LitreDosingValue();
                //    }));


                //    break;

                //case "_2028_OutAlarmNo":
                //    pageFCL1.FlowbalancerInfo_A2028.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageFCL1.FlowbalancerInfo_A2028.lblAlarmNo.Content = t.Value.ToString();
                //    }));
                //    break;


                //case "_2053_InFlowrate":
                //    pageSCL1.FlowbalancerInfo_A2053.lblInFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 1000), 2);
                //        pageSCL1.FlowbalancerInfo_A2053.lblInFlowrate.Content = currentTonnage.ToString();
                //    }));
                //    break;

                //case "_2053_OutFlowrate":
                //    pageSCL1.FlowbalancerInfo_A2053.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //        pageSCL1.FlowbalancerInfo_A2053.lblOutFlowrate.Content = currentTonnage.ToString();
                //        //_d4005_Flowbalancer1Flowrate = currentTonnage;
                //        //_dIntakeFlowrate = _d4005_Flowbalancer1Flowrate + _d4006_Flowbalancer2Flowrate + _d4007_Flowbalancer3Flowrate;
                //        CalculateYield();
                //        //Update4014LitreDosingValue();
                //    }));
                //    break;

                //case "_2053_OutAlarmNo":
                //    pageSCL1.FlowbalancerInfo_A2053.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageSCL1.FlowbalancerInfo_A2053.lblAlarmNo.Content = t.Value.ToString();
                //    }));
                //    break;

                //case "_2054_InFlowrate":
                //    pageSCL1.FlowbalancerInfo_A2054.lblInFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 1000), 2);
                //        pageSCL1.FlowbalancerInfo_A2054.lblInFlowrate.Content = currentTonnage.ToString();
                //    }));
                //    break;

                //case "_2054_OutFlowrate":
                //    pageSCL1.FlowbalancerInfo_A2054.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //        pageSCL1.FlowbalancerInfo_A2054.lblOutFlowrate.Content = currentTonnage.ToString();
                //        //_d4005_Flowbalancer1Flowrate = currentTonnage;
                //        //_dIntakeFlowrate = _d4005_Flowbalancer1Flowrate + _d4006_Flowbalancer2Flowrate + _d4007_Flowbalancer3Flowrate;
                //        CalculateYield();
                //        //Update4014LitreDosingValue();
                //    }));


                //    break;

                //case "_2054_OutAlarmNo":
                //    pageSCL1.FlowbalancerInfo_A2054.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageSCL1.FlowbalancerInfo_A2054.lblAlarmNo.Content = t.Value.ToString();
                //    }));
                //    break;

                //case "_2055_InFlowrate":
                //    pageSCL1.FlowbalancerInfo_A2055.lblInFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        decimal currentTonnage = Math.Round((Convert.ToDecimal(t.Value) / 10000), 2);
                //        pageSCL1.FlowbalancerInfo_A2055.lblInFlowrate.Content = currentTonnage.ToString();
                //    }));
                //    break;

                //case "_2055_OutFlowrate":
                //    pageSCL1.FlowbalancerInfo_A2055.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 10000), 2);
                //        pageSCL1.FlowbalancerInfo_A2055.lblOutFlowrate.Content = currentTonnage.ToString();
                //        //_d4005_Flowbalancer1Flowrate = currentTonnage;
                //        //_dIntakeFlowrate = _d4005_Flowbalancer1Flowrate + _d4006_Flowbalancer2Flowrate + _d4007_Flowbalancer3Flowrate;
                //        CalculateYield();
                //        //Update4014LitreDosingValue();
                //    }));


                //    break;

                //case "_2055_OutAlarmNo":
                //    pageSCL1.FlowbalancerInfo_A2055.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageSCL1.FlowbalancerInfo_A2055.lblAlarmNo.Content = t.Value.ToString();
                //    }));
                //    break;


                //ScaleInfo_A0650
                case "_A0650_PWC03.OutFlowrate":

                    pageINT1.ScaleInfo_A0650.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        pageINT1.ScaleInfo_A0650.lblOutFlowrate.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                    }));
                    pageFCL1.ScaleInfo_A0650.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        pageFCL1.ScaleInfo_A0650.lblOutFlowrate.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                    }));
                    pageMTR1.ScaleInfo_A0650.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        pageMTR1.ScaleInfo_A0650.lblOutFlowrate.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                    }));
                    pageMIL2.ScaleInfo_A0650.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        pageMIL2.ScaleInfo_A0650.lblOutFlowrate.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                    }));
                    break;

             //   case "A0650_PWC03.OutFlowrate":
             //       pageMIL1.ScaleInfo_A4002.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
             //       {
             //           double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
            //            pageMIL1.ScaleInfo_A4002.lblOutFlowrate.Content = currentTonnage.ToString();
           //         }));

           //         _d4002B1OutFlowrate = Convert.ToDouble(t.Value);
           //         CalculateYield();
           //         break;

                case "_A0650_PWC03.OutAlarmNoLog":
                    pageINT1.ScaleInfo_A0650.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        pageINT1.ScaleInfo_A0650.lblAlarmNo.Content = t.Value.ToString();
                    }));
                    pageFCL1.ScaleInfo_A0650.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        pageFCL1.ScaleInfo_A0650.lblAlarmNo.Content = t.Value.ToString();
                    }));
                    pageMTR1.ScaleInfo_A0650.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        pageMTR1.ScaleInfo_A0650.lblAlarmNo.Content = t.Value.ToString();
                    }));
                    pageMIL2.ScaleInfo_A0650.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        pageMIL2.ScaleInfo_A0650.lblAlarmNo.Content = t.Value.ToString();
                    }));
                    break;

                case "_A0650_PWC03.OutJobWeight":
                    pageINT1.ScaleInfo_A0650.lblJobWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageINT1.ScaleInfo_A0650.lblJobWeight.Content = currentTonnage.ToString();
                    }));
                    pageFCL1.ScaleInfo_A0650.lblJobWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageFCL1.ScaleInfo_A0650.lblJobWeight.Content = currentTonnage.ToString();
                    }));
                    pageMTR1.ScaleInfo_A0650.lblJobWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageMTR1.ScaleInfo_A0650.lblJobWeight.Content = currentTonnage.ToString();
                    }));
                    pageMIL2.ScaleInfo_A0650.lblJobWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageMIL2.ScaleInfo_A0650.lblJobWeight.Content = currentTonnage.ToString();
                    }));
                    break;

                case "_A0650_PWC03.OutNotErasableWeight":
                    pageINT1.ScaleInfo_A0650.lblNEWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageINT1.ScaleInfo_A0650.lblNEWeight.Content = currentTonnage.ToString();

                    }));                    
                    pageFCL1.ScaleInfo_A0650.lblNEWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageFCL1.ScaleInfo_A0650.lblNEWeight.Content = currentTonnage.ToString();
                    }));

                    pageMTR1.ScaleInfo_A0650.lblNEWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageMTR1.ScaleInfo_A0650.lblNEWeight.Content = currentTonnage.ToString();
                    }));

                    pageMIL2.ScaleInfo_A0650.lblNEWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageMIL2.ScaleInfo_A0650.lblNEWeight.Content = currentTonnage.ToString();
                    }));
                    break;

                //ScaleInfo_A0650
                case "_A0015_PWC03.OutFlowrate":

                    pageINT1.ScaleInfo_A0015.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        pageINT1.ScaleInfo_A0015.lblOutFlowrate.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                    }));
                    break;

                //   case "A0650_PWC03.OutFlowrate":
                //       pageMIL1.ScaleInfo_A4002.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //       {
                //           double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //            pageMIL1.ScaleInfo_A4002.lblOutFlowrate.Content = currentTonnage.ToString();
                //         }));

                //         _d4002B1OutFlowrate = Convert.ToDouble(t.Value);
                //         CalculateYield();
                //         break;

                case "_A0015_PWC03.OutAlarmNoLog":
                    pageINT1.ScaleInfo_A0015.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        pageINT1.ScaleInfo_A0015.lblAlarmNo.Content = t.Value.ToString();
                    }));
                    break;

                case "_A0015_PWC03.OutJobWeight":
                    pageINT1.ScaleInfo_A0015.lblJobWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageINT1.ScaleInfo_A0015.lblJobWeight.Content = currentTonnage.ToString();
                    }));
                    break;

                case "_A0015_PWC03.OutNotErasableWeight":
                    pageINT1.ScaleInfo_A0015.lblNEWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageINT1.ScaleInfo_A0015.lblNEWeight.Content = currentTonnage.ToString();
                    }));
                    break;


                ////====================
                ////MILL 1ST BREAK SCALE
                ////====================

                case "_0150_InFlowrate":

                    pageMIL1.ScaleInfo_A0150.lblInFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        //double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageMIL1.ScaleInfo_A0150.lblInFlowrate.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                    }));

                    pageMTR1.ScaleInfo_A0150.lblInFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        //double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageMTR1.ScaleInfo_A0150.lblInFlowrate.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                    }));
                    break;

                case "_0150_OutFlowrate":
                    pageMIL1.ScaleInfo_A0150.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageMIL1.ScaleInfo_A0150.lblOutFlowrate.Content = currentTonnage.ToString();
                    }));
                    pageMTR1.ScaleInfo_A0150.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageMTR1.ScaleInfo_A0150.lblOutFlowrate.Content = currentTonnage.ToString();
                    }));

                    //_d0150B1OutFlowrate = Convert.ToDouble(t.Value);
                    //CalculateYield();
                    break;

                case "_0150_OutAlarmNolog":
                    pageMIL1.ScaleInfo_A0150.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        pageMIL1.ScaleInfo_A0150.lblAlarmNo.Content = t.Value.ToString();
                    }));
                    pageMTR1.ScaleInfo_A0150.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        pageMTR1.ScaleInfo_A0150.lblAlarmNo.Content = t.Value.ToString();
                    }));
                    break;

                case "_0150_OutJobWeight":
                    pageMIL1.ScaleInfo_A0150.lblJobWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageMIL1.ScaleInfo_A0150.lblJobWeight.Content = currentTonnage.ToString();
                    }));
                    pageMTR1.ScaleInfo_A0150.lblJobWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageMTR1.ScaleInfo_A0150.lblJobWeight.Content = currentTonnage.ToString();
                    }));
                    break;

                case "_0150_OutNotErasableWeight":
                    pageMIL1.ScaleInfo_A0150.lblNEWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageMIL1.ScaleInfo_A0150.lblNEWeight.Content = currentTonnage.ToString();
                    }));
                    pageMTR1.ScaleInfo_A0150.lblNEWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageMTR1.ScaleInfo_A0150.lblNEWeight.Content = currentTonnage.ToString();
                    }));
                    break;

                ////====================
                /// Finished Product SCALES
                ////====================

                //ScaleInfo_A0640
                case "_A0640_PWC03.OutFlowrate":

                    pageMIL2.ScaleInfo_A0640.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        pageMIL2.ScaleInfo_A0640.lblOutFlowrate.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                    }));
                    break;

                //   case "A0650_PWC03.OutFlowrate":
                //       pageMIL1.ScaleInfo_A4002.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //       {
                //           double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //            pageMIL1.ScaleInfo_A4002.lblOutFlowrate.Content = currentTonnage.ToString();
                //         }));

                //         _d4002B1OutFlowrate = Convert.ToDouble(t.Value);
                //         CalculateYield();
                //         break;

                case "_A0640_PWC03.OutAlarmNoLog":
                    pageMIL2.ScaleInfo_A0640.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        pageMIL2.ScaleInfo_A0640.lblAlarmNo.Content = t.Value.ToString();
                    }));
                    break;

                case "_A0640_PWC03.OutJobWeight":
                    pageMIL2.ScaleInfo_A0640.lblJobWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageMIL2.ScaleInfo_A0640.lblJobWeight.Content = currentTonnage.ToString();
                    }));
                    break;

                case "_A0640_PWC03.OutNotErasableWeight":
                    pageMIL2.ScaleInfo_A0640.lblNEWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageMIL2.ScaleInfo_A0640.lblNEWeight.Content = currentTonnage.ToString();
                    }));
                    break;

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //ScaleInfo_A0645
                case "_A0645_PWC03.OutFlowrate":

                    pageMIL2.ScaleInfo_A0645.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        pageMIL2.ScaleInfo_A0645.lblOutFlowrate.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                    }));
                    break;

                //   case "A0650_PWC03.OutFlowrate":
                //       pageMIL1.ScaleInfo_A4002.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //       {
                //           double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //            pageMIL1.ScaleInfo_A4002.lblOutFlowrate.Content = currentTonnage.ToString();
                //         }));

                //         _d4002B1OutFlowrate = Convert.ToDouble(t.Value);
                //         CalculateYield();
                //         break;

                case "_A0645_PWC03.OutAlarmNoLog":
                    pageMIL2.ScaleInfo_A0645.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        pageMIL2.ScaleInfo_A0645.lblAlarmNo.Content = t.Value.ToString();
                    }));
                    break;

                case "_A0645_PWC03.OutJobWeight":
                    pageMIL2.ScaleInfo_A0645.lblJobWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageMIL2.ScaleInfo_A0645.lblJobWeight.Content = currentTonnage.ToString();
                    }));
                    break;

                case "_A0645_PWC03.OutNotErasableWeight":
                    pageMIL2.ScaleInfo_A0645.lblNEWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                    {
                        double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                        pageMIL2.ScaleInfo_A0645.lblNEWeight.Content = currentTonnage.ToString();
                    }));
                    break;

                //case "_4404_Outflowrate":
                //    pageMIL1B.scaleInfo_A4404.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageMIL1B.scaleInfo_A4404.lblOutFlowrate.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //    }));
                //    //d4404MixbackFlourOutFlowrate = Convert.ToDouble(t.Value);
                //    //CalculateYield();
                //    break;

                //case "_4404_OutAlarmNo":
                //    pageMIL1B.scaleInfo_A4404.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageMIL1B.scaleInfo_A4404.lblAlarmNo.Content = t.Value.ToString();
                //    }));
                //    break;

                //case "_4404_OutJobWeight":
                //    pageMIL1B.scaleInfo_A4404.lblJobWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        //double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //        pageMIL1B.scaleInfo_A4404.lblJobWeight.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //    }));
                //    break;

                //case "_4404_OutNotErasableWeight":
                //    pageMIL1B.scaleInfo_A4404.lblNEWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        //double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //        pageMIL1B.scaleInfo_A4404.lblNEWeight.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //    }));
                //    break;

                ////case "_4404_Inflowrate":
                ////    pageMIL1B.scaleInfo_A4404.lblInFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                ////    {
                ////        //double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                ////        pageMIL1B.scaleInfo_A4404.lblInFlowrate.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                ////    }));
                ////    break;

                //case "_6023_Outflowrate":
                //    pageMIL1B.scaleInfo_A6023.lblOutFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageMIL1B.scaleInfo_A6023.lblOutFlowrate.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //        pageBRN1.scaleInfo_A6023.lblOutFlowrate.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //        pageSCG1.scaleInfo_A6023.lblOutFlowrate.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //    }));
                //    //d6023MixbackFlourOutFlowrate = Convert.ToDouble(t.Value);
                //    //CalculateYield();
                //    break;

                //case "_6023_OutAlarmNo":
                //    pageMIL1B.scaleInfo_A6023.lblAlarmNo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageMIL1B.scaleInfo_A6023.lblAlarmNo.Content = t.Value.ToString();
                //        pageBRN1.scaleInfo_A6023.lblAlarmNo.Content = t.Value.ToString();
                //        pageSCG1.scaleInfo_A6023.lblAlarmNo.Content = t.Value.ToString();
                //    }));
                //    break;

                //case "_6023_OutJobWeight":
                //    pageMIL1B.scaleInfo_A6023.lblJobWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        //double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //        pageMIL1B.scaleInfo_A6023.lblJobWeight.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //        pageBRN1.scaleInfo_A6023.lblJobWeight.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //        pageSCG1.scaleInfo_A6023.lblJobWeight.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //    }));
                //    break;

                //case "_6023_OutNotErasableWeight":
                //    pageMIL1B.scaleInfo_A6023.lblNEWeight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        //double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //        pageMIL1B.scaleInfo_A6023.lblNEWeight.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //        pageBRN1.scaleInfo_A6023.lblNEWeight.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //        pageSCG1.scaleInfo_A6023.lblNEWeight.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //    }));
                //    break;

                //case "_6023_Inflowrate":
                //    pageMIL1B.scaleInfo_A6023.lblInFlowrate.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        //double currentTonnage = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //        pageMIL1B.scaleInfo_A6023.lblInFlowrate.Content = Math.Round((Convert.ToDouble(t.Value) / 1000), 2);
                //    }));
                //    break;
               
                //case "_HammerByPass":
                //    bool HammerByPass = Convert.ToBoolean(t.Value);
                //    if (HammerByPass)
                //    {

                //        pageMIL1B.ByPassHammerButton.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //        {
                //            pageMIL1B.ByPassHammerButton.btnBypassHamer.Visibility = Visibility.Hidden;

                //        }));

                //        pageMIL1B.ByPassHammerButton.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //        {
                //            pageMIL1B.ByPassHammerButton.btnStartHammer.Visibility = Visibility.Visible;

                //        }));
                //    }
                //    else
                //    {

                //        pageMIL1B.ByPassHammerButton.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //        {
                //            pageMIL1B.ByPassHammerButton.btnBypassHamer.Visibility = Visibility.Visible;

                //        }));

                //        pageMIL1B.ByPassHammerButton.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //        {
                //            pageMIL1B.ByPassHammerButton.btnStartHammer.Visibility = Visibility.Hidden;

                //        }));
                //    }
                //    break;

                //Bypass timer (Filter Flour 4080 High Level 4120)

                //case "_4120_ActualTimeValue":
                //    pageMIL1A.ByPassControl_4120.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageMIL1A.ByPassControl_4120.BypassTimerValue = Convert.ToInt32(t.Value);
                //    }));

                //    break;

                //============
                //Hammer Mill
                //===========

               

   

                case "_Mill_Rolls_Engaged":
                    if (Convert.ToBoolean(t.Value) == true)
                    {
                        //Show "ENGAGE" Text label if mill rollers are engaged
                        pageMIL1.textLabelEngaged.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageMIL1.textLabelEngaged.Visibility = System.Windows.Visibility.Visible;
                            pageMIL1.textLabelDisengaged.Visibility = System.Windows.Visibility.Hidden;
                        }));
                    }
                    else
                    {
                        //Show "DISENGAGED" Text label if mill rollers are disengaged
                        pageMIL1.textLabelDisengaged.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            pageMIL1.textLabelDisengaged.Visibility = System.Windows.Visibility.Visible;
                            pageMIL1.textLabelEngaged.Visibility = System.Windows.Visibility.Hidden;
                        }));
                    }
                    break;

               
                //case "_BinsEmpty_StatusWord":
                //    ProductsViewModelDataContext.BinStatusWordValue = Convert.ToInt64(t.Value);
                //    break;
                //case "_201":
                //    pageFCL1.lbl2020Speed.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageFCL1.lbl2020Speed.Content = Convert.ToInt32(t.Value) + "%";
                       
                //    }));
                //    break;
                //case "_202":
                //    pageFCL1.lbl2021Speed.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageFCL1.lbl2021Speed.Content = Convert.ToInt32(t.Value) + "%";

                //    }));
                //    break;
                //case "_203":
                //    pageFCL1.lbl2022Speed.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                //    {
                //        pageFCL1.lbl2022Speed.Content = Convert.ToInt32(t.Value) + "%";

                //    }));
                //    break;
            }

        }

        public void CalculateYield()
        {
            // MessageBox.Show(_dIntakeFlowrate.ToString());
            //_dIntakeFlowrate = (_d4005_Flowbalancer1Flowrate + _d4006_Flowbalancer2Flowrate + _d4007_Flowbalancer3Flowrate);
            //if (_dIntakeFlowrate > 0)
            //{
            //    pageMIL3.YieldCalculator.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            //    {
            //        //double dTotalProduct = _d4403_Flour1Flowrate;
            //        _dFlour1Yield = Math.Round((_d4403_Flour1Flowrate / _dIntakeFlowrate) * 100, 1);
            //        double dLoss = 100 - _dFlour1Yield;
            //        pageMIL3.YieldCalculator.lblTotalYield.Content = _dFlour1Yield;
            //        if (_dFlour1Yield > 100)
            //        {
            //            //   pageMIL1.YieldCalculator1.lblLossGain.Content = "+" + Math.Round(dTotal - 100,1);
            //            pageMIL3.YieldCalculator.lblLossGain.Content = "+" + Math.Round(dLoss, 1);
            //            pageMIL3.YieldCalculator.lblLossGain.Foreground = Brushes.Green;
            //            pageMIL3.YieldCalculator.lblLossGainTitle.Foreground = Brushes.Green;
            //            pageMIL3.YieldCalculator.lblLossGainPercent.Foreground = Brushes.Green;
            //        }
            //        else
            //        {
            //            double dTemp = Math.Round(_dFlour1Yield, 1);
            //            pageMIL3.YieldCalculator.lblLossGain.Content = Math.Round(dLoss, 1);

            //            if (dTemp > 80)
            //            {
            //                pageMIL3.YieldCalculator.lblLossGain.Foreground = Brushes.Green;
            //                pageMIL3.YieldCalculator.lblLossGainTitle.Foreground = Brushes.Green;
            //                pageMIL3.YieldCalculator.lblLossGainPercent.Foreground = Brushes.Green;
            //            }
            //            else if (dTemp > 70)
            //            {
            //                pageMIL3.YieldCalculator.lblLossGain.Foreground = Brushes.Orange;
            //                pageMIL3.YieldCalculator.lblLossGainTitle.Foreground = Brushes.Orange;
            //                pageMIL3.YieldCalculator.lblLossGainPercent.Foreground = Brushes.Orange;
            //            }
            //            else
            //            {
            //                pageMIL3.YieldCalculator.lblLossGain.Foreground = Brushes.Red;
            //                pageMIL3.YieldCalculator.lblLossGainTitle.Foreground = Brushes.Red;
            //                pageMIL3.YieldCalculator.lblLossGainPercent.Foreground = Brushes.Red;
            //            }
            //        }
            //    }));
            //}
            //else
            //{
            //    pageMIL3.YieldCalculator.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            //    {
            //        _dFlour1Yield = 0;
            //        pageMIL3.YieldCalculator.lblLossGain.Content = "0.0";
            //    }));
            //}
        }

        #region StandardMethods [Standard]


        /// <summary>
        /// Starts the threads that communicate with the PLC
        /// </summary>
        public void StartCommunicationThread()
        {
            MainWindow.bThreadsToRun = true;
            threadTagRead = new Thread(new ThreadStart(InitializeTagUpdateThread));
            threadTagRead.Start();
            threadPLCComms = new Thread(new ThreadStart(CheckPLCComms));
            threadPLCComms.Start();
            timerWriteLog.Start();
            timerWriteReporting.Start();
            timerTrends.Start();
            timerCleanHistoricLog.Start();
        }


        /// <summary>
        /// Hides the Emptying Timer on the Template
        /// </summary>
        public void HideEmptyingTimer()
        {
            EmptyingTimer1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                EmptyingTimer1.Visibility = System.Windows.Visibility.Hidden;
            }
            ));
        }



        /// <summary>
        /// Pulses the RequestModify bit for the active line to change jobs
        /// </summary>
        public void ModifyCurrentJob()
        {
            if (bPLCCommsGood)
            {
                if (tControl_CmdRequestModify.Name != "DB0.DBX0.0")
                {
                    if (!PLC1_W.IsConnected)
                    {
                        PLC1_W.Connect();
                    }

                    if (PLC1_W.ErrorCode != ResultCode.E_SUCCESS)
                        MessageBox.Show(PLC1_W.ErrorString + "\n" + PLC1_W.IsConnected);

                    tControl_CmdRequestModify.Value = true;
                    PLC1_W.WriteTag(tControl_CmdRequestModify);
                    PLC1_W.Disconnect();

                    Button b = new Button();
                    b.Name = "btnJobModify";
                    UIInteraction_Change(b, "Job Modified [" + stat_iActiveLineNumber + "]");
                    //SetActiveLineButtonColor_OnStartFeed();
                }
                else
                {
                    MessageBox.Show("Modify --> Default Address");
                }
            }
            else
            {
                MessageBox.Show("No Communication to the PLC", "Communication Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }


        /// <summary>
        /// Sets the lock status of the given bin
        /// </summary>
        /// <param name="booleanDBOffset">Bit offset to toggle true or false</param>
        /// <param name="tagValue">Value to write to the bit</param>
        /// <param name="BinName">The name of the bin to lock/unlock</param>
        public void SetLockStatusOfBin(string booleanDBOffset, bool tagValue, string BinName)
        {
            string sAction = tagValue == true ? "LOCK " : "UNLOCK ";
            MessageBoxResult res = MessageBox.Show("This will " + sAction + BinName + ". Continue?", "Confirm Action", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                if (bPLCCommsGood)
                {
                    if (stat_iActiveLineNumber > 0)
                    {
                        if (!PLC1_W.IsConnected)
                        {
                            PLC1_W.Connect();
                        }

                        if (PLC1_W.ErrorCode != ResultCode.E_SUCCESS)
                            MessageBox.Show(PLC1_W.ErrorString + "\n" + PLC1_W.IsConnected);

                        Tag t = new Tag(booleanDBOffset, S7Link.Tag.ATOMIC.BOOL, 1);
                        t.Value = tagValue;
                        PLC1_W.WriteTag(t);
                        PLC1_W.Disconnect();
                    }
                    else
                    {
                        MessageBox.Show("SetLockStatus --> Default Address");
                    }
                }
                else
                {
                    MessageBox.Show("No Communication to the PLC", "Communication Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }


        #endregion


        //------------------------------------------------------------------------------//
        //                             Line Buttton Clicks                              //
        //------------------------------------------------------------------------------//              

        #region Line Buttons
        //INT1
        private void btnLine1_Click(object sender, RoutedEventArgs e)
        {
            sActiveLineName = "INT1 - PRODUCT INTAKE";
            iActiveLineNumber = 1;
            _mainFrame.Navigate(pageINT1);
        }

        //FCL1
        private void btnLine2_Click(object sender, RoutedEventArgs e)
        {
            sActiveLineName = "FCL1 - FIRST CLEANING";
            iActiveLineNumber = 2;
            _mainFrame.Navigate(pageFCL1);
        }


        //MTR1
        private void btnLine3_Click(object sender, RoutedEventArgs e)
        {
            sActiveLineName = "MTR1 - PRODUCT TRANSFER";
            iActiveLineNumber = 3;
            _mainFrame.Navigate(pageMTR1);
        }

        //MIL1
        private void btnLine4_Click(object sender, RoutedEventArgs e)
        {
            sActiveLineName = "MIL1 - MILL 1";
            iActiveLineNumber = 4;
            _mainFrame.Navigate(pageMIL1);
        }

        //MIL2
        private void btnLine5_Click(object sender, RoutedEventArgs e)
        {
            sActiveLineName = "MIL2 - MIL 2";
            iActiveLineNumber = 5;
            _mainFrame.Navigate(pageMIL2);
        }



        //REPORT VIEWER
        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            iActiveLineNumber = 0;
            _mainFrame.Navigate(new DisplayPages.ReportViewer());
        }

        #endregion
        
        /// <summary>
        /// Job List button click.
        /// Add the relevant job list in for each line
        /// </summary>        
        private void btnJob_Click(object sender, RoutedEventArgs e)
        {
            bool bContinue = true;
            bool bIsWindowOpen = false;

            string sTitle = "";
            SortedList slSender = new SortedList();
            SortedList slReceiver = new SortedList();
            SortedList slSenderDBs = new SortedList();
            SortedList slReceiverDBs = new SortedList();

            if (stat_iActiveLineNumber == 1)
            {
                sTitle = "INT1 Job List";
                slSender = slJobList_INT1SenderBins;
                slSenderDBs = slJobList_INT1SenderDBs;
                slReceiver = slJobList_INT1ReceiverBins;
                slReceiverDBs = slJobList_INT1ReceiverDBs;
                bIsWindowOpen = Classes.StandardCode.IsSpecificWindowOpen(Application.Current.Windows, sTitle);
                
            }
            if (stat_iActiveLineNumber == 2)
            {
                sTitle = "FCL1 Job List";
                slSender = slJobList_FCL1SenderBins;
                slSenderDBs = slJobList_FCL1SenderDBs;
                slReceiver = slJobList_FCL1ReceiverBins;
                slReceiverDBs = slJobList_FCL1ReceiverDBs;
                bIsWindowOpen = Classes.StandardCode.IsSpecificWindowOpen(Application.Current.Windows, sTitle);
            }

            if(stat_iActiveLineNumber == 3)
            {
                sTitle = "MTR1 Job List";
                slSender = slJobList_MTR1SenderBins;
                slSenderDBs = slJobList_MTR1SenderDBs;
                slReceiver = slJobList_MTR1ReceiverBins;
                slReceiverDBs = slJobList_MTR1ReceiverDBs;
                bIsWindowOpen = Classes.StandardCode.IsSpecificWindowOpen(Application.Current.Windows, sTitle);
            }
            
            if(stat_iActiveLineNumber == 4)
            {
                sTitle = "MIL1 Job List";
                slSender = slJobList_MIL1SenderBins;
                slSenderDBs = slJobList_MIL1SenderDBs;
                slReceiver = slJobList_MIL1ReceiverBins;
                slReceiverDBs = slJobList_MIL1ReceiverDBs;
                bIsWindowOpen = Classes.StandardCode.IsSpecificWindowOpen(Application.Current.Windows, sTitle);
            }


            if (!bIsWindowOpen && bContinue)
            {
                //
                //If there are values in the list then continue
                //
                if (slSender.Count != 0 || slReceiver.Count != 0)
                {
                    
                        DisplayWindows.JobList jl = new DisplayWindows.JobList(slSender, slReceiver, slSenderDBs, slReceiverDBs, PLC1_R, PLC1_W);
                        jl.Title = sTitle;
                        jl.Show();
                   
                }
                else
                {
                    MessageBox.Show("There are no Available Senders and/or Receivers for this Line", "Note", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }



        #region Control Buttons [Standard]


        //CONTROL - START
        private void btnControlStart_Click(object sender, RoutedEventArgs e)
        {
            if (bPLCCommsGood)
            {
                if (tControl_CmdStart.Name != "DB0.DBX0.0")
                {
                    if (!PLC1_W.IsConnected)
                    {
                        PLC1_W.Connect();
                    }

                    if (PLC1_W.ErrorCode != ResultCode.E_SUCCESS)
                        MessageBox.Show(PLC1_W.ErrorString + "\n" + PLC1_W.IsConnected + "\n" + PLC1_W.IPAddress);

                    tControl_CmdFeedOn.Value = true;
                    tControl_CmdStart.Value = true;
                    tControl_CmdTransferOn.Value = true;
                    tControl_CmdRequestExecute.Value = true;
                    PLC1_W.WriteTag(tControl_CmdFeedOn);
                    PLC1_W.WriteTag(tControl_CmdStart);
                    PLC1_W.WriteTag(tControl_CmdTransferOn);
                    PLC1_W.WriteTag(tControl_CmdRequestExecute);
                    PLC1_W.Disconnect();

                    UIInteraction_Change(this.btnControlStart, e);
                    //SetActiveLineButtonColor_OnStartFeed();
                }
                else
                {
                    MessageBox.Show("Start --> Default Address");
                }
            }
            else
            {
                MessageBox.Show("No Communication to the PLC", "Communication Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }


        //CONTROL - SUSPEND FEED
        private void btnControlSuspend_Click(object sender, RoutedEventArgs e)
        {
            if (bPLCCommsGood)
            {
                if (tControl_CmdFeedOff.Name != "DB0.DBX0.0")
                {
                    if (!PLC1_W.IsConnected)
                    {
                        PLC1_W.Connect();
                    }

                    if (PLC1_W.ErrorCode != ResultCode.E_SUCCESS)
                        MessageBox.Show(PLC1_W.ErrorString);

                    tControl_CmdFeedOff.Value = true;
                    PLC1_W.WriteTag(tControl_CmdFeedOff);
                    PLC1_W.Disconnect();

                    UIInteraction_Change(this.btnControlSuspend, e);
                    //SetActiveLineButtonColor_OnPauseFeed();
                }
            }
            else
            {
                MessageBox.Show("No Communication to the PLC", "Communication Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        //CONTROL - SEQUENCE STOP
        private void btnControlSeqStop_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Perform a SEQUENCE STOP on this Line ?", "SEQUENCE STOP?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (mbr == MessageBoxResult.Yes)
            {
                if (bPLCCommsGood)
                {
                    if (tControl_CmdSeqStop.Name != "DB0.DBX0.0")
                    {
                        if (!PLC1_W.IsConnected)
                        {
                            PLC1_W.Connect();
                        }

                        if (PLC1_W.ErrorCode != ResultCode.E_SUCCESS)
                            MessageBox.Show(PLC1_W.ErrorString);

                        tControl_CmdSeqStop.Value = true;
                        PLC1_W.WriteTag(tControl_CmdSeqStop);

                        if (stat_iActiveLineNumber > 0)
                        {
                            //
                            //If the line is a mill line then write the acknowledge status bits of all sections
                            //
                            if (Convert.ToInt32(alLineTypes[stat_iActiveLineNumber - 1]) == 2)
                            {
                                ControlBoxSet cbs = (ControlBoxSet)alControlBoxSet[stat_iActiveLineNumber - 1];

                                string sAckStatBit = ".DBX240.0";

                                string s1 = cbs.LineS1DB + sAckStatBit;
                                string s2 = cbs.LineS2DB + sAckStatBit;
                                string s3 = cbs.LineS3DB + sAckStatBit;

                                Tag tAckStat1 = new Tag(s1, S7Link.Tag.ATOMIC.BOOL, 1);
                                Tag tAckStat2 = new Tag(s2, S7Link.Tag.ATOMIC.BOOL, 1);
                                Tag tAckStat3 = new Tag(s3, S7Link.Tag.ATOMIC.BOOL, 1);

                                tAckStat1.Value = true;
                                tAckStat2.Value = true;
                                tAckStat3.Value = true;
                                //tSuspend1.Value = true; //Pause the feed

                                PLC1_W.WriteTag(tAckStat1);
                                PLC1_W.WriteTag(tAckStat2);
                                PLC1_W.WriteTag(tAckStat3);
                                //PLC1_W.WriteTag(tSuspend1);
                            }
                        }

                        PLC1_W.Disconnect();
                        UIInteraction_Change(this.btnControlSeqStop, e);
                    }
                }
                else
                {
                    MessageBox.Show("No Communication to the PLC", "Communication Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        //CONTROL - ACKNOWLEDGE FAULT
        private void btnControlAcknowledge_Click(object sender, RoutedEventArgs e)
        {
            if (bPLCCommsGood)
            {
                if (tControl_CmdFaultReset.Name != "DB0.DBX0.0")
                {
                    if (!PLC1_W.IsConnected)
                    {
                        PLC1_W.Connect();
                    }

                    if (PLC1_W.ErrorCode != ResultCode.E_SUCCESS)
                        MessageBox.Show(PLC1_W.ErrorString);

                    tControl_CmdFaultReset.Value = true;
                    PLC1_W.WriteTag(tControl_CmdFaultReset);
                    PLC1_W.Disconnect();

                    UIInteraction_Change(this.btnControlAcknowledge, e);
                }
            }
            else
            {
                MessageBox.Show("No Communication to the PLC", "Communication Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }

        }

        //CONTROL - MUTE SIREN
        private void btnControlMuteSiren_Click(object sender, RoutedEventArgs e)
        {
            if (bPLCCommsGood)
            {
                if (tControl_CmdHornOff.Name != "DB0.DBX0.0")
                {
                    if (!PLC1_W.IsConnected)
                    {
                        PLC1_W.Connect();
                    }

                    if (PLC1_W.ErrorCode != ResultCode.E_SUCCESS)
                        MessageBox.Show(PLC1_W.ErrorString);

                    tControl_CmdHornOff.Value = true;
                    tControl_CmdRequestDefine.Value = true;
                    PLC1_W.WriteTag(tControl_CmdHornOff);
                    PLC1_W.WriteTag(tControl_CmdRequestDefine);
                    PLC1_W.Disconnect();
                    UIInteraction_Change(this.btnControlMuteSiren, e);
                }
            }
            else
            {
                MessageBox.Show("No Communication to the PLC", "Communication Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        //CONTROL - EMERGENCY STOP
        private void btnControlEmergencyStop_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Perform an EMERGENCY STOP on this Line ?", "EMERGENCY STOP?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (mbr == MessageBoxResult.Yes)
            {
                if (bPLCCommsGood)
                {
                    if (tControl_CmdEStop.Name != "DB0.DBX0.0")
                    {
                        if (!PLC1_W.IsConnected)
                        {
                            PLC1_W.Connect();
                        }

                        if (PLC1_W.ErrorCode != ResultCode.E_SUCCESS)
                            MessageBox.Show(PLC1_W.ErrorString);

                        tControl_CmdEStop.Value = true;
                        tControl_CmdReset.Value = true;

                        TagGroup Estop = new TagGroup();
                        Estop.AddTag(tControl_CmdEStop);
                        Estop.AddTag(tControl_CmdReset);

                        PLC1_W.GroupWrite(Estop);


                        if (stat_iActiveLineNumber > 0)
                        {
                            //
                            //If the line is a mill line then write the acknowledge status bits of all sections
                            //
                            if (Convert.ToInt32(alLineTypes[stat_iActiveLineNumber - 1]) == 2)
                            {
                                ControlBoxSet cbs = (ControlBoxSet)alControlBoxSet[stat_iActiveLineNumber - 1];

                                string sAckStatBit = ".DBX240.0";
                                string sCmdResetSection = ".DBX49.6";

                                string s1 = cbs.LineS1DB + sAckStatBit;
                                string s2 = cbs.LineS2DB + sAckStatBit;
                                string s3 = cbs.LineS3DB + sAckStatBit;

                                string sCmdResetSec1 = cbs.LineS1DB + sCmdResetSection;
                                string sCmdResetSec2 = cbs.LineS2DB + sCmdResetSection;
                                string sCmdResetSec3 = cbs.LineS3DB + sCmdResetSection;

                                Tag tAckStat1 = new Tag(s1, S7Link.Tag.ATOMIC.BOOL, 1);
                                Tag tAckStat2 = new Tag(s2, S7Link.Tag.ATOMIC.BOOL, 1);
                                Tag tAckStat3 = new Tag(s3, S7Link.Tag.ATOMIC.BOOL, 1);
                                Tag tCmdReset1 = new Tag(sCmdResetSec1, S7Link.Tag.ATOMIC.BOOL, 1);
                                Tag tCmdReset2 = new Tag(sCmdResetSec2, S7Link.Tag.ATOMIC.BOOL, 1);
                                Tag tCmdReset3 = new Tag(sCmdResetSec3, S7Link.Tag.ATOMIC.BOOL, 1);

                                tAckStat1.Value = true;
                                tAckStat2.Value = true;
                                tAckStat3.Value = true;
                                tCmdReset1.Value = true;
                                tCmdReset2.Value = true;
                                tCmdReset3.Value = true;


                                PLC1_W.WriteTag(tAckStat1);
                                PLC1_W.WriteTag(tAckStat2);
                                PLC1_W.WriteTag(tAckStat3);
                                PLC1_W.WriteTag(tCmdReset1);
                                PLC1_W.WriteTag(tCmdReset2);
                                PLC1_W.WriteTag(tCmdReset3);
                            }
                        }


                        PLC1_W.Disconnect();

                        UIInteraction_Change(this.btnControlEmergencyStop, e);
                    }
                }
                else
                {
                    MessageBox.Show("No Communication to the PLC", "Communication Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        #endregion




        #region General Buttons [Standard]


        //SETTINGS
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            iActiveLineNumber = 0;
            _mainFrame.Navigate(pageSettings);
        }

        //PROFIBUS NETWORK
        private void btnProfibusNetwork_Click(object sender, RoutedEventArgs e)
        {
            iActiveLineNumber = 0;
            _mainFrame.Navigate(pageProfibusNetwork);
        }


        //START
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            iActiveLineNumber = 0;
            if (bPLCCommsGood)
            {
                if (threadTagRead == null || !threadTagRead.IsAlive)
                {
                    MessageBoxResult res = MessageBox.Show("Are you sure you want to start the communication thread?", "Start Comms?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        StartCommunicationThread();
                    }
                }
            }
            else
            {
                MessageBox.Show("Check PLC Comms?", "Bad PLC Comms?", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        //STOP
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Are you sure you want to stop the communication thread and delete all tags currently in memory?", "Stop Comms?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (res == MessageBoxResult.Yes)
            {
                iActiveLineNumber = 0;

                MainWindow.bThreadsToRun = false;

                //tagroupLineStates.Clear();
                tagroupSecOutEmptying.Clear();
                tagroupSecParEmptying.Clear();
                tagroupSecStates.Clear();
                tagroupSecStatesFAULT.Clear();
                tagroupSmartTags.Clear();
                tagroupAdditionalSmartTags.Clear();
                alControlBoxSet.Clear();
            }
        }


        //LOG ON
        private void btnLogOn_Click(object sender, RoutedEventArgs e)
        {
            iActiveLineNumber = 0;
            _mainFrame.Navigate(new DisplayPages.LogOn(this));
        }


        //LOG OFF
        private void btnLogOff_Click(object sender, RoutedEventArgs e)
        {

            if (stat_iUserLevel > 0)
            {
                MessageBoxResult res = MessageBox.Show("Would you like to Log out " + stat_sLoggedInUser + "?", "Log Out?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (res == MessageBoxResult.Yes)
                {
                    UIInteraction_Change(this.btnLogOff, e, "LogOff (" + stat_sLoggedInUser + ")");
                    MainWindow.stat_sLoggedInUser = "";
                    MainWindow.stat_iUserLevel = 0;
                    MainWindow.bLoggedIn = false;
                    MainWindow.sCurrentUsername = "";
                    MainWindow.sCurrentPassword = "";
                    iActiveLineNumber = 0;
                    _mainFrame.Navigate(new DisplayPages.StartPage());
                }
            }
        }


        //HELP
        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            iActiveLineNumber = 0;
            _mainFrame.Navigate(new DisplayPages.Help(this));
        }


        //EXIT
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Exit the application?", "Exit?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (mbr == MessageBoxResult.Yes)
            {
                try
                {
                    if (standardCode.TestSQLDatabaseConnection(SqlConnectionString))
                    {
                        using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
                        {
                            sqlConn.Open();
                            SqlCommand cmd = sqlConn.CreateCommand();

                            string sInsert = "Application Closed (" + MainWindow.stat_sLoggedInUser + ")";

                            cmd.CommandText = "INSERT INTO ApplicationLog VALUES ('" + DateTime.Now + "'," + DateTime.Now.ToOADate() + ",'" + btnExit.Name + "','" + sInsert + "',30)";
                            int rowsInserted = cmd.ExecuteNonQuery();
                            sqlConn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Log Exit --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                this.Close();
            }
        }


        //ALARM VIEWER
        private void txtAlarms_MouseDown(object sender, MouseButtonEventArgs e)
        {
            iActiveLineNumber = 0;
            _mainFrame.Navigate(new DisplayPages.AlarmViewer(alLoggerToUI));
        }



        //Reset the AutoLogOffCounter every time the screen is touched
        private void WindowMain_TouchDown(object sender, TouchEventArgs e)
        {
            stat_iLogOffCounter = 0;
        }

        //Reset the AutoLogOffCounter every time the screen is clicked
        private void WindowMain_MouseDown(object sender, MouseButtonEventArgs e)
        {
            stat_iLogOffCounter = 0;
        }


        private void lblHideHint_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gbTipBox.Visibility = Visibility.Hidden;
        }

        #endregion



        //------------------------------------------------------------------------------//
        //                             Multitouch Zooming                               //
        //------------------------------------------------------------------------------//
        /// <summary>
        /// Enable The IsManipulationEnabled bit on the selected Display Page for multitouch Zooming
        /// </summary>        
        private void btnEnableZoom_Click(object sender, RoutedEventArgs e)
        {
            if (stat_iActiveLineNumber == 1)
            {
                pageINT1.grid1.IsManipulationEnabled = true;
            }
            else if (stat_iActiveLineNumber == 2)
            {
                pageFCL1.grid1.IsManipulationEnabled = true;
            }
            else if (stat_iActiveLineNumber == 3)
            {
                pageMTR1.grid1.IsManipulationEnabled = true;
            }
            else if (stat_iActiveLineNumber == 4)
            {
                pageMIL1.grid1.IsManipulationEnabled = true;
            }
            else if (stat_iActiveLineNumber == 5)
            {
                pageMIL2.grid1.IsManipulationEnabled = true;
            }
           
        }

        /// <summary>
        /// Disable The IsManipulationEnabled bit on the selected Display Page for multitouch Zooming
        /// </summary>        
        private void btnDisableZoom_Click(object sender, RoutedEventArgs e)
        {
            if (stat_iActiveLineNumber == 1)
            {
                pageINT1.grid1.IsManipulationEnabled = false;
            }
            else if (stat_iActiveLineNumber == 2)
            {
                pageFCL1.grid1.IsManipulationEnabled = false;
            }
            else if (stat_iActiveLineNumber == 3)
            {
                pageMTR1.grid1.IsManipulationEnabled = false;
            }
            else if (stat_iActiveLineNumber == 4)
            {
                pageMIL1.grid1.IsManipulationEnabled = false;
            }
            else if (stat_iActiveLineNumber == 5)
            {
                pageMIL2.grid1.IsManipulationEnabled = false;
            }
      
        }

        /// <summary>
        /// Saves the current zoom setting to the Database
        /// </summary>        
        private void btnSaveZoom_Click(object sender, RoutedEventArgs e)
        {
            bool bSettingSaved = false;

            if (stat_iActiveLineNumber == 1)
            {
                bSettingSaved = standardCode.SaveZoomValue(SqlConnectionString, "VIS_ZOOML1", "" + DisplayPages.INT1.sMatrixTransformValue);
            }
            else if (stat_iActiveLineNumber == 2)
            {
                bSettingSaved = standardCode.SaveZoomValue(SqlConnectionString, "VIS_ZOOML2", "" + DisplayPages.FCL1.sMatrixTransformValue);
            }
            else if (stat_iActiveLineNumber == 3)
            {
                bSettingSaved = standardCode.SaveZoomValue(SqlConnectionString, "VIS_ZOOML3", "" + DisplayPages.MTR1.sMatrixTransformValue);
            }
            else if (stat_iActiveLineNumber == 4)
            {
                bSettingSaved = standardCode.SaveZoomValue(SqlConnectionString, "VIS_ZOOML4", "" + DisplayPages.MIL1A.sMatrixTransformValue);
            }
            else if (stat_iActiveLineNumber == 5)
            {
                bSettingSaved = standardCode.SaveZoomValue(SqlConnectionString, "VIS_ZOOML5", "" + DisplayPages.MIL1B.sMatrixTransformValue);
            }
            //else if (stat_iActiveLineNumber == 6)
            //{
            //    bSettingSaved = standardCode.SaveZoomValue(SqlConnectionString, "VIS_ZOOML6", "" + DisplayPages.MIL1B.sMatrixTransformValue);
            //}
            if (bSettingSaved)
            {
                MessageBox.Show("The zoom setting has been saved.", "Setting Saved", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("The zoom setting was not saved.", "Saved Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnTaglinks_Click(object sender, RoutedEventArgs e)
        {
            //
            //CHECK WHICH GRID TO USE!------------------------------------------------------------------------------------------------------------TODO********************************************
            //
            if (iActiveLineNumber == 1)
            {
                DisplayPages.DisplayWindows.TagLinks tl = new DisplayPages.DisplayWindows.TagLinks("pageINT1", pageINT1.grid1, SqlConnectionString);
                tl.Show();
            }
            else if (iActiveLineNumber == 2)
            {
                DisplayPages.DisplayWindows.TagLinks tl = new DisplayPages.DisplayWindows.TagLinks("pageFCL1", pageFCL1.grid1, SqlConnectionString);
                tl.Show();
            }
            else if (iActiveLineNumber == 3)
            {
                DisplayPages.DisplayWindows.TagLinks tl = new DisplayPages.DisplayWindows.TagLinks("pageMTR1", pageMTR1.grid1, SqlConnectionString);
                tl.Show();
            }
            else if (iActiveLineNumber == 4)
            {
                DisplayPages.DisplayWindows.TagLinks tl = new DisplayPages.DisplayWindows.TagLinks("pageMIL1", pageMIL1.grid1, SqlConnectionString);
                tl.Show();
            }
            else if (iActiveLineNumber == 5)
            {
                DisplayPages.DisplayWindows.TagLinks tl = new DisplayPages.DisplayWindows.TagLinks("pageMIL2", pageMIL2.grid1, SqlConnectionString);
                tl.Show();
            }
         
          
        }

        private void btnErrors_Click(object sender, RoutedEventArgs e)
        {
            DisplayPages.DisplayWindows.ErrorList el = new DisplayPages.DisplayWindows.ErrorList(errorList);
            el.Show();
        }

        /// <summary>
        /// This method hides all the line buttons on the MainWindow
        /// </summary>
        public void HideLineButtons()
        {

            FrameworkElement feGridAsParent = (FrameworkElement)WindowMain;
            IEnumerable children = LogicalTreeHelper.GetChildren(feGridAsParent);   //Gets all Children of the Grid

            foreach (object child in children)
            {
                if (child is FrameworkElement)
                {
                    FrameworkElement Control = (FrameworkElement)child;

                    if (Control.Name.ToLower().Contains("grid"))
                    {

                        FrameworkElement feCurrentChildGridAsParent = (FrameworkElement)Control;
                        IEnumerable currentGridChildren = LogicalTreeHelper.GetChildren(feCurrentChildGridAsParent);   //Gets all Children of the Current Child Grid

                        foreach (object currentChild in currentGridChildren)
                        {
                            if (currentChild is FrameworkElement)
                            {
                                FrameworkElement currentControl = (FrameworkElement)currentChild;

                                if (currentControl.Name.ToLower().Contains("btnline"))
                                {
                                    currentControl.Visibility = Visibility.Hidden;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void HideControlsOnException()
        {
            btnLogOn.IsEnabled = false;
            btnLogOff.IsEnabled = false;
            btnSettings.IsEnabled = false;
            btnRestart.IsEnabled = true;
            image1.IsEnabled = false;
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = false;
            txtAlarms.IsEnabled = false;
            btnHelp.Visibility = Visibility.Hidden;
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                MessageBoxResult mbr = MessageBox.Show("This action will restart the application. Continue?", "Restart Application?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (mbr == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (SqlConnection sqlConn = new SqlConnection(MainWindow.SqlConnectionString))
                        {
                            sqlConn.Open();
                            SqlCommand cmd = sqlConn.CreateCommand();

                            string sInsert = "Application Restarted (" + MainWindow.stat_sLoggedInUser + ")";

                            cmd.CommandText = "INSERT INTO ApplicationLog VALUES ('" + DateTime.Now + "'," + DateTime.Now.ToOADate() + ",'" + btnRestart.Name + "','" + sInsert + "',30)";
                            int rowsInserted = cmd.ExecuteNonQuery();
                            sqlConn.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Log Restart --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("App Restart --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //private void btnLine5_Click(object sender, RoutedEventArgs e)
        //{

        //}


    }
}
