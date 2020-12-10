using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Suculentas.Domain;

namespace Suculentas.Repository
{
    public class SuculentasRepository : ISuculentasRepository
    {
        private readonly SuculentasContext _Context;

        //GERAL
        public SuculentasRepository(SuculentasContext context)
        {
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

        //CATEGORIA
        public async Task<Categoria> GetAllCategoriaByIdCliente(int Id)
        {
            IQueryable<Categoria> categorias = _Context.Categorias.Select(c => new Categoria
            {
                Id = c.Id,
                Nome = c.Nome,
                Descricao = c.Descricao,
                Ativo = c.Ativo,
                Produtos = _Context.Produtos.Where(p => p.CategoriaId == c.Id && p.Ativo == true && (p.TipoProdutoId == 1? true : p.Estoque > 0)).OrderByDescending(p => p.Id).ToList()
            });

            categorias = categorias.Where(c => c.Id == Id);

            return await categorias.FirstOrDefaultAsync();
        }

        public async Task<Categoria> GetAllCategoriaByIdEmpresa(int Id)
        {
            IQueryable<Categoria> query = _Context.Categorias
                .Include(c => c.Produtos);

            query = query.OrderBy(c => c.Id)
                .Where(c => c.Id == Id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Categoria> GetAllCategoriaById(int Id)
        {
            IQueryable<Categoria> query = _Context.Categorias;

            query = query.OrderByDescending(c => c.Id)
                .Where(c => c.Id == Id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Categoria[]> GetAllCategorias()
        {
            IQueryable<Categoria> query = _Context.Categorias
                .Include(c => c.Produtos);

            query = query.OrderBy(c => c.Nome);

            return await query.ToArrayAsync();
        }

        
        public async Task<Categoria[]> GetAllCategoriasSemProdutos()
        {
            IQueryable<Categoria> query = _Context.Categorias;

            query = query.OrderBy(c => c.Nome);

            return await query.ToArrayAsync();    
        }

        public async Task<Categoria[]> GetAllCategoriasPagInicialEmpresa()
        {
            IQueryable<Categoria> categorias = _Context.Categorias.Select(c => new Categoria
            {
                Id = c.Id,
                Nome = c.Nome,
                Descricao = c.Descricao,
                Ativo = c.Ativo,
                Produtos = _Context.Produtos.Where(p => p.CategoriaId == c.Id).OrderByDescending(p => p.Id).Take(4).ToList()
            });

            categorias = categorias.OrderBy(c => c.Nome);

            return await categorias.ToArrayAsync();
        }

        public async Task<Categoria[]> GetAllCategoriasPagInicial()
        {
            IQueryable<Categoria> categorias = _Context.Categorias.Select(c => new Categoria {
                Id = c.Id,
                Nome = c.Nome,
                Descricao = c.Descricao,
                Ativo = c.Ativo,
                Produtos = _Context.Produtos.Where(p => p.CategoriaId == c.Id && p.Ativo == true && (p.TipoProdutoId == 1? true : p.Estoque > 0)).OrderByDescending(p => p.Id).Take(4).ToList()
            });

            categorias = categorias.Where(c => c.Ativo == true).OrderBy(c => c.Nome);

            return await categorias.ToArrayAsync();
        }

        //CIDADE
        public async Task<Cidade[]> GetAllCidadeByEstadoId(int EstadoId)
        {
            IQueryable<Cidade> query = _Context.Cidades;

            query = query.OrderByDescending(c => c.Nome)
                .Where(c => c.EstadoId == EstadoId);

            return await query.ToArrayAsync();
        }

        public async Task<Cidade> GetAllCidadeById(int Id)
        {
            IQueryable<Cidade> query = _Context.Cidades;

            query = query.OrderByDescending(c => c.Id)
                .Where(c => c.Id == Id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Cidade> GetAllCidadeByNome(string Nome)
        {
            IQueryable<Cidade> query = _Context.Cidades;

            query = query.OrderByDescending(c => c.Id)
                .Where(c => c.Nome.ToLower() == Nome.ToLower());

            return await query.FirstOrDefaultAsync();
        }

        //EMPRESA FRETE
        public async Task<EmpresaFrete> GetAllEmpresaById(int Id)
        {
            IQueryable<EmpresaFrete> query = _Context.EmpresaFretes;

            query = query.OrderByDescending(c => c.Id)
                .Where(c => c.Id == Id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<EmpresaFrete[]> GetAllEmpresaFrete()
        {
            IQueryable<EmpresaFrete> query = _Context.EmpresaFretes;

            query = query.OrderByDescending(c => c.Nome);

            return await query.ToArrayAsync();
        }

        //ENDEREÇO
        public async Task<Endereco> GetAllEnderecoById(int Id)
        {
            IQueryable<Endereco> query = _Context.Enderecos;

            query = query.OrderByDescending(c => c.Id)
                .Where(c => c.Id == Id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Endereco[]> GetAllEnderecoByUserId(int UserId)
        {
            IQueryable<Endereco> query = _Context.Enderecos;

            query = query.OrderByDescending(c => c.Descricao)
                .Where(c => c.UserId == UserId);

            return await query.ToArrayAsync();
        }

        //ESTADO
        public async Task<Estado[]> GetAllEstado()
        {
            IQueryable<Estado> query = _Context.Estados;

            query = query.OrderByDescending(c => c.UF);

            return await query.ToArrayAsync();
        }

        public async Task<Estado> GetAllEstadoById(int Id)
        {
            IQueryable<Estado> query = _Context.Estados
                .Include(c => c.Cidades);

            query = query.OrderByDescending(c => c.Id)
                .Where(c => c.Id == Id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Estado> GetAllEstadoByNome(string Nome)
        {
            IQueryable<Estado> query = _Context.Estados
                .Include(c => c.Cidades);

            query = query.OrderByDescending(c => c.Id)
                .Where(c => c.Nome.ToLower() == Nome.ToLower());

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Estado> GetAllEstadoByUf(string Uf)
        {
            IQueryable<Estado> query = _Context.Estados
                .Include(c => c.Cidades);

            query = query.OrderByDescending(c => c.Id)
                .Where(c => c.UF.ToLower() == Uf.ToLower());

            return await query.FirstOrDefaultAsync();
        }
        

        public async Task<Gastos[]> GetAllGastos()
        {
            IQueryable<Gastos> query = _Context.Gastos;

            query = query.OrderByDescending(c => c.Data);

            return await query.ToArrayAsync();
        }

        public async Task<Gastos> GetAllGastosById(int Id)
        {
            IQueryable<Gastos> query = _Context.Gastos;

            query = query.OrderByDescending(c => c.Data)
                .Where(c => c.Id == Id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Gastos[]> GetAllGastosByData(System.DateTime Data)
        {
            IQueryable<Gastos> query = _Context.Gastos;

            query = query.OrderByDescending(c => c.Data)
                .Where(c => c.Data >= Data);

            return await query.ToArrayAsync();
        }

        //PEDIDO
        public async Task<Pedido> GetAllPedidoById(int Id)
        {
            IQueryable<Pedido> query = _Context.Pedidos;

            query = query.OrderByDescending(c => c.Id)
                .Where(c => c.Id == Id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Pedido[]> GetAllPedidoByVendaId(int VendaId)
        {
            IQueryable<Pedido> query = _Context.Pedidos;

            query = query.OrderByDescending(c => c.Id)
                .Where(c => c.VendaId == VendaId);

            return await query.ToArrayAsync();
        }

        //PRODUTO
        public async Task<Produto> GetAllProdutoById(int Id)
        {
            IQueryable<Produto> query = _Context.Produtos;

            query = query.OrderByDescending(c => c.Id)
                .Where(c => c.Id == Id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Produto[]> GetAllProdutoByCategoriaId(int CategoriaId)
        {
            IQueryable<Produto> query = _Context.Produtos;

            query = query.OrderByDescending(c => c.Id)
                .Where(c => c.CategoriaId == CategoriaId);

            return await query.ToArrayAsync();
        }

        //STATUS
        public async Task<Status[]> GetAllStatus()
        {
            IQueryable<Status> query = _Context.Status;

            query = query.OrderByDescending(c => c.Id);

            return await query.ToArrayAsync();        
        }

        public async Task<Status[]> GetAllStatusById(int Id)
        {
            IQueryable<Status> query = _Context.Status;

            query = query.OrderByDescending(c => c.Id)
                .Where(c => c.Id == Id);

            return await query.ToArrayAsync();        
        }

        //TIPO PRODUTO
        public async Task<TipoProduto[]> GetAllTipoProduto()
        {
            IQueryable<TipoProduto> query = _Context.TipoProduto
                .Include(c => c.Produtos);

            query = query.OrderBy(c => c.Nome);

            return await query.ToArrayAsync();        
        }

        public async Task<TipoProduto[]> GetAllTipoProdutoSemProduto()
        {
            IQueryable<TipoProduto> query = _Context.TipoProduto;

            query = query.OrderBy(c => c.Nome);

            return await query.ToArrayAsync();        
        }

        public async Task<TipoProduto> GetAllTipoProdutoById(int Id)
        {
            IQueryable<TipoProduto> query = _Context.TipoProduto
                .Include(c => c.Produtos);

            query = query.OrderBy(c => c.Nome)
                .Where(c => c.Id == Id);

            return await query.FirstOrDefaultAsync();        
        }

        //VENDA
        public async Task<Venda[]> GetAllVenda()
        {
            IQueryable<Venda> query = _Context.Vendas;

            query = query.OrderByDescending(c => c.DataVenda);

            return await query.ToArrayAsync();
        }

        public async Task<Venda> GetAllVendaById(int Id)
        {
            IQueryable<Venda> query = _Context.Vendas
                .Include(c => c.Pedidos);

            query = query.OrderByDescending(c => c.DataVenda)
                .Where(c => c.Id == Id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Venda> GetAllVendaByStatusId(int StatusId)
        {
            IQueryable<Venda> query = _Context.Vendas;

            query = query.OrderByDescending(c => c.DataVenda)
                .Where(c => c.StatusId == StatusId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Venda> GetAllVendaByUserId(int UserId)
        {
            IQueryable<Venda> query = _Context.Vendas;

            query = query.OrderByDescending(c => c.DataVenda)
                .Where(c => c.UserId == UserId);

            return await query.FirstOrDefaultAsync();
        }
    }
}