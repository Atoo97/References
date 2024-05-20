using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using SnakeGame.Persistence;
using SnakeGame.Model;

namespace SnakeGame_WPF.ViewModel
{
    public class SnakeViewModel : ViewModelBase
    {

        #region Fields

        private readonly SnakeModel? _model;
        private String? fSize;
        private int snakeparts = 1;

        #endregion

        #region Properties

        /// <summary>
        /// Játék parancs lekérdezései.
        /// </summary>
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand DirectionChange { get; private set; }
        public DelegateCommand StartStop { get; private set; }
        public DelegateCommand Restart { get; private set; }

        /// <summary>
        /// Pályaméret lekérdezése.
        /// </summary>
        public String Fieldsize { get { return _model!.GetMapsize().ToString(); } }

        /// <summary>
        /// Aktuális pontok lekérdezése.
        /// </summary>
        public String Score { get { return _model!.GameScores.ToString(); } }

        /// <summary>
        /// Eddig maximális pontok lekérdezése.
        /// </summary>
        public String HighScore { get { return _model!.GameHighScores.ToString(); } }

        /// <summary>
        /// Start gomb lekérdezése.
        /// </summary>
        public String Starter { get { return _model!.StartbuttonText; } } 

        /// <summary>
        /// Eddig maximális pontok lekérdezése.
        /// </summary>
        public String Field_Size { get { return fSize!; } }

        /// <summary>
        /// Eddig maximális pontok lekérdezése.
        /// </summary>
        public int CanvasSize { get { return _model!.Table.RegionSize; } }

