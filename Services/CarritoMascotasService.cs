using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Data;
using ProyectoFinalAp1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoFinalAp1.Services
{
    public class CarritoMascotasService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

        public CarritoMascotasService(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        // Obtener todos los items del carrito de mascotas
        public async Task<List<CarritoMascotas>> ObtenerCarritoMascotasAsync()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.CarritoMascotas
                .Include(c => c.Mascota) // Incluir datos de la mascota
                .ToListAsync();
        }

        // Agregar una mascota al carrito
        public async Task<bool> AgregarAlCarritoMascotasAsync(int mascotaId, int cantidad, string nombreMascota)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();

            // Verificar si la mascota existe y hay suficiente cantidad
            var mascota = await contexto.Mascotas.FindAsync(mascotaId);
            if (mascota == null || mascota.Cantidad < cantidad)
            {
                return false; // Mascota no encontrada o no hay suficiente cantidad
            }

            var carritoItem = await contexto.CarritoMascotas
                .FirstOrDefaultAsync(c => c.MascotaId == mascotaId && c.NombreMascota == nombreMascota);

            if (carritoItem == null)
            {
                carritoItem = new CarritoMascotas
                {
                    MascotaId = mascotaId,
                    Cantidad = cantidad,
                    NombreMascota = nombreMascota
                };
                contexto.CarritoMascotas.Add(carritoItem);
            }
            else
            {
                carritoItem.Cantidad += cantidad;
            }

            await contexto.SaveChangesAsync();
            return true;
        }

        // Eliminar una mascota del carrito
        public async Task<bool> EliminarDelCarritoMascotasAsync(int carritoMascotaId)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            var carritoItem = await contexto.CarritoMascotas.FindAsync(carritoMascotaId);
            if (carritoItem == null)
            {
                return false; // Item no encontrado
            }

            contexto.CarritoMascotas.Remove(carritoItem);
            await contexto.SaveChangesAsync();
            return true;
        }

        // Actualizar la cantidad de una mascota en el carrito
        public async Task<bool> ActualizarCantidadCarritoMascotasAsync(int carritoMascotaId, int nuevaCantidad)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            var carritoItem = await contexto.CarritoMascotas.FindAsync(carritoMascotaId);
            if (carritoItem == null)
            {
                return false; // Item no encontrado
            }

            // Verificar que la mascota aún existe y hay suficiente cantidad
            var mascota = await contexto.Mascotas.FindAsync(carritoItem.MascotaId);
            if (mascota == null || mascota.Cantidad < nuevaCantidad)
            {
                return false;
            }

            carritoItem.Cantidad = nuevaCantidad;
            await contexto.SaveChangesAsync();
            return true;
        }

        // Obtener el total de mascotas en el carrito
        public async Task<int> ObtenerTotalMascotasEnCarritoAsync()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.CarritoMascotas.SumAsync(c => c.Cantidad);
        }

        // Vaciar todo el carrito de mascotas
        public async Task VaciarCarritoMascotasAsync()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            var todosLosItems = await contexto.CarritoMascotas.ToListAsync();
            contexto.CarritoMascotas.RemoveRange(todosLosItems);
            await contexto.SaveChangesAsync();
        }
    }
}