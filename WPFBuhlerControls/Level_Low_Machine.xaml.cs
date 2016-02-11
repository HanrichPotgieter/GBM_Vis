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
    /// Interaction logic for Level_Low_Machine.xaml
    /// </summary>
    public partial class Level_Low_Machine : UserControl
    {
        private int _LevelColor;
        private string DescriptionLowLevel;
        private string StatusLowLevel;
        private bool FaultLowLevel;
        private string _ObjectNo;
        private string _PLCName;

        public Level_Low_Machine()
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
                FB14 LL = new FB14();
                _SetColor(LL.SetColor(value, 7170));
                StatusLowLevel = LL.Status_DI_Status;
                FaultLowLevel = LL.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public string Description_LowLevel
        {
            get
            {
                return this.DescriptionLowLevel;
            }
            set
            {
                this.DescriptionLowLevel = value;
            }
        }

        [Category("Buhler")]
        public string Status_LowLevel
        {
            get
            {
                return this.StatusLowLevel;
            }
        }

        [Category("Buhler")]
        public bool Fault_LowLevel
        {
            get
            {
                return this.FaultLowLevel;
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
            RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectMain.Fill = brushColor;
            }));
        }
    }
}
