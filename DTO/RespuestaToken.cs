namespace Api1JwtBearerCurso.DTO
{
    public class RespuestaToken
    {
        public string Token { get; set; }
        public DateTime Caducidad { get; set; }

        public string Usuario { get; set; }
    }
}
