namespace ConsoleApplication2
{
    class Tela
    {
        int p1x, p1y, p2x, p2y;

        public Tela(int p1x, int p1y, int p2x, int p2y)

        {
            this.p1x = p1x;
            this.p1y = p1y;
            this.p2x = p2x;
            this.p2y = p2y;
        }

        public int getP1X()
        {
            return p1x;
        }
        public int getP1Y()
        {
            return p1y;
        }
        public int getP2X()
        {
            return p2x;
        }
        public int getP2Y()
        {
            return p2y;

        }
    }
}
