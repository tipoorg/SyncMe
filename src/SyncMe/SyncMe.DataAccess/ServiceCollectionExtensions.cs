using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SyncMe.DataAccess.Repos;

namespace SyncMe.DataAccess;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSyncMeDataAccess(this IServiceCollection services, string databasePath)
    {
        services
            .AddRepositories()
            .AddTransient<IApplicationContextFactory, ApplicationContextFactory>()
            .AddDbContext<ApplicationContext>(builder => builder.UseSqlite($"Filename={databasePath}"));

        return services;
    }


    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddSingleton<ISyncEventsRepository, SyncEventsRepository>()
            .AddSingleton<ISyncNamespaceRepository, SyncNamespaceRepository>()
            .AddSingleton<INotificationsSwitcherRepository, NotificationsSwitcherRepository>();

        return services;
    }
}
