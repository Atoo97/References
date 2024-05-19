using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZH_forms1.View
{
    internal class GridButton : Button
    {
        #region Fields
        private int _row;
        private int _col;
        #endregion

        public int GridX
        {
            get
            {
                return _row;
            }
            set
            {
                _row = value;
            }
        }
        public int GridY
        {
            get
            {
                return _col;
            }
            set
            {
                _col = value;
            }
        }

        public GridButton(int row, int col)
        {
            this.GridX = row;
            this.GridY = col;
        }
    }
}
