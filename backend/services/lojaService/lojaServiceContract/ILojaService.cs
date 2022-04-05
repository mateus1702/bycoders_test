using lojaServiceContract.DTO;
using System;

namespace lojaServiceContract
{
    public interface ILojaService
    {
        public AdicionarLojaResponse AdicionarLoja(AdicionarLojaRequest request);

        public ListarLojasResponse ListarLojas(ListarLojasRequest request);
    }
}
