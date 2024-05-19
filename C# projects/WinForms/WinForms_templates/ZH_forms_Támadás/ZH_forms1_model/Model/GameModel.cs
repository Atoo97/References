using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ZH_forms1_model.Model.GameField;

namespace ZH_forms1_model.Model
{
    public class GameModel
    {
        #region Fields
        private GameField[,] _gameTable = null!;
        private int _tableSize = 6;                //előre definiált pálya méret

        private Player currentPlayer;
        private int currentfigureNum;

        private GameField[] fstFigure; 
        private GameField[] sndFigure;
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

        #endregion


        public GameModel() 
        {
            currentPlayer = Player.FstPlayer;   //Első játékos kezdi lépést
            currentfigureNum = 1;               //beégetve 1-es babuval lépnek
            fstFigure = new GameField[4];       //beégetve 4 babu lehet
            sndFigure = new GameField[4];       //beégetve 4 babu lehet
        }


        #region menu Methods
        public void modelNewGame()
        {
            createGametable();
            onNewGame(tableSize); //korábbi táblaméret lekérdez

            onGameAdvance(_gameTable, currentPlayer, currentfigureNum);
        }

        public void modelSetTable4x4()
        {
            tableSize = 4;
            createGametable();
            onNewGame(tableSize);
        }

        public void modelSetTable6x6()
        {
            tableSize = 6;
            createGametable();
            onNewGame(tableSize);
        }

        public void modelSetTable8x8()
        {
            tableSize = 8;
            createGametable();
            onNewGame(tableSize);
        }
        #endregion


        #region private Methods
        private void createGametable()              //Tábla generálása
        {
            _gameTable = new GameField[_tableSize, _tableSize];
            for (int i = 0; i < _tableSize; i++)
            {
                for (int j = 0; j < _tableSize; j++)
                {
                    _gameTable[i, j] = new GameField(i, j);
                }
            }

            //first player dummys
            _gameTable[(_tableSize - 1), 0].player = Player.FstPlayer;      //legalsó sarokba tesz
            _gameTable[(_tableSize - 1), 0].playerFigure = 4;               //megadom neki az értékét
            fstFigure[3] = _gameTable[(_tableSize - 1), 0];                //Hozzáadom a player1 gyűjteményhez a bábut

            _gameTable[(_tableSize - 1), 1].player = Player.FstPlayer;
            _gameTable[(_tableSize - 1), 1].playerFigure = 3;
            fstFigure[2] = _gameTable[(_tableSize - 1), 1];

            _gameTable[(_tableSize - 2), 1].player = Player.FstPlayer;
            _gameTable[(_tableSize - 2), 1].playerFigure = 2;
            fstFigure[1] = _gameTable[(_tableSize - 2), 1];

            _gameTable[(_tableSize - 2), 0].player = Player.FstPlayer;
            _gameTable[(_tableSize - 2), 0].playerFigure = 1;
            fstFigure[0] = _gameTable[(_tableSize - 2), 0];


            //second player dummys
            _gameTable[0, (_tableSize - 1)].player = Player.SndPlayer;       //legfelső sarokba tesz
            _gameTable[0, (_tableSize - 1)].playerFigure = 4;               //megadom neki az értékét
            sndFigure[3] = _gameTable[0, (_tableSize - 1)];                //Hozzáadom a player2 gyűjteményhez a bábut

            _gameTable[0, (_tableSize - 2)].player = Player.SndPlayer;
            _gameTable[0, (_tableSize - 2)].playerFigure = 3;
            sndFigure[2] = _gameTable[0, (_tableSize - 2)];

            _gameTable[1, (_tableSize - 1)].player = Player.SndPlayer;
            _gameTable[1, (_tableSize - 1)].playerFigure = 1;
            sndFigure[0] = _gameTable[1, (_tableSize - 1)];

            _gameTable[1, (_tableSize - 2)].player = Player.SndPlayer;
            _gameTable[1, (_tableSize - 2)].playerFigure= 2;
            sndFigure[1] = _gameTable[1, (_tableSize - 2)];

        }

        
        private void checkGameOver(GameField player)                //Ha a játékos kockája akadályba ütközik
        {
            
        }

