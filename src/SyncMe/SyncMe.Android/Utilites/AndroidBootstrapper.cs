using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Configuration;
using SyncMe.Droid.Extensions;
using SyncMe.Lib.Services;

namespace SyncMe.Droid.Utilites;

internal sealed class AndroidBootstrapper : SyncMeBootstrapper
{
    public AndroidBootstrapper(IPathProvider pathProvider) : base(pathProvider)
    {
    }

    public override IServiceCollection AddPlatformSpecific(IServiceCollection services)
    {
        return services.AddSyncMeAndroid();
    }

    public override LoggerConfiguration PlatformSpecificLog(LoggerSinkConfiguration sink)
    {
        return sink.SyncMeAndroidLog();
    }
}
