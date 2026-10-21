using Microsoft.EntityFrameworkCore;
using Packages.Api.Models;

namespace Packages.Api.Data;

public class PackagesDbContext : DbContext
{
    public PackagesDbContext(DbContextOptions<PackagesDbContext> options)
        : base(options)
    {
    }

    public DbSet<Package> Packages => Set<Package>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder .Entity<Package>()
            .Property(p => p.weight)
            .HasPrecision(18, 2);
    }


}
