using System;
using System.Threading.Tasks;
using Suculentas.Domain;

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
        Task<Categoria> GetAllCategoriaByIdCliente(int Id);        
        Task<Categoria> GetAllCategoriaByIdEmpresa(int Id);

        //CIDADE
        Task<Cidade> GetAllCidadeById(int Id);        
        Task<Cidade> GetAllCidadeByNome(string Nome);        
        Task<Cidade[]> GetAllCidadeByEstadoId(int EstadoId);        

        //EMPRESA FRETE
        Task<EmpresaFrete[]> GetAllEmpresaFrete();        
        Task<EmpresaFrete> GetAllEmpresaById(int Id);             

        //ENDEREÇO
        Task<Endereco> GetAllEnderecoById(int Id);        
        Task<Endereco[]> GetAllEnderecoByUserId(int UserId);             

        //ESTADO
        Task<Estado[]> GetAllEstado();        
        Task<Estado> GetAllEstadoById(int Id);        
        Task<Estado> GetAllEstadoByNome(string Nome);        
        Task<Estado> GetAllEstadoByUf(string Uf);     

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
        Task<Venda> GetAllVendaByUserId(int UserId);       
        Task<Venda> GetAllVendaByStatusId(int StatusId);       
    }
}