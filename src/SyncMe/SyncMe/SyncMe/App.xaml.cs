using Microsoft.Extensions.DependencyInjection;
using GraphServiceClient = Microsoft.Graph.GraphServiceClient;
using MsGraph = Microsoft.Graph;
using Microsoft.Identity.Client;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Net.Http.Headers;
using CalendarProviders.Authorization;

namespace SyncMe;

public partial class App : Application, INotifyPropertyChanged
{
    private static IServiceProvider _serviceProvider;
    public static object AuthUIParent { get; set; }

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
