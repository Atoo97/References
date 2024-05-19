using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZH_forms1_model.Model
{
    public class GameAdvanceEventArgs : EventArgs
    {
        #region Fields
        public int player_row;
        public int player_col;
        public int direction;
        #endregion

        public GameAdvanceEventArgs(int player_row, int player_col, int direction)
        {
            this.player_row = player_row;
            this.player_col = player_col;
            this.direction = direction;
        }
    }
}
