using Microsoft.EntityFrameworkCore;
using SyncMe.Models;

namespace SyncMe.DataAccess;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    public DbSet<SyncEvent> Events { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SyncEvent>().HasKey(x => x.Id);

        base.OnModelCreating(modelBuilder);
    }
}
