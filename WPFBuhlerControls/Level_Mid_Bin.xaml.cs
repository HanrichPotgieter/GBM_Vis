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
    /// Interaction logic for Level_Mid_Bin.xaml
    /// </summary>
    public partial class Level_Mid_Bin : UserControl
    {
        private int _LevelColor;
        private string DescriptionMidLevel;
        private string StatusMidLevel;
        private bool FaultMidLevel;
        private string _ObjectNo;
        private string _PLCName;

        public Level_Mid_Bin()
        {
            InitializeComponent();
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
                FB14 ML = new FB14();
                _SetColor(ML.SetColor(value, 7146));
                StatusMidLevel = ML.Status_DI_Status;
                FaultMidLevel = ML.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public string Description_MidLevel
        {
            get
            {
                return this.DescriptionMidLevel;
            }
            set
            {
                this.DescriptionMidLevel = value;
            }
        }

        [Category("Buhler")]
        public string Status_MidLevel
        {
            get
            {
                return this.StatusMidLevel;
            }
        }

        [Category("Buhler")]
        public bool Fault_MidLevel
        {
            get
            {
                return this.FaultMidLevel;
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
