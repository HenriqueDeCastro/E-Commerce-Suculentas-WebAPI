using System.ComponentModel.DataAnnotations;

namespace Suculentas.WebApi.Dtos
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}