using System;
using System.Collections.Generic;
using System.Text;

namespace lojaServiceContract.DTO
{
    public class ProcessarTransacoesResponse : BaseResponse
    {
        public List<string> RelatorioParser { get; set; }
    }
}
