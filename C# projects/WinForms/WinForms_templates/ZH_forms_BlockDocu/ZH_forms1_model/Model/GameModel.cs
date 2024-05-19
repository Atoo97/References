using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZH_forms1_model.Model
{
    public class GameModel
    {
        #region Fields
        private bool[,] _gameTable = new bool[4, 4];      //mérete beégetve
        private bool[,] _nextBlock = new bool[2, 2];     //mérete beégetve
        private int _nextBlockType;
        private Random _random = new Random();

        private int _points;
        #endregion


        #region Getters/Setters

        public int Points
        {
            get { return _points; }
        }

        public bool NextBlock(Int32 x, Int32 y)                     //kövi lehlyezhető block lekérdezése
        {
            if (x < 0 || x >= _nextBlock.GetLength(0))
                throw new ArgumentException("Bad column index.", nameof(x));
            if (y < 0 || y >= _nextBlock.GetLength(1))
                throw new ArgumentException("Bad row index.", nameof(y));

            return _nextBlock[x, y];
        }

        public bool this[Int32 x, Int32 y]      //aktuális gametable lekérdez
        {
            get
            {
                if (x < 0 || x >= _gameTable.GetLength(0))
                    throw new ArgumentException("Bad column index.", nameof(x));
                if (y < 0 || y >= _gameTable.GetLength(1))
                    throw new ArgumentException("Bad row index.", nameof(y));

                return _gameTable[x, y];
            }
        }
        #endregion


        public GameModel()
        {
            _points = 0;

        }


        #region public Methods
        public void NewGame()
        {
            _points = 0;
            OnPointChanged();

            for (Int32 i = 0; i < 4; i++)       //gametable mérete beégetve
            {
                for (Int32 j = 0; j < 4; j++)
                {
                    _gameTable[i, j] = false;
                }
            }

            NewBlock(); // új berakható block létrehoz
        }

        public void StepGame(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _gameTable.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "Bad column index.");
            if (y < 0 || y >= _gameTable.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "Bad row index.");

            CheckBlock(x, y); //Ellenőtzöm tehetek e ide alakzatot
            PlaceBlock(x, y); //Elhelyez és megjelenítés eseménye kivált

            _points++;
            OnPointChanged();

            CheckFilledLines();  //Ellenőrizzük van-e telitett sor vagy oszlop

            NewBlock();             //új blokk generál
            OnNextBlockChanged();   //új blokk változásának esemnye


            CheckNextCantBePlaced();    //check lelehet e még tenni blokkot
        }
        #endregion


        #region private Methods
        private void NewBlock()
        {
            _nextBlock = new bool[2, 2]; //Üresre állítjuk
            int rand = _random.Next(1, 4); // 4 lehetőség közül alakzat

            _nextBlockType = rand;

            switch (rand)
            {
                case 1:     //Lefele 2es
                    _nextBlock[0, 0] = true;
                    _nextBlock[0, 1] = false;
                    _nextBlock[1, 0] = true;
                    _nextBlock[1, 1] = false;
                    break;
                case 2:     //Oldalra 2es
                    _nextBlock[0, 0] = false;
                    _nextBlock[0, 1] = false;
                    _nextBlock[1, 0] = true;
                    _nextBlock[1, 1] = true;
                    break;
                case 3:     //Sima L
                    _nextBlock[0, 0] = true;
                    _nextBlock[0, 1] = false;
                    _nextBlock[1, 0] = true;
                    _nextBlock[1, 1] = true;
                    break;
                case 4:     //Tukrozott L
                    _nextBlock[0, 0] = true;
                    _nextBlock[0, 1] = true;
                    _nextBlock[1, 0] = false;
                    _nextBlock[1, 1] = true;
                    break;
            }
        }

        private void CheckBlock(Int32 x, Int32 y)
        {
            switch (_nextBlockType)
            {
                case 1:     //Lefele 2es
                    if (x == 3 || _gameTable[x, y] || _gameTable[x + 1, y]) 
                        //Ha utolsó sorban kattintottam vagy már van elhelyezve ide alakzat
                        throw new Exception();
                    break;
                case 2:     //Oldalra 2es
                    if (y == 3 || _gameTable[x, y] || _gameTable[x, y + 1]) 
                        //Ha utolsó oszlopban kattintottam vagy már van elhelyezve ide alakzat
                        throw new Exception();
                    break;
                case 3:     //Sima L
                    if (x == 3 || y == 3 || _gameTable[x, y] || _gameTable[x + 1, y] || _gameTable[x + 1, y + 1])
                        //Ha utolsó sorban || utolsó pszlopban kattintottam vagy már van elhelyezve ide alakzat
                        throw new Exception();
                    break;
                case 4:     //Tukrozott L
                    if (x == 3 || y == 3 || _gameTable[x, y] || _gameTable[x, y + 1] || _gameTable[x + 1, y + 1])
                        //Ha utolsó sorban || utolsó pszlopban kattintottam vagy már van elhelyezve ide alakzat
                        throw new Exception();
                    break;
            }
        }

        private void PlaceBlock(Int32 x, Int32 y)
        {
            switch (_nextBlockType)
            {
                case 1:     //Lefele 2es
                    _gameTable[x, y] = true;
                    OnFieldChanged(x, y, true);     //Változás kiváltása
                    _gameTable[x + 1, y] = true;
                    OnFieldChanged(x + 1, y, true);
                    break;
                case 2:     //Oldalra 2es
                    _gameTable[x, y] = true;
                    OnFieldChanged(x, y, true);
                    _gameTable[x, y + 1] = true;
                    OnFieldChanged(x, y + 1, true);
                    break;
                case 3:     //Sima L
                    _gameTable[x, y] = true;
                    OnFieldChanged(x, y, true);
                    _gameTable[x + 1, y] = true;
                    OnFieldChanged(x + 1, y, true);
                    _gameTable[x + 1, y + 1] = true;
                    OnFieldChanged(x + 1, y + 1, true);
                    break;
                case 4:     //Tukrozott L
                    _gameTable[x, y] = true;
                    OnFieldChanged(x, y, true);
                    _gameTable[x, y + 1] = true;
                    OnFieldChanged(x, y + 1, true);
                    _gameTable[x + 1, y + 1] = true;
                    OnFieldChanged(x + 1, y + 1, true);
                    break;
            }
        }

        private void CheckFilledLines() //Ha egy adott so telitett akkor kifehérit
        {
            bool[,] tableCopy = new bool[4, 4];  //Azert kell a copy, mert ha egy blokk lerakasaval több sor is kigyűlik,
                                                 //azt igy lehet lekezelni
            for (Int32 i = 0; i < 4; i++)
            {
                for (Int32 j = 0; j < 4; j++)
                {
                    tableCopy[i, j] = _gameTable[i, j];
                }
            }

            for (Int32 i = 0; i < 4; i++)  // Sorok csekkolása
            {
                if (tableCopy[i, 0] && tableCopy[i, 1] && tableCopy[i, 2] && tableCopy[i, 3])
                {
                    _gameTable[i, 0] = false;
                    _gameTable[i, 1] = false;
                    _gameTable[i, 2] = false;
                    _gameTable[i, 3] = false;
                    OnLineFilled();
                }
            }
            for (Int32 i = 0; i < 4; i++)   // Oszlopok csekkolása
            {
                if (tableCopy[0, i] && tableCopy[1, i] && tableCopy[2, i] && tableCopy[3, i])
                {
                    _gameTable[0, i] = false;
                    _gameTable[1, i] = false;
                    _gameTable[2, i] = false;
                    _gameTable[3, i] = false;
                    OnLineFilled();
                }
            }
        }

        private void CheckNextCantBePlaced()        //Ha 1 esetre is teljesül akkor true lesz
        {
            bool canBePlaced = false;
            switch (_nextBlockType)
            {
                case 1:     //Lefele 2es
                    for (Int32 i = 0; i < 3; i++)
                    {
                        for (Int32 j = 0; j < 4; j++)
                        {
                            if (!_gameTable[i, j] && !_gameTable[i + 1, j])
                            {
                                canBePlaced = true;
                                break;
                            }
                        }
                    }
                    break;
                case 2:     //Oldalra 2es
                    for (Int32 i = 0; i < 4; i++)
                    {
                        for (Int32 j = 0; j < 3; j++)
                        {
                            if (!_gameTable[i, j] && !_gameTable[i, j + 1])
                            {
                                canBePlaced = true;
                                break;
                            }
                        }
                    }
                    break;
                case 3:     //Sima L
                    for (Int32 i = 0; i < 3; i++)
                    {
                        for (Int32 j = 0; j < 3; j++)
                        {
                            if (!_gameTable[i, j] && !_gameTable[i + 1, j] && !_gameTable[i + 1, j + 1])
                            {
                                canBePlaced = true;
                                break;
                            }
                        }
                    }
                    break;
                case 4:     //Tukrozott L
                    for (Int32 i = 0; i < 3; i++)
                    {
                        for (Int32 j = 0; j < 3; j++)
                        {
                            if (!_gameTable[i, j] && !_gameTable[i, j + 1] && !_gameTable[i + 1, j + 1])
                            {
                                canBePlaced = true;
                                break;
                            }
                        }
                    }
                    break;
            }
            if (!canBePlaced)
                OnGameOver(_points);
        }

        #endregion


        #region events/event methods
        public event EventHandler? PointChanged;
        public event EventHandler<FieldChangeEventArgs>? FieldChanged;
        public event EventHandler? LineFilled;
        public event EventHandler? NextBlockChanged;
        public event EventHandler<int>? GameOver;

        private void OnPointChanged()
        {
            PointChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnFieldChanged(Int32 x, Int32 y, bool isBlue)
        {
            FieldChanged?.Invoke(this, new FieldChangeEventArgs(x, y, isBlue));
        }

        private void OnLineFilled()
        {
            LineFilled?.Invoke(this, EventArgs.Empty);
        }
        private void OnNextBlockChanged()
        {
            NextBlockChanged?.Invoke(this, EventArgs.Empty);
        }
        private void OnGameOver(int points)
        {
            GameOver?.Invoke(this, points);
        }
        #endregion






    }
}
