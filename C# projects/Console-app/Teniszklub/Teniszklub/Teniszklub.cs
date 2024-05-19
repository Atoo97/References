namespace Klub
{
    public class Teniszklub
    {
        public class IllegalTenniscourtType : Exception { public IllegalTenniscourtType(string exception) : base(exception) { } }
        public class ReservationsAreAvailable : Exception { public ReservationsAreAvailable(string exception) : base(exception) { } }
        public class TenniscourtNotExist : Exception { public TenniscourtNotExist(string exception) : base(exception) { } }
        public class RegisteredMember : Exception { public RegisteredMember(string exception) : base(exception) { } }
        public class NotRegisteredMember : Exception { public NotRegisteredMember(string exception) : base(exception) { } }
        public class IllegalReservationTime : Exception { public IllegalReservationTime(string exception) : base(exception) { } }
        public class NotExistThisTypeOfTenniscourt : Exception { public NotExistThisTypeOfTenniscourt(string exception) : base(exception) { } }
        public class NotFoundFreeTenniscourt : Exception { public NotFoundFreeTenniscourt(string exception) : base(exception) { } }
        public class NoReservationsAreAvailable : Exception { public NoReservationsAreAvailable(string exception) : base(exception) { } }




        private readonly List<Személy> Tagok = new();
        public readonly List<Teniszpálya> Teniszpályák = new();
        int counter = 0;
        private List<Foglalás> Foglalások = new(); 


        public void PályátLétrehoz(string típus, bool fedett)
        {
            if (típus == "Salakos" || típus == "Műanyag" || típus == "Füves") { Teniszpályák.Add(new Teniszpálya(típus, fedett, ++counter)); }
            else { throw new IllegalTenniscourtType("Nem hozható létre ilyen pályatípus: " + típus); }
        }

        public void PályátTöröl(int sorszám)
        {
            bool létezik = false;
            //Megviszgál létezik-e ilyen pálya és ha igen, van-e aktív foglalás rá. Ha van nem törölhető!
            foreach (Teniszpálya p in Teniszpályák)
            {
                if (p.GetSorszám() == sorszám)
                {
                    létezik = true;
                    if (!VannakFoglalások(sorszám)) { Teniszpályák.Remove(p); }
                    else { throw new ReservationsAreAvailable("Pályához kötött foglalások találhatók. Az " + sorszám + " számú pálya törlése nem lehetséges!"); }
                    break;
                }
            }
            if (létezik == false) throw new TenniscourtNotExist("Nem található az adott " + sorszám + " sorszámú teniszpálya a nyilvántartásban!");
        }

        private bool VannakFoglalások(int sorszám)
        {
            foreach (Foglalás f in Foglalások)
            {
                if (f.GetPályasorszám() == sorszám) { return true; }
            }
            return false;
        }

        public void Regisztrál(Személy person)
        {
            if (!Tag_e(person.név, person.GetId(), out Személy? személy))
            {
                Tagok.Add(person);
            }
            else { throw new RegisteredMember("A " + személy.név + " nevű és " + személy.GetId() + " azonsoítójú személy már tagja a teniszklubbnak!"); }
        }

        public void KiRegisztrál(Személy person)
        {
            if (Tag_e(person.név, person.GetId(), out Személy? személy))
            {
                Tagok.Remove(személy);
                //Személy foglalásainak törlése:
                List<Foglalás> Törölendők = new();
                for (int i = 0; i < Foglalások.Count(); i++) 
                {                  
                    if (Foglalások[i].GetSzemély().GetId() == személy.GetId()) { Törölendők.Add(Foglalások[i]); }
                }
                foreach (Foglalás f in Törölendők) { Foglalások.Remove(f); }
            } else { throw new NotRegisteredMember("A " + person.név + " nevű és " + person.GetId() + " azonsoítójú személy nem tagja a teniszklubbnak!"); }
        }

        private bool Tag_e(string név, string id, out Személy? személy)
        {
            személy = null;
            foreach (Személy sz in Tagok)
            {
                if (sz.név == név && sz.GetId() == id)
                {
                    személy = sz;
                    return true;
                }
            }
            return false;
        }

        public void Lefoglal(Teniszpálya pálya, Személy person, string dátum)
        {
            if (Tag_e(person.név, person.GetId(), out Személy? személy))
            {
                //2023/08/08.16 => [2323/08/08] & [16]:
                string[] tokens = dátum.Split('.');
                if (Convert.ToInt32(tokens[1]) >= 6 && Convert.ToInt32(tokens[1]) < 20)
                {
                    Foglalások.Add(new Foglalás(person, pálya.GetSorszám(), tokens[0], Convert.ToInt32(tokens[1])));
                }
                else { throw new IllegalReservationTime("Ilyen időpontba nem foglalható pálya: " + tokens[1]); }
            }
            else { throw new NotRegisteredMember("A " + person.név + " nevű és " + person.GetId() + " azonsoítójú személy nem tagja a teniszklubbnak ezért nem jogosult foglalni!"); }
        }

        //C részfeladat:
        public bool SzabadPályák(string típus, string dátum, out List<Teniszpálya>? pályák)
        {
            List<Teniszpálya> szabadpályák = new List<Teniszpálya>();

            //2023/08/08.16 => [2323/08/08] & [16]:
            string[] tokens = dátum.Split('.');

            if (típus == "Salakos" || típus == "Műanyag" || típus == "Füves")
            {
                //Kigyüjt az azonos típusú létező pályák a pályák list-be:
                foreach (Teniszpálya t in Teniszpályák)
                {
                    string type = Convert.ToString(t.GetTípus());
                    string[] tokens2 = type.Split('.');             //Get típus

                    if (típus == tokens2[1])
                    {
                        szabadpályák.Add(t);
                        foreach (Foglalás f in Foglalások)
                        {
                            if (f.GetPályasorszám() == t.GetSorszám() && f.GetDátum() == tokens[0] && f.GetIdőpont() == Convert.ToInt32(tokens[1]))
                            {
                                szabadpályák.Remove(t);
                            }
                        }
                    }
                }
            } else { throw new NotExistThisTypeOfTenniscourt("Nem létezik ilyen típusú teniszpálya a klub nyilvántartásában: " + típus); }

            if (szabadpályák.Count() < 1) { throw new NotFoundFreeTenniscourt("Nem található a keresett napon: " + tokens[0] + " " + tokens[1] + " órára szabad " + típus + " típusú teniszpálya!"); }
            else {
                    pályák = szabadpályák;
                    return true; 
            }
        }

        public void Lemond(Foglalás foglalás)
        {
            Foglalások.Remove(foglalás);
        }

        //d részfeladat:
        public bool Pályafoglalások(Személy person, string dátum, out List<Foglalás>? pályafoglalások)
        {
            pályafoglalások = new List<Foglalás>() { };
            if (Tag_e(person.név, person.GetId(), out Személy? személy))
            {
                foreach (Foglalás f in Foglalások)
                {
                    if (f.GetDátum() == dátum && f.GetSzemély().név == person.név && f.GetSzemély().GetId() == person.GetId()) pályafoglalások.Add(f);
                }

                if (pályafoglalások.Count() == 0) { throw new NoReservationsAreAvailable("Ezen a dátumon nem található foglalás: " + dátum); }
                else
                {
                    return true;
                }
            }
            else { throw new NotRegisteredMember("A " + person.név + " nevű és " + person.GetId() + " azonsoítójú személy nem tagja a teniszklubbnak!"); }
        }

        //a részfeledat:
        public int Fizetendőösszeg(Személy person, string dátum)
        {
            int összeg = 0;
            if (Tag_e(person.név, person.GetId(), out Személy? személy))
            {
                if (Pályafoglalások(person, dátum, out List<Foglalás>? pályafoglalások))
                {
                    foreach (Foglalás f in pályafoglalások)
                    {
                        foreach (Teniszpálya t in Teniszpályák)
                        {
                            if (f.GetPályasorszám() == t.GetSorszám())
                            {
                                int díj = t.Kiszámol(t.GetIsSátras());
                                összeg += díj;
                            }
                        }
                    }
                }
            }
            else { throw new NotRegisteredMember("A " + person.név + " nevű és " + person.GetId() + " azonsoítójú személy nem tagja a teniszklubbnak!"); }
            return összeg;
        }

        //b részfeladat:
        public double Bevétel(string nap)
        {
            double összeg = 0;
            foreach (Foglalás f in Foglalások)
            {
                if (f.GetDátum() == nap)
                {
                    foreach (Teniszpálya t in Teniszpályák)
                    {
                        if (f.GetPályasorszám() == t.GetSorszám())
                        {
                            összeg += t.Kiszámol(t.GetIsSátras());
                        }
                    }

                }
            }
            return összeg;
        }

    }
}