using System;
using System.Collections.Generic;
using System.Linq;
using tabuleiro;

namespace tabuleiro
{
    abstract class Peca
    {
        public Posicao Posicao { get; set; }
        public Cor Cor { get; set; }
        public int QntMovimento { get; set; }
        public Tabuleiro Tabuleiro { get; set; }

        public Peca()
        {
        }

        public Peca(Cor cor, Tabuleiro tabuleiro)
        {
            Posicao = null;
            Cor = cor;
            QntMovimento = 0;
            Tabuleiro = tabuleiro;
        }
        public void incrementarQteMovimentos()
        {
            QntMovimento++;
        }

        public bool existeMovimentosPossiveis()
        {
            bool[,] mat = movimentosPossiveis();
            for(int i = 0; i < Tabuleiro.Linhas; i++)
            {
                for(int j = 0; j < Tabuleiro.Colunas; j++)
                {
                    if (mat[i, j])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool podeMoverPara(Posicao pos)
        {
            return movimentosPossiveis()[pos.Linha, pos.Coluna]; //Não entendi direito  oq rolou aqui tirar dúvida com o pessoal do trabalho
        }

        public abstract bool[,] movimentosPossiveis();
    }
}
