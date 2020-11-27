using Microsoft.EntityFrameworkCore;

namespace Ao3RentcarsApi.Models
{
    public class RentcarsContext : DbContext
    {
        public RentcarsContext(DbContextOptions<RentcarsContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Veiculo> Veiculos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
