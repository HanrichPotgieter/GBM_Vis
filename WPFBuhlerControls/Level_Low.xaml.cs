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
    /// Interaction logic for Level_Low.xaml
    /// </summary>
    [Obsolete("Level_Low is Obsolete. Please use Level_Low_Bin or Level_Low_Machine instead.")]
    public partial class Level_Low : UserControl
    {
        //private int _LevelColor;
        private string DescriptionLowLevel;
        private string StatusLowLevel;
        private bool FaultLowLevel;

        public Level_Low()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set the color of the motor
        /// </summary>
        /// <param name="StateCode">State Code of the element</param>
        [Category("Buhler")]
        public void SetColor(int StateCode, int PType)
        {
            FB14 LL = new FB14();
            _SetColor(LL.SetColor(StateCode, PType));
            StatusLowLevel = LL.Status_DI_Status;
            FaultLowLevel = LL.Fault_DI_Element;
        }

        [Category("Buhler")]
        public string Description_LowLevel
        {
            get
            {
                return this.DescriptionLowLevel;
            }
            set
            {
                this.DescriptionLowLevel = value;
            }
        }

        [Category("Buhler")]
        public string Status_LowLevel
        {
            get
            {
                return this.StatusLowLevel;
            }
        }

        [Category("Buhler")]
        public bool Fault_LowLevel
        {
            get
            {
                return this.FaultLowLevel;
            }
        }


        //------------------------------------------------------------------------------//
        //                                    Methods                                   //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            RectMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                RectMain.Fill = brushColor;
            }));
        }
    }
}
