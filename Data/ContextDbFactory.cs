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
            optionsBuilder.UseSqlServer("workstation id=RegistroDb.mssql.somee.com;packet size=4096;user id=ahb45_SQLLogin_1;pwd=59868mjt41;data source=RegistroDb.mssql.somee.com;persist security info=False;initial catalog=RegistroDb;TrustServerCertificate=True");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}