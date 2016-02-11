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
    /// Interaction logic for SampleCollector1.xaml
    /// </summary>
    public partial class SampleCollector1 : UserControl
    {
        private int _SamplerColor;
        private string DescriptionSampler;
        private string StatusSampler;
        private bool FaultSampler;
        private string _ObjectNo;
        private string _PLCName;

        public SampleCollector1()
        {
            InitializeComponent();
        }

        [Category("Buhler")]
        public int SamplerColor
        {
            get
            {
                return _SamplerColor;
            }
            set
            {
                _SamplerColor = value;
                FB29 sampler = new FB29();
                _SetColorMotor(sampler.SetColor(value));
                StatusSampler = sampler.Status_Sampler;
                FaultSampler = sampler.Fault_Sampler;
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
        public string Description_Sampler
        {
            get
            {
                return this.DescriptionSampler;
            }
            set
            {
                this.DescriptionSampler = value;
            }
        }

        [Category("Buhler")]
        public string Status_Sampler
        {
            get
            {
                return this.StatusSampler;
            }
        }

        [Category("Buhler")]
        public bool Fault_Sampler
        {
            get
            {
                return this.FaultSampler;
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

        private void _SetColorMotor(Brush brushColor)
        {
            Polymain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                Polymain.Fill = brushColor;
            }));
        }
    }
}
