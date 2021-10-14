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
        public DateTime? BirthDate { get; set; }

        [Column(TypeName = "bit")]
        public Boolean AcceptTerms { get; set; }

        public List<UserRole> UserRoles { get; set; }
        public List<Sale> Sales { get; set; }
        public List<Address> Adresses { get; set; }
    }
}