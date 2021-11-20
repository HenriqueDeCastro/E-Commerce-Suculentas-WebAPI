using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Suculentas.Domain;
using Suculentas.Domain.Identity;
using Suculentas.Domain.Pagination;

namespace Suculentas.Repository
{
    public class SuculentasRepository : ISuculentasRepository
    {
        private readonly SuculentasContext _Context;
        private readonly IConfiguration _config;

        //GERAL
        public SuculentasRepository(SuculentasContext context, IConfiguration config)
        {
            _config = config;
            _Context = context;
            _Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public void Add<T>(T entity) where T : class
        {
            _Context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _Context.Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _Context.Update(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _Context.SaveChangesAsync()) > 0;
        }

        // ADDRESS
        public async Task<Address> GetAllAddressById(int id)
        {
            IQueryable<Address> query = _Context.Adresses;

            query = query.OrderByDescending(c => c.Id)
                .Where(c => c.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Address[]> GetAllAddressByUserId(int userId)
        {
            IQueryable<Address> query = _Context.Adresses;

            query = query.OrderByDescending(c => c.Description)
                .Where(c => c.UserId == userId);

            return await query.ToArrayAsync();
        }

        // CATEGORY
        public async Task<Category[]> GetAllCategorys()
        {
            IQueryable<Category> query = _Context.Categorys
                .Include(c => c.Products);

            query = query.OrderBy(c => c.Name);

            return await query.ToArrayAsync();
        }

        public async Task<Category[]> GetAllCategorysWithoutProducts()
        {
            IQueryable<Category> query = _Context.Categorys;

            query = query.OrderBy(c => c.Name);

            return await query.ToArrayAsync();    
        }

        public async Task<Category[]> GetAllCategorysHomepage()
        {
            var order = await _Context.productTypes.Where(p => p.Name == _config.GetSection("AppSettings:ProductType:Encomenda").Value).FirstOrDefaultAsync();

            IQueryable<Category> categorys = _Context.Categorys.Select(c => new Category {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Active = c.Active,
                Products = _Context.Products
                    .Where(p => p.CategoryId == c.Id && p.Active == true 
                        && (p.ProductTypeId == order.Id? true : p.Inventory > 0))
                .OrderByDescending(p => p.Id).Take(4).ToList()});

            categorys = categorys.Where(c => c.Active == true).OrderBy(c => c.Name);

            return await categorys.ToArrayAsync();
        }

        public async Task<Category[]> GetAllCategorysHomepageCompany()
        {
            IQueryable<Category> categorias = _Context.Categorys.Select(c => new Category
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Active = c.Active,
                Products = _Context.Products.Where(p => p.CategoryId == c.Id).OrderByDescending(p => p.Id).Take(4).ToList()
            });

            categorias = categorias.OrderBy(c => c.Name);

            return await categorias.ToArrayAsync();
        }
        
        public async Task<Category> GetAllCategoryById(int id)
        {
            IQueryable<Category> query = _Context.Categorys;

            query = query.OrderByDescending(c => c.Id)
                .Where(c => c.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<CategoryPagination> GetAllCategoryByClient(int id, int currentPage, string orderBy, string search)
        {
            CategoryPagination categoryPagination = new CategoryPagination();

            int pagination = Int16.Parse(_config.GetSection("AppSettings:ProductPagination").Value);
            int skip = currentPage * pagination;

            var order = await _Context.productTypes.Where(p => p.Name == _config.GetSection("AppSettings:ProductType:Encomenda").Value).FirstOrDefaultAsync();

            IQueryable<Category> queryCategorys = _Context.Categorys.Where(c => c.Id == id);
            IQueryable<Product> queryProducts = _Context.Products
                .Where(p => p.CategoryId == id && p.Active == true
                    && (p.ProductTypeId == order.Id ? true : p.Inventory > 0));

            if(search != null && search != "")
            {
                queryProducts = queryProducts.Where(p => p.Name.ToUpper().Contains(search.ToUpper()));
            }

            categoryPagination.LastPage = (skip + pagination) < queryProducts.Count() ? false : true;

            if (orderBy == _config.GetSection("AppSettings:OrderBy:Alfabetica").Value)
            {
                queryProducts = queryProducts.OrderBy(p => p.Name);
            }
            else if (orderBy == _config.GetSection("AppSettings:OrderBy:PrecoMaior").Value)
            {
                queryProducts = queryProducts.OrderByDescending(p => p.Price);
            }
            else if (orderBy == _config.GetSection("AppSettings:OrderBy:PrecoMenor").Value)
            {
                queryProducts = queryProducts.OrderBy(p => p.Price);
            }
            else
            {
                queryProducts = queryProducts.OrderByDescending(p => p.Id);
            }

            queryProducts = queryProducts
                .Skip(skip).Take(pagination);

            categoryPagination.Category = await queryCategorys.FirstOrDefaultAsync();
            categoryPagination.Products = await queryProducts.ToArrayAsync(); 

            return categoryPagination;
        }

        public async Task<CategoryPagination> GetAllCategoryByCompany(int id, int currentPage, string orderBy, string search)
        {
            CategoryPagination categoryPagination = new CategoryPagination();

            int pagination = Int16.Parse(_config.GetSection("AppSettings:ProductPagination").Value);
            int skip = currentPage * pagination;

            IQueryable<Category> queryCategory = _Context.Categorys.Where(c => c.Id == id);
            IQueryable<Product> queryProducts = _Context.Products.Where(p => p.CategoryId == id);

            if (search != null && search != "")
            {
                queryProducts = queryProducts.Where(p => p.Name.ToUpper().Contains(search.ToUpper()));
            }

            categoryPagination.LastPage = (skip + pagination) < queryProducts.Count() ? false : true;

            if (orderBy == _config.GetSection("AppSettings:OrderBy:Alfabetica").Value)
            {
                queryProducts = queryProducts.OrderBy(p => p.Name);
            }
            else if (orderBy == _config.GetSection("AppSettings:OrderBy:PrecoMaior").Value)
            {
                queryProducts = queryProducts.OrderByDescending(p => p.Price);
            }
            else if (orderBy == _config.GetSection("AppSettings:OrderBy:PrecoMenor").Value)
            {
                queryProducts = queryProducts.OrderBy(p => p.Price);
            }
            else if (orderBy == _config.GetSection("AppSettings:OrderBy:Estoque").Value)
            {
                var inventory = await _Context.productTypes.Where(p => p.Name == _config.GetSection("AppSettings:ProductType:Estoque").Value).FirstOrDefaultAsync();

                queryProducts = queryProducts
                    .Where(p => p.ProductTypeId == inventory.Id)
                    .OrderBy(p => p.Inventory);
            }
            else
            {
                queryProducts = queryProducts.OrderByDescending(p => p.Id);
            }

            queryProducts = queryProducts
                .Skip(skip).Take(pagination);

            categoryPagination.Category = await queryCategory.FirstOrDefaultAsync();
            categoryPagination.Products = await queryProducts.ToArrayAsync();

            return categoryPagination;
        }

        // ORDER
        public async Task<Order> GetAllOrderById(int id)
        {
            IQueryable<Order> query = _Context.Orders;

            query = query.OrderByDescending(c => c.Id)
                .Where(c => c.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Order[]> GetAllOrderBySaleId(int saleId)
        {
            IQueryable<Order> query = _Context.Orders;

            query = query.OrderByDescending(c => c.Id)
                .Where(c => c.SaleId == saleId);

            return await query.ToArrayAsync();
        }

        // PRODUCT
        public async Task<Product> GetAllProductById(int id)
        {
            IQueryable<Product> query = _Context.Products;

            query = query.OrderByDescending(c => c.Id)
                .Where(c => c.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Product[]> GetAllProductByCategoryId(int categoryId)
        {
            IQueryable<Product> query = _Context.Products;

            query = query.OrderByDescending(c => c.Id)
                .Where(c => c.CategoryId == categoryId);

            return await query.ToArrayAsync();
        }

        // PRODUCT TYPE
        public async Task<ProductType[]> GetAllProductType()
        {
            IQueryable<ProductType> query = _Context.productTypes
                .Include(c => c.Products);

            query = query.OrderBy(c => c.Name);

            return await query.ToArrayAsync();        
        }

        public async Task<ProductType[]> GetAllProductTypeWithoutProduct()
        {
            IQueryable<ProductType> query = _Context.productTypes;

            query = query.OrderBy(c => c.Name);

            return await query.ToArrayAsync();        
        }

        public async Task<ProductType> GetAllProductTypeById(int id)
        {
            IQueryable<ProductType> query = _Context.productTypes
                .Include(c => c.Products);

            query = query.OrderBy(c => c.Name)
                .Where(c => c.Id == id);

            return await query.FirstOrDefaultAsync();        
        }

        public async Task<ProductType> GetAllProductTypeByName(string name)
        {
            IQueryable<ProductType> query = _Context.productTypes;

            query = query.OrderBy(c => c.Name)
                .Where(c => c.Name == name);

            return await query.FirstOrDefaultAsync();
        }

        // STATUS
        public async Task<Status[]> GetAllStatus()
        {
            IQueryable<Status> query = _Context.Status;

            query = query.OrderByDescending(c => c.Id);

            return await query.ToArrayAsync();        
        }

        public async Task<Status[]> GetAllStatusById(int id)
        {
            IQueryable<Status> query = _Context.Status;

            query = query.OrderByDescending(c => c.Id)
                .Where(c => c.Id == id);

            return await query.ToArrayAsync();        
        }

        // SALE
        public async Task<Sale[]> GetAllSales()
        {
            IQueryable<Sale> query = _Context.Sales;

            query = query.OrderByDescending(c => c.Date);

            return await query.ToArrayAsync();
        }

        public async Task<Sale> GetAllSaleById(int id)
        {
            IQueryable<Sale> query = _Context.Sales
                .Include(c => c.Orders)
                .ThenInclude(c => c.Product);

            query = query.Where(c => c.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Sale> GetAllSaleByIdWithoutInclude(int id)
        {
            IQueryable<Sale> query = _Context.Sales;

            query = query.Where(c => c.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Sale> GetAllSaleByIdCompany(int id)
        {
            IQueryable<Sale> query = _Context.Sales
                .Include(c => c.User)
                .Include(c => c.Orders)
                    .ThenInclude(c => c.Product);

            query = query.Where(c => c.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<SalePagination> GetAllSaleByStatusId(int statusId, int currentPage)
        {
            IQueryable<Sale> querySales = _Context.Sales.Where(c => c.StatusId == statusId).OrderBy(c => c.Date);
            SalePagination salesPagination = new SalePagination();

            int pagination = Int16.Parse(_config.GetSection("AppSettings:SalePagination").Value);
            int skip = currentPage * pagination;

            salesPagination.LastPage = (skip + pagination) < querySales.Count()? false : true;

            querySales = querySales.Include(c => c.Orders).ThenInclude(c => c.Product).Skip(skip).Take(pagination);
            salesPagination.Sales = await querySales.ToArrayAsync();

            return salesPagination;
        }

        public async Task<SalePagination> GetAllSaleByUserId(int userId, int statusId, int currentPage)
        {
            IQueryable<Sale> querySales = _Context.Sales.Where(c => c.UserId == userId && c.StatusId == statusId).OrderBy(c => c.Date);
            SalePagination salePagination = new SalePagination();

            int pagination = Int16.Parse(_config.GetSection("AppSettings:SalePagination").Value);
            int skip = currentPage * pagination;

            salePagination.LastPage = (skip + pagination) < querySales.Count() ? false : true;

            querySales = querySales.Include(c => c.Orders).ThenInclude(c => c.Product).Skip(skip).Take(pagination);
            salePagination.Sales = await querySales.ToArrayAsync();

            return salePagination;
        }

        public async Task<SaleStatusCount[]> GetAllSaleCountStatusCompany()
        {
            IQueryable<Sale> querySales = _Context.Sales;

            IQueryable<SaleStatusCount> query = querySales.GroupBy(x => x.StatusId)
                .Select(x => new SaleStatusCount { StatusId = x.Key, CountSale = x.Count() });

            query = query.OrderByDescending(c => c.StatusId);

            return await query.ToArrayAsync();
        }

        public async Task<SaleStatusCount[]> GetAllSaleCountStatusUser(int userId)
        {
            IQueryable<Sale> querySales = _Context.Sales.Where(v => v.UserId == userId);

            IQueryable<SaleStatusCount> query = querySales
                .GroupBy(s => s.StatusId)
                .Select(x => new SaleStatusCount { 
                    StatusId = x.Key, 
                    StatusName = _Context.Status.Where(s => s.Id == x.Key).Select(s=> s.Name).FirstOrDefault(),
                    CountSale = x.Count() 
                });

            query = query.OrderByDescending(c => c.StatusId);

            return await query.ToArrayAsync();
        }
    }
}