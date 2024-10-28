using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rootearAPI.Models
{
    public class Viaje
    {
        [Key]
        public int IdViaje { get; set; }

        public int IdOrigen { get; set; }
        public int IdDestino { get; set; }
        public int IdUsuarioCreador { get; set; }

        [Required]
        public DateTime FechaSalida { get; set; }

        public DateTime? FechaArribo { get; set; }

        [Required]
        public int CantButacas { get; set; }

        [Required]
        public DateTime ActivoDesde { get; set; } = DateTime.Now;

        public bool Estado { get; set; } = true;

        // Foreign Keys y relaciones
        [ForeignKey("IdOrigen")]
        public Lugar? Origen { get; set; }

        [ForeignKey("IdDestino")]
        public Lugar? Destino { get; set; }

        [ForeignKey("IdUsuarioCreador")]
        public Usuario? UsuarioCreador { get; set; }

        // Relación uno a muchos con ViajeUsuario
        public ICollection<ViajeUsuario> ViajesUsuarios { get; set; } = new List<ViajeUsuario>();

        // Relación uno a muchos con DetalleReserva
        public ICollection<DetalleReserva> DetallesReservas { get; set; } = new List<DetalleReserva>();
    }
}
