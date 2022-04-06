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

            var fileLines = new List<string>();

            using (Stream st = file.OpenReadStream())
            {
                using (StreamReader sr = new StreamReader(st))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        fileLines.Add(line);

                    }
                }
            }

            var response = await LojaService.ProcessarTransacoes(new ProcessarTransacoesRequest()
            {
                TransacaoLinha = fileLines
            });


            return Json(response);
        }

    

        [HttpGet]
        public async Task<ListarLojasResponse> Get()
        {
            return await LojaService.ListarLojas(new lojaServiceContract.DTO.ListarLojasRequest());
        }

        [HttpPost("listar_transacoes")]
        public async Task<ListarTransacoesResponse> ListarTransacoes(ListarTransacoesRequest request)
        {
            return await LojaService.ListarTransacoes(request);
        }

    }
}
