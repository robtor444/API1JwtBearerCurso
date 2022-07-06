using System.ComponentModel.DataAnnotations;

namespace Api1JwtBearerCurso.DTO
{
    public class CredencialesUsuario
    {
        [Required]
        
        public string Nombre { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
