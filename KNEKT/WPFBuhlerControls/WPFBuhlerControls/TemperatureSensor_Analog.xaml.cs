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
    /// Interaction logic for TemperatureSensor_Analog.xaml
    /// </summary>
    public partial class TemperatureSensor_Analog : UserControl
    {
        private int _SensorColor;
        private string DescriptionSwitch;
        private string StatusSwitch;
        private bool FaultSwitch;


        public TemperatureSensor_Analog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set the color of the motor
        /// </summary>
        /// <param name="StateCode">State Code of the element</param>
        [Obsolete("SetColor is Obsolete. Please use SensorColor instead."), Category("Buhler")]
        public void SetColor(int StateCode)
        {
            FB19 AI = new FB19();
            _SetColor(AI.SetColor(StateCode));
            StatusSwitch = AI.Status_AI_Element;
            FaultSwitch = AI.Fault_AI_Element;
        }



        //------------------------------------------------------------------------------//
        //                                   Properties                                 //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int SensorColor
        {
            get
            {
                return _SensorColor;
            }
            set
            {
                _SensorColor = value;
                FB19 AI = new FB19();
                _SetColor(AI.SetColor(value));
                StatusSwitch = AI.Status_AI_Element;
                FaultSwitch = AI.Fault_AI_Element;
            }
        }

        [Category("Buhler")]
        public string Description_Switch
        {
            get
            {
                return this.DescriptionSwitch;
            }
            set
            {
                this.DescriptionSwitch = value;
            }
        }

        [Category("Buhler")]
        public string Status_Switch
        {
            get
            {
                return this.StatusSwitch;
            }
        }

        [Category("Buhler")]
        public bool Fault_Switch
        {
            get
            {
                return this.FaultSwitch;
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
