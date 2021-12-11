using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Configuration;
using SyncMe.DataAccess;
using SyncMe.Lib.Extensions;

namespace SyncMe.Lib.Services;

public abstract class SyncMeBootstrapper
{
    private readonly IPathProvider _pathProvider;

    public SyncMeBootstrapper(IPathProvider pathProvider)
    {
        _pathProvider = pathProvider;
    }

    public abstract IServiceCollection AddPlatformSpecific(IServiceCollection services);

    public abstract LoggerConfiguration PlatformSpecificLog(LoggerSinkConfiguration sink);

    public IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection()
            .AddSingleton(_pathProvider)
            .AddSyncMeLib()
            .AddSyncMeOutlook()
            .AddSyncMeDataAccess(_pathProvider.SyncMeDbPath)
            .AddPlatformSpecific(this)
            .AddSyncMeLogging(this, _pathProvider.SyncMeLogsFolder);

        return DIDataTemplate.AppServiceProvider = services.BuildServiceProvider();
    }

    public App CreateApp(IServiceProvider provider)
    {
        var app = new App(provider);

        return app;
    }
}
