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
    public class SaleController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ISuculentasBusiness _business;
        private readonly ISuculentasPagSeguro _pagSeguro;
        private readonly ISuculentasRepository _repo;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ISuculentasEmail _email;

        public SaleController(IConfiguration config,
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
                var sales = await _repo.GetAllSales();

                var results = _mapper.Map<SaleDTO[]>(sales);

                return Ok(results);
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var sales = await _repo.GetAllSaleById(Id);

                var results = _mapper.Map<SaleDTO>(sales);

                return Ok(results);
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("getByIdCompany/{id}")]
        public async Task<IActionResult> GetByIdEmpresa(int id)
        {
            try
            {
                var sales = await _repo.GetAllSaleByIdCompany(id);

                return Ok(sales);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("getStatusByUser")]
        public async Task<IActionResult> GetByUserId(int userId, int statusId, int currentPage)
        {
            try
            {
                var sales = await _repo.GetAllSaleByUserId(userId, statusId, currentPage);

                return Ok(sales);
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("getByStatus")]
        public async Task<IActionResult> GetByStatusId(int statusId, int currentPage)
        {
            try
            {
                var sales = await _repo.GetAllSaleByStatusId(statusId, currentPage);

                return Ok(sales);
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("getByStatusCountCompany")]
        public async Task<IActionResult> GetByStatusCountEmpresa()
        {
            try
            {
                var salesCount = await _repo.GetAllSaleCountStatusCompany();

                return Ok(salesCount);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("getByStatusCountUser/{userId}")]
        public async Task<IActionResult> GetByStatusCountUser(int userId)
        {
            try
            {
                var saleCount = await _repo.GetAllSaleCountStatusUser(userId);

                return Ok(saleCount);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(SaleDTO model)
        {
            try
            {
                var sale = _mapper.Map<Sale>(model);

                List<Order> orders = sale.Orders;
                sale.Orders = null;

                sale.StatusId =  Int16.Parse(_config.GetSection("AppSettings:StatusSales:AguardandoPagamento").Value);

                _repo.Add(sale);

                if(await _repo.SaveChangesAsync()){

                    List<PagSeguroItemDTO> items = new List<PagSeguroItemDTO>();
                    List<Product> unavailableProducts = new List<Product>();

                    foreach (Order order in orders)
                    {
                        var product = await _repo.GetAllProductById(order.ProductId);
                        bool productAvailable = _business.CheckProductAvailable(product, order.Amount);

                        if(productAvailable) {

                            var orderMapper = _mapper.Map<Order>(order);
                            orderMapper.SaleId = sale.Id;
                            _repo.Add(orderMapper);

                            PagSeguroItemDTO item = new PagSeguroItemDTO();
                            item.itemId = product.Id.ToString();
                            item.itemDescription = product.Name;
                            item.itemAmount = product.Price.ToString("N2", CultureInfo.InvariantCulture).Replace(",", "");
                            item.itemQuantity = order.Amount.ToString();
                            item.itemWeight = "0";

                            items.Add(item);
                        } 
                        else {
                            unavailableProducts.Add(product);
                        }
                    }

                    if(unavailableProducts.Count > 0) {
                        return this.StatusCode(StatusCodes.Status404NotFound, unavailableProducts);
                    }

                    if(await _repo.SaveChangesAsync()) {

                        if(sale.Shipping) {
                            PagSeguroItemDTO item = new PagSeguroItemDTO();
                            item.itemId = "Jadlog";
                            item.itemDescription = "Frete";
                            item.itemAmount = sale.ShippingValue.ToString().Replace(",", ".");
                            item.itemQuantity = "1";
                            item.itemWeight = "0";

                            items.Add(item);
                        }

                        string urlPagSeguro = _config.GetSection("PagSeguro:Url_V2").Value + "checkout";
                        string token = _config.GetSection("PagSeguro:Token").Value;
                        string email = _config.GetSection("PagSeguro:Email").Value;

                        try
                        {
                            sale.TrackingCode = _pagSeguro.Checkout(email, token, urlPagSeguro, items, sale.Id.ToString());
                            _repo.Update(sale);   
                        }
                        catch (System.Exception e)
                        {
                            sale.StatusId = Int16.Parse(_config.GetSection("AppSettings:StatusVendas:StatusSales").Value);
                            _repo.Update(sale);

                            return this.StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message);
                        }
                        
                        if(await _repo.SaveChangesAsync()) {
                            return Created($"/sale/{sale.Id}",  _mapper.Map<SaleDTO>(sale));
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
                var request = HttpContext.Request;
                string notificationType = Request.Form["notificationType"];  
                string notificationCode = Request.Form["notificationCode"];

                string urlPagSeguro = _config.GetSection("AppSettings:PagSeguro:Url_V3").Value + "transactions/notifications/";
                string token = _config.GetSection("AppSettings:PagSeguro:Token").Value;
                string email = _config.GetSection("AppSettings:PagSeguro:Email").Value;

                if (notificationType == "transaction") 
                {
                    PagSeguroTransactionDTO consult = _pagSeguro.ConsultByReferenceCode(email, token, urlPagSeguro, notificationCode);

                    if(consult != null)
                    {
                        Sale sale = await _repo.GetAllSaleById(Convert.ToInt32(consult.Reference));

                        string statusPagseguro = _business.FriendlyNameStatusPagSeguro(consult.Status);
                        sale.StatusPagSeguro = sale.StatusPagSeguro == statusPagseguro ? sale.StatusPagSeguro : statusPagseguro;

                        int statusVenda = _business.DefineStatusUpdated(sale.StatusId, consult.Status);
                        sale.StatusId = statusVenda == sale.StatusId ? sale.StatusId : statusVenda;

                        if (statusVenda != sale.StatusId && statusVenda == Int16.Parse(_config.GetSection("AppSettings:StatusSales:AguardandoEnvio").Value))
                        {
                            List<Order> orders = sale.Orders;

                            foreach (Order order in orders)
                            {
                                Product product = await _repo.GetAllProductById(order.Amount);

                                if (product.ProductTypeId == Int16.Parse(_config.GetSection("AppSettings:ProductType:Estoque").Value))
                                {
                                    product.Inventory = product.Inventory - order.Amount;
                                    _repo.Update(product);
                                }

                            }
                        }

                        _repo.Update(sale);

                        if (await _repo.SaveChangesAsync())
                        {
                            return Ok();
                        }
                    }
                } 

                return BadRequest();
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, SaleDTO model)
        {
            try
            {
                var sale = await _repo.GetAllSaleByIdWithoutInclude(id);

                if(model.TrackingCode != null)
                {
                    sale.TrackingCode = model.TrackingCode;
                }

                if (model.StatusId == Int16.Parse(_config.GetSection("AppSettings:StatusSales:Enviado").Value) && model.TrackingCode != null)
                {
                    var user = await _userManager.FindByIdAsync(sale.UserId.ToString());

                    string body = _email.BodySentSale(sale.Id, sale.TrackingCode, user.FullName);
                    _email.SendEmailSuculentas(user.Email, "[Suculentas da Rô] Compra Enviada", body);
                }

                sale.StatusId = model.StatusId;
                sale.Address = model.Address;


                if(sale == null) 
                {
                    return NotFound();
                }

                _repo.Update(sale);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/sale/{model.Id}",  _mapper.Map<SaleDTO>(sale));
                }
            }
            catch (System.Exception e) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return BadRequest();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var sale = await _repo.GetAllSaleById(Id);

                if(sale == null) 
                {
                    return NotFound();
                }
                _repo.Delete(sale);

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