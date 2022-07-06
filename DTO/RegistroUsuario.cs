using System.ComponentModel.DataAnnotations;

namespace Api1JwtBearerCurso.DTO
{
    public class RegistroUsuario
    {

        public string Nombre { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
