namespace Klub
{
    public class Foglalás
    {
        private Személy birtokol;
        private int pályasorszám;
        private string dátum;
        private int időpont;

        public Foglalás(Személy birtokol, int pályasorszám, string dátum, int időpont)
        {
            this.birtokol = birtokol;
            this.pályasorszám = pályasorszám;
            this.dátum = dátum;
            this.időpont = időpont;
        }

        //Getter:
        public Személy GetSzemély() { return this.birtokol; }
        public int GetPályasorszám() { return this.pályasorszám; }
        public string GetDátum() { return this.dátum; }
        public int GetIdőpont() { return this.időpont; }
    }
}