using System;

namespace ConsoleApplication2
{
    class geradorPontos
    {
        Random random = new Random();

        int pontoX, pontoY;
        bool marca;

        public geradorPontos(int pontoX, int PontoY, bool marca)
        {
            this.pontoX = pontoX;
            this.pontoY = PontoY;
            this.marca = marca;

        }


       public bool getMarca()
        {

            return marca;
        }
        public int getpontoX()
        {

            return pontoX;
        }
        public int getpontoY()
        {

            return pontoY;
        }

        public void setMarca(bool marca)
        {
            this.marca = marca;
        }


        public int AleatorioX()
        {
            return random.Next(35,90);
        }
        public int AleatorioY()
        {
            return random.Next(1, 20);
        }

    }
  }


    





