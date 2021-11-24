using SyncMe.Models;
using SyncMe.Repos;
using SyncMe.ViewModels;
using Xamarin.Forms.Xaml;
using Xamarin.Plugin.Calendar.Models;

namespace SyncMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : ContentPage
    {
        private readonly ISyncEventsRepository _syncEventsRepository;
        private readonly ISyncNamespaceRepository _syncNamespaceRepository;

        public CalendarPage(ISyncEventsRepository syncEventsRepository, ISyncNamespaceRepository syncNamespaceRepository)
        {
            InitializeComponent();
            BindingContext = new CalendarPageViewModel();
            AddEvent.Clicked += AddEvent_Clicked;
            _syncEventsRepository = syncEventsRepository;
            _syncNamespaceRepository = syncNamespaceRepository;
        }

        public async void AddEvent_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Create:", "Cancell", null, "Event", "Namespace");

            switch (action)
            {
                case "Event":
                    await Navigation.PushModalAsync(new CreateEventPage(_syncEventsRepository, _syncNamespaceRepository));
                    break;
                case "Namespace":
                    await Navigation.PushModalAsync(new NamespaceManagmentPage());
                    break;
            }
        }

    }
}