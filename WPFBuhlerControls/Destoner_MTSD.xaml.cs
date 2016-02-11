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
    /// Interaction logic for Destoner_MTSD.xaml
    /// </summary>
    public partial class Destoner_MTSD : UserControl
    {
        private int _MotorColor;
        private string DescriptionDestoner;
        private string StatusDestoner;
        private bool FaultDestoner;
        private string _ObjectNo;
        private string _PLCName;

        public Destoner_MTSD()
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
                FB12 motor = new FB12();
                _SetColor(motor.SetColor(value));
                StatusDestoner = motor.Status_Motor;
                FaultDestoner = motor.Fault_Motor;
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
        public string Description_Destoner
        {
            get
            {
                return this.DescriptionDestoner;
            }
            set
            {
                this.DescriptionDestoner = value;
            }
        }

        [Category("Buhler")]
        public string Status_Destoner
        {
            get
            {
                return this.StatusDestoner;
            }
        }

        [Category("Buhler")]
        public bool Fault_Destoner
        {
            get
            {
                return this.FaultDestoner;
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
        //                                   Methods                                    //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
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
