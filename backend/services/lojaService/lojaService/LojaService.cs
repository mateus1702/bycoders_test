using lojaService.Models;
using lojaServiceContract;
using lojaServiceContract.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public class TransacaoAProcessar
        {
            public int TipoDeTransacao { get; set; }

            public DateTime Data { get; set; }

            public decimal Valor { get; set; }

            public string CPF { get; set; }

            public string NumeroCartao { get; set; }

            public string NomeRepresentante { get; set; }

            public string NomeLoja { get; set; }
        }

        public async Task<ProcessarTransacoesResponse> ProcessarTransacoes(ProcessarTransacoesRequest request)
        {
            try
            {
                var response = new ProcessarTransacoesResponse()
                {
                    Success = true,
                    RelatorioParser = new List<string>()
                };

                foreach (var linha in request.TransacaoLinha)
                {
                    var relatorioLinha = await ProcessarLinha(linha);
                    response.RelatorioParser.Add(relatorioLinha);
                }

                return response;

            }
            catch (Exception ex)
            {
                return new ProcessarTransacoesResponse()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<string> ProcessarLinha(string line)
        {
            try
            {
                var year = int.Parse(String.Concat(line.Skip(1).Take(4)));
                var month = int.Parse(String.Concat(line.Skip(5).Take(2)));
                var day = int.Parse(String.Concat(line.Skip(7).Take(2)));
                var hour = int.Parse(String.Concat(line.Skip(42).Take(2)));
                var minute = int.Parse(String.Concat(line.Skip(44).Take(2)));
                var second = int.Parse(String.Concat(line.Skip(46).Take(2)));

                var dateTimeUTCMinus3 = new DateTime(year, month, day, hour, minute, second);

                var dateTimeUTC = dateTimeUTCMinus3.AddHours(3);

                var transacao = new TransacaoAProcessar()
                {
                    TipoDeTransacao = int.Parse(String.Concat(line.Take(1))),
                    Data = dateTimeUTC,
                    CPF = String.Concat(line.Skip(19).Take(11)),
                    NomeLoja = String.Concat(line.Skip(62).Take(19)),
                    NomeRepresentante = String.Concat(line.Skip(48).Take(14)),
                    NumeroCartao = String.Concat(line.Skip(30).Take(12)),
                    Valor = decimal.Parse(String.Concat(line.Skip(9).Take(10))) / 100,
                };

                var message = ValidarTransacao(transacao);
                if (message != string.Empty)
                    return message;

                var tipoDeTransacao = await DbContext.TiposDeTransacao.FirstAsync(x => x.Codigo == transacao.TipoDeTransacao);

                Cliente cliente = await DbContext.Clientes.FirstOrDefaultAsync(x => x.CPF == transacao.CPF);
                if (cliente == null)
                {
                    cliente = new Cliente()
                    {
                        CPF = transacao.CPF
                    };
                }

                Loja loja = await DbContext.Lojas.FirstOrDefaultAsync(x => x.Nome.ToLower().Trim() == transacao.NomeLoja.ToLower().Trim());
                if (loja == null)
                {
                    loja = new Loja()
                    {
                        Nome = transacao.NomeLoja
                    };
                }

                loja.NomeRepresentante = transacao.NomeRepresentante;

                await DbContext.Transacoes.AddAsync(new Transacao()
                {
                    Cliente = cliente,
                    Data = transacao.Data,
                    Loja = loja,
                    NumeroCartao = transacao.NumeroCartao,
                    TipoDeTransacao = tipoDeTransacao,
                    Valor = transacao.Valor
                });


                await DbContext.SaveChangesAsync();

                return $"linha processada com sucesso!";

            }
            catch (Exception ex)
            {
                return $"Erro no processamento da linha --{line}-- | Error: ${ex.Message}";
            }
        }

        private string ValidarTransacao(TransacaoAProcessar request)
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
