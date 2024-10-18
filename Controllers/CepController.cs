using AutoMapper;
using Cep.Data;
using EMixApi.Data.Dtos;
using EMixApi.Data.Models;
using EMixApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace EMixApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CepController : ControllerBase
    {
        private CepDbContext _context { get; set; }
        private IMapper _mapper { get; set; }

        public CepController(CepDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AdicionarCep([FromBody] CreateCepDto cepDto)
        {
            CEP cep = _mapper.Map<CEP>(cepDto);
            _context.Ceps.Add(cep);
            _context.SaveChanges();

            return CreatedAtAction(
                "AdicionarCep",
                new { id = cep.Id },
                cep);
        }

        [HttpGet("{cep}")]
        async public Task<IActionResult> ConsultarCep(string cep)
        {
            try
            {
                var validator = new CepValidator();
                var validated = validator.Validate(cep);

                if (!validated.IsValid)
                {
                    return StatusCode(400, "CEP inválido.");
                }
                string url = String.Format("https://viacep.com.br/ws/{0}/json/", cep);

                var httpClient = new HttpClient();
                var result = await httpClient.GetStringAsync(url);
                dynamic jsonObj = JObject.Parse(result);

                var cepConsultado = new CreateCepDto();
                cepConsultado.Cep = jsonObj.cep;
                if (cepConsultado.Cep.IsNullOrEmpty()) return StatusCode(400, "Inexistente");
                cepConsultado.Cep = cepConsultado.Cep.Replace("-", "");
                cepConsultado.Logradouro = jsonObj.logradouro;
                cepConsultado.Complemento = jsonObj.complemento;
                cepConsultado.Bairro = jsonObj.bairro;
                cepConsultado.Localidade = jsonObj.localidade;
                cepConsultado.Uf = jsonObj.uf;
                cepConsultado.Unidade = (jsonObj.unidade == string.Empty || jsonObj.unidade == null ? 0 : jsonObj.unidade);
                cepConsultado.Ibge = (jsonObj.ibge == string.Empty || jsonObj.unidade == null ? 0 : jsonObj.ibge);
                cepConsultado.Gia = jsonObj.gia;

                if (!_context.Ceps.Where(x => x.Cep == cepConsultado.Cep).Any())
                {
                    CEP cepEntidade = _mapper.Map<CEP>(cepConsultado);
                    _context.Ceps.Add(cepEntidade);
                    _context.SaveChanges();
                }

                return Ok(cepConsultado);

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("logradouro/{logradouro}")]
        public IActionResult ConsultarLogradouro(string logradouro)
        {
            try
            {
                var result = _context.Ceps.Where(x => x.Logradouro.Contains(logradouro)).OrderBy(x => x.Logradouro);

                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("uf/{uf}")]
        public IActionResult VerCepsPorUf(string uf)
        {
            try
            {
                var validator = new UfValidator();
                var validated = validator.Validate(uf);

                if (!validated.IsValid)
                {
                    return StatusCode(400, "UF inválida.");
                }

                var result = _context.Ceps.Where(x => x.Uf == uf).OrderBy(x => x.Cep);

                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}
