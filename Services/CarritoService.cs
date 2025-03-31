using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Data;
using ProyectoFinalAp1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoFinalAp1.Services;

public class CarritoService
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

    public CarritoService(IDbContextFactory<ApplicationDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<List<Carrito>> ObtenerCarritoAsync()
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Carrito
            .Include(c => c.Producto)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> EliminarDelCarrito(int carritoId)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        await using var transaction = await contexto.Database.BeginTransactionAsync();

        try
        {
            var carritoItem = await contexto.Carrito.FindAsync(carritoId);
            if (carritoItem == null) return false;

            contexto.Carrito.Remove(carritoItem);
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

    public async Task<bool> ComprarProducto(int carritoId)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        await using var transaction = await contexto.Database.BeginTransactionAsync();

        try
        {
            var carritoItem = await contexto.Carrito
                .Include(c => c.Producto)
                .FirstOrDefaultAsync(c => c.CarritoId == carritoId);

            if (carritoItem == null) return false;
            if (carritoItem.Producto.Stock < carritoItem.Cantidad) return false;

            carritoItem.Producto.Stock -= carritoItem.Cantidad;
            contexto.Productos.Update(carritoItem.Producto);
            contexto.Carrito.Remove(carritoItem);

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

    public async Task<bool> ComprarCarrito()
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        await using var transaction = await contexto.Database.BeginTransactionAsync();

        try
        {
            var carritoItems = await contexto.Carrito
                .Include(c => c.Producto)
                .ToListAsync();

            foreach (var item in carritoItems)
            {
                if (item.Producto.Stock < item.Cantidad)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                item.Producto.Stock -= item.Cantidad;
                contexto.Productos.Update(item.Producto);
                contexto.Carrito.Remove(item);
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

    public async Task<bool> ComprarProductoYGenerarFactura(int carritoId)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        await using var transaction = await contexto.Database.BeginTransactionAsync();

        try
        {
            var carritoItem = await contexto.Carrito
                .Include(c => c.Producto)
                .FirstOrDefaultAsync(c => c.CarritoId == carritoId);

            if (carritoItem == null)
                return false;

            if (carritoItem.Producto.Stock < carritoItem.Cantidad)
                return false;

            // Actualiza el stock
            carritoItem.Producto.Stock -= carritoItem.Cantidad;
            contexto.Productos.Update(carritoItem.Producto);

            // Generar factura
            var facturaService = new FacturaService(_dbFactory);
            var facturaId = await facturaService.CrearFacturaIndividual(carritoId);

            // Eliminar del carrito
            contexto.Carrito.Remove(carritoItem);
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
            var carritoItems = await contexto.Carrito
                .Include(c => c.Producto)
                .ToListAsync();

            if (!carritoItems.Any())
                return false;

            // Verifica el stocks
            foreach (var item in carritoItems)
            {
                if (item.Producto.Stock < item.Cantidad)
                    return false;
            }

            // Actualiza stocks
            foreach (var item in carritoItems)
            {
                item.Producto.Stock -= item.Cantidad;
                contexto.Productos.Update(item.Producto);
            }

            // Genera factura global
            var facturaService = new FacturaService(_dbFactory);
            var carritoId = await facturaService.CrearFacturaGlobal();

            // Vaciar carrito
            contexto.Carrito.RemoveRange(carritoItems);
            await contexto.SaveChangesAsync();
            await transaction.CommitAsync();

            return carritoId > 0;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}