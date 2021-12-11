using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SyncMe.CalendarProviders.Authorization;
using SyncMe.Lib.Services;
using SyncMe.ViewModels;
using SyncMe.Views;

namespace SyncMe.Lib.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPlatformSpecific(this IServiceCollection services, SyncMeBootstrapper bootstrapper)
    {
        bootstrapper
            .AddPlatformSpecific(services);

        return services;
    }

    public static IServiceCollection AddSyncMeLib(this IServiceCollection services)
    {
        services
            .AddScoped<AppShell>()
            .AddViews()
            .AddServices()
            .AddSingleton<MicrosoftAuthorizationManager>();

        return services;
    }

    private static IServiceCollection AddViews(this IServiceCollection services)
    {
        services
            .AddScoped<CalendarPage>()
            .AddScoped<CalendarPageViewModel>()
            .AddScoped<NamespaceManagmentPage>()
            .AddScoped<IdentityProvidersPage>()
            .AddScoped<SettingsPage>();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddSingleton<IIdentitiesService, IdentitiesService>()
            .AddSingleton<ISyncEventsService, SyncEventsService>()
            .AddSingleton<ISyncNamespaceService, SyncNamespaceService>()
            .AddSingleton<IAlarmProcessor, AlarmProcessor>();

        return services;
    }

    public static IServiceCollection AddSyncMeLogging(this IServiceCollection services, SyncMeBootstrapper bootstrapper, string logsFolder)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.SyncMeFile(logsFolder)
            .WriteTo.PlatformSpecificLog(bootstrapper)
            .CreateLogger();

        services.AddLogging(builder => builder.AddSerilog());

        return services;
    }
}
