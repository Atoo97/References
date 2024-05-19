using Microsoft.VisualStudio.TestTools.UnitTesting;
using Klub;
using System.Collections.Generic;

namespace TesztTeniszklub
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void P�ly�tL�tehozTest()
        {
            Teniszklub klub = new();
            klub.P�ly�tL�trehoz("Salakos", true);

            Assert.IsNotNull(klub.Teniszp�ly�k);
            klub.P�ly�tL�trehoz("M�anyag", true);
            klub.P�ly�tL�trehoz("M�anyag", false);
            klub.P�ly�tL�trehoz("F�ves", false);

            Assert.ThrowsException<Teniszklub.IllegalTenniscourtType>(() => klub.P�ly�tL�trehoz("Salak", true));

            int num = klub.Teniszp�ly�k.Count;
            Assert.AreEqual(num, 4);

            Assert.AreEqual(klub.Teniszp�ly�k[0].GetSorsz�m(), 1);
            Assert.AreEqual(klub.Teniszp�ly�k[2].GetSorsz�m(), 3);
            Assert.AreEqual(klub.Teniszp�ly�k[1].GetT�pus(), klub.Teniszp�ly�k[2].GetT�pus());
            Assert.AreEqual(klub.Teniszp�ly�k[0].Kisz�mol(klub.Teniszp�ly�k[1].GetIsS�tras()), 3600);
        }

        [TestMethod]
        public void P�ly�T�r�lTest()
        {
            Teniszklub klub = new();
            klub.P�ly�tL�trehoz("Salakos", true);
            klub.P�ly�tL�trehoz("M�anyag", true);
            klub.P�ly�tL�trehoz("M�anyag", false);
            klub.P�ly�tL�trehoz("F�ves", false);

            klub.P�ly�tT�r�l(1);
            int num = klub.Teniszp�ly�k.Count;
            Assert.AreEqual(num, 3);

            Assert.ThrowsException<Teniszklub.TenniscourtNotExist>(() => klub.P�ly�tT�r�l(9));
        }

        [TestMethod]
        public void Bel�p�sKil�pTest()
        {
            Teniszklub klub = new();
            new Szem�ly("Lili�na","01").Bel�p(klub);
            new Szem�ly("Dani","03").Bel�p(klub);
            Assert.ThrowsException<Teniszklub.RegisteredMember>(() => new Szem�ly("Dani", "03").Bel�p(klub));

            new Szem�ly("Lili�na", "01").Kil�p(klub);
            Assert.ThrowsException<Teniszklub.NotRegisteredMember>(() => new Szem�ly("B�la", "03").Kil�p(klub));
        }

        [TestMethod]
        public void P�lyafoglal�sFoglal�sT�r�lTest()
        {
            Teniszklub klub = new();
            klub.P�ly�tL�trehoz("Salakos", true);
            klub.P�ly�tL�trehoz("M�anyag", false);
            klub.P�ly�tL�trehoz("F�ves", false);
            new Szem�ly("Lili�na", "01").Bel�p(klub);

            Assert.ThrowsException<Teniszklub.NotRegisteredMember>(() => new Szem�ly("B�la", "03").P�lyafoglal�s(klub, "Salakos", "2023/08/08.16"));
            Assert.ThrowsException<Teniszklub.NotExistThisTypeOfTenniscourt>(() => new Szem�ly("Lili�na", "01").P�lyafoglal�s(klub, "Gazos", "2023/08/08.16"));
            Assert.ThrowsException<Teniszklub.IllegalReservationTime>(() => new Szem�ly("Lili�na", "01").P�lyafoglal�s(klub, "Salakos", "2023/08/08.22"));

            new Szem�ly("Lili�na", "01").P�lyafoglal�s(klub, "Salakos", "2023/08/08.16");
            Assert.ThrowsException<Teniszklub.NotFoundFreeTenniscourt>(() => new Szem�ly("Lili�na", "01").P�lyafoglal�s(klub, "Salakos", "2023/08/08.16"));
            new Szem�ly("Lili�na", "01").P�lyafoglal�s(klub, "Salakos", "2023/08/08.17");

            //P�lya �s szem�ly t�rl�se aki/ami rendelkezik foglal�ssal:
            Assert.ThrowsException<Teniszklub.ReservationsAreAvailable>(() => klub.P�ly�tT�r�l(1));
            new Szem�ly("Lili�na", "01").Kil�p(klub);
            klub.P�ly�tT�r�l(1);
            int num = klub.Teniszp�ly�k.Count;
            Assert.AreEqual(num, 2);

            //Ha ugyanolyan p�lya l�trehoz akkor arra foglal-e ugyanolyan id�pontot? -Foglal�sok sz�ma check!
            klub.P�ly�tL�trehoz("M�anyag", false);
            new Szem�ly("B�la", "01").Bel�p(klub);
            new Szem�ly("B�la", "01").P�lyafoglal�s(klub, "M�anyag", "2023/08/08.16");
            new Szem�ly("B�la", "01").P�lyafoglal�s(klub, "M�anyag", "2023/08/08.16");
            Assert.ThrowsException<Teniszklub.ReservationsAreAvailable>(() => klub.P�ly�tT�r�l(2));
            Assert.ThrowsException<Teniszklub.ReservationsAreAvailable>(() => klub.P�ly�tT�r�l(4));

            //Els� foglal�s ilyen d�tumra t�rl�se:
            new Szem�ly("B�la", "01").Foglal�sT�rl�s(klub, "2023/08/08");
            klub.P�ly�tT�r�l(2);
            Assert.ThrowsException<Teniszklub.ReservationsAreAvailable>(() => klub.P�ly�tT�r�l(4));
        }

        [TestMethod]
        public void Fizetend��sszeg()
        {
            Teniszklub klub = new();
            klub.P�ly�tL�trehoz("Salakos", true);
            klub.P�ly�tL�trehoz("M�anyag", true);
            klub.P�ly�tL�trehoz("F�ves", false);
            new Szem�ly("Lili�na", "01").Bel�p(klub);
            new Szem�ly("Lili�na", "01").P�lyafoglal�s(klub, "Salakos", "2023/08/08.16");
            new Szem�ly("Lili�na", "01").P�lyafoglal�s(klub, "F�ves", "2023/08/08.16");

            Assert.AreEqual(klub.Fizetend��sszeg(new Szem�ly("Lili�na", "01"), "2023/08/08"), 8600);
            Assert.ThrowsException<Teniszklub.NoReservationsAreAvailable>(() => new Szem�ly("Lili�na", "01").Fizetend�(klub, "2023/08/10"));
        }

        [TestMethod]
        public void Napibev�tel()
        {
            Teniszklub klub = new();
            klub.P�ly�tL�trehoz("Salakos", true);
            klub.P�ly�tL�trehoz("M�anyag", true);

            new Szem�ly("Lili�na", "01").Bel�p(klub);
            new Szem�ly("B�la", "02").Bel�p(klub);

            new Szem�ly("Lili�na", "01").P�lyafoglal�s(klub, "Salakos", "2023/08/08.16");
            new Szem�ly("Lili�na", "01").P�lyafoglal�s(klub, "Salakos", "2023/08/08.17");
            new Szem�ly("Lili�na", "01").P�lyafoglal�s(klub, "M�anyag", "2023/08/08.10");

            Assert.AreEqual(klub.Bev�tel("2023/08/08"), 9600);
        }

    }
}