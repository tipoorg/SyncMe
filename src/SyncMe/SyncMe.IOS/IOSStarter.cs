using System;
using Microsoft.Extensions.DependencyInjection;
using SyncMe.IOS.Utilities;

namespace SyncMe.IOS;

public static class IOSStarter
{
    private static readonly IOSBootstrapper _bootstrapper = new(new IOSPathProvider());

    private static IServiceProvider _instance;
    private static IServiceProvider Instance => _instance ??= _bootstrapper.CreateServiceProvider();

    public static T GetService<T>() => Instance.GetRequiredService<T>();

    public static App CreateApp() => _bootstrapper.CreateApp(Instance);
}
