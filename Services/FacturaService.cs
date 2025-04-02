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

        public async Task<int> CrearFactura(List<Carrito> itemsCarrito)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();

            try
            {
                if (itemsCarrito == null || !itemsCarrito.Any())
                    throw new Exception("No hay productos en el carrito para facturar");

                var facturas = new List<Factura>();

                foreach (var item in itemsCarrito)
                {
                    var producto = await contexto.Productos.FindAsync(item.ProductoId);
                    if (producto == null)
                        throw new Exception($"Producto con ID {item.ProductoId} no encontrado");

                    var factura = new Factura
                    {
                        Fecha = DateTime.Now,
                        Total = (decimal)(item.Cantidad * producto.Precio),
                        ProductoId = item.ProductoId,
                        CarritoId = item.CarritoId,
                        Cantidad = item.Cantidad,
                        Precio = producto.Precio,
                        Subtotal = item.Cantidad * producto.Precio,
                        Producto = producto
                    };

                    facturas.Add(factura);
                    contexto.Facturas.Add(factura);
                }

                var cambios = await contexto.SaveChangesAsync();
                if (cambios < facturas.Count)
                    throw new Exception($"Solo se guardaron {cambios} de {facturas.Count} facturas");

                return facturas.First().FacturaId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CrearFactura: {ex.Message}\n{ex.StackTrace}");
                return -1;
            }
        }


        public async Task<Factura> ObtenerFactura(int id)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Facturas
                .Include(f => f.Producto)
                .FirstOrDefaultAsync(f => f.FacturaId == id);
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
    }
}