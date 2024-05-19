using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZH_forms1_model.Model
{
    public class GameOverEventArgs
    {
        #region Fields
        public bool isWon;
        #endregion

        public GameOverEventArgs(bool isWon)
        {
            this.isWon = isWon;
        }
    }
}
