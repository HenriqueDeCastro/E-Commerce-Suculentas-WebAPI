using System.Collections.Generic;
using Suculentas.Domain.Identity;

namespace Suculentas.WebApi.Dtos
{
    public class RoleDto
    {
        public string Name { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}