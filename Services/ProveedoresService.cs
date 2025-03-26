using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Data;
using ProyectoFinalAp1.Models;
using System.Linq.Expressions;

namespace ProyectoFinalAp1.Services;

public class ProveedoresService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory = DbFactory;

    private async Task<bool> Insertar(Proveedores proveedor)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        contexto.Proveedores.Add(proveedor);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Proveedores proveedor)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        contexto.Proveedores.Update(proveedor);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        var eliminados = await contexto.Proveedores
            .Where(p => p.ProveedorId == id)
            .ExecuteDeleteAsync();

        return eliminados > 0;
    }

    public async Task<bool> Existe(int proveedorId)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        return await contexto.Proveedores.AnyAsync(p => p.ProveedorId == proveedorId);
    }

    public async Task<bool> ExisteProveedor(int id, string? nombre, string? email)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        return await contexto.Proveedores.AnyAsync(p =>
            p.ProveedorId == id ||
            p.Nombre == nombre ||
            p.Email == email);
    }

    public async Task<bool> Guardar(Proveedores proveedor)
    {
        if (!await Existe(proveedor.ProveedorId))
        {
            return await Insertar(proveedor);
        }
        else
        {
            return await Modificar(proveedor);
        }
    }

    public async Task<Proveedores?> Buscar(int id = 0, string? nombre = null, string? email = null)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();

        if (id > 0)
        {
            return await contexto.Proveedores.AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProveedorId == id);
        }
        else if (!string.IsNullOrEmpty(nombre))
        {
            return await contexto.Proveedores.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Nombre == nombre);
        }
        else if (!string.IsNullOrEmpty(email))
        {
            return await contexto.Proveedores.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Email == email);
        }

        return null;
    }

    public async Task<List<Proveedores>> Listar(Expression<Func<Proveedores, bool>> criterio = null)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();

        if (criterio == null)
        {
            return await contexto.Proveedores.AsNoTracking().ToListAsync();
        }

        return await contexto.Proveedores.AsNoTracking().Where(criterio).ToListAsync();
    }

    public async Task<List<Proveedores>> ListarProveedores()
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Proveedores.AsNoTracking().ToListAsync();
    }
}