using System;
using System.Collections.Generic;
using TextFile;

namespace Klub
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Teniszklub teniszklub = new();

            TextFileReader reader = new TextFileReader("adatok.txt");
            char[] separators = new char[] { ' ', '\t', };

            while (reader.ReadLine(out string line))
            {
                string[] tokens = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                try
                {
                    switch (tokens[0])
                    {
                        case "CREATE":
                            teniszklub.PályátLétrehoz(tokens[1], Convert.ToBoolean(tokens[2]));
                            break;
                        case "DELETE":
                            teniszklub.PályátTöröl(Convert.ToInt32(tokens[1]));
                            break;
                        case "IN":
                            new Személy(tokens[1], tokens[2]).Belép(teniszklub);
                            break;
                        case "OUT":
                            new Személy(tokens[1], tokens[2]).Kilép(teniszklub);
                            break;
                        case "BOOK":
                            new Személy(tokens[1], tokens[2]).Pályafoglalás(teniszklub, tokens[3], tokens[4]);
                            break;
                        case "DE_BOOK":
                            new Személy(tokens[1], tokens[2]).FoglalásTörlés(teniszklub, tokens[3]);
                            break;
                        case "PAY":
                            new Személy(tokens[1], tokens[2]).Fizetendő(teniszklub, tokens[3]);
                            break;
                        case "INCOME":
                            Console.WriteLine("A klub " + tokens[1] + " napi bevétele: " + teniszklub.Bevétel(tokens[1]));
                            break;
                        default: Console.WriteLine("Rossz parancs található az adott fileban!"); break;
                    }
                }
                catch (Teniszklub.IllegalTenniscourtType ex) { Console.WriteLine(ex.Message); }
                catch (Teniszklub.ReservationsAreAvailable ex) { Console.WriteLine(ex.Message); }
                catch (Teniszklub.TenniscourtNotExist ex) { Console.WriteLine(ex.Message); }
                catch (Teniszklub.RegisteredMember ex) { Console.WriteLine(ex.Message); }
                catch (Teniszklub.NotRegisteredMember ex) { Console.WriteLine(ex.Message); }
                catch (Teniszklub.NotExistThisTypeOfTenniscourt ex) { Console.WriteLine(ex.Message); }
                catch (Teniszklub.NotFoundFreeTenniscourt ex) { Console.WriteLine(ex.Message); }
                catch (Teniszklub.IllegalReservationTime ex) { Console.WriteLine(ex.Message); }
                catch (Teniszklub.NoReservationsAreAvailable ex) { Console.WriteLine(ex.Message); }
            }
        }
    }
}