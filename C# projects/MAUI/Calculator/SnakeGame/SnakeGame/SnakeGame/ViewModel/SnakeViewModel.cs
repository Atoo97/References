using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeLib.Persistence;
using SnakeLib.Model;
using System.Collections.ObjectModel;
using SnakeGame.Model;

namespace SnakeGame.ViewModel
{
    /// <summary>
    /// Snake nézetmodell típusa.
    /// </summary>
    public class SnakeViewModel : ViewModelBase
    {
        #region Fields
        private String _length; //Pálya grid mérete
        private SnakeModel _model; // modell
        private Int32 _tableSize;
        private String _size;
        private int num;
        #endregion

        #region Properties
        public DelegateCommand SizeChangeCommand { get; private set; }
        /// <summary>
        /// Játék parancs lekérdezései.
        /// </summary>
        public DelegateCommand DirectionChangeCommand { get; private set; }
        public DelegateCommand StartStopCommand { get; private set; }
        public DelegateCommand RestartCommand { get; private set; }



        public ObservableCollection<SnakeGameField> Fields { get; set; }

        public Int32 CurrentTableSize { get { return _tableSize; } }

        /// <summary>
        /// Start gomb lekérdezése.
        /// </summary>
        public String Starter { get { return _model!.StartbuttonText; } }

        /// <summary>
        /// Food helyének lekérdezése.
        /// </summary>
        public int FoodCoordinate { get { return _model!.RandomNum; } }

        /// <summary>
        /// Aktuális pontok lekérdezése.
        /// </summary>
        public String Score { get { return _model!.GameScores.ToString(); } }

        /// <summary>
        /// Eddig maximális pontok lekérdezése.
        /// </summary>
        public String HighScore { get { return _model!.GameHighScores.ToString(); } }

        public String Length
        {
            get { return _length; }
            set
            {
                if (_length != value)
                {
                    _length = value;
                    OnPropertyChanged();
                }
            }
        }

        //Pályaméret felirata
        public String Size { get { return _size; } }

        /// <summary>
        /// Segédproperty-k a tábla méretezéséhez
        /// </summary>
        public RowDefinitionCollection RowDefinitions
        {
            //GridLength.star a méretre utal "*" a row-ja
            get => new RowDefinitionCollection(Enumerable.Repeat(new RowDefinition(GridLength.Star), CurrentTableSize).ToArray());
        }
        public ColumnDefinitionCollection ColumnDefinitions
        {
            get => new ColumnDefinitionCollection(Enumerable.Repeat(new ColumnDefinition(GridLength.Star), CurrentTableSize).ToArray());
        }

        #endregion


        #region Events
        /// <summary>
        /// Játék betöltésének eseménye.
        /// </summary>
        public event EventHandler LoadGame;
        public event EventHandler StartStopGame;
        public event EventHandler RestartGame;
        #endregion


        #region Constructor
        /// <summary>
        /// Snake nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A modell típusa.</param>

        public SnakeViewModel(SnakeModel model)
        {
            // játék csatlakoztatása
            _model = model;
            _model.ChangeSize += new EventHandler<int>(Model_ChangeSize);
            _model.GameCreated += new EventHandler<SnakeEventArgs>(Model_GameCreated);
            _model.StartButtonChange += new EventHandler<SnakeEventArgs>(Model_StartButtonChange);
            _model.GameAdvanced += new EventHandler<SnakeEventArgs>(Model_GameAdvanced);
            _model.CanvasUpgrade += new EventHandler<SnakeEventArgs>(Model_CanvasUpgraded);

            //Command handling
            SizeChangeCommand = new DelegateCommand(param => OnSizeChange(param));
            DirectionChangeCommand = new DelegateCommand(param => SetDirection(param?.ToString() ?? String.Empty));
            StartStopCommand = new DelegateCommand(param => OnStartStop());
            RestartCommand = new DelegateCommand(param => OnRestart());

            //Fields: Átírni Figshapes
            Fields = new ObservableCollection<SnakeGameField>();

        }
        #endregion


