using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rootearAPI.Models
{
    public class ViajeUsuario
    {
        [Key]
        public int IdViajeUsuario { get; set; }

        public int IdViaje { get; set; }
        public int IdUsuario { get; set; }

        [ForeignKey("IdViaje")]
        public Viaje? Viaje { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario? Usuario { get; set; }
    }
}
