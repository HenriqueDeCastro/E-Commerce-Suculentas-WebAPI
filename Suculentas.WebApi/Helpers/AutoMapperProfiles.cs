using AutoMapper;
using Suculentas.Domain;
using Suculentas.Domain.Identity;
using Suculentas.WebApi.Dtos;

namespace Suculentas.WebApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Cidade, CidadeDto>().ReverseMap();
            CreateMap<EmpresaFrete, EmpresaFreteDto>().ReverseMap();
            CreateMap<Endereco, EnderecoDto>().ReverseMap();
            CreateMap<Estado, EstadoDto>().ReverseMap();
            CreateMap<Gastos, GastosDto>().ReverseMap();
            CreateMap<Pedido, PedidoDto>().ReverseMap();
            CreateMap<Produto, ProdutoDto>().ReverseMap();
            CreateMap<Status, StatusDto>().ReverseMap();
            CreateMap<TipoProduto, TipoProdutoDto>().ReverseMap();
            CreateMap<Venda, VendaDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
        }
    }
}