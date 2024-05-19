using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Game.SnakeGameConzol.Persistance;

namespace Game.SnakeGameConzol.Model
{
    /// <summary>
    /// Kígyó mozgásának irányának.
    /// </summary>
    public enum Direction { goLeft, goRight, goUp, goDown }


    /// <summary>
    /// Snake táblaméretének elnevezése.
    /// </summary>
    public enum MapSize { Large, Medium, Small }




    /// <summary>
    /// Snake játék típusa.
    /// </summary>
    public class SnakeModel
    {
        #region Difficulty constants

        private const Int32 calculatorWidthandHeight = 25; //pályamegjelenítésre használt arányszám beégetve akódva

        #endregion


        #region Fields

        private readonly Random rand;
        private MapSize _fieldSize; //pályaméret beállítása
        private readonly ISnakeDataAccess _dataAccess; // adatelérés
        private SnakeTable _table; // játéktábla
        private bool _gamePause; //játék végét jelző státusz

        //táblahatárok értékei
        private int MaxWidth;
        private int MaxHeight;

        //snake jákhoz tartozó megjelenítendő vizuális elemek a pályán
        private List<FigShapes> Snake;
        private FigShapes Food;

        //Eventargs fileds
        private Direction _gameDirection;       // utolsó irányváltó művelet
        private Int32 _gameScoresCount;         // aktuális pontok száma
        private Int32 _gameHighScoresCount;     // legtöbb pont pontszáma

        #endregion


        #region Properties

        /// <summary>
        /// Arányszám lekérdezése.
        /// </summary>
        public Int32 CalcWidthandHeight { get { return calculatorWidthandHeight; } }

        /// <summary>
        /// Játéktábla lekérdezése.
        /// </summary>
        public SnakeTable Table { get { return _table!; } }

        /// <summary>
        /// pálya max szélesség és magasság lekérdezés és beállítása.
        /// </summary>
        public int SetMaxWidth { set { MaxWidth = value; } }
        public int SetMaxHeight { set { MaxHeight = value; } }

        public Int32 GetMaxWidth { get { return MaxWidth; } }
        public Int32 GetMaxHeight { get { return MaxHeight; } }

        /// <summary>
        /// Játéktábla méret elnevezés beállítása.
        /// </summary>
        public void SetMapsize(MapSize mapsize) { _fieldSize = mapsize; }

        /// <summary>
        /// Játék végének lekérdezése és beállítása.
        /// </summary>
        public Boolean IsGamePaused { get { return (_gamePause == true); } }

        public void SetGamePaused(Boolean p) { _gamePause = p; }


        /// <summary>
        /// Pályán elhelyezett nugetsek koordinátáinak lekérdezése és beállítása.
        /// </summary>
        public FigShapes GetFood { get { return Food; } }
        public void SetFood(int value, int value2) { Food.X = value; Food.Y = value2; }

        /// <summary>
        /// pontok lekérdezése.
        /// </summary>
        public Int32 GameScores { get { return _gameScoresCount; } }
        public Int32 GameHighScoresTest { get { return _gameHighScoresCount; } }

        /// <summary>
        /// Pályán elhelyezett kigyórészek koordinátáinak lekérdezése.
        /// </summary>
        public List<FigShapes> GetSnake { get { return Snake; } }

        #endregion


        #region Events
        /// <summary>
        /// Kígyó irányának megváltozásának eseménye.
        /// </summary>
        public event EventHandler<SnakeEventArgs>? SnakeDirectionChanged;

        /// <summary>
        /// Játék előrehaladásának eseménye.
        /// </summary>
        public event EventHandler<SnakeEventArgs>? GameAdvanced;

        /// <summary>
        /// Játék végének eseménye.
        /// </summary>
        public event EventHandler<SnakeEventArgs>? GameOver;

        #endregion


        #region Consturctors
        /// <summary>
        /// Snake játék példányosítása.
        /// </summary>
        /// <param name="dataAccess">Az adatelérés.</param>
        public SnakeModel(ISnakeDataAccess dataAccess)
        {
            Snake = new List<FigShapes>();
            Food = new FigShapes();

            rand = new Random();
            _table = new SnakeTable();
            _dataAccess = dataAccess;
            _fieldSize = MapSize.Large; //legnagyobb méretű pálya generálása játék elején beégetjük
            _gameDirection = Direction.goLeft; //kigyó kezdő indulási iránya

            _gamePause = true; //kezdetben az idő nem tellik vala
            _gameScoresCount = 0;
            _gameHighScoresCount = 0;

        }

        #endregion


        /// <summary>
        /// Játék beolvasása fileból és tábla adatok kimentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        public async Task LoadGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            //tábla létrehoz a beolvasás után
            _table = await _dataAccess.LoadAsync(path, _fieldSize);

