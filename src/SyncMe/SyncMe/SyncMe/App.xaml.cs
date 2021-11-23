using Microsoft.Extensions.DependencyInjection;

namespace SyncMe;

public partial class App : Application
{
    private IDisposable _appScope;
    private readonly IServiceProvider _serviceProvider;

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
