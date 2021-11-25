using SyncMe.Repos;
using SyncMe.Services;
using SyncMe.ViewModels;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : ContentPage
    {
        private readonly ISyncEventsRepository _eventsRepository;
        private readonly ISyncNamespaceRepository _namespaceRepository;

        public CalendarPage(CalendarPageViewModel viewModel,
                            ISyncEventsRepository eventsRepository,
                            ISyncNamespaceRepository namespaceRepository,
                            NamespaceManagmentPage namespaceManagmentPage,
                            IdentityProvidersPage identityProvidersPage)
        {
            InitializeComponent();
            BindingContext = viewModel;
            AddEvent.Clicked += AddEvent_Clicked;
            _eventsRepository = eventsRepository;
            _namespaceRepository = namespaceRepository;

            year.BindingContext = Calendar1;
            monthText.BindingContext = Calendar1;
            viewModel.BackgroundColorService = new BackgroundColorService(this, namespaceManagmentPage, identityProvidersPage);
        }

        public async void AddEvent_Clicked(object sender, EventArgs e) => 
            await Navigation.PushAsync(new CreateEventPage(_eventsRepository, _namespaceRepository));
    }
}