        /// <summary>
        /// Játékmező gyűjtemény lekérdezése.
        /// </summary>
        public ObservableCollection<SnakeFields> Fields { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Játék betöltésének eseménye.
        /// </summary>
        public event EventHandler? LoadGame;
        public event EventHandler? StartStopGame;
        public event EventHandler? RestartGame;

        #endregion

        #region Constructors

        /// <summary>
        /// Sudoku nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A modell típusa.</param>
        public SnakeViewModel(SnakeModel model)
        {
            fSize = "Large";

            // játék csatlakoztatása
            _model = model;
            _model.GameAdvanced += new EventHandler<SnakeEventArgs>(Model_GameAdvanced);
            _model.StartButtonChange += new EventHandler<SnakeEventArgs>(Model_StartButtonChange);
            _model.GameCreated += new EventHandler<SnakeEventArgs>(Model_GameCreated);
            _model.CanvasUpgrade += new EventHandler<SnakeEventArgs>(Model_CanvasUpgraded);


            // parancsok kezelése
            LoadGameCommand = new DelegateCommand(param => OnLoadGame(param?.ToString() ?? String.Empty));
            DirectionChange = new DelegateCommand(param => SetDirection(param?.ToString() ?? String.Empty));
            StartStop = new DelegateCommand(param => OnStartStop());
            Restart = new DelegateCommand(param => OnRestart());

            // játéktábla létrehozása
            Fields = new ObservableCollection<SnakeFields>();

        }
        #endregion


        #region Private methods   
        /// <summary>
        /// Tábla felépítése.
        /// </summary>
        private void SetUpTable()
        {
            snakeparts = 1;

            OnPropertyChanged(nameof(CanvasSize));

            Fields.Clear();

            //falak kirajzolása
            var wall = new BitmapImage();
            wall.BeginInit();
            wall.DecodePixelWidth = 40;
            wall.UriSource = new Uri(@"/View/Images/wall.png", UriKind.Relative);
            wall.EndInit();

            for (int i = 0; i < _model!.Table.BordersNumber; i++)
            {
                Fields.Add(new SnakeFields
                {
                    Xcoordinate = _model.Table.BordersCoordinates[i].X * _model.CalcWidthandHeight,
                    Ycoordinate = _model.Table.BordersCoordinates[i].Y * _model.CalcWidthandHeight,
                    Image = wall,
                }
                );
            }

            //tojás kép inicializál
            var egg = new BitmapImage();
            egg.BeginInit();
            egg.DecodePixelWidth = 25;
            egg.UriSource = new Uri(@"/View/Images/egg.png", UriKind.Relative);
            egg.EndInit();

            Fields.Add(new SnakeFields
            {
                Xcoordinate = 0,
                Ycoordinate = 0,
                Image = egg,
            }
            );


            //kígyó feje kép inicializál
            var sneakhead = new BitmapImage();
            sneakhead.BeginInit();
            sneakhead.DecodePixelWidth = 25;
            sneakhead.UriSource = new Uri(@"/View/Images/left.png", UriKind.Relative);
            sneakhead.EndInit();

            Fields.Add(new SnakeFields
            {
                Xcoordinate = 0,
                Ycoordinate = 0,
                Image = sneakhead,
            }
            );

        }


        /// <summary>
		/// Canvas frissítése.
		/// </summary>
		private void CanvasGraphicSetup()
        {
            //Tojás pozíciójának változása
            Fields[_model!.Table.BordersNumber].Xcoordinate = _model!.GetFood.X * _model.CalcWidthandHeight;
            Fields[_model!.Table.BordersNumber].Ycoordinate = _model!.GetFood.Y * _model.CalcWidthandHeight;


            //kígyó testének kirajzolása és növelése, ha nagyobb lett
            if (snakeparts < _model.GetSnake.Count)
            {
                var sneakbody = new BitmapImage();
                sneakbody.BeginInit();
                sneakbody.DecodePixelWidth = 25;
                sneakbody.UriSource = new Uri(@"/View/Images/body.png", UriKind.Relative);
                sneakbody.EndInit();

                for (int i = snakeparts; i < _model.GetSnake.Count; i++)
                {
                    Fields.Add(new SnakeFields
                    {
                        Xcoordinate = _model.GetSnake[i].X * _model.CalcWidthandHeight,
                        Ycoordinate = _model.GetSnake[i].Y * _model.CalcWidthandHeight,
                        Image = sneakbody,
                    }
                    );
                }

                snakeparts = _model.GetSnake.Count;
            }

            for (int i = 0; i < _model.GetSnake.Count; i++)
            {
                if (i == 0)
                {
                    //kígyó fejének kirajzolása
                    Fields[_model!.Table.BordersNumber + 1].Xcoordinate = _model.GetSnake[0].X * _model.CalcWidthandHeight;
                    Fields[_model!.Table.BordersNumber + 1].Ycoordinate = _model.GetSnake[0].Y * _model.CalcWidthandHeight;
                }
                else
                {
                    Fields[_model!.Table.BordersNumber + 1 + i].Xcoordinate = _model.GetSnake[i].X * _model.CalcWidthandHeight;
                    Fields[_model!.Table.BordersNumber + 1 + i].Ycoordinate = _model.GetSnake[i].Y * _model.CalcWidthandHeight;
                }
            }
        }
        #endregion

        #region Game event handler

        /// <summary>
        /// Játék előrehaladásának eseménykezelője.
        /// </summary>
        private void Model_GameAdvanced(object? sender, SnakeEventArgs e)
        {
           OnPropertyChanged(nameof(Score));
           OnPropertyChanged(nameof(HighScore));
            
        }

        /// <summary>
        /// Játék előrehaladásának eseménykezelője.
        /// </summary>
        private void Model_StartButtonChange(object? sender, SnakeEventArgs e)
        {
           OnPropertyChanged(nameof(Starter)); 
        }

        #endregion

        #region Event methods

        /// <summary>
        /// Mozgatás végrehajtása.
        /// </summary>
        /// <param name="operatorString">A művelet szöveges megfelelője.</param>
        private void SetDirection(String operatorString)
        {
            
            switch (operatorString) // művelet végrehajtása a modellel
            {
                case "left":
                    _model!.SetMove(Direction.goLeft);
                    var sneakhead = new BitmapImage();
                    sneakhead.BeginInit();
                    sneakhead.DecodePixelWidth = 25;
                    sneakhead.UriSource = new Uri(@"/View/Images/left.png", UriKind.Relative);
                    sneakhead.EndInit();
                    Fields[_model!.Table.BordersNumber + 1].Image = sneakhead;
                    break;
                case "right":
                    _model!.SetMove(Direction.goRight);
                    var sneakhead2 = new BitmapImage();
                    sneakhead2.BeginInit();
                    sneakhead2.DecodePixelWidth = 25;
                    sneakhead2.UriSource = new Uri(@"/View/Images/right.png", UriKind.Relative);
                    sneakhead2.EndInit();
                    Fields[_model!.Table.BordersNumber + 1].Image = sneakhead2;
                    break;
                case "up":
                    _model!.SetMove(Direction.goUp);
                    var sneakhead3 = new BitmapImage();
                    sneakhead3.BeginInit();
                    sneakhead3.DecodePixelWidth = 25;
                    sneakhead3.UriSource = new Uri(@"/View/Images/up.png", UriKind.Relative);
                    sneakhead3.EndInit();
                    Fields[_model!.Table.BordersNumber + 1].Image = sneakhead3;
                    break;
                case "down":
                    _model!.SetMove(Direction.goDown);
                    var sneakhead4 = new BitmapImage();
                    sneakhead4.BeginInit();
                    sneakhead4.DecodePixelWidth = 25;
                    sneakhead4.UriSource = new Uri(@"/View/Images/down.png", UriKind.Relative);
                    sneakhead4.EndInit();
                    Fields[_model!.Table.BordersNumber + 1].Image = sneakhead4;
                    break;
            }
        }

        /// <summary>
        /// Játék betöltése eseménykiváltása.
        /// </summary>
        private void OnLoadGame(String fieldsize)
        {
            switch (fieldsize) // művelet végrehajtása a modellel
            {
                case "Small":
                    fSize = "Small";
                    LoadGame?.Invoke(this, EventArgs.Empty);
                    OnPropertyChanged(nameof(Fieldsize));
                    break;
                case "Medium":
                    fSize = "Medium";
                    LoadGame?.Invoke(this, EventArgs.Empty);
                    OnPropertyChanged(nameof(Fieldsize));
                    break;
                case "Large":
                    fSize = "Large";
                    LoadGame?.Invoke(this, EventArgs.Empty);
                    OnPropertyChanged(nameof(Fieldsize));
                    break;
            }

        }

        /// <summary>
	    /// Játék létrehozásának eseménykezelője.
	    /// </summary>
		private void Model_GameCreated(object? sender, SnakeEventArgs e)
        {
            SetUpTable();
        }

        /// <summary>
	    /// Canvas felületének frissítése eseménykezelője.
	    /// </summary>
		private void Model_CanvasUpgraded(object? sender, SnakeEventArgs e)
        {
            CanvasGraphicSetup();
        }

        private void OnStartStop()
        {

            StartStopGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnRestart()
        {

            RestartGame?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
