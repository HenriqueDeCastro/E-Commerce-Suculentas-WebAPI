using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Suculentas.Domain.Identity
{
    public class User : IdentityUser<int>
    {
        [Column(TypeName = "nvarchar(100)")]
        public string FullName { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string CPF { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DataNascimento { get; set; }

        public List<UserRole> UserRoles { get; set; }
        public List<Venda> Vendas { get; set; }
        public List<Endereco> Enderecos { get; set; }
    }
}