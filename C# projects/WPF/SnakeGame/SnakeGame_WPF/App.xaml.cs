using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Windows.Controls;
using System.Windows;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Input;
using SnakeGame_WPF.ViewModel;
using SnakeGame_WPF.View;
using SnakeGame.Model;
using SnakeGame.Persistence;

namespace SnakeGame_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private bool notFirstSatrt = false; // első indtást jelzi

        private SnakeModel _model = null!;
        private SnakeViewModel _viewModel = null!;
        private MainWindow _view = null!;
        private System.Windows.Threading.DispatcherTimer _timer = null!;


        #endregion

        #region Constructors

        /// <summary>
        /// Alkalmazás példányosítása.
        /// </summary>
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        #endregion

        #region Application event handlers
        private void App_Startup(object? sender, StartupEventArgs e)
        {
            // modell létrehozása
            _model = new SnakeModel(new SnakeFileDataAcess());
            _model.GameOver += new EventHandler<SnakeEventArgs>(Model_GameOver);

            // nézemodell létrehozása
            _viewModel = new SnakeViewModel(_model);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.StartStopGame += new EventHandler(ViewModel_StartStopGame);
            _viewModel.RestartGame += new EventHandler(ViewModel_RestartGame);

            // nézet létrehozása
            _view = new MainWindow();
           _view.DataContext = _viewModel;
            _viewModel.LoadGameCommand.Execute("Large");
           _view.Show();


            // időzítő létrehozása
            _timer = new System.Windows.Threading.DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.06);
            _timer.Tick += new EventHandler(Timer_Tick);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _model.AdvanceTime();
        }

        #endregion

        #region ViewModel event handlers

        /// <summary>
        /// Játék indításának eseménykezelője.
        /// </summary>
        private void ViewModel_StartStopGame(object? sender, System.EventArgs e) 
        {
            if (_timer.IsEnabled)
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
        private void ViewModel_RestartGame(object? sender, System.EventArgs e)
        {
            MessageBox.Show("Biztos újra kezde a játékot? Az eddig pontjaid elvesznek!", "Snake", MessageBoxButton.OK, MessageBoxImage.Error);
            _model.RestartGame();
        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        
        private async void ViewModel_LoadGame(object? sender, System.EventArgs e)
        {
            
            if (notFirstSatrt) //első indításkor NEM lefutó elágazás
            {
                MessageBox.Show("Biztos pályaméretet szeretnél módosítani? Az eddig pontjaid elvesznek!", "Snake", MessageBoxButton.OK, MessageBoxImage.Error);
                _model.RestartGame();
            }

            notFirstSatrt = true;
            
        

            String size = _viewModel.Field_Size;
            switch (size)
            {
                case "Small":
                    _model.SetMapsize(MapSize.Small);
                    break;
                case "Medium":
                    _model.SetMapsize(MapSize.Medium);
                    break;
                case "Large":
                    _model.SetMapsize(MapSize.Large);
                    break;
            }

            try
            {
                // játék betöltése .txt állományból
                await _model.LoadGameAsync(@".\Persistence\mapsize.txt");
            }
            catch (SnakeDataException)
            {
                MessageBox.Show("A fájl betöltése sikertelen!", "Snake", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        

        #endregion

        #region Model event handler
        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_GameOver(Object? sender, SnakeEventArgs e)
        {
            _timer.Stop(); //leáll az idő

            if (e.IsOver) //ha a kigyó akadályba ütközött
            {
                MessageBox.Show("Vége a játéknak! Akadályba ütköztél!" + Environment.NewLine +
                                "Összesen " + e.ScoresCount + " tojást sikerült megenned.",
                                "Snake game",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            }
            else //ha a kigyó saját magába harapottgo
            {
                MessageBox.Show("Vége a játéknak! A kigyó öngyilkos lett!" + Environment.NewLine +
                                "Összesen " + e.ScoresCount + " tojást sikerült megenned.",
                                "Snake game",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);

            }

        }

        #endregion

    }
}
