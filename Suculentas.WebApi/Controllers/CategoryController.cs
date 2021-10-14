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
    public class CategoryController : ControllerBase
    {
        private readonly ISuculentasRepository _repo;
        private readonly IMapper _mapper;

        public CategoryController(ISuculentasRepository repo, IMapper mapper)
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
                var categorys = await _repo.GetAllCategorys();

                var results = _mapper.Map<CategoryDTO[]>(categorys);

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
                var categorys = await _repo.GetAllCategoryById(id);

                var results = _mapper.Map<CategoryDTO>(categorys);

                return Ok(results);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou: " + e.Message);
            }
        }

        [HttpGet("getClient")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByClient(int id, int currentPage, string orderBy, string search)
        {
            try
            {
                var category = await _repo.GetAllCategoryByClient(id, currentPage, orderBy, search);

                return Ok(category);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("getCompany")]
        public async Task<IActionResult> GetByIdCompany(int id, int currentPage, string orderBy, string search)
        {
            try
            {
                var category = await _repo.GetAllCategoryByCompany(id, currentPage, orderBy, search);

                return Ok(category);
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("getWithoutProducts")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategorysWithoutProducts()
        {
            try
            {
                var categorys = await _repo.GetAllCategorysWithoutProducts();

                var results = _mapper.Map<CategoryDTO[]>(categorys);

                return Ok(results);
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("getPagHomepageCompany")]
        public async Task<IActionResult> GetAllCategorysHomepageCompany()
        {
            try
            {
                var categorys = await _repo.GetAllCategorysHomepageCompany();

                var results = _mapper.Map<CategoryDTO[]>(categorys);

                return Ok(results);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("getHomepage")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategorysHomepage()
        {
            try
            {
                var categorys = await _repo.GetAllCategorysHomepage();

                var results = _mapper.Map<CategoryDTO[]>(categorys);

                return Ok(results);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoryDTO model)
        {
            try
            {
                var category = _mapper.Map<Category>(model);

                _repo.Add(category);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/category/{model.Id}",  _mapper.Map<CategoryDTO>(category));
                }
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CategoryDTO model)
        {
            try
            {
                var category = await _repo.GetAllCategoryById(id);
                var products = await _repo.GetAllProductByCategoryId(category.Id);

                if(category.Active != model.Active) 
                {
                    foreach (var product in products)
                    {
                        product.Active = model.Active;
                        _repo.Update(product);
                    }
                }

                if(category == null) 
                {
                    return NotFound();
                }

                _mapper.Map(model, category);

                _repo.Update(category);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/category/{model.Id}",  _mapper.Map<CategoryDTO>(category));
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
                var category = await _repo.GetAllCategoryById(id);

                if(category == null) 
                {
                    return NotFound();
                }
                _repo.Delete(category);

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