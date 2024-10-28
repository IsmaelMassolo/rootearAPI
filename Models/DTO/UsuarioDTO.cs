using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace rootearAPI.Models.DTO
{
    public class UsuarioDTO
    {
        public string? Apellido { get; set; }

        public string? Nombre { get; set; }

        public string? UsuarioNombre { get; set; }

        public string? Email { get; set; }

        public string? Contrasena { get; set; }

        public string? Celular { get; set; }

        public string? Dni { get; set; }

        public int Edad { get; set; }

        public DateTime FechaNac { get; set; }

        public string? Genero { get; set; }

        public string? Rol { get; set; }

        public bool Activo { get; set; } = true;

        public string? RutaImagen { get; set; }

        public DateTime ActivoDesde { get; set; } = DateTime.Now;
        
        public int idLugar { get; set; }

    }
}