        private bool validStep(int row, int col)                //Vizsgálni kell, hogy a szám körül lévő buttonbe akar-e lépni
        {
            if (_gameTable[row, col].player == Player.Empty) //Ha üres a kocka
            {
                if (currentPlayer == Player.FstPlayer) //player1-re vizsgál
                {
                    if (
                     //(jelen léptetni kivánt elem == kattintott sor -1 vagy +1) - megnézem oszloponként csusztatva majd soronként léptetve 1 távolságon belül maradok e.
                       ( (fstFigure[currentfigureNum - 1].row == row + 1 || fstFigure[currentfigureNum - 1].row == row || fstFigure[currentfigureNum - 1].row == row - 1) && fstFigure[currentfigureNum - 1].col == col - 1) ||
                       ( (fstFigure[currentfigureNum - 1].row == row + 1 || fstFigure[currentfigureNum - 1].row == row || fstFigure[currentfigureNum - 1].row == row - 1) && fstFigure[currentfigureNum - 1].col == col) ||
                       ( (fstFigure[currentfigureNum - 1].row == row + 1 || fstFigure[currentfigureNum - 1].row == row || fstFigure[currentfigureNum - 1].row == row - 1) && fstFigure[currentfigureNum - 1].col == col + 1)
                       )
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (
                       ((sndFigure[currentfigureNum - 1].row == row + 1 || sndFigure[currentfigureNum - 1].row == row || sndFigure[currentfigureNum - 1].row == row - 1) && sndFigure[currentfigureNum - 1].col == col - 1) ||
                       ((sndFigure[currentfigureNum - 1].row == row + 1 || sndFigure[currentfigureNum - 1].row == row || sndFigure[currentfigureNum - 1].row == row - 1) && sndFigure[currentfigureNum - 1].col == col) ||
                       ((sndFigure[currentfigureNum - 1].row == row + 1 || sndFigure[currentfigureNum - 1].row == row || sndFigure[currentfigureNum - 1].row == row - 1) && sndFigure[currentfigureNum - 1].col == col + 1)
                       )
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        #endregion


        #region public Methods        
        public void modelButtonClicked(int row, int col)                //Gombot lenyomták
        {
            if (validStep(row, col)) 
            {
                if (currentPlayer == Player.FstPlayer)
                {
                    //jelenlegi player buttont normálra állítjuk
                    _gameTable[fstFigure[currentfigureNum - 1].row, fstFigure[currentfigureNum - 1].col].player = Player.Empty;
                    _gameTable[fstFigure[currentfigureNum - 1].row, fstFigure[currentfigureNum - 1].col].playerFigure = 0;

                    //kattintott button kitölt player1-re
                    _gameTable[row, col].player = Player.FstPlayer;
                    _gameTable[row, col].playerFigure = currentfigureNum;

                    //eltárolt figurák pozija is átír
                    fstFigure[currentfigureNum - 1].row = row;
                    fstFigure[currentfigureNum - 1].col = col;

                    currentPlayer = Player.SndPlayer;
                }
                else if (currentPlayer == Player.SndPlayer)
                {
                    _gameTable[sndFigure[currentfigureNum - 1].row, sndFigure[currentfigureNum - 1].col].player = Player.Empty;
                    _gameTable[sndFigure[currentfigureNum - 1].row, sndFigure[currentfigureNum - 1].col].playerFigure = 0;

                    _gameTable[row, col].player = Player.SndPlayer;
                    _gameTable[row, col].playerFigure = currentfigureNum;

                    sndFigure[currentfigureNum - 1].row = row;
                    sndFigure[currentfigureNum - 1].col = col;

                    currentPlayer = Player.FstPlayer;
                    if (currentfigureNum == 4)
                    {
                        currentfigureNum = 0;
                    }
                    currentfigureNum++;
                }
                onGameAdvance(_gameTable, currentPlayer, currentfigureNum);
            }
        }

        #endregion


        #region events/event methods
        public event EventHandler<NewGameEventArgs>? NewGame;
        public event EventHandler<GameAdvanceEventArgs>? GameAdvance;
        public event EventHandler<GameOverEventArgs>? GameOver;

        private void onNewGame(int size)
        {
            NewGame?.Invoke(this, new NewGameEventArgs(size));                                         // Pályagenerálás után elhelyezzük a játékost
        }
        private void onGameAdvance(GameField[,] gameTable, Player player, int round)                    //lekezeli a játékos lépéseinek változását
        {
            GameAdvance?.Invoke(this, new GameAdvanceEventArgs(gameTable, currentPlayer, currentfigureNum));
        }
        private void onGameOver(bool isWon)
        {
            GameOver?.Invoke(this, new GameOverEventArgs(isWon));
        }
        #endregion

    }
}
