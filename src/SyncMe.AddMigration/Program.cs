namespace SyncMe.AddMigration;

internal class Program
{
    static void Main(string[] args)
    {
        // call from .src/
        // dotnet ef migrations add -p SyncMe/SyncMe.DataAccess/ -s SyncMe.AddMigration/ -c MigrationContext Init
    }
}
