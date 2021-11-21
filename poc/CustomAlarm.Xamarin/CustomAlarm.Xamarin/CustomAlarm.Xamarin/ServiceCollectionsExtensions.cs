using CustomAlarm.Xamarin.Views;
using Microsoft.Extensions.DependencyInjection;

namespace CustomAlarm.Xamarin
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection AddPages(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<AppShell>()
                .AddSingleton<NotesPage>()
                .AddSingleton<AboutPage>();

            return serviceCollection;
        }
    }
}
