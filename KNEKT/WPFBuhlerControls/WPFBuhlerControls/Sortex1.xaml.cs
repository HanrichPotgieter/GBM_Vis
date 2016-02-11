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
    public enum eSortexType
    {
        DIGITAL_INPUT,
        MEAG_CONTROLLER
    }

    /// <summary>
    /// Interaction logic for Sortex1.xaml
    /// </summary>
    public partial class Sortex1 : UserControl
    {
        private int _SortexColor;
        private string DescriptionSortex;
        private string StatusSortex;
        private bool FaultSortex;
        private eSortexType _sortexType;
        private string _ObjectNo;
        private string _PLCName;

        public Sortex1()
        {
            InitializeComponent();

            Sortex_Type = eSortexType.DIGITAL_INPUT;
        }

        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int SortexColor
        {
            get
            {
                return _SortexColor;
            }
            set
            {
                _SortexColor = value;

                if (Sortex_Type == eSortexType.DIGITAL_INPUT)
                {
                    FB11 sort = new FB11();
                    _SetColor(sort.SetColor(value));
                    StatusSortex = sort.Status_DO_Element;
                    FaultSortex = sort.Fault_DO_Element;
                }
                else if (Sortex_Type == eSortexType.MEAG_CONTROLLER)
                {
                    FB805 sort = new FB805();
                    _SetColor(sort.SetColor(value));
                    StatusSortex = sort.Status_Sortex;
                    FaultSortex = sort.Fault_Sortex;
                }
                
            }
        }

        [Category("Buhler")]
        public string Description_Sortex
        {
            get
            {
                return this.DescriptionSortex;
            }
            set
            {
                this.DescriptionSortex = value;
            }
        }

        [Category("Buhler")]
        public eSortexType Sortex_Type
        {
            get
            {
                return this._sortexType;
            }
            set
            {
                this._sortexType = value;
            }
        }

        [Category("Buhler")]
        public string Status_Sortex
        {
            get
            {
                return this.StatusSortex;
            }
        }

        [Category("Buhler")]
        public bool Fault_Sortex
        {
            get
            {
                return this.FaultSortex;
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
            PolyInlet.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyInlet.Fill = brushColor;
            }));
            

            RectLeft1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectLeft1.Fill = brushColor;
            }));

            RectLeft2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectLeft2.Fill = brushColor;
            }));

            RectRight2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectRight2.Fill = brushColor;
            }));

            RectRight1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                RectRight1.Fill = brushColor;
            }));


            PolyOutlet1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyOutlet1.Fill = brushColor;
            }));

            PolyOutlet2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyOutlet2.Fill = brushColor;
            }));

            PolyOutlet3.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyOutlet3.Fill = brushColor;
            }));

            PolyOutlet4.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyOutlet4.Fill = brushColor;
            }));


            PolyCrown1.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyCrown1.Fill = brushColor;
            }));

            PolyCrown2.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyCrown2.Fill = brushColor;
            }));

            PolyCrown3.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyCrown3.Fill = brushColor;
            }));

            PolyCrown4.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                PolyCrown4.Fill = brushColor;
            }));
        }
    }
}
