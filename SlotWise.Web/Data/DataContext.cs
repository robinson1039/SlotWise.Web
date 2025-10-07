using Microsoft.EntityFrameworkCore;
using SlotWise.Web.Data.Entities;

namespace SlotWise.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        // Definición de las tablas en la base de datos
        // Cada DbSet representa una tabla
        public DbSet<Specialist> Specialist { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar relación Reservation -> User
            modelBuilder.Entity<Reservation>().HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict); // ← CAMBIADO

            // Configurar relación Reservation -> Specialist
            modelBuilder.Entity<Reservation>().HasOne(r => r.Specialist)
                .WithMany(s => s.Reservations)
                .HasForeignKey(r => r.SpecialistId)
                .OnDelete(DeleteBehavior.Restrict); // ← CAMBIADO

            // Configurar relación Reservation -> Service
            modelBuilder.Entity<Reservation>().HasOne(r => r.Service)
                .WithMany()
                .HasForeignKey(r => r.ServiceId)
                .OnDelete(DeleteBehavior.Restrict); // ← CAMBIADO

            // Configurar relación Service -> Specialist
            modelBuilder.Entity<Service>().HasOne(s => s.Specialist)
                .WithMany(sp => sp.Services)
                .HasForeignKey(s => s.SpecialistId)
                .OnDelete(DeleteBehavior.Cascade); // ← Este puede seguir siendo Cascade

            // AGREGAR configuración para el decimal Price
            modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasPrecision(18, 2); // Precisión para decimal
        }
    }
}
