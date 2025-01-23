using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestBlogCore.AccesoDatos.Data.Initializer;
using TestBlogCore.AccesoDatos.Data.Repository;
using TestBlogCore.AccesoDatos.Data.Repository.IRepository;
using TestBlogCore.Data;
using TestBlogCore.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();
builder.Services.AddControllersWithViews();

#region Agregar controlador de trabajo al IoC de inyección de dependencias.

builder.Services.AddScoped<IWorkContainer, WorkContainer>();

#endregion

#region Inyectar el servicio de inicialización de la base de datos.

builder.Services.AddScoped<IInitializerDb, InitializerDb>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

SeedingData();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Client}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

// Método para sembrar datos en la base de datos.
void SeedingData()
{
    using(var scope = app.Services.CreateScope())
    {
        var initializerDb = scope.ServiceProvider.GetRequiredService<IInitializerDb>();
        initializerDb.Initialize();
    }
}
