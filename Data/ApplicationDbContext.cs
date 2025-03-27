using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Models;

namespace ProyectoFinalAp1.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options) { }

    public DbSet<Productos> Productos { get; set; }
    public DbSet<ProductoCategorias> ProductoCategorias { get; set; }
    public DbSet<Citas> Citas { get; set; }
    public DbSet<Mascotas> Mascotas { get; set; }
    public DbSet<Empleados> Empleados { get; set; }
    public DbSet<Proveedores> Proveedores { get; set; }
    public DbSet<Carrito> Carrito { get; set; }
    public DbSet<Usuarios> Usuarios { get; set; }
    public DbSet<CarritoMascotas> CarritoMascotas { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

       
        modelBuilder.Entity<Carrito>()
            .HasOne(c => c.Producto)
            .WithMany(p => p.Carritos)
            .HasForeignKey(c => c.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Productos>()
            .HasIndex(p => p.Nombre)
            .IsUnique();


        modelBuilder.Entity<CarritoMascotas>()
            .HasOne(cm => cm.Mascota)
            .WithMany()
            .HasForeignKey(cm => cm.MascotaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Citas>()
            .HasOne(c => c.Empleado)
            .WithMany(e => e.Cita)
            .HasForeignKey(c => c.EmpleadoId)
            .OnDelete(DeleteBehavior.Restrict);

  
        modelBuilder.Entity<Productos>()
            .HasOne(p => p.Proveedores)
            .WithMany()
            .HasForeignKey(p => p.ProveedorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Mascotas>()
            .HasOne(p => p.Proveedores)
            .WithMany()
            .HasForeignKey(p => p.ProveedorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Proveedores>().HasData(new List<Proveedores>()
    {
        new Proveedores(){
            ProveedorId = 1,
            Nombre = "Alimentos Felinos RD",
            Direccion = "Av. 27 de Febrero #125, Santo Domingo",
            Telefono = "8095551234",
            Email = "contacto@felinosrd.com"
        },
        new Proveedores(){
            ProveedorId = 2,
            Nombre = "NutriCan Dominicana",
            Direccion = "Calle Duarte #45, Santiago",
            Telefono = "8295555678",
            Email = "ventas@nutrican.com.do"
        },
        new Proveedores(){
            ProveedorId = 3,
            Nombre = "Juguetes Mascoteros",
            Direccion = "Calle Las Flores #12, La Vega",
            Telefono = "8495559012",
            Email = "info@mascoterosrd.com"
        },
        new Proveedores(){
            ProveedorId = 4,
            Direccion = "Av. Independencia #78, San Cristóbal",
            Nombre = "Accesorios Pet",
            Telefono = "8095553456",
            Email = "pedidos@accesoriospet.com"
        },
        new Proveedores(){
            ProveedorId = 5,
            Nombre = "Farmacia Veterinaria RD",
            Direccion = "Calle Salud #33, Puerto Plata",
            Telefono = "8295557890",
            Email = "farmacia@vetrd.com"
        }

    });




    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer("workstation id=RegistroDb.mssql.somee.com;packet size=4096;user id=ahb45_SQLLogin_1;pwd=59868mjt41;data source=RegistroDb.mssql.somee.com;persist security info=False;initial catalog=RegistroDb;TrustServerCertificate=True") // Reemplaza con tu cadena de conexión
            .EnableSensitiveDataLogging(); // Habilitar el registro sensible de datos
    }



}