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
    public class AddressController : ControllerBase
    {
        private readonly ISuculentasRepository _repo;
        private readonly IMapper _mapper;

        public AddressController(ISuculentasRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var address = await _repo.GetAllAddressById(id);

                var results = _mapper.Map<AddressDTO>(address);

                return Ok(results);
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("getByUserId/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            try
            {
                var addresses = await _repo.GetAllAddressByUserId(userId);

                var results = _mapper.Map<AddressDTO[]>(addresses);

                return Ok(results);
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddressDTO model)
        {
            try
            {
                var address = _mapper.Map<Address>(model);

                _repo.Add(address);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/address/{model.Id}",  _mapper.Map<AddressDTO>(address));
                }
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, AddressDTO model)
        {
            try
            {
                var address = await _repo.GetAllAddressById(id);

                if(address == null) 
                {
                    return NotFound();
                }

                _mapper.Map(model, address);

                _repo.Update(address);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/address/{model.Id}",  _mapper.Map<AddressDTO>(address));
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
                var address = await _repo.GetAllAddressById(id);

                if(address == null) 
                {
                    return NotFound();
                }
                _repo.Delete(address);

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