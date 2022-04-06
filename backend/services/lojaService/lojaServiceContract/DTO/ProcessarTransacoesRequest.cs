using System;
using System.Collections.Generic;
using System.Text;

namespace lojaServiceContract.DTO
{
    public class ProcessarTransacoesRequest
    {
        public IEnumerable<string> TransacaoLinha { get; set; }
    }
}
