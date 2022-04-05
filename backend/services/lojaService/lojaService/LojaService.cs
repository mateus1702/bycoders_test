using lojaService.Models;
using lojaServiceContract;
using lojaServiceContract.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace lojaService
{
    public class LojaService : ILojaService
    {
        private LojaContext DbContext { get; set; }

        public LojaService(LojaContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<ListarLojasResponse> ListarLojas(ListarLojasRequest request)
        {
            var response = new ListarLojasResponse()
            {
                Success = true
            };

            var lojas = await DbContext.Lojas.ToListAsync();

            response.Lojas = lojas.Select(x => new ListarLojasResponse.Loja()
            {
                Id = x.Id,
                Nome = x.Nome
            });

            return response;
        }

        public async Task<ProcessarTransacaoResponse> ProcessarTransacao(ProcessarTransacaoRequest request)
        {
            try
            {
                var message = ValidarTransacao(request);
                if (message != string.Empty)
                    return new ProcessarTransacaoResponse()
                    {
                        Success = false,
                        Message = message
                    };

                var tipoDeTransacao = await DbContext.TiposDeTransacao.FirstAsync(x => x.Codigo == request.TipoDeTransacao);

                Cliente cliente = await DbContext.Clientes.FirstOrDefaultAsync(x => x.CPF == request.CPF);
                if (cliente == null)
                {
                    cliente = new Cliente()
                    {
                        CPF = request.CPF
                    };
                }

                Loja loja = await DbContext.Lojas.FirstOrDefaultAsync(x => x.Nome.ToLower().Trim() == request.NomeLoja.ToLower().Trim());
                if (loja == null)
                {
                    loja = new Loja()
                    {
                        Nome = request.NomeLoja
                    };
                }

                loja.NomeRepresentante = request.NomeRepresentante;

                var transacao = new Transacao()
                {
                    Cliente = cliente,
                    Data = request.Data,
                    Loja = loja,
                    NumeroCartao = request.NumeroCartao,
                    TipoDeTransacao = tipoDeTransacao,
                    Valor = request.Valor
                };

                await DbContext.Transacoes.AddAsync(transacao);


                await DbContext.SaveChangesAsync();

                return new ProcessarTransacaoResponse()
                {
                    Success = true
                };

            }
            catch (Exception ex)
            {
                return new ProcessarTransacaoResponse()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        private string ValidarTransacao(ProcessarTransacaoRequest request)
        {
            if (string.IsNullOrEmpty(request.NomeLoja)
                || string.IsNullOrEmpty(request.NomeRepresentante)
                || string.IsNullOrEmpty(request.CPF)
                || string.IsNullOrEmpty(request.NumeroCartao))
                return "Campos obrigatórios não preenchidos";
            else
                return string.Empty;
        }
    }
}
