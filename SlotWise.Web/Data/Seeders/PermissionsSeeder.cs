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
                new Permission { Id = Guid.NewGuid(), Name = "showBlogs", Description = "Ver Blogs", Module = "Blogs"},
                new Permission { Id = Guid.NewGuid(), Name = "createBlogs", Description = "Crear Blogs", Module = "Blogs"},
                new Permission { Id = Guid.NewGuid(), Name = "updateBlogs", Description = "Editar Blogs", Module = "Blogs"},
                new Permission { Id = Guid.NewGuid(), Name = "deleteBlogs", Description = "Eliminar Blogs", Module = "Blogs"},
            };
        }
        private List<Permission> Reservation()
        {
            return new List<Permission>
            {
                new Permission { Id = Guid.NewGuid(), Name = "showRoles", Description = "Ver Roles", Module = "Roles"},
                new Permission { Id = Guid.NewGuid(), Name = "createRoles", Description = "Crear Roles", Module = "Roles"},
                new Permission { Id = Guid.NewGuid(), Name = "updateRoles", Description = "Editar Roles", Module = "Roles"},
                new Permission { Id = Guid.NewGuid(), Name = "deleteRoles", Description = "Eliminar Roles", Module = "Roles"},
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
