using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Data;
using ProyectoFinalAp1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoFinalAp1.Services;

public class CarritoMascotasService
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

    public CarritoMascotasService(IDbContextFactory<ApplicationDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    // Obtener todos los items del carrito de mascotas con información de la mascota
    public async Task<List<CarritoMascotas>> ObtenerCarritoMascotasAsync()
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.CarritoMascotas
            .Include(c => c.Mascota)
            .ThenInclude(m => m.Donador) // Incluir también el donador si es necesario
            .AsNoTracking()
            .ToListAsync();
    }

    // Agregar una mascota al carrito
    public async Task<bool> AgregarAlCarritoMascotasAsync(int mascotaId, int cantidad, string nombreMascota)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        await using var transaction = await contexto.Database.BeginTransactionAsync();

        try
        {
            // Verificar si la mascota existe y hay suficiente cantidad
            var mascota = await contexto.Mascotas.FindAsync(mascotaId);
            if (mascota == null)
            {
                return false; // Mascota no encontrada
            }

            if (mascota.Cantidad < cantidad)
            {
                return false; // No hay suficiente cantidad disponible
            }

            // Buscar si ya existe en el carrito
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
                // Verificar que no exceda el stock disponible al sumar
                if (mascota.Cantidad < carritoItem.Cantidad + cantidad)
                {
                    return false;
                }
                carritoItem.Cantidad += cantidad;
            }

            await contexto.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // Eliminar una mascota del carrito
    public async Task<bool> EliminarDelCarrito(int carritoMascotaId)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        await using var transaction = await contexto.Database.BeginTransactionAsync();

        try
        {
            var carritoItem = await contexto.CarritoMascotas.FindAsync(carritoMascotaId);
            if (carritoItem == null)
            {
                return false;
            }

            contexto.CarritoMascotas.Remove(carritoItem);
            await contexto.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // Actualiza la cantidad de una mascota en el carrito
    public async Task<bool> ActualizarCantidadCarritoMascotasAsync(int carritoMascotaId, int nuevaCantidad)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        await using var transaction = await contexto.Database.BeginTransactionAsync();

        try
        {
            var carritoItem = await contexto.CarritoMascotas
                .Include(c => c.Mascota)
                .FirstOrDefaultAsync(c => c.CarritoMascotaId == carritoMascotaId);

            if (carritoItem == null || carritoItem.Mascota == null)
            {
                return false;
            }

            // Valida que la nueva cantidad no exceda el stock disponible
            if (nuevaCantidad < 1 || nuevaCantidad > carritoItem.Mascota.Cantidad)
            {
                return false;
            }

            carritoItem.Cantidad = nuevaCantidad;
            await contexto.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // Obtiene el total de mascotas en el carrito (suma de cantidades)
    public async Task<int> ObtenerTotalMascotasEnCarritoAsync()
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.CarritoMascotas.SumAsync(c => c.Cantidad);
    }

    // Vacia todo el carrito de mascotas
    public async Task VaciarCarritoMascotasAsync()
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        await using var transaction = await contexto.Database.BeginTransactionAsync();

        try
        {
            var todosLosItems = await contexto.CarritoMascotas.ToListAsync();
            contexto.CarritoMascotas.RemoveRange(todosLosItems);
            await contexto.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // Comprar una mascota específica del carrito
    public async Task<bool> ComprarMascota(int carritoMascotaId)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        await using var transaction = await contexto.Database.BeginTransactionAsync();

        try
        {
            var carritoItem = await contexto.CarritoMascotas
                .Include(c => c.Mascota)
                .FirstOrDefaultAsync(c => c.CarritoMascotaId == carritoMascotaId);

            if (carritoItem == null || carritoItem.Mascota == null)
            {
                return false;
            }

            // Verificar stock disponible
            if (carritoItem.Mascota.Cantidad < carritoItem.Cantidad)
            {
                return false;
            }

            // Actualizar stock
            carritoItem.Mascota.Cantidad -= carritoItem.Cantidad;
            contexto.Mascotas.Update(carritoItem.Mascota);

            // Eliminar del carrito
            contexto.CarritoMascotas.Remove(carritoItem);

            await contexto.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // Compra todo el carrito
    public async Task<bool> ComprarCarrito()
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        await using var transaction = await contexto.Database.BeginTransactionAsync();

        try
        {
            var carritoItems = await contexto.CarritoMascotas
                .Include(c => c.Mascota)
                .ToListAsync();

            // Primero verificar que todos los items tienen stock suficiente
            foreach (var item in carritoItems)
            {
                if (item.Mascota == null || item.Mascota.Cantidad < item.Cantidad)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }

            // Luego procesar todas las compras
            foreach (var item in carritoItems)
            {
                item.Mascota.Cantidad -= item.Cantidad;
                contexto.Mascotas.Update(item.Mascota);
                contexto.CarritoMascotas.Remove(item);
            }

            await contexto.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> ComprarMascotaYGenerarFactura(int carritoMascotaId)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        await using var transaction = await contexto.Database.BeginTransactionAsync();

        try
        {
            var carritoItem = await contexto.CarritoMascotas
                .Include(c => c.Mascota)
                .FirstOrDefaultAsync(c => c.CarritoMascotaId == carritoMascotaId);

            if (carritoItem == null)
                return false;

            if (carritoItem.Mascota.Cantidad < carritoItem.Cantidad)
                return false;

            // Actualiza cantidad disponible
            carritoItem.Mascota.Cantidad -= carritoItem.Cantidad;
            contexto.Mascotas.Update(carritoItem.Mascota);

            // Genera factura
            var facturaService = new FacturaMascotaService(_dbFactory);
            var facturaId = await facturaService.CrearFacturaMascotaIndividual(carritoMascotaId);

            // Elimina del carrito
            contexto.CarritoMascotas.Remove(carritoItem);
            await contexto.SaveChangesAsync();
            await transaction.CommitAsync();

            return facturaId > 0;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> ComprarTodoYGenerarFactura()
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        await using var transaction = await contexto.Database.BeginTransactionAsync();

        try
        {
            var carritoItems = await contexto.CarritoMascotas
                .Include(c => c.Mascota)
                .ToListAsync();

            if (!carritoItems.Any())
                return false;

            // Verifica cantidades disponibles
            foreach (var item in carritoItems)
            {
                if (item.Mascota.Cantidad < item.Cantidad)
                    return false;
            }

            // Actualiza cantidades
            foreach (var item in carritoItems)
            {
                item.Mascota.Cantidad -= item.Cantidad;
                contexto.Mascotas.Update(item.Mascota);
            }

            // Genera factura global
            var facturaService = new FacturaMascotaService(_dbFactory);
            var carritoMascotaId = await facturaService.CrearFacturaMascotaGlobal();

            // Vacia carrito
            contexto.CarritoMascotas.RemoveRange(carritoItems);
            await contexto.SaveChangesAsync();
            await transaction.CommitAsync();

            return carritoMascotaId > 0;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}