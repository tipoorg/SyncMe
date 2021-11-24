using SyncMe.Controls;
using SyncMe.Extensions;
using SyncMe.Models;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EventSchedulePage : ContentPage
{
    private readonly SyncEventViewModel _eventModel;

    public EventSchedulePage(SyncEventViewModel eventViewModel)
    {
        InitializeComponent();
        _eventModel = eventViewModel;
        BindingContext = _eventModel;
    }

    private async void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is TagRadioButton radio)
        {
            _eventModel.Schedule = radio.Tag;
            _eventModel.ScheduleButtonText = radio.Tag.GetDescription();
        }
        await Navigation.PopAsync();
    }
}
