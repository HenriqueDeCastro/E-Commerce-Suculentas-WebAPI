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
    public class CategoriaController : ControllerBase
    {
        private readonly ISuculentasRepository _repo;
        private readonly IMapper _mapper;

        public CategoriaController(ISuculentasRepository repo, IMapper mapper)
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
                var categorias = await _repo.GetAllCategorias();

                var results = _mapper.Map<CategoriaDto[]>(categorias);

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
                var categorias = await _repo.GetAllCategoriaById(Id);

                var results = _mapper.Map<CategoriaDto>(categorias);

                return Ok(results);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou: " + e.Message);
            }
        }

        [HttpGet("getCliente/{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdCliente(int Id)
        {
            try
            {
                var categorias = await _repo.GetAllCategoriaByIdCliente(Id);

                var results = _mapper.Map<CategoriaDto>(categorias);

                return Ok(results);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou: " + e.Message);
            }
        }

        [HttpGet("getEmpresa/{Id}")]
        public async Task<IActionResult> GetByIdEmpresa(int Id)
        {
            try
            {
                var categorias = await _repo.GetAllCategoriaByIdEmpresa(Id);

                var results = _mapper.Map<CategoriaDto>(categorias);

                return Ok(results);
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou: " +  e.Message);
            }
        }

        [HttpGet("getSemProduto")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSemProduto()
        {
            try
            {
                var categorias = await _repo.GetAllCategoriasSemProdutos();

                var results = _mapper.Map<CategoriaDto[]>(categorias);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("getPagInicialEmpresa")]
        public async Task<IActionResult> GetPagInicialEmpresa()
        {
            try
            {
                var categorias = await _repo.GetAllCategoriasPagInicialEmpresa();

                var results = _mapper.Map<CategoriaDto[]>(categorias);

                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("getPagInicial")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPagInicial()
        {
            try
            {
                var categorias = await _repo.GetAllCategoriasPagInicial();

                var results = _mapper.Map<CategoriaDto[]>(categorias);

                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoriaDto model)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(model);

                _repo.Add(categoria);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/categoria/{model.Id}",  _mapper.Map<CategoriaDto>(categoria));
                }
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(int Id, CategoriaDto model)
        {
            try
            {
                var categoria = await _repo.GetAllCategoriaById(Id);

                if(categoria == null) 
                {
                    return NotFound();
                }

                _mapper.Map(model, categoria);

                _repo.Update(categoria);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/categoria/{model.Id}",  _mapper.Map<CategoriaDto>(categoria));
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
                var categoria = await _repo.GetAllCategoriaById(Id);

                if(categoria == null) 
                {
                    return NotFound();
                }
                _repo.Delete(categoria);

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