﻿using System;
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


namespace KNEKT.DisplayPages
{
    /// <summary>
    /// Interaction logic for MTR1.xaml
    /// </summary>
    public partial class MTR1 : Page
    {
        public static string sMatrixTransformValue;
        MatrixTransform xform;
        Controller PLCW;
    

        public MTR1(Controller PLC_W)
        {
            InitializeComponent();

            this.DataContext = new KNEKT.Classes.Products.ProductsViewModel();

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
            //uiScaleSlider.Visibility = Visibility.Visible;
            //ScaleTransform st = new ScaleTransform(uiScaleSlider.Value, uiScaleSlider.Value);
            //grid1.LayoutTransform = st;
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

            textLabelA0115.Visibility = v1;
            textLabelA0120.Visibility = v1;
            textLabelA0125.Visibility = v1;
            textLabelA0130.Visibility = v1;
            textLabelA0135.Visibility = v1;
            textLabelA0140.Visibility = v1;
            textLabelA0145.Visibility = v1;
            textLabelA0150.Visibility = v1;
            textLabelA0185.Visibility = v1;
            textLabelA0190.Visibility = v1;
            textLabelA0620.Visibility = v1;
            textLabelA0635.Visibility = v1;
            textLabelA0650.Visibility = v1;
            textLabelA0665.Visibility = v1;
            
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
                //MessageBox.Show("Zoom settings not loaded! \n\nValue : " + sMatrixTransformValue.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void _A0635_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0635.Status_HighLevel;
            MainWindow.sElementDescription = _A0635.Description_HighLevel;
            MainWindow.sActiveControlName = "_A0635";
        }

        private void _A0650_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0650.Status_Scale;
            MainWindow.sElementDescription = _A0650.Description_Scale;
            MainWindow.sActiveControlName = "_A0650";
        }

        private void _A0665_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0665.Status_Conveyor;
            MainWindow.sElementDescription = _A0665.Description_Conveyor;
            MainWindow.sActiveControlName = "_A0665";
        }

        private void _A0665_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0665.ObjectNumber;
        }

        private void _A0185_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0185.Status_Motor;
            MainWindow.sElementDescription = _A0185.Status_Motor;
            MainWindow.sActiveControlName = "_A0185";
        }

        private void _A0185_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0185.ObjectNumber;
        }

        private void _A0190_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0190.Status_Motor;
            MainWindow.sElementDescription = _A0190.Status_Motor;
            MainWindow.sActiveControlName = "_A0190";
        }

        private void _A0190_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0190.ObjectNumber;
        }

        private void _A0115_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0115.Status_HighLevel;
            MainWindow.sElementDescription = _A0115.Description_HighLevel;
            MainWindow.sActiveControlName = "_A0115";
        }

        private void _A0120_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0120.Status_HighLevel;
            MainWindow.sElementDescription = _A0120.Description_HighLevel;
            MainWindow.sActiveControlName = "_A0120";
        }

        private void _A0125_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0125.Status_Flowbalancer;
            MainWindow.sElementDescription = _A0125.Description_Flowbalancer;
            MainWindow.sActiveControlName = "_A0125";
        }

        private void _A0130_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0130.Status_Flowbalancer;
            MainWindow.sElementDescription = _A0130.Description_Flowbalancer;
            MainWindow.sActiveControlName = "_A0130";
        }

        private void _A0135_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0135.Status_Conveyor;
            MainWindow.sElementDescription = _A0135.Description_Conveyor;
            MainWindow.sActiveControlName = "_A0135";
        }

        private void _A0135_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0135.ObjectNumber;
        }

        private void _A0135F_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0135F.Status_Overflow;
            MainWindow.sElementDescription = _A0135F.Description_Overflow;
            MainWindow.sActiveControlName = "_A0135F";
        }

        private void _A0140_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.sElementState = _A0140.Status_Motor;
            MainWindow.sElementDescription = _A0140.Status_Motor;
            MainWindow.sActiveControlName = "_A0140";
        }

        private void _A0140_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.isValve = false;
            MainWindow.stat_sActiveObjectNo = _A0140.ObjectNumber;
        }

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


    }
}

        //---------