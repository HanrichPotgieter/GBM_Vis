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
    /// Interaction logic for Conveyor_LimitSwitch.xaml
    /// </summary>
    public partial class Conveyor_LimitSwitch : UserControl
    {
        private int _LimitColor;
        private string DescriptionLimit;
        private string StatusLimit;
        private bool FaultLimit;
        private string _ObjectNo;
        private string _PLCName;

        public Conveyor_LimitSwitch()
        {
            InitializeComponent();
        }



        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int LimitColor
        {
            get
            {
                return this._LimitColor;
            }
            set
            {
                this._LimitColor = value;
                FB14 oFlow = new FB14();
                _SetColor(oFlow.SetColor(value, 7167));
                StatusLimit = oFlow.Status_DI_Status;
                FaultLimit = oFlow.Fault_DI_Element;
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
        public string Description_LimitSwitch
        {
            get
            {
                return this.DescriptionLimit;
            }
            set
            {
                this.DescriptionLimit = value;
            }
        }

        [Category("Buhler")]
        public string Status_LimitSwitch
        {
            get
            {
                return this.StatusLimit;
            }
        }

        [Category("Buhler")]
        public bool Fault_LimitSwitch
        {
            get
            {
                return this.FaultLimit;
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
