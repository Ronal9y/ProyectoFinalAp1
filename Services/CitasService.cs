using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Data;
using ProyectoFinalAp1.Models;
using System.Linq.Expressions;

namespace ProyectoFinalAp1.Services;

public class CitasService(IDbContextFactory<ApplicationDbContext> dbFactory)
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory = dbFactory;

    // Método privado para insertar una nueva cita
    private async Task<bool> Insertar(Citas cita)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        contexto.Citas.Add(cita);
        return await contexto.SaveChangesAsync() > 0;
    }

    // Método privado para modificar una cita existente
    private async Task<bool> Modificar(Citas cita)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        contexto.Citas.Update(cita);
        return await contexto.SaveChangesAsync() > 0;
    }

    // Eliminar una cita por ID
    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        var eliminados = await contexto.Citas
            .Where(c => c.CitaId == id)
            .ExecuteDeleteAsync();

        return eliminados > 0;
    }

    // Verificar si existe una cita por ID
    public async Task<bool> Existe(int citaId)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        return await contexto.Citas.AnyAsync(c => c.CitaId == citaId);
    }

    // Verificar si existe una cita con el mismo nombre y fecha
    public async Task<bool> ExisteCita(string nombre, DateTime fecha, int excluirId = 0)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        return await contexto.Citas.AnyAsync(c =>
            c.Nombre == nombre &&
            c.Fecha.Date == fecha.Date &&
            c.CitaId != excluirId);
    }

    // Guardar (insertar o modificar) una cita
    public async Task<bool> Guardar(Citas cita)
    {
        if (!await Existe(cita.CitaId))
        {
            return await Insertar(cita);
        }
        else
        {
            return await Modificar(cita);
        }
    }

    // Buscar citas por diferentes criterios
    public async Task<Citas?> Buscar(int id = 0, string? nombre = null, DateTime? fecha = null)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();

        if (id > 0)
        {
            return await contexto.Citas
                .Include(c => c.Empleado) // Incluir datos del empleado
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CitaId == id);
        }
        else if (!string.IsNullOrEmpty(nombre))
        {
            return await contexto.Citas
                .Include(c => c.Empleado)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Nombre == nombre);
        }
        else if (fecha.HasValue)
        {
            return await contexto.Citas
                .Include(c => c.Empleado)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Fecha.Date == fecha.Value.Date);
        }

        return null;
    }

    // Listar citas con filtro opcional
    public async Task<List<Citas>> Listar(Expression<Func<Citas, bool>> criterio = null)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();

        IQueryable<Citas> query = contexto.Citas
            .Include(c => c.Empleado) // Incluir datos del empleado
            .AsNoTracking();

        if (criterio != null)
        {
            query = query.Where(criterio);
        }

        return await query.OrderBy(c => c.Fecha).ToListAsync(); // Ordenar por fecha
    }

    // Listar todas las citas
    public async Task<List<Citas>> ListarCitas()
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Citas
            .Include(c => c.Empleado)
            .AsNoTracking()
            .OrderBy(c => c.Fecha)
            .ToListAsync();
    }

    // Listar citas por rango de fechas
    public async Task<List<Citas>> ListarCitasPorFecha(DateTime fechaInicio, DateTime fechaFin)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Citas
            .Include(c => c.Empleado)
            .Where(c => c.Fecha.Date >= fechaInicio.Date && c.Fecha.Date <= fechaFin.Date)
            .OrderBy(c => c.Fecha)
            .AsNoTracking()
            .ToListAsync();
    }

    // Listar citas por empleado
    public async Task<List<Citas>> ListarCitasPorEmpleado(int empleadoId)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Citas
            .Include(c => c.Empleado)
            .Where(c => c.EmpleadoId == empleadoId)
            .OrderBy(c => c.Fecha)
            .AsNoTracking()
            .ToListAsync();
    }

    // Listar citas por nombre de mascota
    public async Task<List<Citas>> ListarCitasPorMascota(string nombreMascota)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Citas
            .Include(c => c.Empleado)
            .Where(c => c.NombreMascota == nombreMascota)
            .OrderBy(c => c.Fecha)
            .AsNoTracking()
            .ToListAsync();
    }
}