using System;

namespace ConsoleApplication2
{
    class Grafico
    {

        public void desenharTela(Tela t)
        {
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

            if (p.getPX() > t.getP1X() && p.getPX() < t.getP2X() && p.getPY() > t.getP1Y() && p.getPY() < t.getP2Y())
            {

                return true;
            }
            return false;

        }

        public bool alimentar(Pto p, geradorPontos gerador)
        {

            if (p.getPX() == gerador.getpontoX() && p.getPY() == gerador.getpontoY())
            {
                gerador.setMarca(false);

                defineTexto(30, 27, " A COBRA SE ALIMENTOU DE UMA VITIMA");
                return true;

            }

            return false;
        }

        public void desenharCobra(Pto[] cobra, int tamanho)
        {

            for (int i = 0; i < tamanho; i++)
            {

                Console.SetCursorPosition(cobra[i].getPX(), cobra[i].getPY());
                Console.WriteLine(cobra[i].getPC());

            }
        }
        public void limparPosicao(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine(" ");
        }

        public void defineTexto(int LocalizacaoX, int LocalizacaoY, String Mensagem)
        {
            Console.ResetColor();
            Console.SetCursorPosition(LocalizacaoX, LocalizacaoY);
            Console.WriteLine(Mensagem);
        }
    }
}