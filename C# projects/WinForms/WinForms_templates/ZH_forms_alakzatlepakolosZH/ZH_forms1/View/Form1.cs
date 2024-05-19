using System.Xml;
using ZH_forms1_model.Model;

namespace ZH_forms1.View
{
    public partial class Form1 : Form
    {
        #region Fields
        private GridButton[,] _buttonGrid = null!;
        private GridButton[,] _blockGrid = null!;
        private GameModel _gameModel = null!;

        #endregion


        public Form1()
        {
            _gameModel = new GameModel();

            //connect to private methods, this called when event changed in model
            _gameModel.NewGame += setUpNewGame;
            _gameModel.GameAdvance += gameAdvance;
            _gameModel.GameOver += gameOver;
            _gameModel.StepsChange += GameStepsChange;
            _gameModel.IllegalStep += GameIllegalStep;

            InitializeComponent();
        }


        #region menu Methods
        private void newGame_Click(object sender, EventArgs e)
        {
            _gameModel.modelNewGame();
        }

        private void setSize5x5_Click(object sender, EventArgs e)
        {
            _gameModel.modelSetTable5x5();
        }

        private void setSize7x7_Click(object sender, EventArgs e)
        {
            _gameModel.modelSetTable7x7();
        }

        private void setSize9x9_Click(object sender, EventArgs e)
        {
            _gameModel.modelSetTable9x9();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion


        #region private Methods
        private void setUpNewGame(object? sender, NewGameEventArgs e)                   //Pálya kirajzoltatása
        {
            if (gameTable.Controls.Count != 0)
            {
                gameTable.Controls.Clear();
            }
            _buttonGrid = new GridButton[e.size, e.size];
            int buttonSize = gameTable.Width / e.size;
            for (int i = 0; i < e.size; i++)
            {
                for (int j = 0; j < e.size; j++)
                {
                    _buttonGrid[i, j] = new GridButton(i, j);
                    _buttonGrid[i, j].Location = new Point(j * buttonSize, i * buttonSize); //x = col, y = row
                    _buttonGrid[i, j].Size = new Size(buttonSize, buttonSize); //size
                    _buttonGrid[i, j].FlatStyle = FlatStyle.Flat;
                    _buttonGrid[i, j].BackColor = Color.White;
                    _buttonGrid[i, j].Click += buttonClicked; //esemény figyelõ

                    //Add to the gameTable Panel
                    gameTable.Controls.Add(_buttonGrid[i, j]);
                }
            }

            if (blockTable.Controls.Count != 0)
            {
                blockTable.Controls.Clear();
            }
            _blockGrid = new GridButton[3, 3];
            buttonSize = 300 / 3;                   //beégetve a téblaméret
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _blockGrid[i, j] = new GridButton(i, j);
                    _blockGrid[i, j].Location = new Point(j * buttonSize, i * buttonSize); //x = col, y = row
                    _blockGrid[i, j].Size = new Size(buttonSize, buttonSize); //size
                    _blockGrid[i, j].FlatStyle = FlatStyle.Flat;

                    if (_gameModel.GetBlockField(i, j))
                    {
                        _blockGrid[i, j].BackColor = Color.Blue;
                    }

                    _blockGrid[i, j].Enabled = false; //not pressable

                    //Add to the gameTable Panel
                    blockTable.Controls.Add(_blockGrid[i, j]);
                }
            }


        }

        private void gameAdvance(object? sender, GameAdvanceEventArgs e)  //ujraszinezi a pályát és átírja szöveget
        {
            for (int i = 0; i < e.gameTable.GetLength(0); i++)
            {
                for (int j = 0; j < e.gameTable.GetLength(1); j++)
                {
                    if (e.gameTable[i, j].player == Player.FstPlayer)
                    {
                        _buttonGrid[i, j].BackColor = Color.Blue;
                    }
                    else if (e.gameTable[i, j].player == Player.SndPlayer)
                    {
                        _buttonGrid[i, j].BackColor = Color.Red;
                    }
                    else
                    {
                        _buttonGrid[i, j].BackColor = Color.White;
                    }
                }
            }

            for (int i = 0; i < e.blockTable.GetLength(0); i++)
            {
                for (int j = 0; j < e.blockTable.GetLength(1); j++)
                {
                    if (e.blockTable[i, j].isFiled)
                    {
                        if (_gameModel.GetPlayer() == Player.FstPlayer)
                        {
                            _blockGrid[i, j].BackColor = Color.Blue;
                        }
                        else
                        {
                            _blockGrid[i, j].BackColor = Color.Red;
                        }
                    }
                    else
                    {
                        _blockGrid[i, j].BackColor = Color.White;
                    }
                }
            }
        }

        private void gameOver(object? sender, GameOverEventArgs e)
        {
            if (e.isWon)
            {
                int winnerpoints;
                Player winner = Player.Empty;
                if (_gameModel.GetFstPoints() >= _gameModel.GetSndPoints())
                {
                    winnerpoints = _gameModel.GetFstPoints();
                    winner = Player.FstPlayer;
                }
                else
                {
                    winnerpoints = _gameModel.GetSndPoints();
                    winner = Player.SndPlayer;
                }

                DialogResult result = MessageBox.Show("YOU WON! Player: " + winner.ToString()+ " ,points: " + winnerpoints.ToString(), "Game Over", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    _gameModel.modelNewGame();
                }
                else
                {
                    Close();
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("YOU LOST! Do you want to play a new game?", "Game Over", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    _gameModel.modelNewGame();
                }
                else
                {
                    Close();
                }
            }
        }

        private void GameIllegalStep(object? sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Illegal step! It's illegal to drop figures here!", "Step again", MessageBoxButtons.OK);
            if (result == DialogResult.OK)
            {
                
            }
        }


        private void buttonClicked(object? sender, EventArgs e)
        {
            GridButton? clickedButton = sender as GridButton;

            if (clickedButton != null)
            {
                int row = clickedButton.row;
                int col = clickedButton.col;
                _gameModel.modelButtonClicked(row, col);
            }
        }

        private void GameStepsChange(object? sender, int steps)                    //hátralévõ lépésszámok kiiratása
        {
            stepsLabel.Text = steps.ToString();
        }

        #endregion

    }
}
