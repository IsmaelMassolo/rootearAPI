using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using rootearAPI.Data;
using rootearAPI.Models;
using rootearAPI.Models.DTO;

namespace rootearAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly apiContext _context;

        public UsuarioController(apiContext context)
        {
            _context = context;
        }

        
        [HttpGet(Name = "GetUser")]
        public async Task<IActionResult> Obtener()
        {
            try
            {
                var lista = await _context.USUARIO
                           .Include(u => u.Lugar) // Cargar la relación con Lugar
                           .Include(u => u.Vehiculo) // Cargar la relación con Vehiculo
                           .ToListAsync();
                return Ok(lista);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

       
        [HttpGet("ObtenerPorId/{IdUsuario:int}")]
        public async Task<IActionResult> ObtenerPorId([FromRoute(Name = "IdUsuario")] int id)
        {
            try
            {
                var item = await _context.USUARIO.FindAsync(id);
                return Ok(item);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] UsuarioDTO nuevoUsuarioDTO)
        {
            try
            {
                var salaReserva = new SalaReserva
                {
                    FechaCreacion = DateTime.Now,
                    Estado = true
                };

                await _context.SALA_RESERVA.AddAsync(salaReserva);
                await _context.SaveChangesAsync();

                var nuevoUsuario = new Usuario()
                {
                    Nombre = nuevoUsuarioDTO.Nombre,
                    Apellido = nuevoUsuarioDTO.Apellido,
                    UsuarioNombre = nuevoUsuarioDTO.UsuarioNombre,
                    Contrasena = nuevoUsuarioDTO.Contrasena,
                    Email = nuevoUsuarioDTO.Email,
                    Celular = nuevoUsuarioDTO.Celular,
                    Dni = nuevoUsuarioDTO.Dni,
                    FechaNac = nuevoUsuarioDTO.FechaNac,
                    Edad = (DateTime.Now.Year - nuevoUsuarioDTO.FechaNac.Year),
                    Genero = nuevoUsuarioDTO.Genero,
                    Rol = nuevoUsuarioDTO.Rol,
                    Activo = true,
                    RutaImagen = "http://localhost:5161/Imagenes/Usuarios/" + nuevoUsuarioDTO.UsuarioNombre + ".png",
                    ActivoDesde = DateTime.Now,
                    IdLugar = nuevoUsuarioDTO.idLugar,
                    IdSalaReserva = salaReserva.IdSalaReserva
                };
                await _context.USUARIO.AddAsync(nuevoUsuario);
                await _context.SaveChangesAsync();

                // Verificar si el ID de usuario fue generado
                if (nuevoUsuario.IdUsuario == 0)
                {
                    // Si no hay IdUsuario, lanzar una excepción
                    return BadRequest("El Id del usuario no fue generado correctamente.");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("GuardarImagen")]
        public async Task<IActionResult> GuardarImagen([FromForm] IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                return BadRequest("No se ha subido ningún archivo.");
            }

            var rutaGuardado = Path.Combine("wwwroot\\Imagenes\\Usuarios", archivo.FileName);

            using (var stream = new FileStream(rutaGuardado, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            return Ok("Imagen guardada correctamente.");
        }


        [HttpPut("Deshabilitar/{IdUsuario:int}")]
        public async Task<IActionResult> Deshabilitar([FromRoute] int IdUsuario)
        {
            try
            {
                var usuarioExistente = await _context.USUARIO.FindAsync(IdUsuario);

                if (usuarioExistente != null)
                {
                    usuarioExistente.Activo = false;
                    await _context.SaveChangesAsync();
                }
                return NoContent();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }


        [HttpPut("Modificar/{IdUsuario:int}")]
        public async Task<IActionResult> Modificar([FromBody] UsuarioDTO usuarioDto, [FromRoute] int IdUsuario)
        {
            try
            {
                var usuarioExistente = await _context.USUARIO.FindAsync(IdUsuario);
                if (usuarioExistente != null)
                {
                    if (!string.IsNullOrEmpty(usuarioDto.Nombre)) usuarioExistente.Nombre = usuarioDto.Nombre;
                    if (!string.IsNullOrEmpty(usuarioDto.Apellido)) usuarioExistente.Apellido = usuarioDto.Apellido;
                    if (!string.IsNullOrEmpty(usuarioDto.UsuarioNombre)) usuarioExistente.UsuarioNombre = usuarioDto.UsuarioNombre;
                    if (!string.IsNullOrEmpty(usuarioDto.Contrasena)) usuarioExistente.Contrasena = usuarioDto.Contrasena;
                    if (!string.IsNullOrEmpty(usuarioDto.Email)) usuarioExistente.Email = usuarioDto.Email;
                    if (!string.IsNullOrEmpty(usuarioDto.Celular)) usuarioExistente.Celular = usuarioDto.Celular;
                    if (!string.IsNullOrEmpty(usuarioDto.Dni)) usuarioExistente.Dni = usuarioDto.Dni;
                    if (usuarioDto.FechaNac != null) usuarioExistente.FechaNac = usuarioDto.FechaNac;
                    if (usuarioDto.Edad != null) usuarioExistente.Edad = (DateTime.Now.Year - usuarioDto.FechaNac.Year);
                    if (!string.IsNullOrEmpty(usuarioDto.Genero)) usuarioExistente.Genero = usuarioDto.Genero;
                    if (!string.IsNullOrEmpty(usuarioDto.Rol)) usuarioExistente.Rol = usuarioDto.Rol;
                    if (usuarioDto.Activo != null) usuarioExistente.Activo = usuarioDto.Activo;
                    if (!string.IsNullOrEmpty(usuarioDto.RutaImagen)) usuarioExistente.RutaImagen = usuarioDto.RutaImagen;

                    // Buscar el IdLugar en función de la ciudad y provincia
                    //if (!string.IsNullOrEmpty(usuarioDto.Ciudad) && !string.IsNullOrEmpty(usuarioDto.Provincia))
                    //{
                    //    var lugar = await _context.LUGAR
                    //        .FirstOrDefaultAsync(x => x.Ciudad == usuarioDto.Ciudad && x.Provincia == usuarioDto.Provincia);

                    //    if (lugar != null)
                    //    {
                    //        usuarioExistente.IdLugar = lugar.IdLugar; // Asignar el IdLugar encontrado
                    //    }
                    //    else
                    //    {
                    //        return BadRequest("El lugar no existe."); // Manejar el caso si no se encuentra el lugar
                    //    }
                    //}

                    _context.USUARIO.Update(usuarioExistente);
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                return NotFound("Usuario no encontrado.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("ValidarCredencial")]
        public async Task<IActionResult> ValidarCredencial([FromBody] UsuarioLoginDTO usuario)
        {
            var existeLogin = await _context.USUARIO
                .AnyAsync(x => x.UsuarioNombre.Equals(usuario.UsuarioNombre) && x.Contrasena.Equals(usuario.Contrasena));

            Usuario usuarioLogin = await _context.USUARIO
            .Include(u => u.Lugar)
            .Include(u => u.Vehiculo)
            .FirstOrDefaultAsync(x => x.UsuarioNombre.Equals(usuario.UsuarioNombre) && x.Contrasena.Equals(usuario.Contrasena));


            if (!existeLogin)
            {
                return NotFound("Usuario No Existe");
            }

            if (!usuarioLogin.Activo)
            {
                return BadRequest("El usuario no está activo y no puede acceder a la aplicación.");
            }

            SalaReserva salaReserva = await _context.SALA_RESERVA.FirstOrDefaultAsync(x => x.IdSalaReserva.Equals(usuarioLogin.IdSalaReserva));

            LoginResponseDTO loginReponse = new LoginResponseDTO()
            {
                IdUsuario = existeLogin ? usuarioLogin.IdUsuario : 0,
                Nombre = existeLogin ? usuarioLogin.Nombre : "",
                Apellido = existeLogin ? usuarioLogin.Apellido : "",
                UsuarioNombre = existeLogin ? usuarioLogin.UsuarioNombre : "",
                Email = existeLogin ? usuarioLogin.Email : "",
                Direccion = existeLogin ?
                        usuarioLogin.Lugar.Ciudad + ", " +
                        usuarioLogin.Lugar.Provincia : "",
                Rol = existeLogin ? usuarioLogin.Rol: "Sin Rol",
                IdVehiculo = existeLogin ? usuarioLogin.IdVehiculo : 0,
                IdSalaReserva = existeLogin ? usuarioLogin.IdSalaReserva : 0
            };
            return Ok(loginReponse);
        }
    }
}
