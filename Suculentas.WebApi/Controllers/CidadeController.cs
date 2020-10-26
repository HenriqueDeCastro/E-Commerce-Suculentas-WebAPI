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
    public class CidadeController : ControllerBase
    {
        private readonly ISuculentasRepository _repo;
        private readonly IMapper _mapper;

        public CidadeController(ISuculentasRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var cidades = await _repo.GetAllCidadeById(Id);

                var results = _mapper.Map<CidadeDto>(cidades);

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
                var cidades = await _repo.GetAllCidadeByNome(Nome);

                var results = _mapper.Map<CidadeDto>(cidades);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("getByEstadoId/{EstadoId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByEstadoId(int EstadoId)
        {
            try
            {
                var cidades = await _repo.GetAllCidadeByEstadoId(EstadoId);

                var results = _mapper.Map<CidadeDto[]>(cidades);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CidadeDto model)
        {
            try
            {
                var cidade = _mapper.Map<Cidade>(model);

                _repo.Add(cidade);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/cidade/{model.Id}",  _mapper.Map<CidadeDto>(cidade));
                }
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(int Id, CidadeDto model)
        {
            try
            {
                var cidade = await _repo.GetAllCidadeById(Id);

                if(cidade == null) 
                {
                    return NotFound();
                }

                _mapper.Map(model, cidade);

                _repo.Update(cidade);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/cidade/{model.Id}",  _mapper.Map<CidadeDto>(cidade));
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
                var cidade = await _repo.GetAllCidadeById(Id);

                if(cidade == null) 
                {
                    return NotFound();
                }
                _repo.Delete(cidade);

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