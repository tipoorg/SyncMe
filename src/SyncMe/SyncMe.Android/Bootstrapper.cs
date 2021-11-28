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
        var dbPath = GetDatabasePathAsync("syncme.db");

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
            .AddSingleton<INotificationManager, AndroidNotificationManager>()
            .AddSingleton<IAlarmService, AndroidAlarmService>()
            .AddSingleton<IAlarmPlayer, AndroidAlarmPlayer>();

        return services;
    }

    private static string GetDatabasePathAsync(string filename)
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename);

        if (!File.Exists(path))
        {
            File.Create(path).Dispose();
        }

        return path;
    }
}
