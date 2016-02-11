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
    /// Interaction logic for Valve_WaterShutoff.xaml
    /// </summary>
    public partial class Valve_WaterShutoff : UserControl
    {
        private int _VavleColor;
        private string DescriptionVavle;
        private string StatusVavle;
        private bool FaultVavle;
        private string _ObjectNo;
        private string _PLCName;

        public Valve_WaterShutoff()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int VavleColor
        {
            get
            {
                return _VavleColor;
            }
            set
            {
                _VavleColor = value;
                FB14 Vavle = new FB14();
                _SetColor(Vavle.SetColor(value, 7147));
                StatusVavle = Vavle.Status_DI_Status;
                FaultVavle = Vavle.Fault_DI_Element;
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
        public string Description_Vavle
        {
            get
            {
                return this.DescriptionVavle;
            }
            set
            {
                this.DescriptionVavle = value;
            }
        }

        [Category("Buhler")]
        public string Status_Vavle
        {
            get
            {
                return this.StatusVavle;
            }
        }

        [Category("Buhler")]
        public bool Fault_Vavle
        {
            get
            {
                return this.FaultVavle;
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
            polyTriLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyTriLeft.Fill = brushColor;
            }));

            polyTriRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyTriRight.Fill = brushColor;
            }));
          
        }
    }
}
