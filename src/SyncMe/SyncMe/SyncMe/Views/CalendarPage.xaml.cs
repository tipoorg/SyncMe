using SyncMe.ViewModels;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : ContentPage
    {
        private readonly CreateEventPage _createEventPage;

        public CalendarPage(CreateEventPage createEventPage)
        {
            InitializeComponent();
            BindingContext = new CalendarPageViewModel();
            AddEvent.Clicked += AddEvent_Clicked;
            _createEventPage = createEventPage;
        }

        public async void AddEvent_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Create:", "Cancel", null, "Event");

            switch (action)
            {
                case "Event":
                    await Navigation.PushAsync(_createEventPage);
                    break;
            }
        }
    }
}