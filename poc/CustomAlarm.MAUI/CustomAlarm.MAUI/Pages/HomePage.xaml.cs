namespace CustomAlarm.MAUI.Pages;

public partial class HomePage : ContentPage
{
    public IObservable<EventPattern<object>> SetAlarmClicks { get; }

    public HomePage()
    {
        InitializeComponent();
        SetAlarmClicks = Observable.FromEventPattern(SetAlarmButton, nameof(IButton.Clicked));
    }
}
