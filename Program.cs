global using Microsoft.AspNetCore.Components.Authorization;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using ProyectoFinalAp1.Components;
global using ProyectoFinalAp1.Components.Account;
global using ProyectoFinalAp1.Data;
global using ProyectoFinalAp1.Services;
global using ProyectoFinalAp1.Models;
global using Microsoft.EntityFrameworkCore.Design;

namespace ProyectoFinalAp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddBlazorBootstrap();

            var ConStr = builder.Configuration.GetConnectionString("SqlConStr");
            builder.Services.AddDbContextFactory<ApplicationDbContext>(o => o.UseSqlServer(ConStr));

            builder.Services.AddScoped<CarritoService>();
            builder.Services.AddScoped<ProductoService>();
            builder.Services.AddScoped<EmpleadoService>();
            builder.Services.AddScoped<CarritoMascotasService>();
            builder.Services.AddScoped<MascotasService>();
            builder.Services.AddScoped<CitasService>();
            builder.Services.AddScoped<ProveedoresService>();
            builder.Services.AddScoped<ProductoCategoriasService>();
            builder.Services.AddScoped<DonadorService>();
            builder.Services.AddScoped<FacturaMascotaService>();
            builder.Services.AddScoped<FacturaService>();

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
                .AddIdentityCookies();

            //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            // Add additional endpoints required by the Identity /Account Razor components.
            app.MapAdditionalIdentityEndpoints();

            app.Run();
        }
    }
}