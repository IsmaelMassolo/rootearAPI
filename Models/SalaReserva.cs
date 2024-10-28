using System.ComponentModel.DataAnnotations;

namespace rootearAPI.Models
{
    public class SalaReserva
    {
        [Key]
        public int IdSalaReserva { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public bool Estado { get; set; } = true;

        // Relación uno a muchos con DetalleReserva
        public ICollection<DetalleReserva> DetalleReservas { get; set; } = new List<DetalleReserva>();
    }
}
