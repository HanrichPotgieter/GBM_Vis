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

namespace KNEKT.Controls
{
    /// <summary>
    /// Interaction logic for FlowbalancerInfo.xaml
    /// </summary>
    public partial class FlowbalancerInfo : UserControl
    {
        private string _InFlowrateAddress = "DB0.DBD0";
        private Controller PLC_W;


        //------------------------------------------------------------------------------//
        //                                 Constructor                                  //
        //------------------------------------------------------------------------------//

        public FlowbalancerInfo()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//

        public string FlowbalancerInfo_Title
        {
            get
            {
                return lblTitle.Content.ToString();
            }
            set
            {
                lblTitle.Content = value;
            }
        }


        public Visibility Flowbalancer_HeaderVisibiliy
        {
            get
            {
                return lblHeaderInFlowrate.Visibility;
            }
            set
            {
                lblHeaderInFlowrate.Visibility = value;
                lblHeaderOutFlowrate.Visibility = value;
                lblHeaderAlarmNo.Visibility = value;
            }
        }


        public string InFlowrateOffset
        {
            get
            {
                return this._InFlowrateAddress;
            }
            set
            {
                this._InFlowrateAddress = value;
            }
        }

        public Controller Controller_W
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


        //------------------------------------------------------------------------------//
        //                              Button Clicks                                   //
        //------------------------------------------------------------------------------//

        private void lblInFlowrate_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bool bOpen = Classes.StandardCode.IsSpecificWindowOpen(Application.Current.Windows,this.lblTitle.Content.ToString());

            if (!bOpen)
            {
                DisplayWindows.NumericKeypad numKeypad = new DisplayWindows.NumericKeypad(lblInFlowrate.Content.ToString(), this.lblTitle.Content.ToString(), Controller_W, InFlowrateOffset);
                numKeypad.Show();
            }
        }
    }
}
