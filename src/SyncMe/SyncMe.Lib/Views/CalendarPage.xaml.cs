using SyncMe.Services;
using SyncMe.ViewModels;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CalendarPage : ContentPage
{
    private readonly ISyncEventsService _syncEventsService;
    private readonly ISyncNamespaceRepository _namespaceRepository;

    public CalendarPage(
        CalendarPageViewModel viewModel,
        ISyncEventsService syncEventsService,
        ISyncNamespaceRepository namespaceRepository,
        NamespaceManagmentPage namespaceManagmentPage,
        IdentityProvidersPage identityProvidersPage)
    {
        InitializeComponent();
        BindingContext = viewModel;
        AddEvent.Clicked += AddEvent_Clicked;
        _syncEventsService = syncEventsService;
        _namespaceRepository = namespaceRepository;

        year.BindingContext = Calendar1;
        viewModel.BackgroundColorService = new BackgroundColorService(this, namespaceManagmentPage, identityProvidersPage);
    }

    public async void AddEvent_Clicked(object sender, EventArgs e) =>
        await Navigation.PushAsync(new CreateEventPage(_syncEventsService, _namespaceRepository));
}
