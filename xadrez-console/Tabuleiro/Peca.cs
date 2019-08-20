using System;
using System.Collections.Generic;
using System.Linq;
using tabuleiro;

namespace tabuleiro
{
    class Peca
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
        
        public void incrementarQntDeMovimentos()
        {
            QntMovimento++;
        }
    }
}
