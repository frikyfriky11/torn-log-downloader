using Microsoft.EntityFrameworkCore;

namespace TornLogDownloader.Entities;

public sealed class ApplicationDbContext : DbContext
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
  {
  }

  public DbSet<RawLog> RawLogs => Set<RawLog>();

  public DbSet<Run> Runs => Set<Run>();

  public DbSet<RunApiCall> RunApiCalls => Set<RunApiCall>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<RawLog>()
      .Property(p => p.Date)
      .HasComputedColumnSql("DATEADD(S, [Timestamp], '1970-01-01')");

    base.OnModelCreating(modelBuilder);
  }
}


