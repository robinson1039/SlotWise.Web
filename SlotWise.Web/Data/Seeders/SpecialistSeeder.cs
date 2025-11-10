using Microsoft.EntityFrameworkCore;
using SlotWise.Web.Data.Entities;

namespace SlotWise.Web.Data.Seeders
{
    public class SpecialistSeeder
    {
        private readonly DataContext _context;
        public SpecialistSeeder(DataContext context)
        {
            _context = context;
        }
        public async Task SeedAsync()
        {
            List<Specialist> specialists = new List<Specialist>()
                {
                    new Specialist
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "María",
                        LastName = "González",
                        CC = 123456789,
                        Email = "maria.gonzalez@slotwise.com",
                        Phone = "3001112233",
                        SpecialistDescription = "Especialista en cortes modernos y coloración profesional con 5 años de experiencia.",
                        Age = 28,
                        Status = true,
                        Create_at = DateTime.UtcNow.AddMonths(-6)
                    },
                    new Specialist
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Carlos",
                        LastName = "Rodríguez",
                        CC = 987654321,
                        Email = "carlos.rodriguez@slotwise.com",
                        Phone = "3004445566",
                        SpecialistDescription = "Experto en tratamientos capilares y barba. Más de 8 años en la industria.",
                        Age = 35,
                        Status = true,
                        Create_at = DateTime.UtcNow.AddMonths(-4)
                    },
                    new Specialist
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Ana",
                        LastName = "Martínez",
                        CC = 456789123,
                        Email = "ana.martinez@slotwise.com",
                        Phone = "3007778899",
                        SpecialistDescription = "Especialista en peinados para eventos y tratamientos de rejuvenecimiento capilar.",
                        Age = 32,
                        Status = true,
                        Create_at = DateTime.UtcNow.AddMonths(-2)
                    },
                    new Specialist
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "David",
                        LastName = "López",
                        CC = 321654987,
                        Email = "david.lopez@slotwise.com",
                        Phone = "3002223344",
                        SpecialistDescription = "Maestro barbero con enfoque en estilos clásicos y modernos para hombre.",
                        Age = 40,
                        Status = false, // Especialista inactivo
                        Create_at = DateTime.UtcNow.AddMonths(-1)
                    }
                };
            foreach (Specialist specialist in specialists)
            {
                bool exists = await _context.Specialist.AnyAsync(s =>
                        s.Email == specialist.Email ||
                        (s.CC.HasValue && s.CC == specialist.CC));

                if (!exists)
                {
                    await _context.Specialist.AddAsync(specialist);
                    Console.WriteLine($"Servicio agregado: {specialist.FirstName}");
                }
                else
                {
                    Console.WriteLine($"Servicio ya existe: {specialist.FirstName}");
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
