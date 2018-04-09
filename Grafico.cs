using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApplication2
{
    class Grafico
    {


        public void desenharTela(Tela t){
            for (int i = t.getP1X(); i < t.getP2X(); i++)
            {
                Console.SetCursorPosition(i, t.getP1Y());
                Console.WriteLine(" ");

                Console.SetCursorPosition(i, t.getP2Y());
                Console.WriteLine(" ");
            }

            for (int i = t.getP1Y(); i <= t.getP2Y(); i++)
            {
                Console.SetCursorPosition(t.getP1X(), i);
                Console.WriteLine(" ");

                Console.SetCursorPosition(t.getP2X(), i);
                Console.WriteLine(" ");
            }
        }


        public bool validarPto(Pto p, Tela t)
        {

            if (p.getPX() > t.getP1X() && p.getPX() < t.getP2X() &&p.getPY() < t.getP2Y() && p.getPY() < t.getP2Y())
            {

                    return true;
                }
                    return false;

        }

        public void alimentar(Pto p,geradorPontos gerador)
        {

            if (p.getPX() == gerador.getpontoX() && p.getPY() == gerador.getpontoY())
            {
                gerador.setMarca(false);
               
                defineTexto(30, 27, " A COBRA SE ALIMENTOU DE UMA VITIMA");

            }




        }




        public void andarPto(Pto p, Tela t, ConsoleKeyInfo Tecla) {

            for (int i = 0; i < p.gettamanhoCobra(); i++) { 

                if (Tecla.Key == ConsoleKey.LeftArrow)
                {
                    p.setPX(p.getPX() - p.gettamanhoCobra());
                    Console.SetCursorPosition(p.getPX(), p.getPY());
                    Console.WriteLine(p.getPC());
                    Console.SetCursorPosition(p.getPX() + p.gettamanhoCobra(), p.getPY());
                    Console.WriteLine(" ");

                }
                else if (Tecla.Key == ConsoleKey.RightArrow)
                {
                    p.setPX(p.getPX() + 1);
                    Console.SetCursorPosition(p.getPX(), p.getPY());
                    Console.WriteLine(p.getPC());
                    Console.SetCursorPosition(p.getPX() - p.gettamanhoCobra(), p.getPY());
                    Console.WriteLine(" ");
                }
                else if (Tecla.Key == ConsoleKey.UpArrow)
                {
                    p.setPY(p.getPY() - p.gettamanhoCobra());
                    Console.SetCursorPosition(p.getPX(), p.getPY());
                    Console.WriteLine(p.getPC());
                    Console.SetCursorPosition(p.getPX(), p.getPY() + p.gettamanhoCobra());
                    Console.WriteLine(" ");
                }
                else if (Tecla.Key == ConsoleKey.DownArrow)
                {
                    p.setPY(p.getPY() + p.gettamanhoCobra());
                    Console.SetCursorPosition(p.getPX(), p.getPY());
                    Console.WriteLine(p.getPC());
                    Console.SetCursorPosition(p.getPX(), p.getPY() - p.gettamanhoCobra());
                    Console.WriteLine(" ");

                }
                else
                {
                    defineTexto(30, 27, " Utilize as SETAS do teclado para movimentar a cobra louca");
                }

        }
        }


        public void defineTexto (int LocalizacaoX, int LocalizacaoY, String Mensagem)
        {
            Console.ResetColor();
            Console.SetCursorPosition(LocalizacaoX,LocalizacaoY);
            Console.WriteLine(Mensagem);


        }


    }

    }



