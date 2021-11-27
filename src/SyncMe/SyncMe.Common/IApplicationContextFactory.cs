namespace SyncMe;

public interface IApplicationContextFactory
{
    void Migrate();
    Task MigrateAsync();
}
