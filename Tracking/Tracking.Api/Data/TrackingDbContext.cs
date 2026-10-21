
using Microsoft.EntityFrameworkCore;
using Tracking.Api.Models;

namespace Tracking.Api.Data;

public class TrackingDbContext : DbContext
{
    public TrackingDbContext(
        DbContextOptions<TrackingDbContext> options)
        : base(options)
    {
    }

    public DbSet<TrackingEvent> TrackingEvents => Set<TrackingEvent>();
}

