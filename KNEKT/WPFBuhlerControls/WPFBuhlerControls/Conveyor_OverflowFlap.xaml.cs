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
    /// Interaction logic for Conveyor_OverflowFlap.xaml
    /// </summary>
    public partial class Conveyor_OverflowFlap : UserControl
    {
        private int _OverflowColor;
        private string DescriptionOverflow;
        private string StatusOverflow;
        private bool FaultOverflow;
        private string _ObjectNo;
        private string _PLCName;

        public Conveyor_OverflowFlap()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Set the color of the Overflow
        /// </summary>
        /// <param name="StateCode">State Code of the element</param>
        [Obsolete("SetColor is Obsolete. Please use OverFlowColor instead."), Category("Buhler")]        
        public void SetColor(int StateCode, int PType)
        {
            FB14 oFlow = new FB14();
            _SetColor(oFlow.SetColor(StateCode, PType));
            StatusOverflow = oFlow.Status_DI_Status;
            FaultOverflow = oFlow.Fault_DI_Element;
        }

        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int OverFlowColor
        {
            get
            {
                return this._OverflowColor;
            }
            set
            {
                this._OverflowColor = value;
                FB14 oFlow = new FB14();
                _SetColor(oFlow.SetColor(value, 7154));
                StatusOverflow = oFlow.Status_DI_Status;
                FaultOverflow = oFlow.Fault_DI_Element;
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

        //
        //  Overflow
        //
        [Category("Buhler")]
        public string Description_Overflow
        {
            get
            {
                return this.DescriptionOverflow;
            }
            set
            {
                this.DescriptionOverflow = value;
            }
        }

        [Category("Buhler")]
        public string Status_Overflow
        {
            get
            {
                return this.StatusOverflow;
            }
        }

        [Category("Buhler")]
        public bool Fault_Overflow
        {
            get
            {
                return this.FaultOverflow;
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
        //                                  Methods                                     //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {            
            RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectMain.Fill = brushColor;
            }));            
        }

    }
}
