using SyncMe.Controls;
using SyncMe.Extensions;
using SyncMe.Models;
using SyncMe.ViewModels;

namespace SyncMe.Views;

public partial class EventAlertPage : ContentPage
{
    private readonly SyncEventViewModel _eventModel;

    public EventAlertPage(SyncEventViewModel eventModel)
    {
        _eventModel = eventModel;
        InitializeComponent();
    }

    private async void OnClicked(object sender, EventArgs e)
    {
        if (sender is TagButton<SyncReminder> button)
        {
            _eventModel.Notification = button.Tag;
            _eventModel.AlertButtonText = button.Tag.GetDescription();
        }

        await Navigation.PopAsync();
    }
}
