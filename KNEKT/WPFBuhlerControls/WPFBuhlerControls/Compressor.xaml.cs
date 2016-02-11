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


namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for Compressor.xaml
    /// </summary>
    public partial class Compressor : UserControl
    {
        private int _CompressorColor;
        private string DescriptionCompressor;
        private string StatusCompressor;
        private bool FaultCompressor;
        private string _ObjectNo;
        private string _PLCName;

        public Compressor()
        {
            InitializeComponent();            
        }

        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int CompressorColor
        {
            get
            {
                return _CompressorColor;
            }
            set
            {
                _CompressorColor = value;
                Brush b = Brushes.Transparent;

                if (value == 3 || value == 515)
                {
                    b = KNEKTColors.Gray;
                    StatusCompressor = "Healthy";
                    FaultCompressor = false;
                }
                else
                {
                    b = KNEKTColors.Red;
                    StatusCompressor = "Fault";
                    FaultCompressor = true;
                }
                _SetMotorColor(b);                            
            }
        }

        [Category("Buhler")]
        public string Description_Compressor
        {
            get
            {
                return this.DescriptionCompressor;
            }
            set
            {
                this.DescriptionCompressor = value;
            }
        }

        [Category("Buhler")]
        public string Status_Compressor
        {
            get
            {
                return this.StatusCompressor;
            }
        }

        [Category("Buhler")]
        public bool Fault_Compressor
        {
            get
            {
                return this.FaultCompressor;
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

        //------------------------------------------------------------------------------//
        //                                   Methods                                    //
        //------------------------------------------------------------------------------//
        private void _SetMotorColor(Brush brushColor)
        {
            elipseMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                elipseMain.Fill = brushColor;
            }));

            polyTank.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyTank.Fill = brushColor;
            }));           
        }


    }
}
