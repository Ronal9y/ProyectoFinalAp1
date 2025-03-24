using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Models;

namespace ProyectoFinalAp1.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
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

        // Configuraci�n de la relaci�n entre Carrito y Productos
        modelBuilder.Entity<Carrito>()
            .HasOne(c => c.Producto)
            .WithMany(p => p.Carrito) // Especifica la propiedad de navegaci�n en Productos
            .HasForeignKey(c => c.ProductoId)
            .OnDelete(DeleteBehavior.Cascade); // Cambia a Cascade si es necesario

        // Configuraci�n de �ndices �nicos
        modelBuilder.Entity<Productos>()
            .HasIndex(p => p.Nombre)
            .IsUnique();
    }
}
