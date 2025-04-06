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
            optionsBuilder.UseSqlServer("workstation id=KitterDB123.mssql.somee.com;packet size=4096;user id=ahb45_SQLLogin_2;pwd=bovrurrxl4;data source=KitterDB123.mssql.somee.com;persist security info=False;initial catalog=KitterDB123;TrustServerCertificate=True");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}