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
    public class EnderecoController : ControllerBase
    {
        private readonly ISuculentasRepository _repo;
        private readonly IMapper _mapper;

        public EnderecoController(ISuculentasRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var enderecos = await _repo.GetAllEnderecoById(Id);

                var results = _mapper.Map<EnderecoDto>(enderecos);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("getByUserId/{UserId}")]
        public async Task<IActionResult> GetByUserId(int UserId)
        {
            try
            {
                var enderecos = await _repo.GetAllEnderecoByUserId(UserId);

                var results = _mapper.Map<EnderecoDto[]>(enderecos);

                return Ok(results);
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EnderecoDto model)
        {
            try
            {
                var endereco = _mapper.Map<Endereco>(model);

                _repo.Add(endereco);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/endereco/{model.Id}",  _mapper.Map<EnderecoDto>(endereco));
                }
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(int Id, EnderecoDto model)
        {
            try
            {
                var endereco = await _repo.GetAllEnderecoById(Id);

                if(endereco == null) 
                {
                    return NotFound();
                }

                _mapper.Map(model, endereco);

                _repo.Update(endereco);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/endereco/{model.Id}",  _mapper.Map<EnderecoDto>(endereco));
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
                var endereco = await _repo.GetAllEnderecoById(Id);

                if(endereco == null) 
                {
                    return NotFound();
                }
                _repo.Delete(endereco);

                if(await _repo.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (System.Exception ex) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }
    }
}