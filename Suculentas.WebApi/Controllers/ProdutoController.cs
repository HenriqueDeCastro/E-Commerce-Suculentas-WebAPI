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
    public class ProdutoController : ControllerBase
    {
        private readonly ISuculentasRepository _repo;
        private readonly IMapper _mapper;

        public ProdutoController(ISuculentasRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var produto = await _repo.GetAllProdutoById(Id);

                var results = _mapper.Map<ProdutoDto>(produto);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("getByCategoria/{CategoriaId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCategoriaId(int CategoriaId)
        {
            try
            {
                var produto = await _repo.GetAllProdutoByCategoriaId(CategoriaId);

                var results = _mapper.Map<ProdutoDto[]>(produto);

                return Ok(results);
            }
            catch (System.Exception) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProdutoDto model)
        {
            try
            {
                var produto = _mapper.Map<Produto>(model);

                _repo.Add(produto);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/produto/{model.Id}",  _mapper.Map<ProdutoDto>(produto));
                }
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
            return BadRequest();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources");
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
            catch (System.Exception ex) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou " + ex);
            }
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(int Id, ProdutoDto model)
        {
            try
            {
                var produto = await _repo.GetAllProdutoById(Id);

                if(produto == null) 
                {
                    return NotFound();
                }

                _mapper.Map(model, produto);

                _repo.Update(produto);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/produto/{model.Id}",  _mapper.Map<ProdutoDto>(produto));
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
                var produto = await _repo.GetAllProdutoById(Id);

                if(produto == null) 
                {
                    return NotFound();
                }
                _repo.Delete(produto);

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