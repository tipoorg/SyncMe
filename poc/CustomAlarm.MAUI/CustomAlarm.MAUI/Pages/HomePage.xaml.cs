namespace CustomAlarm.MAUI.Pages;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
        ClicksController<HomePage>.SetButtonClickHandler(SetAlarmButton);
    }
}
