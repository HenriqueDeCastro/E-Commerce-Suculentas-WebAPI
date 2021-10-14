namespace Suculentas.WebApi.Dtos
{
    public class UpdateUserRoleDTO
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public bool Delete { get; set; }
    }
}