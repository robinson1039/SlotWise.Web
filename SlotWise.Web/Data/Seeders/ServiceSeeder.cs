using Microsoft.EntityFrameworkCore;
using SlotWise.Web.Data.Entities;

namespace SlotWise.Web.Data.Seeders
{
    public class ServiceSeeder
    {
        private readonly DataContext _context;

        public ServiceSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            try
            {
                // Verificar si existe al menos un especialista
                Specialist specialist = await _context.Specialist.FirstOrDefaultAsync();

                if (specialist == null)
                {
                    Console.WriteLine("⚠️ No hay especialistas en la base de datos. Creando uno por defecto...");

                    // Crear un especialista por defecto
                    specialist = new Specialist
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Especialista",
                        LastName = "Demo",
                        Email = "especialista@demo.com",
                        Phone = "3001234567",
                        SpecialistDescription = "Especialista de demostración para servicios",
                        Age = 30,
                        Status = true,
                        Create_at = DateTime.UtcNow
                    };

                    await _context.Specialist.AddAsync(specialist);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"✅ Especialista demo creado: {specialist.FirstName} {specialist.LastName}");
                }

                Console.WriteLine($"🔧 Usando especialista: {specialist.FirstName} {specialist.LastName}");

                List<Service> services = new List<Service>()
                {
                    new Service
                    {
                        Id = Guid.NewGuid(),
                        NameService = "Corte de Cabello",
                        Price = 25.00m,
                        Description = "Corte de cabello profesional con estilo moderno",
                        Status = true,
                        ImgService = "corte.jpg",
                        SpecialistId = specialist.Id
                    },
                    new Service
                    {
                        Id = Guid.NewGuid(),
                        NameService = "Coloración",
                        Price = 80.00m,
                        Description = "Coloración profesional con productos de calidad",
                        Status = true,
                        ImgService = "coloracion.jpg",
                        SpecialistId = specialist.Id
                    },
                    new Service
                    {
                        Id = Guid.NewGuid(),
                        NameService = "Tratamiento Capilar",
                        Price = 45.00m,
                        Description = "Tratamiento rejuvenecedor para cabello dañado",
                        Status = true,
                        ImgService = "tratamiento.jpg",
                        SpecialistId = specialist.Id
                    },
                    new Service
                    {
                        Id = Guid.NewGuid(),
                        NameService = "Corte de Barba",
                        Price = 15.00m,
                        Description = "Arreglo y diseño profesional de barba",
                        Status = true,
                        ImgService = "barba.jpg",
                        SpecialistId = specialist.Id
                    },
                    new Service
                    {
                        Id = Guid.NewGuid(),
                        NameService = "Manicure y Pedicure",
                        Price = 35.00m,
                        Description = "Servicio completo de uñas",
                        Status = true,
                        ImgService = "manicure.jpg",
                        SpecialistId = specialist.Id
                    }
                };

                int servicesAdded = 0;
                foreach (Service service in services)
                {
                    bool exists = await _context.Services.AnyAsync(s => s.NameService == service.NameService);

                    if (!exists)
                    {
                        await _context.Services.AddAsync(service);
                        servicesAdded++;
                        Console.WriteLine($"✅ Servicio agregado: {service.NameService} - ${service.Price}");
                    }
                    else
                    {
                        Console.WriteLine($"⏭️ Servicio ya existe: {service.NameService}");
                    }
                }

                if (servicesAdded > 0)
                {
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"🎉 ServiceSeeder completado. {servicesAdded} servicios agregados.");
                }
                else
                {
                    Console.WriteLine("ℹ️ Todos los servicios ya existen en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en ServiceSeeder: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"🔍 Detalles: {ex.InnerException.Message}");
                }
            }
        }
    }
}