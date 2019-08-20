using System;
using tabuleiro;
using xadrez;

namespace xadrez_console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Posicao p = new Posicao(3, 4);

                

                Tabuleiro tab = new Tabuleiro(8, 8);

                tab.colocarPeca(new Rei(Cor.Preta, tab), new Posicao(3, 6));

                tab.colocarPeca(new Torre(Cor.Branca, tab), new Posicao(3, 2));

                Tela.imprimirTabulerio(tab);
            }
            catch(TabuleiroException e)
            {
                Console.WriteLine(e.Message);
            }
            
            
        }
    }
}
