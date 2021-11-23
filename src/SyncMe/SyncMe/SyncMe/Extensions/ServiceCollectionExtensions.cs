using Microsoft.Extensions.DependencyInjection;
using SyncMe.Views;

namespace SyncMe.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSyncMeLib(this IServiceCollection services)
    {
        services
            .AddTransient<AppShell>()
            .AddViews()
            .AddServices();

        return services;
    }

    private static IServiceCollection AddViews(this IServiceCollection services)
    {
        services
            .AddTransient<NotesPage>()
            .AddTransient<CalendarPage>()
            .AddTransient<CreateEvent>();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services;
    }
}
