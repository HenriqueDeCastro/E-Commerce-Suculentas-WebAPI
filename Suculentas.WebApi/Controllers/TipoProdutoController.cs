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
    public class TipoProdutoController : ControllerBase
    {
        private readonly ISuculentasRepository _repo;
        private readonly IMapper _mapper;

        public TipoProdutoController(ISuculentasRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var tipoproduto = await _repo.GetAllTipoProduto();

                var results = _mapper.Map<TipoProdutoDto[]>(tipoproduto);

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
                var tipoproduto = await _repo.GetAllTipoProdutoById(Id);

                var results = _mapper.Map<TipoProdutoDto>(tipoproduto);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("getSemProduto")]
        public async Task<IActionResult> GetSemProduto()
        {
            try
            {
                var tipoproduto = await _repo.GetAllTipoProdutoSemProduto();

                var results = _mapper.Map<TipoProdutoDto[]>(tipoproduto);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(TipoProdutoDto model)
        {
            try
            {
                var tipoproduto = _mapper.Map<TipoProduto>(model);

                _repo.Add(tipoproduto);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/tipoproduto/{model.Id}",  _mapper.Map<TipoProdutoDto>(tipoproduto));
                }
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(int Id, TipoProdutoDto model)
        {
            try
            {
                var tipoproduto = await _repo.GetAllTipoProdutoById(Id);

                if(tipoproduto == null) 
                {
                    return NotFound();
                }

                _mapper.Map(model, tipoproduto);

                _repo.Update(tipoproduto);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/tipoproduto/{model.Id}",  _mapper.Map<TipoProdutoDto>(tipoproduto));
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
                var tipoproduto = await _repo.GetAllTipoProdutoById(Id);

                if(tipoproduto == null) 
                {
                    return NotFound();
                }
                _repo.Delete(tipoproduto);

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