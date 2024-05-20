using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SnakeGame.Model;
using SnakeLib.Persistence;

namespace SnakeLib.Model
{
    /// <summary>
    /// Kígyó mozgásának irányának.
    /// </summary>
    public enum Direction { goLeft, goRight, goUp, goDown }

    /// <summary>
    /// Játéktábla méretének típusa.
    /// </summary>
    public enum GameTableSize { Small, Medium, Large }



    public class SnakeModel
    {
        #region Fields
        private readonly Random rand;
        private int randomNum;
        private ISnakeDataAccess _dataAccess; // adatelérés
        private SnakeTable _table; // játéktábla
        private bool _gamePause; //játék végét jelző státusz
        private int snakeHeadCoordinate = 1; //kigyó fejének koordinátája

        //snake jákhoz tartozó megjelenítendő vizuális elemek a pályán
        private readonly List<SnakeField> Snake;

        //Eventargs fileds
        private Direction _gameDirection;       // utolsó irányváltó művelet
        private Int32 _gameScoresCount;         // aktuális pontok száma
        private Int32 _gameHighScoresCount;     // legtöbb pont pontszáma
        private String _startbuttonText;         // start gomb felirata

        #endregion


        #region Properties
        /// <summary>
        /// Randomszám lekérdezése.
        /// </summary>
        public int RandomNum { get { return randomNum; } }

        /// <summary>
        /// Játéktábla lekérdezése.
        /// </summary>
        public GameTableSize GameTableSize { get; set; }

        /// <summary>
        /// Beolvasott játéktábla lekérdezése.
        /// </summary>
        public SnakeTable Table { get { return _table!; } }

        /// <summary>
        /// Játék végének lekérdezése és beállítása.
        /// </summary>
        public Boolean IsGamePaused { get { return (_gamePause == true); } }
        public void SetGamePaused(Boolean p) { _gamePause = p; }

        /// <summary>
        /// Pontok lekérdezése.
        /// </summary>
        public Int32 GameScores { get { return _gameScoresCount; } }
        public Int32 GameHighScores { get { return _gameHighScoresCount; } }

        /// <summary>
        /// Pályán elhelyezett kigyórészek koordinátáinak lekérdezése.
        /// </summary>
        public List<SnakeField> GetSnake { get { return Snake; } }

        /// <summary>
        /// Gomb feliratának lekérdezése.
        /// </summary>
        public String StartbuttonText { get { return _startbuttonText; } }

        #endregion


        #region Events
        /// <summary>
        /// Pályaméret módosításának eseménye.
        /// </summary>
        public event EventHandler<int>? ChangeSize;  //Ezt majd lehet törölhető

        /// <summary>
        /// Játék előrehaladásának eseménye.
        /// </summary>
        public event EventHandler<SnakeEventArgs>? GameAdvanced;

        /// <summary>
        /// Gomb változásának eseménye.
        /// </summary>
        public event EventHandler<SnakeEventArgs>? StartButtonChange;

        /// <summary>
        /// Játék végének eseménye.
        /// </summary>
        public event EventHandler<SnakeEventArgs>? GameOver;

        /// <summary>
        /// Játék létrehozásának eseménye.
        /// </summary>
        public event EventHandler<SnakeEventArgs>? GameCreated;

        /// <summary>
        /// Canvas frissítésének eseménye.
        /// </summary>
        public event EventHandler<SnakeEventArgs>? CanvasUpgrade;

        #endregion


        #region Constructor
        /// <summary>
		/// Sudoku játék példányosítása.
		/// </summary>
		/// <param name="dataAccess">Az adatelérés.</param>
		public SnakeModel(ISnakeDataAccess dataAccess)
        {
            rand = new Random();
            Snake = new List<SnakeField>();
            _table = new SnakeTable();

            _dataAccess = dataAccess;
            _gameDirection = Direction.goLeft; //kigyó kezdő indulási iránya

            _gamePause = true; //kezdetben az idő nem tellik vala
            _gameScoresCount = 0;
            _gameHighScoresCount = 0;
            _startbuttonText = "Start";
        }

        #endregion


        #region Public methods
        /// <summary>
        /// Játék beolvasása fileból és tábla adatok kimentése.
        /// </summary>
        public async Task LoadGameAsync() 
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            //beolvas új pályaméretbe
            (_table) = await _dataAccess.LoadAsync(GameTableSize);
            OnChangeSize(Table.RegionSize);

            OnStartbuttonChange(_startbuttonText, false);


