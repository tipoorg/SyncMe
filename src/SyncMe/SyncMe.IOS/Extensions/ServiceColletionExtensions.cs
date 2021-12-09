using Microsoft.Extensions.DependencyInjection;

namespace SyncMe.IOS.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSyncMeIOS(this IServiceCollection services)
    {
        services
            .AddSingleton<IAlarmService, IOSAlarmService>();

        return services;
    }
}
