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

        public int row
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
        public int col
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
            this.row = row;
            this.col = col;
        }
    }
}
