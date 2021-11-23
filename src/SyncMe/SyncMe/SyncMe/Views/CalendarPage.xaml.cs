using SyncMe.Models;
using SyncMe.ViewModels;
using Xamarin.Plugin.Calendar.Models;

namespace SyncMe.Views
{
    public partial class CalendarPage : ContentPage
    {
        public CalendarPage()
        {
            InitializeComponent();
            BindingContext = new CalendarPageViewModel();
            AddEvent.Clicked += AddEvent_Clicked;
        }

        private async void AddEvent_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Notification", "Event created!", "OK");
        }
    }
}