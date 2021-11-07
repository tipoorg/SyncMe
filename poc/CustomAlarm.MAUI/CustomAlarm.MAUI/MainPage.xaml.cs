using System;
using CustomAlarm.Application;
using Microsoft.Maui.Controls;

namespace CustomAlarm.MAUI;

public partial class MainPage : ContentPage
{
    private readonly IGeneralEventsController _generalEventsController;

    public MainPage(IGeneralEventsController generalEventsController)
    {
        InitializeComponent();
        _generalEventsController = generalEventsController;
    }

    private void OnSetAlarmClicked(object sender, EventArgs e) => _generalEventsController.OnSetAlarmClicked(sender, e);
}
