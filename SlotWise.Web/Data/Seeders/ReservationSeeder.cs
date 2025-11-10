using Microsoft.EntityFrameworkCore;
using SlotWise.Web.Data.Entities;

namespace SlotWise.Web.Data.Seeders
{
    public class ReservationSeeder
    {
        private readonly DataContext _context;
        public ReservationSeeder(DataContext context)
        {
            _context = context;
        }
        public async Task SeedAsync()
        {
            Specialist specialist = await _context.Specialist.FirstOrDefaultAsync();
            User user = await _context.Users.FirstOrDefaultAsync();
            Service service = await _context.Services.FirstOrDefaultAsync();

            List<Reservation> reservations = new List<Reservation>()
                {
                    new Reservation
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        SpecialistId = specialist.Id,
                        ServiceId = service.Id,
                        Status = true,
                        CreateAt = DateTime.UtcNow.AddDays(-2)
                    },
                    new Reservation
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        SpecialistId = specialist.Id,
                        ServiceId = service.Id,
                        Status = false,
                        CreateAt = DateTime.UtcNow.AddDays(-1)
                    },
                    new Reservation
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        SpecialistId = specialist.Id,
                        ServiceId = service.Id,
                        Status = true,
                        CreateAt = DateTime.UtcNow
                    }
                };

            foreach (Reservation reservation in reservations)
            {
                bool exists = await _context.Reservations.AnyAsync(r =>
                   r.UserId == reservation.UserId &&
                   r.SpecialistId == reservation.SpecialistId &&
                   r.ServiceId == reservation.ServiceId &&
                   r.CreateAt.Date == reservation.CreateAt.Date);

                if (!exists)
                {
                    await _context.Reservations.AddAsync(reservation);
                    Console.WriteLine($"reserva agregada: {reservation.Id}");
                }
                else
                {
                    Console.WriteLine($"Servicio ya existe: {reservation.Id}");
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
