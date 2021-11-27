using System.IO;
using Microsoft.Extensions.DependencyInjection;
using SyncMe.Droid.Alarm;
using SyncMe.Lib.Extensions;
using SyncMe.DataAccess;

namespace SyncMe.Droid;

public static class Bootstrapper
{
    private static IServiceProvider _instance;
    private static IServiceProvider Instance => _instance ??= CreateServiceProvider();

    public static T GetService<T>() => Instance.GetRequiredService<T>();

    private static IServiceProvider CreateServiceProvider()
    {
        var dbPath = GetDatabasePath("syncme.db");

        var services = new ServiceCollection()
          .AddSyncMeLib()
          .AddSyncMeDataAccess(dbPath)
          .AddSyncMeAndroid();

        return DIDataTemplate.AppServiceProvider = services.BuildServiceProvider();
    }

    public static App CreateApp()
    {
        var app = new App(Instance);

        return app;
    }

    public static IServiceCollection AddSyncMeAndroid(this IServiceCollection services)
    {
        services
            .AddSingleton<IAlarmService, AndroidAlarmService>()
            .AddSingleton<IAndroidAlarmPlayer, AndroidAlarmPlayer>()
            .AddSingleton<IAndroidAlarmProcessor, AndroidAlarmProcessor>();

        return services;
    }

    private static string GetDatabasePath(string filename)
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename);
    }
}
