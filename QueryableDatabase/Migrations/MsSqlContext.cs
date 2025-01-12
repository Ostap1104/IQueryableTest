using Microsoft.EntityFrameworkCore;
using QueryableDatabase.Models;

namespace QueryableDatabase.Migrations
{
    public class MsSqlContext : DbContext
    {
        public DbSet<Building> Buildings { get; set; }
        //public DbSet<Address> Address { get; set; }

        public MsSqlContext(DbContextOptions<MsSqlContext> options)
            : base(options)
        {
        }

        public static DbContextOptions<MsSqlContext> GetConnectionStringForMigrations()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MsSqlContext>();
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=QueryableTest;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
            return optionsBuilder.Options;
        }
    }
}
