namespace Klub
{
    public class Teniszpálya
    {
        private int sorszám;
        private Pályatípus típus;
        private readonly bool IsSátras;

        public Teniszpálya(string t, bool l, int counter) 
        {
            sorszám = counter;
            switch (t)
            {
                case "Salakos":
                    this.típus = Salakos.Instance();
                    break;
                case "Füves":
                    this.típus = Füves.Instance();
                    break;
                case "Műanyag":
                    this.típus = Műanyag.Instance();
                    break;
            }

            this.IsSátras = l;
        }

        public int GetSorszám() { return this.sorszám; }
        public bool GetIsSátras() { return this.IsSátras; }
        public Pályatípus GetTípus() { return this.típus; }

        public int Kiszámol(bool IsSátras) 
        {
            int díj;
            if (IsSátras && this.típus.IsSalakos())
            {
                return díj = this.típus.GetNapidíj() + 600;
            }
            else if (!IsSátras && this.típus.IsSalakos())
            {
                return díj = this.típus.GetNapidíj();
            }
            else if (IsSátras && this.típus.IsMűanyag())
            {
                return díj = this.típus.GetNapidíj() + 400;
            }
            else if (!IsSátras && this.típus.IsMűanyag())
            {
                return díj = this.típus.GetNapidíj();
            }
            else if (IsSátras && this.típus.IsFüves())
            {
                return díj = this.típus.GetNapidíj() + 1000;
            }
            else
            {
                return díj = this.típus.GetNapidíj();
            }
        }





    }

}