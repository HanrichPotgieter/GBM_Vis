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
    /// Interaction logic for Level_Mid.xaml
    /// </summary>
    public partial class Level_Mid : UserControl
    {
        //private int _LevelColor;
        private string DescriptionMidLevel;
        private string StatusMidLevel;
        private bool FaultMidLevel;

        public Level_Mid()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set the color of the motor
        /// </summary>
        /// <param name="StateCode">State Code of the element</param>
        [Category("Buhler")]
        public void SetColor(int StateCode, int PType)
        {
            FB14 ML = new FB14();
            _SetColor(ML.SetColor(StateCode, PType));
            StatusMidLevel = ML.Status_DI_Status;
            FaultMidLevel = ML.Fault_DI_Element;
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

        //------------------------------------------------------------------------------//
        //                                    Methods                                   //
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
