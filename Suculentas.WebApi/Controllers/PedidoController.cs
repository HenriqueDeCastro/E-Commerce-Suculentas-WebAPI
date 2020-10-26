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
    public class PedidoController : ControllerBase
    {
        private readonly ISuculentasRepository _repo;
        private readonly IMapper _mapper;

        public PedidoController(ISuculentasRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var pedido = await _repo.GetAllPedidoById(Id);

                var results = _mapper.Map<PedidoDto>(pedido);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("getByVenda/{VendaId}")]
        public async Task<IActionResult> GetByVendaId(int VendaId)
        {
            try
            {
                var pedido = await _repo.GetAllPedidoByVendaId(VendaId);

                var results = _mapper.Map<PedidoDto[]>(pedido);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(PedidoDto model)
        {
            try
            {
                var pedido = _mapper.Map<Pedido>(model);

                _repo.Add(pedido);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/pedido/{model.Id}",  _mapper.Map<PedidoDto>(pedido));
                }
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(int Id, PedidoDto model)
        {
            try
            {
                var pedido = await _repo.GetAllPedidoById(Id);

                if(pedido == null) 
                {
                    return NotFound();
                }

                _mapper.Map(model, pedido);

                _repo.Update(pedido);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/pedido/{model.Id}",  _mapper.Map<PedidoDto>(pedido));
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
                var pedido = await _repo.GetAllPedidoById(Id);

                if(pedido == null) 
                {
                    return NotFound();
                }
                _repo.Delete(pedido);

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