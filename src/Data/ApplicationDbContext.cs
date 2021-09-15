using EfCoreDynamicTable.Entities;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDynamicTable.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Table> Tables { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
    }
}
