using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KNEKT.Classes
{
    public class CWAScaleDBInfo
    {
        private int _ScaleNumber;
        public int ScaleNumber
        {
            get { return _ScaleNumber; }
            set { _ScaleNumber = value; }
        }

        private string _ObjMaskDBOffset;
        public string ObjMaskDBOffset
        {
            get { return _ObjMaskDBOffset; }
            set { _ObjMaskDBOffset = value; }
        }

        private string _ObjUsedDBOffset;
        public string ObjUsedDBOffset
        {
            get { return _ObjUsedDBOffset; }
            set { _ObjUsedDBOffset = value; }
        }

        private string _FirstIngredientDBOffset;
        public string FirstIngredientDBOffset
        {
            get { return _FirstIngredientDBOffset; }
            set { _FirstIngredientDBOffset = value; }
        }

        private int _NumberOfIngredients;
        public int NumberOfIngredients
        {
            get { return _NumberOfIngredients; }
            set { _NumberOfIngredients = value; }
        }
    }
}
