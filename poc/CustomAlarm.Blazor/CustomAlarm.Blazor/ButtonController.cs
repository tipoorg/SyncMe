using System;

namespace CustomAlarm.Blazor;

public class ButtonController : IButtonController
{
    public IObservable<EventArgs> Clicks { get; set; }
}