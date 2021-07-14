using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Suculentas.Domain.Identity;

namespace Suculentas.WebApi.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string CPF { get; set; }
        public string DataNascimento { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmedPassword { get; set; }
        public List<VendaDto> Vendas { get; set; }
        public List<EnderecoDto> Enderecos { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}