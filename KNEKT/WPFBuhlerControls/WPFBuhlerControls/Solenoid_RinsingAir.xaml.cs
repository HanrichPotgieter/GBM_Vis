﻿using System;
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
    /// Interaction logic for Solenoid_RinsingAir.xaml
    /// </summary>
    public partial class Solenoid_RinsingAir : UserControl
    {
        private int _ValveColor;
        private string DescriptionValve;
        private string StatusValve;
        private bool FaultValve;
        private string _ObjectNo;
        private string _PLCName;

        public Solenoid_RinsingAir()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int ValveColor
        {
            get
            {
                return _ValveColor;
            }
            set
            {
                _ValveColor = value;
                FB11 valve = new FB11();
                _SetColor(valve.SetColor(value));
                StatusValve = valve.Status_DO_Element;
                FaultValve = valve.Fault_DO_Element;
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
        public string Description_Valve
        {
            get
            {
                return this.DescriptionValve;
            }
            set
            {
                this.DescriptionValve = value;
            }
        }

        [Category("Buhler")]
        public string Status_Valve
        {
            get
            {
                return this.StatusValve;
            }
        }

        [Category("Buhler")]
        public bool Fault_Valve
        {
            get
            {
                return this.FaultValve;
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
            polyTriLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyTriLeft.Fill = brushColor;
            }));

            polyTriRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                polyTriRight.Fill = brushColor;
            }));
        }
    }
}
