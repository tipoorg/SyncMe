using System.Text;
using Serilog;
using Serilog.Configuration;
using SyncMe.Lib.Services;

namespace SyncMe.Lib.Extensions;

public static class SerilogExtensions
{
    public static LoggerConfiguration PlatformSpecificLog(this LoggerSinkConfiguration sink, SyncMeBootstrapper bootstrapper)
    {
        return bootstrapper.PlatformSpecificLog(sink);
    }

    public static LoggerConfiguration SyncMeFile(this LoggerSinkConfiguration sink, string logsDirectory)
    {
        var logFilePath = Path.Combine(logsDirectory, "syncme-.txt");

        return sink.File(
            path: logFilePath,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}",
            fileSizeLimitBytes: 100000000,
            rollingInterval: RollingInterval.Month,
            rollOnFileSizeLimit: true,
            shared: false,
            retainedFileCountLimit: 31,
            encoding: Encoding.UTF8);
    }
}
