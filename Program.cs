using System;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(120, 29);
            Tela t = new Tela(30, 1, 118, 25); // ZoNA JOGAVEL

            geradorPontos gerador = null;

            Tela t2 = new Tela(1, 1, 28, 25); // CAMPO PONTOS
            Tela t3 = new Tela(1, 27, 118, 27); // CAMPO MSG


            Pto[] p = new Pto[5];
            p[0] = new Pto("‡", 45, 15, 1);

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

            int tamanhoCobra = 1; // tamanho da cobra
            while (repete == true)
            {
                if (gerador == null || gerador.getMarca() == false)
                {
                    var sorteio = new geradorPontos(0, 0, true);
                    gerador = new geradorPontos(sorteio.AleatorioX(), sorteio.AleatorioY(), true);
                    g.defineTexto(gerador.getpontoX(), gerador.getpontoY(), "*");
                }

                Tecla = Console.ReadKey(true);

                // memoriza posições anteriores
                for (int x = 0; x < tamanhoCobra; x++)
                {
                    p[x].lembrarPosicao();
                }

                moverCabeça(p[0], Tecla);

                if (!g.validarPto(p[0], t))
                {
                    repete = false;
                    g.defineTexto(60, 27, "GAME OVER");
                    Console.ReadKey();
                    continue;
                }

                // move corpo seguindo a cabeça
                for (int x = 1; x < tamanhoCobra; x++)
                {
                    p[x].setUltimo(p[x].getPX(), p[x].getPY());
                    p[x].setPX(p[x - 1].getultimoX());
                    p[x].setPY(p[x - 1].getultimoY());
                }

                int caudaX = p[tamanhoCobra - 1].getultimoX();
                int caudaY = p[tamanhoCobra - 1].getultimoY();

                // redesenha cobra e limpa cauda antiga
                g.desenharCobra(p, tamanhoCobra);
                g.limparPosicao(caudaX, caudaY);

                if (g.alimentar(p[0], gerador))
                {
                    tamanhoCobra++;
                    if (tamanhoCobra > p.Length)
                    {
                        Array.Resize(ref p, tamanhoCobra + 5);
                    }

                    p[tamanhoCobra - 1] = new Pto("‡", p[tamanhoCobra - 2].getultimoX(), p[tamanhoCobra - 2].getultimoY(), tamanhoCobra);
                    gerador = new geradorPontos(gerador.AleatorioX(), gerador.AleatorioY(), true);
                    g.defineTexto(gerador.getpontoX(), gerador.getpontoY(), "*");
                }

            }

        }

        static void moverCabeça(Pto cabeca, ConsoleKeyInfo Tecla)
        {
            if (Tecla.Key == ConsoleKey.LeftArrow)
            {
                cabeca.setPX(cabeca.getPX() - cabeca.gettamanhoCobra());

            }
            else if (Tecla.Key == ConsoleKey.RightArrow)
            {
                cabeca.setPX(cabeca.getPX() + cabeca.gettamanhoCobra());

            }
            else if (Tecla.Key == ConsoleKey.UpArrow)
            {
                cabeca.setPY(cabeca.getPY() - cabeca.gettamanhoCobra());

            }
            else if (Tecla.Key == ConsoleKey.DownArrow)
            {
                cabeca.setPY(cabeca.getPY() + cabeca.gettamanhoCobra());

            }
            else
            {
                // mantém posição quando tecla inválida
            }

        }

    }
}

