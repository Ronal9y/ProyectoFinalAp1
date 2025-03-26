using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Data;
using ProyectoFinalAp1.Models;
using System.Linq.Expressions;

namespace ProyectoFinalAp1.Services;

public class ProductoCategoriasService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory = DbFactory;

    private async Task<bool> Insertar(ProductoCategorias categoria)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        contexto.ProductoCategorias.Add(categoria);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(ProductoCategorias categoria)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        contexto.ProductoCategorias.Update(categoria);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = _dbFactory.CreateDbContext();

        // Verificar si hay productos con este TipoCategoria
        var nombreCategoria = await contexto.ProductoCategorias
            .Where(c => c.CategoriaId == id)
            .Select(c => c.Nombre)
            .FirstOrDefaultAsync();

        if (!string.IsNullOrEmpty(nombreCategoria))
        {
            var tieneProductos = await contexto.Productos
                .AnyAsync(p => p.TipoCategoria == nombreCategoria);

            if (tieneProductos)
            {
                throw new InvalidOperationException("No se puede eliminar la categoría porque hay productos asociados a este tipo de categoría");
            }
        }

        var eliminados = await contexto.ProductoCategorias
            .Where(c => c.CategoriaId == id)
            .ExecuteDeleteAsync();

        return eliminados > 0;
    }

    public async Task<bool> Existe(int categoriaId)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        return await contexto.ProductoCategorias.AnyAsync(c => c.CategoriaId == categoriaId);
    }

    public async Task<bool> ExisteCategoria(int id, string? nombre)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        return await contexto.ProductoCategorias.AnyAsync(c =>
            c.CategoriaId == id ||
            c.Nombre == nombre);
    }

    public async Task<bool> Guardar(ProductoCategorias categoria)
    {
        if (!await Existe(categoria.CategoriaId))
        {
            return await Insertar(categoria);
        }
        else
        {
            return await Modificar(categoria);
        }
    }

    public async Task<ProductoCategorias?> Buscar(int id = 0, string? nombre = null)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();

        if (id > 0)
        {
            return await contexto.ProductoCategorias
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoriaId == id);
        }
        else if (!string.IsNullOrEmpty(nombre))
        {
            return await contexto.ProductoCategorias
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Nombre == nombre);
        }

        return null;
    }

    public async Task<List<ProductoCategorias>> Listar(Expression<Func<ProductoCategorias, bool>> criterio = null)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();

        IQueryable<ProductoCategorias> query = contexto.ProductoCategorias
            .AsNoTracking();

        if (criterio != null)
        {
            query = query.Where(criterio);
        }

        return await query.ToListAsync();
    }

    public async Task<List<ProductoCategorias>> ListarCategorias()
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.ProductoCategorias
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<string>> ObtenerTiposCategoriaDisponibles()
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.ProductoCategorias
            .Select(c => c.Nombre)
            .Distinct()
            .ToListAsync();
    }

    public async Task<List<Productos>> ObtenerProductosPorCategoria(string nombreCategoria)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Productos
            .Where(p => p.TipoCategoria == nombreCategoria)
            .AsNoTracking()
            .ToListAsync();
    }
}