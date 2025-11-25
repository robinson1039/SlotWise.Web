using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.EntityFrameworkCore;
using SlotWise.Web;
using SlotWise.Web.Data;
using SlotWise.Web.Helpers.Abstractions;
using SlotWise.Web.Helpers.Implementations;
using SlotWise.Web.Services.Abstractions;
using SlotWise.Web.Services.Implementations;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddCustomConfiguration();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRolesHelper, RolesHelper>();
builder.Services.AddScoped<IRolesService, RolesService>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

// Routing and Authorization
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseNotyf();
app.AddCustomWebApplicationConfiguration();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();
