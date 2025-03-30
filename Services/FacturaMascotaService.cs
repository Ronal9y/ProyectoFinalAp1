using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Data;
using ProyectoFinalAp1.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

        public async Task<int> CrearFacturaMascota(List<CarritoMascotas> itemsCarrito)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            await using var transaction = await contexto.Database.BeginTransactionAsync();

            try
            {
                // Verifica que haya items en el carrito
                if (itemsCarrito == null || !itemsCarrito.Any())
                {
                    throw new Exception("No hay mascotas en el carrito para facturar");
                }

                foreach (var item in itemsCarrito)
                {
                    var factura = new FacturaMascota
                    {
                        Fecha = DateTime.Now,
                        Total = (decimal)(item.Cantidad * item.Mascota.Precio),
                        MascotaId = item.MascotaId,
                        CarritoMascotaId = item.CarritoMascotaId,
                        Cantidad = item.Cantidad,
                        Precio = item.Mascota.Precio,
                        Subtotal = item.Cantidad * item.Mascota.Precio
                    };

                    contexto.FacturaMascotas.Add(factura);
                }

                await contexto.SaveChangesAsync();
                await transaction.CommitAsync();

                // Retorna el ID de la primera factura creada
                return itemsCarrito.First().CarritoMascotaId;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error al crear la factura de mascota: {ex.Message}");
            }
        }

        public async Task<FacturaMascota> ObtenerFacturaMascota(int id)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.FacturaMascotas
                .Include(f => f.Mascotas)
                .Include(f => f.CarritoMascotas)
                .FirstOrDefaultAsync(f => f.FacturaMascotaId == id);
        }

        public async Task<List<FacturaMascota>> ObtenerFacturasMascotasPorCarrito(int carritoMascotaId)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.FacturaMascotas
                .Include(f => f.Mascotas)
                .Include(f => f.CarritoMascotas)
                .Where(f => f.CarritoMascotaId == carritoMascotaId)
                .OrderByDescending(f => f.Fecha)
                .ToListAsync();
        }

        public async Task<List<FacturaMascota>> ObtenerTodasFacturasMascotas()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.FacturaMascotas
                .Include(f => f.Mascotas)
                .Include(f => f.CarritoMascotas)
                .OrderByDescending(f => f.Fecha)
                .ToListAsync();
        }
    }
}