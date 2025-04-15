using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Authentification.JWT.DAL.Data
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseSqlServer("Server=OUJC51MK13;Database=authenticationDb;Trusted_Connection=True;TrustServerCertificate=True;", b => b.MigrationsAssembly("Authentification.JWT.DAL"));
            return new MyDbContext(optionsBuilder.Options);
        }
    }
}
