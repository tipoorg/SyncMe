using EventKit;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using SyncMe.Configuration;

namespace SyncMe.IOS.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSyncMeIOS(this IServiceCollection services)
    {
        services
            .AddSingleton<IAlarmService, IOSAlarmService>()
            .AddSingleton(new EKEventStore())
            .AddSingleton(new AuthorizationManagerOptions
            {
                IOSKeychainSecurityGroup = NSBundle.MainBundle.BundleIdentifier
            });

        return services;
    }
}
