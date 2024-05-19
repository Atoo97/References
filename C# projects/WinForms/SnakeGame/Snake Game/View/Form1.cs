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

        private bool notFirstSatrt = false; // els� indt�st jelzi

        private readonly ISnakeDataAccess _dataAccess = null!; // adatel�r�s
        private readonly SnakeModel _model = null!; // j�t�kmodell
        private readonly System.Windows.Forms.Timer _timer = null!; // id�z�t�

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

            // adatel�r�s p�ld�nyos�t�sa
            _dataAccess = new SnakeFileDataAcess();

            // modell l�trehoz�sa �s az esem�nykezel�k t�rs�t�sa
            _model = new SnakeModel(_dataAccess);
            _model.SnakeDirectionChanged += new EventHandler<SnakeEventArgs>(Game_SnakeDirectionChanged);
            _model.GameAdvanced += new EventHandler<SnakeEventArgs>(Game_GameAdvanced);
            _model.GameOver += new EventHandler<SnakeEventArgs>(Game_GameOver);

            //EventArgs

            //Large kezd�m�ret j�t�kt�bla be�ll�t�sa �s bet�lt�se a konstruktor hiv�sakor
            LoadSnakeTable_Click(largeToolStripMenuItem, EventArgs.Empty);

            KeyPreview = true; // billenty�esem�ny kezeles
            KeyDown += new KeyEventHandler(SnakeForm_KeyDown);

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 100; // 0.1 m�sodpercenk�nt t�rt�n� mozgat�s a p�ly�n el�re
            _timer.Tick += new EventHandler(Timer_Tick); // id�z�tett esem�ny t�rs�t�sa
        }

        #endregion

        #region Table event handlers
        /// <summary>
        /// J�t�k bet�lt�s�nek esem�nykezel�je.
        /// </summary>
        private async void LoadSnakeTable_Click(object? sender, EventArgs e)
        {

            if (notFirstSatrt) //els� ind�t�skor NEM lefut� el�gaz�s
            {
                MessageBox.Show("Biztos p�lyam�retet szeretn�l v�ltani?" + Environment.NewLine +
                                "�sszes megszerzett pontod elveszted �gy!",
                                "Snake game",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);

                _model.RestartGame();
            }

            notFirstSatrt = true;


            if (sender is ToolStripMenuItem menuitem)
            {
                // megvizsg�ljuk, milyen az esem�nyt kiv�lt� gomb felirata, �gy eld�nthetj�k, melyik gombot nyomt�k le
                switch (menuitem.Text)
                {
                    case "Large":
                        //Sz�veg megveltoztat
                        txtMapSize.Text = "Map size: Large";
                        //P�lyamaret m�dos�t
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
                // j�t�k bet�lt�se .txt �llom�nyb�l
                await _model.LoadGameAsync(@".\Persistance\mapsize.txt");
            }
            catch (SnakeDataException)
            {
                MessageBox.Show("J�t�k bet�lt�se sikertelen!" + Environment.NewLine + "Hib�s az el�r�si �t, vagy a f�jlform�tum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Be�ll�tja canvas - p�lyamareteket �s hat�rokat
            SetupTable();
        }

        #endregion

        #region Private methods
        /// <summary>
        /// T�bla �s grafikus k�rnyezetbe�ll�t�sa.
        /// </summary>
        private void SetupTable()
        {

            //picturebox h�tt�r be�ll�t�sa:
            this.picCanvas.BackgroundImage = Image.FromFile("bg.jpg");
            this.picCanvas.BackgroundImageLayout = ImageLayout.Stretch;
            this.DoubleBuffered = true;

            //SnakeGame-hez tartoz� k�pek kezel�se
            imageParts = Directory.GetFiles("snake", "*.png").ToList();
            head = Image.FromFile(imageParts[2])!;
            body = Image.FromFile(imageParts[6])!;
            nugets = Image.FromFile(imageParts[1])!;
            border = Image.FromFile(imageParts[5])!;

            //itt �ll�tjuk majd be a picturebox m�ret�t nxn-esre
            this.picCanvas.Size = new System.Drawing.Size(_model.Table.RegionSize, _model.Table.RegionSize);

        }

        #endregion

        #region Game event handlers
        /// <summary>
        /// k�gy�fej ir�nyv�lt�s�nak esem�nykezel�je.
        /// </summary>
        private void Game_SnakeDirectionChanged(Object? sender, SnakeEventArgs e)
        {
            //k�gy� fej�nek k�p cser�je �s ir�nya be�ll�t
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
        /// J�t�k el�rehalad�s�nak esem�nykezel�je.
        /// </summary>
        /// 
        private void Game_GameAdvanced(Object? sender, SnakeEventArgs e)
        {
            txtScore.Text = "Score: " + e.ScoresCount.ToString();
            txtHighScore.Text = "High score: " + e.HighScoresCount.ToString();
            // j�t�k pontok friss�t�se
        }


        /// <summary>
        /// J�t�k v�g�nek esem�nykezel�je.
        /// </summary>
        private void Game_GameOver(Object? sender, SnakeEventArgs e)
        {
            _timer.Stop(); //le�ll az id�

            startButton.Enabled = false;
            txtScore.Text = "Score: 0";

            if (e.IsOver) //ha a k�gy� akad�lyba �tk�z�tt
            {
                MessageBox.Show("V�ge a j�t�knak! Akad�lyba �tk�zt�l!" + Environment.NewLine +
                                "�sszesen " + e.ScoresCount + " toj�st siker�lt megenned.",
                                "Snake game",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);
            }
            else //ha a k�gy� saj�t mag�ba harapott
            {
                MessageBox.Show("V�ge a j�t�knak! A k�gy� �ngyilkos lett!" + Environment.NewLine +
                                "�sszesen " + e.ScoresCount + " toj�st siker�lt megenned.",
                                "Snake game",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);

            }
        }

        #endregion

        #region Timer event handlers
        /// <summary>
        /// Id�z�t� esem�nykeztel�je.
        /// </summary>
        private void Timer_Tick(Object? sender, EventArgs e)
        {
            _model.AdvanceTime(); // j�t�k l�ptet�se

            picCanvas.Invalidate();   //friss�ti the canvast
        }

        #endregion

        #region Update PictureBox
        /// <summary>
        /// Grafikus picturebox fel�let megrarajzol�sa
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
            switch (e.KeyCode) // megkapjuk a billenty�t
            {
                case Keys.A:
                    _model.SetMove(Direction.goLeft);
                    e.SuppressKeyPress = true; // az esem�nyt nem adjuk tov�bb a vez�rl�nek
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
                // megvizsg�ljuk, milyen az esem�nyt kiv�lt� gomb felirata, �gy eld�nthetj�k, melyik gombot nyomt�k le
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