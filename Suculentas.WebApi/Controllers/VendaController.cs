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
    public class VendaController : ControllerBase
    {
        private readonly ISuculentasRepository _repo;
        private readonly IMapper _mapper;

        public VendaController(ISuculentasRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var vendas = await _repo.GetAllVenda();

                var results = _mapper.Map<VendaDto[]>(vendas);

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
                var vendas = await _repo.GetAllVendaById(Id);

                var results = _mapper.Map<VendaDto>(vendas);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }
        
        [HttpGet("getByUser/{UserId}")]
        public async Task<IActionResult> GetByUserId(int UserId)
        {
            try
            {
                var vendas = await _repo.GetAllVendaByUserId(UserId);

                var results = _mapper.Map<VendaDto[]>(vendas);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("getByStatus/{StatusId}")]
        public async Task<IActionResult> GetByStatusId(int StatusId)
        {
            try
            {
                var vendas = await _repo.GetAllVendaByStatusId(StatusId);

                var results = _mapper.Map<VendaDto[]>(vendas);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(VendaDto model)
        {
            try
            {
                var venda = _mapper.Map<Venda>(model);

                _repo.Add(venda);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/venda/{model.Id}",  _mapper.Map<VendaDto>(venda));
                }
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
            return BadRequest();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(int Id, VendaDto model)
        {
            try
            {
                var venda = await _repo.GetAllVendaById(Id);

                if(venda == null) 
                {
                    return NotFound();
                }

                _mapper.Map(model, venda);

                _repo.Update(venda);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/venda/{model.Id}",  _mapper.Map<VendaDto>(venda));
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
                var venda = await _repo.GetAllVendaById(Id);

                if(venda == null) 
                {
                    return NotFound();
                }
                _repo.Delete(venda);

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