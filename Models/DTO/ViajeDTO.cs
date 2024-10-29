using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace rootearAPI.Models.DTO
{
    public class ViajeDTO
    {
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
    }
}
