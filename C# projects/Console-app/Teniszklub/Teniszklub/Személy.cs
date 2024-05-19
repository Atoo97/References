namespace Klub
{
    public class Személy
    {
        public readonly string név;
        private readonly string Id;

        //Constructor:
        public Személy(string name, string id) { név = name; Id = id; }

        //Getter:
        public string GetId() { return this.Id; }


        //Methods:
        public void Belép(Teniszklub teniszklub)
        {
            teniszklub.Regisztrál(this);
        }

        public void Kilép(Teniszklub teniszklub)
        {
            teniszklub.KiRegisztrál(this);
        }

        public void Pályafoglalás(Teniszklub teniszklub, string típus, string dátum)
        {
            string[] tokens = dátum.Split('.');

            if (teniszklub.SzabadPályák(típus, dátum, out List<Teniszpálya>? szabadpályák))
            {
                //Beégetve, hogy első szabad pályát foglalja le adott dátumra:
                teniszklub.Lefoglal(szabadpályák[0], this, dátum);
            }
        }

        public void FoglalásTörlés(Teniszklub teniszklub, string dátum)
        {
            //Adott dátumu napra kigyűjt foglalt pályák közül az elsőt törli mindig:
            if (teniszklub.Pályafoglalások(this, dátum, out List<Foglalás>? pályafoglalások))
            {
                teniszklub.Lemond(pályafoglalások[0]);
            }
        }

        public void Fizetendő(Teniszklub teniszklub, string dátum)
        {
            Console.WriteLine(this.név + " nevű klubtag " + dátum + " napra fizetendő pályafoglalási díjainak összege: " + teniszklub.Fizetendőösszeg(this, dátum));
        }
    }
}