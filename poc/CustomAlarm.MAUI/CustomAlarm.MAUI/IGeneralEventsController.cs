using System;

namespace CustomAlarm.MAUI
{
    public interface IGeneralEventsController
    {
        event EventHandler OnSetAlarmClickedEvent;

        void OnSetAlarmClicked(object sender, EventArgs e);
    }
}
