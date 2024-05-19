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
        #endregion

        public GameAdvanceEventArgs(GameField[,] gameTable)
        {
            this.gameTable = gameTable;
        }
    }
}
