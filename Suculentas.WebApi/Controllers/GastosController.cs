using System;
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
    public class GastosController : ControllerBase
    {
        private readonly ISuculentasRepository _repo;
        private readonly IMapper _mapper;

        public GastosController(ISuculentasRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var gastos = await _repo.GetAllGastos();

                var results = _mapper.Map<GastosDto[]>(gastos);

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
                var gastos = await _repo.GetAllGastosById(Id);

                var results = _mapper.Map<GastosDto>(gastos);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("getByData/{Data}")]
        public async Task<IActionResult> GetByData(string Data)
        {
            try
            {
                DateTime data = Convert.ToDateTime(Data);

                var gastos = await _repo.GetAllGastosByData(data);

                var results = _mapper.Map<GastosDto[]>(gastos);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(GastosDto model)
        {
            try
            {
                var gastos = _mapper.Map<Gastos>(model);

                _repo.Add(gastos);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/gastos/{model.Id}",  _mapper.Map<GastosDto>(gastos));
                }
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(int Id, GastosDto model)
        {
            try
            {
                var gastos = await _repo.GetAllGastosById(Id);

                if(gastos == null) 
                {
                    return NotFound();
                }

                _mapper.Map(model, gastos);

                _repo.Update(gastos);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/gastos/{model.Id}",  _mapper.Map<GastosDto>(gastos));
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
                var gastos = await _repo.GetAllGastosById(Id);

                if(gastos == null) 
                {
                    return NotFound();
                }
                _repo.Delete(gastos);

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