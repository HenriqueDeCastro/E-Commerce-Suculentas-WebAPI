using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Suculentas.Domain;
using Suculentas.Domain.Identity;
using Suculentas.Domain.Pagination;

namespace Suculentas.Repository
{
    public interface ISuculentasRepository
    {
        // GENERAL
        void Add<T>(T entity) where T: class;
        void Update<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveChangesAsync();

        // ADDRESS
        Task<Address> GetAllAddressById(int id);        
        Task<Address[]> GetAllAddressByUserId(int userId); 

        // CATEGORY
        Task<Category[]> GetAllCategorys();        
        Task<Category[]> GetAllCategorysWithoutProducts();        
        Task<Category[]> GetAllCategorysHomepage();
        Task<Category[]> GetAllCategorysHomepageCompany();
        Task<Category> GetAllCategoryById(int id);
        Task<CategoryPagination> GetAllCategoryByClient(int id, int currentPage, string orderBy, string search);        
        Task<CategoryPagination> GetAllCategoryByCompany(int id, int currentPage, string orderBy, string search);                

        // ORDER
        Task<Order> GetAllOrderById(int id);       
        Task<Order[]> GetAllOrderBySaleId(int saleId);       

        //PRODUCT
        Task<Product> GetAllProductById(int Id);       
        Task<Product[]> GetAllProductByCategoryId(int categoryId);  

        // PRODUCT TYPE
        Task<ProductType[]> GetAllProductType();       
        Task<ProductType[]> GetAllProductTypeWithoutProduct();       
        Task<ProductType> GetAllProductTypeById(int id);   
        Task<ProductType> GetAllProductTypeByName(string name);

        // STATUS
        Task<Status[]> GetAllStatus();       
        Task<Status[]> GetAllStatusById(int Id);  

        // SALE
        Task<Sale[]> GetAllSales();       
        Task<Sale> GetAllSaleById(int id);       
        Task<Sale> GetAllSaleByIdWithoutInclude(int id);
        Task<Sale> GetAllSaleByIdCompany(int id);
        Task<SalePagination> GetAllSaleByStatusId(int statusId, int currentPage);       
        Task<SalePagination> GetAllSaleByUserId(int userId, int statusId, int currentPage);       
        Task<SaleStatusCount[]> GetAllSaleCountStatusCompany();
        Task<SaleStatusCount[]> GetAllSaleCountStatusUser(int userId);
    }
}