            //kigyó fejének helyezet kiszámít
            if (Table.RegionSize % 2 == 0)
            {
                snakeHeadCoordinate = ((Table.RegionSize * Table.RegionSize)/ 2)- Table.RegionSize/2;
            }
            else
            {
                snakeHeadCoordinate = (((Table.RegionSize * Table.RegionSize) + 1) / 2);
            }


        }


        /// <summary>
        /// Művelet végrehajtása.
        /// </summary>
        public void SetMove(Direction selectedDirection)
        {
            //direction beállítása
            switch (selectedDirection)
            {
                case Direction.goLeft:
                    if (_gameDirection != Direction.goRight)
                    {
                        _gameDirection = Direction.goLeft;
                    }
                    break;
                case Direction.goRight:
                    if (_gameDirection != Direction.goLeft)
                    {
                        _gameDirection = Direction.goRight;
                    }
                    break;
                case Direction.goUp:
                    if (_gameDirection != Direction.goDown)
                    {
                        _gameDirection = Direction.goUp;
                    }
                    break;
                case Direction.goDown:
                    if (_gameDirection != Direction.goUp)
                    {
                        _gameDirection = Direction.goDown;
                    }
                    break;
            }
        }


        public void AdvanceTime()
        {
            // ha szüneteltetve a játék akkor, nem folytathatjuk
            if (IsGamePaused)
            {
                OnStartbuttonChange("Continoue", false);
                return;
            }

            OnStartbuttonChange("Pause", false);

            for (int i = GetSnake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {

                    switch (_gameDirection)
                    {
                        case Direction.goLeft:
                            GetSnake[i].X--;
                            snakeHeadCoordinate = (snakeHeadCoordinate - 1)+Table.RegionSize;
                            break;
                        case Direction.goRight:
                            GetSnake[i].X++;
                            snakeHeadCoordinate = (snakeHeadCoordinate - 1) - Table.RegionSize;
                            break;
                        case Direction.goDown:
                            GetSnake[i].Y++;
                            snakeHeadCoordinate = (snakeHeadCoordinate - 1) + 1;
                            break;
                        case Direction.goUp:
                            GetSnake[i].Y--;
                            snakeHeadCoordinate = (snakeHeadCoordinate - 1) - 1;
                            break;
                    }

                    //End of the game if snake is leave the canvas
                    if (GetSnake[0].X < 0)
                    {
                        OnGameOver(true);
                    }
                    if (GetSnake[0].X >= Table.RegionSize)
                    {
                        OnGameOver(true);
                    }
                    if (GetSnake[0].Y >= Table.RegionSize)
                    {
                        OnGameOver(true);
                    }
                    if (GetSnake[0].Y < 0)
                    {
                        OnGameOver(true);
                    }
                    

                    //End of the game if snake is eat up his own body
                    for (int j = 1; j < GetSnake.Count; j++)
                    {
                        if (GetSnake[i].X == GetSnake[j].X
                            && GetSnake[i].Y == GetSnake[j].Y)
                        {
                            OnGameOver(false);
                        }
                    }

                    //Eat food
                    if (Table.FieldsCoordinate[randomNum].X == GetSnake[0].X
                        && Table.FieldsCoordinate[randomNum].Y == GetSnake[0].Y)
                    {
                        Eat();
                    }

                    //Eat the wall
                    for (int d = 0; d < Table.FieldsCoordinate.Count; d++)
                    {
                        if (Table.FieldsCoordinate[d].X == GetSnake[0].X 
                            && Table.FieldsCoordinate[d].Y == GetSnake[0].Y
                            && Table.FieldsCoordinate[d].Border
                            )
                        {
                            OnGameOver(true);
                        }
                    }

                }
                else
                {   //Here is the body tell what to do
                    GetSnake[i].X = GetSnake[i - 1].X;
                    GetSnake[i].Y = GetSnake[i - 1].Y;
                }
            }

            OnCanvasUpgrade();
        }

        public void Eat()
        {
            _gameScoresCount += 1;

            //ha aktuális pont több átad érétke00
            if (_gameScoresCount > _gameHighScoresCount) _gameHighScoresCount = _gameScoresCount;

            //Pontok változásának lekezelése
            OnGameAdvanced(_gameScoresCount, _gameHighScoresCount);

            //New body part position
            SnakeField body = new SnakeField();
            body.X = GetSnake[GetSnake.Count - 1].X;
            body.Y = GetSnake[GetSnake.Count - 1].Y;

            GetSnake.Add(body);

            //Tojás koordináta megszűntetése
            Table.FieldsCoordinate[randomNum].Food = false;

            //Tojás koordináta random generálása
            randomNum = rand.Next(0, Table.FieldsCoordinate.Count - 1);

            //Ellenőrzés a tojás nem generálodott-e le egy akadályra, ha igen újra generáljuk
            if (Table.FieldsCoordinate[randomNum].Border)
            {
                while (Table.FieldsCoordinate[randomNum].Border)
                {
                    randomNum = rand.Next(0, Table.FieldsCoordinate.Count - 1);
                }
            }
            Table.FieldsCoordinate[randomNum].Food = true;
        }

