using SlotWise.Web.Services.Abstractions;

namespace SlotWise.Web.Data.Seeders
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserService _usersService;

        public SeedDb(DataContext context, IUserService userService)
        {
            _context = context;
            _usersService = userService;
        }

        public async Task SeedAsync()
        {
            await new ServiceSeeder(_context).SeedAsync();
            await new SpecialistSeeder(_context).SeedAsync();
            await new UserRolesSeeder(_context, _usersService).SeedAsync();
            await new ReservationSeeder(_context).SeedAsync();
        }
    }
}
