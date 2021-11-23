using Microsoft.Extensions.DependencyInjection;
using SyncMe.Views;

namespace SyncMe.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSyncMeLib(this IServiceCollection services)
    {
        services
            .AddSingleton<AppShell>()
            .AddViews()
            .AddServices();

        return services;
    }

    private static IServiceCollection AddViews(this IServiceCollection services)
    {
        services
            .AddSingleton<NotesPage>()
            .AddSingleton<CalendarPage>()
            .AddSingleton<CreateEvent>();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services;
    }
}
