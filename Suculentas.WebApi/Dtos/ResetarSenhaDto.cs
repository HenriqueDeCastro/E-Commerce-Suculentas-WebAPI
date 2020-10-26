using System.ComponentModel.DataAnnotations;

namespace Suculentas.WebApi.Dtos
{
    public class ResetarSenhaDto
    {
        public string Token { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmedPassword { get; set; }
    }
}