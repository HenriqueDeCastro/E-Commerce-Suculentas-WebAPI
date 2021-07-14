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
        //GERAL
        void Add<T>(T entity) where T: class;
        void Update<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveChangesAsync();

        //CATEGORIA
        Task<Categoria[]> GetAllCategorias();        
        Task<Categoria[]> GetAllCategoriasSemProdutos();        
        Task<Categoria[]> GetAllCategoriasPagInicial();
        Task<Categoria[]> GetAllCategoriasPagInicialEmpresa();
        Task<Categoria> GetAllCategoriaById(int Id);
        Task<CategoriaPagination> GetAllCategoriaByCliente(int Id, int pageAtual, string orderBy, string search);        
        Task<CategoriaPagination> GetAllCategoriaByEmpresa(int Id, int pageAtual, string orderBy, string search);                

        //ENDEREÇO
        Task<Endereco> GetAllEnderecoById(int Id);        
        Task<Endereco[]> GetAllEnderecoByUserId(int UserId);              

        //GASTOS  
        Task<Gastos[]> GetAllGastos();     
        Task<Gastos> GetAllGastosById(int Id);     
        Task<Gastos[]> GetAllGastosByData(DateTime Data);     

        //PEDIDO
        Task<Pedido> GetAllPedidoById(int Id);       
        Task<Pedido[]> GetAllPedidoByVendaId(int VendaId);       

        //PRODUTO
        Task<Produto> GetAllProdutoById(int Id);       
        Task<Produto[]> GetAllProdutoByCategoriaId(int CategoriaId);  

        //STATUS
        Task<Status[]> GetAllStatus();       
        Task<Status[]> GetAllStatusById(int Id);  

        //TIPO PRODUTO
        Task<TipoProduto[]> GetAllTipoProduto();       
        Task<TipoProduto[]> GetAllTipoProdutoSemProduto();       
        Task<TipoProduto> GetAllTipoProdutoById(int Id);   

        //VENDA
        Task<Venda[]> GetAllVenda();       
        Task<Venda> GetAllVendaById(int Id);       
        Task<Venda> GetAllVendaByIdSemInclude(int Id);
        Task<Venda> GetAllVendaByIdEmpresa(int Id);
        Task<VendaPagination> GetAllVendaByUserId(int UserId, int StatusId, int pageAtual);       
        Task<VendaPagination> GetAllVendaByStatusId(int StatusId, int pageAtual);       
        Task<VendaStatusCount[]> GetAllVendaCountStatusEmpresa();
        Task<VendaStatusCount[]> GetAllVendaCountStatusUser(int UserId);
    }
}