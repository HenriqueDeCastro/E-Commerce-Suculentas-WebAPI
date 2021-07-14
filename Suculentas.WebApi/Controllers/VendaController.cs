using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Suculentas.Business;
using Suculentas.Domain;
using Suculentas.PagSeguro;
using Suculentas.Repository;
using Suculentas.WebApi.Dtos;
using Suculentas.PagSeguro.Dtos;
using Microsoft.AspNetCore.Identity;
using Suculentas.Domain.Identity;
using System.Globalization;
using Suculentas.Email;

namespace Suculentas.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VendaController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ISuculentasBusiness _business;
        private readonly ISuculentasPagSeguro _pagSeguro;
        private readonly ISuculentasRepository _repo;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ISuculentasEmail _email;

        public VendaController(IConfiguration config,
                               ISuculentasRepository repo, 
                               IMapper mapper, 
                               ISuculentasBusiness business,
                               UserManager<User> userManager,
                               ISuculentasEmail email,
                               ISuculentasPagSeguro pagSeguro)
        {
            _config = config;
            _business = business;
            _mapper = mapper;
            _pagSeguro = pagSeguro;
            _repo = repo;
            _email = email;
            _userManager = userManager;
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

        [HttpGet("getByIdEmpresa/{Id}")]
        public async Task<IActionResult> GetByIdEmpresa(int Id)
        {
            try
            {
                var vendas = await _repo.GetAllVendaByIdEmpresa(Id);

                return Ok(vendas);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("getStatusByUser")]
        public async Task<IActionResult> GetByUserId(int UserId, int StatusId, int pageAtual)
        {
            try
            {
                var vendas = await _repo.GetAllVendaByUserId(UserId, StatusId, pageAtual);

                return Ok(vendas);
            }
            catch (System.Exception ex) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("getByStatus")]
        public async Task<IActionResult> GetByStatusId(int statusId, int pageAtual)
        {
            try
            {
                var vendas = await _repo.GetAllVendaByStatusId(statusId, pageAtual);

                return Ok(vendas);
            }
            catch (System.Exception ex) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("getByStatusCountEmpresa")]
        public async Task<IActionResult> GetByStatusCountEmpresa()
        {
            try
            {
                var vendasCount = await _repo.GetAllVendaCountStatusEmpresa();

                return Ok(vendasCount);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("getByStatusCountUser/{UserId}")]
        public async Task<IActionResult> GetByStatusCountUser(int UserId)
        {
            try
            {
                var vendasCount = await _repo.GetAllVendaCountStatusUser(UserId);

                return Ok(vendasCount);
            }
            catch (System.Exception ex)
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

                // Separando os pedidos
                List<Pedido> pedidos = venda.Pedidos;
                venda.Pedidos = null;

                // Status da Venda = Aguardando Comprovante
                venda.StatusId =  Int16.Parse(_config.GetSection("StatusVendas:AguardandoPagamento").Value);

                _repo.Add(venda);

                if(await _repo.SaveChangesAsync()){

                    List<PagSeguroItemDTO> itens = new List<PagSeguroItemDTO>();
                    List<Produto> produtosIndisponiveis = new List<Produto>();

                    foreach (Pedido pedido in pedidos)
                    {
                        // Recupera o produto pelo Id
                        var produto = await _repo.GetAllProdutoById(pedido.ProdutoId);

                        // Verifica se o produto está ativo, se há estoque e se há estoque suficiente para o pedido
                        bool produtoDisponivel = _business.VerificaProdutoDisponivel(produto, pedido.Quantidade);

                        if(produtoDisponivel) {

                            // Adiciona Pedido no banco de dados
                            var pedidoMapper = _mapper.Map<Pedido>(pedido);
                            pedidoMapper.VendaId = venda.Id;
                            _repo.Add(pedidoMapper);

                            // Monta lista do item para o PagSeguro 
                            PagSeguroItemDTO item = new PagSeguroItemDTO();
                            item.itemId = produto.Id.ToString();
                            item.itemDescription = produto.Nome;
                            item.itemAmount = produto.Preco.ToString("N2", CultureInfo.InvariantCulture).Replace(",", "");
                            item.itemQuantity = pedido.Quantidade.ToString();
                            item.itemWeight = "0";

                            itens.Add(item);
                        } 
                        else {
                            produtosIndisponiveis.Add(produto);
                        }
                    }

                    if(produtosIndisponiveis.Count > 0) {
                        return this.StatusCode(StatusCodes.Status404NotFound, produtosIndisponiveis);
                    }

                    if(await _repo.SaveChangesAsync()) {

                        // Se pedido com frete, adicionar na lista de itens o valor
                        if(venda.Frete) {
                            PagSeguroItemDTO item = new PagSeguroItemDTO();
                            item.itemId = "Jadlog";
                            item.itemDescription = "Frete";
                            item.itemAmount = venda.ValorFrete.ToString().Replace(",", ".");
                            item.itemQuantity = "1";
                            item.itemWeight = "0";

                            itens.Add(item);
                        }

                        // Resgata as nossas informações do PagSeguro do AppSettings
                        string urlPagSeguro = _config.GetSection("PagSeguro:Url_V2").Value + "checkout";
                        string token = _config.GetSection("PagSeguro:Token").Value;
                        string email = _config.GetSection("PagSeguro:Email").Value;

                        try
                        {
                            venda.CodigoTransacao = _pagSeguro.Checkout(email, token, urlPagSeguro, itens, venda.Id.ToString());
                            _repo.Update(venda);   
                        }
                        catch (System.Exception ex)
                        {
                            venda.StatusId = Int16.Parse(_config.GetSection("StatusVendas:Cancelado").Value);
                            _repo.Update(venda);

                            return this.StatusCode(StatusCodes.Status503ServiceUnavailable, ex);
                        }
                        
                        if(await _repo.SaveChangesAsync()) {
                            return Created($"/venda/{venda.Id}",  _mapper.Map<VendaDto>(venda));
                        } 
                    }
                }
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.InnerException.Message);
            }
            return BadRequest();
        }

        [HttpPost("{NotificationPagSeguro}")]
        [AllowAnonymous]
        public async Task<IActionResult> NotificationPagSeguro()
        {
            try
            {
                // Recupera as informações da requisição do PagSeguro
                var request = HttpContext.Request;
                string notificationType = Request.Form["notificationType"];  
                string notificationCode = Request.Form["notificationCode"];

                // Recupera as informações do App Settings do PagSeguro
                string urlPagSeguro = _config.GetSection("PagSeguro:Url_V3").Value + "transactions/notifications/";
                string token = _config.GetSection("PagSeguro:Token").Value;
                string email = _config.GetSection("PagSeguro:Email").Value;

                if (notificationType == "transaction") 
                {
                    ConsultaTransacaoPagSeguroTransactionDTO consulta = _pagSeguro.ConsultaPorCodigoReferencia(email, token, urlPagSeguro, notificationCode);

                    if(consulta != null)
                    {
                        Venda venda = await _repo.GetAllVendaById(Convert.ToInt32(consulta.Reference));

                        string statusPagseguro = _business.NomeAmigavelStatusPagSeguro(consulta.Status);
                        venda.StatusPagSeguro = venda.StatusPagSeguro == statusPagseguro ? venda.StatusPagSeguro : statusPagseguro;

                        int statusVenda = _business.StatusASerAtualizada(venda.StatusId, consulta.Status);
                        venda.StatusId = statusVenda == venda.StatusId ? venda.StatusId : statusVenda;

                        if (statusVenda != venda.StatusId && statusVenda == Int16.Parse(_config.GetSection("StatusVendas:AguardandoEnvio").Value))
                        {
                            List<Pedido> pedidos = venda.Pedidos;

                            foreach (Pedido pedido in pedidos)
                            {
                                Produto produto = await _repo.GetAllProdutoById(pedido.ProdutoId);

                                if (produto.TipoProdutoId == Int16.Parse(_config.GetSection("TipoProduto:Estoque").Value))
                                {
                                    produto.Estoque = produto.Estoque - pedido.Quantidade;
                                    _repo.Update(produto);
                                }

                            }
                        }

                        _repo.Update(venda);

                        if (await _repo.SaveChangesAsync())
                        {
                            return Ok();
                        }
                    }
                } 

                return BadRequest();
            }
            catch (System.Exception ex) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(int Id, VendaDto model)
        {
            try
            {
                var venda = await _repo.GetAllVendaByIdSemInclude(Id);

                if(model.CodigoRastreio != null)
                {
                    venda.CodigoRastreio = model.CodigoRastreio;
                }

                if (model.StatusId == Int16.Parse(_config.GetSection("StatusVendas:Enviado").Value) && model.CodigoRastreio != null)
                {
                    var user = await _userManager.FindByIdAsync(venda.UserId.ToString());

                    string corpoEmail = _email.BodyVendaEnviada(venda.Id, venda.CodigoRastreio, user.FullName);
                    _email.EnviarEmailSuculentas(user.Email, "[Suculentas da Rô] Compra Enviada", corpoEmail);
                }

                venda.StatusId = model.StatusId;
                venda.Endereco = model.Endereco;


                if(venda == null) 
                {
                    return NotFound();
                }

                _repo.Update(venda);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/venda/{model.Id}",  _mapper.Map<VendaDto>(venda));
                }
            }
            catch (System.Exception ex) 
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