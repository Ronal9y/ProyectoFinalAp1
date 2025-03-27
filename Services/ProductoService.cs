using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Data;
using ProyectoFinalAp1.Models;
using System.Linq.Expressions;

namespace ProyectoFinalAp1.Services;

public class ProductoService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory = DbFactory;
    private async Task<bool> Insertar(Productos Productos)
    {
        await using var contexto = DbFactory.CreateDbContext();
        contexto.Productos.Add(Productos);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Productos Productos)
    {
        await using var contexto = DbFactory.CreateDbContext();
        contexto.Productos.Update(Productos);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = DbFactory.CreateDbContext();
        var EliminarProducto = await contexto.Productos.Where(m => m.ProductoId == id).
            ExecuteDeleteAsync();

        return EliminarProducto > 0;
    }

    public async Task<bool> Existe(int productoId)
    {
        await using var contexto = DbFactory.CreateDbContext();
        return await contexto.Productos.AnyAsync(m => m.ProductoId == productoId);
    }

    public async Task<bool> Guardar(Productos Productos)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        if (!await Existe(Productos.ProductoId))
        {
            return await Insertar(Productos);
        }
        else
        {
            return await Modificar(Productos);
        }
    }

    public async Task<Productos?> Buscar(int id = 0, string? nombre = null, string? tipoCategoria = null)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        if (id > 0)
        {
            return await contexto.Productos.AsNoTracking()
                .FirstOrDefaultAsync(s => s.ProductoId == id);
        }

        else if (!string.IsNullOrEmpty(nombre))
        {
            return await contexto.Productos.AsNoTracking()
                .FirstOrDefaultAsync(s => s.Nombre == nombre);

        }
        return null;

    }

    public async Task<List<Productos>> ListarProducto(Expression<Func<Productos, bool>>? filtro = null)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var query = contexto.Productos.AsNoTracking();
        return filtro == null ? await query.ToListAsync() : await query.Where(filtro).ToListAsync();
    }
    public async Task<int> ObtenerTotalProductosEnCarrito()
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Carrito.SumAsync(item => item.Cantidad);
    }

    public async Task AgregarAlCarrito(int productoId, int cantidad)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        var producto = await contexto.Productos.FindAsync(productoId);
        if (producto == null)
        {
            throw new Exception("Producto no encontrado");
        }

        var carritoItem = await contexto.Carrito.FirstOrDefaultAsync(c => c.ProductoId == productoId);
        if (carritoItem == null)
        {
            carritoItem = new Carrito
            {
                ProductoId = productoId,
                Cantidad = cantidad
            };
            contexto.Carrito.Add(carritoItem);
        }
        else
        {
            carritoItem.Cantidad += cantidad;
        }

        await contexto.SaveChangesAsync();
    }

    public async Task<List<Productos>> ListarProducto()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Productos.AsNoTracking().ToListAsync();
    }

    public async Task<bool> ExisteProducto(int id, string? nombre)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Productos.AnyAsync(s => s.ProductoId == id || s.Nombre == nombre);
    }

}