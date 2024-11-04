using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rootearAPI.Data;

namespace rootearAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViajeUsuarioController : Controller
    {
        private readonly apiContext _context;

        public ViajeUsuarioController(apiContext context)
        {
            _context = context;
        }


        [HttpGet("Obtener/{idUsuario}")]
        public async Task<IActionResult> GetViajesEnReserva(int idUsuario)
        {

            var historial = await _context.VIAJE_USUARIO
                .Where(idv => idv.IdUsuario == idUsuario)
                .Select(idv => idv.IdViaje)
                .ToListAsync();

            if (historial == null)
            {
                return NotFound("No se encontró ningún viaje");
            }

             var lista = await _context.VIAJE
                .Include(v => v.Origen)
                .Include(v => v.Destino)
                .Include(v => v.UsuarioCreador)
                .Include(v => v.UsuarioCreador.Vehiculo)
                .Where(v => historial.Contains(v.IdViaje) && 
                       v.FechaSalida < DateTime.Now)
                .ToListAsync();

            return Ok(lista);
        }
    }
}
