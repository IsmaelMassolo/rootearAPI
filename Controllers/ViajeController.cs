using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using rootearAPI.Data;
using rootearAPI.Models;
using rootearAPI.Models.DTO;

namespace rootear.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViajeController : Controller
    {
        private readonly apiContext _context;

        public ViajeController(apiContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "ObtenerTodos")]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var lista = await _context.VIAJE
                    .Include(v=> v.Origen)
                    .Include(v=> v.Destino)
                    .Include(v=> v.UsuarioCreador)
                    .Include (v=> v.UsuarioCreador.Vehiculo)
                    .Where(v => v.FechaSalida >= DateTime.Now &&
                           v.CantButacas > 0 &&
                           v.UsuarioCreador.Activo)
                    .ToListAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("ObtenerPorId/{IdViaje:int}")]
        public async Task<IActionResult> ObtenerPorId([FromRoute(Name = "IdViaje")] int id)
        {
            try
            {
                var item = await _context.VIAJE.FirstOrDefaultAsync(x => x.IdViaje == id);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] ViajeDTO crearViaje)
        {
            try
            {
                var viaje = new Viaje
                {
                    IdOrigen = crearViaje.IdOrigen,
                    IdDestino = crearViaje.IdDestino,
                    IdUsuarioCreador = crearViaje.IdUsuarioCreador,
                    FechaSalida = crearViaje.FechaSalida,
                    FechaArribo = crearViaje.FechaArribo,
                    CantButacas = crearViaje.CantButacas,
                    ActivoDesde = crearViaje.ActivoDesde,
                    Estado = true
                };

                await _context.VIAJE.AddAsync(viaje);
                await _context.SaveChangesAsync();

                if (viaje.IdViaje == 0)
                {
                    return BadRequest("El Viaje no fue generado correctamente.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{IdViaje:int}")]
        public async Task<IActionResult> Borrar([FromRoute] int IdViaje)
        {
            try
            {
                var viajeExistente = await _context.VIAJE.FindAsync(IdViaje);
                if (viajeExistente != null)
                {
                    _context.VIAJE.Remove(viajeExistente);
                    await _context.SaveChangesAsync();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}