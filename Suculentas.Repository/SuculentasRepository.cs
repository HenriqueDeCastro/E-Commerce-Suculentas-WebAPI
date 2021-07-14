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

        //CATEGORIA
        public async Task<CategoriaPagination> GetAllCategoriaByCliente(int Id, int pageAtual, string orderBy, string search)
        {
            CategoriaPagination categoriaPagination = new CategoriaPagination();

            int paginacao = Int16.Parse(_config.GetSection("AppSettings:PaginacaoProdutos").Value);
            int skip = pageAtual * paginacao;

            IQueryable<Categoria> queryCategorias = _Context.Categorias.Where(c => c.Id == Id);
            IQueryable<Produto> queryProdutos = _Context.Produtos
                .Where(p => p.CategoriaId == Id && p.Ativo == true
                        && (p.TipoProdutoId == Int16.Parse(_config.GetSection("TipoProduto:Encomenda").Value) ? true : p.Estoque > 0));

            if(search != null && search != "")
            {
                queryProdutos = queryProdutos.Where(p => p.Nome.ToUpper().Contains(search.ToUpper()));
            }

            categoriaPagination.UltimaPagina = (skip + paginacao) < queryProdutos.Count() ? false : true;

            if (orderBy == _config.GetSection("OrderBy:Alfabetica").Value)
            {
                queryProdutos = queryProdutos.OrderBy(p => p.Nome);
            }
            else if (orderBy == _config.GetSection("OrderBy:PrecoMaior").Value)
            {
                queryProdutos = queryProdutos.OrderByDescending(p => p.Preco);
            }
            else if (orderBy == _config.GetSection("OrderBy:PrecoMenor").Value)
            {
                queryProdutos = queryProdutos.OrderBy(p => p.Preco);
            }
            else
            {
                queryProdutos = queryProdutos.OrderByDescending(p => p.Id);
            }

            queryProdutos = queryProdutos
                .Skip(skip).Take(paginacao);

            categoriaPagination.Categoria = await queryCategorias.FirstOrDefaultAsync();
            categoriaPagination.Produtos = await queryProdutos.ToArrayAsync(); 

            return categoriaPagination;
        }

        public async Task<CategoriaPagination> GetAllCategoriaByEmpresa(int Id, int pageAtual, string orderBy, string search)
        {
            CategoriaPagination categoriaPagination = new CategoriaPagination();

            int paginacao = Int16.Parse(_config.GetSection("AppSettings:PaginacaoProdutos").Value);
            int skip = pageAtual * paginacao;

            IQueryable<Categoria> queryCategorias = _Context.Categorias.Where(c => c.Id == Id);
            IQueryable<Produto> queryProdutos = _Context.Produtos.Where(p => p.CategoriaId == Id);

            if (search != null && search != "")
            {
                queryProdutos = queryProdutos.Where(p => p.Nome.ToUpper().Contains(search.ToUpper()));
            }

            categoriaPagination.UltimaPagina = (skip + paginacao) < queryProdutos.Count() ? false : true;

            if (orderBy == _config.GetSection("OrderBy:Alfabetica").Value)
            {
                queryProdutos = queryProdutos.OrderBy(p => p.Nome);
            }
            else if (orderBy == _config.GetSection("OrderBy:PrecoMaior").Value)
            {
                queryProdutos = queryProdutos.OrderByDescending(p => p.Preco);
            }
            else if (orderBy == _config.GetSection("OrderBy:PrecoMenor").Value)
            {
                queryProdutos = queryProdutos.OrderBy(p => p.Preco);
            }
            else if (orderBy == _config.GetSection("OrderBy:Estoque").Value)
            {
                queryProdutos = queryProdutos.Where(p => p.TipoProdutoId == Int16.Parse(_config.GetSection("TipoProduto:Estoque").Value)).OrderBy(p => p.Estoque);
            }
            else
            {
                queryProdutos = queryProdutos.OrderByDescending(p => p.Id);
            }

            queryProdutos = queryProdutos
                .Skip(skip).Take(paginacao);

            categoriaPagination.Categoria = await queryCategorias.FirstOrDefaultAsync();
            categoriaPagination.Produtos = await queryProdutos.ToArrayAsync();

            return categoriaPagination;
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
                Produtos = _Context.Produtos.Where(p => p.CategoriaId == c.Id && p.Ativo == true 
                                                && (p.TipoProdutoId == Int16.Parse(_config.GetSection("TipoProduto:Encomenda").Value) ? true : p.Estoque > 0))
                .OrderByDescending(p => p.Id).Take(4).ToList()});

            categorias = categorias.Where(c => c.Ativo == true).OrderBy(c => c.Nome);

            return await categorias.ToArrayAsync();
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
        
        // GASTOS 
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
            IQueryable<TipoProduto> query = _Context.TipoProdutos
                .Include(c => c.Produtos);

            query = query.OrderBy(c => c.Nome);

            return await query.ToArrayAsync();        
        }

        public async Task<TipoProduto[]> GetAllTipoProdutoSemProduto()
        {
            IQueryable<TipoProduto> query = _Context.TipoProdutos;

            query = query.OrderBy(c => c.Nome);

            return await query.ToArrayAsync();        
        }

        public async Task<TipoProduto> GetAllTipoProdutoById(int Id)
        {
            IQueryable<TipoProduto> query = _Context.TipoProdutos
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
                .Include(c => c.Pedidos)
                .ThenInclude(c => c.Produto);

            query = query.Where(c => c.Id == Id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Venda> GetAllVendaByIdSemInclude(int Id)
        {
            IQueryable<Venda> query = _Context.Vendas;

            query = query.Where(c => c.Id == Id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Venda> GetAllVendaByIdEmpresa(int Id)
        {
            IQueryable<Venda> query = _Context.Vendas
                .Include(c => c.User)
                .Include(c => c.Pedidos)
                    .ThenInclude(c => c.Produto);

            query = query.Where(c => c.Id == Id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<VendaPagination> GetAllVendaByStatusId(int StatusId, int pageAtual)
        {
            IQueryable<Venda> queryVendas = _Context.Vendas.Where(c => c.StatusId == StatusId).OrderBy(c => c.DataVenda);
            VendaPagination vendasPagination = new VendaPagination();

            int paginacao = Int16.Parse(_config.GetSection("AppSettings:PaginacaoVendas").Value);
            int skip = pageAtual * paginacao;

            vendasPagination.UltimaPagina = (skip + paginacao) < queryVendas.Count()? false : true;

            queryVendas = queryVendas.Include(c => c.Pedidos).ThenInclude(c => c.Produto).Skip(skip).Take(paginacao);
            vendasPagination.Vendas = await queryVendas.ToArrayAsync();

            return vendasPagination;
        }

        public async Task<VendaPagination> GetAllVendaByUserId(int UserId, int StatusId, int pageAtual)
        {
            IQueryable<Venda> queryVendas = _Context.Vendas.Where(c => c.UserId == UserId && c.StatusId == StatusId).OrderBy(c => c.DataVenda);
            VendaPagination vendasPagination = new VendaPagination();

            int paginacao = Int16.Parse(_config.GetSection("AppSettings:PaginacaoVendas").Value);
            int skip = pageAtual * paginacao;

            vendasPagination.UltimaPagina = (skip + paginacao) < queryVendas.Count() ? false : true;

            queryVendas = queryVendas.Include(c => c.Pedidos).ThenInclude(c => c.Produto).Skip(skip).Take(paginacao);
            vendasPagination.Vendas = await queryVendas.ToArrayAsync();

            return vendasPagination;
        }

        public async Task<VendaStatusCount[]> GetAllVendaCountStatusEmpresa()
        {
            IQueryable<Venda> queryVendas = _Context.Vendas;

            IQueryable<VendaStatusCount> query = queryVendas.GroupBy(x => x.StatusId)
                .Select(x => new VendaStatusCount { StatusId = x.Key, CountVenda = x.Count() });

            query = query.OrderByDescending(c => c.StatusId);

            return await query.ToArrayAsync();
        }

        public async Task<VendaStatusCount[]> GetAllVendaCountStatusUser(int UserId)
        {
            IQueryable<Venda> queryVendas = _Context.Vendas.Where(v => v.UserId == UserId);

            IQueryable<VendaStatusCount> query = queryVendas.GroupBy(x => x.StatusId)
                .Select(x => new VendaStatusCount { StatusId = x.Key, CountVenda = x.Count() });

            query = query.OrderByDescending(c => c.StatusId);

            return await query.ToArrayAsync();
        }
    }
}