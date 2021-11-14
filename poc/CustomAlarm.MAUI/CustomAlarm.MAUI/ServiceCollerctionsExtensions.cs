namespace CustomAlarm.MAUI;

public static class ServiceCollerctionsExtensions
{
    public static IServiceCollection AddCustomAlarm(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<MainPage>()
            .AddSingleton<HomePage>()
            .AddSingleton<CalendarPage>();

        return serviceCollection;
    }
}