using Microsoft.VisualStudio.TestTools.UnitTesting;
using Klub;
using System.Collections.Generic;

namespace TesztTeniszklub
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PályátLétehozTest()
        {
            Teniszklub klub = new();
            klub.PályátLétrehoz("Salakos", true);

            Assert.IsNotNull(klub.Teniszpályák);
            klub.PályátLétrehoz("Mûanyag", true);
            klub.PályátLétrehoz("Mûanyag", false);
            klub.PályátLétrehoz("Füves", false);

            Assert.ThrowsException<Teniszklub.IllegalTenniscourtType>(() => klub.PályátLétrehoz("Salak", true));

            int num = klub.Teniszpályák.Count;
            Assert.AreEqual(num, 4);

            Assert.AreEqual(klub.Teniszpályák[0].GetSorszám(), 1);
            Assert.AreEqual(klub.Teniszpályák[2].GetSorszám(), 3);
            Assert.AreEqual(klub.Teniszpályák[1].GetTípus(), klub.Teniszpályák[2].GetTípus());
            Assert.AreEqual(klub.Teniszpályák[0].Kiszámol(klub.Teniszpályák[1].GetIsSátras()), 3600);
        }

        [TestMethod]
        public void PályáTörölTest()
        {
            Teniszklub klub = new();
            klub.PályátLétrehoz("Salakos", true);
            klub.PályátLétrehoz("Mûanyag", true);
            klub.PályátLétrehoz("Mûanyag", false);
            klub.PályátLétrehoz("Füves", false);

            klub.PályátTöröl(1);
            int num = klub.Teniszpályák.Count;
            Assert.AreEqual(num, 3);

            Assert.ThrowsException<Teniszklub.TenniscourtNotExist>(() => klub.PályátTöröl(9));
        }

        [TestMethod]
        public void BelépÉsKilépTest()
        {
            Teniszklub klub = new();
            new Személy("Liliána","01").Belép(klub);
            new Személy("Dani","03").Belép(klub);
            Assert.ThrowsException<Teniszklub.RegisteredMember>(() => new Személy("Dani", "03").Belép(klub));

            new Személy("Liliána", "01").Kilép(klub);
            Assert.ThrowsException<Teniszklub.NotRegisteredMember>(() => new Személy("Béla", "03").Kilép(klub));
        }

        [TestMethod]
        public void PályafoglalÉsFoglalásTörölTest()
        {
            Teniszklub klub = new();
            klub.PályátLétrehoz("Salakos", true);
            klub.PályátLétrehoz("Mûanyag", false);
            klub.PályátLétrehoz("Füves", false);
            new Személy("Liliána", "01").Belép(klub);

            Assert.ThrowsException<Teniszklub.NotRegisteredMember>(() => new Személy("Béla", "03").Pályafoglalás(klub, "Salakos", "2023/08/08.16"));
            Assert.ThrowsException<Teniszklub.NotExistThisTypeOfTenniscourt>(() => new Személy("Liliána", "01").Pályafoglalás(klub, "Gazos", "2023/08/08.16"));
            Assert.ThrowsException<Teniszklub.IllegalReservationTime>(() => new Személy("Liliána", "01").Pályafoglalás(klub, "Salakos", "2023/08/08.22"));

            new Személy("Liliána", "01").Pályafoglalás(klub, "Salakos", "2023/08/08.16");
            Assert.ThrowsException<Teniszklub.NotFoundFreeTenniscourt>(() => new Személy("Liliána", "01").Pályafoglalás(klub, "Salakos", "2023/08/08.16"));
            new Személy("Liliána", "01").Pályafoglalás(klub, "Salakos", "2023/08/08.17");

            //Pálya és személy törlése aki/ami rendelkezik foglalással:
            Assert.ThrowsException<Teniszklub.ReservationsAreAvailable>(() => klub.PályátTöröl(1));
            new Személy("Liliána", "01").Kilép(klub);
            klub.PályátTöröl(1);
            int num = klub.Teniszpályák.Count;
            Assert.AreEqual(num, 2);

            //Ha ugyanolyan pálya létrehoz akkor arra foglal-e ugyanolyan idõpontot? -Foglalások száma check!
            klub.PályátLétrehoz("Mûanyag", false);
            new Személy("Béla", "01").Belép(klub);
            new Személy("Béla", "01").Pályafoglalás(klub, "Mûanyag", "2023/08/08.16");
            new Személy("Béla", "01").Pályafoglalás(klub, "Mûanyag", "2023/08/08.16");
            Assert.ThrowsException<Teniszklub.ReservationsAreAvailable>(() => klub.PályátTöröl(2));
            Assert.ThrowsException<Teniszklub.ReservationsAreAvailable>(() => klub.PályátTöröl(4));

            //Elsõ foglalás ilyen dátumra törlése:
            new Személy("Béla", "01").FoglalásTörlés(klub, "2023/08/08");
            klub.PályátTöröl(2);
            Assert.ThrowsException<Teniszklub.ReservationsAreAvailable>(() => klub.PályátTöröl(4));
        }

        [TestMethod]
        public void Fizetendõösszeg()
        {
            Teniszklub klub = new();
            klub.PályátLétrehoz("Salakos", true);
            klub.PályátLétrehoz("Mûanyag", true);
            klub.PályátLétrehoz("Füves", false);
            new Személy("Liliána", "01").Belép(klub);
            new Személy("Liliána", "01").Pályafoglalás(klub, "Salakos", "2023/08/08.16");
            new Személy("Liliána", "01").Pályafoglalás(klub, "Füves", "2023/08/08.16");

            Assert.AreEqual(klub.Fizetendõösszeg(new Személy("Liliána", "01"), "2023/08/08"), 8600);
            Assert.ThrowsException<Teniszklub.NoReservationsAreAvailable>(() => new Személy("Liliána", "01").Fizetendõ(klub, "2023/08/10"));
        }

        [TestMethod]
        public void Napibevétel()
        {
            Teniszklub klub = new();
            klub.PályátLétrehoz("Salakos", true);
            klub.PályátLétrehoz("Mûanyag", true);

            new Személy("Liliána", "01").Belép(klub);
            new Személy("Béla", "02").Belép(klub);

            new Személy("Liliána", "01").Pályafoglalás(klub, "Salakos", "2023/08/08.16");
            new Személy("Liliána", "01").Pályafoglalás(klub, "Salakos", "2023/08/08.17");
            new Személy("Liliána", "01").Pályafoglalás(klub, "Mûanyag", "2023/08/08.10");

            Assert.AreEqual(klub.Bevétel("2023/08/08"), 9600);
        }

    }
}