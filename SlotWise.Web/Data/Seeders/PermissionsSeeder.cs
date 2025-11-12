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
            List<Permission> permissions = [.. Services(), .. Reservation(), .. Especialistas(), .. Usuarios()];

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
                new Permission { Id = Guid.NewGuid(), Name = "showReservations", Description = "Ver reservacion", Module = "Reservacion"},
                new Permission { Id = Guid.NewGuid(), Name = "createReservations", Description = "Crear reservacion", Module = "Reservacion"},
                new Permission { Id = Guid.NewGuid(), Name = "updateReservations", Description = "Editar reservacion", Module = "Reservacion"},
                new Permission { Id = Guid.NewGuid(), Name = "deleteReservations", Description = "Eliminar reservacion", Module = "Reservacion"},
            };
        }
        private List<Permission> Especialistas()
        {
            return new List<Permission> //me faltó quitar la E en Especialists 
            {
                new Permission { Id = Guid.NewGuid(), Name = "showEspecialists", Description = "Ver especialistas", Module = "Especialistas" },
                new Permission { Id = Guid.NewGuid(), Name = "createEspecialists", Description = "Crear especialistas", Module = "Especialistas" },
                new Permission { Id = Guid.NewGuid(), Name = "updateEspecialists", Description = "Editar especialistas", Module = "Especialistas" },
                new Permission { Id = Guid.NewGuid(), Name = "deleteEspecialists", Description = "Eliminar especialistas", Module = "Especialistas" },
            };
        }
        private List<Permission> Usuarios()
        {
            return new List<Permission>
            {
                new Permission { Id = Guid.NewGuid(), Name = "showUsers", Description = "Ver usuarios", Module = "Usuarios" },
                new Permission { Id = Guid.NewGuid(), Name = "createUsers", Description = "Crear usuarios", Module = "Usuarios" },
                new Permission { Id = Guid.NewGuid(), Name = "updateUsers", Description = "Editar usuarios", Module = "Usuarios" },
                new Permission { Id = Guid.NewGuid(), Name = "deleteUsers", Description = "Eliminar usuarios", Module = "Usuarios" },
            };
        }

    }
}