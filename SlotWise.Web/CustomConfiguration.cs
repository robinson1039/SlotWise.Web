using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
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
            string? cnn = builder.Configuration.GetConnectionString("MyConnection");

            // Data Context
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
            });

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

            // Toast Notification Setup
            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

            // Services
            AddServices(builder);   

            return builder;
        }
        private static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ISpecialistService, SpecialistService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IServiceService, ServiceService>();
            builder.Services.AddScoped<CustomQueryableOperationsService>();
            

        }
    }
}
