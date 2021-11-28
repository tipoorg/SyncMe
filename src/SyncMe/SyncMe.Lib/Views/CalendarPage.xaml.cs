using SyncMe.Models;
using SyncMe.ViewModels;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CalendarPage : ContentPage
{
    private readonly CalendarPageViewModel _viewModel;
    private readonly ISyncEventsService _syncEventsService;
    private readonly ISyncNamespaceService _namespaceService;
    private readonly IConfigRepository _configRepository;

    public CalendarPage(
        CalendarPageViewModel viewModel,
        ISyncEventsService syncEventsService,
        ISyncNamespaceService namespaceService,
        IConfigRepository configRepository)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        _syncEventsService = syncEventsService;
        _namespaceService = namespaceService;
        _configRepository = configRepository;

        SoundToggle.IsToggled = !_configRepository.Get(ConfigKey.IsMute);
        AddEvent.Clicked += AddEvent_Clicked;
        year.BindingContext = Calendar1;
    }

    protected override void OnAppearing()
    {
        _viewModel.InitEventsCollection();
        base.OnAppearing();
    }

    public async void AddEvent_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateEventPage(_syncEventsService, _namespaceService));
    }

    private void OnRemoveClicked(object sender, EventArgs e)
    {
        if (sender is Button { CommandParameter: SyncEventViewModel { SyncEvent.Id: var eventId } })
        {
            _syncEventsService.TryRemoveInternalEvent(eventId);
            _viewModel.InitEventsCollection();
        }
    }

    private void OnSoundToggled(object sender, ToggledEventArgs e)
    {
        _configRepository.Set(ConfigKey.IsMute, !e.Value);
    }
}
