using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SlotWise.Web.Core;
using SlotWise.Web.Data;
using SlotWise.Web.Data.Entities;
using SlotWise.Web.Data.Seeders;
using SlotWise.Web.Services;
using SlotWise.Web.Services.Abstractions;
using SlotWise.Web.Services.Implementations;

namespace SlotWise.Web
{
    public static class CustomConfiguration
    {
        public static WebApplicationBuilder AddCustomConfiguration(this WebApplicationBuilder builder)
        {
            //  1. Cargar el archivo .env (antes de usar la configuración)
            Env.Load();

            //  2. Leer la variable desde el .env
            var envConnection = Environment.GetEnvironmentVariable("MY_DB_CONNECTION");

            //  3. Si existe, sobrescribir el valor del appsettings.json
            if (!string.IsNullOrEmpty(envConnection))
            {
                builder.Configuration["ConnectionStrings:MyConnection"] = envConnection;
            }

            //  4. Verificar qué conexión se está usando
            string? cnn = builder.Configuration.GetConnectionString("MyConnection");
            Console.WriteLine($"🟢 Usando conexión: {cnn}");

            //  5. Configurar DbContext con la conexión ya inyectada
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(cnn);
            });

            // 6. AutoMapper
            builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

            // 7. Toast Notification Setup
            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

            // Indetity and access Management
            AddIAM(builder);

            // 8. Registrar servicios personalizados
            AddServices(builder);

            builder.Services.AddHttpContextAccessor();

            return builder;
        }

        // Identity and Access Management (IAM) configuration
        private static void AddIAM(WebApplicationBuilder builder)
        {
            // Configuración de Identity con User personalizado
            builder.Services.AddIdentity<User, IdentityRole<Guid>>(conf =>
            {
                conf.User.RequireUniqueEmail = true;
                conf.Password.RequireDigit = false;
                conf.Password.RequiredUniqueChars = 0;
                conf.Password.RequireLowercase = false;
                conf.Password.RequireUppercase = false;
                conf.Password.RequireNonAlphanumeric = false;
                conf.Password.RequiredLength = 4;
            }).AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

            // Configuración de la cookie de autenticación
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "SlotWise.Auth.Cookie";
                options.ExpireTimeSpan = TimeSpan.FromDays(3);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Error/403";
            });
        }



        // Registrar servicios personalizados
        private static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ISpecialistService, SpecialistService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IServiceService, ServiceService>();
            builder.Services.AddScoped<IReservationService, ReservationService>();
            builder.Services.AddScoped<CustomQueryableOperationsService>();
            builder.Services.AddTransient<SeedDb>();

        }

        public static WebApplication AddCustomWebApplicationConfiguration(this WebApplication app)
        {
            app.UseNotyf();

            SeedData(app);

            return app;
        }

        private static void SeedData(WebApplication app)
        {
            IServiceScopeFactory scopeFactory = app.Services.GetService<IServiceScopeFactory>();

            using IServiceScope scope = scopeFactory.CreateScope();
            SeedDb service = scope.ServiceProvider.GetService<SeedDb>();
            service.SeedAsync().Wait();
        }
    }
}
