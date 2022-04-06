using lojaServiceContract.DTO;
using System;
using System.Threading.Tasks;

namespace lojaServiceContract
{
    public interface ILojaService
    {
        public Task<ListarLojasResponse> ListarLojas(ListarLojasRequest request);

        public Task<ProcessarTransacoesResponse> ProcessarTransacoes(ProcessarTransacoesRequest request);

        public Task<ListarTransacoesResponse> ListarTransacoes(ListarTransacoesRequest request);
    }
}
