using tabuleiro;
using System.Collections.Generic;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro Tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada;
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;
        public bool Xeque { get; private set; }
        public Peca VulneravelEnPassant { get; private set; }

        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            Xeque = false;
            VulneravelEnPassant = null;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            colocarPecas();
        }

        public Peca executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.retirarPeca(origem);
            p.incrementarQteMovimentos();
            Peca pecaCapturada = Tab.retirarPeca(destino);
            Tab.colocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }

            // # JOGADA ESPECIAL - ROQUE PEQUENO
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tab.retirarPeca(origemTorre);
                T.incrementarQteMovimentos();
                Tab.colocarPeca(T, destinoTorre);
            }

            // # JOGADA ESPECIAL - ROQUE GRANDE
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.retirarPeca(origemTorre);
                T.incrementarQteMovimentos();
                Tab.colocarPeca(T, destinoTorre);
            }

            // # JOGADAS ESPECIAL - EN PASSANT
            if(p is Peao)
            {
                if(origem.Coluna != destino.Coluna && pecaCapturada == null)
                {
                    Posicao posPeao;

                    if(p.Cor == Cor.Branca)
                    {
                        posPeao = new Posicao(destino.Linha + 1, destino.Coluna);
                    }
                    else
                    {
                        posPeao = new Posicao(destino.Linha - 1, destino.Coluna);
                    }

                    pecaCapturada = Tab.retirarPeca(posPeao);
                    capturadas.Add(pecaCapturada);
                }
            }

            return pecaCapturada;
        }

        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = Tab.retirarPeca(destino);
            p.decrementarMovimentos();
            if (pecaCapturada != null)
            {
                Tab.colocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            Tab.colocarPeca(p, origem);

            // # JOGADA ESPECIAL - ROQUE PEQUENO
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tab.retirarPeca(destinoTorre);
                T.decrementarMovimentos();
                Tab.colocarPeca(T, origemTorre);
            }

            // # JOGADA ESPECIAL - ROQUE GRANDE
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.retirarPeca(destinoTorre);
                T.decrementarMovimentos();
                Tab.colocarPeca(T, origemTorre);
            }

            // # JOGADA ESPECIAL - EN PASSANT
            if(p is Peao)
            {
                if(origem.Coluna != destino.Coluna && pecaCapturada == VulneravelEnPassant)
                {
                    Peca peao = Tab.retirarPeca(destino);
                    Posicao posPeao;
                    if(p.Cor == Cor.Branca)
                    {
                        posPeao = new Posicao(3, destino.Coluna);
                    }
                    else
                    {
                        posPeao = new Posicao(4, destino.Coluna);
                    }

                    Tab.colocarPeca(peao, posPeao);
                }
            }
        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = executaMovimento(origem, destino);
            if (estaEmXeque(JogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em Xeque!");
            }

            Peca p = Tab.peca(destino);

            // # JOGADA ESPECIAL - PROMOÇÃO
            if(p is Peao)
            {
                if((p.Cor == Cor.Branca && destino.Linha == 0) || (p.Cor == Cor.Preta && destino.Linha == 7))
                {
                    p = Tab.retirarPeca(destino);
                    pecas.Remove(p);
                    Peca dama = new Dama(p.Cor, Tab);
                    Tab.colocarPeca(dama, destino);
                    pecas.Add(dama);
                }
            }



            if (estaEmXeque(adversaria(JogadorAtual)))
            {
                Xeque = true;
            }
            else
            {
                Xeque = false;
            }
            if (testeXequemate(adversaria(JogadorAtual)))
            {
                Terminada = true;
            }
            else
            {
                Turno++;
                mudaJogador();
            }        

            // # JOGADA ESPECIAL - EN PASSANT
            if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2))
            {
                VulneravelEnPassant = p;
            }

        }

        public void validarPosicaoOrigem(Posicao pos)
        {
            if (Tab.peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peca na posição de origem escolhida!");
            }
            if (JogadorAtual != Tab.peca(pos).Cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!Tab.peca(pos).existeMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possiveis para a peça de origem escolhida!");
            }
        }

        public void validarPosicaoDestino(Posicao origem, Posicao destino)
        {
            if (!Tab.peca(origem).movimentoPossivel(destino))
            {
                throw new TabuleiroException("Posição de destido inválida!");
            }
        }

        private void mudaJogador()
        {
            if (JogadorAtual == Cor.Branca)
            {
                JogadorAtual = Cor.Preta;
            }
            else
            {
                JogadorAtual = Cor.Branca;
            }
        }

        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in capturadas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }

            return aux;
        }

        public HashSet<Peca> pecasJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }

        private Cor adversaria(Cor cor)
        {
            if (cor == Cor.Branca)
            {
                return Cor.Preta;
            }
            else
            {
                return Cor.Branca;
            }
        }

        private Peca rei(Cor cor)
        {
            foreach (Peca x in pecasJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }

            return null;
        }

        public bool estaEmXeque(Cor cor)
        {
            Peca aux = rei(cor);
            Peca R = aux;
            if (R == null)
            {
                throw new TabuleiroException("Não existe rei da cor " + cor + " no tabuleiro!");
            }

            foreach (Peca x in pecasJogo(adversaria(cor)))
            {
                bool[,] mat = x.movimentosPossiveis();
                if (mat[R.Posicao.Linha, R.Posicao.Coluna])
                {
                    return true;
                }
            }

            return false;
        }

        public bool testeXequemate(Cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }

            foreach (Peca x in pecasJogo(cor))
            {
                bool[,] mat = x.movimentosPossiveis();
                for (int i = 0; i < Tab.Linhas; i++)
                {
                    for (int j = 0; j < Tab.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(origem, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        private void colocarPecas()
        {
            colocarNovaPeca('a', 1, new Torre(Cor.Branca, Tab));
            colocarNovaPeca('b', 1, new Cavalo(Cor.Branca, Tab));
            colocarNovaPeca('c', 1, new Bispo(Cor.Branca, Tab));
            colocarNovaPeca('d', 1, new Dama(Cor.Branca, Tab));
            colocarNovaPeca('e', 1, new Rei(Cor.Branca, Tab, this));
            colocarNovaPeca('f', 1, new Bispo(Cor.Branca, Tab));
            colocarNovaPeca('g', 1, new Cavalo(Cor.Branca, Tab));
            colocarNovaPeca('h', 1, new Torre(Cor.Branca, Tab));
            colocarNovaPeca('a', 2, new Peao(Cor.Branca, Tab, this));
            colocarNovaPeca('b', 2, new Peao(Cor.Branca, Tab, this));
            colocarNovaPeca('c', 2, new Peao(Cor.Branca, Tab, this));
            colocarNovaPeca('d', 2, new Peao(Cor.Branca, Tab, this));
            colocarNovaPeca('e', 2, new Peao(Cor.Branca, Tab, this));
            colocarNovaPeca('f', 2, new Peao(Cor.Branca, Tab, this));
            colocarNovaPeca('g', 2, new Peao(Cor.Branca, Tab, this));
            colocarNovaPeca('h', 2, new Peao(Cor.Branca, Tab, this));

            colocarNovaPeca('a', 8, new Torre(Cor.Preta, Tab));
            colocarNovaPeca('b', 8, new Cavalo(Cor.Preta, Tab));
            colocarNovaPeca('c', 8, new Bispo(Cor.Preta, Tab));
            colocarNovaPeca('d', 8, new Dama(Cor.Preta, Tab));
            colocarNovaPeca('e', 8, new Rei(Cor.Preta, Tab, this));
            colocarNovaPeca('f', 8, new Bispo(Cor.Preta, Tab));
            colocarNovaPeca('g', 8, new Cavalo(Cor.Preta, Tab));
            colocarNovaPeca('h', 8, new Torre(Cor.Preta, Tab));
            colocarNovaPeca('a', 7, new Peao(Cor.Preta, Tab, this));
            colocarNovaPeca('b', 7, new Peao(Cor.Preta, Tab, this));
            colocarNovaPeca('c', 7, new Peao(Cor.Preta, Tab, this));
            colocarNovaPeca('d', 7, new Peao(Cor.Preta, Tab, this));
            colocarNovaPeca('e', 7, new Peao(Cor.Preta, Tab, this));
            colocarNovaPeca('f', 7, new Peao(Cor.Preta, Tab, this));
            colocarNovaPeca('g', 7, new Peao(Cor.Preta, Tab, this));
            colocarNovaPeca('h', 7, new Peao(Cor.Preta, Tab, this));
        }
    }
}
