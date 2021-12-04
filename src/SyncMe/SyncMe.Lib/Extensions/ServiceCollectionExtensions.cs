using Microsoft.Extensions.DependencyInjection;
using Serilog;
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
            .AddSingleton<ISyncAlarmCalculator, SyncAlarmCalculator>()
            .AddSingleton<ISyncEventsService, SyncEventsService>()
            .AddSingleton<ISyncNamespaceService, SyncNamespaceService>()
            .AddSingleton<IAlarmProcessor, AlarmProcessor>();

        return services;
    }

    public static IServiceCollection AddSyncMeLogging(this IServiceCollection services, SyncMeBootstrapper bootstrapper, string logsFolder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.SyncMeFile(logsFolder)
            .WriteTo.PlatformSpecificLog(bootstrapper)
            .CreateLogger();

        services.AddLogging(builder => builder.AddSerilog());

        return services;
    }
}
