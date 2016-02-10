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
using WPFBuhlerControls.FB_Code;
using System.ComponentModel;

namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for Flowbalancer.xaml
    /// </summary>
    public partial class Flowbalancer : UserControl
    {
        private int _FlowbalancerColor;
        private string DescriptionFlowBalancer;
        private string StatusFlowBalancer;
        private bool FaultFlowBalancer;
        private string _ObjectNo;
        private string _PLCName;

        public Flowbalancer()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                   Properties                                 //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int FlowbalancerColor
        {
            get
            {
                return _FlowbalancerColor;
            }
            set
            {
                _FlowbalancerColor = value;
                FB28 FB = new FB28();
                _SetColor(FB.SetColor(value));
                StatusFlowBalancer = FB.Status_Flowbalancer;
                FaultFlowBalancer = FB.Fault_Flowbalancer;
            }
        }

        [Category("Buhler")]
        public string Description_Flowbalancer
        {
            get
            {
                return this.DescriptionFlowBalancer;
            }
            set
            {
                this.DescriptionFlowBalancer = value;
            }
        }

        [Category("Buhler")]
        public string Status_Flowbalancer
        {
            get
            {
                return this.StatusFlowBalancer;
            }
        }

        [Category("Buhler")]
        public bool Fault_Flowbalancer
        {
            get
            {
                return this.FaultFlowBalancer;
            }
        }


        [Category("Buhler")]
        public string ObjectNumber
        {
            get
            {
                return this._ObjectNo;
            }
            set
            {
                this._ObjectNo = value;
            }
        }
        
        [Category("Buhler")]
        public string PLCName
        {
            get
            {
                return this._PLCName;
            }
            set
            {
                this._PLCName = value;
            }
        }

        //------------------------------------------------------------------------------//
        //                                    Methods                                   //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectMain.Fill = brushColor;
            }));

            RectSmall.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectSmall.Fill = brushColor;
            }));

            PolyBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                PolyBot.Fill = brushColor;
            }));
        }
    }
}
