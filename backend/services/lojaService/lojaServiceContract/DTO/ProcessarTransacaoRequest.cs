using System;
using System.Collections.Generic;
using System.Text;

namespace lojaServiceContract.DTO
{
    public class ProcessarTransacaoRequest 
    {
        public int TipoDeTransacao { get; set; }

        public DateTime Data { get; set; }

        public decimal Valor { get; set; }

        public string CPF { get; set; }

        public string NumeroCartao { get; set; }

        public string NomeRepresentante { get; set; }

        public string NomeLoja { get; set; }
    }
}
