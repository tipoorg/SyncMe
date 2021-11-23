using Microsoft.Extensions.DependencyInjection;
using SyncMe.Views;

namespace SyncMe.Droid;

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
            .AddSingleton<AboutPage>()
            .AddSingleton<NotesPage>()
            .AddSingleton<NamespaceManagmentPage>();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services;
    }
}
