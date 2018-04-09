using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(120, 29);
            Tela t = new Tela(30, 1, 118, 25); // ZoNA JOGAVEL



            geradorPontos[] gerador = new geradorPontos[100];




            Tela t2 = new Tela(1, 1, 28, 25); // CAMPO PONTOS
            Tela t3 = new Tela(1, 27, 118, 27); // CAMPO MSG


            Pto[] p = new Pto[5];
            p[0] = new Pto("‡", 45, 15,1);

            Grafico g = new Grafico();

            bool repete = true;

            ConsoleKeyInfo Tecla;

            Console.BackgroundColor = ConsoleColor.DarkYellow;

            g.desenharTela(t);

            g.desenharTela(t2);
            Console.BackgroundColor = ConsoleColor.Gray;

            g.desenharTela(t3);
            Console.ResetColor();


            g.defineTexto(3, 2, "JOGO DA COBRA");
            g.defineTexto(3, 3, "Feito por Marcos");

            



            int i = 1; // tamanho da cobra
            while (repete == true)
            {
                gerador[0] = new geradorPontos(50, 20, true);
                g.defineTexto(50, 20, "*");

                Tecla = Console.ReadKey(true);
                if (g.validarPto(p[0], t))

                {
                    for (int x = 0; x < i; x++)
                    {
                        g.andarPto(p[x], t, Tecla);

                    }

                    g.alimentar(p[0],gerador[0]);
                    if (gerador[0].getMarca() == false)
                    {
                        i++;
                        // p[i] = new Pto("‡", p[i-1].getPX()-1, p[i-1].getPY(), i);
                        p[1] = new Pto("‡", p[0].getPX()-1, p[0].getPX(), i);

                       // p[i].setPX(p[i - 1].getPX() - 1);
                       // p[i].setPY(p[i - 1].getPY());



                    }

                    //MOVIMENTAÇÂO DEMAIS PONTOS SERÁ EXECUTADO NA CLASSE GRAFICO, SERA FEITO UMA VERIFICAÇÂO CONSTANTE DE QUANTIDADE DE PONTOS CONSUMIDOS O PONTO ANTERIOR IRA ACEDER A CORDENADA DO PONTO MOVIMENTAVEL

                }
                else
                {
                    repete = false;
                    g.defineTexto(60, 27, "GAME OVER");

                    Console.ReadKey();

                }

            }

        }

    }
}

        
    
