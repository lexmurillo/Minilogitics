using Microsoft.EntityFrameworkCore;
using Shipping.Api.Models;

namespace Shipping.Api.Data;

public class ShippingDbContext : DbContext
{
    public ShippingDbContext(DbContextOptions<ShippingDbContext> options)
        : base(options)
    {
    }

    public DbSet<Shipment> Shipments { get; set; }
    
}

