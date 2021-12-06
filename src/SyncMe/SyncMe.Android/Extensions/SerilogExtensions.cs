using Serilog;
using Serilog.Configuration;

namespace SyncMe.Droid.Extensions;

public static class SerilogExtensions
{ 
    public static LoggerConfiguration SyncMeAndroidLog(this LoggerSinkConfiguration sink)
    {
        return sink.AndroidLog();
    }
}
