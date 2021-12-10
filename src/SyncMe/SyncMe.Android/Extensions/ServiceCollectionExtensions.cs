using Microsoft.Extensions.DependencyInjection;
using SyncMe.Configuration;
using SyncMe.Droid.Alarm;
using SyncMe.Droid.Utilites;

namespace SyncMe.Droid.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSyncMeAndroid(this IServiceCollection services)
    {
        services
            .AddSingleton<INotificationManager, AndroidNotificationManager>()
            .AddSingleton<IAlarmService, AndroidAlarmService>()
            .AddSingleton<IAlarmPlayer, AndroidAlarmPlayer>()
            .AddSingleton<IPathProvider, AndroidPathProvider>()
            .AddSingleton(new AuthorizationManagerOptions());

        return services;
    }
}
