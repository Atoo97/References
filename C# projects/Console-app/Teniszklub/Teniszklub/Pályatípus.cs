namespace Klub
{
    abstract public class Pályatípus
    {

        public virtual int GetNapidíj() { return 0; }
        public virtual bool IsSalakos() { return false; }
        public virtual bool IsFüves() { return false; }
        public virtual bool IsMűanyag() { return false; }
    }

    class Salakos : Pályatípus 
    {
        private static Salakos? instance = null;
        private Salakos() { }

        public static Salakos Instance()
        {
            if (instance == null)
            {
                instance = new Salakos();
            }
            return instance;
        }

        public override int GetNapidíj() { return 3000; }
        public override bool IsSalakos() { return true; }

    }

    class Füves : Pályatípus
    {
        private static Füves? instance = null;
        private Füves() { }

        public static Füves Instance()
        {
            if (instance == null)
            {
                instance = new Füves();
            }
            return instance;
        }

        public override int GetNapidíj() { return 5000; }
        public override bool IsFüves() { return true; }

    }

    class Műanyag : Pályatípus
    {
        private static Műanyag? instance = null;
        private Műanyag() { }

        public static Műanyag Instance()
        {
            if (instance == null)
            {
                instance = new Műanyag();
            }
            return instance;
        }

        public override int GetNapidíj() { return 2000; }
        public override bool IsMűanyag() { return true; }

    }

}