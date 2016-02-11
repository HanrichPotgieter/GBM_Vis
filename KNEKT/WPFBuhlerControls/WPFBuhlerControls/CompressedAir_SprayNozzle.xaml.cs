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
    /// Interaction logic for CompressedAir_SprayNozzle.xaml
    /// </summary>
    public partial class CompressedAir_SprayNozzle : UserControl
    {
        private int _NozzleColor;
        private string DescriptionNozzle;
        private string StatusNozzle;
        private bool FaultNozzle;
        private string _ObjectNo;
        private string _PLCName;

        public CompressedAir_SprayNozzle()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//
        [Category("Buhler")]
        public int NozzleColor
        {
            get
            {
                return _NozzleColor;
            }
            set
            {
                _NozzleColor = value;
                FB11 output = new FB11();
                _SetColor(output.SetColor(value));
                StatusNozzle = output.Status_DO_Element;
                FaultNozzle = output.Fault_DO_Element;
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
        public string Description_Nozzle
        {
            get
            {
                return this.DescriptionNozzle;
            }
            set
            {
                this.DescriptionNozzle = value;
            }
        }

        [Category("Buhler")]
        public string Status_Nozzle
        {
            get
            {
                return this.StatusNozzle;
            }
        }

        [Category("Buhler")]
        public bool Fault_Nozzle
        {
            get
            {
                return this.FaultNozzle;
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
        //                                Methods                                       //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            ElipseMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                ElipseMain.Fill = brushColor;
            }));
        }
    }
}
