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
        private GameField[,] _gameTable = null!;
        private int _tableSize = 9;                   //előre definiált pálya méret
        private int gameTime = 0;                    //előre definiált idő

        private int bombsnumber;                    //pályán elhelyezett bombák száma
        private readonly Random rand;

        private int bombUsedConter = 0;             //bombahasználatok száma
        #endregion


        #region Getters/Setters
        public int tableSize
        {
            get
            {
                return _tableSize;
            }
            set
            {
                _tableSize = value;
            }
        }

        public bool GetGameField(int x, int y)      //Lekérdezzük bomba-e és szinét a viewban ugy modositom
        {
            return _gameTable[x, y].isBomb;
        }

        public int BombUsedCounter() 
        {
            return bombUsedConter;
        }
        #endregion


        public GameModel() 
        {
            rand = new Random();
        }


        #region menu Methods
        public void modelNewGame()
        {
            bombsnumber = 2 * tableSize;
            bombUsedConter = 0;
            createGametable();
            onNewGame(tableSize); //korábbi táblaméret lekérdez
        }

        public void modelSetTable9x9()
        {
            tableSize = 9;
            bombsnumber = 2 * tableSize;
            createGametable();
            onNewGame(tableSize);
        }

        public void modelSetTable13x13()
        {
            tableSize = 13;
            bombsnumber = 2 * tableSize;
            createGametable();
            onNewGame(tableSize);
        }

        public void modelSetTable17x17()
        {
            tableSize = 17;
            bombsnumber = 2 * tableSize;
            createGametable();
            onNewGame(tableSize);
        }
        #endregion


        #region private Methods
        private void createGametable()              //Tábla generálása
        {
            gameTime = 0; //idő kezdeti értéke

            //bombák listálya
            var Bombs = new List<int>();

            for (int i = 0; i < bombsnumber; i++)
            {
                int bomb = rand.Next(1, (_tableSize * _tableSize) - 1);
                while (Bombs.Contains(bomb)) { bomb = rand.Next(1, (_tableSize * _tableSize) - 1); }
                Bombs.Add(bomb);
            }

            int index = 1; //elhelyezett grid buttonok indexelése
            _gameTable = new GameField[_tableSize, _tableSize];
            for (int i = 0; i < _tableSize; i++)
            {
                for (int j = 0; j < _tableSize; j++)
                {
                    _gameTable[i, j] = new GameField(i, j);
                    if (Bombs.Contains(index))
                    {
                        _gameTable[i, j].isBomb = true;
                    }
                    index++;
                }
            }
        }

        private bool checkGameOver()                //Ha az összes bomba felrobbant vége a játéknak
        {
            if (bombsnumber == 0)
            {
                return true;
            }
            return false;
        }

        #endregion


        #region public Methods
        public void modelButtonClicked(int row, int col)                //Gombot lenyomták
        {
            bombUsedConter += 1; //bomba használata megtörént, növeljük

            //3x3-as hatósugár:
            for (Int32 i = row - 1; i <= row + 1; i++)  //col és row-hoz képest tolom +-1 irányba
            {
                for (Int32 j = col - 1; j <= col + 1; j++)  //button a 3x3 vizsgált mátrix közepe
                {
                    // >= vel kikötöm ne indexeljem túl, és vizsgálom a 3x3 mátrik középső elemét is
                    if (i >= 0 && i <= _tableSize - 1 && j >= 0 && j <= tableSize - 1 && _gameTable[i, j].isBomb)
                    {
                        _gameTable[i, j].isBomb = false;
                        _gameTable[i, j].isFound = true;
                        bombsnumber--;
                    }
                }
            }


            /* Ez itt törölhető kód csak példa
            if (row == 0 && col == 0) // bal felső sarok
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (_gameTable[row+i, col+j].isBomb == true)
                        {
                            _gameTable[row+i, col+j].isBomb = false;
                            _gameTable[row + i, col + j].isFound = true;
                            bombsnumber--;
                        }
                    }
                }
            }
            else if (row == 0 && col == _tableSize - 1) // jobb felső sarok
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (_gameTable[row + i, col - j].isBomb == true)
                        {
                            _gameTable[row + i, col - j].isBomb = false;
                            _gameTable[row + i, col - j].isFound = true;
                            bombsnumber--;
                        }
                    }
                }
            }
            else if (row == _tableSize - 1 && col == 0) // bal alsó sarok
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (_gameTable[row - i, col + j].isBomb == true)
                        {
                            _gameTable[row - i, col + j].isBomb = false;
                            _gameTable[row - i, col + j].isFound = true;
                            bombsnumber--;
                        }
                    }
                }
            }
            else if (row == _tableSize - 1 && col == _tableSize - 1) // jobb alsó sarok
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (_gameTable[row - i, col - j].isBomb == true)
                        {
                            _gameTable[row - i, col - j].isBomb = false;
                            _gameTable[row - i, col - j].isFound = true;
                            bombsnumber--;
                        }
                    }
                }
            }
            else if (row==0) //ha csak felső sorban van valahol klikk
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (_gameTable[row + i, col + j].isBomb == true)
                        {
                            _gameTable[row + i, col + j].isBomb = false;
                            _gameTable[row + i, col + j].isFound = true;
                            bombsnumber--;
                        }
                    }
                }
            }
            else if(row == _tableSize - 1)    //ha csak alsó sorban van valahol klikk
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (_gameTable[row - i, col + j].isBomb == true)
                        {
                            _gameTable[row - i, col + j].isBomb = false;
                            _gameTable[row - i, col + j].isFound = true;
                            bombsnumber--;
                        }
                    }
                }
            }
            else if (col== 0)    //ha csak balszélső oszlopban van valahol klikk
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (_gameTable[row + i, col + j].isBomb == true)
                        {
                            _gameTable[row + i, col + j].isBomb = false;
                            _gameTable[row + i, col + j].isFound = true;
                            bombsnumber--;
                        }
                    }
                }
            }
            else if (col == _tableSize - 1)    //ha csak balszélső oszlopban van valahol klikk
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (_gameTable[row + i, col - j].isBomb == true)
                        {
                            _gameTable[row + i, col - j].isBomb = false;
                            _gameTable[row + i, col - j].isFound = true;
                            bombsnumber--;
                        }
                    }
                }
            }
            else   //különben mindehol 3x3 körbe pusztítson
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (_gameTable[row + i, col + j].isBomb == true)
                        {
                            _gameTable[row + i, col + j].isBomb = false;
                            _gameTable[row + i, col + j].isFound = true;
                            bombsnumber--;
                        }
                    }
                }
            }
            */

            onGameAdvance(_gameTable);

            //ha összes bomba felrobbant vége a játéknak
            if (checkGameOver())
            {
                onGameOver(true);
            }

        }

        public void modelCalculateGameTime()
        {
            gameTime += 1;
            onCalculateTime();
        }

        #endregion


        #region events/event methods
        public event EventHandler<NewGameEventArgs>? NewGame;
        public event EventHandler<GameAdvanceEventArgs>? GameAdvance;
        public event EventHandler<GameOverEventArgs>? GameOver;
        public event EventHandler<int>? CalculateTime;

        private void onNewGame(int size)
        {
            NewGame?.Invoke(this, new NewGameEventArgs(size));
        }
        private void onGameAdvance(GameField[,] gameTable)                      //lekezeli a szinek változását a pályán 
        {
            GameAdvance?.Invoke(this, new GameAdvanceEventArgs(gameTable));
        }
        private void onGameOver(bool isWon)
        {
            GameOver?.Invoke(this, new GameOverEventArgs(isWon));
        }
        private void onCalculateTime()
        {
            CalculateTime?.Invoke(this, gameTime);
        }
        #endregion

    }
}
