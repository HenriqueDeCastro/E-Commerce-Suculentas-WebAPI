using System;
using System.IO;
using System.Net.Http.Headers;
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
    public class ProductController : ControllerBase
    {
        private readonly ISuculentasRepository _repo;
        private readonly IMapper _mapper;

        public ProductController(ISuculentasRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _repo.GetAllProductById(id);

                var results = _mapper.Map<ProductDTO>(product);

                return Ok(results);
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("getByCategory/{categoryId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCategoryId(int categoryId)
        {
            try
            {
                var product = await _repo.GetAllProductByCategoryId(categoryId);

                var results = _mapper.Map<ProductDTO[]>(product);

                return Ok(results);
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductDTO model)
        {
            try
            {
                var product = _mapper.Map<Product>(model);

                _repo.Add(product);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/product/{model.Id}",  _mapper.Map<ProductDTO>(product));
                }
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return BadRequest();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var mini = bool.Parse(Request.Form["mini"].ToString());
                string folderName = null;

                if (mini)
                {
                    folderName = Path.Combine(@"Resources\Mini");
                } else
                {
                    folderName = Path.Combine(@"Resources\Normal");
                }
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if(file.Length > 0)
                {
                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSave, filename.Replace("\"", " ").Trim());

                    using(var stream = new FileStream(fullPath, FileMode.Create)) 
                    {
                        file.CopyTo(stream);
                    }
                }

                return Ok();
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ProductDTO model)
        {
            try
            {
                var product = await _repo.GetAllProductById(id);

                if(product == null) 
                {
                    return NotFound();
                }

                _mapper.Map(model, product);

                _repo.Update(product);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/product/{model.Id}",  _mapper.Map<ProductDTO>(product));
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
                var product = await _repo.GetAllProductById(id);

                if(product == null) 
                {
                    return NotFound();
                }
                _repo.Delete(product);

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