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
    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Antiforgery.Internal;

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
                builder.Services.AddRazorPages();
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
                //esto de aqui soluciona las cookies haciendo que tenga su tiempo de duracion de expiracion
                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
     .AddIdentityCookies(options =>
     {
         options.ApplicationCookie.Configure(opt =>
         {
             opt.Cookie.Name = "Kitter.Auth.Cookie";
             opt.LoginPath = "/Account/Login";
             opt.LogoutPath = "/Account/Logout";
             opt.ExpireTimeSpan = TimeSpan.FromMinutes(30);
             opt.SlidingExpiration = true;
             opt.Cookie.HttpOnly = true;
             opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
             opt.Cookie.SameSite = SameSiteMode.Lax;

             opt.Events.OnSigningOut = async context =>
             {
                 context.Response.Cookies.Delete("Kitter.Auth.Cookie");
                 await Task.CompletedTask;
             };
         });
     });

                builder.Services.AddDatabaseDeveloperPageExceptionFilter();

                builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddSignInManager()
                    .AddDefaultTokenProviders();

                builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

                builder.Logging.ClearProviders();
                builder.Logging.AddConsole();
                builder.Logging.AddDebug();

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
                app.UseStaticFiles();
                app.UseRouting();
                app.UseAntiforgery();

                app.UseAuthentication();
                app.UseAuthorization();
                app.UseCookiePolicy();


                app.MapStaticAssets();
                //y aqui se agrega el endpoint de la cuenta esto es lo que arreglo todo
                app.MapGet("/Account/Logout", async (
        SignInManager<ApplicationUser> signInManager,
        HttpContext context) =>
                {
                    await signInManager.SignOutAsync();
                    context.Response.Cookies.Delete("Kitter.Auth.Cookie");
                    context.Response.Redirect("/Account/Login");
                });
                app.MapRazorComponents<App>()
                    .AddInteractiveServerRenderMode();

                // Add additional endpoints required by the Identity /Account Razor components.
                app.MapAdditionalIdentityEndpoints();

                app.Run();
            }
        }
    }