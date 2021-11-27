using SyncMe.Extensions;
using SyncMe.Lib.Controls;
using SyncMe.Models;
using SyncMe.ViewModels;
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

    private async void OnClicked(object sender, EventArgs e)
    {
        if (sender is TagButton<SyncRepeat> button)
        {
            _eventModel.Schedule = button.Tag;
            _eventModel.ScheduleButtonText = button.Tag.GetDescription();
        }
        await Navigation.PopAsync();
    }
}
