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
using KNEKT.Classes;
using S7Link;


namespace KNEKT.DisplayPages
{
    /// <summary>
    /// Interaction logic for MIL1A.xaml
    /// </summary>
    public partial class MIL1A : Page
    {
        public static string sMatrixTransformValue;
        MatrixTransform xform;
        Controller PLCW;

        public MIL1A(Controller PLC_W)
        {
            InitializeComponent();

            SetShowTagnamesVisibility();
            PLCW = PLC_W;
            //
            // Check screen type for zooming settings
            //
            //if (MainWindow.stat_bMultitouchS1) //Multitouch is enabled
            //{
            //    uiScaleSlider.Visibility = Visibility.Hidden;
            //}
            //else
            //{
            uiScaleSlider.Visibility = Visibility.Visible;
            ScaleTransform st = new ScaleTransform(uiScaleSlider.Value, uiScaleSlider.Value);
            grid1.LayoutTransform = st;
            //}
        }

        //------------------------------------------------------------------------------//
        //                               Tag Labels Visibility                          //
        //------------------------------------------------------------------------------// 

        /// <summary>
        /// Show or hide visibility of tagname boxes
        /// </summary>
        public void SetShowTagnamesVisibility()
        {
            Visibility v1 = MainWindow.bShowTagnames == true ? Visibility.Visible : Visibility.Hidden;

            //textLabelA1000.Visibility = v1;



        }


        //------------------------------------------------------------------------------//
        //                                  Pinch To Zoom                               //
        //------------------------------------------------------------------------------// 

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] values = sMatrixTransformValue.Split(',');

