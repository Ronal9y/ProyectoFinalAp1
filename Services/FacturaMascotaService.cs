using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Data;
using ProyectoFinalAp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinalAp1.Services
{
    public class FacturaMascotaService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

        public FacturaMascotaService(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<int> CrearFacturaMascotaIndividual(int carritoMascotaId)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            await using var transaction = await contexto.Database.BeginTransactionAsync();

            try
            {
                var carritoItem = await contexto.CarritoMascotas
                    .Include(c => c.Mascota)
                    .FirstOrDefaultAsync(c => c.CarritoMascotaId == carritoMascotaId);

                if (carritoItem == null)
                    throw new Exception("Mascota no encontrada en el carrito");

                var factura = new FacturaMascota
                {
                    Fecha = DateTime.Now,
                    Total = (decimal)(carritoItem.Cantidad * carritoItem.Mascota.Precio),
                    MascotaId = carritoItem.MascotaId,
                    CarritoMascotaId = carritoItem.CarritoMascotaId,
                    Cantidad = carritoItem.Cantidad,
                    Precio = carritoItem.Mascota.Precio,
                    Subtotal = carritoItem.Cantidad * carritoItem.Mascota.Precio,
                    Mascotas = carritoItem.Mascota
                };

                contexto.FacturaMascotas.Add(factura);
                await contexto.SaveChangesAsync();
                await transaction.CommitAsync();

                return factura.FacturaMascotaId;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error al crear factura individual: {ex.Message}");
            }
        }

        public async Task<int> CrearFacturaMascotaGlobal()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            await using var transaction = await contexto.Database.BeginTransactionAsync();

            try
            {
                var carritoItems = await contexto.CarritoMascotas
                    .Include(c => c.Mascota)
                    .ToListAsync();

                if (!carritoItems.Any())
                    throw new Exception("No hay mascotas en el carrito");

                // Creamos una factura por cada mascota pero las relacionamos con el mismo CarritoMascotaId
                var facturas = new List<FacturaMascota>();
                var primerItem = carritoItems.First();

                foreach (var item in carritoItems)
                {
                    var factura = new FacturaMascota
                    {
                        Fecha = DateTime.Now,
                        Total = (decimal)(item.Cantidad * item.Mascota.Precio),
                        MascotaId = item.MascotaId,
                        CarritoMascotaId = primerItem.CarritoMascotaId, // Mismo CarritoMascotaId para agrupar
                        Cantidad = item.Cantidad,
                        Precio = item.Mascota.Precio,
                        Subtotal = item.Cantidad * item.Mascota.Precio,
                        Mascotas = item.Mascota
                    };

                    facturas.Add(factura);
                    contexto.FacturaMascotas.Add(factura);
                }

                await contexto.SaveChangesAsync();
                await transaction.CommitAsync();

                return primerItem.CarritoMascotaId; // Retornamos el CarritoMascotaId para agrupar
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error al crear factura global: {ex.Message}");
            }
        }

        public async Task<List<FacturaMascota>> ObtenerFacturasPorCarritoMascota(int carritoMascotaId)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.FacturaMascotas
                .Include(f => f.Mascotas)
                .ThenInclude(m => m.Donador)
                .Where(f => f.CarritoMascotaId == carritoMascotaId)
                .OrderByDescending(f => f.Fecha)
                .ToListAsync();
        }

        public async Task<List<FacturaMascota>> ObtenerTodasFacturasMascotas()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.FacturaMascotas
                .Include(f => f.Mascotas)
                .ThenInclude(m => m.Donador)
                .OrderByDescending(f => f.Fecha)
                .ToListAsync();
        }

        public async Task EliminarFacturasTemporales(int carritoMascotaId)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            var facturas = await contexto.FacturaMascotas
                .Where(f => f.CarritoMascotaId == carritoMascotaId)
                .ToListAsync();

            if (facturas.Any())
            {
                contexto.FacturaMascotas.RemoveRange(facturas);
                await contexto.SaveChangesAsync();
            }
        }
    }
}