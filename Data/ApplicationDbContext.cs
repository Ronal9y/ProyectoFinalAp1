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
    public DbSet<Donador> Donador { get; set; }
    public DbSet<CarritoMascotas> CarritoMascotas { get; set; }
    public DbSet<Factura> Facturas { get; set; }
    public DbSet<FacturaMascota> FacturaMascotas { get; set; }


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




        modelBuilder.Entity<Donador>().HasData(new List<Donador>()
        {
            new Donador() { DonadorId = 1, Nombre = "Nadie", Direccion = "N/A", Telefono = "0", Email = "N/A" },
            new Donador() { DonadorId = 2, Nombre = "Juan Pérez", Direccion = "Calle 1, Santo Domingo", Telefono = "8095551111", Email = "juanperez@example.com" },
            new Donador() { DonadorId = 3, Nombre = "María Gómez", Direccion = "Av. Bolívar, Santiago", Telefono = "8095552222", Email = "mariagomez@example.com" },
            new Donador() { DonadorId = 4, Nombre = "Carlos López", Direccion = "Calle Duarte, La Vega", Telefono = "8095553333", Email = "carloslopez@example.com" },
            new Donador() { DonadorId = 5, Nombre = "Ana Rodríguez", Direccion = "Calle las Palmas, San Cristóbal", Telefono = "8295554444", Email = "anarodriguez@example.com" },
            new Donador() { DonadorId = 6, Nombre = "Pedro Sánchez", Direccion = "Av. Independencia, Puerto Plata", Telefono = "8295555555", Email = "pedrosanchez@example.com" },
            new Donador() { DonadorId = 7, Nombre = "Luis Fernández", Direccion = "Calle Central, Barahona", Telefono = "8495556666", Email = "luisfernandez@example.com" },
            new Donador() { DonadorId = 8, Nombre = "Gloria Martínez", Direccion = "Calle Norte, Higuey", Telefono = "8495557777", Email = "gloriamartinez@example.com" },
            new Donador() { DonadorId = 9, Nombre = "José Domínguez", Direccion = "Calle Este, Moca", Telefono = "8095558888", Email = "josedominguez@example.com" },
            new Donador() { DonadorId = 10, Nombre = "Elena Vargas", Direccion = "Av. Sur, San Pedro", Telefono = "8295559999", Email = "elenavargas@example.com" },
            new Donador() { DonadorId = 11, Nombre = "Ricardo Jiménez", Direccion = "Calle Oeste, Bonao", Telefono = "8095550000", Email = "ricardojimenez@example.com" },
            new Donador() { DonadorId = 12, Nombre = "Sofía Castro", Direccion = "Av. Metropolitana, La Romana", Telefono = "8295561111", Email = "sofiacastro@example.com" },
            new Donador() { DonadorId = 13, Nombre = "Andrés Herrera", Direccion = "Calle del Sol, Monte Plata", Telefono = "8495562222", Email = "andresherrera@example.com" },
            new Donador() { DonadorId = 14, Nombre = "Carmen Ortiz", Direccion = "Calle Primavera, Baní", Telefono = "8095563333", Email = "carmenortiz@example.com" },
            new Donador() { DonadorId = 15, Nombre = "Francisco Medina", Direccion = "Av. Central, Nagua", Telefono = "8295564444", Email = "franciscomedina@example.com" },
            new Donador() { DonadorId = 16, Nombre = "Diana Pérez", Direccion = "Calle del Mar, Samaná", Telefono = "8495565555", Email = "dianaperez@example.com" },
            new Donador() { DonadorId = 17, Nombre = "Alberto Ríos", Direccion = "Calle Principal, Azua", Telefono = "8095566666", Email = "albertorios@example.com" },
            new Donador() { DonadorId = 18, Nombre = "Fernanda Castro", Direccion = "Av. Florida, Cotuí", Telefono = "8295567777", Email = "fernandacastro@example.com" },
            new Donador() { DonadorId = 19, Nombre = "Esteban Vargas", Direccion = "Calle Sol Naciente, Jarabacoa", Telefono = "8495568888", Email = "estebanvargas@example.com" },
            new Donador() { DonadorId = 20, Nombre = "Patricia Herrera", Direccion = "Av. Primavera, Hato Mayor", Telefono = "8095569999", Email = "patriciaherrera@example.com" }
        });






    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer("workstation id=RegistroDb.mssql.somee.com;packet size=4096;user id=ahb45_SQLLogin_1;pwd=Maximo134190;data source=RegistroDb.mssql.somee.com;persist security info=False;initial catalog=RegistroDb;TrustServerCertificate=True") // Reemplaza con tu cadena de conexión
            .EnableSensitiveDataLogging(); // Habilitar el registro sensible de datos
    }



}