using Api1JwtBearerCurso.DbContextClass;
using Api1JwtBearerCurso.Modelo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api1JwtBearerCurso.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ComentariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ComentariosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Comentario>>> Get()
        {
            var resultado= await context.Comentarios.ToListAsync();


            return Ok(resultado);

        }

        //[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Comentario>> Get(int id)
        {
            var resultado = await context.Comentarios.FindAsync(id);

            if (resultado==null)
            {
                return NotFound("Comentario no encontrado");
            }

            return Ok(resultado);

        }


        [HttpPost]
        public async Task<ActionResult<Comentario>> Post(Comentario comentario)
        {

            var resultado = await context.Comentarios.AnyAsync(comentarioDb => comentario.Id == comentarioDb.Id);

            if (resultado)
            {
                return BadRequest("El comentario con ese id ya existe");

            }

            await context.AddAsync(comentario);

            await context.SaveChangesAsync();


            return Ok(comentario);

        }





    }
}
