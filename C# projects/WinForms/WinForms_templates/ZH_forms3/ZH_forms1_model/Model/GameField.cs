using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZH_forms1_model.Model
{
    public class GameField
    {
        #region Fields
        private int _row;
        private int _col;

        private int _index;
        private bool _border;
        #endregion

        #region Getters/Setters
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
        public int index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
            }
        }
        public bool border
        {
            get
            {
                return _border;
            }
            set
            {
                _border = value;
            }
        }

        #endregion

        public GameField(int row, int col, int index)
        {
            this.row = row;
            this.col = col;
            this.index = index;
            this.border = false;
        }
    }
}
