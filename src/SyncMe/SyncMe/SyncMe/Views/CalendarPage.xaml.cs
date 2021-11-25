using SyncMe.ViewModels;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : ContentPage
    {
        private readonly CreateEventPage _createEventPage;

        public CalendarPage(CalendarPageViewModel viewModel, CreateEventPage createEventPage)
        {
            InitializeComponent();
            BindingContext = viewModel;
            AddEvent.Clicked += AddEvent_Clicked;
            _createEventPage = createEventPage;
        }

        public async void AddEvent_Clicked(object sender, EventArgs e) => await Navigation.PushAsync(_createEventPage);
    }
}