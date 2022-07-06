using Api1JwtBearerCurso.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api1JwtBearerCurso.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;

        public CuentasController(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager
            )
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }


        [HttpPost("Registrar")]
        public async Task<ActionResult<RespuestaToken>> Registrar(RegistroUsuario registro)
        {
            var usuario = new IdentityUser { UserName = registro.Nombre, Email = registro.Email };

            var resultado = await userManager.CreateAsync(usuario, registro.Password);

            if (resultado.Succeeded)
            {
                //aki retorno el token
                return await ConstruirToken(registro,null);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }



        [HttpPost("loguin")]
        public async Task<ActionResult<RespuestaToken>> Loguearse(CredencialesUsuario credencialesUsuario)
        {
           

            var resultado = await signInManager.PasswordSignInAsync(credencialesUsuario.Nombre
                ,credencialesUsuario.Password,isPersistent: false,lockoutOnFailure:false);

            if (resultado.Succeeded)
            {
                //aki retorno el token
                return await ConstruirToken(null,credencialesUsuario);
            }
            else
            {
                return BadRequest("Login Incorrecto");
            }
        }


        private async Task<ActionResult<RespuestaToken>> ConstruirToken(RegistroUsuario registro,CredencialesUsuario credencial)
        {
            var claims = new List<Claim>() { };
            dynamic usuario = null;
            if (credencial==null)
            {
                //mandamos registro
                claims.Add(new Claim("email", registro.Email));
                claims.Add(new Claim("nombre", registro.Nombre));
              
                claims.Add(new Claim("cualquiera", "valor lo quesea"));
                 usuario = await userManager.FindByNameAsync(registro.Nombre);
            }

            if (registro==null)
            {

                //mandamos credencia
              
                claims.Add(new Claim("nombre", credencial.Nombre));
                claims.Add(new Claim("cualquiera", "valor lo quesea"));
                usuario = await userManager.FindByNameAsync(credencial.Nombre);
            }

            string nombreUsuario =credencial!=null?credencial.Nombre:registro.Nombre;

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llaveJwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddDays(1);

            //contruimos el token
            var seguridadToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims
                , expires: expiracion, signingCredentials: creds);

            return new RespuestaToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(seguridadToken),
                Caducidad=expiracion,
                Usuario=nombreUsuario
            };

        }
        

        
    }
}
