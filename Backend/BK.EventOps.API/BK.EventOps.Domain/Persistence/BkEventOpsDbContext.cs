using Microsoft.EntityFrameworkCore;

namespace BK.EventOps.Domain.Persistence;

public partial class BkEventOpsDbContext : DbContext
{
    public BkEventOpsDbContext(DbContextOptions<BkEventOpsDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Generated EF Core configuration

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}