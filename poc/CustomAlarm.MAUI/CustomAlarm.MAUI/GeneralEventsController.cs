using System;

namespace CustomAlarm.MAUI
{
    internal class GeneralEventsController : IGeneralEventsController
    {
        public event EventHandler OnSetAlarmClickedEvent;
        
        public void OnSetAlarmClicked(object sender, EventArgs e) => OnSetAlarmClickedEvent(sender, e);
    }
}
