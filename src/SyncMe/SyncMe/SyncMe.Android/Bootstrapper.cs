using Android.Content;
using Microsoft.Extensions.DependencyInjection;
using SyncMe.Droid.Alarm;

namespace SyncMe.Droid;

public static class Bootstrapper
{
    public static App CreateApp()
    {
        var services = new ServiceCollection()
            .AddSyncMeLib()
            .AddSyncMeAndroid();

        var serviceProvider = DIDataTemplate.AppServiceProvider = services.BuildServiceProvider();


        var app = new App(serviceProvider);

        return app;
    }

    public static IServiceCollection AddSyncMeAndroid(this IServiceCollection services)
    {
        return services;
    }
}
