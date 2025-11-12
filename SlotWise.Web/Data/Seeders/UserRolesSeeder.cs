using Microsoft.EntityFrameworkCore;
using SlotWise.Web.Core;
using SlotWise.Web.Data.Entities;
using SlotWise.Web.Services.Abstractions;

namespace SlotWise.Web.Data.Seeders
{
    public class UserRolesSeeder
    {
        private readonly DataContext _context;
        private readonly IUserService _usersService;
        private const string EMPLOYEE_ROLE_NAME = "Employee";
        private const string BASIC_ROLE_NAME = "Basic";

        public UserRolesSeeder(DataContext context, IUserService usersService)
        {
            _context = context;
            _usersService = usersService;
        }

        public async Task SeedAsync()
        {
            await CheckRolesAsync();
            await CheckUsersAsync();
        }

        private async Task CheckRolesAsync()
        {
            await AdminRoleAsync();
            await BasicRoleAsync();
            await EmployeeRoleAsync();
        }

        private async Task CheckUsersAsync()
        {
            // Admin
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == "robinson@yopmail.com");

            if (user is null)
            {
                PrivateRole adminRole = await _context.PrivateRoles.FirstOrDefaultAsync(r => r.Name == Env.SUPER_ADMIN_ROLE_NAME);

                user = new User
                {
                    Email = "robinson@yopmail.com",
                    FirstName = "Robinson",
                    LastName = "Higuita",
                    CC = 12345678,
                    Age = 30,
                    Birthdate = new DateTime(1993, 1, 15),
                    UserName = "robinson@yopmail.com",
                    PrivateRoleId = adminRole.Id

                };

                await _usersService.AddUserAsync(user, "1234");

                string token = (await _usersService.GenerateConfirmationTokenAsync(user)).Result;
                await _usersService.ConfirmUserAsync(user, token);
            }
            // empleado
            user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == "stefa@yopmail.com");

            if (user is null)
            {
                PrivateRole employeeManagerRole = await _context.PrivateRoles.FirstOrDefaultAsync(r => r.Name == EMPLOYEE_ROLE_NAME);

                user = new User
                {
                    Email = "stefa@yopmail.com",
                    FirstName = "stefania",
                    LastName = "x",
                    CC = 12345678,
                    Age = 30,
                    Birthdate = new DateTime(1993, 1, 15),
                    UserName = "stefa@yopmail.com",
                    PrivateRoleId = employeeManagerRole.Id
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = (await _usersService.GenerateConfirmationTokenAsync(user)).Result;
                await _usersService.ConfirmUserAsync(user, token);
            }

            // Basic
            user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == "sebas@yopmail.com");

            if (user is null)
            {
                PrivateRole basicRole = await _context.PrivateRoles.FirstOrDefaultAsync(r => r.Name == BASIC_ROLE_NAME);

                user = new User
                {
                    Email = "sebas@yopmail.com",
                    FirstName = "sebastian",
                    LastName = "Higuita",
                    CC = 12345678,
                    Age = 30,
                    Birthdate = new DateTime(1993, 1, 15),
                    UserName = "sebas@yopmail.com",
                    PrivateRoleId = basicRole.Id
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = (await _usersService.GenerateConfirmationTokenAsync(user)).Result;
                await _usersService.ConfirmUserAsync(user, token);
            }
        }
        private async Task AdminRoleAsync()
        {
            bool exists = await _context.PrivateRoles.AnyAsync(r => r.Name == Env.SUPER_ADMIN_ROLE_NAME);

            if (!exists)
            {
                PrivateRole role = new PrivateRole { Id = Guid.NewGuid(), Name = Env.SUPER_ADMIN_ROLE_NAME };
                await _context.PrivateRoles.AddAsync(role);
                await _context.SaveChangesAsync();

                //ADMIN TIENE TODOS LOS PERMISOS
                List<Permission> allPermissions = await _context.Permissions.ToListAsync();
                foreach (Permission permission in allPermissions)
                {
                    await _context.RolePermissions.AddAsync(new RolePermission
                    {
                        PermissionId = permission.Id,
                        PrivateRoleId = role.Id
                    });
                }

                await _context.SaveChangesAsync();
            }
        

        }
        
        private async Task BasicRoleAsync()
        {
            bool exists = await _context.PrivateRoles.AnyAsync(r => r.Name == BASIC_ROLE_NAME);

            if (!exists)
            {
                PrivateRole role = new PrivateRole { Id = Guid.NewGuid(), Name = BASIC_ROLE_NAME };
                await _context.PrivateRoles.AddAsync(role);
                await _context.SaveChangesAsync();

                List<Permission> viewPermissions = await _context.Permissions
                    .Where(p => p.Name.StartsWith("show"))
                    .ToListAsync();

                foreach (Permission permission in viewPermissions)
                {
                    await _context.RolePermissions.AddAsync(new RolePermission
                    {
                        PermissionId = permission.Id,
                        PrivateRoleId = role.Id
                    });
                }

                await _context.SaveChangesAsync();
            }
        }
        
        private async Task EmployeeRoleAsync()
        {
            bool exists = await _context.PrivateRoles.AnyAsync(r => r.Name == EMPLOYEE_ROLE_NAME);

            if (!exists)
            {
                PrivateRole role = new PrivateRole { Id = Guid.NewGuid(), Name = EMPLOYEE_ROLE_NAME };
                await _context.PrivateRoles.AddAsync(role);
                await _context.SaveChangesAsync();

                List<Permission> employeePermissions = await _context.Permissions
                    .Where(p => p.Name.StartsWith("show") ||
                                p.Name.StartsWith("create") ||
                                p.Name.StartsWith("update"))
                    .ToListAsync();

                foreach (Permission permission in employeePermissions)
                {
                    await _context.RolePermissions.AddAsync(new RolePermission
                    {
                        PermissionId = permission.Id,
                        PrivateRoleId = role.Id
                    });
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}