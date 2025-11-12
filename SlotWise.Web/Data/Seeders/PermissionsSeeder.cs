using Microsoft.EntityFrameworkCore;
using SlotWise.Web.Data.Entities;

namespace SlotWise.Web.Data.Seeders
{
    public class PermissionsSeeder
    {
        private readonly DataContext _context;

        public PermissionsSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Permission> permissions = [.. Services(), .. Reservation(), .. Roles()];

            foreach (Permission permission in permissions)
            {
                bool exists = await _context.Permissions.AnyAsync(p => p.Name == permission.Name);

                if (!exists)
                {
                    await _context.Permissions.AddAsync(permission);
                }
            }

            await _context.SaveChangesAsync();
        }

        private List<Permission> Services()
        {
            return new List<Permission>
            {
                new Permission { Id = Guid.NewGuid(), Name = "showServices", Description = "Ver servicio", Module = "Service"},
                new Permission { Id = Guid.NewGuid(), Name = "createServices", Description = "Crear servicio", Module = "Service"},
                new Permission { Id = Guid.NewGuid(), Name = "updateServices", Description = "Editar servicio", Module = "Service"},
                new Permission { Id = Guid.NewGuid(), Name = "deleteServices", Description = "Eliminar servicio", Module = "Service"},
            };
        }
        private List<Permission> Reservation()
        {
            return new List<Permission>
            {
                new Permission { Id = Guid.NewGuid(), Name = "showReservations", Description = "Ver reservacion", Module = "Reservation"},
                new Permission { Id = Guid.NewGuid(), Name = "createReservations", Description = "Crear reservacion", Module = "Reservation"},
                new Permission { Id = Guid.NewGuid(), Name = "updateReservations", Description = "Editar reservacion", Module = "Reservation"},
                new Permission { Id = Guid.NewGuid(), Name = "deleteReservations", Description = "Eliminar reservacion", Module = "Reservation"},
            };
        }

        private List<Permission> Roles()
        {
            return new List<Permission>
            {
                new Permission { Name = "showSections", Description = "Ver Secciones", Module = "Secciones"},
                new Permission { Name = "createSections", Description = "Crear Secciones", Module = "Secciones"},
                new Permission { Name = "updateSections", Description = "Editar Secciones", Module = "Secciones"},
                new Permission { Name = "deleteSections", Description = "Eliminar Secciones", Module = "Secciones"},
            };
        }
    }
}