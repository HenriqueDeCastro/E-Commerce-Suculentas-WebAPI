using System.ComponentModel.DataAnnotations;

namespace Suculentas.WebApi.Dtos
{
    public class EsqueciSenhaDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}