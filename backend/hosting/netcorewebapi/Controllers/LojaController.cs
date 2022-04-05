using lojaService;
using lojaServiceContract;
using lojaServiceContract.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace netcorewebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LojaController : Controller
    {
        private ILojaService LojaService { get; set; }
        public static IWebHostEnvironment _environment;


        public LojaController(ILojaService lojaService, IWebHostEnvironment environment)
        {
            LojaService = lojaService;
            _environment = environment;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            var relatorioParser = new List<string>();

            if (file.Length == 0)
                return Json(new
                {
                    Success = false,
                    Message = "Arquivo inexistente."
                });

            if (file.ContentType != "text/plain")
                return Json(new
                {
                    Success = false,
                    Message = "Arquivo inválido."
                });

            using (Stream st = file.OpenReadStream())
            {
                using (StreamReader sr = new StreamReader(st))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var relatorioLinha = await ProcessLine(line);
                        relatorioParser.Add(relatorioLinha);
                    }
                }
            }

            return Json(new
            {
                Success = true,
                relatorioParser
            });
        }

        private async Task<string> ProcessLine(string line)
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

                var response = await LojaService.ProcessarTransacao(new ProcessarTransacaoRequest()
                {
                    TipoDeTransacao = int.Parse(String.Concat(line.Take(1))),
                    Data = dateTimeUTC,
                    CPF = String.Concat(line.Skip(19).Take(11)),
                    NomeLoja = String.Concat(line.Skip(62).Take(19)),
                    NomeRepresentante = String.Concat(line.Skip(48).Take(14)),
                    NumeroCartao = String.Concat(line.Skip(30).Take(12)),
                    Valor = decimal.Parse(String.Concat(line.Skip(9).Take(10))) / 100,
                });

                if (response.Success)
                    return $"linha processada com sucesso!";
                else
                    return $"Erro no processamento da linha --{line}-- | Error: ${response.Message}";
            }
            catch (Exception ex)
            {
                return $"Erro no processamento da linha --{line}-- | Error: ${ex.Message}";
            }
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ListarLojasResponse> Get()
        {
            return await LojaService.ListarLojas(new lojaServiceContract.DTO.ListarLojasRequest());
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