        /// <summary>
        /// új játék indítása.
        /// </summary>
        public void NewGame()
        {
            //Pontok változásának lekezelése és alap pontszám 0-az
            OnGameAdvanced(_gameScoresCount = 0, _gameHighScoresCount);
            _startbuttonText = "Start";

            //új alapértelmezett Snake mező létrehoz
            GetSnake.Clear();

            
            //kígyó fejének kezdő pozíció koordináta megad 
            SnakeField sneakhead = new SnakeField
            {
                X = Table.FieldsCoordinate[snakeHeadCoordinate-1].X,
                Y = Table.FieldsCoordinate[snakeHeadCoordinate-1].Y,
            };

            
            GetSnake.Add(sneakhead); // kígyó feje hozzáad a Snake listához


            for (int i = 1; i < 5; i++) //snake 4 hosszú, beégetve
            {
                SnakeField sneakbody = new SnakeField();
                sneakbody.X = Table.FieldsCoordinate[snakeHeadCoordinate - 1].X + i;
                sneakbody.Y = Table.FieldsCoordinate[snakeHeadCoordinate - 1].Y;
                GetSnake.Add(sneakbody);
            }

            //kígyó eredeti kiinduló pozícióba állít
            _gameDirection = Direction.goLeft;          //a kígyó kezdetben balra induljon el a táblán

            //Tojás koordináta random generálása
            randomNum = rand.Next(0, Table.FieldsCoordinate.Count-1);

            //Ellenőrzés a tojás nem generálodott-e le egy akadályra, ha igen újra generáljuk
            if (Table.FieldsCoordinate[randomNum].Border)
            {
                while (Table.FieldsCoordinate[randomNum].Border)
                {
                    randomNum = rand.Next(0, Table.FieldsCoordinate.Count - 1);
                }
            }
            Table.FieldsCoordinate[randomNum].Food = true;

            _gamePause = false; //idő telik újra

            OnGameCreated();
        }

        /// <summary>
        /// játék újrakezdés.
        /// </summary>
        public void RestartGame()
        {
            //Pontok változásának lekezelése
            OnGameAdvanced(_gameScoresCount = 0, _gameHighScoresCount = 0);

            NewGame();
        }
        #endregion


        #region Event trigger
        /// <summary>
        /// Játéktábla méretének eseményének kiváltása.
        /// </summary>
        private void OnChangeSize(Int32 size)
        {
            ChangeSize?.Invoke(this, size);
        }

        /// <summary>
        /// Játékidő változás eseményének kiváltása.
        /// </summary>
        private void OnGameAdvanced(Int32 gameScoreCount, Int32 gameHighScoreCount)
        {
            GameAdvanced?.Invoke(this, new SnakeEventArgs(false, gameScoreCount, gameHighScoreCount, _gameDirection));
        }

        // <summary>
        /// Játék vége eseményének kiváltása.
        /// </summary>
        /// <param name="gameisOver">Vége-e a játéknak.</param>
        private void OnGameOver(Boolean gameIsOver)
        {
            OnStartbuttonChange("Start", true);
            GameOver?.Invoke(this, new SnakeEventArgs(gameIsOver, _gameScoresCount, _gameHighScoresCount, _gameDirection));
        }

        /// <summary>
        /// Gomb változásának eseményének kiváltása.
        /// </summary>
        /// <param name="buttontext">Gomb szövege</param>
        private void OnStartbuttonChange(String buttontext, bool gameover)
        {
            _startbuttonText = buttontext;
            StartButtonChange?.Invoke(this, new SnakeEventArgs(gameover, _gameScoresCount, _gameHighScoresCount, _gameDirection));
        }

        /// <summary>
	    /// Játék létrehozás eseményének kiváltása.
	    /// </summary>
	    private void OnGameCreated()
        {
            //_model.Table.RegionSize lekérdez
            GameCreated?.Invoke(this, new SnakeEventArgs(false, _gameScoresCount, _gameHighScoresCount, _gameDirection));
        }

        /// <summary>
	    /// Canvas frissítésének eseményének kiváltása.
	    /// </summary>
	    private void OnCanvasUpgrade()
        {
            CanvasUpgrade?.Invoke(this, new SnakeEventArgs(false, _gameScoresCount, _gameHighScoresCount, _gameDirection));
        }
        #endregion

    }
}
