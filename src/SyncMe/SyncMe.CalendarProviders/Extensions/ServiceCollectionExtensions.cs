using Microsoft.Extensions.DependencyInjection;
using SyncMe.CalendarProviders.Authorization;

namespace SyncMe.Lib.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSyncMeOutlook(this IServiceCollection services)
    {
        services
            .AddSingleton<IMicrosoftAuthorizationManager, MicrosoftAuthorizationManager>();

        return services;
    }
}
