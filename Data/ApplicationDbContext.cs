using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Models;

namespace ProyectoFinalAp1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Definición de DbSet para cada entidad
        public DbSet<Productos> Productos { get; set; }
        public DbSet<Citas> Citas { get; set; }
        public DbSet<Mascotas> Mascotas { get; set; }
        public DbSet<Empleados> Empleados { get; set; }
        public DbSet<Proveedores> Proveedores { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<ArticuloMascotas> ArticuloMascotas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la relación entre Carrito y Productos
            modelBuilder.Entity<Carrito>()
                .HasOne(c => c.Producto)
                .WithMany(p => p.Carritos) // Especifica la propiedad de navegación en Productos
                .HasForeignKey(c => c.ProductoId)
                .OnDelete(DeleteBehavior.Cascade); // Cambia a Cascade si es necesario

            // Configuración de índices únicos
            modelBuilder.Entity<Productos>()
                .HasIndex(p => p.Nombre)
                .IsUnique();

            // Configuración de valores predeterminados
            modelBuilder.Entity<Carrito>()
                .Property(c => c.Fecha)
                .HasDefaultValueSql("GETDATE()"); // Valor predeterminado: fecha actual
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Server=ADRIAN\\MSSQLSERVER01;Database=TiendaMascotas;Trusted_Connection=True;TrustServerCertificate=True;") // Reemplaza con tu cadena de conexión
                .EnableSensitiveDataLogging(); // Habilitar el registro sensible de datos
        }
    }
}