using Microsoft.Extensions.Logging;
using SyncMe.ExceptionHandling;

namespace SyncMe.Lib.ExceptionHandling;

internal sealed class ExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(ILogger<ExceptionHandler> logger)
    {
        _logger = logger;
    }

    public void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
    {
        _logger.LogError(unobservedTaskExceptionEventArgs.Exception, "Unhandled exception from task scheduler.");
    }

    public void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
    {
        _logger.LogError(unhandledExceptionEventArgs.ExceptionObject as Exception, "Unhandled exception from current domain.");
    }
}
