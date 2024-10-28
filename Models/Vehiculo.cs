using System.ComponentModel.DataAnnotations;

namespace rootearAPI.Models
{
    public class Vehiculo
    {
        [Key]
        public int IdVehiculo { get; set; }

        [Required]
        public int CantPlazas { get; set; }

        [Required]
        [StringLength(100)]
        public string? Marca { get; set; }

        [Required]
        [StringLength(100)]
        public string? Modelo { get; set; }

        [Required]
        public int Anio { get; set; }

        public bool EspacioGuardado { get; set; } = false;

        [StringLength(50)]
        public string? TipoVehiculo { get; set; }

        public bool AptoDiscapacitados { get; set; } = false;

        [Required]
        [StringLength(10)]
        public string? Patente { get; set; }

        [StringLength(255)]
        public string? RutaImagen { get; set; }
    }
}
