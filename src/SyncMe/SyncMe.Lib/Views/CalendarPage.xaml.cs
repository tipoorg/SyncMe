using SyncMe.Services;
using SyncMe.ViewModels;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CalendarPage : ContentPage
{
    private readonly CalendarPageViewModel _viewModel;
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
        _viewModel=viewModel;
        _syncEventsService = syncEventsService;
        _namespaceRepository = namespaceRepository;

        year.BindingContext = Calendar1;
        viewModel.BackgroundColorService = new BackgroundColorService(this, namespaceManagmentPage, identityProvidersPage);
    }

    protected override void OnAppearing()
    {
        _viewModel.InitEventsCollection();
        base.OnAppearing();
    }

    public async void AddEvent_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateEventPage(_syncEventsService, _namespaceRepository));
    }

    private void OnRemoveClicked(object sender, EventArgs e)
    {
        if (sender is Button { CommandParameter: SyncEventViewModel { SyncEvent.Id: var eventId } })
        {
            _syncEventsService.TryRemoveInternalEvent(eventId);
        }
    }
}
