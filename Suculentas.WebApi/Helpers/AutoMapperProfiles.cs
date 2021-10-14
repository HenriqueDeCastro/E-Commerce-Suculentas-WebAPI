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
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<LogEmail, LogEmailDTO>().ReverseMap();
            CreateMap<LogException, LogExceptionDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Status, StatusDTO>().ReverseMap();
            CreateMap<ProductType, ProductTypeDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserLoginDTO>().ReverseMap();
            CreateMap<Sale, SaleDTO>().ReverseMap();
        }
    }
}