using System;
using System.Drawing;
using System.Collections.Generic;
using Game.SnakeGameConzol.Model;
using Game.SnakeGameConzol.Persistance;

namespace Game.Snake_Game
{
   
    public partial class SankeForm : Form
    {
        #region Fields

        private bool notFirstSatrt = false; // elsõ indtást jelzi

        private readonly ISnakeDataAccess _dataAccess = null!; // adatelérés
        private readonly SnakeModel _model = null!; // játékmodell
        private readonly System.Windows.Forms.Timer _timer = null!; // idõzítõ

        //Images
        private List<String> imageParts = new List<String>();
        private Image? head;
        private Image? body;
        private Image? nugets;
        private Image? border;

        #endregion

        #region Constructors
        public SankeForm()
        {
            InitializeComponent();

            // adatelérés példányosítása
            _dataAccess = new SnakeFileDataAcess();

            // modell létrehozása és az eseménykezelõk társítása
            _model = new SnakeModel(_dataAccess);
            _model.SnakeDirectionChanged += new EventHandler<SnakeEventArgs>(Game_SnakeDirectionChanged);
            _model.GameAdvanced += new EventHandler<SnakeEventArgs>(Game_GameAdvanced);
            _model.GameOver += new EventHandler<SnakeEventArgs>(Game_GameOver);

            //EventArgs

            //Large kezdõméret játéktábla beállítása és betöltése a konstruktor hivásakor
            LoadSnakeTable_Click(largeToolStripMenuItem, EventArgs.Empty);

            KeyPreview = true; // billentyüesemény kezeles
            KeyDown += new KeyEventHandler(SnakeForm_KeyDown);

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 100; // 0.1 másodpercenként történõ mozgatás a pályán elõre
            _timer.Tick += new EventHandler(Timer_Tick); // idõzített esemény társítása
        }

        #endregion

