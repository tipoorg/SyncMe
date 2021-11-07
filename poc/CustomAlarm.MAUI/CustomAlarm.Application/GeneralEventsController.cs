namespace CustomAlarm.Application;

internal class GeneralEventsController : IGeneralEventsController
{
    public event EventHandler? OnSetAlarmClickedEvent;

    public void OnSetAlarmClicked(object sender, EventArgs e) => OnSetAlarmClickedEvent?.Invoke(sender, e);
}
