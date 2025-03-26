using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Data;
using ProyectoFinalAp1.Models;
using System.Linq.Expressions;

namespace ProyectoFinalAp1.Services;

public class UsuariosService(IDbContextFactory<ApplicationDbContext> dbFactory)
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory = dbFactory;

  
    private async Task<bool> Insertar(Usuarios usuario)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        contexto.Usuarios.Add(usuario);
        return await contexto.SaveChangesAsync() > 0;
    }

 
    private async Task<bool> Modificar(Usuarios usuario)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        contexto.Usuarios.Update(usuario);
        return await contexto.SaveChangesAsync() > 0;
    }

 
    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        var eliminados = await contexto.Usuarios
            .Where(u => u.UsuarioId == id)
            .ExecuteDeleteAsync();

        return eliminados > 0;
    }

 
    public async Task<bool> Existe(int usuarioId)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        return await contexto.Usuarios.AnyAsync(u => u.UsuarioId == usuarioId);
    }

  
    public async Task<bool> ExisteUsuario(string email, string telefono, int excluirId = 0)
    {
        await using var contexto = _dbFactory.CreateDbContext();
        return await contexto.Usuarios.AnyAsync(u =>
            (u.Email == email || u.Telefono == telefono) &&
            u.UsuarioId != excluirId);
    }

    public async Task<bool> Guardar(Usuarios usuario)
    {
        if (!await Existe(usuario.UsuarioId))
        {
            return await Insertar(usuario);
        }
        else
        {
            return await Modificar(usuario);
        }
    }


    public async Task<Usuarios?> Buscar(int id = 0, string? email = null, string? telefono = null)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();

        if (id > 0)
        {
            return await contexto.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UsuarioId == id);
        }
        else if (!string.IsNullOrEmpty(email))
        {
            return await contexto.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }
        else if (!string.IsNullOrEmpty(telefono))
        {
            return await contexto.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Telefono == telefono);
        }

        return null;
    }

    public async Task<List<Usuarios>> Listar(Expression<Func<Usuarios, bool>> criterio = null)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();

        IQueryable<Usuarios> query = contexto.Usuarios
            .AsNoTracking();

        if (criterio != null)
        {
            query = query.Where(criterio);
        }

        return await query
            .OrderBy(u => u.Nombre)
            .ThenBy(u => u.Apellido)
            .ToListAsync();
    }

    public async Task<List<Usuarios>> ListarTodos()
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Usuarios
            .AsNoTracking()
            .OrderBy(u => u.Nombre)
            .ThenBy(u => u.Apellido)
            .ToListAsync();
    }

    // Autenticación básica por email
    public async Task<Usuarios?> Autenticar(string email)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}