using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProyectoFinalAp1.Data;

namespace RegistroTecnico.Context
{
    public class ContextoFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Configura el DbContext usando una cadena de conexión estática o predeterminada.
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=tcp:aplicada-1.database.windows.net,1433;Initial Catalog=AdrianDb;Persist Security Info=False;User ID=Adrian;Password=Maximo00;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}