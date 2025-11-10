using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SlotWise.Web.Data.Entities;

namespace SlotWise.Web.Data
{
    // Contexto de la base de datos usamos IdentityDbContext para incluir la gestión de usuarios con un IdentityUser personalizado user 
    public class DataContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        // Definición de las tablas en la base de datos
        // Cada DbSet representa una tabla
        public DbSet<Specialist> Specialist { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PrivateRole> PrivateRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Llamar al método base para configurar las tablas de Identity
            base.OnModelCreating(modelBuilder);
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
            // Configurar relación muchos a muchos entre PrivateRole y Permission a través de RolePermission
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.PrivateRoleId, rp.PermissionId });
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.PrivateRole)
                .WithMany(pr => pr.RolePermissions)
                .HasForeignKey(rp => rp.PrivateRoleId);
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);
        }
    }
}