            //Játék megjelenésérét felelős táblahatár értékek kikalkulálása
            SetMaxWidth = (int)(Table.RegionSize / CalcWidthandHeight) - 1;
            SetMaxHeight = (int)(Table.RegionSize / CalcWidthandHeight) - 1;

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
                    if (_gameDirection != Direction.goRight) IsSnakeDirectionChanged(Direction.goLeft);
                    _gameDirection = Direction.goLeft;
                    break;
                case Direction.goRight:
                    if (_gameDirection != Direction.goLeft) IsSnakeDirectionChanged(Direction.goRight);
                    _gameDirection = Direction.goRight;
                    break;
                case Direction.goUp:
                    if (_gameDirection != Direction.goDown) IsSnakeDirectionChanged(Direction.goUp);
                    _gameDirection = Direction.goUp;
                    break;
                case Direction.goDown:
                    if (_gameDirection != Direction.goUp) IsSnakeDirectionChanged(Direction.goDown);
                    _gameDirection = Direction.goDown;
                    break;
            }
        }



        public void AdvanceTime()
        {
            if (IsGamePaused) // ha szüneteltetve a játék akkor, nem folytathatjuk
                return;

            for (int i = GetSnake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {

                    switch (_gameDirection)
                    {
                        case Direction.goLeft:
                            GetSnake[i].X--;
                            break;
                        case Direction.goRight:
                            GetSnake[i].X++;
                            break;
                        case Direction.goDown:
                            GetSnake[i].Y++;
                            break;
                        case Direction.goUp:
                            GetSnake[i].Y--;
                            break;
                    }

                    //End of the game if snake is leave the canvas
                    if (GetSnake[i].X < 0)
                    {
                        OnGameOver(true);
                    }
                    if (GetSnake[i].X > GetMaxWidth)
                    {
                        OnGameOver(true);
                    }
                    if (GetSnake[i].Y < 0)
                    {
                        OnGameOver(true);
                    }
                    if (GetSnake[i].Y > GetMaxHeight)
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
                    if (GetSnake[i].X == GetFood.X
                        && GetSnake[i].Y == GetFood.Y)
                    {
                        Eat();
                    }

                    //Eat the wall
                    for (int c = 0; c < Table.BordersCoordinates.Count; c++)
                    {
                        if ((GetSnake[0].X == Table.BordersCoordinates[c].X && GetSnake[0].Y == Table.BordersCoordinates[c].Y) ||
                            (GetSnake[0].X == Table.BordersCoordinates[c].X & GetSnake[0].Y == Table.BordersCoordinates[c].Y))
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
        }


        public void Eat()
        {
            _gameScoresCount += 1;

            //ha aktuális pont több átad érétke00
            if (_gameScoresCount > _gameHighScoresCount) _gameHighScoresCount = _gameScoresCount;

            //Pontok változásának lekezelése
            OnGameAdvanced(_gameScoresCount, _gameHighScoresCount);

            //New body part position
            FigShapes body = new FigShapes
            {
                X = GetSnake[GetSnake.Count - 1].X,
                Y = GetSnake[GetSnake.Count - 1].Y
            };

            GetSnake.Add(body);

            //Tojás koordináta random generálása
            SetFood(rand.Next(2, GetMaxWidth), rand.Next(2, GetMaxHeight));

            //Ellenőrzés a tojás nem generálodott-e le egy akadályra, ha igen újra generáljuk
            for (int i = 0; i < Table.BordersNumber; i++)
            {
                while (GetFood.X == Table.BordersCoordinates[i].X && GetFood.Y == Table.BordersCoordinates[i].Y)
                {
                    SetFood(rand.Next(2, MaxWidth), rand.Next(2, MaxHeight));
                }
            }

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


        /// <summary>
        /// új játék indítása.
        /// </summary>
        public void NewGame() 
        {

            //Pontok változásának lekezelése és alap pontszám 0-az
            OnGameAdvanced(_gameScoresCount = 0, _gameHighScoresCount);

            //új alapértelmezett pálya létrehoz
            GetSnake.Clear();

            //kígyó fejének kezdő pozíció koordináta megad
            FigShapes sneakhead = new FigShapes
            {
                X = MaxWidth / 2,
                Y = MaxHeight / 2
            }; 
            
           
            GetSnake.Add(sneakhead); // kígyó feje hozzáad a Snake listához

            for (int i = 0; i < 4; i++) //snake 4 hosszú, beégetve
            {
                FigShapes sneakbody = new FigShapes();
                GetSnake.Add(sneakbody);
            }

            //kígyó eredeti kiinduló pozícióba állít
            _gameDirection = Direction.goLeft;          //a kígyó kezdetben balra induljon el a táblán

            //Tojás koordináta random generálása
            SetFood(rand.Next(2,MaxWidth), rand.Next(2, MaxHeight));

            //Ellenőrzés a tojás nem generálodott-e le egy akadályra, ha igen újra generáljuk
            for (int i = 0; i < Table.BordersNumber; i++)
            {
                while (GetFood.X == Table.BordersCoordinates[i].X && GetFood.Y == Table.BordersCoordinates[i].Y)
                {
                    SetFood(rand.Next(2, MaxWidth), rand.Next(2, MaxHeight));
                }
            }

            _gamePause = false; //idő telik újra

        }


        #region Private event methods
        /// <summary>
        /// Kígyó irányváltozás esemény kiváltása.
        /// </summary>
        private void IsSnakeDirectionChanged(Direction gameDirection)
        {
            SnakeDirectionChanged?.Invoke(this, new SnakeEventArgs(false, _gameScoresCount, _gameHighScoresCount, gameDirection));
        }

        /// <summary>
        /// Játékidő változás eseményének kiváltása.
        /// </summary>
        private void OnGameAdvanced(Int32 gameScoreCount, Int32 gameHighScoreCount)
        {
            GameAdvanced?.Invoke(this, new SnakeEventArgs(false, gameScoreCount, gameHighScoreCount, _gameDirection));
        }

        /// <summary>
        /// Játék vége eseményének kiváltása.
        /// </summary>
        /// <param name="gameisOver">Vége-e a játéknak.</param>
        private void OnGameOver(Boolean gameIsOver)
        {
            GameOver?.Invoke(this, new SnakeEventArgs(gameIsOver, _gameScoresCount, _gameHighScoresCount, _gameDirection));
        }

        #endregion

    }
}
