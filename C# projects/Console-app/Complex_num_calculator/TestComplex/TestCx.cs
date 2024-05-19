using Complex_feladat;

namespace TestComplex
{
    [TestClass]
    public class TestCx
    {
        [TestMethod]
        public void TestAdd()
        {
            Complex a = new (2,4);
            Complex b = new(5,3);
            Assert.AreEqual(Complex.Add(a, b).ToString(), "7+7i");
        }

        [TestMethod]
        public void TestSub()
        {
            Complex a = new(2, 4);
            Complex b = new(5, 4);
            Assert.AreEqual(Complex.Sub(a, b).ToString(), "-3+0i");
        }

        [TestMethod]
        public void TestNull()
        {
            Complex a = new(2, 4);
            Complex b = new(5, 4);
            Assert.AreEqual(Complex.Null(a, b).ToString(), "-6+28i");
        }

        [TestMethod]
        public void TestDivl()
        {
            Complex a = new(2, 4);
            Complex b = new(1, 2);
            Assert.AreEqual(Complex.Div(a, b).ToString(), "2+0i");

            Complex c = new(2, 4);
            Complex d = new(0, 0);
            Assert.ThrowsException<Complex.IllegalArgumentException>(() => Complex.Div(c,d));
        }
    }
}