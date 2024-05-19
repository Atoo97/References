using System.Xml;
using ZH_forms1_model.Model;

namespace ZH_forms1.View
{
    public partial class Form1 : Form
    {
        #region Fields
        private GameModel _gameModel = null!;
        private Button[,] _buttonGrid = null!;
        private Button[,] _nextBlockGrid = null!;

        #endregion


        public Form1()
        {
            _gameModel = new GameModel();
            _gameModel.PointChanged += Model_PointChanged;
            _gameModel.FieldChanged += new EventHandler<FieldChangeEventArgs>(Model_FieldChanged);
            _gameModel.LineFilled += Model_LineFilled;
            _gameModel.NextBlockChanged += Model_NextBlockChanged;
            _gameModel.GameOver += new EventHandler<int>(Model_GameOver);

            InitializeComponent();

        }

        #region menu Methods
        private void newGame_Clicked(object sender, EventArgs e)
        {
            _gameModel.NewGame();
            GenerateTable();
            GenerateNextBlock();
            SetNextBlock();
        }

        private void exit_Clicked(object sender, EventArgs e)
        {
            Close();
        }

        #endregion


        #region private Methods
        private void Model_PointChanged(object? sender, EventArgs e)
        {
            pointLabel.Text = _gameModel.Points.ToString();
        }

        private void Model_FieldChanged(object? sender, FieldChangeEventArgs e)
        {
            switch (e.IsBlue)
            {
                case true:
                    _buttonGrid[e.X, e.Y].BackColor = Color.Blue;
                    break;
                case false:
                    _buttonGrid[e.X, e.Y].BackColor = Color.White;
                    break;
            }
        }

        private void Model_LineFilled(object? sender, EventArgs e)
        {
            SetTable();
        }

        private void Model_NextBlockChanged(object? sender, EventArgs e)
        {
            SetNextBlock();
        }

        private void SetTable()
        {
            for (Int32 i = 0; i < 4; i++)
                for (Int32 j = 0; j < 4; j++)
                {
                    switch (_gameModel[i, j])
                    {
                        case false:
                            _buttonGrid[i, j].BackColor = Color.White;
                            break;
                        case true:
                            _buttonGrid[i, j].BackColor = Color.Blue;
                            break;
                    }
                }
        }

        private void GenerateTable()
        {
            _buttonGrid = new Button[4, 4]; //beégetve
            for (Int32 i = 0; i < 4; i++)
            {
                for (Int32 j = 0; j < 4; j++)
                {
                    _buttonGrid[i, j] = new GridButton(i, j);
                    _buttonGrid[i, j].BackColor = Color.White;
                    _buttonGrid[i, j].Size = new Size(100, 100);
                    _buttonGrid[i, j].MouseClick += ButtonGrid_MouseClick; // ha lekattintják a táblázat elemét

                    mainTable.Controls.Add(_buttonGrid[i, j], j, i);
                }
            }
        }

        private void GenerateNextBlock()
        {
            _nextBlockGrid = new Button[2, 2];  //beégetve
            for (Int32 i = 0; i < 2; i++)
            {
                for (Int32 j = 0; j < 2; j++)
                {
                    _nextBlockGrid[i, j] = new GridButton(i, j);
                    _nextBlockGrid[i, j].BackColor = Color.White;
                    _nextBlockGrid[i, j].Size = new Size(100, 100);
                    _nextBlockGrid[i, j].Enabled = false;

                    nextBlock.Controls.Add(_nextBlockGrid[i, j], j, i);
                }
            }
        }

        private void SetNextBlock()     //Lekérdezzük adott button true-e ha igen beszinezzük
        {
            for (Int32 i = 0; i < 2; i++)       //Beégetve a méret
                for (Int32 j = 0; j < 2; j++)
                {
                    switch (_gameModel.NextBlock(i, j))
                    {
                        case false:
                            _nextBlockGrid[i, j].BackColor = Color.White;
                            break;
                        case true:
                            _nextBlockGrid[i, j].BackColor = Color.Blue;
                            break;
                    }
                }
        }

        private void ButtonGrid_MouseClick(object? sender, MouseEventArgs e)
        {
            if (sender is GridButton button)
            {
                Int32 x = button.GridX;
                Int32 y = button.GridY;

                try
                {
                    _gameModel.StepGame(x, y);
                }
                catch
                {
                }
            }
        }

        private void Model_GameOver(object? sender, int e)
        {
            DialogResult dialogResult =
                MessageBox.Show("Congratulations!\nYour score is: " + e.ToString() + " points!\nDo you want to start a new game?",
                                                    "Game Over", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                _gameModel.NewGame();


                SetTable();
                SetNextBlock();
            }
            else if (dialogResult == DialogResult.No)
            {
                Close();
            }
        }
        #endregion
    }
}
