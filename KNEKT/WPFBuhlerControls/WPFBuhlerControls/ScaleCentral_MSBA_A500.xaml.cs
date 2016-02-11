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
    public enum eScaleType
    {
        DUMP,
        DOSING,
        DIFF,
        CWA,
        CWA_DMST
    }

    /// <summary>
    /// Interaction logic for ScaleCentral_MSBA_E500.xaml
    /// </summary>
    public partial class ScaleCentral_MSBA_A500 : UserControl
    {
        private bool _IsControlled = false;
        private eScaleType _ScaleType;
        private int _ScaleColor;
        private string DescriptionScale;
        private string StatusScale;
        private bool FaultScale;
        private string _ObjectNo;
        private string _PLCName;

        public ScaleCentral_MSBA_A500()
        {
            InitializeComponent();
            Scale_IsControlled = true;
        }



        //------------------------------------------------------------------------------//
        //                                Properties                                    //
        //------------------------------------------------------------------------------//
        [Category("Buhler")]
        public bool Scale_IsControlled
        {
            get
            {
                return _IsControlled;
            }
            set
            {
                _IsControlled = value;
                _SetDefaultColor(value);
            }
        }

        [Category("Buhler")]
        public eScaleType Scale_ScaleType
        {
            get
            {
                return _ScaleType;
            }
            set
            {
                _ScaleType = value; 
            }
        }

        [Category("Buhler")]
        public int ScaleColor
        {
            get
            {
                return _ScaleColor;
            }
            set
            {
                _ScaleColor = value;

                if (Scale_ScaleType == eScaleType.CWA)
                {
                    FB97 cwa = new FB97();
                    _SetColor(cwa.SetColor(value));
                    StatusScale = cwa.Status_CWA;
                    FaultScale = cwa.Fault_CWA;
                }
                else if (Scale_ScaleType == eScaleType.CWA_DMST)
                {
                    FB83 cwa = new FB83();
                    _SetColor(cwa.SetColor(value));
                    StatusScale = cwa.Status_MEAF;
                    FaultScale = cwa.Fault_MEAF;
                }
            }
        }

        [Category("Buhler")]
        public string Description_Scale
        {
            get
            {
                return this.DescriptionScale;
            }
            set
            {
                this.DescriptionScale = value;
            }
        }

        [Category("Buhler")]
        public string Status_Scale
        {
            get
            {
                return this.StatusScale;
            }
        }

        [Category("Buhler")]
        public bool Fault_Scale
        {
            get
            {
                return this.FaultScale;
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
            polyMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
            {
                polyMain.Fill = brushColor;
            }));
        }
        
        
        private void _SetDefaultColor(bool Value)
        {
            if (Value)
            {
                polyMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { polyMain.Fill = Brushes.Transparent; }));
            }
            else
            {
                polyMain.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { polyMain.Fill = KNEKTColors.Gray; }));
            }
        }
    }
}
