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
        private bool _isBomb;
        private bool _isFound;
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
        public bool isBomb
        {
            get
            {
                return _isBomb;
            }
            set
            {
                _isBomb = value;
            }
        }
        public bool isFound
        {
            get
            {
                return _isFound;
            }
            set
            {
                _isFound = value;
            }
        }
        #endregion

        public GameField(int row, int col)
        {
            this.row = row;
            this.col = col;
            _isBomb = false;
            _isFound = false;
        }
    }
}
