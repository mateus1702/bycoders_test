using lojaServiceContract;
using lojaServiceContract.DTO;
using System;
using System.Linq;

namespace lojaService
{
    public class LojaService : ILojaService
    {
        private LojaContext DbContext { get; set; }

        public LojaService(LojaContext dbContext)
        {
            DbContext = dbContext;
        }

        public AdicionarLojaResponse AdicionarLoja(AdicionarLojaRequest request)
        {


            return null;
        }

        public ListarLojasResponse ListarLojas(ListarLojasRequest request)
        {
            var response = new ListarLojasResponse()
            {
                Success = true
            };

            response.Lojas = DbContext.Lojas.ToList().Select(x => new ListarLojasResponse.Loja()
            {
                Id = x.Id,
                Nome = x.Nome
            });

            return response;
        }
    }
}
