using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Data;
using ProyectoFinalAp1.Models;
using System.Linq.Expressions;

namespace ProyectoFinalAp1.Services;

public class DonadorService(IDbContextFactory<ApplicationDbContext> dbFactory)
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory = dbFactory;

    private async Task<bool> Insertar(Donador donador)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        contexto.Donador.Add(donador);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Donador donador)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        contexto.Donador.Update(donador);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        var eliminados = await contexto.Donador
            .Where(d => d.DonadorId == id)
            .ExecuteDeleteAsync();

        return eliminados > 0;
    }

    public async Task<bool> Existe(int donadorId)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        return await contexto.Donador.AnyAsync(d => d.DonadorId == donadorId);
    }

    public async Task<bool> ExisteDonador(string email, string telefono, int excluirId = 0)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        return await contexto.Donador.AnyAsync(d =>
            (d.Email == email || d.Telefono == telefono) &&
            d.DonadorId != excluirId);
    }

    public async Task<bool> Guardar(Donador donador)
    {
        if (!await Existe(donador.DonadorId))
        {
            return await Insertar(donador);
        }
        else
        {
            return await Modificar(donador);
        }
    }

    public async Task<Donador?> Buscar(int id = 0, string? email = null, string? telefono = null)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();

        if (id > 0)
        {
            return await contexto.Donador
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.DonadorId == id);
        }
        else if (!string.IsNullOrEmpty(email))
        {
            return await contexto.Donador
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Email == email);
        }
        else if (!string.IsNullOrEmpty(telefono))
        {
            return await contexto.Donador
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Telefono == telefono);
        }

        return null;
    }

    public async Task<List<Donador>> Listar(Expression<Func<Donador, bool>> criterio = null)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();

        IQueryable<Donador> query = contexto.Donador
            .AsNoTracking();

        if (criterio != null)
        {
            query = query.Where(criterio);
        }

        return await query
            .OrderBy(d => d.Nombre)
            .ToListAsync();
    }

    public async Task<List<Donador>> ListarTodos()
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Donador
            .AsNoTracking()
            .OrderBy(d => d.Nombre)
            .ToListAsync();
    }
}
