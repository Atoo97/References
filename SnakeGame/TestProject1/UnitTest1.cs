using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
using SnakeLib.Model;
using SnakeLib.Persistence;
using Moq;
using SnakeGame.Model;


namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private SnakeModel _model = null!; // a tesztelendõ modell
        private SnakeTable _mockedTable = null!; // mockolt játéktábla
        private Mock<ISnakeDataAccess> _mock = null!; // az adatelérés mock-ja
        GameTableSize _mapSize = GameTableSize.Large;

        [TestInitialize]
        public void Initialize()
        {
            _mockedTable = new SnakeTable(40, 8);
            // elõre definiálunk egy játáktáblát a perzisztencia mockolt teszteléséhez

            _mock = new Mock<ISnakeDataAccess>();
            _mock.Setup(mock => mock.LoadAsync(_mapSize))
                .Returns(() => Task.FromResult(_mockedTable));
            // a mock a LoadAsync mûveletben bármilyen paraméterre az elõre beállátott játéktáblát fogja visszaadni

            _model = new SnakeModel(_mock.Object);
            // példányosítjuk a modellt a mock objektummal

            _model.Table.FieldsCoordinate.Add(new SnakeField { X = 4, Y = 4 });
            _model.Table.FieldsCoordinate.Add(new SnakeField { X = 3, Y = 2 });
            _model.Table.FieldsCoordinate.Add(new SnakeField { X = 2, Y = 5 });
            _model.Table.FieldsCoordinate.Add(new SnakeField { X = 4, Y = 4 });

        }

        [TestMethod]
        public void SnakeModelNewGameTest()
        {
            Assert.AreEqual(_model.Table.RegionSize, 25);
            Assert.AreEqual(_model.Table.FieldsCoordinate.Count, 4);
            Assert.AreEqual(_model.Table.FieldsCoordinate[1].X, 3);
            Assert.AreEqual(_model.Table.FieldsCoordinate[1].Y, 2);
            Assert.AreEqual(_model.Table.BordersNumber, 1);
        }

        [TestMethod]
        public void SnakeEatTest()
        {
            _model.NewGame();

            //kezdeti értékek
            Assert.AreEqual(_model.GameScores, 0);
            Assert.AreEqual(_model.GetSnake.Count, 5);

             _model.Eat();
             _model.Eat();

            Assert.AreEqual(_model.GameScores, 2);
            Assert.AreEqual(_model.GameHighScores, 2); //pontok nõttek 2-vel
            Assert.AreEqual(_model.GetSnake.Count, 7); //kigyó 2-vel nõtt

            _model.RestartGame();

            Assert.AreEqual(_model.GameScores, 0);
            Assert.AreEqual(_model.GameHighScores, 0); //max pontok 0-ázodtak
            Assert.AreEqual(_model.GetSnake.Count, 5); //kigyó mérete 5 lett megint
        }

        [TestMethod]
        public void SnakeGameOver()
        {
           _model.GetSnake.Add(new SnakeField { X = 12, Y = 12 });
            Assert.AreEqual(_model.GetSnake[0].X, 12); //kigyó fejének kiinduló helyzete
            Assert.AreEqual(_model.GetSnake[0].Y, 12);

            _model.Table.FieldsCoordinate.Add(new SnakeField { X = 11, Y = 12 }); //akadály a kigyóra helyez

            _model.SetGamePaused(false);


            //Eat the wall
            _model.SetMove(Direction.goLeft);
            for (int c = 0; c < 2; c++)
            {
                _model.GetSnake[0].X -= 1; //Léptetem a kígyót balra

                if ((_model.GetSnake[0].X == _model.Table.FieldsCoordinate[4].X && _model.GetSnake[0].Y == _model.Table.FieldsCoordinate[4].Y))
                {
                    _model.SetGamePaused(true);
                }
            }

            Assert.IsTrue(_model.IsGamePaused);

        }

    }
}