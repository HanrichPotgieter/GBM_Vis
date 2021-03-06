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
    /// Interaction logic for Level_High.xaml
    /// </summary>
    [Obsolete("Level_High is Obsolete. Please use Level_High_Bin or Level_High_Machine instead.")]
    public partial class Level_High : UserControl
    {
        //private int _LevelColor;
        private string DescriptionHighLevel;
        private string StatusHighLevel;
        private bool FaultHighLevel;

        public Level_High()
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
            FB14 HL = new FB14();
            _SetColor(HL.SetColor(StateCode, PType));
            StatusHighLevel = HL.Status_DI_Status;
            FaultHighLevel = HL.Fault_DI_Element;
        }

        [Category("Buhler")]
        public string Description_HighLevel
        {
            get
            {
                return this.DescriptionHighLevel;
            }
            set
            {
                this.DescriptionHighLevel = value;
            }
        }

        [Category("Buhler")]
        public string Status_HighLevel
        {
            get
            {
                return this.StatusHighLevel;
            }
        }

        [Category("Buhler")]
        public bool Fault_HighLevel
        {
            get
            {
                return this.FaultHighLevel;
            }
        }


        //------------------------------------------------------------------------------//
        //                                  Methods                                     //
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
