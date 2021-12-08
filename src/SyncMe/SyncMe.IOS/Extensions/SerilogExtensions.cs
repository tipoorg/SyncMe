using Serilog;
using Serilog.Configuration;

namespace SyncMe.IOS.Extensions;

public static class SerilogExtensions
{
    public static LoggerConfiguration SyncMeIOSLog(this LoggerSinkConfiguration sink)
    {
        return sink.NSLog();
    }
}
