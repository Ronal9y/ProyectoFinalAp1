using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Models;

namespace ProyectoFinalAp1.Services
{
    public class CarritoService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

        public CarritoService(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        // Obtener todos los artículos del carrito
        public async Task<List<Carrito>> ObtenerCarrito()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Carritos.Include(ci => ci.Producto).ToListAsync();
        }

        // Agregar un artículo al carrito
        public async Task<bool> AgregarAlCarrito(int productoId, int cantidad)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();

            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor que 0.");

            var producto = await contexto.Productos.FindAsync(productoId);
            if (producto == null)
                throw new ArgumentException("El producto no existe.");

            if (producto.Cantidad < cantidad)
                throw new InvalidOperationException("No hay suficiente stock disponible.");

            var carritoItem = await contexto.Carritos.FirstOrDefaultAsync(ci => ci.ProductoId == productoId);

            if (carritoItem == null)
            {
                carritoItem = new Carrito { ProductoId = productoId, Cantidad = cantidad };
                contexto.Carritos.Add(carritoItem);
            }
            else
            {
                carritoItem.Cantidad += cantidad;
                contexto.Carritos.Update(carritoItem);
            }

            await contexto.SaveChangesAsync();
            return true;
        }

        // Comprar un producto del carrito
        public async Task<bool> ComprarProducto(int carritoId)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();

            var carritoItem = await contexto.Carritos.Include(c => c.Producto).FirstOrDefaultAsync(c => c.CarritoId == carritoId);
            if (carritoItem == null)
                return false;

            var producto = await contexto.Productos.FindAsync(carritoItem.ProductoId);
            if (producto == null || producto.Cantidad < carritoItem.Cantidad)
                return false;

            // Restar la cantidad comprada del stock
            producto.Cantidad -= carritoItem.Cantidad;
            contexto.Productos.Update(producto);

            // Eliminar el producto del carrito
            contexto.Carritos.Remove(carritoItem);

            await contexto.SaveChangesAsync();
            return true;
        }

        // Comprar todos los productos del carrito
        public async Task<bool> ComprarCarrito()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            var carritoItems = await contexto.Carritos.Include(c => c.Producto).ToListAsync();

            bool compraExitosa = true;

            foreach (var item in carritoItems.ToList())
            {
                bool comprado = await ComprarProducto(item.CarritoId);
                if (!comprado)
                    compraExitosa = false;
            }

            return compraExitosa;
        }

        // Eliminar un artículo del carrito (sin modificar el stock)
        public async Task<bool> EliminarDelCarrito(int carritoId)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();

            var carritoItem = await contexto.Carritos.FindAsync(carritoId);
            if (carritoItem == null)
                return false;

            // Solo eliminar del carrito, sin afectar la cantidad del producto
            contexto.Carritos.Remove(carritoItem);
            await contexto.SaveChangesAsync();
            return true;
        }

        // Calcular total de artículos en el carrito
        public async Task<int> CalcularTotalArticulos()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Carritos.SumAsync(ci => ci.Cantidad);
        }

        // Calcular total del precio de los artículos en el carrito
        public async Task<double> CalcularTotalPrecio()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Carritos
                .Include(ci => ci.Producto)
                .Where(ci => ci.Producto != null)
                .SumAsync(ci => ci.Cantidad * (ci.Producto.Precio ?? 0));
        }
    }
}
