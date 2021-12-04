using Microsoft.Extensions.DependencyInjection;
using SyncMe.Droid.Utilites;

namespace SyncMe.Droid;

public static class AndroidStarter
{
    private static readonly AndroidBootstrapper _bootstrapper = new(new AndroidPathProvider());

    private static IServiceProvider _instance;
    private static IServiceProvider Instance => _instance ??= _bootstrapper.CreateServiceProvider();

    public static T GetService<T>() => Instance.GetRequiredService<T>();

    public static App CreateApp() => _bootstrapper.CreateApp(Instance);
}