        #region Private methods
        private void SetupTable()
        {
            Fields.Clear();

            //pálya láthatóságának létrehozása
            for (int i = 0; i < _model.Table.RegionSize * _model.Table.RegionSize; i++)
            {
                SnakeGameField field = new SnakeGameField();
                field.X = _model.Table.FieldsCoordinate[i].X;
                field.Y = _model.Table.FieldsCoordinate[i].Y;
                if (_model.Table.FieldsCoordinate[i].Border)
                {
                    field.Color = Colors.Black;
                }
                else
                {
                    field.Color = Colors.White;
                }

                Fields.Add(field);
            }
            num = Fields.Count;

        }

        /// <summary>
		/// Canvas frissítése.
		/// </summary>
		private void CanvasGraphicSetup()
        {
            //Tojás pozíciójának változása
            for (int i = 0; i < Fields.Count-1; i++)
            {
                if (Fields[i].Color == Colors.Yellow)
                {
                    Fields[i].Color = Colors.White;
                }
                if (i == FoodCoordinate)
                {
                    Fields[FoodCoordinate].Color = Colors.Yellow;
                }
            }

            //Töröljük a kígyókat
            if (Fields.Count > num)
            {
                for (int i = 0; i < _model.GetSnake.Count; i++)
                {
                    Fields.Remove(Fields[Fields.Count-1 - i]);
                }
            }


            //kígyó testének kirajzolása és növelése, ha nagyobb lett...
            for (int i = 0; i < _model.GetSnake.Count; i++)
            {
                SnakeGameField field = new SnakeGameField();
                field.X = _model.GetSnake[i].X;
                field.Y = _model.GetSnake[i].Y;
                field.Color = Colors.Green;
                if (field.X > -1 && field.Y > -1 && field.X <= _model.Table.RegionSize && field.Y <= _model.Table.RegionSize) 
                {
                    Fields.Add(field);
                }
            }
            

        }

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
                    break;
                case "right":
                    _model!.SetMove(Direction.goRight);
                    break;
                case "up":
                    _model!.SetMove(Direction.goUp);
                    break;
                case "down":
                    _model!.SetMove(Direction.goDown);
                    break;
            }
        }
        #endregion


        #region Model event handlers
        private void Model_ChangeSize(object sender, Int32 e)
        {
            _tableSize = e;
            Length = (DeviceDisplay.Current.MainDisplayInfo.Width/4.5).ToString();
            OnPropertyChanged(nameof(Length));
            OnPropertyChanged(nameof(CurrentTableSize));
            OnPropertyChanged(nameof(RowDefinitions));
            OnPropertyChanged(nameof(ColumnDefinitions));
            OnPropertyChanged(nameof(Size));

            SetupTable();
        }

        /// <summary>
        /// Játék előrehaladásának eseménykezelője.
        /// </summary>
        private void Model_StartButtonChange(object sender, SnakeEventArgs e)
        {
            OnPropertyChanged(nameof(Starter));
        }

        /// <summary>
        /// Játék létrehozásának eseménykezelője.
        /// </summary>
        private void Model_GameCreated(object sender, SnakeEventArgs e)
        {
            CanvasGraphicSetup();
        }

        /// <summary>
        /// Játék előrehaladásának eseménykezelője.
        /// </summary>
        private void Model_GameAdvanced(object sender, SnakeEventArgs e)
        {
            OnPropertyChanged(nameof(Score));
            OnPropertyChanged(nameof(HighScore));

        }

        /// <summary>
	    /// Canvas felületének frissítése eseménykezelője.
	    /// </summary>
		private void Model_CanvasUpgraded(object sender, SnakeEventArgs e)
        {
            CanvasGraphicSetup();
        }
        #endregion  


        #region Event trigger
        private void OnSizeChange(object param)
        {
            switch (param) // művelet végrehajtása a modellel
            {
                case "Small":
                    //pályaméret beállítása
                    _size = "Small";
                    _model.GameTableSize = GameTableSize.Small;
                    LoadGame?.Invoke(this, EventArgs.Empty);
                    break;
                case "Medium":
                    _size = "Medium";
                    _model.GameTableSize = GameTableSize.Medium;
                    LoadGame?.Invoke(this, EventArgs.Empty);
                    break;
                case "Large":
                    _size = "Large";
                    _model.GameTableSize = GameTableSize.Large;
                    LoadGame?.Invoke(this, EventArgs.Empty);
                    break;
            }
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
