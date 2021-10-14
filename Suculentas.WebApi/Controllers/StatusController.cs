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
    public class StatusController : ControllerBase
    {
        private readonly ISuculentasRepository _repo;
        private readonly IMapper _mapper;

        public StatusController(ISuculentasRepository repo, IMapper mapper)
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
                var status = await _repo.GetAllStatus();

                var results = _mapper.Map<StatusDTO>(status);

                return Ok(results);
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var status = await _repo.GetAllStatusById(id);

                var results = _mapper.Map<StatusDTO[]>(status);

                return Ok(results);
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(StatusDTO model)
        {
            try
            {
                var status = _mapper.Map<Status>(model);

                _repo.Add(status);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/status/{model.Id}",  _mapper.Map<StatusDTO>(status));
                }
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, StatusDTO model)
        {
            try
            {
                var status = await _repo.GetAllStatusById(id);

                if(status == null) 
                {
                    return NotFound();
                }

                _mapper.Map(model, status);

                _repo.Update(status);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/status/{model.Id}",  _mapper.Map<StatusDTO>(status));
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
                var status = await _repo.GetAllStatusById(id);

                if(status == null) 
                {
                    return NotFound();
                }
                _repo.Delete(status);

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