using lojaServiceContract.DTO;
using System;
using System.Threading.Tasks;

namespace lojaServiceContract
{
    public interface ILojaService
    {
        public Task<ListarLojasResponse> ListarLojas(ListarLojasRequest request);

        public Task<ProcessarTransacaoResponse> ProcessarTransacao(ProcessarTransacaoRequest request);
    }
}
