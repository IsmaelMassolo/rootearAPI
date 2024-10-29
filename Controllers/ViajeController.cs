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
                var lista = await _context.VIAJE.ToListAsync();
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
                // Crear un nuevo objeto Viaje
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

        //[HttpPost("GuardarImagen")]
        //public async Task<IActionResult> GuardarImagen([FromForm] IFormFile archivo)
        //{
        //    if (archivo == null || archivo.Length == 0)
        //    {
        //        return BadRequest("No se ha subido ningún archivo.");
        //    }

        //    var rutaGuardado = Path.Combine("wwwroot\\ImagenesViajes", archivo.FileName);

        //    using (var stream = new FileStream(rutaGuardado, FileMode.Create))
        //    {
        //        await archivo.CopyToAsync(stream);
        //    }

        //    return Ok("Imagen guardada correctamente.");
        //}


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

        //[HttpPut("{IdViaje:int}")]
        //public async Task<IActionResult> Modificar([FromBody] ModificarViajeDTO modificarViaje, [FromRoute] int IdViaje)
        //{
        //    try
        //    {
        //        var viajeExistente = await _context.VIAJE.FindAsync(IdViaje);

        //        if (viajeExistente != null)
        //        {
        //            if (!modificarViaje.Titulo.IsNullOrEmpty()) viajeExistente.Titulo = modificarViaje.Titulo;
        //            if (!modificarViaje.Autor.IsNullOrEmpty()) viajeExistente.Autor = modificarViaje.Autor;
        //            if (modificarViaje.AnioEdicion != null) viajeExistente.AnioEdicion = modificarViaje.AnioEdicion;
        //            if (!modificarViaje.Editorial.IsNullOrEmpty()) viajeExistente.Editorial = modificarViaje.Editorial;
        //            if (!modificarViaje.Sinopsis.IsNullOrEmpty()) viajeExistente.Sinopsis = modificarViaje.Sinopsis;
        //            if (!modificarViaje.Idioma.IsNullOrEmpty()) viajeExistente.Idioma = modificarViaje.Idioma;
        //            if (modificarViaje.CantPaginas != null) viajeExistente.CantPaginas = modificarViaje.CantPaginas;
        //            if (!modificarViaje.ISBN.IsNullOrEmpty()) viajeExistente.ISBN = modificarViaje.ISBN;
        //            if (!modificarViaje.Estado.IsNullOrEmpty()) viajeExistente.Estado = modificarViaje.Estado;
        //            if (modificarViaje.Stock != null) viajeExistente.Stock = modificarViaje.Stock;
        //            if (!modificarViaje.Formato.IsNullOrEmpty()) viajeExistente.Formato = modificarViaje.Formato;
        //            if (modificarViaje.Precio != null) viajeExistente.Precio = modificarViaje.Precio;
        //            if (modificarViaje.Cuotas != null) viajeExistente.Cuotas = modificarViaje.Cuotas;
        //            if (!modificarViaje.Envio.IsNullOrEmpty()) viajeExistente.Envio = modificarViaje.Envio;
        //            if (modificarViaje.Habilitado != null) viajeExistente.Habilitado = modificarViaje.Habilitado;

        //            //if (!modificarViaje.RutaImagen.IsNullOrEmpty()) viajeExistente.RutaImagen = modificarViaje.RutaImagen;
        //            _context.VIAJE.Update(viajeExistente);
        //            await _context.SaveChangesAsync();
        //        }

        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}