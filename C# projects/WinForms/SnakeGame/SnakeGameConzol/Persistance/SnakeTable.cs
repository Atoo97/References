using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.SnakeGameConzol.Model;

namespace Game.SnakeGameConzol.Persistance
{
    /// <summary>
    /// Snake játéktábla típusa.
    /// </summary>
    public class SnakeTable
    {
        #region Fields

        private Int32 _widthAndHeight; // pálya n x n mérete
        private Int32 _bordersNumber; // pályán elhelyezett akadályok száma

        private List<FigShapes> bordersCoordinates; //Pályaakadályok

        #endregion

        /// <summary>
        /// Pálya méretének lekérdezése.
        /// </summary>
        public Int32 RegionSize { get { return _widthAndHeight; } }

        /// <summary>
        /// Pályán elhelyezett akadályok számának lekérdezése.
        /// </summary>
        public Int32 BordersNumber { get { return _bordersNumber; } }

        /// <summary>
        /// Pályán elhelyezett akadályok koordinátáinak lekérdezése.
        /// </summary>
        public List<FigShapes> BordersCoordinates { get { return bordersCoordinates; } }



        #region Constructors

        /// <summary>
        /// Snake játéktábla példányosítása.
        /// </summary>
        public SnakeTable() : this(600, 8) { } 

        /// <summary>
        /// Snake játéktábla példányosítása.
        /// </summary>
        /// <param name="tableSize">Játéktábla mérete.</param>
        /// <param name="bordersNum">Akadályok száma</param>
        public SnakeTable(Int32 tableSize, Int32 bordersNum)
        {
            bordersCoordinates = new List<FigShapes>();

            //Táblaméret ellenőrzés
            if (tableSize < 0)
                throw new ArgumentOutOfRangeException(nameof(tableSize), "The table size is less than 0.");
            if (tableSize > 800)
                throw new ArgumentOutOfRangeException(nameof(tableSize), "The table size is larger than 800.");

            //Akadályok számának ellenőrzése
            int vol = (int)(tableSize / 20); //Maximum mennyiségű elhelyezhető egységnyi akadály a pályán
            if (bordersNum/2 > vol)
                throw new ArgumentOutOfRangeException(nameof(bordersNum), "The borders number is more than the expected: tableSize / 20.");
            if (bordersNum < 0)
                throw new ArgumentOutOfRangeException(nameof(bordersNum), "The borders number is less than 0");

            _widthAndHeight = tableSize;
            _bordersNumber = bordersNum/2; //két koordinátá kell megadni ezért összesnek a felét kell venni
        }

        #endregion


    }
}
