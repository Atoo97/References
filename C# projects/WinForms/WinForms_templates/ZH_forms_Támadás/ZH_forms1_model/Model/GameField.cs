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

        private Player _player;
        private int _playerFigure;
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

        public Player player
        {
            get { return _player; }
            set { _player = value; }
        }
        public int playerFigure
        {
            get { return _playerFigure; }
            set
            {
                _playerFigure = value;
            }
        }
        #endregion

        public GameField(int row, int col)
        {
            this.row = row;
            this.col = col;
            this.player = Player.Empty;
            this._playerFigure = 0;
        }
    }
}
