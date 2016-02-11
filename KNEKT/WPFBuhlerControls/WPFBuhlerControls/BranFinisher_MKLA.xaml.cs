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
    /// Interaction logic for BranFinisher_MKLA.xaml
    /// </summary>
    public partial class BranFinisher_MKLA : UserControl
    {
        private int _MotorColor;
        private string DescriptionBranFinisher;
        private string StatusBranFinisher;
        private bool FaultBranFinisher;
        private string _ObjectNo;
        private string _PLCName;

        public BranFinisher_MKLA()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int MotorColor
        {
            get
            {
                return _MotorColor;
            }
            set
            {
                _MotorColor = value;
                FB12 branFinisher = new FB12();
                _SetColorBranFinisher(branFinisher.SetColor(value));
                StatusBranFinisher = branFinisher.Status_Motor;
                FaultBranFinisher = branFinisher.Fault_Motor;
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
        public string Description_BranFinisher
        {
            get
            {
                return this.DescriptionBranFinisher;
            }
            set
            {
                this.DescriptionBranFinisher = value;
            }
        }

        [Category("Buhler")]
        public string Status_BranFinisher
        {
            get
            {
                return this.StatusBranFinisher;
            }
        }

        [Category("Buhler")]
        public bool Fault_BranFinisher
        {
            get
            {
                return this.FaultBranFinisher;
            }
        }

        [Category("Buhler")]
        public string BranFinisher_Text
        {
            get
            {
                return txtBranFinisherName.Text;
            }
            set
            {
                txtBranFinisherName.Text = value;
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
        private void _SetColorBranFinisher(Brush brushColor)
        {
            RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectMain.Fill = brushColor;
            }));

            PolyMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                PolyMain.Fill = brushColor;
            }));           
        }
    }
}
