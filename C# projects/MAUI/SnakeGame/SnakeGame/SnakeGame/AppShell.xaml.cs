using SnakeLib.Model;
using SnakeLib.Persistence;
using SnakeGame.View;
using SnakeGame.ViewModel;
using Microsoft.Maui.Dispatching;
using System.Threading;

namespace SnakeGame
{
    public partial class AppShell : Shell
    {
        #region Fields
        private bool notFirstSatrt = false; // első indtást jelzi

        private ISnakeDataAccess _snakeDataAccess;
        private readonly SnakeModel _model;
        private readonly SnakeViewModel _viewModel;
        private IDispatcherTimer _timer = null!;
        #endregion

        public AppShell(ISnakeDataAccess snakeDataAccess,SnakeModel model, SnakeViewModel viewModel)
        {
            InitializeComponent();

            // játék összeállítása
            _snakeDataAccess = snakeDataAccess;

            _model = model;
            _model.GameOver += new EventHandler<SnakeEventArgs>(Model_GameOver);

            _viewModel = viewModel;
            _viewModel.LoadGame += SnakeViewModel_LoadGame;
            _viewModel.StartStopGame += new EventHandler(SnakeViewModel_StartStopGame);
            _viewModel.RestartGame += new EventHandler(ViewModel_RestartGame);

            //időzítő létrehoz
            _timer = Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += new EventHandler(Timer_Tick);


            //kezdeti large pályabetöltése
            _viewModel.SizeChangeCommand.Execute("Large");
        }


        #region ViewModel event handlers
        private void Timer_Tick(object sender, EventArgs e)
        {
            _model.AdvanceTime();
        }

        /// <summary>
        /// Játék betöltésének/ pályaméretváltásának eseménykezelője.
        /// </summary>
        private async void SnakeViewModel_LoadGame(object sender, EventArgs e)
        {
            if (notFirstSatrt) //első indításkor NEM lefutó elágazás
            {
                bool answer = await DisplayAlert("Figyelem!",
                                                "Biztos pályát váltasz?", "Yes", "No");
                //_model.RestartGame();
            }

            notFirstSatrt = true;

            _timer.Stop();
            await  _model.LoadGameAsync();
        }

        /// <summary>
        /// Játék indításának eseménykezelője.
        /// </summary>
        private void SnakeViewModel_StartStopGame(object sender, System.EventArgs e)
        {
            if (_timer.IsRunning)
            {
                if (_model.IsGamePaused == false)
                {
                    _model.SetGamePaused(true);
                }
                else
                {
                    _model.SetGamePaused(false);
                }
            }
            else
            {
                _model.NewGame();
                _timer.Start();
            }
            
        }

        /// <summary>
        /// Játék újraindításának eseménykezelője.
        /// </summary>
        private async void ViewModel_RestartGame(object sender, System.EventArgs e)
        {
            bool answer = await DisplayAlert("Figyelem!",
                                               "Biztos újra kezded a játékot?", "Yes", "No");
            if (answer)
            {
                await _model.LoadGameAsync();
                _model.RestartGame();
            }
        }

        #endregion

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private async void Model_GameOver(Object sender, SnakeEventArgs e)
        {
            _timer.Stop(); //leáll az idő

            if (e.IsOver) //ha a kigyó akadályba ütközött
            {
                bool answer = await DisplayAlert("Vége a játéknak! Akadályba ütköztél!" + Environment.NewLine +
                                                 "Összesen " + e.ScoresCount + " tojást sikerült megenned.",
                                                "Új játékot kezdesz?", "Yes", "No");
                if (answer)
                {
                    await _model.LoadGameAsync();
                    _model.NewGame();
                }
            }
            else //ha a kigyó saját magába harapottgo
            {
                bool answer = await DisplayAlert("Vége a játéknak! A kigyó öngyilkos lett!" + Environment.NewLine +
                                                    "Összesen " + e.ScoresCount + " tojást sikerült megenned.",
                                                  "Új játékot kezdesz?", "Yes", "No");
                if (answer)
                {
                    await _model.LoadGameAsync();
                    _model.NewGame();
                }
            }

        }

    }
}
