using System;
using Microsoft.Extensions.DependencyInjection;
using SyncMe.Lib.Extensions;
using SyncMe.Models;

namespace SyncMe.IOS;

public static class Bootstrapper
{
    private static IServiceProvider _instance;
    private static IServiceProvider Instance => _instance ??= CreateServiceProvider();

    public static T GetService<T>() => Instance.GetRequiredService<T>();

    private static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection()
          .AddSyncMeLib()
          .AddSyncMeiOS();

        return DIDataTemplate.AppServiceProvider = services.BuildServiceProvider();
    }

    public static App CreateApp()
    {
        var app = new App(Instance);

        return app;
    }

    public static IServiceCollection AddSyncMeiOS(this IServiceCollection services)
    {
        services
            .AddSingleton<IAlarmService, IOSAlarmService>();

        return services;
    }
}

internal class IOSAlarmService : IAlarmService
{
    public void SetAlarm(SyncAlarm syncAlarm)
    {
        throw new NotImplementedException();
    }
}