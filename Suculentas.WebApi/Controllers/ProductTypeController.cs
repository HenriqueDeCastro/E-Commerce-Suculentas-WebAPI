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
    public class ProductTypeController : ControllerBase
    {
        private readonly ISuculentasRepository _repo;
        private readonly IMapper _mapper;

        public ProductTypeController(ISuculentasRepository repo, IMapper mapper)
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
                var productTypes = await _repo.GetAllProductType();

                var results = _mapper.Map<ProductTypeDTO[]>(productTypes);

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
                var productType = await _repo.GetAllProductTypeById(id);

                var results = _mapper.Map<ProductTypeDTO>(productType);

                return Ok(results);
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("getWithoutProduct")]
        [AllowAnonymous]
        public async Task<IActionResult> GetWithoutProduct()
        {
            try
            {
                var productTypes = await _repo.GetAllProductTypeWithoutProduct();

                var results = _mapper.Map<ProductTypeDTO[]>(productTypes);

                return Ok(results);
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductTypeDTO model)
        {
            try
            {
                var productType = _mapper.Map<ProductType>(model);

                _repo.Add(productType);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/producttype/{model.Id}",  _mapper.Map<ProductTypeDTO>(productType));
                }
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ProductTypeDTO model)
        {
            try
            {
                var productType = await _repo.GetAllProductTypeById(id);

                if(productType == null) 
                {
                    return NotFound();
                }

                _mapper.Map(model, productType);

                _repo.Update(productType);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/producttype/{model.Id}",  _mapper.Map<ProductTypeDTO>(productType));
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
                var productType = await _repo.GetAllProductTypeById(id);

                if(productType == null) 
                {
                    return NotFound();
                }
                _repo.Delete(productType);

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