using Microsoft.EntityFrameworkCore;
using rootearAPI.Models;

namespace rootearAPI.Data
{
    public class apiContext : DbContext
    {
        public apiContext(DbContextOptions<apiContext> options) : base(options) { }

        public DbSet<Lugar> LUGAR { get; set; }
        public DbSet<SalaReserva> SALA_RESERVA { get; set; }
        public DbSet<Usuario> USUARIO { get; set; }
        public DbSet<Vehiculo> VEHICULO { get; set; }
        public DbSet<Viaje> VIAJE { get; set; }
        public DbSet<DetalleReserva> DETALLE_RESERVA { get; set; }
        public DbSet<ViajeUsuario> VIAJE_USUARIO { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación uno a uno entre Usuario y SalaReserva
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.SalaReserva)
                .WithOne()
                .HasForeignKey<Usuario>(u => u.IdSalaReserva)
                .OnDelete(DeleteBehavior.NoAction);

            // Relación uno a uno entre Usuario y Vehiculo
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Vehiculo)
                .WithOne()
                .HasForeignKey<Usuario>(u => u.IdVehiculo)
                .OnDelete(DeleteBehavior.NoAction);

            // Relación uno a muchos entre Lugar y Usuario (residencia)
            modelBuilder.Entity<Lugar>()
                .HasMany<Usuario>()
                .WithOne(u => u.Lugar)
                .HasForeignKey(u => u.IdLugar)
                .OnDelete(DeleteBehavior.NoAction);

            // Relación entre Lugar y Viaje (origen y destino)
            modelBuilder.Entity<Viaje>()
                .HasOne(v => v.Origen)
                .WithMany()
                .HasForeignKey(v => v.IdOrigen)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Viaje>()
                .HasOne(v => v.Destino)
                .WithMany()
                .HasForeignKey(v => v.IdDestino)
                .OnDelete(DeleteBehavior.NoAction);

            // Relación uno a muchos entre Viaje y DetalleReserva
            modelBuilder.Entity<Viaje>()
                .HasMany(v => v.DetallesReservas)
                .WithOne(dr => dr.Viaje)
                .HasForeignKey(dr => dr.IdViaje)
                .OnDelete(DeleteBehavior.NoAction);

            // Relación uno a muchos entre SalaReserva y DetalleReserva
            modelBuilder.Entity<SalaReserva>()
                .HasMany(sr => sr.DetalleReservas)
                .WithOne(dr => dr.SalaReserva)
                .HasForeignKey(dr => dr.IdSalaReserva)
                .OnDelete(DeleteBehavior.NoAction);

            // Configuración de ViajeUsuario para evitar cascadas
            modelBuilder.Entity<ViajeUsuario>()
                .HasOne(vu => vu.Viaje)
                .WithMany(v => v.ViajesUsuarios)
                .HasForeignKey(vu => vu.IdViaje)
                .OnDelete(DeleteBehavior.NoAction); // Evita cascada en la relación Viaje - ViajeUsuario

            modelBuilder.Entity<ViajeUsuario>()
                .HasOne(vu => vu.Usuario)
                .WithMany(u => u.ViajesUsuarios)
                .HasForeignKey(vu => vu.IdUsuario)
                .OnDelete(DeleteBehavior.NoAction); // Evita cascada en la relación Usuario - ViajeUsuario
        }



    }
}
