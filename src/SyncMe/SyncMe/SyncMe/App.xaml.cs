using Microsoft.Extensions.DependencyInjection;

namespace SyncMe;

public partial class App : Application
{
    private static IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        MainPage = serviceProvider.GetRequiredService<AppShell>();
        _serviceProvider = serviceProvider;
    }

    public static T GetRequiredService<T>() => _serviceProvider.GetRequiredService<T>();
    public static Lazy<T> GetLazyRequiredService<T>() => new(() => GetRequiredService<T>());

    protected override void OnStart()
    {
    }

    protected override void OnSleep()
    {
    }

    protected override void OnResume()
    {
    }
}
