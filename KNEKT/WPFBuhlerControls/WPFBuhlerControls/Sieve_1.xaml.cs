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
    /// Interaction logic for Sieve_1.xaml
    /// </summary>
    public partial class Sieve_1 : UserControl
    {
        private int _SieveColor;
        private string DescriptionSieve;
        private string StatusSieve;
        private bool FaultSieve;
        private string _ObjectNo;
        private string _PLCName;

        public Sieve_1()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------//
        //                                  Properties                                  //
        //------------------------------------------------------------------------------//

        [Category("Buhler")]
        public int SieveColor
        {
            get
            {
                return _SieveColor;
            }
            set
            {
                _SieveColor = value;
                FB14 input = new FB14();
                _SetColor(input.SetColor(value,7147));
                StatusSieve = input.Status_DI_Status;
                FaultSieve = input.Fault_DI_Element;
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
        public string Description_Sieve
        {
            get
            {
                return this.DescriptionSieve;
            }
            set
            {
                this.DescriptionSieve = value;
            }
        }

        [Category("Buhler")]
        public string Status_Sieve
        {
            get
            {
                return this.StatusSieve;
            }
        }

        [Category("Buhler")]
        public bool Fault_Sieve
        {
            get
            {
                return this.FaultSieve;
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
        //                                     Methods                                  //
        //------------------------------------------------------------------------------//
        private void _SetColor(Brush brushColor)
        {
            poltBot.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                poltBot.Fill = brushColor;
            }));

            poltTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                poltTop.Fill = brushColor;
            }));

            rectTop.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectTop.Fill = brushColor;
            }));

            rectBottom.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                rectBottom.Fill = brushColor;
            }));
        }
    }
}
