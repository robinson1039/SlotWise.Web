using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SlotWise.Web.Data;
using SlotWise.Web.Data.Entities;
using System.Security.Claims;


namespace SlotWise.Web.Helpers.Security
{
    public class CustomUserClaimsPrincipalFactory
    : UserClaimsPrincipalFactory<User, IdentityRole<Guid>>
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomUserClaimsPrincipalFactory(
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            IServiceProvider serviceProvider
        ) : base(userManager, roleManager, optionsAccessor)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            // Use injected IServiceProvider instead of Program.ServiceProvider
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            var role = context.PrivateRoles
                .Where(r => r.Id == user.PrivateRoleId)
                .Select(r => new
                {
                    r.Name,
                    Permissions = r.RolePermissions.Select(p => p.Permission)
                })
                .FirstOrDefault();

            if (role != null)
            {
                foreach (var perm in role.Permissions)
                {
                    // Use perm.Name instead of perm (which is a Permission object)
                    identity.AddClaim(new Claim("permission", perm.Name));
                }
            }

            return identity;
        }
    }
}
