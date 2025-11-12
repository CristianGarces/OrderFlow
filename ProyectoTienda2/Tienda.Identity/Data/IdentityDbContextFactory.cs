using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tienda.Identity.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Cadena de conexión para migraciones
            optionsBuilder.UseNpgsql("Host=localhost;Port=49729;Database=postgres;Username=postgres;Password=Tw(Arr7MPpWQahHBjEBF8g;");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}