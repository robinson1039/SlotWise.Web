using AspNetCoreHero.ToastNotification;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using SlotWise.Web.Core;
using SlotWise.Web.Data;
using SlotWise.Web.Services;
using SlotWise.Web.Services.Abstractions;
using SlotWise.Web.Services.Implementations;

namespace SlotWise.Web
{
    public static class CustomConfiguration
    {
        public static WebApplicationBuilder AddCustomConfiguration(this WebApplicationBuilder builder)
        {
            // 🔹 1. Cargar el archivo .env (antes de usar la configuración)
            Env.Load();

            // 🔹 2. Leer la variable desde el .env
            var envConnection = Environment.GetEnvironmentVariable("MY_DB_CONNECTION");

            // 🔹 3. Si existe, sobrescribir el valor del appsettings.json
            if (!string.IsNullOrEmpty(envConnection))
            {
                builder.Configuration["ConnectionStrings:MyConnection"] = envConnection;
            }

            // 🔹 4. Verificar qué conexión se está usando
            string? cnn = builder.Configuration.GetConnectionString("MyConnection");
            Console.WriteLine($"🟢 Usando conexión: {cnn}");

            // 🔹 5. Configurar DbContext con la conexión ya inyectada
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(cnn);
            });

            // 🔹 6. AutoMapper
            builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

            // 🔹 7. Toast Notification Setup
            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

            // 🔹 8. Registrar servicios personalizados
            AddServices(builder);

            return builder;
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ISpecialistService, SpecialistService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IServiceService, ServiceService>();
            builder.Services.AddScoped<IReservationService, ReservationService>();
            builder.Services.AddScoped<CustomQueryableOperationsService>();
        }
    }
}
