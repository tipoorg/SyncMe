using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace SyncMe.DataAccess;

public class ApplicationContextFactory : IApplicationContextFactory
{
    private readonly ApplicationContext _context;

    public ApplicationContextFactory(ApplicationContext context)
    {
        _context = context;
    }

    public void Migrate()
    {
        try
        {
            _context.Database.Migrate();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    public async Task MigrateAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }
}