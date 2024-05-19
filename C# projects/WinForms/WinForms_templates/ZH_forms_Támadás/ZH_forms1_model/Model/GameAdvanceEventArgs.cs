using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ZH_forms1_model.Model.GameField;

namespace ZH_forms1_model.Model
{
    public class GameAdvanceEventArgs : EventArgs
    {
        #region Fields
        public GameField[,] gameTable;
        public Player player;
        public int round;
        #endregion

        public GameAdvanceEventArgs(GameField[,] gameTable, Player player, int round)
        {
            this.gameTable = gameTable;
            this.player = player;
            this.round = round;
        }
    }
}
