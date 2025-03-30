using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Data;
using ProyectoFinalAp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinalAp1.Services
{
    public class CarritoMascotasService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        private readonly FacturaMascotaService _facturaMascotaService;

        public CarritoMascotasService(
            IDbContextFactory<ApplicationDbContext> dbFactory,
            FacturaMascotaService facturaMascotaService)
        {
            _dbFactory = dbFactory;
            _facturaMascotaService = facturaMascotaService;
        }

        public async Task<List<CarritoMascotas>> ObtenerCarritoMascotasAsync()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.CarritoMascotas
                .Include(c => c.Mascota)
                .ThenInclude(m => m.Donador)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> AgregarAlCarritoMascotasAsync(int mascotaId, string nombreMascota)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();

            try
            {
                // Verificar si la mascota ya está en el carrito
                var existeEnCarrito = await contexto.CarritoMascotas
                    .AnyAsync(c => c.MascotaId == mascotaId);

                if (existeEnCarrito)
                {
                    return false; // Ya existe en el carrito (solo 1 por tipo)
                }

                var mascota = await contexto.Mascotas.FindAsync(mascotaId);
                if (mascota == null || mascota.Cantidad < 1)
                {
                    return false; // Mascota no disponible
                }

                var carritoItem = new CarritoMascotas
                {
                    MascotaId = mascotaId,
                    Cantidad = 1, // Solo 1 unidad
                    NombreMascota = nombreMascota
                };

                contexto.CarritoMascotas.Add(carritoItem);
                await contexto.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar al carrito: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EliminarDelCarritoAsync(int carritoMascotaId)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();

            try
            {
                var carritoItem = await contexto.CarritoMascotas
                    .FindAsync(carritoMascotaId);

                if (carritoItem == null) return false;

                contexto.CarritoMascotas.Remove(carritoItem);
                await contexto.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar del carrito: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ComprarMascota(int carritoMascotaId)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            await using var transaction = await contexto.Database.BeginTransactionAsync();

            try
            {
                // Obtener el item del carrito con la mascota relacionada
                var carritoItem = await contexto.CarritoMascotas
                    .Include(c => c.Mascota)
                    .FirstOrDefaultAsync(c => c.CarritoMascotaId == carritoMascotaId);

                // Verificar si el item existe y tiene mascota asociada
                if (carritoItem?.Mascota == null)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                // Verificar disponibilidad directamente en la base de datos
                var mascotaEnDB = await contexto.Mascotas
                    .FirstOrDefaultAsync(m => m.MascotaId == carritoItem.MascotaId);

                if (mascotaEnDB == null || mascotaEnDB.Cantidad < 1)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                // Crear factura
                var facturaId = await _facturaMascotaService.CrearFacturaMascota(
                    new List<CarritoMascotas> { carritoItem });

                if (facturaId <= 0)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                // Actualizar stock
                mascotaEnDB.Cantidad -= 1;
                contexto.Mascotas.Update(mascotaEnDB);

                // Eliminar del carrito
                contexto.CarritoMascotas.Remove(carritoItem);

                await contexto.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error al comprar mascota: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> ComprarCarritoMascotas()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            await using var transaction = await contexto.Database.BeginTransactionAsync();

            try
            {
                var carritoItems = await contexto.CarritoMascotas
                    .Include(c => c.Mascota)
                    .ToListAsync();

                if (!carritoItems.Any())
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                // Verificar que todas las mascotas están disponibles
                foreach (var item in carritoItems)
                {
                    if (item.Mascota?.Cantidad < 1)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }

                // Crear factura
                var facturaId = await _facturaMascotaService.CrearFacturaMascota(carritoItems);
                if (facturaId <= 0)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                // Actualizar stock y eliminar del carrito
                foreach (var item in carritoItems)
                {
                    item.Mascota.Cantidad -= 1;
                    contexto.Mascotas.Update(item.Mascota);
                    contexto.CarritoMascotas.Remove(item);
                }

                await contexto.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error al comprar carrito: {ex.Message}");
                return false;
            }
        }

        public async Task VaciarCarritoMascotasAsync()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            var items = await contexto.CarritoMascotas.ToListAsync();
            contexto.CarritoMascotas.RemoveRange(items);
            await contexto.SaveChangesAsync();
        }
    }
}