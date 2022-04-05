using System;
using System.Collections.Generic;
using System.Text;

namespace lojaServiceContract.DTO
{
    public class ListarTransacoesResponse : BaseResponse
    {
        public class Transacao
        {
            public long Id { get; set; }

            public string TipoDeTransacao { get; set; }

            public string DataFormatada { get; set; }

            public decimal Valor { get; set; }

            public string CPF { get; set; }

            public string NumeroCartao { get; set; }

            public string NomeLoja { get; set; }
        }

        public IEnumerable<Transacao> Transacoes { get; set; }

        public decimal Saldo { get; set; }
    }
}
