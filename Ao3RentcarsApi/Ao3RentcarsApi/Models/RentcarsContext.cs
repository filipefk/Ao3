using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ao3RentcarsApi.Models
{
    public class RentcarsContext : DbContext
    {
        public RentcarsContext(DbContextOptions<RentcarsContext> options)
            : base(options)
        {
        }

        public DbSet<Veiculo> TodoItems { get; set; }
    }
}
