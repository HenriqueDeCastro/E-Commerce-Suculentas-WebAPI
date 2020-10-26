using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Suculentas.Domain;
using Suculentas.Repository;
using Suculentas.WebApi.Dtos;

namespace Suculentas.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmpresaFreteController : ControllerBase
    {
        private readonly ISuculentasRepository _repo;
        private readonly IMapper _mapper;

        public EmpresaFreteController(ISuculentasRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            try
            {
                var empresas = await _repo.GetAllEmpresaFrete();

                var results = _mapper.Map<EmpresaFreteDto[]>(empresas);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var empresas = await _repo.GetAllEmpresaById(Id);

                var results = _mapper.Map<EmpresaFreteDto>(empresas);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EmpresaFreteDto model)
        {
            try
            {
                var empresa = _mapper.Map<EmpresaFrete>(model);

                _repo.Add(empresa);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/empresa/{model.Id}",  _mapper.Map<EmpresaFreteDto>(empresa));
                }
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(int Id, EmpresaFreteDto model)
        {
            try
            {
                var empresa = await _repo.GetAllEmpresaById(Id);

                if(empresa == null) 
                {
                    return NotFound();
                }

                _mapper.Map(model, empresa);

                _repo.Update(empresa);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/empresa/{model.Id}",  _mapper.Map<EmpresaFreteDto>(empresa));
                }
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var empresa = await _repo.GetAllEmpresaById(Id);

                if(empresa == null) 
                {
                    return NotFound();
                }
                _repo.Delete(empresa);

                if(await _repo.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }
    }
}