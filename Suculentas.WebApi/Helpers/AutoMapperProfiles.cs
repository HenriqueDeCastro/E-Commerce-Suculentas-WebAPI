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
            CreateMap<Endereco, EnderecoDto>().ReverseMap();
            CreateMap<LogEmail, LogEmailDto>().ReverseMap();
            CreateMap<Gastos, GastosDto>().ReverseMap();
            CreateMap<LogExcecao, LogExcecaoDto>().ReverseMap();
            CreateMap<Pedido, PedidoDto>().ReverseMap();
            CreateMap<Produto, ProdutoDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Status, StatusDto>().ReverseMap();
            CreateMap<TipoProduto, TipoProdutoDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<Venda, VendaDto>().ReverseMap();
        }
    }
}