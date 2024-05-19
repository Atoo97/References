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
        private GameField player = null!;
        private int _tableSize = 10;                //előre definiált pálya méret
        private int gameTime = 0;                    //előre definiált idő

        private readonly Random rand;
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

        public bool GetGameField(int x, int y)      //Lekérdezzük akadáyl-e és szinét a viewban ugy modositom
        {
                return _gameTable[x,y].border;
        }

        #endregion


        public GameModel() 
        {
            rand = new Random();
        }


        #region menu Methods
        public void modelNewGame()
        {
            createGametable();
            onNewGame(tableSize); //korábbi táblaméret lekérdez
        }

        public void modelSetTable10x10()
        {
            tableSize = 10;
            createGametable();
            onNewGame(tableSize);
        }

        public void modelSetTable20x20()
        {
            tableSize = 20;
            createGametable();
            onNewGame(tableSize);
        }
        #endregion


        #region private Methods
        private void createGametable()              //Tábla generálása
        {
            gameTime = 0; //idő kezdeti értéke


            //elhelyezhető akadályok száma
            int bordersNum = (_tableSize * _tableSize) / 10;

            //akadályok listálya
            var Borders = new List<int>();

            for (int i = 0; i < bordersNum; i++)
            {
                int border = rand.Next(1, (_tableSize * _tableSize) - 1);
                while (Borders.Contains(border)) { border = rand.Next(1, (_tableSize * _tableSize) - 1); }
                Borders.Add(border);
            }


            int index = 1;
            _gameTable = new GameField[_tableSize, _tableSize];
            for (int i = 0; i < _tableSize; i++)
            {
                for (int j = 0; j < _tableSize; j++)
                {
                    _gameTable[i, j] = new GameField(i, j, index);
                    if (Borders.Contains(index))
                    {
                        _gameTable[i, j].border = true;
                    }
                    index++;
                }
            }

            player = _gameTable[rand.Next(0, _tableSize-1), rand.Next(0, _tableSize-1)];     //játékos elhelyez a pályán random
            //onGameAdvance(player, 5);                                                        //5 bc the player just spawning - - nincs lekezelt case hozzá

        }

        
        private void checkGameOver(GameField player)                //Ha a játékos kockája akadályba ütközik
        {
            if (player.border)
            {
                onGameOver(false);
            }
        }
        

        #endregion


        #region public Methods
        public void stepPlayer(int direction) //0 : /\, 1 : >, 2 : \/, 3 : <
        {

            //játék vége ha rossz lépés:
            if (player.row == 0 && direction == 0 ||                    //első sor és felfele lép
                player.row == tableSize - 1 && direction == 2 ||        //utolsó sor és lefele lép
                player.col == 0 && direction == 3 ||                    //első oszlop és balra lép
                player.col == tableSize - 1 && direction == 1)          //utolsó oszlop és jobbra lép
            {
                onGameOver(false);
            }
            else
            {
                switch (direction)
                {
                    case 0:
                        player = _gameTable[player.row - 1, player.col];
                        checkGameOver(player);
                        break;
                    case 1:
                        player = _gameTable[player.row, player.col + 1];
                        checkGameOver(player);
                        break;
                    case 2:
                        player = _gameTable[player.row + 1, player.col];
                        checkGameOver(player);
                        break;
                    case 3:
                        player = _gameTable[player.row, player.col - 1];
                        checkGameOver(player);
                        break;
                    default:
                        break;
                }
                onGameAdvance(player, direction);
            }
        }

        /*
        public void modelButtonClicked(int row, int col)                //Gombot lenyomták
        {
            if (_gameTable[row, col].isBlack)
            {
                _gameTable[row, col].isBlack = false;
            }
            else
            {
                _gameTable[row, col].isBlack = true;
            }

            onGameAdvance(_gameTable);

            if (checkGameOver())
            {
                onGameOver(true);
            }
        }
        */

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
            onGameAdvance(player, 5);                                          // Pályagenerálás után elhelyezzük a játékost
        }
        private void onGameAdvance(GameField player, int direction)             //lekezeli a játékos lépéseinek változását
        {
            GameAdvance?.Invoke(this, new GameAdvanceEventArgs(player.row, player.col, direction));
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
