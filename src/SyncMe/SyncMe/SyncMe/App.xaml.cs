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
    private IDisposable _appScope;

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        MainPage = serviceProvider.GetRequiredService<AppShell>();
        _serviceProvider = serviceProvider;
    }

    protected override void OnStart()
    {
        if (_appScope is not null)
            CloseScope();

        _appScope = _serviceProvider.CreateScope();
    }

    protected override void OnSleep()
    {
        CloseScope();
    }

    protected override void OnResume()
    {
        if (_appScope is not null)
            CloseScope();

        _appScope = _serviceProvider.CreateScope();
    }

    private void CloseScope()
    {
        _appScope.Dispose();
        _appScope = null;
    }
}
