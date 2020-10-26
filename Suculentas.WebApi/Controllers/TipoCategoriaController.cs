using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Suculentas.Domain;
using Suculentas.Repository;
using Suculentas.WebApi.Dtos;

namespace Suculentas.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TipoCategoriaController : ControllerBase
    {
        private readonly ISuculentasRepository _repo;
        private readonly IMapper _mapper;

        public TipoCategoriaController(ISuculentasRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var tipoCategoria = await _repo.GetAllTipoCategoria();

                var results = _mapper.Map<TipoCategoriaDto[]>(tipoCategoria);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }
        
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var tipoCategoria = await _repo.GetAllTipoCategoriaById(Id);

                var results = _mapper.Map<TipoCategoriaDto>(tipoCategoria);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("getSemCategoria")]
        public async Task<IActionResult> GetSemProduto()
        {
            try
            {
                var tipoCategoria = await _repo.GetAllTipoCategoriaSemCategoria();

                var results = _mapper.Map<TipoCategoriaDto[]>(tipoCategoria);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(TipoCategoriaDto model)
        {
            try
            {
                var tipoCategoria = _mapper.Map<TipoCategoria>(model);

                _repo.Add(tipoCategoria);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/tipocategoria/{model.Id}",  _mapper.Map<TipoCategoriaDto>(tipoCategoria));
                }
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(int Id, TipoCategoriaDto model)
        {
            try
            {
                var tipoCategoria = await _repo.GetAllTipoCategoriaById(Id);

                if(tipoCategoria == null) 
                {
                    return NotFound();
                }

                _mapper.Map(model, tipoCategoria);

                _repo.Update(tipoCategoria);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/tipocategoria/{model.Id}",  _mapper.Map<TipoCategoriaDto>(tipoCategoria));
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
                var tipoCategoria = await _repo.GetAllTipoCategoriaById(Id);

                if(tipoCategoria == null) 
                {
                    return NotFound();
                }
                _repo.Delete(tipoCategoria);

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