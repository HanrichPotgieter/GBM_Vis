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
using System.ComponentModel;
using WPFBuhlerControls.FB_Code;

namespace WPFBuhlerControls
{
    /// <summary>
    /// Interaction logic for Scourer_DRPA.xaml
    /// </summary>
    public partial class Scourer_DRPA : UserControl
    {

        private int _MotorColor;
        private string DescriptionScourer;
        private string StatusScourer;
        private bool FaultScourer;
        private string _ObjectNo;
        private string _PLCName;

        public Scourer_DRPA()
        {
            InitializeComponent();
        }

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
                StatusScourer = motor.Status_Motor;
                FaultScourer = motor.Fault_Motor;
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
        public string Description_Scourer
        {
            get
            {
                return this.DescriptionScourer;
            }
            set
            {
                this.DescriptionScourer = value;
            }
        }

        [Category("Buhler")]
        public string Status_Scourer
        {
            get
            {
                return this.StatusScourer;
            }
        }

        [Category("Buhler")]
        public bool Fault_Scourer
        {
            get
            {
                return this.FaultScourer;
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
            RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectMain.Fill = brushColor;
            }));

            RectLeft.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectLeft.Fill = brushColor;
            }));

            RectRight.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectRight.Fill = brushColor;
            }));

            RectRightSmall.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectRightSmall.Fill = brushColor;
            }));

            RectTopSmall.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectTopSmall.Fill = brushColor;
            }));

            PolyMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyMain.Fill = brushColor;
            }));


            PolyOutlet.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyOutlet.Fill = brushColor;
            }));
        }
    }
}
