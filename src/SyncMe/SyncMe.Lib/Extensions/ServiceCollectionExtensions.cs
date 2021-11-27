using Microsoft.Extensions.DependencyInjection;
using SyncMe.Lib.Services;
using SyncMe.ViewModels;
using SyncMe.Views;

namespace SyncMe.Lib.Extensions;

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
            .AddScoped<CalendarPageViewModel>()
            .AddScoped<NamespaceManagmentPage>()
            .AddScoped<IdentityProvidersPage>();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddSingleton<ISoundSwitcherService, SoundSwitcherService>()
            .AddSingleton<ISyncAlarmCalculator, SyncAlarmCalculator>()
            .AddSingleton<ISyncEventsService, SyncEventsService>()
            .AddSingleton<ISyncNamespaceService, SyncNamespaceService>();

        return services;
    }
}
