using System.Numerics;
using System.Xml;
using ZH_forms1_model.Model;

namespace ZH_forms1.View
{

    public partial class Form1 : Form
    {
        #region Fields
        private GridButton[,] _buttonGrid = null!;
        private GameModel _gameModel = null!;

        #endregion


        public Form1()
        {
            _gameModel = new GameModel();

            //connect to private methods, this called when event changed in model
            _gameModel.NewGame += setUpNewGame;
            _gameModel.GameAdvance += gameAdvance;
            _gameModel.GameOver += gameOver;

            InitializeComponent();
        }


        #region menu Methods
        private void newGame_Click(object sender, EventArgs e)
        {
            _gameModel.modelNewGame();
        }

        private void setSize4x4_Click(object sender, EventArgs e)
        {
            _gameModel.modelSetTable4x4();
        }

        private void setSize6x6_Click(object sender, EventArgs e)
        {
            _gameModel.modelSetTable6x6();

        }

        private void setSize8x8_Click(object sender, EventArgs e)
        {
            _gameModel.modelSetTable8x8();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion


        #region private Methods
        private void setUpNewGame(object? sender, NewGameEventArgs e)                   //P�lya kirajzoltat�sa
        {
            //GameTable egy panel 500x500 m�retben
            //e.size a kock�k m�ret�nek sz�lsess�g�t adja int-be

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
                     _buttonGrid[i, j].Click += buttonClicked; //esem�ny figyel�

                    //Add to the gameTable Panel
                    gameTable.Controls.Add(_buttonGrid[i, j]);
                }
            }
        }

        private void gameAdvance(object? sender, GameAdvanceEventArgs e)  
        {
            for (int i = 0; i < e.gameTable.GetLength(0); i++)
            {
                for (int j = 0; j < e.gameTable.GetLength(1); j++)
                {
                    if (e.gameTable[i, j].player.ToString() == "FstPlayer")
                    {
                        _buttonGrid[i,j].BackColor = Color.Blue;
                        _buttonGrid[i, j].Text = e.gameTable[i, j].playerFigure.ToString();
                    }
                    else if (e.gameTable[i, j].player.ToString() == "SndPlayer")
                    {
                        _buttonGrid[i, j].BackColor = Color.Red;
                        _buttonGrid[i, j].Text = e.gameTable[i, j].playerFigure.ToString();
                    }
                    else
                    {
                        _buttonGrid[i, j].BackColor = Color.White;
                        _buttonGrid[i, j].Text = "";
                    }
                }
            }

            figureNum.Text = e.round.ToString();
            Player.Text = e.player.ToString();
        }

        private void gameOver(object? sender, GameOverEventArgs e)
        {
            if (e.isWon)
            {
                DialogResult result = MessageBox.Show("YOU WON! Do you want to play a new game?", "Game Over", MessageBoxButtons.YesNo);
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

        #endregion
    }
}
