using SnakeLib.Model;
using SnakeLib.Persistence;
using SnakeGame.ViewModel;

namespace SnakeGame
{
    public partial class App : Application
    {
        private readonly AppShell _appShell = null!;
        private readonly ISnakeDataAccess _snakeDataAccess;
        private readonly SnakeModel _model = null!;
        private readonly SnakeViewModel _viewModel = null!;

        public App()
        {
            InitializeComponent();

            _snakeDataAccess = new SnakeFileDataAccess("mapsize.txt"); 

            _model = new SnakeModel(_snakeDataAccess);
            _viewModel = new SnakeViewModel(_model);


            _appShell = new AppShell(_snakeDataAccess, _model, _viewModel)
            {
                BindingContext = _viewModel
            };

            MainPage = _appShell;
        }

    }
}
