using System;

namespace CustomAlarm.Blazor;

public interface IButtonController
{
    public IObservable<EventArgs> Clicks { get; set; }

}
