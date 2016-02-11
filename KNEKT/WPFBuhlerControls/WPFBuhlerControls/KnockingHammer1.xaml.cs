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
    /// Interaction logic for KnockingHammer1.xaml
    /// </summary>
    public partial class KnockingHammer1 : UserControl
    {
        private int _HammerColor;
        private string DescriptionHammer;
        private string StatusHammer;
        private bool FaultHammer;
        private string _ObjectNo;
        private string _PLCName;

        public KnockingHammer1()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int HammerColor
        {
            get
            {
                return _HammerColor;
            }
            set
            {
                _HammerColor = value;
                FB11 hammer = new FB11();
                _SetColor(hammer.SetColor(value));
                StatusHammer = hammer.Status_DO_Element;
                FaultHammer = hammer.Fault_DO_Element;
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
        public string Description_Hammer
        {
            get
            {
                return this.DescriptionHammer;
            }
            set
            {
                this.DescriptionHammer = value;
            }
        }

        [Category("Buhler")]
        public string Status_Hammer
        {
            get
            {
                return this.StatusHammer;
            }
        }

        [Category("Buhler")]
        public bool Fault_Hammer
        {
            get
            {
                return this.FaultHammer;
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
        //                                    Methods                                   //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            ellipseMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,new Action(delegate()
            {
                ellipseMain.Fill = brushColor;
            }));
        }    
    }
}
