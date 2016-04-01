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
using Snap7;
using System.Threading;

namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for RollerStand1.xaml
    /// </summary>
    public partial class RollerStand_MDDR : UserControl
    {
        private int _MotorColor1;
        private int _MotorColor2;
        private string _ObjectNo1;
        private string _ObjectNo2;
        //private int _FrequencyColor1;
        //private int _FrequencyColor2;

        private string DescriptionMotor1;
        private string StatusMotor1;
        private bool FaultMotor1;

        private string DescriptionMotor2;
        private string StatusMotor2;
        private bool FaultMotor2;

        private string DescriptionFrequencyConverter1;
        private string StatusFrequencyConverter1;
        private bool FaultFrequencyConverter1;

        private string DescriptionFrequencyConverter2;
        private string StatusFrequencyConverter2;
        private bool FaultFrequencyConverter2;

        private string _PLCName;
        Worker workerObject;

        // The worker threads runs in the background and updates our control
        #region[Worker Thread]
        public class Worker
        {
            S7Client plc;
            RollerStand_MDDR parent;
            private int updateTime = 100;
            public int dbnumber { get; set; }
            public int dboffset { get; set; }
            public int dboffsetSpeedMonitor { get; set; }

            public Worker(S7Client tmp, RollerStand_MDDR parent)
            {
                plc = tmp;
                this.parent = parent;
            }
            // This method will be called when the thread is started.
            public void DoWork()
            {
                while (!_shouldStop)
                {
                    if (plc != null)
                    {
                        if (plc.Connected())
                        {
                            byte[] buffer = new byte[2];
                            Plc.DBRead(dbnumber, dboffset, 2, buffer);
                            Array.Reverse(buffer);
                            //Console.Out.WriteLine(BitConverter.ToUInt16(buffer, 0));
                            parent.MotorColor1 = BitConverter.ToUInt16(buffer, 0);
                            Thread.Sleep(200);
                        }
                    }
                }
                Thread.Sleep(updateTime);
            }
            public void RequestStop()
            {
                _shouldStop = true;
            }
            // Volatile is used as hint to the compiler that this data
            // member will be accessed by multiple threads.
            private volatile bool _shouldStop;
        }
        #endregion

        public RollerStand_MDDR()
        {
            InitializeComponent();

            PolyLeftMid.Fill = KNEKTColors.White;
            PolyRightMid.Fill = KNEKTColors.White;                 
        }

        /// <summary>
        /// Set the color of the motor
        /// </summary>
        /// <param name="StateCode">State Code of the element</param>
        /// <param name="MotorNumber">Motor number on the roller stand. 1 is Left motor, 2 is Right Motor</param>
        [Obsolete("SetMotorColor is deprecated , please use MotorColor1, MotorColor2 instead.")]
        public void SetMotorColor(int StateCode, int MotorNumber)
        {
            FB12 Motor = new FB12();
            _SetColorMotor(Motor.SetColor(StateCode), MotorNumber);

            if (MotorNumber == 1)
            {
                StatusMotor1 = Motor.Status_Motor;
                FaultMotor1 = Motor.Fault_Motor;
            }
            else if (MotorNumber == 2)
            {
                StatusMotor2 = Motor.Status_Motor;
                FaultMotor2 = Motor.Fault_Motor;
            }
        }



        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//


        [Category("Buhler")]
        public int dbnumber
        {
            get
            {
                return dbnumber;
            }
            set
            {
                workerObject.dbnumber = value;
            }
        }

        [Category("Buhler")]
        public int dboffset
        {
            get
            {
                return dbnumber;
            }
            set
            {
                workerObject.dboffset = value;
            }
        }

        [Category("Buhler")]
        public int MotorColor1
        {
            get
            {
                return _MotorColor1;
            }
            set
            {
                _MotorColor1 = value;
                FB12 Motor = new FB12();
                _SetColorMotor(Motor.SetColor(value), 1);
                StatusMotor1 = Motor.Status_Motor;
                FaultMotor1 = Motor.Fault_Motor;
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
        public string ObjectNumber1
        {
            get
            {
                return this._ObjectNo1;
            }
            set
            {
                this._ObjectNo1 = value;
            }
        }

        [Category("Buhler")]
        public int MotorColor2
        {
            get
            {
                return _MotorColor2;
            }
            set
            {
                _MotorColor2 = value;
                FB12 Motor = new FB12();
                _SetColorMotor(Motor.SetColor(value), 2);
                StatusMotor2 = Motor.Status_Motor;
                FaultMotor2 = Motor.Fault_Motor;
            }
        }

        [Category("Buhler")]
        public string ObjectNumber2
        {
            get
            {
                return this._ObjectNo2;
            }
            set
            {
                this._ObjectNo2 = value;
            }
        }


        //
        //  FREQUENCY CONVERTER (LEFT)
        //
        [Category("Buhler")]
        public int FrequencyConverter_ColorSide1
        {
            set
            {
                FB14 Converter = new FB14();
                _SetColorFreq1(Converter.SetColor(value, 7154));
                StatusFrequencyConverter1 = Converter.Status_DI_Status;
                FaultFrequencyConverter1 = Converter.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public string Description_FrequencyConverter1
        {
            get
            {
                return this.DescriptionFrequencyConverter1;
            }
            set
            {
                this.DescriptionFrequencyConverter1 = value;
            }
        }

        [Category("Buhler")]
        public string Status_FrequencyConverter1
        {
            get
            {
                return this.StatusFrequencyConverter1;
            }
        }

        [Category("Buhler")]
        public bool Fault_FrequencyConverter1
        {
            get
            {
                return this.FaultFrequencyConverter1;
            }
        }


        //
        //  FREQUENCY CONVERTER (RIGHT)
        //
        [Category("Buhler")]
        public int FrequencyConverter_ColorSide2
        {
            set
            {
                FB14 Converter = new FB14();
                _SetColorFreq2(Converter.SetColor(value, 7154));
                StatusFrequencyConverter2 = Converter.Status_DI_Status;
                FaultFrequencyConverter2 = Converter.Fault_DI_Element;
            }
        }

        [Category("Buhler")]
        public string Description_FrequencyConverter2
        {
            get
            {
                return this.DescriptionFrequencyConverter2;
            }
            set
            {
                this.DescriptionFrequencyConverter2 = value;
            }
        }

        [Category("Buhler")]
        public string Status_FrequencyConverter2
        {
            get
            {
                return this.StatusFrequencyConverter2;
            }
        }

        [Category("Buhler")]
        public bool Fault_FrequencyConverter2
        {
            get
            {
                return this.FaultFrequencyConverter2;
            }
        }


        //
        //  MOTOR 1 (LEFT)
        //
        [Category("Buhler")]
        public string Description_Motor1
        {
            get
            {
                return this.DescriptionMotor1;
            }
            set
            {
                this.DescriptionMotor1 = value;
            }
        }

        [Category("Buhler")]
        public string Status_Motor1
        {
            get
            {
                return this.StatusMotor1;
            }
        }

        [Category("Buhler")]
        public bool Fault_Motor1
        {
            get
            {
                return this.FaultMotor1;
            }
        }


        //
        //  MOTOR 2 (RIGHT)
        //
        [Category("Buhler")]
        public string Description_Motor2
        {
            get
            {
                return this.DescriptionMotor2;
            }
            set
            {
                this.DescriptionMotor2 = value;
            }
        }

        [Category("Buhler")]
        public string Status_Motor2
        {
            get
            {
                return this.StatusMotor2;
            }
        }

        [Category("Buhler")]
        public bool Fault_Motor2
        {
            get
            {
                return this.FaultMotor2;
            }
        }


        //------------------------------------------------------------------------------//
        //                                   Methods                                    //
        //------------------------------------------------------------------------------//
        private void _SetColorMotor(Brush brushColor, int MotorNum)
        {
            if (MotorNum == 1)
            {
                rectBotLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    rectBotLeft.Fill = brushColor;
                }));
            }
            else if (MotorNum == 2)
            {
                rectMotRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
                {
                    rectMotRight.Fill = brushColor;
                }));
            }
        }

        private void _SetColorFreq1(Brush brushColor)
        {            
            PolyLeftMid.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                PolyLeftMid.Fill = brushColor;
            }));
        }

        private void _SetColorFreq2(Brush brushColor)
        {
            PolyRightMid.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                PolyRightMid.Fill = brushColor;
            }));
        }
    }
}