                xform = new MatrixTransform(double.Parse(values[0]), double.Parse(values[1]), double.Parse(values[2]), double.Parse(values[3]), double.Parse(values[4]), double.Parse(values[5]));
                grid1.RenderTransform = xform;
            }
            catch
            {
                MessageBox.Show("Zoom settings not loaded! \n\nValue : " + sMatrixTransformValue.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnManipulationStarting(ManipulationStartingEventArgs args)
        {
            args.ManipulationContainer = this;
            args.Handled = true;
            base.OnManipulationStarting(args);
        }

        protected override void OnManipulationDelta(ManipulationDeltaEventArgs args)
        {
            UIElement element = args.Source as UIElement;
            xform = element.RenderTransform as MatrixTransform;
            Matrix matrix = xform.Matrix;
            ManipulationDelta delta = args.DeltaManipulation;
            Point center = args.ManipulationOrigin;

            matrix.ScaleAt(delta.Scale.X, delta.Scale.Y, center.X, center.Y);
            //matrix.RotateAt(delta.Rotation, center.X, center.Y);
            matrix.Translate(delta.Translation.X, delta.Translation.Y);
            xform.Matrix = matrix;

            sMatrixTransformValue = "" + matrix;

            args.Handled = true;
            base.OnManipulationDelta(args);
        }

        //------------------------------------------------------------------------------//
        //                                Manual Start Stop                             //
        //------------------------------------------------------------------------------// 

        public void ManualStart(string ObjectNumber, bool SwitchOn)
        {
            if (MainWindow.stat_iUserLevel >= 2)
            {
                try
                {
                    if (!PLCW.IsConnected)
                        PLCW.Connect();

                    if (PLCW.IsConnected)
                    {
                        Tag tObjNo = new Tag("DB3.DBD50", S7Link.Tag.ATOMIC.DWORD, 1);
                        Tag tCmdOn = new Tag("DB3.DBX55.1", S7Link.Tag.ATOMIC.BOOL, 1);
                        Tag tCmdOff = new Tag("DB3.DBX55.0", S7Link.Tag.ATOMIC.BOOL, 1);
                        Tag tCmdManOff = new Tag("DB3.DBX57.5", S7Link.Tag.ATOMIC.BOOL, 1);
                        Tag tCmdManOn = new Tag("DB3.DBX55.5", S7Link.Tag.ATOMIC.BOOL, 1);
                        Tag tReqDefine = new Tag("DB3.DBX43.0", S7Link.Tag.ATOMIC.BOOL, 1);
                        Tag tCmdHN = new Tag("DB3.DBX55.1", S7Link.Tag.ATOMIC.BOOL, 1);

                        if (SwitchOn)
                        {
                            tObjNo.Value = ObjectNumber;
                            tCmdOn.Value = true;
                            tCmdManOn.Value = true;
                            tCmdOff.Value = false;
                            tCmdManOff.Value = false;
                            tReqDefine.Value = true;
                        }
                        else
                        {
                            tObjNo.Value = ObjectNumber;
                            tCmdOn.Value = false;
                            tCmdManOn.Value = false;
                            tCmdOff.Value = true;
                            tCmdManOff.Value = true;
                            tReqDefine.Value = true;
                        }


                        TagGroup tg1 = new TagGroup();
                        tg1.AddTag(tObjNo);
                        tg1.AddTag(tCmdOn);
                        tg1.AddTag(tCmdManOn);
                        tg1.AddTag(tCmdOff);
                        tg1.AddTag(tCmdManOff);
                        tg1.AddTag(tReqDefine);

                        PLCW.GroupWrite(tg1);
                        PLCW.Disconnect();
                        MainWindow.stat_sActiveObjectNo = "";
                    }
                    else
                    {
                        MessageBox.Show("Not Connected");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Manual Start --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("You do not have sufficient privileges to perform this action! ", " access denied", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        public void ManualStartValve(string ObjectNumber, bool SwitchOn)
        {
            if (MainWindow.stat_iUserLevel >= 2)
            {
                try
                {
                    if (!PLCW.IsConnected)
                        PLCW.Connect();

                    if (PLCW.IsConnected)
                    {
                        Tag tObjNo = new Tag("DB3.DBD50", S7Link.Tag.ATOMIC.DWORD, 1);
                        Tag tCmdOn = new Tag("DB3.DBX55.1", S7Link.Tag.ATOMIC.BOOL, 1);
                        Tag tCmdOff = new Tag("DB3.DBX55.0", S7Link.Tag.ATOMIC.BOOL, 1);
                        Tag tCmdManOff = new Tag("DB3.DBX57.5", S7Link.Tag.ATOMIC.BOOL, 1);
                        Tag tCmdManOn = new Tag("DB3.DBX55.5", S7Link.Tag.ATOMIC.BOOL, 1);
                        Tag tReqDefine = new Tag("DB3.DBX43.0", S7Link.Tag.ATOMIC.BOOL, 1);
                        Tag tCmdHN = new Tag("DB3.DBX55.1", S7Link.Tag.ATOMIC.BOOL, 1);
                        Tag tCmdLN = new Tag("DB3.DBX55.0", S7Link.Tag.ATOMIC.BOOL, 1);

                        if (SwitchOn)
                        {
                            tObjNo.Value = ObjectNumber;
                            tCmdOn.Value = true;
                            tCmdManOn.Value = true;
                            tCmdOff.Value = false;
                            tCmdManOff.Value = false;
                            tReqDefine.Value = true;


                        }
                        else
                        {
                            tObjNo.Value = ObjectNumber;
                            tCmdOn.Value = true;
                            tCmdManOn.Value = false;
                            tCmdOff.Value = false;
                            tCmdManOff.Value = true;
                            tReqDefine.Value = true;

                        }

                        if (MainWindow.isValve == true && SwitchOn)
                        {
                            tCmdHN.Value = true;
                            tCmdLN.Value = false;
                        }
                        else if (MainWindow.isValve == true && !SwitchOn)
                        {
                            tCmdManOn.Value = true;
                            tCmdManOff.Value = false;
                            tCmdLN.Value = true;
                            tCmdHN.Value = false;
                        }


                        TagGroup tg1 = new TagGroup();
                        tg1.AddTag(tObjNo);
                        tg1.AddTag(tCmdOn);
                        tg1.AddTag(tCmdManOn);
                        tg1.AddTag(tCmdOff);
                        tg1.AddTag(tCmdManOff);
                        tg1.AddTag(tReqDefine);
                        tg1.AddTag(tCmdHN);
                        tg1.AddTag(tCmdLN);

                        PLCW.GroupWrite(tg1);
                        PLCW.Disconnect();
                        MainWindow.stat_sActiveObjectNo = "";
                    }
                    else
                    {
                        MessageBox.Show("Not Connected");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Manual Start --> " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("You do not have sufficient privileges to perform this action! ", " access denied", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }


        private void contMen_CmdManOn_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.stat_sActiveObjectNo != "" && MainWindow.isValve == false)
                ManualStart(MainWindow.stat_sActiveObjectNo, true);
            if (MainWindow.stat_sActiveObjectNo != "" && MainWindow.isValve == true)
                ManualStartValve(MainWindow.stat_sActiveObjectNo, true);
        }

        private void contMen_CmdManOff_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.stat_sActiveObjectNo != "" && MainWindow.isValve == false)
                ManualStart(MainWindow.stat_sActiveObjectNo, false);
            if (MainWindow.stat_sActiveObjectNo != "" && MainWindow.isValve == true)
                ManualStartValve(MainWindow.stat_sActiveObjectNo, false);
        }

        //------------------------------------------------------------------------------//
        //                                Page Close Event                              //
        //------------------------------------------------------------------------------// 

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Classes.StandardCode.CloseAllOpenWindows(Application.Current.Windows);
        }

        private void uiScaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ScaleTransform st = new ScaleTransform(uiScaleSlider.Value, uiScaleSlider.Value);
            grid1.LayoutTransform = st;
        }


 //------------------------------------------------------------------------------//
        //                               Mouse / Touch Events                           //
        //------------------------------------------------------------------------------// 

        private void _A0145_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0145.Status_HighLevel;
            MainWindow.sElementDescription = _A0145.Description_HighLevel;
            MainWindow.sActiveControlName = "_A0145";
        }

        private void _A0150_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0150.Status_Scale;
            MainWindow.sElementDescription = _A0150.Description_Scale;
            MainWindow.sActiveControlName = "_A0150";
        }

        private void _A0165_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0165.Status_MOZF;
            MainWindow.sElementDescription = _A0165.Description_MOZF;
            MainWindow.sActiveControlName = "_A0165";
        }

        private void _A0160_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0160.Status_Conveyor;
            MainWindow.sElementDescription = _A0160.Description_Conveyor;
            MainWindow.sActiveControlName = "_A0160";
        }

        private void _A0160_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0160.ObjectNumber;
        }

        private void _A0160F_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0160F.Status_Overflow;
            MainWindow.sElementDescription = _A0160F.Description_Overflow;
            MainWindow.sActiveControlName = "_A0160F";
        }

        private void _A0170_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0170.Status_Conveyor;
            MainWindow.sElementDescription = _A0170.Description_Conveyor;
            MainWindow.sActiveControlName = "_A0170";
        }

        private void _A0170_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0170.ObjectNumber;
        }

        private void _A0170F_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0170.Status_Conveyor;
            MainWindow.sElementDescription = _A0170.Description_Conveyor;
            MainWindow.sActiveControlName = "_A0170";
        }

        private void _A0180_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0180.Status_MainMotor;
            MainWindow.sElementDescription = _A0180.Description_MainMotor;
            MainWindow.sActiveControlName = "_A0180";
        }

        private void _A0180_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0180.ObjectNumber1;
        }

        private void _A0510_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0510.Status_PressureSwitch;
            MainWindow.sElementDescription = _A0510.Description_PressureSwitch;
            MainWindow.sActiveControlName = "_A0510";
        }

        private void _A0305_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0305.Status_Motor;
            MainWindow.sElementDescription = _A0305.Description_Motor;
            MainWindow.sActiveControlName = "_A0305";
        }

        private void _A0305_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0305.ObjectNumber;
        }

        private void _A0305A1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0305A1.Status_Motor;
            MainWindow.sElementDescription = _A0305A1.Description_Motor;
            MainWindow.sActiveControlName = "_A0305A1";
        }

        private void _A0305A2_MouseDown(object sender, MouseButtonEventArgs e)
        {
         MainWindow.sElementState = _A0305A2.Status_Motor;
            MainWindow.sElementDescription = _A0305A2.Description_Motor;
            MainWindow.sActiveControlName = "_A0305A2";
        }

        private void _A0330_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0330.Status_Motor;
            MainWindow.sElementDescription = _A0330.Description_Motor;
            MainWindow.sActiveControlName = "_A0330";
        }

        private void _A0330_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0330.ObjectNumber;
        }

        private void _A0330A1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0330A1.Status_Motor;
            MainWindow.sElementDescription = _A0330A1.Description_Motor;
            MainWindow.sActiveControlName = "_A0330A1";
        }

        private void _A0330A2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0330A2.Status_Motor;
            MainWindow.sElementDescription = _A0330A2.Description_Motor;
            MainWindow.sActiveControlName = "_A0330A2";
        }

        private void _A0330A3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0330A3.Status_Motor;
            MainWindow.sElementDescription = _A0330A3.Description_Motor;
            MainWindow.sActiveControlName = "_A0330A3";
        }

        private void _A0330A4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0330A4.Status_Motor;
            MainWindow.sElementDescription = _A0330A4.Description_Motor;
            MainWindow.sActiveControlName = "_A0330A4";
        }

        private void _A0330A5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0330A5.Status_Motor;
            MainWindow.sElementDescription = _A0330A5.Description_Motor;
            MainWindow.sActiveControlName = "_A0330A5";
        }

        private void _A0355_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0355.Status_Motor;
            MainWindow.sElementDescription = _A0355.Description_Motor;
            MainWindow.sActiveControlName = "_A0355";
        }

        private void _A0355_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0355.ObjectNumber;
        }

        private void _A0355A1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0355A1.Status_Motor;
            MainWindow.sElementDescription = _A0355A1.Description_Motor;
            MainWindow.sActiveControlName = "_A0355A1";
        }

        private void _A0355A2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0355A2.Status_Motor;
            MainWindow.sElementDescription = _A0355A2.Description_Motor;
            MainWindow.sActiveControlName = "_A0355A2";
        }

        private void _A0355A3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0355A3.Status_Motor;
            MainWindow.sElementDescription = _A0355A3.Description_Motor;
            MainWindow.sActiveControlName = "_A0355A3";
        }

        private void _A0355A4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0355A4.Status_Motor;
            MainWindow.sElementDescription = _A0355A4.Description_Motor;
            MainWindow.sActiveControlName = "_A0355A4";
        }

        private void _A0425_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0425.Status_Motor;
            MainWindow.sElementDescription = _A0425.Description_Motor;
            MainWindow.sActiveControlName = "_A0425";
        }

        private void _A0425_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0425.ObjectNumber;
        }

        private void _A0425A1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0425A1.Status_Motor;
            MainWindow.sElementDescription = _A0425A1.Description_Motor;
            MainWindow.sActiveControlName = "_A0425A1";
        }

        private void _A0340_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0340.Status_Sifter;
            MainWindow.sElementDescription = _A0340.Description_Sifter;
            MainWindow.sActiveControlName = "_A0340";
        }

        private void _A0340_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0340.ObjectNumber;
        }

        private void _A0340SM01_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0340SM01.Status_Monitor;
            MainWindow.sElementDescription = _A0340SM01.Description_Monitor;
            MainWindow.sActiveControlName = "_A0340SM01";
        }

        private void _A0340SM02_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0340SM02.Status_Monitor;
            MainWindow.sElementDescription = _A0340SM02.Description_Monitor;
            MainWindow.sActiveControlName = "_A0340SM02";
        }

        private void _A0430_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0430.Status_Motor;
            MainWindow.sElementDescription = _A0430.Description_Motor;
            MainWindow.sActiveControlName = "_A0430";
        }

        private void _A0430_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0430.ObjectNumber;
        }

        private void _A0450_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0450.Status_Motor;
            MainWindow.sElementDescription = _A0450.Description_Motor;
            MainWindow.sActiveControlName = "_A0450";
        }

        private void _A0450_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0450.ObjectNumber;
        }

        private void _A0445_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0445.Status_Motor;
            MainWindow.sElementDescription = _A0445.Description_Motor;
            MainWindow.sActiveControlName = "_A0445";
        }

        private void _A0445_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0445.ObjectNumber;
        }

        private void _A0350_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0350.Status_Motor;
            MainWindow.sElementDescription = _A0350.Description_Motor;
            MainWindow.sActiveControlName = "_A0350";
        }

        private void _A0350_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0350.ObjectNumber;
        }

        private void _A0345_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0345.Status_Motor;
            MainWindow.sElementDescription = _A0345.Description_Motor;
            MainWindow.sActiveControlName = "_A0345";
        }

        private void _A0345_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0345.ObjectNumber;
        }

        private void _A0365_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0365.Status_Motor;
            MainWindow.sElementDescription = _A0365.Description_Motor;
            MainWindow.sActiveControlName = "_A0365";
        }

        private void _A0365_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0365.ObjectNumber;
        }

        private void _A0370_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0370.Status_Motor;
            MainWindow.sElementDescription = _A0370.Description_Motor;
            MainWindow.sActiveControlName = "_A0370";
        }

        private void _A0370_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0370.ObjectNumber;
        }

        private void _A0390_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0390.Status_Motor;
            MainWindow.sElementDescription = _A0390.Description_Motor;
            MainWindow.sActiveControlName = "_A0390";
        }

        private void _A0390_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0390.ObjectNumber;
        }

        private void _A0395_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0395.Status_Motor;
            MainWindow.sElementDescription = _A0395.Description_Motor;
            MainWindow.sActiveControlName = "_A0395";
        }

        private void _A0395_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0395.ObjectNumber;
        }

        private void _A0415_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0415.Status_Motor;
            MainWindow.sElementDescription = _A0415.Description_Motor;
            MainWindow.sActiveControlName = "_A0415";
        }

        private void _A0415_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0415.ObjectNumber;
        }

        private void _A0420_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0420.Status_Motor;
            MainWindow.sElementDescription = _A0420.Description_Motor;
            MainWindow.sActiveControlName = "_A0420";
        }

        private void _A0420_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0420.ObjectNumber;
        }

        private void _A0435_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0435.Status_Motor;
            MainWindow.sElementDescription = _A0435.Description_Motor;
            MainWindow.sActiveControlName = "_A0435";
        }

        private void _A0435_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0435.ObjectNumber;
        }

        private void _A0440_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0440.Status_Motor;
            MainWindow.sElementDescription = _A0440.Description_Motor;
            MainWindow.sActiveControlName = "_A0440";
        }

        private void _A0440_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0440.ObjectNumber;
        }

        private void _A0325_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0325.Status_PressureSwitch;
            MainWindow.sElementDescription = _A0325.Description_PressureSwitch;
            MainWindow.sActiveControlName = "_A0325";
        }

        private void _A0485_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0485.Status_HighLevel;
            MainWindow.sElementDescription = _A0485.Description_HighLevel;
            MainWindow.sActiveControlName = "_A0485";
        }

        private void _A0490_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0490.Status_Conveyor;
            MainWindow.sElementDescription = _A0490.Description_Conveyor;
            MainWindow.sActiveControlName = "_A0490";
        }

        private void _A0490_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0490.ObjectNumber;
        }

        private void _A0605_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0605.Status_Conveyor;
            MainWindow.sElementDescription = _A0605.Description_Conveyor;
            MainWindow.sActiveControlName = "_A0605";
        }

        private void _A0605_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0605.ObjectNumber;
        }

        private void _A0605F_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0605F.Status_Overflow;
            MainWindow.sElementDescription = _A0605F.Description_Overflow;
            MainWindow.sActiveControlName = "_A0605F";
        }

        private void _A0615_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0615.Status_Conveyor;
            MainWindow.sElementDescription = _A0615.Description_Conveyor;
            MainWindow.sActiveControlName = "_A0615";
        }

        private void _A0615_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0615.ObjectNumber;
        }

        private void _A0615F_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0615F.Status_Overflow;
            MainWindow.sElementDescription = _A0615F.Description_Overflow;
            MainWindow.sActiveControlName = "_A0615F";
        }

        private void _A0620_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0620.Status_Conveyor;
            MainWindow.sElementDescription = _A0620.Description_Conveyor;
            MainWindow.sActiveControlName = "_A0620";
        }

        private void _A0620_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0620.ObjectNumber;
        }

        private void _A0620F_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0620F.Status_Overflow;
            MainWindow.sElementDescription = _A0620F.Description_Overflow;
            MainWindow.sActiveControlName = "_A0620F";
        }

        private void _A0455_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0455.Status_Valve;
            MainWindow.sElementDescription = _A0455.Description_Valve;
            MainWindow.sActiveControlName = "_A0455";
        }

        private void _A0460_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0460.Status_Motor;
            MainWindow.sElementDescription = _A0460.Description_Motor;
            MainWindow.sActiveControlName = "_A0460";
        }

        private void _A0460_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0460.ObjectNumber;
        }

        private void _A0465_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0465.Status_Filter;
            MainWindow.sElementDescription = _A0465.Description_Filter;
            MainWindow.sActiveControlName = "_A0460";
        }

        private void _A0465_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0465.ObjectNumber;
        }

        private void _A0470_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0470.Status_Motor;
            MainWindow.sElementDescription = _A0470.Description_Motor;
            MainWindow.sActiveControlName = "_A0470";
        }

        private void _A0470_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0470.ObjectNumber;
        }

        private void _A0475_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0475.Status_Motor;
            MainWindow.sElementDescription = _A0475.Description_Motor;
            MainWindow.sActiveControlName = "_A0475";
        }

        private void _A0475_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0475.ObjectNumber;
        }

        private void _A0480_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0480.Status_Motor;
            MainWindow.sElementDescription = _A0480.Description_Motor;
            MainWindow.sActiveControlName = "_A0480";
        }

        private void _A0480_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0480.ObjectNumber;
        }

        private void _A0495_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0495.Status_Motor;
            MainWindow.sElementDescription = _A0495.Description_Motor;
            MainWindow.sActiveControlName = "_A0495";
        }

        private void _A0495_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0495.ObjectNumber;
        }

        private void _A0500_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0500.Status_Motor;
            MainWindow.sElementDescription = _A0500.Description_Motor;
            MainWindow.sActiveControlName = "_A0500";
        }

        private void _A0500_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0500.ObjectNumber;
        }

        private void _A0320_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            double dWidth = _A0320.Width;
            double dHeight = _A0320.Height;
            MouseButtonEventArgs mbea = (MouseButtonEventArgs)e;            //Get the point on the control where the mouse was clicked
            Point p = mbea.GetPosition(_A0320);

            ClickCoordinate cc = new ClickCoordinate(dWidth, dHeight, p);

            double mouseX = p.X;
            double halfWidth = cc.XControlHalf;                                  //Get half of the height and width
            double halfHeight = cc.YControlHalf;



            //Left
            if (mouseX < halfWidth)                                         //Check if the control was clicked to the left or the right of Center
            {
                if (mouseX > cc.XControlQuarter1)
                {
                    //Quarter 2
                    MainWindow.sElementDescription = _A0320.Description_FrequencyConverter1;
                    MainWindow.sElementState = _A0320.Status_FrequencyConverter1;
                }
                else
                {
                    //Quarter 1
                    MainWindow.sElementDescription = _A0320.Description_Motor1;
                    MainWindow.sElementState = _A0320.Status_Motor1;
                }
            }
            //Right
            else
            {
                if (mouseX < cc.XControlQuarter3)
                {
                    //Quarter 3
                    MainWindow.sElementDescription = _A0320.Description_FrequencyConverter2;
                    MainWindow.sElementState = _A0320.Status_FrequencyConverter2;
                }
                else
                {
                    //Quarter 4
                    MainWindow.sElementDescription = _A0320.Description_Motor2;
                    MainWindow.sElementState = _A0320.Status_Motor2;
                }
            }

            MainWindow.sActiveControlName = "_A0320";
        }

        private void _A0320_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            double dWidth = _A0320.Width;
            double dHeight = _A0320.Height;
            MouseButtonEventArgs mbea = (MouseButtonEventArgs)e;            //Get the point on the control where the mouse was clicked
            Point p = mbea.GetPosition(_A0320);

            ClickCoordinate cc = new ClickCoordinate(dWidth, dHeight, p);

            double mouseX = p.X;
            double halfWidth = cc.XControlHalf;                                  //Get half of the height and width
            double halfHeight = cc.YControlHalf;


            //Left
            if (mouseX < halfWidth)                                         //Check if the control was clicked to the left or the right of Center
            {
                if (mouseX < cc.XControlQuarter1)
                {
                    //Quarter 1
                    MainWindow.isValve = false;
                    MainWindow.stat_sActiveObjectNo = _A0320.ObjectNumber1;
                }
            }
            //Right
            else if (mouseX > halfWidth)
            {
                if (mouseX > cc.XControlQuarter4)
                {
                    //Quarter 4
                    MainWindow.isValve = false;
                    MainWindow.stat_sActiveObjectNo = _A0320.ObjectNumber2;
                }
            }
        }

        private void _A0380_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            double dWidth = _A0380.Width;
            double dHeight = _A0380.Height;
            MouseButtonEventArgs mbea = (MouseButtonEventArgs)e;            //Get the point on the control where the mouse was clicked
            Point p = mbea.GetPosition(_A0380);

            ClickCoordinate cc = new ClickCoordinate(dWidth, dHeight, p);

            double mouseX = p.X;
            double halfWidth = cc.XControlHalf;                                  //Get half of the height and width
            double halfHeight = cc.YControlHalf;



            //Left
            if (mouseX < halfWidth)                                         //Check if the control was clicked to the left or the right of Center
            {
                if (mouseX > cc.XControlQuarter1)
                {
                    //Quarter 2
                    MainWindow.sElementDescription = _A0380.Description_FrequencyConverter1;
                    MainWindow.sElementState = _A0380.Status_FrequencyConverter1;
                }
                else
                {
                    //Quarter 1
                    MainWindow.sElementDescription = _A0380.Description_Motor1;
                    MainWindow.sElementState = _A0380.Status_Motor1;
                }
            }
            //Right
            else
            {
                if (mouseX < cc.XControlQuarter3)
                {
                    //Quarter 3
                    MainWindow.sElementDescription = _A0380.Description_FrequencyConverter2;
                    MainWindow.sElementState = _A0380.Status_FrequencyConverter2;
                }
                else
                {
                    //Quarter 4
                    MainWindow.sElementDescription = _A0380.Description_Motor2;
                    MainWindow.sElementState = _A0380.Status_Motor2;
                }
            }

            MainWindow.sActiveControlName = "_A0380";
        }

        private void _A0405_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            double dWidth = _A0405.Width;
            double dHeight = _A0405.Height;
            MouseButtonEventArgs mbea = (MouseButtonEventArgs)e;            //Get the point on the control where the mouse was clicked
            Point p = mbea.GetPosition(_A0405);

            ClickCoordinate cc = new ClickCoordinate(dWidth, dHeight, p);

            double mouseX = p.X;
            double halfWidth = cc.XControlHalf;                                  //Get half of the height and width
            double halfHeight = cc.YControlHalf;



            //Left
            if (mouseX < halfWidth)                                         //Check if the control was clicked to the left or the right of Center
            {
                if (mouseX > cc.XControlQuarter1)
                {
                    //Quarter 2
                    MainWindow.sElementDescription = _A0405.Description_FrequencyConverter1;
                    MainWindow.sElementState = _A0405.Status_FrequencyConverter1;
                }
                else
                {
                    //Quarter 1
                    MainWindow.sElementDescription = _A0405.Description_Motor1;
                    MainWindow.sElementState = _A0405.Status_Motor1;
                }
            }
            //Right
            else
            {
                if (mouseX < cc.XControlQuarter3)
                {
                    //Quarter 3
                    MainWindow.sElementDescription = _A0405.Description_FrequencyConverter2;
                    MainWindow.sElementState = _A0405.Status_FrequencyConverter2;
                }
                else
                {
                    //Quarter 4
                    MainWindow.sElementDescription = _A0405.Description_Motor2;
                    MainWindow.sElementState = _A0405.Status_Motor2;
                }
            }

            MainWindow.sActiveControlName = "_A0405";
        }

        private void _A0405_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            double dWidth = _A0405.Width;
            double dHeight = _A0405.Height;
            MouseButtonEventArgs mbea = (MouseButtonEventArgs)e;            //Get the point on the control where the mouse was clicked
            Point p = mbea.GetPosition(_A0405);

            ClickCoordinate cc = new ClickCoordinate(dWidth, dHeight, p);

            double mouseX = p.X;
            double halfWidth = cc.XControlHalf;                                  //Get half of the height and width
            double halfHeight = cc.YControlHalf;


            //Left
            if (mouseX < halfWidth)                                         //Check if the control was clicked to the left or the right of Center
            {
                if (mouseX < cc.XControlQuarter1)
                {
                    //Quarter 1
                    MainWindow.isValve = false;
                    MainWindow.stat_sActiveObjectNo = _A0405.ObjectNumber1;
                }
            }
            //Right
            else if (mouseX > halfWidth)
            {
                if (mouseX > cc.XControlQuarter4)
                {
                    //Quarter 4
                    MainWindow.isValve = false;
                    MainWindow.stat_sActiveObjectNo = _A0405.ObjectNumber2;
                }
            }
        }

        private void _A0380_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            double dWidth = _A0380.Width;
            double dHeight = _A0380.Height;
            MouseButtonEventArgs mbea = (MouseButtonEventArgs)e;            //Get the point on the control where the mouse was clicked
            Point p = mbea.GetPosition(_A0380);

            ClickCoordinate cc = new ClickCoordinate(dWidth, dHeight, p);

            double mouseX = p.X;
            double halfWidth = cc.XControlHalf;                                  //Get half of the height and width
            double halfHeight = cc.YControlHalf;


            //Left
            if (mouseX < halfWidth)                                         //Check if the control was clicked to the left or the right of Center
            {
                if (mouseX < cc.XControlQuarter1)
                {
                    //Quarter 1
                    MainWindow.isValve = false;
                    MainWindow.stat_sActiveObjectNo = _A0380.ObjectNumber1;
                }
            }
            //Right
            else if (mouseX > halfWidth)
            {
                if (mouseX > cc.XControlQuarter4)
                {
                    //Quarter 4
                    MainWindow.isValve = false;
                    MainWindow.stat_sActiveObjectNo = _A0380.ObjectNumber2;
                }
            }
        }

        private void _A4012_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void _A4012_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void _A4014MA03_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void _A4014MA05_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void _A4014R03_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void _A4014R05_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void _A4015MA03_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void _A4015MA05_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void _A4015R03_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void _A4015R05_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void _A4016MA03_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void _A4016MA05_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void _A4016R03_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void _A4016R05_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}