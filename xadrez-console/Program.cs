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
                PartidaDeXadrez partida = new PartidaDeXadrez();

                while (!partida.Terminada)
                {
                    Console.Clear();
                    Tela.imprimirTabulerio(partida.Tab);

                    Console.Write("Origem: ");
                    Posicao origem = Tela.lerPosicaoXadrez().toPosicao();

                    bool[,] posicoesPossiveis = partida.Tab.peca(origem).movimentosPossiveis();

                    Console.Clear();
                    Tela.imprimirTabulerio(partida.Tab, posicoesPossiveis);

                    Console.Write("Destido: ");
                    Posicao destino = Tela.lerPosicaoXadrez().toPosicao();

                    partida.executaMovimento(origem, destino);
                }
            }
            catch(TabuleiroException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
            }
            
            
        }
    }
}
