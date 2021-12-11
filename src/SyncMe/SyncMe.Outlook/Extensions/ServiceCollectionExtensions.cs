using Microsoft.Extensions.DependencyInjection;
using SyncMe.Outlook.Authorization;

namespace SyncMe.Outlook.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSyncMeOutlook(this IServiceCollection services)
    {
        services
            .AddSingleton<IMicrosoftAuthorizationManager, MicrosoftAuthorizationManager>();

        return services;
    }
}
