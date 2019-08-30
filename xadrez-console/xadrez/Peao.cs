using tabuleiro;

namespace xadrez
{
    class Peao : Peca
    {
        private PartidaDeXadrez Partida;
        public Peao(Cor cor, Tabuleiro tab, PartidaDeXadrez partida) : base(cor, tab)
        {
            Partida = partida;
        }

        public override string ToString()
        {
            return "P";
        }

        private bool existeInimigo(Posicao pos)
        {
            Peca p = Tabuleiro.peca(pos);
            return p != null && p.Cor != Cor; // Tira dúvida com o Fred
        }

        private bool livre(Posicao pos)
        {
            return Tabuleiro.peca(pos) == null; // Tira dúvida com o Fred
        }

        private bool podeMover(Posicao pos)
        {
            Peca p = Tabuleiro.peca(pos);
            return p == null || p.Cor != Cor;
        }

        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];
            Posicao pos = new Posicao(0, 0);

            if (Cor == Cor.Branca)
            {
                pos.definirValores(Posicao.Linha - 1, Posicao.Coluna);
                if (Tabuleiro.posicaoValida(pos) && livre(pos))
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }

                pos.definirValores(Posicao.Linha - 2, Posicao.Coluna);
                Posicao posAux = new Posicao(0, 0);
                posAux.definirValores(Posicao.Linha - 1, Posicao.Coluna);
                if (Tabuleiro.posicaoValida(pos) && livre(posAux) && livre(pos) && QntMovimento == 0)
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }

                pos.definirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
                if (Tabuleiro.posicaoValida(pos) && existeInimigo(pos))
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }
                pos.definirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
                if (Tabuleiro.posicaoValida(pos) && existeInimigo(pos))
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }

                //#JOGADA ESPECIAL - EN PASSANT BRANCOS
                if(Posicao.Linha == 3)
                {
                    Posicao esquerda = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                    if(Tabuleiro.posicaoValida(esquerda) && existeInimigo(esquerda) && Tabuleiro.peca(esquerda) == Partida.VulneravelEnPassant)
                    {
                        mat[esquerda.Linha - 1, esquerda.Coluna] = true;
                    }

                    Posicao diretia = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                    if (Tabuleiro.posicaoValida(diretia) && existeInimigo(diretia) && Tabuleiro.peca(diretia) == Partida.VulneravelEnPassant)
                    {
                        mat[diretia.Linha -1, diretia.Coluna] = true;
                    }
                }
            }
            else
            {
                pos.definirValores(Posicao.Linha + 1, Posicao.Coluna);
                if (Tabuleiro.posicaoValida(pos) && livre(pos))
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }

                pos.definirValores(Posicao.Linha + 2, Posicao.Coluna);
                Posicao posAux = new Posicao(0, 0);
                posAux.definirValores(Posicao.Linha + 1, Posicao.Coluna);
                if (Tabuleiro.posicaoValida(pos) && livre(posAux) && livre(pos) && QntMovimento == 0)
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }

                pos.definirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
                if (Tabuleiro.posicaoValida(pos) && existeInimigo(pos))
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }
                pos.definirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
                if (Tabuleiro.posicaoValida(pos) && existeInimigo(pos))
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }

                //#JOGADA ESPECIAL - EN PASSANT PRETAS  
                if (Posicao.Linha == 4)
                {
                    Posicao esquerda = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                    if (Tabuleiro.posicaoValida(esquerda) && existeInimigo(esquerda) && Tabuleiro.peca(esquerda) == Partida.VulneravelEnPassant)
                    {
                        mat[esquerda.Linha + 1, esquerda.Coluna] = true;
                    }

                    Posicao diretia = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                    if (Tabuleiro.posicaoValida(diretia) && existeInimigo(diretia) && Tabuleiro.peca(diretia) == Partida.VulneravelEnPassant)
                    {
                        mat[diretia.Linha + 1, diretia.Coluna] = true;
                    }
                }
            }

            return mat;
        }
    }
}
