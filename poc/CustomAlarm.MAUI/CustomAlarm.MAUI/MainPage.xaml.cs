using System;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace CustomAlarm.MAUI;

public partial class MainPage : ContentPage
{
    public IObservable<EventPattern<object>> SetAlarmClicks { get; }

    public MainPage()
    {
        InitializeComponent();
        SetAlarmClicks = Observable.FromEventPattern(SetAlarmButton, nameof(IButton.Clicked));
    }
}
