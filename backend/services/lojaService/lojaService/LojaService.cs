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
            try
            {
                var response = new ListarLojasResponse()
                {
                    Success = true
                };

                var lojas = await DbContext.Lojas.ToListAsync();

                response.Lojas = lojas.Select(x => new ListarLojasResponse.Loja()
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    NomeRepresentante = x.NomeRepresentante
                });

                return response;

            }
            catch (Exception ex)
            {
                return new ListarLojasResponse()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
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

        public async Task<ListarTransacoesResponse> ListarTransacoes(ListarTransacoesRequest request)
        {
            try
            {
                var loja = await DbContext.Lojas.FirstOrDefaultAsync(x => x.Id == request.LojaId);
                if (loja == null)
                {
                    return new ListarTransacoesResponse()
                    {
                        Success = false,
                        Message = "Loja não encontrada."
                    };
                }

                var response = new ListarTransacoesResponse()
                {
                    Success = true
                };

                var transacoes = await DbContext.Transacoes
                    .Include(x => x.Cliente)
                    .Include(x => x.Loja)
                    .Include(x => x.TipoDeTransacao)
                    .Where(x => x.Loja.Id == request.LojaId).ToListAsync();

                response.Transacoes = transacoes.Select(x => new ListarTransacoesResponse.Transacao()
                {
                    Id = x.Id,
                    CPF = x.Cliente.CPF,
                    DataFormatada = x.Data.ToString("yyyy/MM/dd HH:mm"),
                    NomeLoja = x.Loja.Nome,
                    NumeroCartao = x.NumeroCartao,
                    TipoDeTransacao = x.TipoDeTransacao.Descricao,
                    Valor = x.Valor
                });

                response.Saldo = transacoes.Sum((x) =>
                {
                    return x.TipoDeTransacao.Entrada ? x.Valor : x.Valor * -1;
                });

                return response;

            }
            catch (Exception ex)
            {
                return new ListarTransacoesResponse()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
