using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace KNEKT.Classes
{
    class ClickCoordinate
    {
        private double _ControlWidth;
        private double _ControlHeight;
        private Point _Point;

        private double _XCoOrdinate;
        private double _YCoOrdinate;

        private double _XControlHalf;
        private double _YControlHalf;

        private double _XControlQuarter1;
        private double _XControlQuarter2;
        private double _XControlQuarter3;
        private double _XControlQuarter4;

        private double _YControlQuarter1;
        private double _YControlQuarter2;
        private double _YControlQuarter3;
        private double _YControlQuarter4;



        //------------------------------------------------------------------------------//
        //                                Constructor                                   //
        //------------------------------------------------------------------------------//  

        /// <summary>
        /// Calculate the region of the control that was clicked
        /// </summary>
        /// <param name="ControlWidth">Width of the User Control</param>
        /// <param name="ControlHeight">height of the User Control</param>
        /// <param name="point">X,Y CoOrdinate of the click on the Control</param>
        public ClickCoordinate(double ControlWidth, double ControlHeight, Point point)
        {
            //_ControlWidth = ControlWidth;
            //_ControlHeight = ControlHeight;
            XControlWidth = ControlWidth;
            YControlHeight = ControlHeight;
            _Point = point;

            Calculate();
        }


        //------------------------------------------------------------------------------//
        //                           Functionality Methods                              //
        //------------------------------------------------------------------------------//  

        private void Calculate()
        {
            //Click Co-ordinate
            XMouseCoordinate = _Point.X;
            YMouseCoordinate = _Point.Y;

            //Half of the control
            XControlHalf = XControlWidth / 2;
            YControlHalf = YControlHeight / 2;

            //First Quarter 
            XControlQuarter1 = _ControlWidth / 4;
            YControlQuarter1 = _ControlHeight / 4;

            //Second Quarter
            XControlQuarter2 = XControlHalf;
            YControlQuarter2 = YControlHalf;

            //Third Quarter
            XControlQuarter3 = XControlHalf + XControlQuarter1;
            YControlQuarter3 = YControlHalf + YControlQuarter1;

            ////Fourth Quarter
            //XControlQuarter4 = XControlHalf + XControlQuarter1;
            //YControlQuarter4 = YControlHalf + YControlQuarter1;

        }


        //------------------------------------------------------------------------------//
        //                                 Properties                                   //
        //------------------------------------------------------------------------------//  

        /// <summary>
        /// X CoOrdinate of the Mouse Click
        /// </summary>
        public double XMouseCoordinate
        {
            get
            {
                return _XCoOrdinate;
            }
            set
            {
                _XCoOrdinate = value;
            }
        }

        /// <summary>
        /// Y CoOrdinate of the Mouse Click
        /// </summary>
        public double YMouseCoordinate
        {
            get
            {
                return _YCoOrdinate;
            }
            set
            {
                _YCoOrdinate = value;
            }
        }

        /// <summary>
        /// Half the width of the control
        /// </summary>
        public double XControlWidth
        {
            get
            {
                return _ControlWidth;
            }
            set
            {
                _ControlWidth = value;
            }
        }

        /// <summary>
        /// Half the width of the control
        /// </summary>
        public double YControlHeight
        {
            get
            {
                return _ControlHeight;
            }
            set
            {
                _ControlHeight = value;
            }
        }

        /// <summary>
        /// Half the width of the control
        /// </summary>
        public double XControlHalf
        {
            get
            {
                return _XControlHalf;
            }
            set
            {
                _XControlHalf = value;
            }
        }

        /// <summary>
        /// Half the Height of the control
        /// </summary>
        public double YControlHalf
        {
            get
            {
                return _YControlHalf;
            }
            set
            {
                _YControlHalf = value;
            }
        }

        /// <summary>
        /// First Quarter of the width of the control
        /// </summary>
        public double XControlQuarter1
        {
            get
            {
                return _XControlQuarter1;
            }
            set
            {
                _XControlQuarter1 = value;
            }
        }

        /// <summary>
        /// Second Quarter of the width of the control
        /// </summary>
        public double XControlQuarter2
        {
            get
            {
                return _XControlQuarter2;
            }
            set
            {
                _XControlQuarter2 = value;
            }
        }

        /// <summary>
        /// Third Quarter of the width of the control
        /// </summary>
        public double XControlQuarter3
        {
            get
            {
                return _XControlQuarter3;
            }
            set
            {
                _XControlQuarter3 = value;
            }
        }

        /// <summary>
        /// Fourth Quarter of the width of the control
        /// </summary>
        public double XControlQuarter4
        {
            get
            {
                return _XControlQuarter4;
            }
            set
            {
                _XControlQuarter4 = value;
            }
        }

        /// <summary>
        /// First Quarter of the height of the control
        /// </summary>
        public double YControlQuarter1
        {
            get
            {
                return _YControlQuarter1;
            }
            set
            {
                _YControlQuarter1 = value;
            }
        }

        /// <summary>
        /// Second Quarter of the height of the control
        /// </summary>
        public double YControlQuarter2
        {
            get
            {
                return _YControlQuarter2;
            }
            set
            {
                _YControlQuarter2 = value;
            }
        }

        /// <summary>
        /// Third Quarter of the height of the control
        /// </summary>
        public double YControlQuarter3
        {
            get
            {
                return _YControlQuarter3;
            }
            set
            {
                _YControlQuarter3 = value;
            }
        }

        /// <summary>
        /// Fourth Quarter of the height of the control
        /// </summary>
        public double YControlQuarter4
        {
            get
            {
                return _YControlQuarter4;
            }
            set
            {
                _YControlQuarter4 = value;
            }
        }

    }
}
