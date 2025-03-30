using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Data;
using ProyectoFinalAp1.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

        public async Task<int> CrearFactura(List<Carrito> itemsCarrito)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            await using var transaction = await contexto.Database.BeginTransactionAsync();

            try
            {
                // Verifica que haya items en el carrito
                if (itemsCarrito == null || !itemsCarrito.Any())
                {
                    throw new Exception("No hay productos en el carrito para facturar");
                }

                foreach (var item in itemsCarrito)
                {
                    var factura = new Factura
                    {
                        Fecha = DateTime.Now,
                        Total = (decimal)(item.Cantidad * item.Producto.Precio),
                        ProductoId = item.ProductoId,
                        CarritoId = item.CarritoId,
                        Cantidad = item.Cantidad,
                        Precio = item.Producto.Precio,
                        Subtotal = item.Cantidad * item.Producto.Precio
                    };

                    contexto.Facturas.Add(factura);
                }

                await contexto.SaveChangesAsync();
                await transaction.CommitAsync();

                // Retorna el ID de la primera factura creada
                return itemsCarrito.First().CarritoId;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error al crear la factura: {ex.Message}");
            }
        }

        public async Task<Factura> ObtenerFactura(int id)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Facturas
                .Include(f => f.Producto)
                .Include(f => f.Carritos)
                .FirstOrDefaultAsync(f => f.FacturaId == id);
        }

        public async Task<List<Factura>> ObtenerFacturasPorCarrito(int carritoId)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Facturas
                .Include(f => f.Producto)
                .Include(f => f.Carritos)
                .Where(f => f.CarritoId == carritoId)
                .OrderByDescending(f => f.Fecha)
                .ToListAsync();
        }

        public async Task<List<Factura>> ObtenerTodasFacturas()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Facturas
                .Include(f => f.Producto)
                .Include(f => f.Carritos)
                .OrderByDescending(f => f.Fecha)
                .ToListAsync();
        }
    }
}