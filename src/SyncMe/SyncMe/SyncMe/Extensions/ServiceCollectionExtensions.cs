using Microsoft.Extensions.DependencyInjection;
using SyncMe.Repos;
using SyncMe.Services;
using SyncMe.Views;

namespace SyncMe.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSyncMeLib(this IServiceCollection services)
    {
        services
            .AddScoped<AppShell>()
            .AddViews()
            .AddServices();

        return services;
    }

    private static IServiceCollection AddViews(this IServiceCollection services)
    {
        services
            .AddScoped<CalendarPage>()
            .AddScoped<CreateEventPage>()
            .AddScoped<NamespaceManagmentPage>()
            .AddScoped<IdentityProvidersPage>();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddSingleton<ISyncAlarmService, SyncAlarmService>()
            .AddSingleton<ISyncEventsRepository, SyncEventsRepository>()
            .AddSingleton<ISyncNamespaceRepository, SyncNamespaceRepository>()
            .AddSingleton<ISyncNamespaceService, SyncNamespaceService>();

        return services;
    }
}
