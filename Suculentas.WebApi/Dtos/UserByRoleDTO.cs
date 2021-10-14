using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suculentas.WebApi.Dtos
{
    public class UserByRoleDTO
    {
        public string RoleName { get; set; }
        public List<UserDTO> Users { get; set; }
    }
}
