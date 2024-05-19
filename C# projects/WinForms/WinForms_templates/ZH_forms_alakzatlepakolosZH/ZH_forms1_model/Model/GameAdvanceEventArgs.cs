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
        public GameField[,] gameTable;
        public GameField[,] blockTable;
        #endregion

        public GameAdvanceEventArgs(GameField[,] gameTable, GameField[,] blockTable)
        {
            this.gameTable = gameTable;
            this.blockTable = blockTable;
        }
    }
}
