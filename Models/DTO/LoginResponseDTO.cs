namespace rootearAPI.Models.DTO
{
    public class LoginResponseDTO
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? UsuarioNombre { get; set; }
        public string Direccion { get; set; }
        public string Rol { get; set; }
        public int? IdVehiculo { get; set; }
        public string Email { get; set; }
        public int? IdSalaReserva { get; set; }
    }
}
