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
    public class OrderController : ControllerBase
    {
        private readonly ISuculentasRepository _repo;
        private readonly IMapper _mapper;

        public OrderController(ISuculentasRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var order = await _repo.GetAllOrderById(id);

                var results = _mapper.Map<OrderDTO>(order);

                return Ok(results);
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("getBySale/{saleId}")]
        public async Task<IActionResult> GetBySaleId(int saleId)
        {
            try
            {
                var order = await _repo.GetAllOrderBySaleId(saleId);

                var results = _mapper.Map<OrderDTO[]>(order);

                return Ok(results);
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(OrderDTO model)
        {
            try
            {
                var order = _mapper.Map<Order>(model);

                _repo.Add(order);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/order/{model.Id}",  _mapper.Map<OrderDTO>(order));
                }
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, OrderDTO model)
        {
            try
            {
                var order = await _repo.GetAllOrderById(id);

                if(order == null) 
                {
                    return NotFound();
                }

                _mapper.Map(model, order);

                _repo.Update(order);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/order/{model.Id}",  _mapper.Map<OrderDTO>(order));
                }
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var order = await _repo.GetAllOrderById(id);

                if(order == null) 
                {
                    return NotFound();
                }
                _repo.Delete(order);

                if(await _repo.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return BadRequest();
        }
    }
}