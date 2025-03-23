using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Data;
using ProyectoFinalAp1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoFinalAp1.Services
{
    public class CarritoService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

        public CarritoService(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        // Obtener todos los items del carrito
        public async Task<List<Carrito>> ObtenerCarritoAsync()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Carrito
                .Include(c => c.Producto) // Incluir datos del producto
                .ToListAsync();
        }

        // Agregar un producto al carrito
        public async Task<bool> AgregarAlCarritoAsync(int productoId, int cantidad)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            var producto = await contexto.Productos.FindAsync(productoId);
            if (producto == null)
            {
                return false; // Producto no encontrado
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
            return true;
        }

        // Eliminar un producto del carrito
        public async Task<bool> EliminarDelCarritoAsync(int carritoId)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            var carritoItem = await contexto.Carrito.FindAsync(carritoId);
            if (carritoItem == null)
            {
                return false; // Item no encontrado
            }

            contexto.Carrito.Remove(carritoItem);
            await contexto.SaveChangesAsync();
            return true;
        }

        // Obtener el total de productos en el carrito
        public async Task<int> ObtenerTotalProductosEnCarritoAsync()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Carrito.SumAsync(c => c.Cantidad);
        }
    }
}