using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Data;
using ProyectoFinalAp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinalAp1.Services
{
    public class CarritoService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        private readonly FacturaService _facturaService;

        public CarritoService(IDbContextFactory<ApplicationDbContext> dbFactory, FacturaService facturaService)
        {
            _dbFactory = dbFactory;
            _facturaService = facturaService;
        }

        // Obtener el carrito con productos asociados
        public async Task<List<Carrito>> ObtenerCarritoAsync()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Carrito
                .Include(c => c.Producto)
                .AsNoTracking()
                .ToListAsync();
        }

        // Eliminar un producto del carrito
        public async Task<bool> EliminarDelCarritoAsync(int carritoId)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            await using var transaction = await contexto.Database.BeginTransactionAsync();

            try
            {
                // Primero eliminar las facturas asociadas al carrito
                var facturas = await contexto.Facturas
                    .Where(f => f.CarritoId == carritoId)
                    .ToListAsync();

                contexto.Facturas.RemoveRange(facturas);
                await contexto.SaveChangesAsync();

                // Luego eliminar el item del carrito
                var carritoItem = await contexto.Carrito.FindAsync(carritoId);
                if (carritoItem == null) return false;

                contexto.Carrito.Remove(carritoItem);
                await contexto.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        // Comprar un producto específico del carrito
        public async Task<bool> ComprarProducto(int carritoId)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            await using var transaction = await contexto.Database.BeginTransactionAsync();

            try
            {
                var carritoItem = await contexto.Carrito
                    .Include(c => c.Producto)
                    .FirstOrDefaultAsync(c => c.CarritoId == carritoId);

                if (carritoItem == null)
                {
                    Console.WriteLine("Error: Item de carrito no encontrado");
                    return false;
                }

                if (carritoItem.Producto == null)
                {
                    Console.WriteLine("Error: Producto asociado no encontrado");
                    return false;
                }

                if (carritoItem.Producto.Stock < carritoItem.Cantidad)
                {
                    Console.WriteLine($"Error: Stock insuficiente. Disponible: {carritoItem.Producto.Stock}, Requerido: {carritoItem.Cantidad}");
                    return false;
                }

                // 1. Crear factura primero
                var facturaId = await _facturaService.CrearFactura(new List<Carrito> { carritoItem });
                if (facturaId <= 0)
                {
                    Console.WriteLine("Error: No se pudo crear la factura");
                    return false;
                }

                // 2. Actualizar stock
                carritoItem.Producto.Stock -= carritoItem.Cantidad;
                contexto.Entry(carritoItem.Producto).State = EntityState.Modified;

                // 3. Eliminar del carrito
                contexto.Carrito.Remove(carritoItem);

                var cambios = await contexto.SaveChangesAsync();
                await transaction.CommitAsync();

                Console.WriteLine($"Éxito: Factura {facturaId} creada y producto comprado");
                return cambios > 0;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error en ComprarProducto: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        // Comprar todos los productos del carrito
        public async Task<bool> ComprarCarrito()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            await using var transaction = await contexto.Database.BeginTransactionAsync();

            try
            {
                var carritoItems = await contexto.Carrito
                    .Include(c => c.Producto)
                    .ToListAsync();

                if (!carritoItems.Any())
                    return false;

                // Verificar stock para todos los productos
                var productosSinStock = carritoItems
                    .Where(item => item.Producto.Stock < item.Cantidad)
                    .ToList();

                if (productosSinStock.Any())
                    return false;

                // Crear factura
                var facturaId = await _facturaService.CrearFactura(carritoItems);
                if (facturaId <= 0)
                    return false;

                // Restar el stock de los productos
                foreach (var item in carritoItems)
                {
                    item.Producto.Stock -= item.Cantidad;
                    contexto.Entry(item.Producto).State = EntityState.Modified;
                }

                // Eliminar los productos del carrito
                contexto.Carrito.RemoveRange(carritoItems);
                await contexto.SaveChangesAsync();
                await transaction.CommitAsync();

                Console.WriteLine($"Éxito: Factura {facturaId} creada y productos comprados");
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error en ComprarCarrito: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }
    }
}
