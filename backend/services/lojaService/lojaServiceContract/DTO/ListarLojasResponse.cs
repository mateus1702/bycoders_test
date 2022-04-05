using System;
using System.Collections.Generic;
using System.Text;

namespace lojaServiceContract.DTO
{
    public class ListarLojasResponse : BaseResponse
    {
        public class Loja
        {
            public long Id { get; set; }

            public string Nome { get; set; }

            public string NomeRepresentante { get; set; }
        }

        public IEnumerable<Loja> Lojas { get; set; }
    }
}
