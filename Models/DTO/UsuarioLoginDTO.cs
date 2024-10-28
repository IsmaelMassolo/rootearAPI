using System.ComponentModel.DataAnnotations;

namespace rootearAPI.Models.DTO
{
    public class UsuarioLoginDTO
    {
        public string Contrasena { get; set; }

        public string UsuarioNombre { get; set; }
    }
}
