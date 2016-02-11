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
using System.ComponentModel;
using WPFBuhlerControls.FB_Code;

namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for DeadBox.xaml
    /// </summary>
    public partial class DeadBox : UserControl
    {
        private int _LevelColor;
        private string DescriptionHighLevel;
        private string StatusHighLevel;
        private bool FaultHighLevel;
        private string _ObjectNo;
        private string _PLCName;

        public DeadBox()
        {
            InitializeComponent();
            PolyMain.Fill = KNEKTColors.NoControlColor;
        }

        //------------------------------------------------------------------------------//
        //                                  Methods                                     //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int LevelColor
        {
            get
            {
                return _LevelColor;
            }
            set
            {
                _LevelColor = value;
                FB14 HL = new FB14();
                _SetColor(HL.SetColor(value, 7165));
                StatusHighLevel = HL.Status_DI_Status;
                FaultHighLevel = HL.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public string Description_HighLevel
        {
            get
            {
                return this.DescriptionHighLevel;
            }
            set
            {
                this.DescriptionHighLevel = value;
            }
        }

        [Category("Buhler")]
        public string Status_HighLevel
        {
            get
            {
                return this.StatusHighLevel;
            }
        }

        [Category("Buhler")]
        public bool Fault_HighLevel
        {
            get
            {
                return this.FaultHighLevel;
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
        //                                  Methods                                     //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectMain.Fill = brushColor;
            }));
        }     
    }
}
