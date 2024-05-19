
namespace Complex_feladat
{
    class Menu
    {
        private readonly Complex[] matrices = new Complex[2];

        public void Run()
        {
            int n;
            do
            {
                MenuWrite();
                try
                {
                    n = int.Parse(Console.ReadLine()!);
                }
                catch (FormatException)
                {
                    n = -1;
                }

                switch (n)
                {
                    case 1:
                        GetDatas();
                        AddComplex();
                        Console.ReadKey();
                        break;
                    case 2:
                        GetDatas();
                        SubComplex();
                        Console.ReadKey();
                        break;
                    case 3:
                        GetDatas();
                        NullComplex();
                        Console.ReadKey();
                        break;
                    case 4:
                        GetDatas();
                        DivComplex();
                        Console.ReadKey();
                        break;
                    case 0:
                    default:
                        return;
                }

            } while (n != 0);
        }


        private void AddComplex() {
            Console.WriteLine($"\nAddition of two complex numbers: ({matrices[0]}) + ({matrices[1]})");
            Console.WriteLine(Complex.Add(matrices[0], matrices[1]));
        }

        private void SubComplex()
        {
            Console.WriteLine($"\nSubtracting two complex numbers: ({matrices[0]}) - ({matrices[1]})");
            Console.WriteLine(Complex.Sub(matrices[0], matrices[1]));
        }

        private void NullComplex()
        {
            Console.WriteLine($"\nMultiplication of two complex numbers: ({matrices[0]}) * ({matrices[1]})");
            Console.WriteLine(Complex.Null(matrices[0], matrices[1]));
        }

        private void DivComplex()
        {
            try
            {
                Console.WriteLine($"\nDivision of two complex numbers: ({matrices[0]}) / ({matrices[1]})");
                Console.WriteLine(Complex.Div(matrices[0], matrices[1]));
            }
            catch (Complex.IllegalArgumentException)
            {

                Console.WriteLine("Invalid second complex number (a) and (b) argument!");
            }     
        }

        public void GetDatas() {
            int n;
            int m;
            bool ok;
            do
            {
                ok = false;
                try
                {
                    Console.Write("First complex number (a) argument: ");
                    n = int.Parse(Console.ReadLine()!);
                    Console.Write("First complex number (b) argument: ");
                    m = int.Parse(Console.ReadLine()!);

                    matrices[0] = new Complex(n,m);

                    Console.Write("Second complex number (a) argument: ");
                    n = int.Parse(Console.ReadLine()!);
                    Console.Write("Second complex number (b) argument: ");
                    m = int.Parse(Console.ReadLine()!);

                    matrices[1] = new Complex(n, m);

                    ok = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Elements must be a number!");
                }
            } while (!ok);
        }


        private static void MenuWrite()
        {
            Console.Clear();
            Console.WriteLine("\n 0. - Quit");
            Console.WriteLine(" 1. - Addition of two complex numbers");
            Console.WriteLine(" 2. - Subtracting two complex numbers");
            Console.WriteLine(" 3. - Multiplication of two complex numbers");
            Console.WriteLine(" 4. - Division of two complex numbers");
        }
    }

    internal class Program
    {
        static void Main()
        {
            Menu m = new Menu();
            m.Run();
        }
    }
}