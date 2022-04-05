using lojaService;
using lojaServiceContract;
using lojaServiceContract.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace netcorewebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LojaController : ControllerBase
    {
        private ILojaService LojaService { get; set; }
        public static IWebHostEnvironment _environment;


        public LojaController(ILojaService lojaService, IWebHostEnvironment environment)
        {
            LojaService = lojaService;
            _environment = environment;
        }

        [HttpPost("upload")]
        public async Task<string> EnviaArquivo([FromForm] IFormFile arquivo)
        {
            if (arquivo.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\imagens\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\imagens\\");
                    }
                    using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\imagens\\" + arquivo.FileName))
                    {
                        await arquivo.CopyToAsync(filestream);
                        filestream.Flush();
                        return "\\imagens\\" + arquivo.FileName;
                    }
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            else
            {
                return "Ocorreu uma falha no envio do arquivo...";
            }
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public ListarLojasResponse Get()
        {
            return LojaService.ListarLojas(new lojaServiceContract.DTO.ListarLojasRequest());
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
