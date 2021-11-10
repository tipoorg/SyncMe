using Microsoft.AspNetCore.Components;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace CustomAlarm.Blazor.Pages;

public partial class Counter
{
	public event EventHandler<EventArgs> clickHanler;

	[Inject]
    public IButtonController ButtonController { get; set; }

    protected override void OnInitialized()
    {
        ButtonController.Clicks = Observable.FromEvent<EventHandler<EventArgs>, EventArgs>(h => clickHanler += h, h => clickHanler -= h);
        base.OnInitialized();
    }

    public void IncrementCount() => clickHanler(this, new EventArgs());
}