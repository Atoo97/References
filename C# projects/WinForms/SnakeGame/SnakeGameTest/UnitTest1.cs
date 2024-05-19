using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Game.SnakeGameConzol.Model;
using Game.SnakeGameConzol.Persistance;
using Moq;


namespace Game.SnakeGameTest
{

    [TestClass]
    public class UnitTest1
    {

        private SnakeModel _model = null!; // a tesztelendõ modell
        private SnakeTable _mockedTable = null!; // mockolt játéktábla
        private Mock<ISnakeDataAccess> _mock = null!; // az adatelérés mock-ja
        MapSize _mapSize = MapSize.Large;

        [TestInitialize]
        public void Initialize()
        {
            _mockedTable = new SnakeTable(600,8);
            // elõre definiálunk egy játéktáblát a perzisztencia mockolt teszteléséhez

            _mock = new Mock<ISnakeDataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<String>(), _mapSize))
                .Returns(() => Task.FromResult(_mockedTable));
            // a mock a LoadAsync mûveletben bármilyen paraméterre az elõre beállított játéktáblát fogja visszaadni

            _model = new SnakeModel(_mock.Object);
            // példányosítjuk a modellt a mock objektummal

            _model.Table.BordersCoordinates.Add(new FigShapes { X = 4, Y = 4 });
            _model.Table.BordersCoordinates.Add(new FigShapes { X = 3, Y = 2 });
            _model.Table.BordersCoordinates.Add(new FigShapes { X = 2, Y = 5 });
            _model.Table.BordersCoordinates.Add(new FigShapes { X = 4, Y = 4 });

            _model.SetMaxHeight = (int)(_mockedTable.RegionSize / _model.CalcWidthandHeight) - 1;
            _model.SetMaxWidth = (int)(_mockedTable.RegionSize / _model.CalcWidthandHeight) - 1;

        }

        [TestMethod]
        public void SnakeModelNewGameTest()
        {
            Assert.AreEqual(_model.Table.RegionSize, 600);
            Assert.AreEqual(_model.Table.BordersCoordinates.Count, 4);
            Assert.AreEqual(_model.Table.BordersCoordinates[1].X, 3);
            Assert.AreEqual(_model.Table.BordersCoordinates[1].Y, 2);
            Assert.AreEqual(_model.Table.BordersNumber, 4);

        }

        [TestMethod]
        public void SnakeEatTest()
        {
            _model.NewGame();

            //kezdeti értékek
            Assert.AreEqual(_model.GameScores, 0);
            Assert.AreEqual(_model.GetSnake.Count, 5);

            _model.SetFood(2, 5);

            for (int i = 0; i < _model.Table.BordersNumber; i++)
            {
                if (_model.Table.BordersCoordinates[i].X == _model.GetFood.X
                        && _model.Table.BordersCoordinates[i].Y == _model.GetFood.Y)
                {
                    _model.Eat();
                    _model.SetFood(4, 4);
                }
            }

            Assert.AreEqual(_model.GameScores, 2);
            Assert.AreEqual(_model.GameHighScoresTest, 2); //pontok nõttek 2-vel
            Assert.AreEqual(_model.GetSnake.Count, 7); //kigyó 2-vel nõtt

            _model.RestartGame();

            Assert.AreEqual(_model.GameScores, 0);
            Assert.AreEqual(_model.GameHighScoresTest, 0); //max pontok 0ázodtak
            Assert.AreEqual(_model.GetSnake.Count, 5); //kigyó mérete 5 lett megint
        }

        [TestMethod]
        public void SnakeGameOver() 
        {
            _model.NewGame();
            _model.SetFood(2, 5);

            for (int i = 0; i < _model.Table.BordersNumber; i++)
            {
                if (_model.Table.BordersCoordinates[i].X == _model.GetFood.X
                        && _model.Table.BordersCoordinates[i].Y == _model.GetFood.Y)
                {
                    _model.Eat();
                    _model.SetFood(4, 4);
                }
            }

            Assert.AreEqual(_model.GameScores, 2);
            Assert.AreEqual(_model.GetSnake[0].X, 11); //kigyó fejének kiinduló helyzete
            Assert.AreEqual(_model.GetSnake[0].Y, 11);

            _model.Table.BordersCoordinates.Add(new FigShapes { X = 9, Y = 11 }); //akadály a kigyóra helyez

            _model.SetGamePaused(false);


            //Eat the wall
            for (int c = 0; c < _model.Table.BordersCoordinates.Count; c++)
            {
                _model.AdvanceTime(); //léptetem a kigyót balra

                if ((_model.GetSnake[0].X == _model.Table.BordersCoordinates[4].X && _model.GetSnake[0].Y == _model.Table.BordersCoordinates[4].Y) ||
                    (_model.GetSnake[0].X == _model.Table.BordersCoordinates[4].X & _model.GetSnake[0].Y == _model.Table.BordersCoordinates[4].Y))
                {
                    _model.SetGamePaused(true);
                }
            }

            Assert.IsTrue(_model.IsGamePaused);



           
            


        }


    }
}