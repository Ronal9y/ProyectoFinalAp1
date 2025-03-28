using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Data;
using ProyectoFinalAp1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace ProyectoFinalAp1.Services
{
    public class EmpleadoService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

        // Constructor correcto
        public EmpleadoService(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        private async Task<bool> Insertar(Empleados empleados)
        {
            await using var contexto = _dbFactory.CreateDbContext();
            contexto.Empleados.Add(empleados);
            return await contexto.SaveChangesAsync() > 0;
        }

        private async Task<bool> Modificar(Empleados empleado)
        {
            await using var contexto = _dbFactory.CreateDbContext();
            contexto.Empleados.Update(empleado);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task<bool> Eliminar(int id)
        {
            await using var contexto = _dbFactory.CreateDbContext();
            var eliminarEmpleado = await contexto.Empleados.Where(m => m.EmpleadoId == id).ExecuteDeleteAsync();
            return eliminarEmpleado > 0;
        }

        public async Task<bool> Existe(int empleadoId)
        {
            await using var contexto = _dbFactory.CreateDbContext();
            return await contexto.Empleados.AnyAsync(m => m.EmpleadoId == empleadoId);
        }

        public async Task<bool> Guardar(Empleados empleado)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            if (!await Existe(empleado.EmpleadoId))
            {
                return await Insertar(empleado);
            }
            else
            {
                return await Modificar(empleado);
            }
        }

        public async Task<Empleados?> Buscar(int id = 0, string? nombre = null, string? apellidos = null)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            if (id > 0)
            {
                return await contexto.Empleados.AsNoTracking()
                    .FirstOrDefaultAsync(s => s.EmpleadoId == id);
            }
            else if (!string.IsNullOrEmpty(nombre))
            {
                return await contexto.Empleados.AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Nombre == nombre);
            }
            else if (!string.IsNullOrEmpty(apellidos))
            {
                return await contexto.Empleados.AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Apellidos == apellidos);
            }
            return null;
        }

        public async Task<List<Empleados>> Listar(Expression<Func<Empleados, bool>> criterio)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Empleados.AsNoTracking().Where(criterio).ToListAsync();
        }

        public async Task<List<Empleados>> ListarEmpleado()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Empleados.AsNoTracking().ToListAsync();
        }

        public async Task<bool> ExisteEmpleado(int id, string? nombre)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Empleados.AnyAsync(s => s.EmpleadoId == id || s.Nombre == nombre);
        }

        public async Task<double> ObtenerTotalSalarios()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Empleados.SumAsync(m => m.Salario);
        }
    }
}