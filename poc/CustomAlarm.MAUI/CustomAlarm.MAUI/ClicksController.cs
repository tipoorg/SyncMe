namespace CustomAlarm.MAUI;

public static class ClicksController<TOwner>
{
    private static IObservable<EventPattern<object>> _clicks = Observable.Never<EventPattern<object>>();
    
    public static IObservable<EventPattern<object>> Clicks => _clicks;

    public static void SetButtonClickHandler(IButton button)
    {
        _clicks = _clicks.Amb(Observable.FromEventPattern(button, nameof(IButton.Clicked)));
    }
}
