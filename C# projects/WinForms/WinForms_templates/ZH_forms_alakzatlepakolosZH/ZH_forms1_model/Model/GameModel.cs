using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace ZH_forms1_model.Model
{
    public class GameModel
    {
        #region Fields
        private GameField[,] _gameTable = null!;        //játéktábla
        private GameField[,] _blockTable = null!;       //blockTábla
        private int _tableSize = 5;                     //előre definiált pálya méret
        private readonly Random rand;

        private Player currentPlayer;                   //melyik játékos jön épp
        private GameField[] fstFigures;                 //játkosok bábuinak pozíciója
        private GameField[] sndFigures;

        private int fstPoints;                          //játkosok pontjai
        private int sndPoints;

        private int gameSteps;                          //játék végéig maradt lépések száma


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

        public bool GetBlockField(int x, int y)      //Lekérdezzük blockTable kitöltötségét
        {
            return _blockTable[x, y].isFiled;
        }

        public Player GetPlayer()      //Lekérdezzük melyik játékos jön
        {
            return currentPlayer;
        }

        public int GetFstPoints()      //Lekérdezzük fst pontot
        {
            return fstPoints;
        }

        public int GetSndPoints()      //Lekérdezzük fst pontot
        {
            return sndPoints;
        }

        #endregion


        public GameModel() 
        {
            rand = new Random();
            currentPlayer = Player.FstPlayer;                       //Első játékos kezdi lépést
            fstFigures = new GameField[tableSize*tableSize];       //beégetve max foglalt kockája lehet
            sndFigures = new GameField[tableSize*tableSize];       //beégetve max foglalt kockája lehet
            fstPoints = 0; sndPoints = 0;

            gameSteps = _tableSize * _tableSize;
        }


        #region menu Methods
        public void modelNewGame()
        {
            fstFigures = new GameField[tableSize * tableSize];       //beégetve max foglalt kockája lehet
            sndFigures = new GameField[tableSize * tableSize];
            fstPoints = 0; sndPoints = 0;
            currentPlayer = Player.FstPlayer;

            createGametable();
            createBlocktable();
            onNewGame(tableSize); //korábbi táblaméret lekérdez
        }

        public void modelSetTable5x5()
        {
            tableSize = 5;
            createGametable();
            onNewGame(tableSize);
        }

        public void modelSetTable7x7()
        {
            tableSize = 7;
            createGametable();
            onNewGame(tableSize);
        }

        public void modelSetTable9x9()
        {
            tableSize = 9;
            createGametable();
            onNewGame(tableSize);
        }
        #endregion


        #region private Methods
        private void createGametable()              //Tábla generálása
        {
            gameSteps = _tableSize*_tableSize;      //hátralévő lépsek száma


            _gameTable = new GameField[_tableSize, _tableSize];
            for (int i = 0; i < _tableSize; i++)
            {
                for (int j = 0; j < _tableSize; j++)
                {
                    _gameTable[i, j] = new GameField(i, j);
                }
            }
        }

        private void createBlocktable()              //Tábla generálása
        {
            //bombák listálya
            var Shapes = new List<int>();

            for (int i = 0; i < 5; i++)     //5 kockát szinezünk
            {
                int shape = rand.Next(1, 9);
                while (Shapes.Contains(shape)) { shape = rand.Next(1, 9); }
                Shapes.Add(shape);
            }

            int index = 1;                  //elhelyezett grid buttonok indexelése
            _blockTable = new GameField[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _blockTable[i, j] = new GameField(i, j);
                    if (Shapes.Contains(index))
                    {
                        _blockTable[i, j].isFiled = true;
                    }
                    index++;
                }
            }
        }

        private bool checkGameOver()                //Ha az összes bomba felrobbant vége a játéknak
        {
            if (gameSteps == 0)
            {
                return true;
            }
            return false;
        }

        private bool validstep(int row, int col)      
        {
            if (row == 0 && col == 0 ||
                row == 0 && col == _tableSize - 1 ||
                row == _tableSize - 1 && col == 0 ||
                row == _tableSize - 1 && col == _tableSize - 1)
            // érvénytelen kattintás a pálya sarokba mindig, mert legalább 5 kocka lehet szinezve
            {
                return false;
            }
            else if (row == 0) //ha felső sorba teszi
            {
                int filledblockcounter = 5;    //3x3-as rácsnak csak alsó 2 sorát viszgálom!
                for (int i = 0; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (_blockTable[1 + i, 1 + j].isFiled == true)
                        {
                            filledblockcounter--;
                        }
                    }
                }
                if (filledblockcounter == 0)          //vizsgáljuk megvan-e az 5 kitöltött kocka
                {
                    //Ha igen, végrehajtjuk a lepakolást:
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            if (_blockTable[1 + i, 1 + j].isFiled == true) //3x3-as rács adott kockája ha szinezve van azt pakolom le
                            {
                                //pontok kiosztása:
                                if (_blockTable[1 + i, 1 + j].isFiled == false) //ha még üres a kocka 1p plusz
                                {
                                    if (currentPlayer == Player.FstPlayer)
                                    {
                                        fstPoints += 1;
                                    }
                                    else
                                    {
                                        sndPoints += 1;
                                    }
                                }
                                else if (_blockTable[1 + i, 1 + j].isFiled == true)  //ha már nem üres a kocka 2p plusz
                                {
                                    if (currentPlayer == Player.FstPlayer)
                                    {
                                        fstPoints += 1;
                                    }
                                    else
                                    {
                                        sndPoints += 1;
                                    }
                                }

                                _gameTable[row + i, col + j].isFiled = true;
                                _gameTable[row + i, col + j].player = currentPlayer;
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (row == _tableSize-1) //ha alsó sorba teszi
            {
                int filledblockcounter = 5;
                for (int i = 0; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (_blockTable[1 - i, 1 + j].isFiled == true)
                        {
                            filledblockcounter--;
                        }
                    }
                }
                if (filledblockcounter == 0)          //vizsgáljuk megvan-e az 5 kitöltött kocka
                {
                    //Ha igen, végrehajtjuk a lepakolást:
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            if (_blockTable[1 - i, 1 + j].isFiled == true)
                            {

                                //pontok kiosztása:
                                if (_blockTable[1 - i, 1 + j].isFiled == false) //ha még üres a kocka 1p plusz
                                {
                                    if (currentPlayer == Player.FstPlayer)
                                    {
                                        fstPoints += 1;
                                    }
                                    else
                                    {
                                        sndPoints += 1;
                                    }
                                }
                                else if (_blockTable[1 - i, 1 + j].isFiled == true)  //ha már nem üres a kocka 2p plusz
                                {
                                    if (currentPlayer == Player.FstPlayer)
                                    {
                                        fstPoints += 1;
                                    }
                                    else
                                    {
                                        sndPoints += 1;
                                    }
                                }

                                _gameTable[row - i, col + j].isFiled = true;
                                _gameTable[row - i, col + j].player = currentPlayer;
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (col == 0) //ha balszélső oszlopba teszi
            {
                int filledblockcounter = 5;
                for (int i = -1; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (_blockTable[1 + i, 1 + j].isFiled == true)
                        {
                            filledblockcounter--;
                        }
                    }
                }
                if (filledblockcounter == 0)          //vizsgáljuk megvan-e az 5 kitöltött kocka
                {
                    //Ha igen, végrehajtjuk a lepakolást:
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            if (_blockTable[1 + i, 1 + j].isFiled == true)
                            {

                                //pontok kiosztása:
                                if (_blockTable[1 + i, 1 + j].isFiled == false) //ha még üres a kocka 1p plusz
                                {
                                    if (currentPlayer == Player.FstPlayer)
                                    {
                                        fstPoints += 1;
                                    }
                                    else
                                    {
                                        sndPoints += 1;
                                    }
                                }
                                else if (_blockTable[1 + i, 1 + j].isFiled == true)  //ha már nem üres a kocka 2p plusz
                                {
                                    if (currentPlayer == Player.FstPlayer)
                                    {
                                        fstPoints += 1;
                                    }
                                    else
                                    {
                                        sndPoints += 1;
                                    }
                                }

                                _gameTable[row + i, col + j].isFiled = true;
                                _gameTable[row + i, col + j].player = currentPlayer;
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (col == _tableSize - 1) //ha jobbszélső oszlopba teszi
            {
                int filledblockcounter = 5;
                for (int i = -1; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (_blockTable[1 + i, 1 - j].isFiled == true)
                        {
                            filledblockcounter--;
                        }
                    }
                }
                if (filledblockcounter == 0)          //vizsgáljuk megvan-e az 5 kitöltött kocka
                {
                    //Ha igen, végrehajtjuk a lepakolást:
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            if (_blockTable[1 + i, 1 - j].isFiled == true)
                            {

                                //pontok kiosztása:
                                if (_blockTable[1 + i, 1 - j].isFiled == false) //ha még üres a kocka 1p plusz
                                {
                                    if (currentPlayer == Player.FstPlayer)
                                    {
                                        fstPoints += 1;
                                    }
                                    else
                                    {
                                        sndPoints += 1;
                                    }
                                }
                                else if (_blockTable[1 + i, 1 - j].isFiled == true)  //ha már nem üres a kocka 2p plusz
                                {
                                    if (currentPlayer == Player.FstPlayer)
                                    {
                                        fstPoints += 1;
                                    }
                                    else
                                    {
                                        sndPoints += 1;
                                    }
                                }

                                _gameTable[row + i, col - j].isFiled = true;
                                _gameTable[row + i, col - j].player = currentPlayer;
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else   //egyébként meg kitölt csak simán
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (_blockTable[1 + i, 1 + j].isFiled == true)
                        {
                            //pontok kiosztása:
                            if (_blockTable[1 + i, 1 + j].isFiled == false) //ha még üres a kocka 1p plusz
                            {
                                if (currentPlayer == Player.FstPlayer)
                                {
                                    fstPoints += 1;
                                }
                                else
                                {
                                    sndPoints += 1;
                                }
                            }
                            else if (_blockTable[1 + i, 1 + j].isFiled == true)  //ha már nem üres a kocka 2p plusz
                            {
                                if (currentPlayer == Player.FstPlayer)
                                {
                                    fstPoints += 1;
                                }
                                else
                                {
                                    sndPoints += 1;
                                }
                            }

                            _gameTable[row + i, col + j].isFiled = true;
                            _gameTable[row + i, col + j].player = currentPlayer;
                        }
                    }
                }

                return true;
            }
        }

        #endregion


        #region public Methods
        public void modelButtonClicked(int row, int col)                //Gombot lenyomták
        {
            if (validstep(row, col))
            {
                
                gameSteps -= 1;
                onStepsChage();

                createBlocktable();
                if (currentPlayer == Player.FstPlayer)        //következő játékos aki lépni fog
                {
                    currentPlayer = Player.SndPlayer;
                }
                else
                {
                    currentPlayer = Player.FstPlayer;
                }
                onGameAdvance(_gameTable, _blockTable);

                //ha összes bomba felrobbant vége a játéknak
                if (checkGameOver())
                {
                    onGameOver(true);
                }
            }
            else
            {
                onIllegalStep();
            }


            /*
            bombUsedConter += 1; //bomba használata megtörént, növeljük

            //3x3-as hatósugár:
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


        }

        #endregion


        #region events/event methods
        public event EventHandler<NewGameEventArgs>? NewGame;
        public event EventHandler<GameAdvanceEventArgs>? GameAdvance;
        public event EventHandler<GameOverEventArgs>? GameOver;
        public event EventHandler<int>? StepsChange;
        public event EventHandler? IllegalStep;

        private void onNewGame(int size)
        {
            NewGame?.Invoke(this, new NewGameEventArgs(size));
        }
        private void onGameAdvance(GameField[,] gameTable, GameField[,] blockTable)                      //lekezeli a szinek változását a pályán 
        {
            GameAdvance?.Invoke(this, new GameAdvanceEventArgs(gameTable, blockTable));
        }
        private void onGameOver(bool isWon)
        {
            GameOver?.Invoke(this, new GameOverEventArgs(isWon));
        }
        private void onStepsChage()
        {
            StepsChange?.Invoke(this, gameSteps);
        }
        private void onIllegalStep()
        {
            IllegalStep?.Invoke(this, EventArgs.Empty);
        }
        #endregion

    }
}
