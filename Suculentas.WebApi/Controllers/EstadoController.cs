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
    public class EstadoController : ControllerBase
    {
        private readonly ISuculentasRepository _repo;
        private readonly IMapper _mapper;

        public EstadoController(ISuculentasRepository repo, IMapper mapper)
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
                var estados = await _repo.GetAllEstado();

                var results = _mapper.Map<EstadoDto[]>(estados);

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
                var estados = await _repo.GetAllEstadoById(Id);

                var results = _mapper.Map<EstadoDto>(estados);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("getByNome/{Nome}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByNome(string Nome)
        {
            try
            {
                var estados = await _repo.GetAllEstadoByNome(Nome);

                var results = _mapper.Map<EstadoDto>(estados);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("getByUF/{UF}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByUF(string UF)
        {
            try
            {
                var estados = await _repo.GetAllEstadoByUf(UF);

                var results = _mapper.Map<EstadoDto>(estados);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EstadoDto model)
        {
            try
            {
                var estados = _mapper.Map<Estado>(model);

                _repo.Add(estados);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/estado/{model.Id}",  _mapper.Map<EstadoDto>(estados));
                }
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(int Id, EstadoDto model)
        {
            try
            {
                var estados = await _repo.GetAllEstadoById(Id);

                if(estados == null) 
                {
                    return NotFound();
                }

                _mapper.Map(model, estados);

                _repo.Update(estados);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/estado/{model.Id}",  _mapper.Map<EstadoDto>(estados));
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
                var estados = await _repo.GetAllEstadoById(Id);

                if(estados == null) 
                {
                    return NotFound();
                }
                _repo.Delete(estados);

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