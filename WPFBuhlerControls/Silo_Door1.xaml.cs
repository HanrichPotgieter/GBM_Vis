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
    /// Interaction logic for Silo_Door1.xaml
    /// </summary>
    public partial class Silo_Door1 : UserControl
    {
        private int _DoorColor;
        private string DescriptionDoor;
        private string StatusDoor;
        private bool FaultDoor;
        private string _ObjectNo;
        private string _PLCName;

        public Silo_Door1()
        {
            InitializeComponent();
        }


        //------------------------------------------------------------------------------//
        //                                  Prooerties                                  //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int DoorColor
        {
            get
            {
                return this._DoorColor;
            }
            set
            {
                this._DoorColor = value;
                FB14 di = new FB14();
                _SetColor(di.SetColor(value, 7167));
                StatusDoor = di.Status_DI_Status;
                FaultDoor = di.Fault_DI_Element;
            }
        }


        //
        //  Overflow
        //
        [Category("Buhler")]
        public string Description_Door
        {
            get
            {
                return this.DescriptionDoor;
            }
            set
            {
                this.DescriptionDoor = value;
            }
        }

        [Category("Buhler")]
        public string Status_Door
        {
            get
            {
                return this.StatusDoor;
            }
        }

        [Category("Buhler")]
        public bool Fault_Door
        {
            get
            {
                return this.FaultDoor;
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
        //                                  Methods                                     //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            rectDoor.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectDoor.Fill = brushColor;
            }));
        }
    }
}
