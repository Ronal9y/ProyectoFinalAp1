using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Models;

namespace ProyectoFinalAp1.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
     public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
     : base(options) { }

    public DbSet<Productos> Producto { get; set; }
    public DbSet<ProductoCategorias> ProductoCategoria { get; set; }
    public DbSet<Citas> Cita { get; set; }
    public DbSet<Mascotas> Mascota { get; set; }
    public DbSet<Empleados> Empleado { get; set; }
    public DbSet<Proveedores> Proveedore { get; set; }
    public DbSet<Articulos> Articulo { get; set; }
    public DbSet<Carrito> Carritos { get; set; }
    public DbSet<Usuarios> Usuario { get; set; }
    public DbSet<ArticuloMascotas> ArticuloMascota { get; set; }
}
