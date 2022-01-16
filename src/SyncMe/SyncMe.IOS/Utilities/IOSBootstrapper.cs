using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Configuration;
using SyncMe.IOS.Extensions;
using SyncMe.Lib.Services;

namespace SyncMe.IOS.Utilities;

internal sealed class IOSBootstrapper : SyncMeBootstrapper
{
    public IOSBootstrapper(IPathProvider pathProvider) : base(pathProvider)
    {
    }

    public override IServiceCollection AddPlatformSpecific(IServiceCollection services)
    {
        return services.AddSyncMeIOS();
    }

    public override LoggerConfiguration PlatformSpecificLog(LoggerSinkConfiguration sink)
    {
        return sink.SyncMeIOSLog();
    }
}

