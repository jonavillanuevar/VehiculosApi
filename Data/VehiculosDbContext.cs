using Microsoft.EntityFrameworkCore;
using VehiculosApi.Models;

namespace VehiculosApi.Data
{
    public class VehiculosDbContext : DbContext
    {


        public VehiculosDbContext(DbContextOptions<VehiculosDbContext> options) : base(options) {
        
        }


        public DbSet<Vehiculo> Vehiculos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehiculo>(entity =>
            {
                entity.HasKey(v => v.Id); // EF ya detecta por convención que Id es la clave primaria, así que esa línea es opcional.
                entity.Property(v => v.Marca).IsRequired().HasMaxLength(50);
                entity.Property(v => v.Modelo).IsRequired().HasMaxLength(50);
                entity.Property(v => v.Precio).HasPrecision(12, 2);

            });
        }

    }
}
