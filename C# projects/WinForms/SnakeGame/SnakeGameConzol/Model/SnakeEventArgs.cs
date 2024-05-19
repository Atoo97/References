using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.SnakeGameConzol.Model
{
    /// <summary>
    /// Snake eseményargumentum típusa.
    /// </summary>
    public class SnakeEventArgs : EventArgs
    {
        private readonly Boolean _isOver;        //Játék végetérését jelzi
        private readonly Int32 _scores;          //aktuális pontok
        private readonly Int32 _highScores;      //eddigi legtöbb pont
        private readonly Direction _direction;   //kigyó mozgásának iránya


        /// <summary>
        /// Játék végénel lekérdezése.
        /// </summary>
        public Boolean IsOver { get { return _isOver; } }

        /// <summary>
        /// Játéklépések számának lekérdezése.
        /// </summary>
        public Direction SnakeDirection { get { return _direction; } }

        /// <summary>
        /// Játékos aktuális pontajinak lekérdezése.
        /// </summary>
        public Int32 ScoresCount { get { return _scores; } }

        /// <summary>
        /// Játékos eddigi legtöbb pontajinak lekérdezése.
        /// </summary>
        public Int32 HighScoresCount { get { return _highScores; } }

        public SnakeEventArgs(Boolean isOver, Int32 scoresCount, Int32 highScoresCount, Direction direction)
        {
            _isOver = isOver;
            _scores = scoresCount;
            _highScores = highScoresCount;
            _direction = direction;

        }


    }
}
