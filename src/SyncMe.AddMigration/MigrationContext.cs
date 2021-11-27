using Microsoft.EntityFrameworkCore;

namespace SyncMe.DataAccess;

public class MigrationContext : ApplicationContext
{
    public MigrationContext() : base(GetOptions())
    {
    }

    private static DbContextOptions<ApplicationContext> GetOptions()
    {
        var builder = new DbContextOptionsBuilder<ApplicationContext>()
            .UseSqlite($"Data Source=nothing.db", x => x.MigrationsAssembly("SyncMe.DataAccess"));

        return builder.Options;
    }
}