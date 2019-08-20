using tabuleiro;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro  Tab { get; private set; }
        private int Turno;
        private Cor JogadorAtual;
        public bool Terminada;

        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            colocarPecas();
        }

        public void executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.retirarPeca(origem);
            p.incrementarQntDeMovimentos();
            Peca pecaCapturada = Tab.retirarPeca(destino);
            Tab.colocarPeca(p, destino);
        }

        private void colocarPecas()
        {
            Tab.colocarPeca(new Torre(Cor.Branca, Tab), new PosicaoXadrez('c', 1).toPosicao());
            Tab.colocarPeca(new Torre(Cor.Branca, Tab), new PosicaoXadrez('c', 2).toPosicao());
            Tab.colocarPeca(new Torre(Cor.Branca, Tab), new PosicaoXadrez('d', 2).toPosicao());
            Tab.colocarPeca(new Torre(Cor.Branca, Tab), new PosicaoXadrez('e', 2).toPosicao());
            Tab.colocarPeca(new Torre(Cor.Branca, Tab), new PosicaoXadrez('e', 1).toPosicao());
            Tab.colocarPeca(new Rei(Cor.Branca, Tab), new PosicaoXadrez('d', 1).toPosicao());

            Tab.colocarPeca(new Torre(Cor.Preta, Tab), new PosicaoXadrez('c', 7).toPosicao());
            Tab.colocarPeca(new Torre(Cor.Preta, Tab), new PosicaoXadrez('c', 8).toPosicao());
            Tab.colocarPeca(new Torre(Cor.Preta, Tab), new PosicaoXadrez('d', 7).toPosicao());
            Tab.colocarPeca(new Torre(Cor.Preta, Tab), new PosicaoXadrez('e', 7).toPosicao());
            Tab.colocarPeca(new Torre(Cor.Preta, Tab), new PosicaoXadrez('e', 8).toPosicao());
            Tab.colocarPeca(new Rei(Cor.Preta, Tab), new PosicaoXadrez('d', 8).toPosicao());
        }
    }
}
