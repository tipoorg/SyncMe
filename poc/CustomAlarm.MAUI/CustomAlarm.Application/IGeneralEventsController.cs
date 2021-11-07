namespace CustomAlarm.Application;

public interface IGeneralEventsController
{
    event EventHandler OnSetAlarmClickedEvent;

    void OnSetAlarmClicked(object sender, EventArgs e);
}
