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
    /// Interaction logic for DustDetector1.xaml
    /// </summary>
    public partial class DustDetector1 : UserControl
    {
      
        private int _DustDetectorColor;
        private string DescriptionDustDetector;
        private string StatusDustDetector;
        private bool FaultDustDetector;
        private string _ObjectNo;
        private string _PLCName;
 
        public DustDetector1()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Set the color of the motor
        /// </summary>
        /// <param name="StateCode">State Code of the element</param>
        /// 
        //------------------------------------------------------------------------------//
        //                                   Properties                                 //
        //------------------------------------------------------------------------------//

        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int DustDetectorColor
        {
            get
            {
                return this._DustDetectorColor;
            }
            set
            {
                this._DustDetectorColor = value;
                FB14 dDetector = new FB14();
                _SetColor(dDetector.SetColor(value, 7154));
                StatusDustDetector = dDetector.Status_DI_Status;
                FaultDustDetector = dDetector.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public string Description_DustDetector
        {
            get
            {
                return this.DescriptionDustDetector;
            }
            set
            {
                this.DescriptionDustDetector = value;
            }
        }

        [Category("Buhler")]
        public string Status_DustDetector
        {
            get
            {
                return this.StatusDustDetector;
            }
        }

       
        [Category("Buhler")]
        public eSwitchType SwitchType
        {
            get;
            set;
        }

        [Category("Buhler")]
        public bool Fault_DustDetector
        {
            get
            {
                return this.FaultDustDetector;
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
        //                                     Methods                                  //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            elipseMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                elipseMain.Fill = brushColor;
            }));
        }  
    }
}
