namespace Complex_feladat
{
    public class Complex
    {
        private int x, y;
        public class IllegalArgumentException : Exception { }

        public Complex(int a, int b)
        {
            this.x = a;
            this.y = b;
        }

        public static Complex Add(Complex a, Complex b)
        {
            Complex c = new Complex((a.x + b.x), (a.y + b.y));
            return c;
        }

        public static Complex Sub(Complex a, Complex b)
        {
            Complex c = new Complex((a.x - b.x), (a.y - b.y));
            return c;
        }

        public static Complex Null(Complex a, Complex b)
        {
            Complex c = new Complex((a.x * b.x - a.y * b.y), (a.x * b.y + a.y * b.x));
            return c;
        }

        public static Complex Div(Complex a, Complex b)
        {
            if (b.x == 0 && b.y == 0)
            {
                throw new IllegalArgumentException(); //"Invalid j argument!"
            }
            else
            {
                int i = (b.x * b.x + b.y * b.y);
                Complex c = new Complex((a.x * b.x + a.y * b.y) / i, (a.y * b.x - a.x * b.y) / i);
                return c;
            }
        }
        //(a.x*b.x + a.y*b.y) / Math.Sqrt(b.x*b.x + b.y*b.y), (a.y * b.x - a.x * b.y) / Math.Sqrt(b.x * b.x + b.y * b.y)

        
        public override string ToString()
        {
            return x + "+" + y + "i";
        }
        
    }
}