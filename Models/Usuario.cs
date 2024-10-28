using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rootearAPI.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Apellido { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Nombre { get; set; }

        [Required]
        [MaxLength(50)]
        public string? UsuarioNombre { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Contrasena { get; set; }

        [Required]
        [MaxLength(20)]
        public string? Celular { get; set; }

        [Required]
        [MaxLength(10)]
        public string? Dni { get; set; }

        [Required]
        public int Edad { get; set; }

        [Required]
        public DateTime FechaNac { get; set; }

        [MaxLength(30)]
        public string? Genero { get; set; }

        [MaxLength(50)]
        public string? Rol { get; set; }

        public bool Activo { get; set; } = true;

        [MaxLength(255)]
        public string? RutaImagen { get; set; }

        [Required]
        public DateTime ActivoDesde { get; set; } = DateTime.Now;

        // Relaciones uno a uno
        public int IdLugar { get; set; }
        [ForeignKey("IdLugar")]
        public Lugar? Lugar { get; set; }

        public int? IdVehiculo { get; set; }
        [ForeignKey("IdVehiculo")]
        public Vehiculo? Vehiculo { get; set; }

        public int? IdSalaReserva { get; set; }
        [ForeignKey("IdSalaReserva")]
        public SalaReserva? SalaReserva { get; set; }

        // Relación uno a muchos
        public ICollection<ViajeUsuario> ViajesUsuarios { get; set; } = new List<ViajeUsuario>();
    }
}
