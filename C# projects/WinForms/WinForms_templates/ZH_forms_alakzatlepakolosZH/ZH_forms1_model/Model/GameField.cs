using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZH_forms1_model.Model
{
    public enum Player
    {
        Empty,
        FstPlayer,
        SndPlayer
    }

    public class GameField
    {
        #region Fields
        private int _row;
        private int _col;
        private bool _isFiled;

        private Player _player;
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
        public bool isFiled
        {
            get
            {
                return _isFiled;
            }
            set
            {
                _isFiled = value;
            }
        }

        public Player player
        {
            get { return _player; }
            set { _player = value; }
        }
        #endregion

        public GameField(int row, int col)
        {
            this.row = row;
            this.col = col;
            _isFiled = false;
            this.player = Player.Empty;
        }
    }
}
