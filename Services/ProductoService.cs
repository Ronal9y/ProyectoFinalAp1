using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Models;

namespace ProyectoFinalAp1.Services
{
    public class ProductoService(IDbContextFactory<ApplicationDbContext> DbFactory)
    {
        // Verifica si un producto existe
        private async Task<bool> Existe(int ProductoId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Productos.AnyAsync(p => p.ProductoId == ProductoId);
        }

        // Inserta un nuevo producto
        private async Task<bool> Insertar(Productos producto)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();

            // Guardar el producto
            contexto.Productos.Add(producto);
            return await contexto.SaveChangesAsync() > 0;
        }

        // Modifica un producto existente
        public async Task<bool> Modificar(Productos producto)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();

            var productoOriginal = await contexto.Productos
                .FirstOrDefaultAsync(p => p.ProductoId == producto.ProductoId);

            if (productoOriginal == null)
                return false;

            contexto.Entry(productoOriginal).CurrentValues.SetValues(producto);
            return await contexto.SaveChangesAsync() > 0;
        }

        // Guarda un producto (inserta o modifica)
        public async Task<bool> Guardar(Productos producto)
        {
            if (!await Existe(producto.ProductoId))
                return await Insertar(producto);
            else
                return await Modificar(producto);
        }

        // Elimina un producto
        public async Task<bool> Eliminar(int ProductoId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            var producto = await contexto.Productos
                .FirstOrDefaultAsync(p => p.ProductoId == ProductoId);

            if (producto == null)
                return false;

            contexto.Productos.Remove(producto);
            return await contexto.SaveChangesAsync() > 0;
        }

        // Busca un producto por su ID
        public async Task<Productos?> Buscar(int ProductoId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Productos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductoId == ProductoId);
        }

        // Lista todos los productos que cumplan con un criterio
        public async Task<List<Productos>> Listar(Expression<Func<Productos, bool>> criterio)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Productos
                .AsNoTracking()
                .Where(criterio)
                .ToListAsync();
        }
    }
}