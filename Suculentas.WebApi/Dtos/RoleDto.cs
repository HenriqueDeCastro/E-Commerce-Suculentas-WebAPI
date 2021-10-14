using System.Collections.Generic;
using Suculentas.Domain.Identity;

namespace Suculentas.WebApi.Dtos
{
    public class RoleDTO
    {
        public string Name { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}