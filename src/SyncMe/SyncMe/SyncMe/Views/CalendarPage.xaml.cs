using SyncMe.ViewModels;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : ContentPage
    {
        private readonly CreateEventPage _createEventPage;
        private readonly NamespaceManagmentPage _namespaceManagmentPage;

        public CalendarPage(CreateEventPage createEventPage, NamespaceManagmentPage namespaceManagmentPage)
        {
            InitializeComponent();
            BindingContext = new CalendarPageViewModel();
            AddEvent.Clicked += AddEvent_Clicked;
            _createEventPage = createEventPage;
            _namespaceManagmentPage = namespaceManagmentPage;
        }

        public async void AddEvent_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Create:", "Cancel", null, "Event", "Namespace");

            switch (action)
            {
                case "Event":
                    await Navigation.PushAsync(_createEventPage);
                    break;
                case "Namespace":
                    await Navigation.PushAsync(_namespaceManagmentPage);
                    break;
            }
        }
    }
}