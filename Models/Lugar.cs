using System.ComponentModel.DataAnnotations;

namespace rootearAPI.Models
{
    public class Lugar
    {
        [Key]
        public int IdLugar { get; set; }

        [Required]
        [StringLength(100)]
        public string? Ciudad { get; set; }

        [Required]
        [StringLength(100)]
        public string? Provincia { get; set; }

        [Required]
        [StringLength(100)]
        public string? Pais { get; set; }

        [Required]
        [StringLength(10)]
        public string? CodPostal { get; set; }

    }
}