        #region Table event handlers
        /// <summary>
        /// Játék betöltésének eseménykezelõje.
        /// </summary>
        private async void LoadSnakeTable_Click(object? sender, EventArgs e)
        {

            if (notFirstSatrt) //elsõ indításkor NEM lefutó elágazás
            {
                MessageBox.Show("Biztos pályaméretet szeretnél váltani?" + Environment.NewLine +
                                "Összes megszerzett pontod elveszted így!",
                                "Snake game",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);

                _model.RestartGame();
            }

            notFirstSatrt = true;


            if (sender is ToolStripMenuItem menuitem)
            {
                // megvizsgáljuk, milyen az eseményt kiváltó gomb felirata, úgy eldönthetjük, melyik gombot nyomták le
                switch (menuitem.Text)
                {
                    case "Large":
                        //Szöveg megveltoztat
                        txtMapSize.Text = "Map size: Large";
                        //Pályamaret módosít
                        _model.SetMapsize(MapSize.Large);
                        break;
                    case "Medium":
                        txtMapSize.Text = "Map size: Medium";
                        _model.SetMapsize(MapSize.Medium);
                        break;
                    case "Small":
                        txtMapSize.Text = "Map size: Small";
                        _model.SetMapsize(MapSize.Small);
                        break;
                }
            }

            try
            {
                // játék betöltése .txt állományból
                await _model.LoadGameAsync(@".\Persistance\mapsize.txt");
            }
            catch (SnakeDataException)
            {
                MessageBox.Show("Játék betültése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Beállítja canvas - pályamareteket és határokat
            SetupTable();
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Tábla és grafikus környezetbeállítása.
        /// </summary>
        private void SetupTable()
        {

            //picturebox háttér beállítása:
            this.picCanvas.BackgroundImage = Image.FromFile("bg.jpg");
            this.picCanvas.BackgroundImageLayout = ImageLayout.Stretch;
            this.DoubleBuffered = true;

            //SnakeGame-hez tartozó képek kezelése
            imageParts = Directory.GetFiles("snake", "*.png").ToList();
            head = Image.FromFile(imageParts[2])!;
            body = Image.FromFile(imageParts[6])!;
            nugets = Image.FromFile(imageParts[1])!;
            border = Image.FromFile(imageParts[5])!;

            //itt állítjuk majd be a picturebox méretét nxn-esre
            this.picCanvas.Size = new System.Drawing.Size(_model.Table.RegionSize, _model.Table.RegionSize);

        }

        #endregion

        #region Game event handlers
        /// <summary>
        /// kígyófej irányváltásának eseménykezelõje.
        /// </summary>
        private void Game_SnakeDirectionChanged(Object? sender, SnakeEventArgs e)
        {
            //kígyó fejének kép cseréje és iránya beállít
            switch (e.SnakeDirection)
            {
                case Direction.goLeft:
                    head = Image.FromFile(imageParts[2])!;
                    break;
                case Direction.goRight:
                    head = Image.FromFile(imageParts[3])!;
                    break;
                case Direction.goUp:
                    head = Image.FromFile(imageParts[4])!;
                    break;
                case Direction.goDown:
                    head = Image.FromFile(imageParts[0])!;
                    break;
            }
        }


        /// <summary>
        /// Játék elõrehaladásának eseménykezelõje.
        /// </summary>
        /// 
        private void Game_GameAdvanced(Object? sender, SnakeEventArgs e)
        {
            txtScore.Text = "Score: " + e.ScoresCount.ToString();
            txtHighScore.Text = "High score: " + e.HighScoresCount.ToString();
            // játék pontok frissítése
        }


        /// <summary>
        /// Játék végének eseménykezelõje.
        /// </summary>
        private void Game_GameOver(Object? sender, SnakeEventArgs e)
        {
            _timer.Stop(); //leáll az idõ

            startButton.Enabled = false;
            txtScore.Text = "Score: 0";

            if (e.IsOver) //ha a kígyó akadályba ütközött
            {
                MessageBox.Show("Vége a játéknak! Akadályba ütköztél!" + Environment.NewLine +
                                "Összesen " + e.ScoresCount + " tojást sikerült megenned.",
                                "Snake game",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);
            }
            else //ha a kígyó saját magába harapott
            {
                MessageBox.Show("Vége a játéknak! A kígyó öngyilkos lett!" + Environment.NewLine +
                                "Összesen " + e.ScoresCount + " tojást sikerült megenned.",
                                "Snake game",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);

            }
        }

        #endregion

        #region Timer event handlers
        /// <summary>
        /// Idõzítõ eseménykeztelõje.
        /// </summary>
        private void Timer_Tick(Object? sender, EventArgs e)
        {
            _model.AdvanceTime(); // játék léptetése

            picCanvas.Invalidate();   //frissíti the canvast
        }

        #endregion

        #region Update PictureBox
        /// <summary>
        /// Grafikus picturebox felület megrarajzolása
        /// </summary>
        private void UpdatePictureBoxGraphic(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            for (int i = 0; i < _model.GetSnake.Count; i++)
            {
                if (i == 0)
                {
                    canvas.DrawImage(head!, _model.GetSnake[0].X * _model.CalcWidthandHeight, _model.GetSnake[0].Y * _model.CalcWidthandHeight, 
                        _model.CalcWidthandHeight, _model.CalcWidthandHeight);
                }
                else
                {
                    canvas.DrawImage(body!, _model.GetSnake[i].X * _model.CalcWidthandHeight, _model.GetSnake[i].Y * _model.CalcWidthandHeight,
                        _model.CalcWidthandHeight, _model.CalcWidthandHeight);
                }
            }
            
            canvas.DrawImage(nugets!, _model.GetFood.X * _model.CalcWidthandHeight, _model.GetFood.Y * _model.CalcWidthandHeight,
                _model.CalcWidthandHeight + 10, _model.CalcWidthandHeight + 10);

            
           
            for (int i = 0; i < _model.Table.BordersNumber; i++)
            {
                canvas.DrawImage(border!, _model.Table.BordersCoordinates[i].X * _model.CalcWidthandHeight, 
                    _model.Table.BordersCoordinates[i].Y * _model.CalcWidthandHeight,
                    _model.CalcWidthandHeight + 10, _model.CalcWidthandHeight + 10);
            }

        }

        #endregion

        #region KeyboardClick events
        /// <summary>
        /// billentyu esemenykezeloje
        /// </summary>
        private void SnakeForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode) // megkapjuk a billentyüt
            {
                case Keys.A:
                    _model.SetMove(Direction.goLeft);
                    e.SuppressKeyPress = true; // az eseményt nem adjuk tovább a vezérlõnek
                    break;
                case Keys.D:
                    _model.SetMove(Direction.goRight);
                    e.SuppressKeyPress = true;
                    break;
                case Keys.W:
                    _model.SetMove(Direction.goUp);
                    e.SuppressKeyPress = true;
                    break;
                case Keys.S:
                    _model.SetMove(Direction.goDown);
                    e.SuppressKeyPress = true;
                    break;
                //Handle the start/stop and reset buttons
                case Keys.Q:
                    if (startButton.Text == "Start")
                    {
                        _model.NewGame();
                        _timer.Start();
                        startButton.Text = "Pause";
                        startButton.BackColor = Color.LightGray;
                    }
                    else
                    {
                        if (_model.IsGamePaused == true)
                        {
                            _model.SetGamePaused(false);
                            startButton.Text = "Play";
                            startButton.BackColor = Color.DarkGray;
                        }
                        else
                        {
                            _model.SetGamePaused(true);
                            startButton.Text = "Pause";
                            startButton.BackColor = Color.LightGray;
                        }
                    }
                    break;
                case Keys.R:
                    _model.RestartGame();
                    SetupTable();
                    startButton.Text = "Start";
                    startButton.BackColor = Color.Lime;
                    break;
            }
        }

        #endregion

        #region ButtonClick events
        /// <summary>
        /// Gomb esemenykezeleje.
        /// </summary>
        private void Button_Click(object? sender, EventArgs e)
        {
            if (sender is Button button)
            {
                switch (button.Text)
                // megvizsgáljuk, milyen az eseményt kiváltó gomb felirata, így eldönthetjük, melyik gombot nyomták le
                {
                    case "Start":
                        _model.NewGame();
                        _timer.Start();
                        startButton.Text = "Pause";
                        startButton.BackColor = Color.LightGray;
                        break;
                    case "Pause":
                        _model.SetGamePaused(false);
                        startButton.Text = "Play";
                        startButton.BackColor = Color.DarkGray;
                        break;
                    case "Play":
                        _model.SetGamePaused(true);
                        startButton.Text = "Pause";
                        startButton.BackColor = Color.LightGray;
                        break;
                    case "Restart":
                        _model.RestartGame();
                        SetupTable();
                        startButton.Text = "Start";
                        startButton.BackColor = Color.Lime;
                        break;
                }
            }
        }

        #endregion

    }
}