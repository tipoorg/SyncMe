namespace CustomAlarm.MAUI.Pages;

public partial class MainPage : TabbedPage
{
    public MainPage(HomePage homePage, CalendarPage calendarPage)
    {
        InitializeComponent();

        Children.Add(homePage);
        Children.Add(calendarPage);
    }
}
