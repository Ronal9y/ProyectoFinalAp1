using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Data;
using ProyectoFinalAp1.Models;
using System.Linq;
using System.Linq.Expressions;

namespace ProyectoFinalAp1.Services;

public class MascotasService(IDbContextFactory<ApplicationDbContext> dbFactory)
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory = dbFactory;

    // Método privado para insertar una nueva mascota
    private async Task<bool> Insertar(Mascotas mascota)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        contexto.Mascotas.Add(mascota);
        return await contexto.SaveChangesAsync() > 0;
    }

    // Método privado para modificar una mascota existente
    private async Task<bool> Modificar(Mascotas mascota)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        contexto.Mascotas.Update(mascota);
        return await contexto.SaveChangesAsync() > 0;
    }

    // Eliminar una mascota por ID
    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        var eliminados = await contexto.Mascotas
            .Where(m => m.MascotaId == id)
            .ExecuteDeleteAsync();

        return eliminados > 0;
    }

    // Verificar si existe una mascota por ID
    public async Task<bool> Existe(int mascotaId)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        return await contexto.Mascotas.AnyAsync(m => m.MascotaId == mascotaId);
    }

    // Verificar si existe una mascota con el mismo tipo y raza
    public async Task<bool> ExisteMascota(string tipo, string raza, int excluirId = 0)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        return await contexto.Mascotas.AnyAsync(m =>
            m.Tipo == tipo &&
            m.Raza == raza &&
            m.MascotaId != excluirId);
    }

    // Guardar (insertar o modificar) una mascota
    public async Task<bool> Guardar(Mascotas mascota)
    {
        if (!await Existe(mascota.MascotaId))
        {
            return await Insertar(mascota);
        }
        else
        {
            return await Modificar(mascota);
        }
    }

    // Buscar mascotas por diferentes criterios
    public async Task<Mascotas?> Buscar(int id = 0, string? tipo = null, string? raza = null)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();

        if (id > 0)
        {
            return await contexto.Mascotas
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.MascotaId == id);
        }
        else if (!string.IsNullOrEmpty(tipo))
        {
            return await contexto.Mascotas
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Tipo == tipo);
        }
        else if (!string.IsNullOrEmpty(raza))
        {
            return await contexto.Mascotas
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Raza == raza);
        }

        return null;
    }

    // Listar mascotas con filtro opcional
    public async Task<List<Mascotas>> Listar(Expression<Func<Mascotas, bool>> criterio = null)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();

        IQueryable<Mascotas> query = contexto.Mascotas
            .AsNoTracking();

        if (criterio != null)
        {
            query = query.Where(criterio);
        }

        return await query.ToListAsync();
    }

    // Listar todas las mascotas
    public async Task<List<Mascotas>> ListarMascotas(Expression<Func<Mascotas, bool>>? filtro = null)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Mascotas
            .AsNoTracking()
            .ToListAsync();
    }

    // Listar mascotas disponibles (con cantidad > 0)
    public async Task<List<Mascotas>> ListarMascotasDisponibles()
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Mascotas
            .Where(m => m.Cantidad > 0)
            .AsNoTracking()
            .ToListAsync();
    }

    // Obtener tipos de mascotas únicos
    public async Task<List<string>> ObtenerTiposMascotas()
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Mascotas
            .Select(m => m.Tipo)
            .Distinct()
            .ToListAsync();
    }

    // Obtener razas por tipo de mascota
    public async Task<List<string>> ObtenerRazasPorTipo(string tipo)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Mascotas
            .Where(m => m.Tipo == tipo)
            .Select(m => m.Raza)
            .Distinct()
            .ToListAsync();
    }

    // Actualizar stock de mascotas
    public async Task<bool> ActualizarStock(int mascotaId, int cantidad)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        var mascota = await contexto.Mascotas.FindAsync(mascotaId);
        if (mascota == null)
        {
            return false;
        }

        mascota.Cantidad += cantidad;
        await contexto.SaveChangesAsync();
        return true;
    }

    // Obtener mascotas por rango de precios
    public async Task<List<Mascotas>> ObtenerMascotasPorPrecio(double precioMin, double precioMax)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Mascotas
            .Where(m => m.Precio >= precioMin && m.Precio <= precioMax)
            .AsNoTracking()
            .ToListAsync();
    }
}