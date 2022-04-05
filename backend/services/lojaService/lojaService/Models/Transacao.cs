using System;
using System.Collections.Generic;
using System.Text;

namespace lojaService.Models
{
    public class Transacao
    {
        public long Id { get; set; }

        public TipoDeTransacao TipoDeTransacao { get; set; }

        public DateTime Data { get; set; }

        public decimal Valor { get; set; }

        public Cliente Cliente { get; set; }

        public string NumeroCartao { get; set; }

        public Loja Loja { get; set; }
    }
}
