using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Pto
    {
        string c;
        int x, ultimoX;

        int y, ultimoY;
        int tamanhoCobra;

        public Pto(string c, int x, int y,int tamanhoCobra)
        {
            this.c = c;
            this.x = x;
            this.y = y;
            this.tamanhoCobra = tamanhoCobra;
        }
        public string getPC()
        {
            return c;
        }

        public int getPX()
        {
            return x;
        }

        public int getultimoX()
        {
            return ultimoX;
        }



        public int gettamanhoCobra()
        {
            return tamanhoCobra;
        }

        public int getPY()
        {
            return y;
        }
        public int getultimoY()
        {
            return ultimoY;
        }
        public void lembrarPosicao()
        {
            ultimoX = x;
            ultimoY = y;
        }
        public void setPX(int x)
        {
            this.x = x;
        }
        public void setPY(int y)
        {
            this.y = y;
        }

        public void setUltimo(int ultimoX, int ultimoY)
        {
            this.ultimoX = ultimoX;
            this.ultimoY = ultimoY;
        }

        public void settamanhoCobra(int tamanhoCobra)
        {
            this.tamanhoCobra= tamanhoCobra;
        }

    }
}
