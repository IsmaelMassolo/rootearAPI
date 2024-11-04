using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rootearAPI.Data;
using rootearAPI.Models;
using rootearAPI.Models.DTO;

namespace rootearAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleReservaController : Controller
    {
        private readonly apiContext _context;

        public DetalleReservaController(apiContext context)
        {
            _context = context;
        }


        [HttpGet("Reserva/{idSala}")]
        public async Task<IActionResult> GetViajesEnReserva(int idSala)
        {
            // Verifica si el usuario tiene un carrito activo
            var salaReserva = await _context.SALA_RESERVA
                .FirstOrDefaultAsync(sr => sr.IdSalaReserva == idSala && sr.Estado == true);

            if (salaReserva == null)
            {
                return NotFound("No se encontró una Sala de Reserva asociada");
            }

            // Filtra primero en DETALLE_RESERVA para obtener los Ids de los Viajes relacionados
            var idsViajes = await _context.DETALLE_RESERVA
                .Where(idv => idv.IdSalaReserva == salaReserva.IdSalaReserva)
                .Select(idv => idv.IdViaje)
                .ToListAsync();

            // Luego, filtra en VIAJE usando los IDs obtenidos y aplica los Includes y filtros adicionales
            var lista = await _context.VIAJE
                .Include(v => v.Origen)
                .Include(v => v.Destino)
                .Include(v => v.UsuarioCreador)
                .Include(v => v.UsuarioCreador.Vehiculo)
                .Where(v => idsViajes.Contains(v.IdViaje) &&  // Filtra los viajes con los IDs seleccionados
                            v.FechaSalida >= DateTime.Now &&
                            v.UsuarioCreador.Activo)
                .ToListAsync();

            return Ok(lista);
        }

        [HttpPost("Agregar")]
        public async Task<IActionResult> AgregarViaje([FromBody] DetalleReservaDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("El objeto es nulo.");
                }
                var viaje = await _context.VIAJE.FindAsync(dto.IdViaje);

                if (viaje == null)
                {
                    return BadRequest("Viaje no encontrado");
                }

                // Verificar si el producto ya está en el carrito
                var viajeExistente = await _context.DETALLE_RESERVA
                    .FirstOrDefaultAsync(dr => dr.IdSalaReserva == dto.IdSalaReserva && dr.IdViaje == viaje.IdViaje);

                if (viajeExistente != null) //si existe en la sala de reserva...
                {
                    viaje.CantButacas -= 1; // Actualizar el stock del producto
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var nuevoViajeEnReserva = new DetalleReserva
                    {
                        IdSalaReserva = dto.IdSalaReserva,
                        IdViaje = dto.IdViaje,
                        FechaAgregado = DateTime.Now
                    };

                    var nuevoViajeEnHistorial = new ViajeUsuario
                    {
                        IdViaje = dto.IdViaje,
                        IdUsuario = dto.IdUsuario
                    };

                    await _context.DETALLE_RESERVA.AddAsync(nuevoViajeEnReserva);
                    await _context.SaveChangesAsync();

                    await _context.VIAJE_USUARIO.AddAsync(nuevoViajeEnHistorial);
                    await _context.SaveChangesAsync();

                    viaje.CantButacas -= 1;
                    await _context.SaveChangesAsync();
                }

                var viajesEnReserva = await _context.DETALLE_RESERVA
                    .Where(ver => ver.IdSalaReserva == dto.IdSalaReserva)
                    .ToListAsync();

                if (!viajesEnReserva.Any())
                {
                    return BadRequest("No tienes viajes penientes! rooteá!");
                }

                return Ok("Viaje agregado a la Sala de Reserva");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("Eliminar")]
        public async Task<IActionResult> EliminarViaje([FromBody] DetalleReservaDTO dto)
        {
            try
            {
                var viajeEnReserva = await _context.DETALLE_RESERVA
                    .FirstOrDefaultAsync(d => d.IdViaje == dto.IdViaje && d.IdSalaReserva == dto.IdSalaReserva);

                if (viajeEnReserva != null)
                {
                    var viaje = await _context.VIAJE
                        .FirstOrDefaultAsync(d => d.IdViaje == dto.IdViaje);
                    viaje.CantButacas += 1;
                    await _context.SaveChangesAsync();

                    _context.DETALLE_RESERVA.Remove(viajeEnReserva);
                    await _context.SaveChangesAsync();

                    var viajeEnHistorial = await _context.VIAJE_USUARIO
                        .FirstOrDefaultAsync(d => d.IdViaje == dto.IdViaje && d.IdUsuario == dto.IdUsuario);

                    _context.VIAJE_USUARIO.Remove(viajeEnHistorial);
                    await _context.SaveChangesAsync();
                }
                return NoContent();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}