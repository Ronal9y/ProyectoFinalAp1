using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Data;
using ProyectoFinalAp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinalAp1.Services
{
    public class FacturaService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

        public FacturaService(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<int> CrearFacturaIndividual(int carritoId)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            await using var transaction = await contexto.Database.BeginTransactionAsync();

            try
            {
                var carritoItem = await contexto.Carrito
                    .Include(c => c.Producto)
                    .FirstOrDefaultAsync(c => c.CarritoId == carritoId);

                if (carritoItem == null)
                    throw new Exception("Producto no encontrado en el carrito");

                var factura = new Factura
                {
                    Fecha = DateTime.Now,
                    Total = (decimal)(carritoItem.Cantidad * carritoItem.Producto.Precio),
                    ProductoId = carritoItem.ProductoId,
                    CarritoId = carritoItem.CarritoId,
                    Cantidad = carritoItem.Cantidad,
                    Precio = carritoItem.Producto.Precio,
                    Subtotal = carritoItem.Cantidad * carritoItem.Producto.Precio,
                    Producto = carritoItem.Producto
                };

                contexto.Facturas.Add(factura);
                await contexto.SaveChangesAsync();
                await transaction.CommitAsync();

                return factura.FacturaId;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error al crear factura individual: {ex.Message}");
            }
        }

        public async Task<int> CrearFacturaGlobal()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            await using var transaction = await contexto.Database.BeginTransactionAsync();

            try
            {
                var carritoItems = await contexto.Carrito
                    .Include(c => c.Producto)
                    .ToListAsync();

                if (!carritoItems.Any())
                    throw new Exception("No hay productos en el carrito");

                // Crea una factura por cada producto pero las relacionamos con el mismo CarritoId
                var facturas = new List<Factura>();
                var primerItem = carritoItems.First();

                foreach (var item in carritoItems)
                {
                    var factura = new Factura
                    {
                        Fecha = DateTime.Now,
                        Total = (decimal)(item.Cantidad * item.Producto.Precio),
                        ProductoId = item.ProductoId,
                        CarritoId = primerItem.CarritoId, // Mismo CarritoId para agrupar
                        Cantidad = item.Cantidad,
                        Precio = item.Producto.Precio,
                        Subtotal = item.Cantidad * item.Producto.Precio,
                        Producto = item.Producto
                    };

                    facturas.Add(factura);
                    contexto.Facturas.Add(factura);
                }

                await contexto.SaveChangesAsync();
                await transaction.CommitAsync();

                return primerItem.CarritoId; // Retorna al CarritoId para agrupar
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error al crear factura global: {ex.Message}");
            }
        }

        public async Task<List<Factura>> ObtenerFacturasPorCarrito(int carritoId)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Facturas
                .Include(f => f.Producto)
                .Where(f => f.CarritoId == carritoId)
                .OrderByDescending(f => f.Fecha)
                .ToListAsync();
        }

        public async Task<List<Factura>> ObtenerTodasFacturas()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Facturas
                .Include(f => f.Producto)
                .OrderByDescending(f => f.Fecha)
                .ToListAsync();
        }

        public async Task EliminarFacturasTemporales(int carritoId)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            var facturas = await contexto.Facturas
                .Where(f => f.CarritoId == carritoId)
                .ToListAsync();

            if (facturas.Any())
            {
                contexto.Facturas.RemoveRange(facturas);
                await contexto.SaveChangesAsync();
            }
        }
    }
}