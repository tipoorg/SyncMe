using MaUIApplication = Microsoft.Maui.Controls.Application;

namespace CustomAlarm.MAUI;

public partial class App : MaUIApplication
{
    private static IServiceProvider _serviceProvider;

    public static T GetRequiredService<T>() => _serviceProvider.GetRequiredService<T>();

    public App(IServiceProvider serviceProvider, MainPage mainPage)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        MainPage = mainPage;
    }
}
