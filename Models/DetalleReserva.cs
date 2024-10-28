using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rootearAPI.Models
{
    public class DetalleReserva
    {
        [Key]
        public int IdDetalleReserva { get; set; }

        public int IdSalaReserva { get; set; }
        public int IdViaje { get; set; }

        [Required]
        public DateTime FechaAgregado { get; set; } = DateTime.Now;

        // Foreign Keys
        [ForeignKey("IdSalaReserva")]
        public SalaReserva? SalaReserva { get; set; }

        [ForeignKey("IdViaje")]
        public Viaje? Viaje { get; set; }
    }
}
